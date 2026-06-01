using AutoMapper;
using ClubeDaLeitura.WebApplication.Compartilhado.Infraestrutura.Arquivos;
using ClubeDaLeitura.WebApplication.ModuloAmigo.Aplicacao;
using ClubeDaLeitura.WebApplication.ModuloAmigo.Apresentacao;
using ClubeDaLeitura.WebApplication.ModuloAmigo.Dominio;
using ClubeDaLeitura.WebApplication.ModuloAmigo.Infraestrutura;
using ClubeDaLeitura.WebApplication.ModuloCaixa.Apresentacao;
using ClubeDaLeitura.WebApplication.ModuloCaixa.Aplicacao;
using ClubeDaLeitura.WebApplication.ModuloCaixa.Dominio;
using ClubeDaLeitura.WebApplication.ModuloCaixa.Infraestrutura;
using ClubeDaLeitura.WebApplication.ModuloEmprestimo.Aplicacao;
using ClubeDaLeitura.WebApplication.ModuloEmprestimo.Apresentacao;
using ClubeDaLeitura.WebApplication.ModuloEmprestimo.Dominio;
using ClubeDaLeitura.WebApplication.ModuloEmprestimo.Infraestrutura;
using ClubeDaLeitura.WebApplication.ModuloRevista.Aplicacao;
using ClubeDaLeitura.WebApplication.ModuloRevista.Apresentacao;
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

builder.Services.AddScoped<ServicoCaixa>();
builder.Services.AddScoped<ServicoAmigo>();
builder.Services.AddScoped<ServicoRevista>();
builder.Services.AddScoped<ServicoEmprestimo>();

builder.Services.AddAutoMapper(config =>
{
    config.AddProfile<CaixaProfile>();
    config.AddProfile<AmigoProfile>();
    config.AddProfile<RevistaProfile>();
    config.AddProfile<EmprestimoProfile>();
});

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

