namespace Mottu.Application.Exceptions
{
    public class MessageException
    {
        public string Mensagem { get; set; }
        public MessageException(string mensagem = "Dados inválidos") => Mensagem = mensagem;
    }
}
