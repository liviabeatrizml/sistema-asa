using Back_end.Models;
using Microsoft.EntityFrameworkCore;

namespace Back_end.Data
{
    /// <summary>
    /// Instancia a API e cria a conexão com o banco de dados
    /// </summary>
    public class ApiDbContext : DbContext
    {
        public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options) { }
        
        /// <summary>
        /// Propriedade que representa a tabela Discente no banco de dados
        /// </summary>
        public DbSet<Discente> Discentes { get; set; }

        /// <summary>
        /// Propriedade que representa a tabela Profissionais no banco de dados
        /// </summary>
        public DbSet<Profissional> Profissionais { get; set; }
<<<<<<< HEAD
        public DbSet<ServicoDisponivel> ServicoDisponivel { get; set; }

=======

        /// <summary>
        /// Propriedade que representa a tabela ServicosDisponiveis no banco de dados
        /// </summary>
        public DbSet<ServicoDisponivel> ServicosDisponiveis { get; set; }
        
        /// <summary>
        /// Propriedade que representa a tabela HararioDisponivel no banco de dados
        /// </summary>
>>>>>>> origin/back-end
        public DbSet<HorarioDisponivel> HorarioDisponivel { get; set; }

        /// <summary>
        /// Propriedade que representa a tabela Agendamento no banco de dados
        /// </summary>
        public DbSet<Agendamento> Agendamento { get; set; }
    }
}
