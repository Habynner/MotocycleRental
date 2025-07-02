using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using challange_bikeRental.Config.Rabbit;
using challange_bikeRental.Models;
using challange_bikeRental.Repositories.Logs;

/// <summary>
/// Hosted service that consumes messages from a RabbitMQ queue and logs bike creation events.
/// </summary>
public class RabbitMqConsumerHostedService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly RabbitMqSettings _settings;
    private IConnection? _connection;
    private IModel? _channel;

    /// <summary>
    /// Initializes a new instance of the <see cref="RabbitMqConsumerHostedService"/> class.
    /// </summary>
    /// <param name="serviceProvider">The service provider for dependency injection.</param>
    /// <param name="options">The RabbitMQ settings options.</param>
    public RabbitMqConsumerHostedService(IServiceProvider serviceProvider, IOptions<RabbitMqSettings> options)
    {
        _serviceProvider = serviceProvider;
        _settings = options.Value;
    }

    /// <summary>
    /// Starts the hosted service and initializes the RabbitMQ connection and channel.
    /// </summary>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous start operation.</returns>
    public override Task StartAsync(CancellationToken cancellationToken)
    {
        var factory = new ConnectionFactory()
        {
            HostName = _settings.HostName,
            Port = _settings.Port
        };

        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
        _channel.QueueDeclare(queue: _settings.QueueName, durable: true, exclusive: false, autoDelete: false);

        return base.StartAsync(cancellationToken);
    }

    /// <summary>
    /// Executes the background service to consume messages from the RabbitMQ queue.
    /// </summary>
    /// <param name="stoppingToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous execute operation.</returns>
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += async (model, ea) =>
 {
     var body = ea.Body.ToArray();
     var message = Encoding.UTF8.GetString(body);
     Console.WriteLine($"[RabbitMQ Consumer] Mensagem recebida: {message}");

     var bike = JsonSerializer.Deserialize<Bike>(message);
     Console.WriteLine($"FabricationYear recebido: {bike?.FabricationYear}");

     if (bike?.FabricationYear == 2024)
     {
         using var scope = _serviceProvider.CreateScope();
         var repo = scope.ServiceProvider.GetRequiredService<ILogsMotorcycleCreatedRepository>();

         var log = new LogsMotorcycleCreated
         {
             Log = $"Bike {bike.Plate} created with fabrication year {bike.FabricationYear}."
         };

         await repo.CreateLogAsync(log);
         Console.WriteLine($"[RabbitMQ Consumer] Log criado para bike: {bike.Plate}");
     }
 };

        _channel.BasicConsume(queue: _settings.QueueName, autoAck: true, consumer: consumer);
        return Task.CompletedTask;
    }

    /// <summary>
    /// Releases the unmanaged resources used by the service and optionally releases the managed resources.
    /// </summary>
    public override void Dispose()
    {
        _channel?.Close();
        _connection?.Close();
        base.Dispose();
    }
}
