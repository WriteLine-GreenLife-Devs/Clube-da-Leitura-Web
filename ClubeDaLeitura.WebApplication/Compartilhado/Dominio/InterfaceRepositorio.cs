using ClubeDaLeitura.WebApplication.Compartilhado.Dominio;

namespace ClubeDaLeitura.WebApplication.Compartilhado.Dominio;

public interface InterfaceRepositorio<T> where T : EntidadeBase<T>
{
    void Cadastrar(T entidade);
    bool Editar(string idSelecionado, T entidadeAtualizada);
    bool Excluir(T registro);
    T? SelecionarPorId(string idSelecionado);
    List<T> SelecionarTodos();
    List<T> Filtrar(Predicate<T> filtro);
}
