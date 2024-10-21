using Back_end.Models;
using Microsoft.EntityFrameworkCore;

namespace Back_end.Data
{
    /// <summary>
    /// Contexto da API que gerencia a conexão com o banco de dados e fornece acesso às entidades.
    /// </summary>
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
