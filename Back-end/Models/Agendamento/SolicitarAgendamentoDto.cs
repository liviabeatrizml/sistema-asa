using System;

namespace Back_end.Dtos
{
    /// <summary>
    /// Representa os dados necessários para solicitar um agendamento.
    /// </summary>
    public class SolicitarAgendamentoDto
    {
        public int DiscenteId { get; set; }
        public int ProfissionalId { get; set; }
        public int ServicoId { get; set; }
        public int HorarioId { get; set; }
        public DateTime Data { get; set; }
    }
}
