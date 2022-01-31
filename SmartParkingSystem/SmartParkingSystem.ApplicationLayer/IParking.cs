using SmartParkingSystem.DataBase.model;
using SmartParkingSystem.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartParkingSystem.ApplicationLayer
{
    public interface IParking
    {
        //add new parking for a building or a company
        public void AddCompanyParking(AddParking parking);

        //get company parking by Id
        public AddParking GetCompanyParking(int Id);

        //add new parking for a building or a company
        public void AddParkingSpace(AddParking parking);

        //get company parking by Id
        public AddParking GetCompanyParking(int Id);


    }
}
