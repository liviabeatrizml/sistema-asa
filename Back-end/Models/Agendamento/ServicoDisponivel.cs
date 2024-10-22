using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Back_end.Models
{
    public class ServicoDisponivel
    {
        [Key]
        [Column("id_servico")] // Define a coluna no banco de dados
        public int IdServico { get; set; }

        [Column("tipo")]
        public string Tipo { get; set; }

        [Column("tipoAtendimento")]
        public string TipoAtendimento { get; set; }
    }
}
