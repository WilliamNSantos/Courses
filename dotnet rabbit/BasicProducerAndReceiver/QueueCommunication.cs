using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace BasicProducerAndReceiver;

public class QueueCommunication
{
    ConnectionFactory factory;
    public QueueCommunication()
    {
        factory = new ConnectionFactory() { HostName = "localhost" };
    }

    public void Produce(string? message){
        using (var connection = factory.CreateConnection())
        using (var channel = connection.CreateModel())
        {
            channel.QueueDeclare("teste123", false, false, false, null);

            message = !string.IsNullOrEmpty(message) ? message : "teste 123 teste 123";
            var body = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish("", "teste123", null, body);
        }
    }

    public void Consume()
    {
        using (var connection = factory.CreateConnection())
        using (var channel = connection.CreateModel())
        {
            channel.QueueDeclare("teste123", false, false, false, null);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, aa) => 
            {
                Console.WriteLine("teste 123");
                var body = aa.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine("Received Message {0}", message);
            };

            channel.BasicConsume("teste123", true, consumer);
            Console.WriteLine("Press key to stop receiver");
            Console.ReadKey();
        }
    }
}
