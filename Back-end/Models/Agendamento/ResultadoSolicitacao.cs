namespace Back_end.Models
{
    /// <summary>
    /// Representa o resultado de uma solicitação de agendamento.
    /// </summary>
    public class ResultadoSolicitacao
    {
        public bool Sucesso { get; set; }
        public string Mensagem { get; set; }
        public Agendamento Agendamento { get; set; }
    }
}
