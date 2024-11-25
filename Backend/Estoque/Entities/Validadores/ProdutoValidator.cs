using Entities.Entidades;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Validadores
{
    public class ProdutoValidator : AbstractValidator<Produto>
    {
        public ProdutoValidator() 
        {
            RuleFor(produto => produto.Nome).NotEmpty().WithMessage("Nome obrigatório");

            RuleFor(produto => produto.Preco)
                .GreaterThan(0).WithMessage("O valor deve ser maior do que zero.");
        }
    }
}
