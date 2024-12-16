using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Mesaj_Oku
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory
            {
                Uri = new Uri("amqp://guest:guest@localhost:5672"),
                ClientProvidedName = "Mesaj Gönderici Uygulama"
            };

            using var connection = factory.CreateConnection();

            using IModel channel = connection.CreateModel();

            string queueName = "ornek_kuyruk";
            channel.QueueDeclare(
                queue: queueName,
                durable: true,     // Kuyruk kalıcı mı?
                exclusive: false,  // Diğer bağlantılar da kuyruğa erişebilir mi?
                autoDelete: false  // Kuyruk kullanılmadığında silinsin mi?
            );

            var consumer = new EventingBasicConsumer(channel);
            channel.BasicConsume(
                "ornek_kuyruk",
                true, 
                consumer
            );

            consumer.Received += Consumer_Received;

            Console.ReadLine();
        }

        private static void Consumer_Received(object? sender, BasicDeliverEventArgs e)
        {
            Console.WriteLine($"Gelen Mesaj: {Encoding.UTF8.GetString(e.Body.ToArray())}");
        }
    }
}
