/// <summary>
/// Representa os dados necessários para atualizar o perfil de um discente.
/// </summary>
public class AtualizarPerfilDto
{
    public string Nome { get; set; }
    public string Email { get; set; }
    public string Telefone { get; set; }
    public int Matricula { get; set; }
    public string Curso { get; set; }
}
