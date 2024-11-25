﻿using Dominio.InterfacesDeRepositorio;
using Entities.Entidades;
using Infra.Configuracao;
using Infra.Repositorio.Generico;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infra.Repositorio
{
    public class RepositorioPedidoDetalhe : RepositorioGenerico<PedidoDetalhe>, InterfaceRepositorioPedidoDetalhe
    {
        private readonly DbContextOptions<ContextBase> _OptionBuilder;

        public RepositorioPedidoDetalhe()
        {
            _OptionBuilder = new DbContextOptions<ContextBase>();
        }
    }
}