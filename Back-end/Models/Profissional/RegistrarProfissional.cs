using System.ComponentModel.DataAnnotations;

/// <summary>
/// Representa os dados necessários para registrar um profissional.
/// </summary>
public class RegistrarProfissional
{
    public string Nome { get; set; }
    public string Email { get; set; }

    [Required]
    [StringLength(99, MinimumLength = 6)]
    public string Senha { get; set; }
    public string AreaAtuacao { get; set; } // Campo para a área de atuação
    public int ServicoId { get; set; }
}
