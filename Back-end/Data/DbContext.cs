using Back_end.Models;
using Microsoft.EntityFrameworkCore;

namespace Back_end.Data
{
    public class ApiDbContext : DbContext
    {
        public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options) { }

        public DbSet<Discente> Discentes { get; set; }
        public DbSet<Profissional> Profissionais { get; set; }
        public DbSet<ServicoDisponivel> ServicoDisponivel { get; set; }

        public DbSet<HorarioDisponivel> HorarioDisponivel { get; set; }
        public DbSet<Agendamento> Agendamento { get; set; }
    }
}
