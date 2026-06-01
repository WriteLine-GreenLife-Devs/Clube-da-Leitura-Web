using System.Collections.Generic;
using ClubeDaLeitura.WebApplication.Compartilhado.Dominio;

namespace ClubeDaLeitura.WebApplication.ModuloEmprestimo.Dominio;

public interface InterfaceRepositorioEmprestimo : InterfaceRepositorio<Emprestimo>
{
    List<Emprestimo> ObterTodos();
    Emprestimo ObterPorId(string id);
    List<Emprestimo> ObterPorAmigo(string amigoId);
    List<Emprestimo> ObterAbertos();
    void Adicionar(Emprestimo emprestimo);
    void Atualizar(Emprestimo emprestimo);
    void Remover(string id);

    bool AmigoTemEmprestimoAtivo(string amigoId);
    bool RevistaEstaEmprestada(string revistaId);
}
