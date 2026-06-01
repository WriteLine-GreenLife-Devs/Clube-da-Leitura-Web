using AutoMapper;
using ClubeDaLeitura.WebApplication.Compartilhado.Apresentacao.Extensions;
using ClubeDaLeitura.WebApplication.ModuloAmigo.Aplicacao;
using FluentResults;
using Microsoft.AspNetCore.Mvc;

namespace ClubeDaLeitura.WebApplication.ModuloAmigo.Apresentacao;

public class AmigoController(ServicoAmigo servicoAmigo, IMapper mapeador) : Controller
{
    [HttpGet]
    public ActionResult Listar()
    {
        List<ListarAmigosDto> dtos = servicoAmigo.SelecionarTodos();

        List<ListarAmigosViewModel> listarVms = mapeador.Map<List<ListarAmigosViewModel>>(dtos);

        return View(listarVms);
    }

    [HttpGet]
    public ActionResult Cadastrar()
    {
        CadastrarAmigoViewModel cadastrarVm = new(
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

        Result resultado = servicoAmigo.Cadastrar(mapeador.Map<CadastrarAmigoDto>(cadastrarVm));

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
        Result<DetalhesAmigoDto> resultado = servicoAmigo.SelecionarPorId(id);

        if (resultado.IsFailed)
            return RedirectToAction(nameof(Listar));

        EditarAmigoViewModel editarVm = mapeador.Map<EditarAmigoViewModel>(resultado.Value);

        return View(editarVm);
    }

    [HttpPost]
    public ActionResult Editar(EditarAmigoViewModel editarVm)
    {
        if (!ModelState.IsValid)
            return View(editarVm);

        Result resultado = servicoAmigo.Editar(mapeador.Map<EditarAmigoDto>(editarVm));

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
        Result<DetalhesAmigoDto> resultado = servicoAmigo.SelecionarPorId(id);

        if (resultado.IsFailed)
            return RedirectToAction(nameof(Listar));

        ExcluirAmigoViewModel excluirVm = mapeador.Map<ExcluirAmigoViewModel>(resultado.Value);

        return View(excluirVm);
    }

    [HttpPost]
    [ActionName("Excluir")]
    public ActionResult ConfirmarExcluir(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
            return RedirectToAction(nameof(Listar));

        Result resultado = servicoAmigo.Excluir(id);

        if (resultado.IsFailed)
        {
            ModelState.AddModelError(resultado);
            return View(new ExcluirAmigoViewModel(id, string.Empty, string.Empty, string.Empty));
        }

        return RedirectToAction(nameof(Listar));
    }
}

