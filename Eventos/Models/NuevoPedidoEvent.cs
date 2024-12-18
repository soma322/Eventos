namespace Eventos.Models
{
    public class NuevoPedidoEvent
    {
        public Guid PedidoId { get; set; }
        public string ClienteId { get; set; }
        public List<string> Productos { get; set; }
        public decimal Total { get; set; }
        public DateTime FechaCreacion { get; set; }
    }
}
