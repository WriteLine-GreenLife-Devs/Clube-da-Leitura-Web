using System.ComponentModel.DataAnnotations;

namespace ClubeDaLeitura.WebApplication.ModuloRevista.Apresentacao;

public record ListarRevistasViewModel(
    string Id,
    string Titulo,
    int NumeroEdicao,
    DateTime AnoPublicacao,
    string CaixaEtiqueta,
    string Status
);

public record CadastrarRevistaViewModel(
    [Required(ErrorMessage = "O campo \"Título\" deve ser preenchido.")]
    [StringLength(100, ErrorMessage = "O campo \"Título\" deve conter no máximo 100 caracteres.")]
    string Titulo,

    [Range(1, int.MaxValue, ErrorMessage = "O campo \"Número da Edição\" deve conter um valor maior que 0.")]
    int NumeroEdicao,

    [Required(ErrorMessage = "O campo \"Ano de Publicação\" deve ser preenchido.")]
    DateTime AnoPublicacao,

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

    [Required(ErrorMessage = "O campo \"Ano de Publicação\" deve ser preenchido.")]
    DateTime AnoPublicacao,

    [Required(ErrorMessage = "O campo \"Caixa\" deve ser preenchido.")]
    string CaixaId
);

public record ExcluirRevistaViewModel(
    string Id,
    string Titulo,
    int NumeroEdicao,
    DateTime AnoPublicacao,
    string CaixaId
);
