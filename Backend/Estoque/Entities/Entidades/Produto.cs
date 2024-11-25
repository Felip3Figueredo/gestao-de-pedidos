using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Entities.Entidades
{
    [Table("Produto")]
    public class Produto
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string Nome { get; set; }

        public string? Descricao { get; set; }

        public decimal Preco { get; set; }
    }
}
