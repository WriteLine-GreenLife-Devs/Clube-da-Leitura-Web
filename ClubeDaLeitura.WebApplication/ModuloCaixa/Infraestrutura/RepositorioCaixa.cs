using ClubeDaLeitura.WebApplication.Compartilhado.Infraestrutura.Arquivos;
using ClubeDaLeitura.WebApplication.ModuloCaixa.Dominio;

namespace ClubeDaLeitura.WebApplication.ModuloCaixa.Infraestrutura;

public class RepositorioCaixa : RepositorioBase<Caixa>, InterfaceRepositorioCaixa
{
    public RepositorioCaixa(Serializable serializable) : base(serializable) { }

    protected override List<Caixa> CarregarArquivos()
    {
        return serializable.Caixas;
    }
}

