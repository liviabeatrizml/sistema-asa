using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;

public class ProfissionalService
{
    private readonly HttpClient _httpClient;

    public ProfissionalService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    // Método para listar os profissionais
    public async Task<List<ProfissionalModel>> ListProfissionais()
    {
        var response = await _httpClient.GetAsync("/api/Discente/listar-profissionais");
        
        if (response.IsSuccessStatusCode)
        {
            var jsonResponse = await response.Content.ReadAsStringAsync();
            var profissionais = JsonSerializer.Deserialize<List<ProfissionalModel>>(jsonResponse, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true // Ignora a diferença de maiúsculas e minúsculas
            });

            return profissionais;
        }
        else
        {
            // Lidar com erros, se necessário
            throw new Exception("Falha ao obter os dados dos profissionais.");
        }
    }
}

public class ProfissionalModel{
    public string Nome {get; set;}
    public string Email {get; set;}
    public int ServicoId {get; set;}
    public string ServicoNome {get; set;} 
}