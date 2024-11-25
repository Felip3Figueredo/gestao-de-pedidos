using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace Entities.Entidades
{
    [Table("Pedido")]
    public class Pedido
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey("Cliente")]
        public string IdCliente { get; set; }

        [JsonIgnore]
        public virtual Cliente Cliente { get; set; } 

        [ForeignKey("EnderecoEntrega")]
        public string IdEnderecoEntrega { get; set; }

        [JsonIgnore]
        public virtual Endereco EnderecoEntrega { get; set; } 

        [ForeignKey("EnderecoCobranca")]
        public string IdEnderecoCobranca { get; set; }

        [JsonIgnore]
        public virtual Endereco EnderecoCobranca { get; set; }

        public DateTime DataPedido { get; set; } = DateTime.UtcNow.ToLocalTime();

        public virtual ICollection<PedidoDetalhe> Detalhes { get; set; } = new List<PedidoDetalhe>();

        public decimal ValorTotal { get; set; }   
    }

}
