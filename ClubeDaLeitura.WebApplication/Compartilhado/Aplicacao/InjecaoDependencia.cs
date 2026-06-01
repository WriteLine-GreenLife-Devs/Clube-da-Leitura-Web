using ClubeDaLeitura.WebApplication.ModuloCaixa.Aplicacao;

namespace ClubeDaLeitura.WebApplication.Compartilhado.Aplicacao;

public static class InjecaoDependencia
{
    public static void AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<ServicoCaixa>();
    }
}
