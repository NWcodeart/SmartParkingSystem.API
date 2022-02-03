using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartParkingSystem.Entity
{
    public class SpacesDto
    {
        public int Id { get; set; }

        public string ParkingNumber { get; set; }

        public int ParkingId { get; set; }

        public bool IsVacant { get; set; }

        public string CarNumber { get; set; }
    }
}
