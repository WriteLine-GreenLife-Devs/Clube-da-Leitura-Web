using AutoMapper;
using ClubeDaLeitura.WebApplication.ModuloAmigo.Aplicacao;

namespace ClubeDaLeitura.WebApplication.ModuloAmigo.Apresentacao;

public class AmigoProfile : Profile
{
    public AmigoProfile()
    {
        CreateMap<ListarAmigosDto, ListarAmigosViewModel>();
        CreateMap<CadastrarAmigoViewModel, CadastrarAmigoDto>();
        CreateMap<EditarAmigoViewModel, EditarAmigoDto>();

        CreateMap<DetalhesAmigoDto, EditarAmigoViewModel>();
        CreateMap<DetalhesAmigoDto, ExcluirAmigoViewModel>();
    }
}
