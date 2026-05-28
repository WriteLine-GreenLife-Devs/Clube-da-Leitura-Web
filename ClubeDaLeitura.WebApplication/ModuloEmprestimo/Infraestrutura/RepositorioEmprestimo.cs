using System;
using System.Collections.Generic;
using System.Linq;
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

    public List<Emprestimo> ObterTodos() => SelecionarTodos();

    public Emprestimo ObterPorId(string id) => SelecionarPorId(id) ?? throw new ArgumentException("Empréstimo não encontrado.", nameof(id));

    public List<Emprestimo> ObterPorAmigo(string amigoId) => Filtrar(e => e.AmigoId == amigoId);

    public List<Emprestimo> ObterAbertos() => Filtrar(e => e.Status == StatusEmprestimo.Aberto || e.Status == StatusEmprestimo.Atrasado);

    public void Adicionar(Emprestimo emprestimo) => Cadastrar(emprestimo);

    public void Atualizar(Emprestimo emprestimo) => Editar(emprestimo.Id, emprestimo);

    public void Remover(string id) => Excluir(id);

    public bool AmigoTemEmprestimoAtivo(string amigoId) => Filtrar(e => e.AmigoId == amigoId && (e.Status == StatusEmprestimo.Aberto || e.Status == StatusEmprestimo.Atrasado)).Any();

    public bool RevistaEstaEmprestada(string revistaId) => Filtrar(e => e.RevistaId == revistaId && (e.Status == StatusEmprestimo.Aberto || e.Status == StatusEmprestimo.Atrasado)).Any();
}