using System;
using System.Collections.Generic;

namespace ClubeDaLeitura.WebApplication.ModuloRevista.Dominio;

public enum StatusRevista
{
    Disponivel,
    Emprestada,
    Reservada
}

public sealed class Revista
{
    public int Id { get; set; }
    public string Titulo { get; set; }
    public int NumeroEdicao { get; set; }
    public int AnoPublicacao { get; set; }

    // vínculo obrigatório com Caixa
    public int CaixaId { get; set; }

    public StatusRevista Status { get; set; } = StatusRevista.Disponivel;

    public List<string> Validar()
    {
        var erros = new List<string>();

        if (string.IsNullOrWhiteSpace(Titulo) || Titulo.Length < 2 || Titulo.Length > 100)
            erros.Add("O título deve ter entre 2 e 100 caracteres.");

        if (NumeroEdicao <= 0)
            erros.Add("O número da edição deve ser positivo.");

        if (AnoPublicacao < 1900 || AnoPublicacao > DateTime.Now.Year)
            erros.Add("Ano de publicação inválido.");

        if (CaixaId <= 0)
            erros.Add("A revista deve estar vinculada a uma caixa.");

        return erros;
    }
}