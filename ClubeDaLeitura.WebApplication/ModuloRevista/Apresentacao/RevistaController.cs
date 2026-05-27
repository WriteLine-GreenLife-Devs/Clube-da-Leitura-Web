using ClubeDaLeitura.WebApplication.ModuloCaixa.Dominio;
using ClubeDaLeitura.WebApplication.ModuloCaixa.Infraestrutura;
using ClubeDaLeitura.WebApplication.ModuloRevista.Dominio;
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

        List<ListarRevistasViewModel> listarVms = new List<ListarRevistasViewModel>();

        foreach (Revista r in revistas)
        {
            ListarRevistasViewModel viewModel = new ListarRevistasViewModel(
                r.Id,
                r.Titulo,
                r.NumeroEdicao,
                r.AnoPublicacao,
                r.CaixaId,
                r.Status.ToString()
            );

            listarVms.Add(viewModel);
        }

        return View(listarVms);
    }

    [HttpGet]
    public ActionResult Cadastrar()
    {
        var caixas = repositorioCaixa.SelecionarTodos();
        ViewBag.Caixas = caixas;

        var cadastrarVm = new CadastrarRevistaViewModel(
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

        bool tituloDuplicado = repositorioRevista
        .SelecionarTodos()
        .Any(r => r.Titulo.Equals(cadastrarVm.Titulo, StringComparison.OrdinalIgnoreCase));

        if (tituloDuplicado)
        {
            ModelState.AddModelError("Titulo", "Já existe uma revista com este título.");
            var caixas = repositorioCaixa.SelecionarTodos();
            ViewBag.Caixas = caixas;
            return View(cadastrarVm);
        }

        Revista novaRevista = new Revista(
            cadastrarVm.Titulo,
            cadastrarVm.NumeroEdicao,
            cadastrarVm.AnoPublicacao,
            cadastrarVm.CaixaId
        );

        repositorioRevista.Cadastrar(novaRevista);

        return RedirectToAction(nameof(Listar));
    }

    [HttpGet]
    public ActionResult Editar(string id)
    {
        Revista? revista = repositorioRevista.SelecionarPorId(id);

        if (revista == null)
            return RedirectToAction(nameof(Listar));

        var caixas = repositorioCaixa.SelecionarTodos();
        ViewBag.Caixas = caixas;

        var editarVm = new EditarRevistaViewModel(
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

        Revista revistaAtualizada = new Revista(
            editarVm.Titulo,
            editarVm.NumeroEdicao,
            editarVm.AnoPublicacao,
            editarVm.CaixaId
        );

        repositorioRevista.Editar(editarVm.Id, revistaAtualizada);

        return RedirectToAction(nameof(Listar));
    }


    [HttpGet]
    public ActionResult Excluir(string id)
    {
        Revista? revista = repositorioRevista.SelecionarPorId(id);

        if (revista == null)
            return RedirectToAction(nameof(Listar));

        ExcluirRevistaViewModel excluirVm = new ExcluirRevistaViewModel(
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