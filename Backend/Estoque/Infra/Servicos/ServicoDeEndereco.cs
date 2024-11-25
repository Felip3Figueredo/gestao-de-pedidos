using Dominio.InterfacesDeServicos;
using Entities.Entidades;
using Entities.Enums;
using Infra.Configuracao;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class ServicoDeEndereco : IServicoDeEndereco
    {
        private readonly DbContextOptions<ContextBase> _OptionBuilder;

        public ServicoDeEndereco()
        {
            _OptionBuilder = new DbContextOptions<ContextBase>();
        }

        public async Task<IEnumerable<Endereco>> ListarEnderecosComFiltros(string? logradouro, string? cep, string? bairro, string? cidade, string? estado,
     string? complemento, string? tipoEndereco, string? numero)
        {
            using (var data = new ContextBase(_OptionBuilder))
            {
                var query = data.Endereco.AsQueryable();

                if (!string.IsNullOrEmpty(logradouro)) query = query.Where(e => e.Logradouro.Contains(logradouro));
                if (!string.IsNullOrEmpty(cep)) query = query.Where(e => e.CEP.Contains(cep));
                if (!string.IsNullOrEmpty(bairro)) query = query.Where(e => e.Bairro.Contains(bairro));
                if (!string.IsNullOrEmpty(cidade)) query = query.Where(e => e.Cidade.Contains(cidade));
                if (!string.IsNullOrEmpty(estado)) query = query.Where(e => e.Estado.Contains(estado));
                if (!string.IsNullOrEmpty(complemento)) query = query.Where(e => e.Complemento.Contains(complemento));

                if (!string.IsNullOrEmpty(tipoEndereco) && Enum.TryParse<EnumTipoEndereco>(tipoEndereco, true, out var tipo))
                {
                    query = query.Where(e => e.TipoEndereco == tipo);
                }

                if (!string.IsNullOrEmpty(numero) && int.TryParse(numero, out var numeroInt))
                {
                    query = query.Where(e => e.Numero == numeroInt);
                }

                return await query.ToListAsync();
            }
        }
    }
}
