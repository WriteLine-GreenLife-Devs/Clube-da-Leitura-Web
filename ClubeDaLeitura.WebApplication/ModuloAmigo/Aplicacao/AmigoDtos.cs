namespace ClubeDaLeitura.WebApplication.ModuloAmigo.Aplicacao;

public record ListarAmigosDto(
    string Id,
    string Nome,
    string NomeResponsavel,
    string Telefone
);

public record CadastrarAmigoDto(
    string Nome,
    string NomeResponsavel,
    string Telefone
);

public record EditarAmigoDto(
    string Id,
    string Nome,
    string NomeResponsavel,
    string Telefone
);

public record DetalhesAmigoDto(
    string Id,
    string Nome,
    string NomeResponsavel,
    string Telefone
);
