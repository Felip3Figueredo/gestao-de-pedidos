using Dominio;
using Dominio.InterfacesDeRepositorio;
using Dominio.InterfacesDeServicos;
using Entities.Entidades;
using Entities.Validadores;
using Infra.Configuracao;
using Infra.Repositorio;
using Infra.Repositorio.Generico;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;

namespace GestaoDePedidos
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Adicionando o CORS (Cross-Origin Resource Sharing) com a política de permitir todas as origens
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigins",
                    policyBuilder => policyBuilder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
            });

            // Adicionando Controllers e configurando o uso de JsonOptions para evitar serialização de valores padrão
            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault);

            // Adicionando a exploração de endpoints e Swagger
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Pedidos API",
                    Version = "v1",
                    Description = "API para gerenciamento de pedidos.",
                    Contact = new OpenApiContact
                    {
                        Name = "Felipe Figueredo",
                        Email = "felipefigueredo1208@gmail.com",
                    }
                });
                c.EnableAnnotations();
            });

            // Configuração do contexto de banco de dados para uso do PostgreSQL
            builder.Services.AddDbContext<ContextBase>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Registro de Repositórios
            builder.Services.AddScoped(typeof(InterfaceGenerica<>), typeof(RepositorioGenerico<>));
            builder.Services.AddScoped<InterfaceRepositorioCliente, RepositorioCliente>();
            builder.Services.AddScoped<InterfaceRepositorioEndereco, RepositorioEndereco>();
            builder.Services.AddScoped<InterfaceRepositorioPedido, RepositorioPedido>();
            builder.Services.AddScoped<InterfaceRepositorioPedidoDetalhe, RepositorioPedidoDetalhe>();
            builder.Services.AddScoped<InterfaceRepositorioProduto, RepositorioProduto>();

            // Registro de Serviços
            builder.Services.AddScoped<IServicoDeCliente, ServicoDeCliente>();
            builder.Services.AddScoped<IServicoDePedido, ServicoDePedido>();
            builder.Services.AddScoped<IServicoDePedidoDetalhe, ServicoDePedidoDetalhe>();
            builder.Services.AddScoped<IServicoDeEndereco, ServicoDeEndereco>();
            builder.Services.AddScoped<IServicoDeProduto, ServicoDeProduto>();

            // Registro de Validadores
            builder.Services.AddTransient<ClienteValidator>();
            builder.Services.AddTransient<EnderecoValidator>();
            builder.Services.AddTransient<ProdutoValidator>();
            builder.Services.AddTransient<PedidoDetalheValidator>();
            builder.Services.AddTransient<PedidoValidator>();

            // Adicionando Razor Pages, caso seja necessário
            builder.Services.AddRazorPages();

            var app = builder.Build();

            // Configuração do pipeline de requisições HTTP
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Gestão de Pedidos API v1"));
            }

            // Configurando CORS
            app.UseCors("AllowAllOrigins");

            // Habilitando redirecionamento HTTPS
            app.UseHttpsRedirection();

            // Autorização de requisições
            app.UseAuthorization();

            // Mapeando os controllers
            app.MapControllers();

            // Rodando a aplicação
            app.Run();
        }
    }
}
