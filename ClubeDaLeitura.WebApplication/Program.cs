using ClubeDaLeitura.WebApplication.Compartilhado.Infraestrutura.Arquivos;
using ClubeDaLeitura.WebApplication.ModuloAmigo.Dominio;
using ClubeDaLeitura.WebApplication.ModuloAmigo.Infraestrutura;
using ClubeDaLeitura.WebApplication.ModuloCaixa.Dominio;
using ClubeDaLeitura.WebApplication.ModuloCaixa.Infraestrutura;
using ClubeDaLeitura.WebApplication.ModuloEmprestimo.Dominio;
using ClubeDaLeitura.WebApplication.ModuloEmprestimo.Infraestrutura;
using ClubeDaLeitura.WebApplication.ModuloRevista.Dominio;
using ClubeDaLeitura.WebApplication.ModuloRevista.Infraestrutura;

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
builder.Services.AddScoped<InterfaceRepositorioRevista, RepositorioRevista>();
builder.Services.AddScoped<InterfaceRepositorioEmprestimo, RepositorioEmprestimo>();
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
