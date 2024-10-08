using System.ComponentModel.DataAnnotations;

namespace Back_end.Models
{
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
