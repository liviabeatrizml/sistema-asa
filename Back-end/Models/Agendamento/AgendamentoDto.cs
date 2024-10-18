namespace Back_end.Dtos
{
    public class AgendamentoDto
    {
        public int IdAgendamento { get; set; }
        public DateTime Data { get; set; }
        public int DiscenteId { get; set; }
        public int ProfissionalId { get; set; }
        public int ServicoId { get; set; }
        public int HorarioId { get; set; }
        public string Status { get; set; }
        public TimeSpan HoraInicio { get; set; } // Tipo TimeSpan para horas
        public TimeSpan HoraFim { get; set; }    // Tipo TimeSpan para horas
    }
}
