using Entities.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.InterfacesDeServicos
{
    public interface IServicoDePedidoDetalhe
    {
        Task<List<PedidoDetalhe>> ListarPedidosDetalhe(int? idPedido, string? idProduto, int? quantidade, decimal? valorUnitario);

        Task<PedidoDetalhe> ObtenhaPedidoDetalhe(string id);

        List<PedidoDetalhe> DefinaValorUnitario(List<PedidoDetalhe> listaDeDetalhes);

        List<PedidoDetalhe> DefinaIdentificadorPedido(List<PedidoDetalhe> listaDeDetalhes, int idPedido);
    }
}
