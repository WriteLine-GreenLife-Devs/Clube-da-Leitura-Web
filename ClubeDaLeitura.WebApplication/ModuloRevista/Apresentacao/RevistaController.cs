using AutoMapper;
using ClubeDaLeitura.WebApplication.Compartilhado.Apresentacao.Extensions;
using ClubeDaLeitura.WebApplication.ModuloCaixa.Dominio;
using ClubeDaLeitura.WebApplication.ModuloRevista.Aplicacao;
using FluentResults;
using Microsoft.AspNetCore.Mvc;

namespace ClubeDaLeitura.WebApplication.ModuloRevista.Apresentacao;

public class RevistaController(ServicoRevista servicoRevista, InterfaceRepositorioCaixa repositorioCaixa, IMapper mapeador) : Controller
{
    [HttpGet]
    public ActionResult Listar()
    {
        List<ListarRevistasDto> dtos = servicoRevista.SelecionarTodos();

        List<ListarRevistasViewModel> listarVms = mapeador.Map<List<ListarRevistasViewModel>>(dtos ?? new List<ListarRevistasDto>());

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

        Result resultado = servicoRevista.Cadastrar(mapeador.Map<CadastrarRevistaDto>(cadastrarVm));

        if (resultado.IsFailed)
        {
            ModelState.AddModelError(resultado);
            ViewBag.Caixas = repositorioCaixa.SelecionarTodos();
            return View(cadastrarVm);
        }

        return RedirectToAction(nameof(Listar));
    }

    [HttpGet]
    public ActionResult Editar(string id)
    {
        Result<DetalhesRevistaDto> resultado = servicoRevista.SelecionarPorId(id);

        if (resultado.IsFailed)
            return RedirectToAction(nameof(Listar));

        ViewBag.Caixas = repositorioCaixa.SelecionarTodos();

        EditarRevistaViewModel editarVm = mapeador.Map<EditarRevistaViewModel>(resultado.Value);

        return View(editarVm);
    }

    [HttpPost]
    public ActionResult Editar(EditarRevistaViewModel editarVm)
    {
        if (!ModelState.IsValid)
            return View(editarVm);

        Result resultado = servicoRevista.Editar(mapeador.Map<EditarRevistaDto>(editarVm));

        if (resultado.IsFailed)
        {
            ModelState.AddModelError(resultado);
            ViewBag.Caixas = repositorioCaixa.SelecionarTodos();
            ViewBag.Titulo = "Editar Revista";
            return View(editarVm);
        }

        return RedirectToAction(nameof(Listar));
    }

    [HttpGet]
    public ActionResult Excluir(string id)
    {
        Result<DetalhesRevistaDto> resultado = servicoRevista.SelecionarPorId(id);

        if (resultado.IsFailed)
            return RedirectToAction(nameof(Listar));

        ExcluirRevistaViewModel excluirVm = mapeador.Map<ExcluirRevistaViewModel>(resultado.Value);

        return View(excluirVm);
    }

    [HttpPost]
    [ActionName("Excluir")]
    public ActionResult ConfirmarExcluir(string id)
    {
        Result resultado = servicoRevista.Excluir(id);

        if (resultado.IsFailed)
        {
            ModelState.AddModelError(resultado);
            Result<DetalhesRevistaDto> detalhes = servicoRevista.SelecionarPorId(id);
            ExcluirRevistaViewModel excluirVm = mapeador.Map<ExcluirRevistaViewModel>(detalhes.Value);
            return View(excluirVm);
        }

        return RedirectToAction(nameof(Listar));
    }
}
