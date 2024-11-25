using Entities.Entidades;
using Entities.Enums;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace GestaoDePedidos.Models
{
    public class EnderecoModel
    {
        public EnumTipoEndereco TipoEndereco { get; set; }

        public string Logradouro { get; set; }

        public int Numero { get; set; }

        public string Complemento { get; set; }

        public string Bairro { get; set; }

        public string Cidade { get; set; }

        public string Estado { get; set; }

        public string CEP { get; set; }

        public string IdCliente { get; set; }     
    }
}
