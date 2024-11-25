using Dominio.InterfacesDeRepositorio;
using Dominio.InterfacesDeServicos;
using Entities.Entidades;
using Entities.Validadores;
using GestaoDePedidos.Models;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using Swashbuckle.AspNetCore.Annotations;

namespace GestaoDePedidos.Controllers
{
    [Route("api/[controller]")]
    public class PedidoController : ControllerBase
    {
        

        private readonly InterfaceRepositorioPedido _interfaceRepositorioPedido;
        private readonly InterfaceRepositorioProduto _interfaceRepositorioProduto;
        private readonly InterfaceRepositorioEndereco _interfaceRepositorioEndereco;
        private readonly InterfaceRepositorioCliente _interfaceRepositorioCliente;
        private readonly IServicoDePedidoDetalhe _servicoPedidoDetalhe;
        private readonly IServicoDePedido _servicoDePedido;
        private readonly PedidoValidator _pedidoValidator;

        public PedidoController(
            InterfaceRepositorioPedido interfacePedido,
            InterfaceRepositorioCliente interfaceCliente,
            InterfaceRepositorioEndereco interfaceEndereco,
            InterfaceRepositorioEndereco interfaceRepositorioEndereco,
            InterfaceRepositorioProduto interfaceRepositorioProduto,
            IServicoDePedidoDetalhe servicoDePedidoDetalhe,
            IServicoDePedido servicoDePedido,
            PedidoValidator pedidoValidator)
        {
            _interfaceRepositorioPedido = interfacePedido;
            _interfaceRepositorioCliente = interfaceCliente;
            _interfaceRepositorioEndereco = interfaceEndereco;
            _interfaceRepositorioProduto = interfaceRepositorioProduto;
            _servicoPedidoDetalhe = servicoDePedidoDetalhe;
            _servicoDePedido = servicoDePedido;
            _pedidoValidator = pedidoValidator;

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        }

