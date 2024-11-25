using Dominio.InterfacesDeRepositorio;
using Dominio.InterfacesDeServicos;
using Entities.Entidades;
using Entities.Validadores;
using GestaoDePedidos.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Cryptography.X509Certificates;

namespace GestaoDePedidos.Controllers
{
    [Route("api/[controller]")]
    public class ClienteController : Controller
    {
        private readonly InterfaceRepositorioCliente _interfaceRepositorioCliente;
        private readonly IServicoDeCliente _servicoDeCliente;
        private readonly ClienteValidator _clienteValidator;

        public ClienteController(InterfaceRepositorioCliente interfaceCliente, IServicoDeCliente servicoDeCliente, ClienteValidator clienteValidator)
        {
            _interfaceRepositorioCliente = interfaceCliente;
            _servicoDeCliente = servicoDeCliente;
            _clienteValidator = clienteValidator;
        }

        [HttpPost("/api/AdicionarCliente")]
        [Produces("application/json")]
        [SwaggerOperation(
            Summary = "Adiciona um novo cliente",
            Description = "Cria um novo cliente e salva seus dados no banco."
        )]
        [SwaggerResponse(200, "Cliente adicionado com sucesso.", typeof(string))]
        [SwaggerResponse(400, "Erro ao adicionar o cliente.")]
        public async Task<IActionResult> AdicionarCliente([FromBody] ClienteModel clienteModel)
        {
            try
            {
                var cliente = new Cliente
                {
                    CPF = clienteModel.CPF,
                    Email = clienteModel.Email,
                    Nome = clienteModel.Nome,
                    Telefone = clienteModel.Telefone,
                    Endereco = clienteModel.Endereco
                };

                foreach (var endereco in cliente.Endereco)
                {
                    endereco.IdCliente = cliente.Id; 
                }

                var validationResult = await _clienteValidator.ValidateAsync(cliente);
                if (!validationResult.IsValid)
                {
                    return BadRequest(validationResult.Errors);
                }

                var CPFExiste = await _servicoDeCliente.ListarClientes(null, cliente.CPF, null, null);
                if (CPFExiste.FirstOrDefault() != null)
                {
                    return BadRequest("CPF já esta cadastrado.");
                }

                var emailExiste = await _servicoDeCliente.ListarClientes(null, null, cliente.Email, null);
                if (emailExiste.FirstOrDefault() != null)
                {
                    return BadRequest("E-mail já esta cadastrado.");
                }

                await _interfaceRepositorioCliente.Add(cliente);
                return Ok($"Pessoa adicionada: {cliente.Nome}");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("/api/ListarClientesCadastrados")]
        [Produces("application/json")]
        [SwaggerOperation(
            Summary = "Lista todos os clientes cadastrados",
            Description = "Retorna uma lista de clientes existentes no sistema."
        )]
        [SwaggerResponse(200, "Lista de clientes retornada com sucesso.", typeof(IEnumerable<Cliente>))]
        [SwaggerResponse(400, "Erro ao listar os clientes.")]
        public async Task<IActionResult> ListarClientes(
            [FromQuery] string? nome = null,
            [FromQuery] string? cpf = null,
            [FromQuery] string? email = null,
            [FromQuery] string? telefone = null)
        {
            try
            {
                var lista = await _servicoDeCliente.ListarClientes(nome, cpf, email, telefone);

                return Ok(lista);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("/api/ObterCliente/{id}")]
        [Produces("application/json")]
        [SwaggerOperation(
            Summary = "Obtém um cliente pelo ID",
            Description = "Retorna os dados de um cliente específico pelo seu identificador."
        )]
        [SwaggerResponse(200, "Dados do cliente retornados com sucesso.", typeof(Cliente))]
        [SwaggerResponse(404, "Cliente não encontrado.")]
        public async Task<IActionResult> ObterCliente([SwaggerParameter(Description = "ID do cliente a ser consultado")] string id)
        {
            var cliente = await _servicoDeCliente.ObtenhaCliente(id);
            if (cliente == null)
            {
                return NotFound("Cliente não encontrado.");
            }
            return Ok(cliente);
        }

        [HttpPut("/api/AtualizarCliente/{id}")]
        [Produces("application/json")]
        [SwaggerOperation(
            Summary = "Atualiza os dados de um cliente",
            Description = "Permite atualizar os dados do cliente com base no ID fornecido."
        )]
        [SwaggerResponse(200, "Cadastro atualizado com sucesso.", typeof(string))]
        [SwaggerResponse(400, "Erro ao atualizar o cliente.")]
        [SwaggerResponse(404, "Cliente não encontrado.")]
        public async Task<IActionResult> AtualizarCliente(string id, [FromBody] Cliente clienteUpdate)
        {
            try
            {
                var cliente = await _servicoDeCliente.ObtenhaCliente(id);

                if (cliente == null || string.IsNullOrEmpty(id))
                {
                    return NotFound("Usuário não cadastrado");
                }

                cliente.Nome = clienteUpdate.Nome;
                cliente.Email = clienteUpdate.Email;
                cliente.Telefone = clienteUpdate.Telefone;

                await _interfaceRepositorioCliente.Update(cliente);
                return Ok("Cadastro atualizado.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("/api/DeletarCliente/{id}")]
        [Produces("application/json")]
        [SwaggerOperation(
            Summary = "Deleta um cliente",
            Description = "Exclui um cliente do sistema com base no ID fornecido."
        )]
        [SwaggerResponse(200, "Cliente deletado com sucesso.", typeof(string))]
        [SwaggerResponse(400, "Erro ao deletar o cliente.")]
        public async Task<IActionResult> DeletarCliente(string id)
        {
            try
            {
                var cliente = await _interfaceRepositorioCliente.GetEntityById(id);

                if (cliente == null)
                {
                    return NotFound("Cliente não encontrado.");
                }

                await _interfaceRepositorioCliente.Delete(cliente);
                return Ok("Cliente deletado");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
