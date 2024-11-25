using Entities.Entidades;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Validadores
{
    public class PedidoValidator : AbstractValidator<Pedido>
    {
        public PedidoValidator() 
        {
            RuleFor(pedido => pedido.IdCliente)
                .NotEmpty().WithMessage("O ID do cliente é obrigatório.");

            RuleFor(pedido => pedido.IdEnderecoEntrega)
                .NotEmpty().WithMessage("O ID do endereço de entrega é obrigatório.");

            RuleFor(pedido => pedido.IdEnderecoCobranca)
                .NotEmpty().WithMessage("O ID do endereço de cobrança é obrigatório.");

            RuleFor(pedido => pedido.Detalhes)
                .NotNull().WithMessage("O pedido deve ter pelo menos um detalhe.")
                .Must(DetalhesValidos).WithMessage("O pedido deve conter detalhes válidos.");

            RuleForEach(pedido => pedido.Detalhes)
                .ChildRules(detalhe =>
                {
                    detalhe.RuleFor(d => d.Quantidade)
                        .GreaterThan(0).WithMessage("A quantidade de deve ser maior que zero.");
                });
        }

        private bool DetalhesValidos(ICollection<PedidoDetalhe> detalhes)
        {
            return detalhes != null && detalhes.Count > 0;
        }
    }
}
