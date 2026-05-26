using System.ComponentModel.DataAnnotations;

namespace ClubeDaLeituraWeb.WebApp.ModuloAmigo.Apresentacao;
public record ListarAmigosViewModel(
    string Id,
    string Nome,
    string NomeResponsavel,
    string Telefone
);

public record CadastrarAmigoViewModel(
    [Required(ErrorMessage = "O campo \"Nome\" deve ser preenchido.")]
    [StringLength(100, ErrorMessage = "O campo \"Nome\" deve conter no máximo 100 caracteres.")]
    string Nome,

    [Required(ErrorMessage = "O campo \"Nome do Responsável\" deve ser preenchido.")]
    [StringLength(100, ErrorMessage = "O campo \"Nome do Responsável\" deve conter no máximo 100 caracteres.")]
    string NomeResponsavel,

    [Required(ErrorMessage = "O campo \"Telefone\" deve ser preenchido.")]
    [StringLength(14, ErrorMessage = "O campo \"Telefone\" deve conter no máximo 14 caracteres.")]
    string Telefone
);

public record EditarAmigoViewModel(
    string Id,

    [Required(ErrorMessage = "O campo \"Nome\" deve ser preenchido.")]
    [StringLength(100, ErrorMessage = "O campo \"Nome\" deve conter no máximo 100 caracteres.")]
    string Nome,

    [Required(ErrorMessage = "O campo \"Nome do Responsável\" deve ser preenchido.")]
    [StringLength(100, ErrorMessage = "O campo \"Nome do Responsável\" deve conter no máximo 100 caracteres.")]
    string NomeResponsavel,

    [Required(ErrorMessage = "O campo \"Telefone\" deve ser preenchido.")]
    [StringLength(14, ErrorMessage = "O campo \"Telefone\" deve conter no máximo 14 caracteres.")]
    string Telefone
);

public record ExcluirAmigoViewModel(
    string Id,
    string Nome,
    string NomeResponsavel,
    string Telefone
);
