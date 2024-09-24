using System;

namespace Back_end.Dtos
{
    public class SolicitarAgendamentoDto
    {
        public int DiscenteId { get; set; }
        public int ProfissionalId { get; set; }
        public int ServicoId { get; set; }
        public int HorarioId { get; set; }
        public DateTime Data { get; set; }
    }
}
