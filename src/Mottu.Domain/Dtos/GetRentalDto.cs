using System.Text.Json.Serialization;

namespace Mottu.Domain.Dtos
{
    public class GetRentalDto
    {
        [JsonPropertyName("identificador")]
        public string Identificador { get; set; }

        [JsonPropertyName("valor_diaria")]
        public decimal ValorDiaria { get; set; }

        [JsonPropertyName("entregador_id")]
        public string EntregadorId { get; set; }

        [JsonPropertyName("moto_id")]
        public string MotoId { get; set; }

        [JsonPropertyName("data_inicio")]
        public DateTime DataInicio { get; set; }

        [JsonPropertyName("data_termino")]
        public DateTime DataTermino { get; set; }

        [JsonPropertyName("data_previsao_termino")]
        public DateTime DataPrevisaoTermino { get; set; }

        [JsonPropertyName("data_devolucao")]
        public DateTime? DataDevolucao { get; set; }
    }
}
