using ClubeDaLeitura.WebApplication.Compartilhado;
using ClubeDaLeitura.WebApplication.Compartilhado.Infraestrutura.Arquivos;
using ClubeDaLeitura.WebApplication.ModuloRevista.Dominio;
using System.Collections.Generic;

namespace ClubeDaLeitura.WebApplication.ModuloRevista.Infraestrutura;

public sealed class RepositorioRevista : RepositorioBase<Revista>
{
    public RepositorioRevista(Serializable serializable) : base(serializable) { }

    protected override List<Revista> CarregarArquivos()
    {
        return serializable.Revistas;
    }
}