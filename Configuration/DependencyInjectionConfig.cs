using Microsoft.EntityFrameworkCore;
using Wsm.Aplication.Interfaces;
using Wsm.Aplication.Services;
using Wsm.Domain.Interfaces;
using Wsm.Domain.Interfaces.Repositories;
using Wsm.Infra.Core.Interfaces;
using Wsm.Infra.Core.Services;
using Wsm.Infra.Estrutura.Data;
using Wsm.Infra.Estrutura.Repositories;

namespace net_maui_api.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection ResolveDependencies(this IServiceCollection services)
        {
            // Repositórios
            services.AddScoped<IUsuarioRepository, UsuarioRepository>();
            services.AddScoped<ICategoriaRepository, CategoriaRepository>();
            services.AddScoped<IProdutoRepository, ProdutoRepository>();
            services.AddScoped<IMovimentacaoEstoqueRepository, MovimentacaoEstoqueRepository>();
            services.AddScoped<IPedidoRepository, PedidoRepository>();
            services.AddScoped<IVoucherRepository, VoucherRepository>();
            services.AddScoped<ICarrinhoRepository, CarrinhoRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Services
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ICategoriaService, CategoriaService>();
            services.AddScoped<IProdutoService, ProdutoService>();
            services.AddScoped<IMovimentacaoEstoqueService, MovimentacaoEstoqueService>();
            services.AddScoped<IPedidoService, PedidoService>();
            services.AddScoped<IVoucherService, VoucherService>();
            services.AddScoped<ICarrinhoService, CarrinhoService>();

            // Infraestrutura
            services.AddScoped<ITokenService, TokenService>();

            return services;
        }
    }
}
