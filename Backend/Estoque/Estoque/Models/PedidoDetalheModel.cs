using Entities.Entidades;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GestaoDePedidos.Models
{
    public class PedidoDetalheModel
    {
        public int IdPedido { get; set; }

        public string IdProduto { get; set; }

        public int Quantidade { get; set; }
    }
}
