using Entities.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.InterfacesDeServicos
{
    public interface IServicoDeEndereco
    {
        Task<IEnumerable<Endereco>> ListarEnderecosComFiltros(string? logradouro, string? cep, string? bairro, string? cidade, string? estado,
        string? complemento, string? tipoEndereco, string? numero);
    }
}
