using RabbitMQ.Client;
using System.Text;

namespace Mesaj_Gonder
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

            string message = "Merhaba, Bu bir mesajdır!!";
            var body = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish(
                exchange: "",
                routingKey: queueName,
                basicProperties: null,
                body: body
            );

            Console.WriteLine($"Mesaj gönderildi: {message}");

            Console.ReadLine();
        }
    }
}
