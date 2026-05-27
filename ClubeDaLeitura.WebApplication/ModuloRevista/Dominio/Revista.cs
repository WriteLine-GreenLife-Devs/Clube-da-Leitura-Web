using System;
using System.Collections.Generic;
using ClubeDaLeitura.WebApplication.Compartilhado.Dominio;
using ClubeDaLeitura.WebApplication.Compartilhado.Infraestrutura.Arquivos;

namespace ClubeDaLeitura.WebApplication.ModuloRevista.Dominio;

public enum StatusRevista
{
    Disponivel,
    Emprestada,
    Reservada
}

public sealed class Revista : EntidadeBase<Revista>
{
    public string Titulo { get; set; }
    public int NumeroEdicao { get; set; }
    public DateTime AnoPublicacao { get; set; }
    public string CaixaId { get; set; }
    public StatusRevista Status { get; set; } = StatusRevista.Disponivel;

    public Revista(string titulo, int numeroEdicao, DateTime anoPublicacao, string caixaId)
    {
        Titulo = titulo;
        NumeroEdicao = numeroEdicao;
        AnoPublicacao = anoPublicacao;
        CaixaId = caixaId;
    }

    public override void AtualizarDados(Revista entidadeAtualizada)
    {
        Titulo = entidadeAtualizada.Titulo;
        NumeroEdicao = entidadeAtualizada.NumeroEdicao;
        AnoPublicacao = entidadeAtualizada.AnoPublicacao;
        CaixaId = entidadeAtualizada.CaixaId;
        Status = entidadeAtualizada.Status;
    }

    public override List<string> Validar()
    {
        var erros = new List<string>();

        if (string.IsNullOrWhiteSpace(Titulo) || Titulo.Length < 2 || Titulo.Length > 100)
            erros.Add("O título deve ter entre 2 e 100 caracteres.");

        if (NumeroEdicao <= 0)
            erros.Add("O número da edição deve ser positivo.");

        if (AnoPublicacao < new DateTime(1900, 1, 1) || AnoPublicacao > DateTime.Now)
            erros.Add("Data de publicação inválida.");

        if (string.IsNullOrWhiteSpace(CaixaId))
            erros.Add("A revista deve estar vinculada a uma caixa.");

        return erros;
    }
}