using System.ComponentModel.DataAnnotations;

namespace Mottu.Infrastructure.DbContext.Models
{
    public class Rental
    {
        [Key]
        [Required]
        public string Identificador { get; set; }

        [Required]
        public string EntregadorId { get; set; }

        [Required]
        public string MotoId { get; set; }

        [Required]
        public DateTime DataInicio { get; set; }

        [Required]
        public DateTime DataTermino { get; set; }

        [Required]
        public DateTime DataPrevisaoTermino { get; set; }

        public DateTime? DataDevolucao { get; set; }

        [Required]
        public int Plano { get; set; }


        public Motorcycle Motorcycle { get; set; }
        public Deliverer Deliverer { get; set; }
    }
}
