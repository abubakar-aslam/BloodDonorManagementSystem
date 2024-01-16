using Data_Access_Layer.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data_Access_Layer.DataContext
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options):base(options) { }
        public DbSet<Donor>Donors { get; set; }
        public DbSet<BloodInventoryStore> BloodInventoryStore { get; set; }
        public DbSet<Credentials> Credentials { get; set; }
    }
}
