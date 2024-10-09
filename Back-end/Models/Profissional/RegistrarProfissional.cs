using System.ComponentModel.DataAnnotations;

public class RegistrarProfissional
{
    public string Nome { get; set; }
    public string Email { get; set; }

    [Required]
    [StringLength(99, MinimumLength = 6)]
    public string Senha { get; set; }
    public string AreaAtuacao { get; set; } // Campo para a área de atuação
}
