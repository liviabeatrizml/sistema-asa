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

    // Método para obter profissional
    public async Task<ProfissionalModel> GetProfissional(int id_profissional)
    {
        var response = await _httpClient.GetAsync($"/api/Discente/obter-profissional/{id_profissional}");
        
        // Garantindo que a requisição foi bem-sucedida
        if (response.IsSuccessStatusCode)
        {
            // Lendo o conteúdo da resposta como string
            var jsonResponse = await response.Content.ReadAsStringAsync();
            
            // Desserializando o JSON
            var profissional = JsonSerializer.Deserialize<ProfissionalModel>(jsonResponse, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true // Ignora a diferença de maiúsculas e minúsculas
            });
            
            return profissional;
        }
        else
        {
            // Lidar com erros, se necessário
            throw new Exception("Falha ao obter os dados do profissional.");
        }
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

    // Método para obter os horários de um profissional
    public async Task<List<ProfissionalHorariosModel>> GetProfissionalHorarios(int id_profissional)
    {
        var response = await _httpClient.GetAsync($"/api/Agendamento/horarios/{id_profissional}");
        
        // Garantindo que a requisição foi bem-sucedida
        if (response.IsSuccessStatusCode)
        {
            // Lendo o conteúdo da resposta como string
            var jsonResponse = await response.Content.ReadAsStringAsync();
            
            // Desserializando o JSON
            var profissional_horarios = JsonSerializer.Deserialize<List<ProfissionalHorariosModel>>(jsonResponse, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true // Ignora a diferença de maiúsculas e minúsculas
            });
            
            profissional_horarios = profissional_horarios
            .OrderBy(c => c.DiaDaSemana)
            .ThenBy(c => c.HoraInicio)
            .ToList();
            
            return profissional_horarios;
        }
        else
        {
            // Lidar com erros, se necessário
            throw new Exception("Falha ao obter os dados do profissional.");
        }
    }
}

public class ProfissionalModel{
    public int IdProfissional { get; set;}
    public string Nome {get; set;}
    public string Email {get; set;}
    public int ServicoId {get; set;}
    public string ServicoNome {get; set;} 
    public string Descricao {get; set;} 
}

public class ProfissionalHorariosModel{
    public int IdHorario {get; set;}
    public TimeSpan HoraInicio { get; set; }
    public TimeSpan HoraFim {get; set;}
    public int DiaDaSemana{get; set;}
    public int ProfissionalId {get; set;}
}