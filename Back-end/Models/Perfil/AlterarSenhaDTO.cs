/// <summary>
/// Classe responsavel para definir atributos para alterar email e senha
/// do usuario que serão usados na API de AtualizarPerfil
/// </summary>
public class AlterarSenhaDto
{
    public string Email { get; set; }
    public string SenhaAtual { get; set; }
    public string NovaSenha { get; set; }
}
