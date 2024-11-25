using Dominio.InterfacesDeServicos;
using Entities.Entidades;
using Infra.Configuracao;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class ServicoDeProduto : IServicoDeProduto
    {
        private readonly DbContextOptions<ContextBase> _OptionBuilder;

        public ServicoDeProduto()
        {
            _OptionBuilder = new DbContextOptions<ContextBase>();
        }

        public async Task<List<Produto>> ListeProdutos(string? nomeFiltro, string? descricaoFiltro, decimal? precoMin, decimal? precoMax, int? codigoUnicoFiltro)
        {
            using (var data = new ContextBase(_OptionBuilder))
            {
                var query = data.Produto.AsQueryable();

                if (!string.IsNullOrEmpty(nomeFiltro))
                    query = query.Where(p => p.Nome.Contains(nomeFiltro));

                if (!string.IsNullOrEmpty(descricaoFiltro))
                    query = query.Where(p => p.Descricao.Contains(descricaoFiltro));

                if (precoMin.HasValue)
                    query = query.Where(p => p.Preco >= precoMin.Value);

                if (precoMax.HasValue)
                    query = query.Where(p => p.Preco <= precoMax.Value);


                return await query.ToListAsync();
            }
        }
    }
}
