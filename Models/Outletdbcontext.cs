using Microsoft.EntityFrameworkCore;
using System;

namespace OutletStatusPortal.Models
{
    public class Outletdbcontext : DbContext
    {
       public Outletdbcontext(DbContextOptions<Outletdbcontext> options) :base(options) { }

        public DbSet<Users> Users { get; set; }
        public DbSet<NewOutletInfo> NewOutletInfos { get; set; }
        public DbSet<DeviceSetupStatus> DeviceSetupStatuses { get; set; }
        public DbSet<Arise_Problem> Arise_Problems { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Seed 2 users
            modelBuilder.Entity<Users>().HasData(
                new Users
                {
                    StafId = "l53335",
                    Name = "Jaber Hosen",
                    PassWord = "1234", // You should hash passwords in real apps
                    Phone = "01700000001",
                    Role = "Admin",
                    Date = DateTime.Now
                },
                new Users
                {
                    StafId = "l54445",
                    Name = "Sadia Akter",
                    PassWord = "jaber hosen",
                    Phone = "01700000002",
                    Role = "User",
                    Date = DateTime.Now
                }
            );
        }
    }
}
