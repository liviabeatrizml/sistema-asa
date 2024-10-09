using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Back_end.Models
{
    [Table("discente")]
    public class Discente
    {
        [Key]
        [Column("id_discente")] // Mapeia a coluna 'id_discente' no BD
        public int IdDiscente { get; set; }

        [Required]
        [StringLength(70)]
        [Column("nome")] // Mapeia a coluna 'nome'
        public string Nome { get; set; }

        [Required]
        [StringLength(255)]
        [Column("email")] // Mapeia a coluna 'email'
        public string Email { get; set; }

        [Required]
        [StringLength(99)]
        [Column("senha")] // Mapeia a coluna 'senha'
        public string Senha { get; set; }

        [Required]
        [StringLength(255)]
        [Column("salt")]
        public string Salt { get; set; }

        [Required]
        [Column("matricula", TypeName = "INT")] // Mapeia a coluna 'matricula'
        public int Matricula { get; set; }

        [Required]
        [StringLength(14)]
        [Column("telefone")] // Mapeia a coluna 'telefone'
        public string Telefone { get; set; }

        [StringLength(100)]
        [Column("curso")] // Mapeia a coluna 'curso'
        public string Curso { get; set; }
    }
}
