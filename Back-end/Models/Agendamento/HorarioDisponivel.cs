using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Back_end.Models
{
    /// <summary>
    /// Representação da tabela HorarioDisponivel no banco de dados em model
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
        public string DiaDaSemana { get; set; }

        [ForeignKey("Profissional")]
        [Column("profissional_id")]
        public int ProfissionalId { get; set; }
        public Profissional Profissional { get; set; }
    }
}