#!/usr/bin/python
# -*- coding: latin-1 -*-
import cv2
import imutils
import numpy as np
import pytesseract
from pytesseract import Output

import easyocr

#pytesseract.pytesseract.tesseract_cmd = 'C:\Program Files\Tesseract-OCR\tesseract.exe'


def showimage(titl, img):
    cv2.imshow(titl, img)
    cv2.waitKey(0)


def cropped_image(gray_im, screenCnt):
    mask = np.zeros(gray_im.shape,np.uint8)
    _ = cv2.drawContours(mask,[screenCnt],0,255,-1,)
    _ = cv2.bitwise_and(img,img,mask=mask)

    (x, y) = np.where(mask == 255)
    (topx, topy) = (np.min(x), np.min(y))
    (bottomx, bottomy) = (np.max(x), np.max(y))
    return gray_im[topx:bottomx+1, topy:bottomy+1]
    

def get_arabic_letter(enLetter):
    SaudiLP = {
    "A":"ا","B":"ب","D":"د","E":"ع",
    "F":"ف","G":"ق","H":"ـه","J":"ح",
    "K":"ك","L":"ل","M":"م","N":"ن",
    "R":"ر","S":"س","T":"ط","V":"ي",
    "X":"ص","Z":"م","U":"و",
    "1":"١",
    "2":"٢",
    "3":"٣",
    "4":"٤",
    "5":"٥",
    "6":"٦",
    "7":"٧",
    "8":"٨",
    "9":"٩",
    "0":"٠"
    }
    arLetter = ""
    for en in enLetter:
        try:
            arLetter += SaudiLP[en]
        except:
            arLetter+=""
    
    return arLetter

def initi_ocr():
    return easyocr.Reader(['en'], gpu=False)
    
def img_preprocessing(img_name):
    img = cv2.imread(img_name,cv2.IMREAD_COLOR)
    img = cv2.resize(img, (600,400) )
    gray = cv2.cvtColor(img, cv2.COLOR_BGR2GRAY) 
    gray = cv2.bilateralFilter(gray, 13, 15, 15) 

    _, threshold = cv2.threshold(gray, 127, 255, cv2.THRESH_BINARY)
    return threshold, img, gray


def ocr_processing(threshold, img, gray):

    contours = cv2.findContours(threshold, cv2.RETR_LIST, cv2.CHAIN_APPROX_SIMPLE)

    contours = imutils.grab_contours(contours)
    contours = sorted(contours, key = cv2.contourArea, reverse = True)[:10]
    
    final_plate = ""
    first_cont = True
    for cont in contours:
        apprx = cv2.approxPolyDP(cont, 0.018 * cv2.arcLength(cont, True), True)
        if first_cont:  #ignore first image as it represnts the whole image
            first_cont = False
        else:
            if len(apprx) == 4:
                cv2.drawContours(img, [apprx], -1, (0, 0, 255), 3)

                cropped = cropped_image(gray, apprx)
                showimage("Extracted Image", cropped)

                result = reader.readtext(cropped, width_ths=1.0)
                print( result)

                plate_str = ""
                if result: 
                    plate_str = result[0][1].replace(" ","")
                    
                    if (result[0][2] >=0.50 and len(plate_str)>1):
                        if(plate_str.isdigit()):
                            final_plate += plate_str
                        
                        if(plate_str.isalpha() and len(plate_str) == 3):
                            final_plate += plate_str

                        if(plate_str.isalpha() and len(plate_str)>3):
                            final_plate += plate_str.rstrip(plate_str[-1])
                        
                        
                        
                        

    
    showimage("Processed Image", img)
    if not final_plate:
        print ("No License Plate Found")
    else:
        final_plate = final_plate.lstrip('ABCDEFGHIJKLMNOPQRSTWXYZabcdefghijklmnopqrstwxyz')
        print(final_plate)
        print(get_arabic_letter(final_plate))
    cv2.destroyAllWindows()

if __name__ == '__main__':
    reader = initi_ocr()
    threshold, img , gray= img_preprocessing('s10.jpeg')
    showimage("", threshold)
    ocr_processing(threshold, img, gray)

