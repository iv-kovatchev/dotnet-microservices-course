using System.Text;
using System.Text.Json;
using RabbitMQ.Client;

public class MessageBusClient : IMessageBusClient
{
    private readonly IConfiguration _configuration;
    private readonly IConnection? _connection;
    private readonly IModel? _channel;

    public MessageBusClient(IConfiguration configuration)
    {
        _configuration = configuration;
        var factory = new ConnectionFactory() { 
            HostName = _configuration["RabbitMQHost"] ?? "localhost", 
            Port = int.Parse(_configuration["RabbitMQPort"] ?? "5672")
        };

        try {
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.ExchangeDeclare(exchange: "trigger", type: ExchangeType.Fanout);

            _connection.ConnectionShutdown += RabbitMQ_ConnetionShutdown!;

            Console.WriteLine("--> Connected to MessageBus");
        }
        catch(Exception ex) {
            Console.WriteLine($"--> Could not connect to the Message Bus: {ex.Message}");
        }
    }

    public void PublishNewPlatform(PlatformPublishedDto platformPublishedDto)
    {
        var message = JsonSerializer.Serialize(platformPublishedDto);

        if(_connection!.IsOpen) {
            Console.WriteLine("--> RabbitMQ Connection Open, sending message...");
            SendMessage(message);
        }
        else {
            Console.WriteLine("--> RabbitMQ Connections closed, not sending.");
        }
    }

    private void SendMessage(string message) {
        var body = Encoding.UTF8.GetBytes(message);

        _channel.BasicPublish(exchange: "trigger", routingKey: "", basicProperties: null, body);
    }

    private void RabbitMQ_ConnetionShutdown(object sender, ShutdownEventArgs e) {
        Console.WriteLine("--> RabbitMQ Connection Shutdown!");
    }
}