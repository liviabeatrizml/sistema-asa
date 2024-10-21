using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Back_end.Models
{   
    /// <summary>
    /// Representa um serviço disponível no sistema.
    /// </summary>
    public class ServicoDisponivel
    {
        [Key]
        [Column("id_servico")] // Define a coluna no banco de dados
        public int IdServico { get; set; }

        [Column("tipo")]
        public string Tipo { get; set; }

        [Column("status")]
        public string status { get; set; }
    }
}
