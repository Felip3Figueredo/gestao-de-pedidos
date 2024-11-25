using Entities.Entidades;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestaoDePedidos.Models
{
    public class PedidoModel
    {
        public string IdCliente { get; set; }

        public string IdEnderecoEntrega { get; set; }

        public string IdEnderecoCobranca { get; set; }

        public virtual ICollection<PedidoDetalhe> Detalhes { get; set; } = new List<PedidoDetalhe>();
    }
}
