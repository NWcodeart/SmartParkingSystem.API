using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SmartParkingSystem.DataBase.model
{
    public class CompanyParking
    {
        [Key]
        public int Id { get; set; }

        public String Name { get; set; }

        public ICollection<ParkingSpace> ParkingList { get; set; }
    }
}
