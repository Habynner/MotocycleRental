using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using challange_bikeRental.Config.Rabbit;

namespace challange_bikeRental.Utils.Producers
{
    /// <summary>
    /// Provides functionality to publish messages to a RabbitMQ queue.
    /// </summary>
    public class RabbitMqProducer
    {
        private readonly RabbitMqSettings _settings;

        /// <summary>
        /// Initializes a new instance of the <see cref="RabbitMqProducer"/> class with the specified RabbitMQ settings.
        /// </summary>
        /// <param name="options">The RabbitMQ settings options.</param>
        public RabbitMqProducer(IOptions<RabbitMqSettings> options)
        {
            _settings = options.Value;
        }

        /// <summary>
        /// Publishes a message to the configured RabbitMQ queue.
        /// </summary>
        /// <param name="message">The message object to be published.</param>
        public void Publish(object message)
        {
            try
            {
                var factory = new ConnectionFactory
                {
                    HostName = _settings.HostName,
                    Port = _settings.Port
                };

                using var connection = factory.CreateConnection();
                using var channel = connection.CreateModel();

                // Console.WriteLine($"[RabbitMQ] Conectado em {_settings.HostName}:{_settings.Port}");

                channel.QueueDeclare(queue: _settings.QueueName,
                                     durable: true,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                var json = JsonSerializer.Serialize(message);
                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = null, // respeita o JsonPropertyName
                    WriteIndented = false
                };
                var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message, options));

                // Console.WriteLine($"[RabbitMQ] Publicando na fila {_settings.QueueName}: {json}");

                channel.BasicPublish(exchange: "",
                                     routingKey: _settings.QueueName,
                                     basicProperties: null,
                                     body: body);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[RabbitMQ] Erro ao publicar mensagem: {ex.Message}");
            }
        }
    }
}
