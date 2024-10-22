using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

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

    // Método para registrar discente
    public async Task<HttpResponseMessage> EditarDiscente(DiscenteRegistroModel model)
    {
        return await _httpClient.PutAsJsonAsync("/api/Discente/atualizar-perfil", model);
    }
    // Método para registrar discente
    public async Task<HttpResponseMessage> EditarSenha(DiscenteEditarSenhaModel model)
    {
        return await _httpClient.PutAsJsonAsync("/api/Discente/alterar-senha", model);
    }

    // Método para login de discente
    public async Task<HttpResponseMessage> LoginDiscente(DiscenteLoginModel model)
    {
        return await _httpClient.PostAsJsonAsync("/api/Discente/login", model);
    }

    // Método para obter discente
    public async Task<DiscenteRegistroModel> GetDiscente(int id_discente)
    {
        // Fazendo a requisição GET
        var response = await _httpClient.GetAsync($"/api/Discente/obter-discente/{id_discente}");

        // Garantindo que a requisição foi bem-sucedida
        if (response.IsSuccessStatusCode)
        {
            // Lendo o conteúdo da resposta como string
            var jsonResponse = await response.Content.ReadAsStringAsync();
            
            // Desserializando o JSON na classe Discente
            var discente = JsonSerializer.Deserialize<DiscenteRegistroModel>(jsonResponse, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true // Ignora a diferença de maiúsculas e minúsculas
            });
            
            return discente;
        }
        else
        {
            // Lidar com erros, se necessário
            throw new Exception("Falha ao obter os dados do discente.");
        }
    }

    public async Task<HttpResponseMessage> ExcluirDiscente(ExcluirDiscenteModel model)
    {
        var request = new HttpRequestMessage(HttpMethod.Delete, "/api/Discente/deletar-usuario/")
        {
            Content = JsonContent.Create(model) // Adiciona o corpo da requisição
        };

        return await _httpClient.SendAsync(request);
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

    public string? Telefone { get; set; }

    public string? Curso { get; set; }
}

public class DiscenteLoginModel
{
    [Required(ErrorMessage = "O campo Email é obrigatório.")]
    public string Email { get; set; }
    [Required(ErrorMessage = "O campo Senha é obrigatório.")]
    public string Senha { get; set; }
}

public class DiscenteEditarSenhaModel
{
    [Required(ErrorMessage = "O campo Email é obrigatório.")]
    public string Email { get; set; }

    [Required(ErrorMessage = "O campo Senha é obrigatório.")]
    public string SenhaAtual { get; set; }  

    [Required(ErrorMessage = "O campo da nova senha é obrigatório.")]
    public string NovaSenha { get; set; }
}

public class ExcluirDiscenteModel
{
    [Required(ErrorMessage = "O campo Email é obrigatório.")]
    public int IdUsuario { get; set; }

    [Required(ErrorMessage = "O campo Senha é obrigatório.")]
    public string Senha { get; set; }  
}
