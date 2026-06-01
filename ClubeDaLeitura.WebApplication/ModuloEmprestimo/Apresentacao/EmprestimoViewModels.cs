using System;
using System.ComponentModel.DataAnnotations;

namespace ClubeDaLeitura.WebApplication.ModuloEmprestimo.Apresentacao;

public record ListarEmprestimosViewModel(
    string Id,
    string AmigoNome,
    string RevistaTitulo,
    DateTime DataEmprestimo,
    DateTime DataDevolucaoPrevista,
    DateTime? DataDevolucaoReal,
    string Status
);

public record CadastrarEmprestimoViewModel(
    [Required(ErrorMessage = "O campo \"Amigo\" deve ser selecionado.")]
    string AmigoId,

    [Required(ErrorMessage = "O campo \"Revista\" deve ser selecionado.")]
    string RevistaId
);

public record DevolverEmprestimoViewModel(
    string Id,
    string AmigoNome,
    string RevistaTitulo,
    DateTime DataEmprestimo,
    DateTime DataDevolucaoPrevista,
    string Status
);