        [HttpPost("AdicionarPedido")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Adiciona um novo pedido", Description = "Este endpoint permite adicionar um novo pedido ao sistema.")]
        [SwaggerResponse(200, "Pedido adicionado com sucesso", typeof(string))]
        [SwaggerResponse(400, "Erro ao adicionar pedido", typeof(string))]
        public async Task<IActionResult> AdicionarPedido([FromBody] PedidoModel pedidoModel)
        {
            try
            {
                pedidoModel.Detalhes = _servicoPedidoDetalhe.DefinaValorUnitario(pedidoModel.Detalhes.ToList());

                var pedido = new Pedido
                {
                    Detalhes = pedidoModel.Detalhes,
                    IdEnderecoCobranca = pedidoModel.IdEnderecoCobranca,
                    IdEnderecoEntrega = pedidoModel.IdEnderecoEntrega,
                    IdCliente = pedidoModel.IdCliente,
                    ValorTotal = pedidoModel.Detalhes.Sum(x => x.ValorTotal)
                };

                pedidoModel.Detalhes = _servicoPedidoDetalhe.DefinaIdentificadorPedido(pedidoModel.Detalhes.ToList(), pedido.Id);

                var validationResult = await _pedidoValidator.ValidateAsync(pedido);
                if (!validationResult.IsValid)
                {
                    return BadRequest(validationResult.Errors);
                }

                await _interfaceRepositorioPedido.Add(pedido);
                return Ok("Pedido adicionado: " + pedido.Id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        
        }

        [Produces("application/json")]
        [HttpPost("ExportarPedidosExcel")]
        [SwaggerOperation(Summary = "Exporta o Pedido filtrado do Frontend", Description = "Este endpoint permite exportar um arquivo em excel.")]
        [SwaggerResponse(200, "Pedido adicionado com sucesso", typeof(byte[]))]
        [SwaggerResponse(400, "Erro ao exportar arquivo.", typeof(string))]
        public IActionResult ExportarPedidosExcel([FromBody] List<Pedido> pedidos)
        {
            // Criar um pacote Excel
            using (var package = new ExcelPackage())
            {
                // Criar uma planilha
                var worksheet = package.Workbook.Worksheets.Add("Pedidos");

                // Adicionar cabeçalhos
                worksheet.Cells[1, 1].Value = "Cliente";
                worksheet.Cells[1, 2].Value = "Produtos";
                worksheet.Cells[1, 3].Value = "Endereço de entrega";
                worksheet.Cells[1, 4].Value = "Endereço de Cobrança";
                worksheet.Cells[1, 5].Value = "Valor Total";

                // Preencher dados dos pedidos
                for (int i = 0; i < pedidos.Count; i++)
                {
                    var pedido = pedidos[i];
                    var mensagem = string.Empty;

                    var listaDeProdutos = _interfaceRepositorioProduto.List();
                    var cliente = _interfaceRepositorioCliente.GetEntityById(pedido.IdCliente);
                    var enderecoDeEntrega = _interfaceRepositorioEndereco.GetEntityById(pedido.IdEnderecoEntrega).Result;
                    var enderecoDeCobranca = _interfaceRepositorioEndereco.GetEntityById(pedido.IdEnderecoCobranca).Result;

                    var listaDeDetalhes = pedido.Detalhes;

                    foreach (var detalhe in listaDeDetalhes)
                    {
                        var produto = listaDeProdutos.Result.FirstOrDefault(x => x.Id == detalhe.IdProduto);
                        mensagem += $"* {produto.Nome} (X{detalhe.Quantidade})\n";
                    }

                    worksheet.Cells[i + 2, 1].Value = cliente.Result.Nome;
                    worksheet.Cells[i + 2, 2].Value = mensagem;
                    worksheet.Cells[i + 2, 3].Value = $"{ enderecoDeEntrega.Logradouro } - { enderecoDeEntrega.Cidade }";
                    worksheet.Cells[i + 2, 4].Value = $"{enderecoDeCobranca.Logradouro} - {enderecoDeCobranca.Cidade}";
                    worksheet.Cells[i + 2, 5].Value = pedido.ValorTotal;
                }

                // Converter o pacote para um byte array
                var fileBytes = package.GetAsByteArray();

                // Retornar o arquivo como resposta
                return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "PedidosFiltrados.xlsx");
            }
        }

        [HttpGet("ListarPedidosCadastrados")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Lista todos os pedidos cadastrados", Description = "Retorna todos os pedidos registrados no sistema.")]
        [SwaggerResponse(200, "Pedidos encontrados", typeof(IEnumerable<Pedido>))]
        [SwaggerResponse(400, "Erro ao listar pedidos", typeof(string))]
        public async Task<object> ListarPedidos(
            [FromQuery] string? idCliente,
            [FromQuery] string? idEnderecoCobranca,
            [FromQuery] string? idEnderecoEntrega,
            [FromQuery] DateTime? dataPedidoInicio,
            [FromQuery] DateTime? dataPedidoFim,
            [FromQuery] decimal? valorMinimo,
            [FromQuery] decimal? valorMaximo)
        {
            try
            {
                var listaDePedidos = await _servicoDePedido.ListarPedidos(idCliente, idEnderecoCobranca, idEnderecoEntrega, dataPedidoInicio, dataPedidoFim, valorMinimo, valorMaximo);

                return Ok(listaDePedidos);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("ObterPedido/{id}")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Obtém um pedido específico", Description = "Este endpoint retorna os detalhes de um pedido com base no seu ID.")]
        [SwaggerResponse(200, "Pedido encontrado", typeof(Pedido))]
        [SwaggerResponse(404, "Pedido não encontrado", typeof(string))]
        public async Task<IActionResult> ObterPedido(int id)
        {
            var pedido = await _servicoDePedido.ObtenhaPedido(id);
            if (pedido == null)
            {
                return NotFound("Pedido não encontrado.");
            }
            return Ok(pedido);
        }

        [HttpPut("AtualizarPedido/{id}")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Atualiza um pedido existente", Description = "Este endpoint atualiza as informações de um pedido com base no ID.")]
        [SwaggerResponse(200, "Pedido atualizado", typeof(string))]
        [SwaggerResponse(400, "Erro ao atualizar pedido", typeof(string))]
        [SwaggerResponse(404, "Pedido não encontrado", typeof(string))]
        public async Task<IActionResult> AtualizarPedido(string id, [FromBody] Pedido pedidoUpdate)
        {
            try
            {
                var pedido = await _interfaceRepositorioPedido.GetEntityById(id);
                if (pedido == null || string.IsNullOrEmpty(id))
                {
                    return NotFound("Pedido não cadastrado");
                }

                if (_interfaceRepositorioCliente.GetEntityById(pedidoUpdate.IdCliente) == null)
                {
                    return BadRequest("Cliente não encontrado.");
                }

                if (_interfaceRepositorioEndereco.GetEntityById(pedidoUpdate.IdEnderecoCobranca) == null)
                {
                    return NotFound("Endereço de cobrança não encontrado.");
                }

                if (_interfaceRepositorioEndereco.GetEntityById(pedidoUpdate.IdEnderecoEntrega) == null)
                {
                    return NotFound("Endereço de entrega não encontrado.");
                }

                await _interfaceRepositorioPedido.Update(pedido);
                return Ok("Pedido atualizado.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("DeletarPedido/{id}")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Deleta um pedido", Description = "Este endpoint permite a exclusão de um pedido com base no seu ID.")]
        [SwaggerResponse(200, "Pedido deletado com sucesso", typeof(string))]
        [SwaggerResponse(404, "Pedido não encontrado", typeof(string))]
        public async Task<IActionResult> DeletarPedido(string id)
        {
            try
            {
                var pedido = await _interfaceRepositorioPedido.GetEntityById(id);
                if (pedido == null)
                {
                    return NotFound("Pedido não encontrado.");
                }

                await _interfaceRepositorioPedido.Delete(pedido);
                return Ok("Pedido deletado");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
