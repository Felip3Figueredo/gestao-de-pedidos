using Entities.Entidades;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Validadores
{
    public class PedidoDetalheValidator : AbstractValidator<PedidoDetalhe>
    {
        public PedidoDetalheValidator()
        {
            RuleFor(detalhe => detalhe.IdProduto).NotEmpty().WithMessage("Produto deve ser informado.");

            RuleFor(detalhe => detalhe.Quantidade)
                .GreaterThan(0).WithMessage("A quantidade deve ser maior que 0.");
        }
    }
}
