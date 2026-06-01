namespace ClubeDaLeitura.WebApplication.ModuloEmprestimo.Aplicacao;

public record ListarEmprestimosDto(
    string Id,
    string AmigoNome,
    string RevistaTitulo,
    DateTime DataEmprestimo,
    DateTime DataDevolucaoPrevista,
    DateTime? DataDevolucaoReal,
    string Status
);

public record CadastrarEmprestimoDto(
    string AmigoId,
    string RevistaId
);

public record DevolverEmprestimoDto(
    string Id,
    string AmigoNome,
    string RevistaTitulo,
    DateTime DataEmprestimo,
    DateTime DataDevolucaoPrevista,
    string Status
);
