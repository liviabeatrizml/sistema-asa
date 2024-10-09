using System.ComponentModel.DataAnnotations;

/// <summary>
/// Model que complementa informações relacionadas ao profissional 
/// para a construção da API de Registro
/// </summary>
public class RegistrarProfissional
{
    public string Nome { get; set; }
    public string Email { get; set; }

    [Required]
    [StringLength(99, MinimumLength = 6)]
    public string Senha { get; set; }
    public string AreaAtuacao { get; set; } // Campo para a área de atuação
}
