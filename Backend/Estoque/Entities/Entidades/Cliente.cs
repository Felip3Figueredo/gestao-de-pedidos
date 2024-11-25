using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Entidades
{
    [Table("Cliente")]
    public class Cliente
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString(); 

        public string Nome { get; set; }

        public string CPF { get; set; }

        public string Email { get; set; }

        public string? Telefone { get; set; }

        public virtual ICollection<Endereco> Endereco { get; set; } = new List<Endereco>();
    }
}
