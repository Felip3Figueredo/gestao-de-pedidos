﻿// <auto-generated />
using System;
using Infra.Configuracao;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infra.Migrations
{
    [DbContext(typeof(ContextBase))]
    [Migration("20241125155529_init")]
    partial class init
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Entities.Entidades.Cliente", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("CPF")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Telefone")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Cliente");
                });

            modelBuilder.Entity("Entities.Entidades.Endereco", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("Bairro")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("CEP")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Cidade")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Complemento")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Estado")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("IdCliente")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Logradouro")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Numero")
                        .HasColumnType("integer");

                    b.Property<int>("TipoEndereco")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("IdCliente");

                    b.ToTable("Endereco");
                });

            modelBuilder.Entity("Entities.Entidades.Pedido", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("DataPedido")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("IdCliente")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("IdEnderecoCobranca")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("IdEnderecoEntrega")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<decimal>("ValorTotal")
                        .HasColumnType("numeric");

                    b.HasKey("Id");

                    b.HasIndex("IdCliente");

                    b.HasIndex("IdEnderecoCobranca");

                    b.HasIndex("IdEnderecoEntrega");

                    b.ToTable("Pedido");
                });

            modelBuilder.Entity("Entities.Entidades.PedidoDetalhe", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<int>("IdPedido")
                        .HasColumnType("integer");

                    b.Property<string>("IdProduto")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Quantidade")
                        .HasColumnType("integer");

                    b.Property<decimal>("ValorUnitario")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.HasIndex("IdPedido");

                    b.HasIndex("IdProduto");

                    b.ToTable("PedidoDetalhe");
                });

            modelBuilder.Entity("Entities.Entidades.Produto", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("Descricao")
                        .HasColumnType("text");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<decimal>("Preco")
                        .HasColumnType("numeric");

                    b.HasKey("Id");

                    b.ToTable("Produto");
                });

            modelBuilder.Entity("Entities.Entidades.Endereco", b =>
                {
                    b.HasOne("Entities.Entidades.Cliente", "Cliente")
                        .WithMany("Endereco")
                        .HasForeignKey("IdCliente")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Cliente");
                });

            modelBuilder.Entity("Entities.Entidades.Pedido", b =>
                {
                    b.HasOne("Entities.Entidades.Cliente", "Cliente")
                        .WithMany()
                        .HasForeignKey("IdCliente")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Entities.Entidades.Endereco", "EnderecoCobranca")
                        .WithMany()
                        .HasForeignKey("IdEnderecoCobranca")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Entities.Entidades.Endereco", "EnderecoEntrega")
                        .WithMany()
                        .HasForeignKey("IdEnderecoEntrega")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Cliente");

                    b.Navigation("EnderecoCobranca");

                    b.Navigation("EnderecoEntrega");
                });

            modelBuilder.Entity("Entities.Entidades.PedidoDetalhe", b =>
                {
                    b.HasOne("Entities.Entidades.Pedido", "Pedido")
                        .WithMany("Detalhes")
                        .HasForeignKey("IdPedido")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Entities.Entidades.Produto", "Produto")
                        .WithMany()
                        .HasForeignKey("IdProduto")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Pedido");

                    b.Navigation("Produto");
                });

            modelBuilder.Entity("Entities.Entidades.Cliente", b =>
                {
                    b.Navigation("Endereco");
                });

            modelBuilder.Entity("Entities.Entidades.Pedido", b =>
                {
                    b.Navigation("Detalhes");
                });
#pragma warning restore 612, 618
        }
    }
}
