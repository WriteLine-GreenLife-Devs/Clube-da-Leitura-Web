using AutoMapper;
using ClubeDaLeitura.WebApplication.ModuloRevista.Aplicacao;

namespace ClubeDaLeitura.WebApplication.ModuloRevista.Apresentacao;

public class RevistaProfile : Profile
{
    public RevistaProfile()
    {
        CreateMap<ListarRevistasDto, ListarRevistasViewModel>();
        CreateMap<CadastrarRevistaViewModel, CadastrarRevistaDto>();
        CreateMap<EditarRevistaViewModel, EditarRevistaDto>();

        CreateMap<DetalhesRevistaDto, EditarRevistaViewModel>();
        CreateMap<DetalhesRevistaDto, ExcluirRevistaViewModel>();
    }
}
