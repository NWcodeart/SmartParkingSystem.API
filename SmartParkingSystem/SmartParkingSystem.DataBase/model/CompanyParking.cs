using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
