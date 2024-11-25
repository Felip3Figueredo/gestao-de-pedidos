using Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Entities.Entidades
{
    [Table("Endereco")]
    public class Endereco
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public EnumTipoEndereco TipoEndereco { get; set; }

        public string Logradouro { get; set; }

        public int Numero { get; set; }

        public string Complemento { get; set; }

        public string Bairro { get; set; }

        public string Cidade { get; set; }

        public string Estado { get; set; }

        public string CEP { get; set; }

        public string IdCliente { get; set; }

        [JsonIgnore]
        public virtual Cliente Cliente { get; set; }
    }
}
