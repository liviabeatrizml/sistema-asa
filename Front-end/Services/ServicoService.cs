using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;

public class ServicoService
{
    private readonly HttpClient _httpClient;
    private readonly ProfissionalService _profissionalService;

    public ServicoService(HttpClient httpClient, ProfissionalService profissionalService)
    {
        _httpClient = httpClient;
        _profissionalService = profissionalService;
    }

    // Método para obter os serviços
    public async Task<List<ServicoProfissionalModel>> ListServicos()
    {
        var servicos = new List<ServicoProfissionalModel>();

        // Obtendo os profissionais através do ProfissionalService
        var profissionais = await _profissionalService.ListProfissionais();

        foreach (var profissional in profissionais)
        {
            var response = await _httpClient.GetAsync($"/api/Servico/{profissional.ServicoId}");
            var jsonResponse = await response.Content.ReadAsStringAsync();

            var servico = JsonSerializer.Deserialize<ServicoModel>(jsonResponse, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true // Ignora a diferença de maiúsculas e minúsculas
            });

            var servico_profissional = new ServicoProfissionalModel
            {
                Nome_Profissional = profissional.Nome,
                Tipo_Servico = servico.Tipo,
                Descricao = servico.Descricao
            };

            servicos.Add(servico_profissional);
        }

        return servicos;
    }
}

public class ServicoProfissionalModel{
    public string Nome_Profissional {get; set;}
    public string Tipo_Servico {get; set;}
    public string Descricao {get; set;}
}


public class ServicoModel{
    public int IdServico {get; set;}
    public string Tipo {get; set;}
    public string Descricao {get; set;}
}