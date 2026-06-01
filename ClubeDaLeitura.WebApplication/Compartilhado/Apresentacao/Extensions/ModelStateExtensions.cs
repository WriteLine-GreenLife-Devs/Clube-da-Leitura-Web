using FluentResults;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ClubeDaLeitura.WebApplication.Compartilhado.Apresentacao.Extensions;

public static class ModelStateExtensions
{
    public static void AddModelError(this ModelStateDictionary modelState, ResultBase result)
    {
        foreach (IError erro in result.Errors)
        {
            string campo = string.Empty;

            try
            {
                if (erro.Metadata != null && erro.Metadata.ContainsKey("Campo") && erro.Metadata["Campo"] is string s)
                    campo = s;
            }
            catch
            {
                campo = string.Empty;
            }

            modelState.AddModelError(campo, erro.Message);
        }
    }
}
