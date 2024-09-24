using System.ComponentModel.DataAnnotations;

namespace Mottu.Infrastructure.DbContext.Models
{
    public class BrandNewMotorcycle
    {
        [Key]
        public string Identificador { get; set; }
        public int Ano { get; set; }
        public string Modelo { get; set; }
        public string Placa { get; set; }
    }
}