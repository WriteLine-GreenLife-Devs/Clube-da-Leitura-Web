using ClubeDaLeitura.WebApplication.Compartilhado.Infraestrutura.Arquivos;
using ClubeDaLeitura.WebApplication.ModuloCaixa.Dominio;

public class RepositorioCaixa : RepositorioBase<Caixa>, InterfaceRepositorioCaixa
{
    public RepositorioCaixa(Serializable serializable) : base(serializable) { }

    public void Cadastrar(Caixa entidade)
    {
        throw new NotImplementedException();
    }

    public bool Editar(string idSelecionado, Caixa entidadeAtualizada)
    {
        throw new NotImplementedException();
    }

    public bool Excluir(Caixa registro)
    {
        throw new NotImplementedException();
    }

    public List<Caixa> Filtrar(Predicate<Caixa> filtro)
    {
        throw new NotImplementedException();
    }

    public Caixa? SelecionarPorId(string idSelecionado)
    {
        throw new NotImplementedException();
    }

    public List<Caixa> SelecionarTodos()
    {
        throw new NotImplementedException();
    }

    protected override List<Caixa> CarregarArquivos()
    {
        throw new NotImplementedException();
    }

    protected override List<Caixa> CarregarRegistros()
    {
        return serializable.Caixas;
    }
}
