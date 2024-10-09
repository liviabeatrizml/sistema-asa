namespace Back_end.Models
{
    public class ResultadoSolicitacao
    {
        public bool Sucesso { get; set; }
        public string Mensagem { get; set; }
        public Agendamento Agendamento { get; set; }
    }
}
