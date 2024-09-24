using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Mottu.Domain.Dtos
{
    public class CreateRentalDto
    {
        [Required]
        [JsonPropertyName("entregador_id")]
        public string EntregadorId { get; set; }

        [Required]
        [JsonPropertyName("moto_id")]
        public string MotoId { get; set; }

        [Required]
        [JsonPropertyName("data_inicio")]
        public DateTime DataInicio { get; set; }

        [Required]
        [JsonPropertyName("data_termino")]
        public DateTime DataTermino { get; set; }

        [Required]
        [JsonPropertyName("data_previsao_termino")]
        public DateTime DataPrevisaoTermino { get; set; }

        [Required]
        [JsonPropertyName("plano")]
        public int Plano { get; set; }
    }
}
