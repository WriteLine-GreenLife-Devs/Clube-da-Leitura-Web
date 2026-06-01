using ClubeDaLeitura.WebApplication.Compartilhado.Infraestrutura.Arquivos;
using ClubeDaLeitura.WebApplication.ModuloAmigo.Dominio;
using ClubeDaLeitura.WebApplication.ModuloAmigo.Infraestrutura;
using ClubeDaLeitura.WebApplication.ModuloCaixa.Dominio;
using ClubeDaLeitura.WebApplication.ModuloCaixa.Infraestrutura;
using ClubeDaLeitura.WebApplication.ModuloEmprestimo.Dominio;
using ClubeDaLeitura.WebApplication.ModuloEmprestimo.Infraestrutura;
using ClubeDaLeitura.WebApplication.ModuloRevista.Dominio;
using ClubeDaLeitura.WebApplication.ModuloRevista.Infraestrutura;

namespace ClubeDaLeitura.WebApplication.Compartilhado.Infraestrutura;

public static class InjecaoDependencia
{
    public static void AddInfraRepositories(this IServiceCollection services)
    {
        services.AddScoped(provider =>
        {
            Serializable serializable = new Serializable();

            serializable.Carregar();

            return serializable;
        });

        services.AddScoped<InterfaceRepositorioCaixa, RepositorioCaixa>();
        services.AddScoped<InterfaceRepositorioRevista, RepositorioRevista>();
        services.AddScoped<InterfaceRepositorioAmigo, RepositorioAmigo>();
        services.AddScoped<InterfaceRepositorioEmprestimo, RepositorioEmprestimo>();
    }
}
