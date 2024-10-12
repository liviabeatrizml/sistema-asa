using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;

public class ServicoService
{
    private readonly HttpClient _httpClient;
    public ServicoService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    // Método para obter os serviços
    public async Task<List<ServicoProfissionalModel>> ListServicos()
    {
        // Fazendo a requisição GET
        var response = await _httpClient.GetAsync("/api/Discente/listar-profissionais");
        
        var servicos = new List<ServicoProfissionalModel> {};
        
        // Garantindo que a requisição foi bem-sucedida
        if (response.IsSuccessStatusCode){

            var jsonResponse = await response.Content.ReadAsStringAsync();

            var profissionais = JsonSerializer.Deserialize<List<ProfissionalModel>>(jsonResponse, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true // Ignora a diferença de maiúsculas e minúsculas
            });

            if (profissionais != null)
            {
                foreach (var profissional in profissionais)
                {
                    var response_servico = await _httpClient.GetAsync($"/api/Servico/{profissional.ServicoId}");
                    var jsonResponse_servico = await response_servico.Content.ReadAsStringAsync();

                    var servico =  JsonSerializer.Deserialize<ServicoModel>(jsonResponse_servico, new JsonSerializerOptions
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
                
            }
        }
        else
        {
            // Lidar com erros, se necessário
            throw new Exception("Falha ao obter os dados do discente.");
        }

        
        return servicos;
    }

}

public class ServicoProfissionalModel{
    public string Nome_Profissional {get; set;}
    public string Tipo_Servico {get; set;}
    public string Descricao {get; set;}
}

public class ProfissionalModel{
    public string Nome {get; set;}
    public string Email {get; set;}
    public int ServicoId {get; set;}
    public string ServicoNome {get; set;} 
}

public class ServicoModel{
    public int IdServico {get; set;}
    public string Tipo {get; set;}
    public string Descricao {get; set;}
}