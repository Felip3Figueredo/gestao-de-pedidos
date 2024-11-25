using Entities.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.InterfacesDeServicos
{
    public interface IServicoDeProduto
    {
        Task<List<Produto>> ListeProdutos(string? nomeFiltro, string? descricaoFiltro, decimal? precoMin, decimal? precoMax, int? codigoUnicoFiltro);
    }
}
