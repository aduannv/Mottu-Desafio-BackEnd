namespace Mottu.Application.Settings
{
    public class RabbitMQSettings
    {
        public string HostName { get; set; }
        public ushort Port { get; set; } = 5672;
        public string CreateMotorcycleQueue { get; set; } = "create_motorcycle_queue";
    }
}
