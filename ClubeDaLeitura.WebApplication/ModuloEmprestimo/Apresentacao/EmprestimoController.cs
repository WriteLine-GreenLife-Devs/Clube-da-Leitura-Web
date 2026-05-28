using System;
using System.Collections.Generic;
using System.Linq;
using ClubeDaLeitura.WebApplication.ModuloAmigo.Dominio;
using ClubeDaLeitura.WebApplication.ModuloCaixa.Dominio;
using ClubeDaLeitura.WebApplication.ModuloEmprestimo.Dominio;
using ClubeDaLeitura.WebApplication.ModuloRevista.Dominio;
using ClubeDaLeituraWeb.WebApp.ModuloEmprestimo.Apresentacao;
using Microsoft.AspNetCore.Mvc;

namespace ClubeDaLeitura.WebApplication.ModuloEmprestimo.Apresentacao;

public class EmprestimoController : Controller
{
    private readonly InterfaceRepositorioEmprestimo repositorioEmprestimo;
    private readonly InterfaceRepositorioAmigo repositorioAmigo;
    private readonly InterfaceRepositorioRevista repositorioRevista;
    private readonly InterfaceRepositorioCaixa repositorioCaixa;

    public EmprestimoController(
        InterfaceRepositorioEmprestimo repositorioEmprestimo,
        InterfaceRepositorioAmigo repositorioAmigo,
        InterfaceRepositorioRevista repositorioRevista,
        InterfaceRepositorioCaixa repositorioCaixa)
    {
        this.repositorioEmprestimo = repositorioEmprestimo;
        this.repositorioAmigo = repositorioAmigo;
        this.repositorioRevista = repositorioRevista;
        this.repositorioCaixa = repositorioCaixa;
    }

    [HttpGet]
    public ActionResult Listar(string status, string amigoId)
    {
        List<Emprestimo> emprestimos = repositorioEmprestimo.SelecionarTodos();

        foreach (Emprestimo emprestimo in emprestimos)
        {
            StatusEmprestimo statusAnterior = emprestimo.Status;
            emprestimo.VerificarEAtualizarAtraso(DateTime.Now);

            if (emprestimo.Status != statusAnterior)
                repositorioEmprestimo.Editar(emprestimo.Id, emprestimo);
        }

        if (!string.IsNullOrWhiteSpace(amigoId))
            emprestimos = emprestimos.Where(e => e.AmigoId == amigoId).ToList();

        if (!string.IsNullOrWhiteSpace(status) && Enum.TryParse(status, true, out StatusEmprestimo filtroStatus))
            emprestimos = emprestimos.Where(e => e.Status == filtroStatus).ToList();

        List<ListarEmprestimosViewModel> listarVms = new();

        foreach (Emprestimo emprestimo in emprestimos)
        {
            Amigo? amigo = repositorioAmigo.SelecionarPorId(emprestimo.AmigoId);
            Revista? revista = repositorioRevista.SelecionarPorId(emprestimo.RevistaId);

            listarVms.Add(new ListarEmprestimosViewModel(
                emprestimo.Id,
                amigo?.Nome ?? "Amigo não encontrado",
                revista?.Titulo ?? "Revista não encontrada",
                emprestimo.DataEmprestimo,
                emprestimo.DataDevolucaoPrevista,
                emprestimo.DataDevolucaoReal,
                emprestimo.Status.ToString()
            ));
        }

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

        Amigo? amigo = repositorioAmigo.SelecionarPorId(cadastrarVm.AmigoId);
        Revista? revista = repositorioRevista.SelecionarPorId(cadastrarVm.RevistaId);

        if (amigo == null)
            ModelState.AddModelError(string.Empty, "Amigo não encontrado.");

        if (revista == null)
            ModelState.AddModelError(string.Empty, "Revista não encontrada.");

        if (amigo == null || revista == null)
            return View(cadastrarVm);

        if (repositorioEmprestimo.AmigoTemEmprestimoAtivo(amigo.Id))
            ModelState.AddModelError(string.Empty, "O amigo já possui um empréstimo ativo.");

        if (revista.Status != StatusRevista.Disponivel)
            ModelState.AddModelError(string.Empty, "A revista não está disponível no momento.");

        Caixa? caixa = repositorioCaixa.SelecionarPorId(revista.CaixaId);
        if (caixa == null)
        {
            ModelState.AddModelError(string.Empty, "A caixa da revista não foi encontrada.");
            return View(cadastrarVm);
        }

        if (!ModelState.IsValid)
            return View(cadastrarVm);

        Emprestimo emprestimo = new Emprestimo(amigo.Id, revista.Id, DateTime.Now, caixa.DiasDeEmprestimo);

        List<string> erros = emprestimo.Validar();

        if (erros.Any())
        {
            foreach (string erro in erros)
                ModelState.AddModelError(string.Empty, erro);

            return View(cadastrarVm);
        }

        repositorioEmprestimo.Cadastrar(emprestimo);

        revista.Status = StatusRevista.Emprestada;
        repositorioRevista.Editar(revista.Id, revista);

        return RedirectToAction(nameof(Listar));
    }

    [HttpGet]
    public ActionResult Devolver(string id)
    {
        Emprestimo? emprestimo = repositorioEmprestimo.SelecionarPorId(id);

        if (emprestimo is null || emprestimo.Status == StatusEmprestimo.Concluido)
            return RedirectToAction(nameof(Listar));

        Amigo? amigo = repositorioAmigo.SelecionarPorId(emprestimo.AmigoId);
        Revista? revista = repositorioRevista.SelecionarPorId(emprestimo.RevistaId);

        DevolverEmprestimoViewModel devolverVm = new(
            emprestimo.Id,
            amigo?.Nome ?? "Amigo não encontrado",
            revista?.Titulo ?? "Revista não encontrada",
            emprestimo.DataEmprestimo,
            emprestimo.DataDevolucaoPrevista,
            emprestimo.Status.ToString()
        );

        return View(devolverVm);
    }

    [HttpPost]
    public ActionResult Devolver(DevolverEmprestimoViewModel devolverVm)
    {
        Emprestimo? emprestimo = repositorioEmprestimo.SelecionarPorId(devolverVm.Id);

        if (emprestimo == null)
            return RedirectToAction(nameof(Listar));

        if (emprestimo.Status == StatusEmprestimo.Concluido)
            return RedirectToAction(nameof(Listar));

        emprestimo.RegistrarDevolucao(DateTime.Now);
        repositorioEmprestimo.Editar(emprestimo.Id, emprestimo);

        Revista? revista = repositorioRevista.SelecionarPorId(emprestimo.RevistaId);
        if (revista != null)
        {
            revista.Status = StatusRevista.Disponivel;
            repositorioRevista.Editar(revista.Id, revista);
        }

        return RedirectToAction(nameof(Listar));
    }
}