using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartParkingSystem.Entity
{
    public class ParkingDto
    {
        public int Id { get; set; }

        public String Name { get; set; }

        public ICollection<SpacesDto> ParkingList { get; set; }
    }
}
