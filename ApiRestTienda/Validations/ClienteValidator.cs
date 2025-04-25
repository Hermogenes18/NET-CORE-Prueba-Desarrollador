using ApiRestTienda.Domain.Entities;
using FluentValidation;

namespace ApiRestTienda.Validations
{
    public class ClienteValidator : AbstractValidator<Cliente>
    {
        public ClienteValidator()
        {
            RuleFor(c => c.Nombre)
                .NotEmpty().WithMessage("El nombre es obligatorio")
                .Length(2, 100).WithMessage("El nombre debe tener entre 2 y 100 caracteres");

            RuleFor(c => c.Correo)
                .NotEmpty().WithMessage("El correo es obligatorio")
                .EmailAddress().WithMessage("El correo no tiene un formato válido");

            RuleFor(c => c.FechaNacimiento)
                .NotEmpty().WithMessage("La fecha de nacimiento es obligatoria")
                .LessThan(DateTime.Now).WithMessage("La fecha de nacimiento debe ser menor a la fecha actual");
        }
    }

    public class ProductoValidator : AbstractValidator<Producto>
    {
        public ProductoValidator()
        {
            RuleFor(p => p.Nombre)
                .NotEmpty().WithMessage("El nombre es obligatorio")
                .Length(2, 100).WithMessage("El nombre debe tener entre 2 y 100 caracteres");

            RuleFor(p => p.Precio)
                .NotEmpty().WithMessage("El precio es obligatorio")
                .GreaterThan(0).WithMessage("El precio debe ser mayor a cero");

            RuleFor(p => p.Stock)
                .NotEmpty().WithMessage("El stock es obligatorio")
                .GreaterThanOrEqualTo(0).WithMessage("El stock debe ser mayor o igual a cero");
        }
    }

    public class PedidoValidator : AbstractValidator<Pedido>
    {
        public PedidoValidator()
        {
            RuleFor(p => p.ClienteId)
                .NotEmpty().WithMessage("El ID del cliente es obligatorio")
                .GreaterThan(0).WithMessage("El ID del cliente debe ser mayor a cero");

            RuleFor(p => p.Detalles)
                .NotEmpty().WithMessage("El pedido debe contener al menos un producto");

            RuleForEach(p => p.Detalles).SetValidator(new DetallePedidoValidator());
        }
    }

    public class DetallePedidoValidator : AbstractValidator<DetallePedido>
    {
        public DetallePedidoValidator()
        {
            RuleFor(d => d.ProductoId)
                .NotEmpty().WithMessage("El ID del producto es obligatorio")
                .GreaterThan(0).WithMessage("El ID del producto debe ser mayor a cero");

            RuleFor(d => d.Cantidad)
                .NotEmpty().WithMessage("La cantidad es obligatoria")
                .GreaterThan(0).WithMessage("La cantidad debe ser mayor a cero");
        }
    }
}
