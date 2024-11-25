using Entities.Entidades;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Validadores
{
    public class EnderecoValidator : AbstractValidator<Endereco>
    {
        public EnderecoValidator() 
        {
            RuleFor(x => x.Cidade).NotEmpty().WithMessage("Cidade deve ser informada.");

            RuleFor(x => x.Estado).NotEmpty().WithMessage("Estado deve ser informado.");

            RuleFor(x => x.IdCliente).NotEmpty().WithMessage("Cliente deve ser informado.");

            RuleFor(x => x.Logradouro).NotEmpty().WithMessage("Logradouro deve ser informado.");

            RuleFor(x => x.Numero).NotEmpty().WithMessage("Número deve ser informado.");

            RuleFor(x => x.CEP)
                .NotEmpty().WithMessage("O CEP é obrigatório")
                .Matches(@"^\d{5}-\d{3}$").WithMessage("O CEP deve estar no formato XXXXX-XXX")
                .When(cliente => !string.IsNullOrWhiteSpace(cliente.CEP));
        }
    }
}
