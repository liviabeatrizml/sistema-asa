namespace Back_end.Models
{
    /// <summary>
    /// Model que complementa informações necessarias para a construção das APIs de Agendamento
    /// </summary>
    public class ResultadoSolicitacao
    {
        public bool Sucesso { get; set; }
        public string Mensagem { get; set; }
        public Agendamento Agendamento { get; set; }
    }
}
