using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Back_end.Models
{
    [Table("profissional")] // Define o nome da tabela no banco de dados
    public class Profissional
    {
        [Key]
        [Column("id_profissional")]
        public int IdProfissional { get; set; }

        [Required]
        [StringLength(70)]
        [Column("nome")]
        public string Nome { get; set; }

        [Required]
        [StringLength(255)]
        [Column("email")]
        public string Email { get; set; }

        [Required]
        [StringLength(99)]
        [Column("senha")]
        public string Senha { get; set; }

        [Required]
        [StringLength(255)]
        [Column("salt")]
        public string Salt { get; set; }

        [Column("servico_id")]
        public int ServicoId { get; set; }

        [ForeignKey("ServicoId")]
        public ServicoDisponivel Servico { get; set; }


    }
}
