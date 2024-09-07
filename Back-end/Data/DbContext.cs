using Back_end.Models;
using Microsoft.EntityFrameworkCore;

namespace Back_end.Data
{
    public class ApiDbContext : DbContext
    {
        public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options) { }

        public DbSet<Discente> Discentes { get; set; }

    }
}
