// ClubeDaLeitura.WebApplication.ModuloEmprestimo.Dominio/Emprestimo.cs
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using ClubeDaLeitura.WebApplication.Compartilhado.Dominio;

namespace ClubeDaLeitura.WebApplication.ModuloEmprestimo.Dominio;

public enum StatusEmprestimo
{
    Aberto,
    Concluido,
    Atrasado
}

public sealed class Emprestimo : EntidadeBase<Emprestimo>
{
    public string AmigoId { get; set; } = string.Empty;
    public string RevistaId { get; set; } = string.Empty;
    public DateTime DataEmprestimo { get; set; }
    public DateTime DataDevolucaoPrevista { get; set; }
    public DateTime? DataDevolucaoReal { get; set; }
    public StatusEmprestimo Status { get; set; } = StatusEmprestimo.Aberto;

    public Emprestimo() { }

    public Emprestimo(string amigoId, string revistaId, DateTime dataEmprestimo, int diasPrazo)
    {
        AmigoId = amigoId;
        RevistaId = revistaId;
        DataEmprestimo = dataEmprestimo;
        DataDevolucaoPrevista = dataEmprestimo.AddDays(diasPrazo);
    }

    public override void AtualizarDados(Emprestimo entidadeAtualizada)
    {
        AmigoId = entidadeAtualizada.AmigoId;
        RevistaId = entidadeAtualizada.RevistaId;
        DataEmprestimo = entidadeAtualizada.DataEmprestimo;
        DataDevolucaoPrevista = entidadeAtualizada.DataDevolucaoPrevista;
        DataDevolucaoReal = entidadeAtualizada.DataDevolucaoReal;
        Status = entidadeAtualizada.Status;
    }

    public void RegistrarDevolucao(DateTime dataDevolucao)
    {
        if (Status == StatusEmprestimo.Concluido)
            throw new InvalidOperationException("Empréstimo já concluído.");

        DataDevolucaoReal = dataDevolucao;
        Status = StatusEmprestimo.Concluido;
    }

    public void VerificarEAtualizarAtraso(DateTime referencia)
    {
        if (Status == StatusEmprestimo.Aberto && referencia.Date > DataDevolucaoPrevista.Date)
            Status = StatusEmprestimo.Atrasado;
    }

    public override List<string> Validar()
    {
        var erros = new List<string>();

        if (string.IsNullOrWhiteSpace(AmigoId))
            erros.Add("AmigoId é obrigatório.");

        if (string.IsNullOrWhiteSpace(RevistaId))
            erros.Add("RevistaId é obrigatório.");

        if (DataEmprestimo == default)
            erros.Add("Data de empréstimo inválida.");

        if (DataDevolucaoPrevista <= DataEmprestimo)
            erros.Add("Data de devolução prevista deve ser posterior à data de empréstimo.");

        return erros;
    }
}