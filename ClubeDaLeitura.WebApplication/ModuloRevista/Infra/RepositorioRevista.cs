using ClubeDaLeitura.WebApplication.Compartilhado.Infraestrutura.Arquivos;
using ClubeDaLeitura.WebApplication.ModuloRevista.Dominio;

namespace ClubeDaLeitura.WebApplication.ModuloRevista.Infraestrutura;

public class RepositorioRevista : RepositorioBase<Revista>
{
    public RepositorioRevista(Serializable serializable) : base(serializable)
    {
    }

    protected override List<Revista> ObterRegistros()
    {
        return serializable.Revistas;
    }
}