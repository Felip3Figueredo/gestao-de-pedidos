using Dominio.InterfacesDeRepositorio;
using Dominio.InterfacesDeServicos;
using Entities.Entidades;
using Entities.Validadores;
using GestaoDePedidos.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace GestaoDePedidos.Controllers
{
    [Route("api/[controller]")]
    public class EnderecoController : Controller
    {
        private readonly InterfaceRepositorioEndereco _interfaceRepositorioEndereco;
        private readonly IServicoDeEndereco _servicoDeEndereco;
        private readonly EnderecoValidator _enderecoValidator;
        public EnderecoController(EnderecoValidator enderecoValidator, InterfaceRepositorioEndereco interfaceRepositorioEndereco, IServicoDeEndereco servicoDeEndereco)
        {
            _interfaceRepositorioEndereco = interfaceRepositorioEndereco;
            _servicoDeEndereco = servicoDeEndereco;
            _enderecoValidator = enderecoValidator;
        }

        // Endpoint para adicionar um endereço
        [HttpPost("/api/AdicionarEndereco")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Adicionar um novo endereço",
                          Description = "Este endpoint adiciona um novo endereço no sistema.")]
        [SwaggerResponse(200, "Endereço adicionado com sucesso", typeof(string))]
        [SwaggerResponse(400, "Erro ao adicionar o endereço", typeof(string))]
        public async Task<IActionResult> AdicionarEndereco([FromBody] EnderecoModel enderecoModel)
        {
            try
            {
                var endereco = new Endereco
                {
                    Complemento = enderecoModel.Complemento,
                    Estado = enderecoModel.Estado,
                    CEP = enderecoModel.CEP,
                    Bairro = enderecoModel.Bairro,
                    Cidade = enderecoModel.Cidade,
                    IdCliente = enderecoModel.IdCliente,
                    Logradouro = enderecoModel.Logradouro,
                    Numero = enderecoModel.Numero,
                    TipoEndereco = enderecoModel.TipoEndereco
                };

                var validationResult = await _enderecoValidator.ValidateAsync(endereco);
                if (!validationResult.IsValid)
                {
                    return BadRequest(validationResult.Errors);
                }

                await _interfaceRepositorioEndereco.Add(endereco);
                return Ok("Endereço adicionado: " + endereco.CEP);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Endpoint para listar endereços com filtros opcionais
        [HttpGet("/api/ListarEnderecosCadastrados")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Listar endereços cadastrados",
                          Description = "Este endpoint permite listar os endereços cadastrados, com filtros opcionais.")]
        [SwaggerResponse(200, "Lista de endereços", typeof(IEnumerable<Endereco>))]
        [SwaggerResponse(400, "Erro ao listar endereços", typeof(string))]
        public async Task<IActionResult> ListarEnderecos(
            [FromQuery] string? logradouro = null,   
            [FromQuery] string? cep = null,          
            [FromQuery] string? bairro = null,       
            [FromQuery] string? cidade = null,       
            [FromQuery] string? estado = null,       
            [FromQuery] string? complemento = null,  
            [FromQuery] string? tipoEndereco = null, 
            [FromQuery] string? numero = null        
        )
        {
            try
            {
                var lista = await _servicoDeEndereco.ListarEnderecosComFiltros(logradouro, cep, bairro, cidade, estado, complemento, tipoEndereco, numero);
                return Ok(lista);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Endpoint para obter um endereço por ID
        [HttpGet("/api/ObterEndereco/{id}")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Obter um endereço por ID",
                          Description = "Este endpoint retorna um endereço específico com base no ID fornecido.")]
        [SwaggerResponse(200, "Endereço encontrado", typeof(Endereco))]
        [SwaggerResponse(404, "Endereço não encontrado", typeof(string))]
        public async Task<IActionResult> ObterEndereco(string id)
        {
            var endereco = await _interfaceRepositorioEndereco.GetEntityById(id);
            if (endereco == null)
            {
                return NotFound("Endereço não encontrado.");
            }
            return Ok(endereco);
        }

        // Endpoint para atualizar um endereço
        [HttpPut("/api/AtualizarEndereco/{id}")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Atualizar um endereço",
                          Description = "Este endpoint atualiza os dados de um endereço existente.")]
        [SwaggerResponse(200, "Endereço atualizado com sucesso", typeof(string))]
        [SwaggerResponse(400, "Erro ao atualizar o endereço", typeof(string))]
        [SwaggerResponse(404, "Endereço não encontrado", typeof(string))]
        public async Task<IActionResult> AtualizarEndereco(string id, [FromBody] Endereco enderecoUpdate)
        {
            try
            {
                var endereco = await _interfaceRepositorioEndereco.GetEntityById(id);
                if (endereco == null)
                {
                    return NotFound("Endereço não cadastrado.");
                }

                // Atualização do endereço
                endereco.Logradouro = enderecoUpdate.Logradouro;
                endereco.CEP = enderecoUpdate.CEP;
                endereco.Cidade = enderecoUpdate.Cidade;
                endereco.Complemento = enderecoUpdate.Complemento;
                endereco.Estado = enderecoUpdate.Estado;
                endereco.TipoEndereco = enderecoUpdate.TipoEndereco;
                endereco.Numero = enderecoUpdate.Numero;
                endereco.Bairro = enderecoUpdate.Bairro;

                await _interfaceRepositorioEndereco.Update(endereco);

                return Ok("Endereço atualizado.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Endpoint para deletar um endereço
        [HttpDelete("/api/DeletarEndereco/{id}")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Deletar um endereço",
                          Description = "Este endpoint deleta um endereço existente pelo ID.")]
        [SwaggerResponse(200, "Endereço deletado com sucesso", typeof(string))]
        [SwaggerResponse(400, "Erro ao deletar o endereço", typeof(string))]
        [SwaggerResponse(404, "Endereço não encontrado", typeof(string))]
        public async Task<IActionResult> DeletarEndereco(string id)
        {
            try
            {
                var endereco = await _interfaceRepositorioEndereco.GetEntityById(id);
                if (endereco == null)
                {
                    return NotFound("Endereço não encontrado.");
                }

                await _interfaceRepositorioEndereco.Delete(endereco);
                return Ok("Endereço deletado");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}