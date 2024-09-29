using System.ComponentModel.DataAnnotations;

namespace Back_end.Models
{
    /// <summary>
    /// Model que complementa informações necessarias para a construção da API de Login
    /// </summary>
    public class LoginDiscente
    {
        [Required]
        [EmailAddress]
        [StringLength(255)]
        public string Email { get; set; }

        [Required]
        [StringLength(99, MinimumLength = 6)]
        public string Senha { get; set; }
    }
}
