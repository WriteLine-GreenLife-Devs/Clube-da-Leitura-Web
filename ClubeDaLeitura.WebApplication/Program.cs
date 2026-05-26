using ClubeDaLeitura.WebApplication.Compartilhado.Infraestrutura.Arquivos;
using ClubeDaLeitura.WebApplication.ModuloAmigo.Dominio;
using ClubeDaLeitura.WebApplication.ModuloAmigo.Infraestrutura;
using ClubeDaLeitura.WebApplication.ModuloCaixa.Dominio;
using ClubeDaLeitura.WebApplication.ModuloCaixa.Infraestrutura;

var builder = WebApplication.CreateBuilder(args);

// Configuração de Serviços
builder.Services.AddScoped(provider =>
{
    Serializable serializable = new Serializable();

    serializable.Carregar();

    return serializable;
});

#region Scopes dos Repositórios
builder.Services.AddScoped<InterfaceRepositorioCaixa, RepositorioCaixa>();
builder.Services.AddScoped<InterfaceRepositorioAmigo, RepositorioAmigo>();
#endregion

builder.Services.AddControllersWithViews().AddRazorOptions(options =>
{
    options.ViewLocationFormats.Clear();

    options.ViewLocationFormats.Add("/Modulo{1}/Apresentacao/Views/{0}.cshtml");

    options.ViewLocationFormats.Add("/Compartilhado/Apresentacao/Views/{0}.cshtml");
});

var app = builder.Build();

app.UseStaticFiles();

app.UseRouting();
app.MapDefaultControllerRoute();

app.Run();
