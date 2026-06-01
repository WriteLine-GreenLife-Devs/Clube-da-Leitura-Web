using System.Text.Json;
using System.Text.Json.Serialization;
using ClubeDaLeitura.WebApplication.ModuloAmigo.Dominio;
using ClubeDaLeitura.WebApplication.ModuloCaixa.Dominio;
using ClubeDaLeitura.WebApplication.ModuloEmprestimo.Dominio;
using ClubeDaLeitura.WebApplication.ModuloRevista.Dominio;

namespace ClubeDaLeitura.WebApplication.Compartilhado.Infraestrutura.Arquivos;

public sealed class Serializable
{
    #region Listas dos Modulos
    public List<Caixa> Caixas { get; set; } = new List<Caixa>();
    public List<Amigo> Amigos { get; set; } = new List<Amigo>();
    public List<Revista> Revistas { get; set; } = new List<Revista>();
    public List<Emprestimo> Emprestimos { get; set; } = new List<Emprestimo>();
    
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
        opcoesJson.PropertyNameCaseInsensitive = true;

        Serializable? arquivoSalvo = null;

        try
        {
            arquivoSalvo = JsonSerializer.Deserialize<Serializable>(jsonString, opcoesJson);
        }
        catch (JsonException)
        {
            arquivoSalvo = CarregarFormatoPreservado(jsonString, opcoesJson);
        }

        if (arquivoSalvo == null)
            return;

        #region Carregar Listas dos Modulos
        
        Caixas = arquivoSalvo.Caixas;
        Amigos = arquivoSalvo.Amigos;
        Revistas = arquivoSalvo.Revistas;
        Emprestimos = arquivoSalvo.Emprestimos;

        #endregion
    }

    private static Serializable? CarregarFormatoPreservado(string jsonString, JsonSerializerOptions opcoesJson)
    {
        using JsonDocument documento = JsonDocument.Parse(jsonString);

        JsonElement raiz = documento.RootElement;
        if (raiz.ValueKind != JsonValueKind.Object)
            return null;

        return new Serializable
        {
            Caixas = DeserializarLista<Caixa>(raiz, "caixas", opcoesJson),
            Amigos = DeserializarLista<Amigo>(raiz, "amigos", opcoesJson),
            Revistas = DeserializarLista<Revista>(raiz, "revistas", opcoesJson),
            Emprestimos = DeserializarLista<Emprestimo>(raiz, "emprestimos", opcoesJson)
        };
    }

    private static List<T> DeserializarLista<T>(JsonElement raiz, string propriedade, JsonSerializerOptions opcoesJson)
    {
        if (!raiz.TryGetProperty(propriedade, out JsonElement elemento))
            return new List<T>();

        if (elemento.ValueKind == JsonValueKind.Object && elemento.TryGetProperty("$values", out JsonElement valores))
        {
            return JsonSerializer.Deserialize<List<T>>(valores.GetRawText(), opcoesJson) ?? new List<T>();
        }

        if (elemento.ValueKind == JsonValueKind.Array)
        {
            return JsonSerializer.Deserialize<List<T>>(elemento.GetRawText(), opcoesJson) ?? new List<T>();
        }

        return new List<T>();
    }
}

