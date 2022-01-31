
using Microsoft.EntityFrameworkCore;

namespace SmartParkingSystem.DataBase.model
{
    public class ParkingContext : DbContext
    {
        public ParkingContext(DbContextOptions<ParkingContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("DbConnection");
            }
        }

        //models of the  database
        public DbSet<AddParking> companyParkings { get; set; }
        public DbSet<ParkingSpace> parkingSpaces { get; set; }


        //fluent api
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //every company parking has Multiple parking spaces
            modelBuilder.Entity<ParkingSpace>()
                .HasOne<AddParking>(p => p.companyParking)
                .WithMany(p => p.ParkingList)
                .HasForeignKey(f => f.ParkingId);
        }
            
    }
}
