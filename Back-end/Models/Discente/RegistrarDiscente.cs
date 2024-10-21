using System.ComponentModel.DataAnnotations;

namespace Back_end.Models
{
    /// <summary>
    /// Representa os dados necessários para registrar um novo discente.
    /// </summary>
    public class RegistrarDiscente
    {
        [Required]
        [StringLength(70)]
        public string Nome { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(255)]
        public string Email { get; set; }

        [Required]
        [StringLength(99, MinimumLength = 6)]
        public string Senha { get; set; }

        [Required]
        public int Matricula { get; set; }
        
        public string? Telefone { get; set; }

        [StringLength(100)]
        public string Curso { get; set; }
    }
}
