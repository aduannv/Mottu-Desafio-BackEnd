using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Mottu.Domain.Dtos
{
    public class ImagemCnhDto
    {
        [Required]
        [JsonPropertyName("imagem_cnh")]
        public string ImagemCnh { get; set; }
    }
}
