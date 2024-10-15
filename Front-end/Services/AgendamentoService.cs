using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;

public class AgendamentoService{
    private readonly HttpClient _httpClient;
    private readonly ProfissionalService _profissionalService;

    public AgendamentoService(HttpClient httpClient, ProfissionalService profissionalService)
    {
        _httpClient = httpClient;
        _profissionalService = profissionalService;
    }

    public async Task<List<CompromissoModel>> GetCompromissosDiscente(int id_discente)
    {
        var compromissos = new List<CompromissoModel>();

        var response = await _httpClient.GetAsync($"/api/Agendamento/listar/discente/{id_discente}");
        if (response.IsSuccessStatusCode)
        {
            var jsonResponse = await response.Content.ReadAsStringAsync();
            var agendamentos = JsonSerializer.Deserialize<List<AgendamentoModel>>(jsonResponse, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true // Ignora a diferença de maiúsculas e minúsculas
            });

            DateTime dataHoje = DateTime.Today;

            foreach (var agendamento in agendamentos)
            {
                if (agendamento.Data >= dataHoje)
                {
                    var profissional = await _profissionalService.GetProfissional(agendamento.ProfissionalId);

                    var profissional_horarios = await _profissionalService.GetProfissionalHorarios(agendamento.ProfissionalId);

                    var horario_inicio = TimeSpan.Zero;
                    var horario_fim = TimeSpan.Zero;

                    foreach (var horario in profissional_horarios)
                    {
                        if (agendamento.HorarioId == horario.IdHorario)
                        {
                            horario_inicio = horario.HoraInicio;
                            horario_fim = horario.HoraFim;
                        }
                    }

                    var compromisso = new CompromissoModel
                    {
                        Data = agendamento.Data,
                        HoraInicio = horario_inicio,
                        HoraFim = horario_fim,
                        Servico = profissional.ServicoNome,
                        Profissional_Nome = profissional.Nome
                    };
                    
                    compromissos.Add(compromisso);
                }
                
            }

        }

        compromissos = compromissos
        .OrderBy(c => c.Data)
        .ThenBy(c => c.HoraInicio)
        .ToList();
        return compromissos;
    }

    public async Task<List<CompromissoModel>> GetCompromissosDiscenteSemanaAtual(int id_discente)
    {
        var compromissos = new List<CompromissoModel>();

        var response = await _httpClient.GetAsync($"/api/Agendamento/listar/discente/{id_discente}");
        if (response.IsSuccessStatusCode)
        {
            var jsonResponse = await response.Content.ReadAsStringAsync();
            var agendamentos = JsonSerializer.Deserialize<List<AgendamentoModel>>(jsonResponse, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true // Ignora a diferença de maiúsculas e minúsculas
            });

            DateTime hoje = DateTime.Today;

            // Calcula o primeiro e o último dia da semana atual
            DateTime primeiroDiaSemana = hoje.AddDays(-(int)hoje.DayOfWeek);
            DateTime ultimoDiaSemana = primeiroDiaSemana.AddDays(6);

            foreach (var agendamento in agendamentos)
            {
                // Filtra apenas os compromissos que estão entre o domingo e o sábado da semana atual
                if (agendamento.Data >= primeiroDiaSemana && agendamento.Data <= ultimoDiaSemana)
                {
                    var profissional = await _profissionalService.GetProfissional(agendamento.ProfissionalId);

                    var profissional_horarios = await _profissionalService.GetProfissionalHorarios(agendamento.ProfissionalId);

                    var horario_inicio = TimeSpan.Zero;
                    var horario_fim = TimeSpan.Zero;

                    foreach (var horario in profissional_horarios)
                    {
                        if (agendamento.HorarioId == horario.IdHorario)
                        {
                            horario_inicio = horario.HoraInicio;
                            horario_fim = horario.HoraFim;
                        }
                    }

                    var compromisso = new CompromissoModel
                    {
                        Data = agendamento.Data,
                        HoraInicio = horario_inicio,
                        HoraFim = horario_fim,
                        Servico = profissional.ServicoNome,
                        Profissional_Nome = profissional.Nome
                    };

                    compromissos.Add(compromisso);
                }
            }
        }

        compromissos = compromissos
            .OrderBy(c => c.Data)
            .ThenBy(c => c.HoraInicio)
            .ToList();
        return compromissos;
    }

    public async Task<List<CompromissoModel>> GetCompromissosDiscenteHoje(int id_discente)
    {
        var compromissos = new List<CompromissoModel>();

        var response = await _httpClient.GetAsync($"/api/Agendamento/listar/discente/{id_discente}");
        if (response.IsSuccessStatusCode)
        {
            var jsonResponse = await response.Content.ReadAsStringAsync();
            var agendamentos = JsonSerializer.Deserialize<List<AgendamentoModel>>(jsonResponse, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true // Ignora a diferença de maiúsculas e minúsculas
            });

            DateTime hoje = DateTime.Today;

            foreach (var agendamento in agendamentos)
            {
                // Filtra apenas os compromissos que estão no dia de hoje
                if (agendamento.Data.Date == hoje)
                {
                    var profissional = await _profissionalService.GetProfissional(agendamento.ProfissionalId);

                    var profissional_horarios = await _profissionalService.GetProfissionalHorarios(agendamento.ProfissionalId);

                    var horario_inicio = TimeSpan.Zero;
                    var horario_fim = TimeSpan.Zero;

                    foreach (var horario in profissional_horarios)
                    {
                        if (agendamento.HorarioId == horario.IdHorario)
                        {
                            horario_inicio = horario.HoraInicio;
                            horario_fim = horario.HoraFim;
                        }
                    }

                    var compromisso = new CompromissoModel
                    {
                        Data = agendamento.Data,
                        HoraInicio = horario_inicio,
                        HoraFim = horario_fim,
                        Servico = profissional.ServicoNome,
                        Profissional_Nome = profissional.Nome
                    };

                    compromissos.Add(compromisso);
                }
            }
        }

        compromissos = compromissos
            .OrderBy(c => c.Data)
            .ThenBy(c => c.HoraInicio)
            .ToList();
        
        return compromissos;
    }

}

public class CompromissoModel{
    public DateTime Data { get; set; }
    public TimeSpan HoraInicio { get; set; }
    public TimeSpan HoraFim {get; set;}
    public string Servico { get; set; }
    public string Profissional_Nome { get; set; }
}

public class AgendamentoModel{
    public DateTime Data { get; set; }
    public int ProfissionalId{get; set;}
    public int HorarioId {get; set;}
}