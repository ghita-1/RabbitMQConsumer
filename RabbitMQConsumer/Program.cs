using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

class Program
{
    static void Main(string[] args)
    {
        ConnectionFactory factory = new ConnectionFactory() { HostName = "localhost", UserName = "guest", Password = "guest" };
        using IConnection connection = factory.CreateConnection();
        using IModel channel = connection.CreateModel();

        channel.QueueDeclare(queue: "Messages", durable: false, exclusive: false, autoDelete: false, arguments: null);

        EventingBasicConsumer consumer = new EventingBasicConsumer(channel);
        consumer.Received += (model, ea) =>
        {
            Byte[] body = ea.Body.ToArray();
            string message = Encoding.UTF8.GetString(body);
            Console.WriteLine(" [x] Received {0}", message);
        };

        channel.BasicConsume(queue: "Messages", autoAck: true, consumer: consumer);

        Console.WriteLine(" Press [enter] to exit.");
        Console.ReadLine();
    }
}
