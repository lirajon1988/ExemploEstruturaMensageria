using DomainClasses;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsumidorMensagem
{
    public class MessageConsumer : BackgroundService
    {
        private readonly Rabbit _rabbitConfiguration;
        private readonly IConnection _connection;
        private readonly IModel _channel;
        public MessageConsumer(IOptions<Rabbit> option)
        {
            _rabbitConfiguration = option.Value;
            var factory = new ConnectionFactory { HostName = _rabbitConfiguration.RabbitServer};
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(queue: _rabbitConfiguration.QueueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (sender, eventargs) =>
            {
                var contentArray = eventargs.Body.ToArray();
                var contentString = Encoding.UTF8.GetString(contentArray);
                var message = JsonConvert.DeserializeObject<DomainClasses.Rabbit>(contentString);

                //Aqui pode ter o método que vai fazer a chamada para a api que vai fazer o serviço

                _channel.BasicAck(eventargs.DeliveryTag, false);
            };

            _channel.BasicConsume(_rabbitConfiguration.QueueName, false, consumer);

            return Task.CompletedTask;
        }
    }
}
