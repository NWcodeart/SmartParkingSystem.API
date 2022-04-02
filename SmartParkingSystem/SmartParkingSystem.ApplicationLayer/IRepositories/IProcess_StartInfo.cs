using Microsoft.AspNetCore.Http;
using System;

namespace SmartParkingSystem.ApplicationLayer.IRepositories
{
    public interface IProcess_StartInfo
    {
        // this function will run the python script saudilp.py in folder VPR
        public String ExecuteOCR(String imagePath);
    }
}
