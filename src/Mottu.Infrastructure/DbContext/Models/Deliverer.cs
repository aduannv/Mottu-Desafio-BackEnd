using System.ComponentModel.DataAnnotations;

namespace Mottu.Infrastructure.DbContext.Models
{
    public class Deliverer
    {
        [Key]
        public string Identificador { get; set; }
        public string Nome { get; set; }
        public string Cnpj { get; set; }
        public DateTime DataNascimento { get; set; }
        public string NumeroCnh { get; set; }
        public string TipoCnh { get; set; }

        public ICollection<Rental> Rentals { get; set; }
    }
}
