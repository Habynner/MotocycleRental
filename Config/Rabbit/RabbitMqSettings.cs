namespace challange_bikeRental.Config.Rabbit
{
    /// <summary>
    /// Represents the settings required to configure RabbitMQ connections.
    /// </summary>
    public class RabbitMqSettings
    {
        /// <summary>
        /// Gets or sets the host name used to connect to RabbitMQ.
        /// </summary>
        public string HostName { get; set; } = "localhost";
        /// <summary>
        /// Gets or sets the port number used to connect to RabbitMQ.
        /// </summary>
        public int Port { get; set; } = 5672;
        /// <summary>
        /// Gets or sets the name of the RabbitMQ queue.
        /// </summary>
        public string QueueName { get; set; } = "bike-queue";
    }
}
