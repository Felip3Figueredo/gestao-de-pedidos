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
    [Table("PedidoDetalhe")]
    public class PedidoDetalhe
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [ForeignKey("Pedido")]
        public int IdPedido { get; set; }

        [JsonIgnore]
        public virtual Pedido Pedido { get; set; } 

        [ForeignKey("Produto")]
        public string IdProduto { get; set; }

        [JsonIgnore]
        public virtual Produto Produto { get; set; } 

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "A quantidade deve ser maior que zero.")]
        public int Quantidade { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal ValorUnitario { get; set; }

        public decimal ValorTotal => Quantidade * ValorUnitario; 
    }
}
