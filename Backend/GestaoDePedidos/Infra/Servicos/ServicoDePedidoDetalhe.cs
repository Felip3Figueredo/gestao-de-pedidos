using Dominio.InterfacesDeRepositorio;
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
    public class ServicoDePedidoDetalhe : IServicoDePedidoDetalhe
    {
        private readonly DbContextOptions<ContextBase> _OptionBuilder;
        private readonly InterfaceRepositorioProduto _interfaceRepositorioProduto;

        public ServicoDePedidoDetalhe(InterfaceRepositorioProduto interfaceRepositorioProduto)
        {
            _OptionBuilder = new DbContextOptions<ContextBase>();
            _interfaceRepositorioProduto = interfaceRepositorioProduto;
        }

        public async Task<List<PedidoDetalhe>> ListarPedidosDetalhe(
            int? idPedido, 
            string? idProduto,
            int? quantidade,
            decimal? valorUnitario)
        {
            using (var data = new ContextBase(_OptionBuilder))
            {
                var query = data.PedidoDetalhe.Include(c => c.Produto).AsQueryable();

                if (idPedido != null)
                {
                    query = query.Where(p => p.IdPedido == idPedido);
                }

                if (!string.IsNullOrEmpty(idProduto))
                {
                    query = query.Where(p => p.IdProduto.Contains(idProduto));
                }

                if (quantidade.HasValue)
                {
                    query = query.Where(p => p.Quantidade == quantidade);
                }

                if (valorUnitario.HasValue)
                {
                    query = query.Where(p => p.ValorUnitario == valorUnitario);
                }

                return await query.ToListAsync();
            }
        }

        public async Task<PedidoDetalhe> ObtenhaPedidoDetalhe(string id)
        {
            using (var data = new ContextBase(_OptionBuilder))
            {
                try
                {
                    var pedido = await data.PedidoDetalhe
                        .Include(c => c.Produto)
                        .FirstOrDefaultAsync(x => x.Id == id);

                    if (pedido == null)
                    {
                        throw new KeyNotFoundException($"Detalhes com ID '{id}' não foi encontrado.");
                    }

                    return pedido;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }

        public List<PedidoDetalhe> DefinaIdentificadorPedido(List<PedidoDetalhe> listaDeDetalhes, int id)
        {
            foreach (var detalhe in listaDeDetalhes)
            {
                detalhe.IdPedido = id;
            }

            return listaDeDetalhes;
        }


        public List<PedidoDetalhe> DefinaValorUnitario(List<PedidoDetalhe> listaDeDetalhes)
        {
            foreach (var detalhe in listaDeDetalhes)
            {
                var produto = _interfaceRepositorioProduto.GetEntityById(detalhe.IdProduto).Result;

                if (produto == null)
                    throw new KeyNotFoundException($"Produto com ID '{produto.Id}' não foi encontrado.");

                detalhe.ValorUnitario = produto.Preco;
            }

            return listaDeDetalhes;
        }
    
    }
}
