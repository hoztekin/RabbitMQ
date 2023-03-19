using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Consumer
{
	internal class Program
	{
		static void Main(string[] args)
		{
			//Bağlantı oluşturma
			ConnectionFactory factory = new ConnectionFactory();
			factory.Uri = new("amqps://rwbcodtq:CLgFjQjBrxJqVhDCFTBLel6RFHyqV7nL@shrimp.rmq.cloudamqp.com/rwbcodtq");  //Manager AMQP Details Url 

			//Bağlantıyı Aktifleştirme ve Kanal Açma
			using IConnection connection = factory.CreateConnection();
			using IModel channel = connection.CreateModel();

			//Queue Oluşturma
			channel.QueueDeclare(queue: "example-queue", exclusive: false); //Cunsomerdaki kuyruk publisher ile aynı olmalıdır.


			//Queuedaki mesajı okuma
			EventingBasicConsumer consumer = new(channel);
			channel.BasicConsume(queue: "example-queue", autoAck:false, consumer : consumer ); //AutoAck mesaj onaylama sürecini aktifleştirmek için cunsomer uygulamasında autoack parametresine false değerinin getirilmesiyle sağlanır.
			consumer.Received += (sender, e) =>
			{
				//Kuyruğa gelen mesajın işlendiği yer
				//e.body: Kuyruktaki mesajın verisini bütünsel olarak getirecektir.
				//e.Body.Span veya e.body.ToArray() kuyruktaki mesajı byte olarak bize verecektir.
				Console.WriteLine(Encoding.UTF8.GetString(e.Body.Span));
				channel.BasicAck(deliveryTag: e.DeliveryTag, multiple: false);
			};
			Console.ReadLine();
		}
	}
}  