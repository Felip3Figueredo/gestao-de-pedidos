using Entities.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.InterfacesDeServicos
{
    public interface IServicoDeCliente
    {
        Task<List<Cliente>> ListarClientes(string? nome, string? cpf, string? email, string? telefone);

        Task<Cliente> ObtenhaCliente(string id);
    }
}
