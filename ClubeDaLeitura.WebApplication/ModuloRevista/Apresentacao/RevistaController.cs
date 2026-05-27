using ClubeDaLeitura.WebApplication.ModuloRevista.Dominio;
using ClubeDaLeitura.WebApplication.ModuloCaixa.Dominio;
using ClubeDaLeituraWeb.WebApp.ModuloRevista.Apresentacao;
using Microsoft.AspNetCore.Mvc;

namespace ClubeDaLeitura.WebApplication.ModuloRevista.Apresentacao;

public class RevistaController : Controller
{
    private readonly InterfaceRepositorioRevista repositorioRevista;
    private readonly InterfaceRepositorioCaixa repositorioCaixa;

    public RevistaController(InterfaceRepositorioRevista repositorioRevista, InterfaceRepositorioCaixa repositorioCaixa)
    {
        this.repositorioRevista = repositorioRevista;
        this.repositorioCaixa = repositorioCaixa;
    }

    [HttpGet]
    public ActionResult Listar()
    {
        List<Revista> revistas = repositorioRevista.SelecionarTodos();

        List<ListarRevistasViewModel> listarVms = new();

        foreach (Revista r in revistas)
        {
            Caixa? caixa = repositorioCaixa.SelecionarPorId(r.CaixaId);

            ListarRevistasViewModel vm = new ListarRevistasViewModel(
                r.Id,
                r.Titulo,
                r.NumeroEdicao,
                r.AnoPublicacao,
                caixa?.Etiqueta ?? "Sem Caixa",
                r.Status.ToString()
            );

            listarVms.Add(vm);
        }

        return View(listarVms);
    }

    [HttpGet]
    public ActionResult Cadastrar()
    {
        ViewBag.Caixas = repositorioCaixa.SelecionarTodos();

        CadastrarRevistaViewModel cadastrarVm = new(
            string.Empty,
            0,
            DateTime.Now,
            string.Empty
        );

        return View(cadastrarVm);
    }

    [HttpPost]
    public ActionResult Cadastrar(CadastrarRevistaViewModel cadastrarVm)
    {
        if (!ModelState.IsValid)
            return View(cadastrarVm);

        Revista novaRevista = new(
            cadastrarVm.Titulo,
            cadastrarVm.NumeroEdicao,
            cadastrarVm.AnoPublicacao,
            cadastrarVm.CaixaId
        );

        List<string> erros = novaRevista.Validar();

        if (repositorioRevista.SelecionarTodos()
            .Any(r => r.Titulo == cadastrarVm.Titulo && r.NumeroEdicao == cadastrarVm.NumeroEdicao))
        {
            erros.Add("Já existe uma revista com este título e edição.");
        }

        if (erros.Any())
        {
            foreach (string erro in erros)
                ModelState.AddModelError(string.Empty, erro);

            ViewBag.Caixas = repositorioCaixa.SelecionarTodos();
            return View(cadastrarVm);
        }

        repositorioRevista.Cadastrar(novaRevista);

        return RedirectToAction(nameof(Listar));
    }

    [HttpGet]
    public ActionResult Editar(string id)
    {
        Revista? revista = repositorioRevista.SelecionarPorId(id);

        if (revista == null)
            return RedirectToAction(nameof(Listar));

        ViewBag.Caixas = repositorioCaixa.SelecionarTodos();

        EditarRevistaViewModel editarVm = new(
            id,
            revista.Titulo,
            revista.NumeroEdicao,
            revista.AnoPublicacao,
            revista.CaixaId
        );

        return View(editarVm);
    }

    [HttpPost]
    public ActionResult Editar(EditarRevistaViewModel editarVm)
    {
        if (!ModelState.IsValid)
            return View(editarVm);

        Revista revistaAtualizada = new(
            editarVm.Titulo,
            editarVm.NumeroEdicao,
            editarVm.AnoPublicacao,
            editarVm.CaixaId
        );

        List<string> erros = revistaAtualizada.Validar();

        if (repositorioRevista.SelecionarTodos()
            .Any(r => r.Titulo == editarVm.Titulo && r.NumeroEdicao == editarVm.NumeroEdicao && r.Id != editarVm.Id))
        {
            erros.Add("Já existe uma revista com este título e edição.");
        }

        if (erros.Any())
        {
            foreach (string erro in erros)
                ModelState.AddModelError(string.Empty, erro);

            ViewBag.Caixas = repositorioCaixa.SelecionarTodos();
            return View(editarVm);
        }

        repositorioRevista.Editar(editarVm.Id, revistaAtualizada);

        return RedirectToAction(nameof(Listar));
    }

    [HttpGet]
    public ActionResult Excluir(string id)
    {
        Revista? revista = repositorioRevista.SelecionarPorId(id);

        if (revista == null)
            return RedirectToAction(nameof(Listar));

        ExcluirRevistaViewModel excluirVm = new(
            id,
            revista.Titulo,
            revista.NumeroEdicao,
            revista.AnoPublicacao,
            revista.CaixaId
        );

        return View(excluirVm);
    }

    [HttpPost]
    public ActionResult Excluir(ExcluirRevistaViewModel excluirVm)
    {
        Revista? revista = repositorioRevista.SelecionarPorId(excluirVm.Id);

        if (revista != null)
            repositorioRevista.Excluir(revista);

        return RedirectToAction(nameof(Listar));
    }
}