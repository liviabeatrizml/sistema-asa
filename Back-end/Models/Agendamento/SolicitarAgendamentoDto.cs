using System;

namespace Back_end.Dtos
{
    /// <summary>
    /// Model que complementa informações necessarias para a construção das APIs de Agendamento
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
