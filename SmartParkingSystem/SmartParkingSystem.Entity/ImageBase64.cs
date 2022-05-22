using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartParkingSystem.Entity
{
    public class ImageBase64
    {
        public int ParkingId { get; set; }
        public string image { get; set; }
        public string SpaceNumber { get; set; }
    }
}
