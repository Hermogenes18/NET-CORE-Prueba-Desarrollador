using ApiRestTienda.Domain.Entities;
using ApiRestTienda.Repositories;

namespace ApiRestTienda.Services
{
    public class ChatbotService : IChatbotService
    {
        private readonly IRepository<Producto> _productoRepository;

        public ChatbotService(IRepository<Producto> productoRepository)
        {
            _productoRepository = productoRepository;
        }

        public async Task<string> ProcesarPreguntaAsync(string pregunta)
        {
            pregunta = pregunta.ToLower();

            if (pregunta.Contains("buscar") || pregunta.Contains("encontrar") || pregunta.Contains("tienes"))
            {
                return await BuscarProductosPorNombreAsync(pregunta);
            }

            if (pregunta.Contains("precio") || pregunta.Contains("cuesta") || pregunta.Contains("valor"))
            {
                return await ConsultarPrecioProductoAsync(pregunta);
            }

            
            if (pregunta.Contains("stock") || pregunta.Contains("disponible") || pregunta.Contains("hay") ||
                pregunta.Contains("quedan") || pregunta.Contains("existencias"))
            {
                return await ConsultarStockProductoAsync(pregunta);
            }

            
            if (pregunta.Contains("listar") || pregunta.Contains("mostrar") || pregunta.Contains("ver todos") ||
                pregunta.Contains("catálogo") || pregunta.Contains("catalogo"))
            {
                return await ListarProductosAsync();
            }

            
            return "Lo siento, no entendí tu pregunta. Puedes preguntar sobre productos, precios o disponibilidad.";
        }

        private async Task<string> BuscarProductosPorNombreAsync(string pregunta)
        {
            var productos = await _productoRepository.GetAllAsync();
            var resultados = new List<Producto>();

            foreach (var producto in productos)
            {
                if (pregunta.Contains(producto.Nombre.ToLower()))
                {
                    resultados.Add(producto);
                }
            }

            if (!resultados.Any())
            {

                var palabrasClave = pregunta.Split(' ')
                    .Where(p => p.Length > 3 && !EsPalabraComun(p))
                    .ToList();

                foreach (var producto in productos)
                {
                    foreach (var palabra in palabrasClave)
                    {
                        if (producto.Nombre.ToLower().Contains(palabra))
                        {
                            if (!resultados.Contains(producto))
                            {
                                resultados.Add(producto);
                            }
                        }
                    }
                }
            }

            if (resultados.Any())
            {
                var respuesta = "He encontrado los siguientes productos:\n";
                foreach (var producto in resultados)
                {
                    respuesta += $"- {producto.Nombre}: ${producto.Precio} (Stock: {producto.Stock})\n";
                }
                return respuesta;
            }

            return "Lo siento, no encontré productos que coincidan con tu búsqueda.";
        }

        private async Task<string> ConsultarPrecioProductoAsync(string pregunta)
        {
            var productos = await _productoRepository.GetAllAsync();

            foreach (var producto in productos)
            {
                if (pregunta.Contains(producto.Nombre.ToLower()))
                {
                    return $"El precio de {producto.Nombre} es ${producto.Precio}.";
                }
            }

            var palabrasClave = pregunta.Split(' ')
                .Where(p => p.Length > 3 && !EsPalabraComun(p))
                .ToList();

            foreach (var producto in productos)
            {
                foreach (var palabra in palabrasClave)
                {
                    if (producto.Nombre.ToLower().Contains(palabra))
                    {
                        return $"El precio de {producto.Nombre} es ${producto.Precio}.";
                    }
                }
            }

            return "Lo siento, no pude identificar el producto para consultar su precio.";
        }

        private async Task<string> ConsultarStockProductoAsync(string pregunta)
        {
            var productos = await _productoRepository.GetAllAsync();

            foreach (var producto in productos)
            {
                if (pregunta.Contains(producto.Nombre.ToLower()))
                {
                    if (producto.Stock > 0)
                        return $"Tenemos {producto.Stock} unidades disponibles de {producto.Nombre}.";
                    else
                        return $"Lo siento, actualmente no hay stock disponible de {producto.Nombre}.";
                }
            }

            var palabrasClave = pregunta.Split(' ')
                .Where(p => p.Length > 3 && !EsPalabraComun(p))
                .ToList();

            foreach (var producto in productos)
            {
                foreach (var palabra in palabrasClave)
                {
                    if (producto.Nombre.ToLower().Contains(palabra))
                    {
                        if (producto.Stock > 0)
                            return $"Tenemos {producto.Stock} unidades disponibles de {producto.Nombre}.";
                        else
                            return $"Lo siento, actualmente no hay stock disponible de {producto.Nombre}.";
                    }
                }
            }

            return "Lo siento, no pude identificar el producto para consultar su stock.";
        }

        private async Task<string> ListarProductosAsync()
        {
            var productos = await _productoRepository.GetAllAsync();

            if (!productos.Any())
            {
                return "No hay productos disponibles en este momento.";
            }

            var respuesta = "Estos son nuestros productos:\n";
            foreach (var producto in productos)
            {
                respuesta += $"- {producto.Nombre}: ${producto.Precio} (Stock: {producto.Stock})\n";
            }

            return respuesta;
        }

        private bool EsPalabraComun(string palabra)
        {

            var palabrasComunes = new[]
            {
                "que", "del", "los", "las", "por", "con", "para", "una", "tiene", "sobre",
                "como", "este", "esta", "estos", "estas", "cual", "cuando", "donde", "quien",
                "cuanto", "cuanta", "cuantos", "cuantas"
            };

            return palabrasComunes.Contains(palabra);
        }
    }
}
