using Dominio.InterfacesDeRepositorio;
using Entities.Entidades;
using Entities.Enums;
using Infra.Configuracao;
using Infra.Repositorio.Generico;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infra.Repositorio
{
    public class RepositorioEndereco : RepositorioGenerico<Endereco>, InterfaceRepositorioEndereco
    {
        private readonly DbContextOptions<ContextBase> _OptionBuilder;

        public RepositorioEndereco()
        {
            _OptionBuilder = new DbContextOptions<ContextBase>();
        }   

    }
}
