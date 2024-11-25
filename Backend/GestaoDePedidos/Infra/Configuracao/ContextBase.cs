using Microsoft.EntityFrameworkCore;
using Entities.Entidades;

namespace Infra.Configuracao
{
    public class ContextBase : DbContext 
    {
        public ContextBase(DbContextOptions<ContextBase> options) : base(options) { }

        public DbSet<Cliente> Cliente { get; set; }
        public DbSet<Endereco> Endereco { get; set; }
        public DbSet<Pedido> Pedido { get; set; }
        public DbSet<PedidoDetalhe> PedidoDetalhe { get; set; }
        public DbSet<Produto> Produto { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql(
                    "Host=localhost;Port=5432;Database=Pedidos;Username=postgres;Password=Murilo1023@"
                );

                base.OnConfiguring(optionsBuilder);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            ConfigureCliente(modelBuilder);
            ConfigureEndereco(modelBuilder);
            ConfigurePedido(modelBuilder);
            ConfigurePedidoDetalhe(modelBuilder);
            ConfigureProduto(modelBuilder);
        }

        private void ConfigureCliente(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cliente>()
            .HasMany(c => c.Endereco)
            .WithOne(e => e.Cliente)
            .HasForeignKey(e => e.IdCliente)
            .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Cliente>()
                .HasMany(c => c.Endereco)
                .WithOne(e => e.Cliente)
                .HasForeignKey(e => e.IdCliente)
                .IsRequired();
        }

        private void ConfigureEndereco(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Endereco>()
                .HasOne(e => e.Cliente)
                .WithMany(c => c.Endereco)
                .HasForeignKey(e => e.IdCliente);
        }

        private void ConfigurePedido(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Pedido>()
                .Property(p => p.DataPedido)
                .HasColumnType("timestamp without time zone");

            modelBuilder.Entity<Pedido>()
                .HasMany(p => p.Detalhes)
                .WithOne(d => d.Pedido)
                .HasForeignKey(d => d.IdPedido);

            modelBuilder.Entity<Pedido>()
                .HasOne(p => p.Cliente)
                .WithMany()
                .HasForeignKey(p => p.IdCliente);

            modelBuilder.Entity<Pedido>()
                .HasOne(p => p.EnderecoEntrega)
                .WithMany()
                .HasForeignKey(p => p.IdEnderecoEntrega);

            modelBuilder.Entity<Pedido>()
                .HasOne(p => p.EnderecoCobranca)
                .WithMany()
                .HasForeignKey(p => p.IdEnderecoCobranca);
        }

        private void ConfigurePedidoDetalhe(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PedidoDetalhe>()
                .HasOne(pd => pd.Pedido)
                .WithMany(p => p.Detalhes)
                .HasForeignKey(pd => pd.IdPedido);

            modelBuilder.Entity<PedidoDetalhe>()
                .HasOne(pd => pd.Produto)
                .WithMany()
                .HasForeignKey(pd => pd.IdProduto);
        }

        private void ConfigureProduto(ModelBuilder modelBuilder)
        {
        }

    }
}
