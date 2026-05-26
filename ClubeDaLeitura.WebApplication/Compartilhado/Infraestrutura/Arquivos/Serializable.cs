using System.Text.Json;
using System.Text.Json.Serialization;

namespace ClubeDaLeitura.WebApplication.Compartilhado.Infraestrutura.Arquivos;

public sealed class Serializable
{
    #region Listas dos Modulos
    //Exemplo: public List<Caixa> Caixas { get; set; } = new List<Caixa>();
    #endregion
    private readonly string caminhoArquivo;

    public Serializable()
    {
        string caminhoAppData = Environment
            .GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

        string caminhoDiretorio = Path.Combine(caminhoAppData, "ClubeDaLeituraWeb");

        Directory.CreateDirectory(caminhoDiretorio);

        caminhoArquivo = Path.Combine(caminhoDiretorio, "dados.json");
    }

    public void Salvar()
    {
        JsonSerializerOptions opcoesJson = new JsonSerializerOptions();
        opcoesJson.WriteIndented = true;
        opcoesJson.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        opcoesJson.ReferenceHandler = ReferenceHandler.Preserve;

        string jsonString = JsonSerializer.Serialize(this, opcoesJson);

        File.WriteAllText(caminhoArquivo, jsonString);
    }

    public void Carregar()
    {
        if (!File.Exists(caminhoArquivo))
            return;

        string jsonString = File.ReadAllText(caminhoArquivo);

        JsonSerializerOptions opcoesJson = new JsonSerializerOptions();
        opcoesJson.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        opcoesJson.ReferenceHandler = ReferenceHandler.Preserve;

        Serializable? arquivoSalvo = JsonSerializer
            .Deserialize<Serializable>(jsonString, opcoesJson);

        if (arquivoSalvo == null)
            return;

        #region Carregar Listas dos Modulos
        // Exemplo: Caixas = arquivoSalvo.Caixas;
        #endregion
    }
}
