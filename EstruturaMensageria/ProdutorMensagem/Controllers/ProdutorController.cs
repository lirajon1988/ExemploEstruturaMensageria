using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ProdutorMensagem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutorController : ControllerBase
    {
        [HttpPost]
        [Route("publicar")]
        public ActionResult Publicar(DomainClasses.Domain.Person mensagem)
        {
            try
            {
                var message = JsonConvert.SerializeObject(mensagem);
                RunAsync(message).Wait();

                /* var message = JsonConvert.SerializeObject(mensagem);
                 //caminho da api que faz a conexão com a fila
                 var requisicaoWeb = WebRequest.CreateHttp("http://localhost:2305/api/rabbit/publicar/");
                 requisicaoWeb.Method = "POST";
                 requisicaoWeb.ContentType = "application/json";
                 requisicaoWeb.ContentLength = message.Length;

                 using (var stream = requisicaoWeb.GetRequestStream())
                 {
                     //stream.Write(message, 0, message.Length);
                     stream.Close();
                 }

                 var teste = requisicaoWeb.GetResponse();*/
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

            return StatusCode(200);
        }


        static async Task<object> RunAsync(string message)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new System.Uri("http://localhost:2305/api/rabbit/publicar");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                

                var cha = new DomainClasses.Rabbit() { Credential = "", RabbitServer = "localhost", Mensagem = message, QueueName = "Integracao" };
                var msg = JsonConvert.SerializeObject(cha);
                var httpContent = new StringContent(msg, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync(client.BaseAddress, httpContent);
                return response;
            }

        }
    }
}
