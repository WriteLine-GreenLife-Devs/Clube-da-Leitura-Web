using System.Linq;
using ClubeDaLeitura.WebApplication.ModuloAmigo.Dominio;
using ClubeDaLeitura.WebApplication.ModuloCaixa.Dominio;
using ClubeDaLeitura.WebApplication.ModuloEmprestimo.Dominio;
using ClubeDaLeitura.WebApplication.ModuloRevista.Dominio;
using FluentResults;

namespace ClubeDaLeitura.WebApplication.ModuloEmprestimo.Aplicacao;

public class ServicoEmprestimo
{
    private readonly InterfaceRepositorioEmprestimo repositorioEmprestimo;
    private readonly InterfaceRepositorioAmigo repositorioAmigo;
    private readonly InterfaceRepositorioRevista repositorioRevista;
    private readonly InterfaceRepositorioCaixa repositorioCaixa;

    public ServicoEmprestimo(
        InterfaceRepositorioEmprestimo repositorioEmprestimo,
        InterfaceRepositorioAmigo repositorioAmigo,
        InterfaceRepositorioRevista repositorioRevista,
        InterfaceRepositorioCaixa repositorioCaixa)
    {
        this.repositorioEmprestimo = repositorioEmprestimo;
        this.repositorioAmigo = repositorioAmigo;
        this.repositorioRevista = repositorioRevista;
        this.repositorioCaixa = repositorioCaixa;
    }

    public Result Cadastrar(CadastrarEmprestimoDto dto)
    {
        Amigo? amigo = repositorioAmigo.SelecionarPorId(dto.AmigoId);
        Revista? revista = repositorioRevista.SelecionarPorId(dto.RevistaId);

        if (amigo == null)
            return Result.Fail("Amigo não encontrado.");

        if (revista == null)
            return Result.Fail("Revista não encontrada.");

        if (repositorioEmprestimo.AmigoTemEmprestimoAtivo(amigo.Id))
            return Result.Fail("O amigo já possui um empréstimo ativo.");

        if (revista.Status != StatusRevista.Disponivel)
            return Result.Fail("A revista não está disponível no momento.");

        Caixa? caixa = repositorioCaixa.SelecionarPorId(revista.CaixaId);
        if (caixa == null)
            return Result.Fail("A caixa da revista não foi encontrada.");

        Emprestimo emprestimo = new(amigo.Id, revista.Id, DateTime.Now, caixa.DiasDeEmprestimo);
        List<string> erros = emprestimo.Validar();

        if (erros.Any())
            return Result.Fail(erros.Select(erro => new Error(erro)).ToList());

        repositorioEmprestimo.Adicionar(emprestimo);

        revista.Status = StatusRevista.Emprestada;
        repositorioRevista.Editar(revista.Id, revista);

        return Result.Ok();
    }

    public List<ListarEmprestimosDto> SelecionarTodos(string status, string amigoId)
    {
        List<Emprestimo> emprestimos = repositorioEmprestimo.SelecionarTodos();

        foreach (Emprestimo emprestimo in emprestimos)
        {
            StatusEmprestimo statusAnterior = emprestimo.Status;
            emprestimo.VerificarEAtualizarAtraso(DateTime.Now);

            if (emprestimo.Status != statusAnterior)
                repositorioEmprestimo.Atualizar(emprestimo);
        }

        if (!string.IsNullOrWhiteSpace(amigoId))
            emprestimos = emprestimos.Where(e => e.AmigoId == amigoId).ToList();

        if (!string.IsNullOrWhiteSpace(status) && Enum.TryParse(status, true, out StatusEmprestimo filtroStatus))
            emprestimos = emprestimos.Where(e => e.Status == filtroStatus).ToList();

        return emprestimos.Select(emprestimo =>
        {
            Amigo? amigo = repositorioAmigo.SelecionarPorId(emprestimo.AmigoId);
            Revista? revista = repositorioRevista.SelecionarPorId(emprestimo.RevistaId);

            return new ListarEmprestimosDto(
                emprestimo.Id,
                amigo?.Nome ?? "Amigo não encontrado",
                revista?.Titulo ?? "Revista não encontrada",
                emprestimo.DataEmprestimo,
                emprestimo.DataDevolucaoPrevista,
                emprestimo.DataDevolucaoReal,
                emprestimo.Status.ToString()
            );
        }).ToList();
    }

    public Result<DevolverEmprestimoDto> SelecionarPorId(string id)
    {
        Emprestimo? emprestimo = repositorioEmprestimo.SelecionarPorId(id);

        if (emprestimo == null)
            return Result.Fail("Empréstimo não encontrado.");

        Amigo? amigo = repositorioAmigo.SelecionarPorId(emprestimo.AmigoId);
        Revista? revista = repositorioRevista.SelecionarPorId(emprestimo.RevistaId);

        return Result.Ok(new DevolverEmprestimoDto(
            emprestimo.Id,
            amigo?.Nome ?? "Amigo não encontrado",
            revista?.Titulo ?? "Revista não encontrada",
            emprestimo.DataEmprestimo,
            emprestimo.DataDevolucaoPrevista,
            emprestimo.Status.ToString()
        ));
    }

    public Result Devolver(string id, DateTime dataDevolucao)
    {
        Emprestimo? emprestimo = repositorioEmprestimo.SelecionarPorId(id);

        if (emprestimo == null)
            return Result.Fail("Empréstimo não encontrado.");

        if (emprestimo.Status == StatusEmprestimo.Concluido)
            return Result.Fail("Empréstimo já concluído.");

        emprestimo.RegistrarDevolucao(dataDevolucao);
        repositorioEmprestimo.Atualizar(emprestimo);

        Revista? revista = repositorioRevista.SelecionarPorId(emprestimo.RevistaId);
        if (revista != null)
        {
            revista.Status = StatusRevista.Disponivel;
            repositorioRevista.Editar(revista.Id, revista);
        }

        return Result.Ok();
    }
}
