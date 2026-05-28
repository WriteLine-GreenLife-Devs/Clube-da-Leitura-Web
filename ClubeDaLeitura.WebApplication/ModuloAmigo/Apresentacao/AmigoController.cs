using System.Linq;
using ClubeDaLeitura.WebApplication.ModuloAmigo.Dominio;
using ClubeDaLeitura.WebApplication.ModuloEmprestimo.Dominio;
using ClubeDaLeituraWeb.WebApp.ModuloAmigo.Apresentacao;
using Microsoft.AspNetCore.Mvc;

namespace ClubeDaLeitura.WebApplication.ModuloAmigo.Apresentacao;

public class AmigoController : Controller
{
    private readonly InterfaceRepositorioAmigo repositorioAmigo;
    private readonly InterfaceRepositorioEmprestimo repositorioEmprestimo;

    public AmigoController(InterfaceRepositorioAmigo repositorioAmigo, InterfaceRepositorioEmprestimo repositorioEmprestimo)
    {
        this.repositorioAmigo = repositorioAmigo;
        this.repositorioEmprestimo = repositorioEmprestimo;
    }

    [HttpGet]
    public ActionResult Listar()
    {
        List<Amigo> amigos = repositorioAmigo.SelecionarTodos();

        List<ListarAmigosViewModel> listarVms = new List<ListarAmigosViewModel>();

        foreach (Amigo a in amigos)
        {
            ListarAmigosViewModel viewModel = new ListarAmigosViewModel(
                a.Id,
                a.Nome,
                a.NomeResponsavel,
                a.Telefone
            );

            listarVms.Add(viewModel);
        }

        return View(listarVms);
    }

    [HttpGet]
    public ActionResult Cadastrar()
    {
        CadastrarAmigoViewModel cadastrarVm = new CadastrarAmigoViewModel(
            string.Empty,
            string.Empty,
            string.Empty
        );

        return View(cadastrarVm);
    }

    [HttpPost]
    public ActionResult Cadastrar(CadastrarAmigoViewModel cadastrarVm)
    {
        if (!ModelState.IsValid)
            return View(cadastrarVm);

        Amigo novoAmigo = new Amigo(
            cadastrarVm.Nome,
            cadastrarVm.NomeResponsavel,
            cadastrarVm.Telefone
        );

        List<string> erros = novoAmigo.Validar();

        if (erros.Any())
        {
            foreach (string erro in erros)
            {
                ModelState.AddModelError(string.Empty, erro);
            }

            return View(cadastrarVm);
        }

        repositorioAmigo.Cadastrar(novoAmigo);

        return RedirectToAction(nameof(Listar));
    }


    [HttpGet]
    public ActionResult Editar(string id)
    {
        Amigo? amigo = repositorioAmigo.SelecionarPorId(id);

        if (amigo == null)
            return RedirectToAction(nameof(Listar));

        EditarAmigoViewModel editarVm = new EditarAmigoViewModel(
            id,
            amigo.Nome,
            amigo.NomeResponsavel,
            amigo.Telefone
        );

        return View(editarVm);
    }

    [HttpPost]
    public ActionResult Editar(EditarAmigoViewModel editarVm)
    {
        if (!ModelState.IsValid)
            return View(editarVm);

        Amigo amigoAtualizado = new Amigo(
            editarVm.Nome,
            editarVm.NomeResponsavel,
            editarVm.Telefone
        );

        List<string> erros = amigoAtualizado.Validar();

        if (erros.Any())
        {
            foreach (string erro in erros)
            {
                ModelState.AddModelError(string.Empty, erro);
            }

            return View(editarVm);
        }

        repositorioAmigo.Editar(editarVm.Id, amigoAtualizado);

        return RedirectToAction(nameof(Listar));
    }

    [HttpGet]
    public ActionResult Excluir(string id)
    {
        Amigo? amigo = repositorioAmigo.SelecionarPorId(id);

        if (amigo == null)
            return RedirectToAction(nameof(Listar));

        ExcluirAmigoViewModel excluirVm = new ExcluirAmigoViewModel(
            id,
            amigo.Nome,
            amigo.NomeResponsavel,
            amigo.Telefone
        );

        return View(excluirVm);
    }

    [HttpPost]
    public ActionResult Excluir(ExcluirAmigoViewModel excluirVm)
    {
        Amigo? amigo = repositorioAmigo.SelecionarPorId(excluirVm.Id);

        if (amigo == null)
            return RedirectToAction(nameof(Listar));

        if (repositorioEmprestimo.ObterPorAmigo(excluirVm.Id).Any())
        {
            ModelState.AddModelError(string.Empty, "Não é possível excluir um amigo com empréstimos vinculados.");
            return View(excluirVm);
        }

        repositorioAmigo.Excluir(amigo);

        return RedirectToAction(nameof(Listar));
    }
}
