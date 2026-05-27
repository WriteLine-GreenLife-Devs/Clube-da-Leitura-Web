using ClubeDaLeitura.WebApplication.Compartilhado.Infraestrutura.Arquivos;
using ClubeDaLeitura.WebApplication.ModuloEmprestimo.Dominio;

namespace ClubeDaLeitura.WebApplication.ModuloEmprestimo.Infraestrutura;

public class RepositorioEmprestimo : RepositorioBase<Emprestimo>, InterfaceRepositorioEmprestimo
{
    public RepositorioEmprestimo(Serializable serializable) : base(serializable) { }

    protected override List<Emprestimo> CarregarArquivos()
    {
        return serializable.Emprestimos;
    }
}