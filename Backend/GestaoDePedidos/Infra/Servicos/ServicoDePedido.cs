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
    public class ServicoDePedido : IServicoDePedido
    {
        private readonly DbContextOptions<ContextBase> _OptionBuilder;

        public ServicoDePedido()
        {
            _OptionBuilder = new DbContextOptions<ContextBase>();
        }

        public async Task<List<Pedido>> ListarPedidos(
            string? idCliente,
            string? idEnderecoCobranca,
            string? idEnderecoEntrega,
            DateTime? dataPedidoInicio,
            DateTime? dataPedidoFim ,
            decimal? valorMinimo,
            decimal? valorMaximo)
        {
            using (var data = new ContextBase(_OptionBuilder))
            {

                var query = data.Pedido.Include(c => c.Detalhes).AsQueryable();

                if (!string.IsNullOrEmpty(idCliente))
                {
                    query = query.Where(p => p.IdCliente == idCliente);
                }

                // Filtro por IdEnderecoCobranca
                if (!string.IsNullOrEmpty(idEnderecoCobranca))
                {
                    query = query.Where(p => p.IdEnderecoCobranca == idEnderecoCobranca);
                }

                // Filtro por IdEnderecoEntrega
                if (!string.IsNullOrEmpty(idEnderecoEntrega))
                {
                    query = query.Where(p => p.IdEnderecoEntrega == idEnderecoEntrega);
                }

                // Filtro por DataPedido (período)
                if (dataPedidoInicio.HasValue)
                {
                    query = query.Where(p => p.DataPedido >= dataPedidoInicio.Value);
                }

                if (dataPedidoFim.HasValue)
                {
                    query = query.Where(p => p.DataPedido <= dataPedidoFim.Value);
                }

                // Filtro por Valor Total
                if (valorMinimo.HasValue)
                {
                    query = query.Where(p => p.ValorTotal >= valorMinimo.Value);
                }

                if (valorMaximo.HasValue)
                {
                    query = query.Where(p => p.ValorTotal <= valorMaximo.Value);
                }

                // Retornar a lista de pedidos com os filtros aplicados
                return await query.ToListAsync();
            }
        }

        public async Task<Pedido> ObtenhaPedido(int id)
        {
            using (var data = new ContextBase(_OptionBuilder))
            {
                try
                {
                    var pedido = await data.Pedido
                        .Include(c => c.Detalhes)
                        .FirstOrDefaultAsync(x => x.Id == id);

                    if (pedido == null)
                    {
                        throw new KeyNotFoundException($"Pedido com ID '{id}' não foi encontrado.");
                    }

                    return pedido;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }
    }
}
