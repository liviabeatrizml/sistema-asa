using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Back_end.Models
{
    /// <summary>
    /// Representa um horário disponível para agendamento.
    /// </summary>
    public class HorarioDisponivel
    {
        [Key]
        [Column("id_horario")] // Nome da coluna no banco de dados
        public int IdHorario { get; set; }

        [Column("horaInicio")]
        public TimeSpan HoraInicio { get; set; }

        [Column("horaFim")]
        public TimeSpan HoraFim { get; set; }

        [Column("diaDaSemana")]
        public int DiaDaSemana { get; set; }

        [ForeignKey("Profissional")]
        [Column("profissional_id")]
        public int ProfissionalId { get; set; }
        public Profissional Profissional { get; set; }
    }
}