using ClubeDaLeitura.WebApplication.Compartilhado.Infraestrutura.Arquivos;
using ClubeDaLeitura.WebApplication.ModuloRevista.Dominio;

namespace ClubeDaLeitura.WebApplication.ModuloRevista.Infraestrutura;

public sealed class RepositorioRevista : RepositorioBase<Revista>, InterfaceRepositorioRevista
{
    public RepositorioRevista(Serializable serializable) : base(serializable) { }

    protected override List<Revista> CarregarArquivos()
    {
        return serializable.Revistas;
    }
}
