using Azure.Messaging.ServiceBus;
using Eventos.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Amqp.Framing;
using System.Net.Mime;
using System.Text.Json;

namespace Eventos.Controllers
{
    [ApiController]
    [Route("api/pedidos")]
    public class PedidoController : ControllerBase
    {
        private readonly ServiceBusClient _serviceBusClient;
        private readonly string _queueName = "nuevo-pedido-queue";

        public PedidoController( ServiceBusClient serviceBusClient)
        {
            _serviceBusClient = serviceBusClient;
        }
        [HttpPost("crear")]
        public async Task<IActionResult> crearPedido([FromBody] NuevoPedidoEvent pedido)
        {
            pedido.PedidoId = Guid.NewGuid();
            pedido.FechaCreacion = DateTime.Now;

            string mensaje = JsonSerializer.Serialize(pedido);

            ServiceBusSender sender = _serviceBusClient.CreateSender(_queueName);

            ServiceBusMessage message = new ServiceBusMessage(mensaje)
            {
                ContentType = "application/json",
                Subject = "PedidoCreado",
                MessageId = pedido.PedidoId.ToString()
            };

            await sender.SendMessageAsync(message);

            return Ok(new { Message = "Pedido creado y enviado a la cola", PedidoId = pedido.PedidoId });


        }


    }
}
