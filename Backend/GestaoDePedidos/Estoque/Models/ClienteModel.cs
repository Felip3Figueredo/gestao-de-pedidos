using Entities.Entidades;
using System.Text.Json.Serialization;

namespace GestaoDePedidos.Models
{
    public class ClienteModel
    {
        [JsonPropertyName("nome")]
        public string Nome { get; set; }

        [JsonPropertyName("cpf")]
        public string CPF { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("telefone")]
        public string Telefone { get; set; }

        [JsonPropertyName("endereco")]
        public virtual ICollection<Endereco> Endereco { get; set; } = new List<Endereco>();
    }

}
