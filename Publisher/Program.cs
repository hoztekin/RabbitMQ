using RabbitMQ.Client;
using System.Text;

namespace Publisher
{
	internal class Program
	{
		static void Main(string[] args)
		{
			/* Rabbit Mq Erland diliyle geliştirilmiştir.
			 * Open source olan bir mesaj quering sistemdir.
			 * Cross platformdur.
			 * Default exchange direct exchange'dir.
			 * Dağıtık yapılarda fanout exchange kullanılabilir.
			 * Direct exchange'in default router ismi kuyruk ismidir.
			 */
			

			//Bağlantı oluşturma
			ConnectionFactory factory = new ConnectionFactory();
			factory.Uri = new("amqps://rwbcodtq:CLgFjQjBrxJqVhDCFTBLel6RFHyqV7nL@shrimp.rmq.cloudamqp.com/rwbcodtq");  //Manager AMQP Details Url 


			//Bağlantıyı Aktifleştirme ve Kanal Açma
			using IConnection connection = factory.CreateConnection();
			using IModel channel =  connection.CreateModel();


			//Queue Oluşturma
			channel.QueueDeclare(queue: "example-queue", exclusive:false,  durable: true); //durable mesajların kalıcı olarak kaybolmaması için kuyruk konfigürasyonudur.

			//Mesajın kalıcı olması mesaj bölümü konfigürasyonudur.
			IBasicProperties properties = channel.CreateBasicProperties();
			properties.Persistent = true; 


			//Queue'ya mesaj gönderme

			//RabbitMQ kuyruğa atacağı mesajları byte türünden göndermektedir. Dolayısıyla mesajşarımızı byte a dönüştürmemiz gerekir.

			byte[] message = Encoding.UTF8.GetBytes("Test Mesajı");
			channel.BasicPublish(exchange:"", routingKey: "example-queue", body:message);

			Console.ReadLine();

		}
	}
}