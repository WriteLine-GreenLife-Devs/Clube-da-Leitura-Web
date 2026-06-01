using AutoMapper;
using ClubeDaLeitura.WebApplication.ModuloCaixa.Aplicacao;

namespace ClubeDaLeitura.WebApplication.ModuloCaixa.Apresentacao;

public class CaixaProfile : Profile
{
    public CaixaProfile()
    {
        CreateMap<ListarCaixasDto, ListarCaixasViewModel>();
        CreateMap<CadastrarCaixaViewModel, CadastrarCaixaDto>();
        CreateMap<EditarCaixaViewModel, EditarCaixaDto>();

        CreateMap<DetalhesCaixaDto, EditarCaixaViewModel>();
        CreateMap<DetalhesCaixaDto, ExcluirCaixaViewModel>();
    }
}
