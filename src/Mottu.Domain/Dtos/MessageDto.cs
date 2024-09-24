namespace Mottu.Domain.Dtos
{
    public class MessageDto
    {
        public string Mensagem { get; set; }
        public MessageDto(string mensagem) => Mensagem = mensagem;
    }
}