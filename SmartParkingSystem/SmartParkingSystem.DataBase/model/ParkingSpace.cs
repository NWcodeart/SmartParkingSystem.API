using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartParkingSystem.DataBase.model
{
    public class ParkingSpace
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string ParkingNumber { get; set; }

        [Required]
        [ForeignKey("Id")]
        public int ParkingId { get; set; }
        public CompanyParking companyParking { get; set; }

#nullable disable
        public bool IsVacant = true;

        #nullable enable
        public string? CarNumber { get; set; }

    }
}
