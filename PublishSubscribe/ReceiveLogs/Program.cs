using System;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace ReceiveLogs
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: "logs", type: ExchangeType.Fanout);

                // declara uma fila com nome gerado automaticamente pelo servidor
                //var queueName = channel.QueueDeclare(queue: "").QueueName;


                //autodelete: se verdadeiro vai deletar a fila quando o ultimo consumidor for fechado
                //exclusive: se verdadeiro a fila só vai poder ser atualizada pela connection onde ela foi declarada. Se outra connection tentar ler dessa fila, Rabbit vai dar erro. São deletadas quando a conexão é fechada.
                var queueName = channel.QueueDeclare(queue: "", autoDelete: false, exclusive:false).QueueName;

                // a
                channel.QueueBind(queue: queueName, exchange: "logs", routingKey: "");

                Console.WriteLine(" [*] Waiting for logs.");

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    byte[] body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    Console.WriteLine(" [x] {0}", message);
                };
                channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);

                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();
            }
        }
    }
}
