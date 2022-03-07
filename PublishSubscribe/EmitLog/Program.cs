using System;
using RabbitMQ.Client;
using System.Text;

namespace EmitLog
{
    /// <summary>
    /// Programa para publicar mensagens para um exchange ao invés de mandar uma mensagem diretamente para a fila
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                //Tipo de exchange Fanout é o que serve para entregar as mensagens para um exchange nesse executável
                //durable: metadados da mensagem são armazenados em disco. Caso falso as mensagens são "transient" transitorias, gravando em memória
                //channel.ExchangeDeclare(exchange: "logs", type: ExchangeType.Fanout);

                channel.ExchangeDeclare(exchange: "logs", type: ExchangeType.Fanout, durable: true);

                var message = GetMessage(args);
                var body = Encoding.UTF8.GetBytes(message);

                //Routingkey é ignorado quando o tipo de exchange informado é Fanout (não vamos direcionar a mensagem pra uma fila específica nesse caso)
                channel.BasicPublish(exchange: "logs", routingKey: "", basicProperties: null, body: body);
                Console.WriteLine(" [x] Sent {0}", message);
            }

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }

        private static string GetMessage(string[] args)
        {
            return ((args.Length > 0) ? string.Join(" ", args) : "info: Hello World!");
        }
    }
}
