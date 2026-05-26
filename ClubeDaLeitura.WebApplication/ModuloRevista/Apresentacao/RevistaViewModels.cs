using System.ComponentModel.DataAnnotations;

namespace ClubeDaLeituraWeb.WebApp.ModuloRevista.Apresentacao;

public record ListarRevistasViewModel(
    string Id,
    string Titulo,
    int NumeroEdicao,
    int AnoPublicacao,
    string CaixaId,
    string Status
);

public record CadastrarRevistaViewModel(
    [Required(ErrorMessage = "O campo \"Título\" deve ser preenchido.")]
    [StringLength(100, ErrorMessage = "O campo \"Título\" deve conter no máximo 100 caracteres.")]
    string Titulo,

    [Range(1, int.MaxValue, ErrorMessage = "O campo \"Número da Edição\" deve conter um valor maior que 0.")]
    int NumeroEdicao,

    [Range(1900, 2100, ErrorMessage = "O campo \"Ano de Publicação\" deve estar entre 1900 e 2100.")]
    int AnoPublicacao,

    [Required(ErrorMessage = "O campo \"Caixa\" deve ser preenchido.")]
    string CaixaId
);

public record EditarRevistaViewModel(
    string Id,

    [Required(ErrorMessage = "O campo \"Título\" deve ser preenchido.")]
    [StringLength(100, ErrorMessage = "O campo \"Título\" deve conter no máximo 100 caracteres.")]
    string Titulo,

    [Range(1, int.MaxValue, ErrorMessage = "O campo \"Número da Edição\" deve conter um valor maior que 0.")]
    int NumeroEdicao,

    [Range(1900, 2100, ErrorMessage = "O campo \"Ano de Publicação\" deve estar entre 1900 e 2100.")]
    int AnoPublicacao,

    [Required(ErrorMessage = "O campo \"Caixa\" deve ser preenchido.")]
    string CaixaId
);

public record ExcluirRevistaViewModel(
    string Id,
    string Titulo,
    int NumeroEdicao,
    int AnoPublicacao,
    string CaixaId
);