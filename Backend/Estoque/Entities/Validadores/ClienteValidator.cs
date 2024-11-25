using Entities.Entidades;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Entities.Validadores
{
    public class ClienteValidator : AbstractValidator<Cliente>
    {        
        public ClienteValidator()
        {
            RuleFor(cliente => cliente.Nome).NotEmpty().WithMessage("Nome Obrigatório.");

            RuleFor(cliente => cliente.CPF)
                .NotEmpty().WithMessage("O CPF é obrigatório")
                .Must(ValidarCPF).WithMessage("CPF é inválido");

            RuleForEach(cliente => cliente.Endereco)  
                .ChildRules(endereco =>
                {
                    endereco.RuleFor(e => e.CEP)
                        .NotEmpty().WithMessage("O CEP é obrigatório")
                        .Must(ValidarCEP).WithMessage("O CEP deve estar no formato correto (xxxxx-xxx ou xxxxxxxx)");
                });
        }

        private bool ValidarCPF(string cpf)
        {
            if (string.IsNullOrWhiteSpace(cpf))
                return false;

            cpf = cpf.Replace(".", "").Replace("-", "");

            if (cpf.Length != 11 || !long.TryParse(cpf, out _))
                return false;

            if (cpf.Distinct().Count() == 1)
                return false;

            var multiplicadores1 = new int[] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            var multiplicadores2 = new int[] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

            var tempCpf = cpf.Substring(0, 9);
            var soma = tempCpf.Select((t, i) => int.Parse(t.ToString()) * multiplicadores1[i]).Sum();
            var resto = soma % 11;
            var digito1 = resto < 2 ? 0 : 11 - resto;

            tempCpf += digito1;
            soma = tempCpf.Select((t, i) => int.Parse(t.ToString()) * multiplicadores2[i]).Sum();
            resto = soma % 11;
            var digito2 = resto < 2 ? 0 : 11 - resto;

            return cpf.EndsWith($"{digito1}{digito2}");
        }

        private bool ValidarCEP(string cep)
        {
            var cepLimpo = cep.Replace("-", "").Replace(".", "").Trim();
          
            return Regex.IsMatch(cepLimpo, @"^\d{5}(\d{3})?$");
        }
    }
}
