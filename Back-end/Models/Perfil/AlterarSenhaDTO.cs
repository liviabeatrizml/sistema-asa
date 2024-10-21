/// <summary>
/// Representa os dados necessários para alterar a senha de um discente.
/// </summary>
public class AlterarSenhaDto
{
    public string Email { get; set; }
    public string SenhaAtual { get; set; }
    public string NovaSenha { get; set; }
}
