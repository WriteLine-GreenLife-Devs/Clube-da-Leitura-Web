using ClubeDaLeitura.WebApplication.Compartilhado.Infraestrutura.Arquivos;
using ClubeDaLeitura.WebApplication.ModuloAmigo.Dominio;

namespace ClubeDaLeitura.WebApplication.ModuloAmigo.Infraestrutura;

public class RepositorioAmigo : RepositorioBase<Amigo>, InterfaceRepositorioAmigo
{
    public RepositorioAmigo(Serializable serializable) : base(serializable) { }

    protected override List<Amigo> CarregarArquivos()
    {
        return serializable.Amigos;
    }
}

