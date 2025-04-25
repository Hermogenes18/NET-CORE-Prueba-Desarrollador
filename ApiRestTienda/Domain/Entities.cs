
namespace ApiRestTienda.Domain.Entities
{
    public class Cliente
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Correo { get; set; }
        public DateTime FechaNacimiento { get; set; }
    }

    public class Producto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public decimal Precio { get; set; }
        public int Stock { get; set; }
    }

    public class Pedido
    {
        public int Id { get; set; }
        public int ClienteId { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Total { get; set; }
        public List<DetallePedido> Detalles { get; set; } = new List<DetallePedido>();
    }

    public class DetallePedido
    {
        public int Id { get; set; }
        public int PedidoId { get; set; }
        public int ProductoId { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Subtotal { get; set; }
    }
}