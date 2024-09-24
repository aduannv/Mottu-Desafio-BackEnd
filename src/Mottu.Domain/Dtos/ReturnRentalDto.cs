using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Mottu.Domain.Dtos
{
    public class ReturnRentalDto
    {
        [Required]
        [JsonPropertyName("data_devolucao")]
        public DateTime DataDevolucao { get; set; }
    }
}
