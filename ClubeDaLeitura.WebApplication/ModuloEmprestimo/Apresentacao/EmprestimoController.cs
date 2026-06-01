using AutoMapper;
using ClubeDaLeitura.WebApplication.Compartilhado.Apresentacao.Extensions;
using ClubeDaLeitura.WebApplication.ModuloAmigo.Dominio;
using ClubeDaLeitura.WebApplication.ModuloCaixa.Dominio;
using ClubeDaLeitura.WebApplication.ModuloEmprestimo.Aplicacao;
using ClubeDaLeitura.WebApplication.ModuloRevista.Dominio;
using FluentResults;
using Microsoft.AspNetCore.Mvc;

namespace ClubeDaLeitura.WebApplication.ModuloEmprestimo.Apresentacao;

public class EmprestimoController(
    ServicoEmprestimo servicoEmprestimo,
    InterfaceRepositorioAmigo repositorioAmigo,
    InterfaceRepositorioRevista repositorioRevista,
    IMapper mapeador) : Controller
{
    [HttpGet]
    public ActionResult Listar(string status, string amigoId)
    {
        List<ListarEmprestimosDto> dtos = servicoEmprestimo.SelecionarTodos(status, amigoId);

        List<ListarEmprestimosViewModel> listarVms = mapeador.Map<List<ListarEmprestimosViewModel>>(dtos);

        ViewBag.Amigos = repositorioAmigo.SelecionarTodos();
        ViewBag.Status = status ?? string.Empty;
        ViewBag.AmigoId = amigoId ?? string.Empty;

        return View(listarVms);
    }

    [HttpGet]
    public ActionResult Cadastrar()
    {
        ViewBag.Amigos = repositorioAmigo.SelecionarTodos();
        ViewBag.Revistas = repositorioRevista.SelecionarTodos().Where(r => r.Status == StatusRevista.Disponivel).ToList();

        CadastrarEmprestimoViewModel cadastrarVm = new(string.Empty, string.Empty);

        return View(cadastrarVm);
    }

    [HttpPost]
    public ActionResult Cadastrar(CadastrarEmprestimoViewModel cadastrarVm)
    {
        ViewBag.Amigos = repositorioAmigo.SelecionarTodos();
        ViewBag.Revistas = repositorioRevista.SelecionarTodos().Where(r => r.Status == StatusRevista.Disponivel).ToList();

        if (!ModelState.IsValid)
            return View(cadastrarVm);

        Result resultado = servicoEmprestimo.Cadastrar(mapeador.Map<CadastrarEmprestimoDto>(cadastrarVm));

        if (resultado.IsFailed)
        {
            ModelState.AddModelError(resultado);
            return View(cadastrarVm);
        }

        return RedirectToAction(nameof(Listar));
    }

    [HttpGet]
    public ActionResult Devolver(string id)
    {
        Result<DevolverEmprestimoDto> resultado = servicoEmprestimo.SelecionarPorId(id);

        if (resultado.IsFailed)
            return RedirectToAction(nameof(Listar));

        DevolverEmprestimoViewModel devolverVm = mapeador.Map<DevolverEmprestimoViewModel>(resultado.Value);

        return View(devolverVm);
    }

    [HttpPost]
    public ActionResult Devolver(DevolverEmprestimoViewModel devolverVm)
    {
        Result resultado = servicoEmprestimo.Devolver(devolverVm.Id, DateTime.Now);

        if (resultado.IsFailed)
        {
            ModelState.AddModelError(resultado);
            return View(devolverVm);
        }

        return RedirectToAction(nameof(Listar));
    }
}
