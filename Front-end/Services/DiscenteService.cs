using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

public class DiscenteService
{
    private readonly HttpClient _httpClient;

    public DiscenteService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    // Método para registrar discente
    public async Task<HttpResponseMessage> RegistrarDiscente(DiscenteRegistroModel model)
    {
        return await _httpClient.PostAsJsonAsync("/api/Discente/registrar", model);
    }

    // Método para login de discente
    public async Task<HttpResponseMessage> LoginDiscente(DiscenteLoginModel model)
    {
        return await _httpClient.PostAsJsonAsync("/api/Discente/login", model);
    }
}

// Modelos de dados
public class DiscenteRegistroModel
{
    [Required(ErrorMessage = "O campo Nome é obrigatório.")]
    public string Nome { get; set; }

    [Required(ErrorMessage = "O campo Email é obrigatório.")]
    public string Email { get; set; }

    [Required(ErrorMessage = "O campo Senha é obrigatório.")]
    public string Senha { get; set; }

    [Required(ErrorMessage = "O campo Matrícula é obrigatório.")]
    public int Matricula { get; set; }

    public int? Telefone { get; set; }

    [Required(ErrorMessage = "O campo CPF é obrigatório.")]
    public string Cpf { get; set; }

    public string? Curso { get; set; }
}

public class DiscenteLoginModel
{
    [Required(ErrorMessage = "O campo Email é obrigatório.")]
    public string Email { get; set; }
    [Required(ErrorMessage = "O campo Senha é obrigatório.")]
    public string Senha { get; set; }
}
