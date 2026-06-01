using System.Linq;
using ClubeDaLeitura.WebApplication.ModuloAmigo.Dominio;
using ClubeDaLeitura.WebApplication.ModuloEmprestimo.Dominio;
using FluentResults;

namespace ClubeDaLeitura.WebApplication.ModuloAmigo.Aplicacao;

public class ServicoAmigo
{
    private readonly InterfaceRepositorioAmigo repositorioAmigo;
    private readonly InterfaceRepositorioEmprestimo repositorioEmprestimo;

    public ServicoAmigo(
        InterfaceRepositorioAmigo repositorioAmigo,
        InterfaceRepositorioEmprestimo repositorioEmprestimo)
    {
        this.repositorioAmigo = repositorioAmigo;
        this.repositorioEmprestimo = repositorioEmprestimo;
    }

    public Result Cadastrar(CadastrarAmigoDto dto)
    {
        Amigo novoAmigo = new(dto.Nome, dto.NomeResponsavel, dto.Telefone);

        List<string> erros = novoAmigo.Validar();

        if (erros.Any())
            return Result.Fail(erros.Select(erro => new Error(erro)).ToList());

        repositorioAmigo.Cadastrar(novoAmigo);

        return Result.Ok();
    }

    public Result Editar(EditarAmigoDto dto)
    {
        Amigo amigoAtualizado = new(dto.Nome, dto.NomeResponsavel, dto.Telefone);

        List<string> erros = amigoAtualizado.Validar();

        if (erros.Any())
            return Result.Fail(erros.Select(erro => new Error(erro)).ToList());

        bool conseguiuEditar = repositorioAmigo.Editar(dto.Id, amigoAtualizado);

        if (!conseguiuEditar)
            return Result.Fail("Amigo não encontrado.");

        return Result.Ok();
    }

    public Result Excluir(string id)
    {
        Amigo? amigo = repositorioAmigo.SelecionarPorId(id);

        if (amigo == null)
            return Result.Fail("Amigo não encontrado.");

        if (repositorioEmprestimo.AmigoTemEmprestimoAtivo(id))
            return Result.Fail("Não é possível excluir um amigo com empréstimos ativos.");

        repositorioAmigo.Excluir(amigo);

        return Result.Ok();
    }

    public List<ListarAmigosDto> SelecionarTodos()
    {
        return repositorioAmigo.SelecionarTodos()
            .Select(a => new ListarAmigosDto(a.Id, a.Nome, a.NomeResponsavel, a.Telefone))
            .ToList();
    }

    public Result<DetalhesAmigoDto> SelecionarPorId(string id)
    {
        Amigo? amigo = repositorioAmigo.SelecionarPorId(id);

        if (amigo == null)
            return Result.Fail("Amigo não encontrado.");

        return Result.Ok(new DetalhesAmigoDto(amigo.Id, amigo.Nome, amigo.NomeResponsavel, amigo.Telefone));
    }
}
