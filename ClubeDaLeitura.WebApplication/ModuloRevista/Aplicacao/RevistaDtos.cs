namespace ClubeDaLeitura.WebApplication.ModuloRevista.Aplicacao;

public record ListarRevistasDto(
    string Id,
    string Titulo,
    int NumeroEdicao,
    DateTime AnoPublicacao,
    string CaixaEtiqueta,
    string Status
);

public record CadastrarRevistaDto(
    string Titulo,
    int NumeroEdicao,
    DateTime AnoPublicacao,
    string CaixaId
);

public record EditarRevistaDto(
    string Id,
    string Titulo,
    int NumeroEdicao,
    DateTime AnoPublicacao,
    string CaixaId
);

public record DetalhesRevistaDto(
    string Id,
    string Titulo,
    int NumeroEdicao,
    DateTime AnoPublicacao,
    string CaixaId,
    string Status
);
