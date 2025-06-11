using Microsoft.EntityFrameworkCore;
using System.Configuration;
using WerkstattlagerAPI.Models;

namespace WerkstattlagerAPI
{
    public class InventoryContext : DbContext
    {
        public InventoryContext() { }
        public InventoryContext(DbContextOptions<InventoryContext> options) : base(options) { }

        public DbSet<Item> Items { get; set; } = null!;
        public DbSet<Device> Devices { get; set; } = null!;
        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<Manufacturer> Manufacturers { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            if (!options.IsConfigured)
            {
                var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
                if (environment == "Testing")
                    options.UseInMemoryDatabase("TestDatabase");
                else
                    options.UseSqlServer(ConfigurationManager.ConnectionStrings["Werkstattlager"]?.ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder model)
        {
            model.Entity<Item>()
                .HasOne(i => i.Device)
                .WithMany(d => d.Items)
                .HasForeignKey(i => i.DeviceId)
                .OnDelete(DeleteBehavior.Restrict);

            model.Entity<Device>(d =>
            {
                d.HasOne(d => d.Category).
                WithMany(c => c.Devices).
                HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
                d.HasOne(d => d.Manufacturer)
                .WithMany(m => m.Devices)
                .HasForeignKey(d => d.ManufacturerId)
                .OnDelete(DeleteBehavior.Restrict);
            });

            model.Entity<Category>(c =>
            {
                c.Property(c => c.Id)
                .ValueGeneratedNever();
            });
        }
    }
}
