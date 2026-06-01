using System.Linq;
using ClubeDaLeitura.WebApplication.ModuloCaixa.Dominio;
using ClubeDaLeitura.WebApplication.ModuloEmprestimo.Dominio;
using ClubeDaLeitura.WebApplication.ModuloRevista.Dominio;
using FluentResults;

namespace ClubeDaLeitura.WebApplication.ModuloRevista.Aplicacao;

public class ServicoRevista
{
    private readonly InterfaceRepositorioRevista repositorioRevista;
    private readonly InterfaceRepositorioCaixa repositorioCaixa;
    private readonly InterfaceRepositorioEmprestimo repositorioEmprestimo;

    public ServicoRevista(
        InterfaceRepositorioRevista repositorioRevista,
        InterfaceRepositorioCaixa repositorioCaixa,
        InterfaceRepositorioEmprestimo repositorioEmprestimo)
    {
        this.repositorioRevista = repositorioRevista;
        this.repositorioCaixa = repositorioCaixa;
        this.repositorioEmprestimo = repositorioEmprestimo;
    }

    public Result Cadastrar(CadastrarRevistaDto dto)
    {
        Revista novaRevista = new(dto.Titulo, dto.NumeroEdicao, dto.AnoPublicacao, dto.CaixaId);

        List<string> erros = novaRevista.Validar();

        if (repositorioRevista.SelecionarTodos()
            .Any(r => r.Titulo == dto.Titulo && r.NumeroEdicao == dto.NumeroEdicao))
        {
            erros.Add("Já existe uma revista com este título e edição.");
        }

        if (erros.Any())
            return Result.Fail(erros.Select(erro => new Error(erro)).ToList());

        repositorioRevista.Cadastrar(novaRevista);

        return Result.Ok();
    }

    public Result Editar(EditarRevistaDto dto)
    {
        Revista? revistaExistente = repositorioRevista.SelecionarPorId(dto.Id);

        if (revistaExistente == null)
            return Result.Fail("Revista não encontrada.");

        Revista revistaAtualizada = new(dto.Titulo, dto.NumeroEdicao, dto.AnoPublicacao, dto.CaixaId)
        {
            Status = revistaExistente.Status
        };

        List<string> erros = revistaAtualizada.Validar();

        if (repositorioRevista.SelecionarTodos()
            .Any(r => r.Titulo == dto.Titulo && r.NumeroEdicao == dto.NumeroEdicao && r.Id != dto.Id))
        {
            erros.Add("Já existe uma revista com este título e edição.");
        }

        if (erros.Any())
            return Result.Fail(erros.Select(erro => new Error(erro)).ToList());

        bool conseguiuEditar = repositorioRevista.Editar(dto.Id, revistaAtualizada);

        if (!conseguiuEditar)
            return Result.Fail("Revista não encontrada.");

        return Result.Ok();
    }

    public Result Excluir(string id)
    {
        Revista? revista = repositorioRevista.SelecionarPorId(id);

        if (revista == null)
            return Result.Fail("Revista não encontrada.");

        if (repositorioEmprestimo.RevistaEstaEmprestada(id))
            return Result.Fail("Não é possível excluir uma revista com empréstimos ativos.");

        bool conseguiuExcluir = repositorioRevista.Excluir(revista);

        if (!conseguiuExcluir)
            return Result.Fail("Não foi possível excluir a revista.");

        return Result.Ok();
    }

    public List<ListarRevistasDto> SelecionarTodos()
    {
        List<Revista> revistas = repositorioRevista.SelecionarTodos();

        return revistas
            .Select(r => new ListarRevistasDto(
                r.Id,
                r.Titulo,
                r.NumeroEdicao,
                r.AnoPublicacao,
                repositorioCaixa.SelecionarPorId(r.CaixaId)?.Etiqueta ?? "Sem Caixa",
                r.Status.ToString()))
            .ToList();
    }

    public Result<DetalhesRevistaDto> SelecionarPorId(string id)
    {
        Revista? revista = repositorioRevista.SelecionarPorId(id);

        if (revista == null)
            return Result.Fail("Revista não encontrada.");

        return Result.Ok(new DetalhesRevistaDto(revista.Id, revista.Titulo, revista.NumeroEdicao, revista.AnoPublicacao, revista.CaixaId, revista.Status.ToString()));
    }
}
