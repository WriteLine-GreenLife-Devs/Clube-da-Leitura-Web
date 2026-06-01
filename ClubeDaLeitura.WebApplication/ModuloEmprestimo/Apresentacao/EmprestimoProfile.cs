using AutoMapper;
using ClubeDaLeitura.WebApplication.ModuloEmprestimo.Aplicacao;

namespace ClubeDaLeitura.WebApplication.ModuloEmprestimo.Apresentacao;

public class EmprestimoProfile : Profile
{
    public EmprestimoProfile()
    {
        CreateMap<ListarEmprestimosDto, ListarEmprestimosViewModel>();
        CreateMap<CadastrarEmprestimoViewModel, CadastrarEmprestimoDto>();

        CreateMap<DevolverEmprestimoDto, DevolverEmprestimoViewModel>();
    }
}
