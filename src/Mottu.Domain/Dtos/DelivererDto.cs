using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Mottu.Domain.Dtos
{
    public class DelivererDto
    {
        [Required]
        public string Identificador { get; set; }

        [Required]
        public string Nome { get; set; }

        [Required]
        public string Cnpj { get; set; }

        [Required]
        [JsonPropertyName("data_nascimento")]
        public DateTime DataNascimento { get; set; }

        [Required]
        [JsonPropertyName("numero_cnh")]
        public string NumeroCnh { get; set; }

        [Required]
        [JsonPropertyName("tipo_cnh")]
        [RegularExpression(@"^(A|B|A\+B)$", ErrorMessage = "O tipo_cnh deve ser 'A', 'B' ou 'A+B'.")]
        public string TipoCnh { get; set; }

        [JsonPropertyName("imagem_cnh")]
        public string? ImagemCnh { get; set; }
    }
}
