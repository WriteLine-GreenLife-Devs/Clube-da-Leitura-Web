using ClubeDaLeitura.WebApplication.Compartilhado.Dominio;

namespace ClubeDaLeitura.WebApplication.Compartilhado.Infraestrutura.Arquivos;

public abstract class RepositorioBaseEmArquivo<T> where T : EntidadeBase<T>
{
    protected Serializable serializable;
    protected List<T> arquivos;

    public RepositorioBaseEmArquivo(Serializable serializable)
    {
        this.serializable = serializable;
        this.arquivos = CarregarArquivos();
    }

    protected abstract List<T> CarregarArquivos();

    public void Cadastrar(T entidade)
    {
        arquivos.Add(entidade);

        serializable.Salvar();
    }

    public bool Editar(string idSelecionado, T entidadeAtualizada)
    {
        T? arquivoSelecionado = SelecionarPorId(idSelecionado);

        if (arquivoSelecionado == null)
            return false;

        arquivoSelecionado.AtualizarDados(entidadeAtualizada);

        serializable.Salvar();

        return true;
    }

    public bool Excluir(T registro)
    {
        bool conseguiuExcluir = arquivos.Remove(registro);

        if (conseguiuExcluir)
            serializable.Salvar();

        return conseguiuExcluir;
    }

    public bool Excluir(string idSelecionado)
    {
        T? arquivoSelecionado = SelecionarPorId(idSelecionado);

        if (arquivoSelecionado == null)
            return false;

        return Excluir(arquivoSelecionado);
    }

    public T? SelecionarPorId(string idSelecionado)
    {
        foreach (T arquivo in arquivos)
        {
            if (arquivo.Id == idSelecionado)
                return arquivo;
        }

        return null;
    }

    public List<T> SelecionarTodos()
    {
        return arquivos;
    }

    public List<T> Filtrar(Predicate<T> filtro)
    {
        List<T> arquivosFiltrados = new List<T>();

        foreach (T e in arquivos)
        {
            if (filtro(e))
                arquivosFiltrados.Add(e);
        }

        return arquivosFiltrados;
    }
}
