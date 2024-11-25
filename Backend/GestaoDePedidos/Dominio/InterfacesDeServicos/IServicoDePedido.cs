using Entities.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.InterfacesDeServicos
{
    public interface IServicoDePedido
    {
        Task<List<Pedido>> ListarPedidos(string? idCliente, string? idEnderecoCobranca, string? idEnderecoEntrega, DateTime? dataPedidoInicio, DateTime? dataPedidoFim, decimal? valorMinimo, decimal? valorMaximo);

        Task<Pedido> ObtenhaPedido(int id);

    }
}
