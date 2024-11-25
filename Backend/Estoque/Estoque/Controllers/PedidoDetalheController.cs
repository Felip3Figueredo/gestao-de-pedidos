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
    [ApiController]
    public class PedidoDetalheController : ControllerBase
    {
        private readonly InterfaceRepositorioPedidoDetalhe _interfaceRepositorioPedidoDetalhe;
        private readonly InterfaceRepositorioProduto _interfaceRepositorioProduto;
        private readonly IServicoDePedidoDetalhe _servicoDetalhePedido;
        private readonly PedidoDetalheValidator _detalhePedidoValidator;

        public PedidoDetalheController(InterfaceRepositorioPedidoDetalhe interfacePedidoDetalhe,
            InterfaceRepositorioProduto interfaceRepositorioProduto,
            IServicoDePedidoDetalhe servicoDePedidoDetalhe,
            PedidoDetalheValidator detalhePedidoValidator)
        {
            _interfaceRepositorioPedidoDetalhe = interfacePedidoDetalhe;
            _interfaceRepositorioProduto = interfaceRepositorioProduto;
            _servicoDetalhePedido = servicoDePedidoDetalhe;
            _detalhePedidoValidator = detalhePedidoValidator;
        }

        /// <summary>
        /// Adiciona um novo detalhe de pedido.
        /// </summary>
        /// <param name="pedidoDetalheModel">Objeto contendo as informações do detalhe do pedido a ser adicionado.</param>
        /// <returns>Mensagem indicando o sucesso ou falha da operação.</returns>
        [HttpPost("AdicionarPedidoDetalhe")]
        [Produces("application/json")]
        [SwaggerResponse(200, "PedidoDetalhe adicionado com sucesso.", typeof(string))]
        [SwaggerResponse(400, "Erro ao adicionar PedidoDetalhe.", typeof(string))]
        [SwaggerOperation(Summary = "Adicionar um novo detalhe de pedido", Description = "Adiciona um novo detalhe de pedido ao sistema.")]
        public async Task<IActionResult> AdicionarPedidoDetalhe([FromBody] PedidoDetalheModel pedidoDetalheModel)
        {
            try
            {
                var valorProduto = _interfaceRepositorioProduto.GetEntityById(pedidoDetalheModel.IdProduto).Result;
                if (valorProduto == null)
                    return BadRequest("Insira um produto cadastrado.");

                var pedidoDetalhe = new PedidoDetalhe
                {
                    IdPedido = pedidoDetalheModel.IdPedido,
                    IdProduto = pedidoDetalheModel.IdProduto,
                    Quantidade = pedidoDetalheModel.Quantidade,
                    ValorUnitario = valorProduto.Preco
                };

                var validationResult = await _detalhePedidoValidator.ValidateAsync(pedidoDetalhe);
                if (!validationResult.IsValid)
                {
                    return BadRequest(validationResult.Errors);
                }

                await _interfaceRepositorioPedidoDetalhe.Add(pedidoDetalhe);

                return Ok($"PedidoDetalhe adicionado: {pedidoDetalhe.Id}");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Lista todos os detalhes de pedidos cadastrados.
        /// </summary>
        /// <param name="statusPedido">Parâmetro opcional para filtrar os pedidos por status.</param>
        /// <returns>Lista de detalhes de pedidos.</returns>
        [HttpGet("ListarPedidoDetalhesCadastrados")]
        [Produces("application/json")]
        [SwaggerResponse(200, "Lista de detalhes de pedidos", typeof(IEnumerable<PedidoDetalhe>))]
        [SwaggerResponse(400, "Erro ao listar detalhes de pedidos.", typeof(string))]
        [SwaggerOperation(Summary = "Listar todos os detalhes de pedidos", Description = "Recupera todos os detalhes de pedidos cadastrados no sistema.")]
        public async Task<IActionResult> ListarPedidoDetalhes(
            [FromQuery] int? idPedido,
            [FromQuery] string? idProduto,
            [FromQuery] int? quantidade,
            [FromQuery] decimal? valorUnitario)
        {
            try
            {
                var lista = await _servicoDetalhePedido.ListarPedidosDetalhe(idPedido, idProduto, quantidade, valorUnitario);

                return Ok(lista);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Obtém os detalhes de um pedido específico.
        /// </summary>
        /// <param name="id">ID do pedido detalhado a ser consultado.</param>
        /// <returns>Detalhes do pedido.</returns>
        [HttpGet("ObterPedidoDetalhe/{id}")]
        [Produces("application/json")]
        [SwaggerResponse(200, "Detalhe do pedido obtido com sucesso.", typeof(PedidoDetalhe))]
        [SwaggerResponse(404, "PedidoDetalhe não encontrado.", typeof(string))]
        [SwaggerOperation(Summary = "Obter detalhes de um pedido", Description = "Recupera os detalhes de um pedido específico, utilizando o ID.")]
        public async Task<IActionResult> ObterPedidoDetalhe(string id)
        {
            var pedidoDetalhe = await _servicoDetalhePedido.ObtenhaPedidoDetalhe(id);
            if (pedidoDetalhe == null)
            {
                return NotFound("PedidoDetalhe não encontrado.");
            }

            return Ok(pedidoDetalhe);
        }

        /// <summary>
        /// Atualiza os detalhes de um pedido específico.
        /// </summary>
        /// <param name="id">ID do pedido detalhado a ser atualizado.</param>
        /// <param name="pedidoDetalheUpdate">Objeto com as novas informações do pedido detalhado.</param>
        /// <returns>Mensagem indicando o sucesso ou falha da operação.</returns>
        [HttpPut("AtualizarPedidoDetalhe/{id}")]
        [Produces("application/json")]
        [SwaggerResponse(200, "PedidoDetalhe atualizado com sucesso.", typeof(string))]
        [SwaggerResponse(400, "Erro ao atualizar PedidoDetalhe.", typeof(string))]
        [SwaggerOperation(Summary = "Atualizar os detalhes de um pedido", Description = "Atualiza os detalhes de um pedido específico utilizando seu ID.")]
        public async Task<IActionResult> AtualizarPedidoDetalhe(string id, [FromBody] PedidoDetalhe pedidoDetalheUpdate)
        {
            try
            {
                var pedidoDetalhe = await _interfaceRepositorioPedidoDetalhe.GetEntityById(id);
                if (pedidoDetalhe == null || string.IsNullOrEmpty(id))
                {
                    return BadRequest("Id vazio ou Objeto não encontrado.");
                }

                pedidoDetalhe.IdProduto = pedidoDetalheUpdate.IdProduto;
                await _interfaceRepositorioPedidoDetalhe.Update(pedidoDetalhe);

                return Ok("Detalhe do pedido atualizado.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Deleta um detalhe de pedido específico.
        /// </summary>
        /// <param name="id">ID do pedido detalhado a ser deletado.</param>
        /// <returns>Mensagem indicando o sucesso ou falha da operação.</returns>
        [HttpDelete("DeletarPedidoDetalhe/{id}")]
        [Produces("application/json")]
        [SwaggerResponse(200, "PedidoDetalhe deletado com sucesso.", typeof(string))]
        [SwaggerResponse(400, "Erro ao deletar PedidoDetalhe.", typeof(string))]
        [SwaggerOperation(Summary = "Deletar detalhe de um pedido", Description = "Deleta um detalhe de pedido específico utilizando seu ID.")]
        public async Task<IActionResult> DeletarPedidoDetalhe(string id)
        {
            try
            {
                var pedidoDetalhe = await _interfaceRepositorioPedidoDetalhe.GetEntityById(id);

                if (pedidoDetalhe == null)
                {
                    return BadRequest("PedidoDetalhe não encontrado.");
                }

                await _interfaceRepositorioPedidoDetalhe.Delete(pedidoDetalhe);

                return Ok("PedidoDetalhe deletado");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
