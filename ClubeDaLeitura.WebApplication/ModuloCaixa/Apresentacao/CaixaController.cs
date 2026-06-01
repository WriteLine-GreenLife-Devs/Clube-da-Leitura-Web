using ClubeDaLeitura.WebApplication.ModuloCaixa.Dominio;
using ClubeDaLeituraWeb.WebApp.ModuloCaixa.Apresentacao;
using Microsoft.AspNetCore.Mvc;

namespace ClubeDaLeitura.WebApplication.ModuloCaixa.Apresentacao;

public class CaixaController : Controller
{
    private readonly InterfaceRepositorioCaixa repositorioCaixa;

    public CaixaController(InterfaceRepositorioCaixa repositorioCaixa)
    {
        this.repositorioCaixa = repositorioCaixa;
    }

    [HttpGet]
    public ActionResult Listar()
    {
        List<Caixa> caixas = repositorioCaixa.SelecionarTodos();

        List<ListarCaixasViewModel> listarVms = new List<ListarCaixasViewModel>();

        foreach (Caixa c in caixas)
        {
            ListarCaixasViewModel viewModel = new ListarCaixasViewModel(
                c.Id,
                c.Etiqueta,
                c.Cor,
                c.DiasDeEmprestimo
            );

            listarVms.Add(viewModel);
        }

        return View(listarVms);
    }

    [HttpGet]
    public ActionResult Cadastrar()
    {
        CadastrarCaixaViewModel cadastrarVm = new CadastrarCaixaViewModel(
            string.Empty,
            string.Empty,
            7
        );

        return View(cadastrarVm);
    }

    [HttpPost]
    public ActionResult Cadastrar(CadastrarCaixaViewModel cadastrarVm)
    {
        if (!ModelState.IsValid)
            return View(cadastrarVm);

        Caixa novaCaixa = new Caixa(
            cadastrarVm.Etiqueta,
            cadastrarVm.Cor,
            cadastrarVm.DiasDeEmprestimo
        );

        List<string> erros = novaCaixa.Validar();

        if (ExisteCaixaComEtiqueta(cadastrarVm.Etiqueta))
            erros.Add("Já existe uma caixa com esta etiqueta.");

        if (erros.Any())
        {
            foreach (string erro in erros)
            {
                ModelState.AddModelError(string.Empty, erro);
            }

            return View(cadastrarVm);
        }

        repositorioCaixa.Cadastrar(novaCaixa);

        return RedirectToAction(nameof(Listar));
    }


    [HttpGet]
    public ActionResult Editar(string id)
    {
        Caixa? caixa = repositorioCaixa.SelecionarPorId(id);

        if (caixa == null)
            return RedirectToAction(nameof(Listar));

        EditarCaixaViewModel editarVm = new EditarCaixaViewModel(
            id,
            caixa.Etiqueta,
            caixa.Cor,
            caixa.DiasDeEmprestimo
        );

        return View(editarVm);
    }

    private bool ExisteCaixaComEtiqueta(string etiqueta, string? idIgnorado = null)
    {
        List<Caixa> caixas = repositorioCaixa.SelecionarTodos();

        foreach (Caixa c in caixas)
        {
            if (c.Id != idIgnorado && string.Equals(c.Etiqueta, etiqueta, StringComparison.OrdinalIgnoreCase))
                return true;
        }

        return false;
    }

    [HttpPost]
    public ActionResult Editar(EditarCaixaViewModel editarVm)
    {
        if (!ModelState.IsValid)
            return View(editarVm);

        Caixa caixaAtualizada = new Caixa(
            editarVm.Etiqueta,
            editarVm.Cor,
            editarVm.DiasDeEmprestimo
        );

        List<string> erros = caixaAtualizada.Validar();

        if (ExisteCaixaComEtiqueta(caixaAtualizada.Etiqueta, editarVm.Id))
            erros.Add("Já existe uma caixa com esta etiqueta.");

        if (erros.Any())
        {
            foreach (string erro in erros)
            {
                ModelState.AddModelError(string.Empty, erro);
            }

            return View(editarVm);
        }

        repositorioCaixa.Editar(editarVm.Id, caixaAtualizada);

        return RedirectToAction(nameof(Listar));
    }

    [HttpGet]
    public ActionResult Excluir(string id)
    {
        Caixa? caixa = repositorioCaixa.SelecionarPorId(id);

        if (caixa == null)
            return RedirectToAction(nameof(Listar));

        ExcluirCaixaViewModel excluirVm = new ExcluirCaixaViewModel(
            id,
            caixa.Etiqueta,
            caixa.Cor,
            caixa.DiasDeEmprestimo
        );

        return View(excluirVm);
    }

    [HttpPost]
    public ActionResult Excluir(ExcluirCaixaViewModel excluirVm)
    {
        Caixa? caixa = repositorioCaixa.SelecionarPorId(excluirVm.Id);

        if (caixa != null)
            repositorioCaixa.Excluir(caixa);

        return RedirectToAction(nameof(Listar));
    }
}
