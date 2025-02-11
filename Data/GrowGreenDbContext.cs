
using growgreen_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace growgreen_backend.Data
{
    public class GrowGreenDbContext : DbContext
    {
        public GrowGreenDbContext(DbContextOptions<GrowGreenDbContext> options) : base(options) { }

        public DbSet<ImageEntity> Images { get; set; }
        public DbSet<PesticidesModel> Pesticides { get; set; }
        public DbSet<CropsModel> Crops { get; set; }
    }

}
