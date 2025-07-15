using Microsoft.EntityFrameworkCore;
using System;

namespace OutletStatusPortal.Models
{
    public class Outletdbcontext : DbContext
    {
       public Outletdbcontext(DbContextOptions<Outletdbcontext> options) :base(options) { }

        public DbSet<Users> Users { get; set; }

        public DbSet<StockItem> StockItems { get; set; }
        public DbSet<BeforeOutletSetUp> BeforeOutletSetUps { get; set; }
        public DbSet<AfterOutletSetup> AfterOutletSetups { get; set; }
        public DbSet<DeviceSetupStatus> DeviceSetupStatuses { get; set; }
        public DbSet<StockTransaction> StockTransactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Store enums as strings (optional)
            modelBuilder.Entity<StockItem>()
                .Property(s => s.StockType)
                .HasConversion<string>();

            modelBuilder.Entity<StockTransaction>()
                .Property(t => t.OperationType)
                .HasConversion<string>();

            // Cascade delete DeviceStatus if BeforeOutletSetUp is deleted
            modelBuilder.Entity<BeforeOutletSetUp>()
                .HasMany(b => b.DeviceStatuses)
                .WithOne(d => d.Outlet)
                .HasForeignKey(d => d.Sl)
                .OnDelete(DeleteBehavior.Cascade);
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
        public void SeedStockData()
        {
            if (!StockItems.Any())
            {
                StockItems.AddRange(
                    new StockItem
                    {
                        VendorName = "Vendor A",
                        StockType = StockTypeEnum.StockForAll,
                        Pos = 10,
                        Om = 15,
                        Server = 5,
                        Router = 8,
                        Scanner = 6,
                        Icmo = 4
                    },
                    new StockItem
                    {
                        VendorName = "Vendor B",
                        OutletName = "Outlet-101",
                        StockType = StockTypeEnum.DirectStockForOutlet,
                        Pos = 2,
                        Om = 3,
                        Server = 1,
                        Router = 1,
                        Scanner = 1,
                        Icmo = 1
                    },
                    new StockItem
                    {
                        VendorName = "Vendor C",
                        StockType = StockTypeEnum.StockForAll,
                        Pos = 20,
                        Om = 20,
                        Server = 10,
                        Router = 10,
                        Scanner = 10,
                        Icmo = 5
                    }
                );

                SaveChanges();
            }
        }

    }
}
