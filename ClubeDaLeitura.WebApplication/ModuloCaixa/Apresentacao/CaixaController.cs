using AutoMapper;
using ClubeDaLeitura.WebApplication.Compartilhado.Apresentacao.Extensions;
using ClubeDaLeitura.WebApplication.ModuloCaixa.Aplicacao;
using FluentResults;
using Microsoft.AspNetCore.Mvc;

namespace ClubeDaLeitura.WebApplication.ModuloCaixa.Apresentacao;

public class CaixaController(ServicoCaixa servicoCaixa, IMapper mapeador) : Controller
{
    [HttpGet]
    public ActionResult Listar()
    {
        List<ListarCaixasDto> dtos = servicoCaixa.SelecionarTodos();

        List<ListarCaixasViewModel> listarVms = mapeador.Map<List<ListarCaixasViewModel>>(dtos);

        return View(listarVms);
    }

    [HttpGet]
    public ActionResult Cadastrar()
    {
        CadastrarCaixaViewModel cadastrarVm = new(
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

        Result resultado = servicoCaixa.Cadastrar(mapeador.Map<CadastrarCaixaDto>(cadastrarVm));

        if (resultado.IsFailed)
        {
            ModelState.AddModelError(resultado);
            return View(cadastrarVm);
        }

        return RedirectToAction(nameof(Listar));
    }

    [HttpGet]
    public ActionResult Editar(string id)
    {
        Result<DetalhesCaixaDto> resultado = servicoCaixa.SelecionarPorId(id);

        if (resultado.IsFailed)
            return RedirectToAction(nameof(Listar));

        EditarCaixaViewModel editarVm = mapeador.Map<EditarCaixaViewModel>(resultado.Value);

        return View(editarVm);
    }

    [HttpPost]
    public ActionResult Editar(EditarCaixaViewModel editarVm)
    {
        if (!ModelState.IsValid)
            return View(editarVm);

        Result resultado = servicoCaixa.Editar(mapeador.Map<EditarCaixaDto>(editarVm));

        if (resultado.IsFailed)
        {
            ModelState.AddModelError(resultado);
            return View(editarVm);
        }

        return RedirectToAction(nameof(Listar));
    }

    [HttpGet]
    public ActionResult Excluir(string id)
    {
        Result<DetalhesCaixaDto> resultado = servicoCaixa.SelecionarPorId(id);

        if (resultado.IsFailed)
            return RedirectToAction(nameof(Listar));

        ExcluirCaixaViewModel excluirVm = mapeador.Map<ExcluirCaixaViewModel>(resultado.Value);

        return View(excluirVm);
    }

    [HttpPost]
    public ActionResult Excluir(ExcluirCaixaViewModel excluirVm)
    {
        Result resultado = servicoCaixa.Excluir(excluirVm.Id);

        if (resultado.IsFailed)
        {
            ModelState.AddModelError(resultado);
            return View(excluirVm);
        }

        return RedirectToAction(nameof(Listar));
    }
}

