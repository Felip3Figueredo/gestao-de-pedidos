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
    public class ProdutoController : ControllerBase
    {
        private readonly InterfaceRepositorioProduto _interfaceRepositorioProduto;
        private readonly IServicoDeProduto _servicoDeProduto;
        ProdutoValidator _produtoValidator;

        public ProdutoController(InterfaceRepositorioProduto interfaceProduto, IServicoDeProduto servicoDeProduto, ProdutoValidator produtoValidator)
        {
            _interfaceRepositorioProduto = interfaceProduto;
            _servicoDeProduto = servicoDeProduto;
            _produtoValidator = produtoValidator;
        }

        /// <summary>
        /// Adiciona um novo produto ao estoque.
        /// </summary>
        /// <param name="produtoModel">Modelo do produto a ser adicionado.</param>
        /// <returns>Mensagem de sucesso ou erro.</returns>
        /// <response code="200">Produto adicionado com sucesso</response>
        /// <response code="400">Erro ao adicionar o produto</response>
        [HttpPost("/api/AdicionarProduto")]
        [Produces("application/json")]
        [SwaggerResponse(200, "Produto adicionado com sucesso.")]
        [SwaggerResponse(400, "Erro ao adicionar o produto.")]
        [SwaggerOperation(Summary = "Adiciona um novo produto ao estoque.", Description = "Este endpoint adiciona um novo produto ao estoque com base nos dados fornecidos.")]
        public async Task<IActionResult> AdicionarProduto([FromBody] ProdutoModel produtoModel)
        {
            try
            {
                var produto = new Produto
                {
                    Descricao = produtoModel.Descricao,
                    Nome = produtoModel.Nome,
                    Preco = produtoModel.Preco
                };

                var validationResult = await _produtoValidator.ValidateAsync(produto);
                if (!validationResult.IsValid)
                {
                    return BadRequest(validationResult.Errors);
                }

                await _interfaceRepositorioProduto.Add(produto);

                return Ok(new { message = $"Produto adicionado: {produto.Nome}" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Lista todos os produtos cadastrados.
        /// </summary>
        /// <returns>Lista de produtos.</returns>
        /// <response code="200">Lista de produtos retornada com sucesso.</response>
        [HttpGet("/api/ListarProdutosCadastrados")]
        [Produces("application/json")]
        [SwaggerResponse(200, "Lista de produtos retornada com sucesso.")]
        [SwaggerResponse(400, "Erro ao listar os produtos.")]
        [SwaggerOperation(Summary = "Lista todos os produtos cadastrados.", Description = "Este endpoint retorna todos os produtos cadastrados no estoque, com a possibilidade de aplicar filtros.")]
        public async Task<IActionResult> ListarProdutos(
            [FromQuery] string? nomeFiltro = null,
            [FromQuery] string? descricaoFiltro = null,
            [FromQuery] decimal? precoMin = null,
            [FromQuery] decimal? precoMax = null,
            [FromQuery] int? codigoUnicoFiltro = null)
        {
            try
            {
                var lista = await _servicoDeProduto.ListeProdutos(nomeFiltro, descricaoFiltro, precoMin, precoMax, codigoUnicoFiltro);
                return Ok(lista);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Obtém os detalhes de um produto específico.
        /// </summary>
        /// <param name="id">ID do produto.</param>
        /// <returns>Detalhes do produto.</returns>
        /// <response code="200">Produto encontrado.</response>
        /// <response code="404">Produto não encontrado.</response>
        [HttpGet("/api/ObterProduto/{id}")]
        [Produces("application/json")]
        [SwaggerResponse(200, "Produto encontrado.")]
        [SwaggerResponse(404, "Produto não encontrado.")]
        [SwaggerOperation(Summary = "Obtém os detalhes de um produto específico.", Description = "Este endpoint retorna os detalhes de um produto específico, com base no ID fornecido.")]
        public async Task<IActionResult> ObterProduto(int id)
        {
            var produto = await _interfaceRepositorioProduto.GetEntityById(id.ToString());

            if (produto == null)
                return NotFound(new { message = "Produto não encontrado." });

            return Ok(produto);
        }

        /// <summary>
        /// Atualiza as informações de um produto.
        /// </summary>
        /// <param name="id">ID do produto a ser atualizado.</param>
        /// <param name="produtoUpdate">Modelo com os dados atualizados.</param>
        /// <returns>Mensagem de sucesso ou erro.</returns>
        /// <response code="200">Produto atualizado com sucesso.</response>
        /// <response code="400">Erro ao atualizar o produto.</response>
        [HttpPut("/api/AtualizarProduto/{id}")]
        [Produces("application/json")]
        [SwaggerResponse(200, "Produto atualizado com sucesso.")]
        [SwaggerResponse(400, "Erro ao atualizar o produto.")]
        [SwaggerOperation(Summary = "Atualiza as informações de um produto.", Description = "Este endpoint permite a atualização das informações de um produto, como nome, descrição e preço.")]
        public async Task<IActionResult> AtualizarProduto(string id, [FromBody] Produto produtoUpdate)
        {
            try
            {
                var produto = await _interfaceRepositorioProduto.GetEntityById(id);

                if (produto == null || string.IsNullOrEmpty(id))
                {
                    return NotFound(new { message = "Produto não cadastrado." });
                }

                produto.Nome = produtoUpdate.Nome;
                produto.Descricao = produtoUpdate.Descricao;
                produto.Preco = produtoUpdate.Preco;

                await _interfaceRepositorioProduto.Update(produto);

                return Ok(new { message = "Produto atualizado." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Deleta um produto do estoque.
        /// </summary>
        /// <param name="id">ID do produto a ser deletado.</param>
        /// <returns>Mensagem de sucesso ou erro.</returns>
        /// <response code="200">Produto deletado com sucesso.</response>
        /// <response code="400">Erro ao deletar o produto.</response>
        [HttpDelete("/api/DeletarProduto/{id}")]
        [Produces("application/json")]
        [SwaggerResponse(200, "Produto deletado com sucesso.")]
        [SwaggerResponse(400, "Erro ao deletar o produto.")]
        [SwaggerOperation(Summary = "Deleta um produto do estoque.", Description = "Este endpoint permite a exclusão de um produto do estoque, identificando-o pelo seu ID.")]
        public async Task<IActionResult> DeletarProduto(string id)
        {
            try
            {
                var produto = await _interfaceRepositorioProduto.GetEntityById(id);

                if (produto == null)
                    return NotFound(new { message = "Produto não encontrado." });

                await _interfaceRepositorioProduto.Delete(produto);

                return Ok(new { message = "Produto deletado." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
