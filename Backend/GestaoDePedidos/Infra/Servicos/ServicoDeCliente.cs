using Dominio.InterfacesDeServicos;
using Entities.Entidades;
using Infra.Configuracao;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class ServicoDeCliente : IServicoDeCliente
    {
        private readonly DbContextOptions<ContextBase> _OptionBuilder;

        public ServicoDeCliente()
        {
            _OptionBuilder = new DbContextOptions<ContextBase>();
        }

        public async Task<List<Cliente>> ListarClientes(string? nome, string? cpf, string? email, string? telefone)
        {
            using (var data = new ContextBase(_OptionBuilder))
            {
                var query = data.Cliente.Include(c => c.Endereco).AsQueryable();

                if (!string.IsNullOrWhiteSpace(nome))
                    query = query.Where(c => c.Nome.Contains(nome));

                if (!string.IsNullOrWhiteSpace(cpf))
                    query = query.Where(c => c.CPF == cpf);

                if (!string.IsNullOrWhiteSpace(email))
                    query = query.Where(c => c.Email.Contains(email));

                if (!string.IsNullOrWhiteSpace(telefone))
                    query = query.Where(c => c.Telefone.Contains(telefone));

                return await query.ToListAsync();
            }
        }

        public async Task<Cliente> ObtenhaCliente(string id)
        {
            using (var data = new ContextBase(_OptionBuilder))
            {
                try
                {
                    var cliente = await data.Cliente
                        .Include(c => c.Endereco)
                        .FirstOrDefaultAsync(x => x.Id == id);

                    if (cliente == null)
                    {
                        throw new KeyNotFoundException($"Endereco com ID '{id}' não foi encontrado.");
                    }

                    return cliente;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }
    }
}
