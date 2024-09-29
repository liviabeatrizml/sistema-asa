using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Back_end.Models
{
    /// <summary>
    /// Representação da tabela Agendamento no banco de dados em model
    /// </summary>
    public class Agendamento
    {
        [Key]
        [Column("id_agendamento")]
        public int IdAgendamento { get; set; }

        [Column("data")]
        public DateTime Data { get; set; }

        [Column("discente_id")]
        public int DiscenteId { get; set; }
        public Discente Discente { get; set; }

        [Column("profissional_id")]
        public int ProfissionalId { get; set; }
        public Profissional Profissional { get; set; }

        [Column("servico_id")]
        public int ServicoId { get; set; }

        [Column("horario_id")]
        public int HorarioId { get; set; }
    }

}
