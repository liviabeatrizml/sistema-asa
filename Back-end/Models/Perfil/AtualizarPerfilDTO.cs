/// <summary>
/// Classe responsavel para definir atributos para alterar email, nome e telefone
/// do usuario que serão usados na API de AtualizarPerfil
/// </summary>
public class AtualizarPerfilDto
{
    public string Email { get; set; }
    public string Nome { get; set; }
    public string Telefone { get; set; }
}
