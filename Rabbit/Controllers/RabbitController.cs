using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rabbit.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RabbitController : ControllerBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mensagem">mensagem</param>
        /// <param name="queueName">Nome da fila</param>
        /// <param name="rabbitServer">string de conexão com o servidor RabbitMQ</param>
        /// <param name="credential">credencial para acesso</param>
        /// <returns></returns>
        [HttpPost]
        [Route("publicar")]
        public ActionResult Publicar(DomainClasses.Rabbit request)
        {
            try
            {
                var factory = new ConnectionFactory() { HostName = request.RabbitServer };
                using (var connection = factory.CreateConnection())
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: request.QueueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
                    //Fala para a fila não encaminhar mais do que uma mensagem para o consumidor
                    //De forma que um consumidor ocupado não possa receber mais de uma mensagem
                    channel.BasicQos(0, 1, false);

                    var message = request.Mensagem;
                    var body = Encoding.UTF8.GetBytes(message);

                    var properties = channel.CreateBasicProperties();
                    properties.Persistent = true;
                    channel.BasicPublish(exchange: "", routingKey: request.QueueName, basicProperties: properties, body: body);
                    return StatusCode(200);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
            finally
            {

            }

        }
    }
}
