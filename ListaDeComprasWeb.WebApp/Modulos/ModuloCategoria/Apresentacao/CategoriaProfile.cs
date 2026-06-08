using AutoMapper;
using ListaDeComprasWeb.WebApp.Modulos.ModuloCategoria.Aplicacao;

namespace ListaDeComprasWeb.WebApp.Modulos.ModuloCategoria.Apresentacao;

public class CategoriaProfile : Profile
{
    public CategoriaProfile()
    {
        CreateMap<ListarCategoriasDto, ListarCategoriasViewModel>();
        CreateMap<CadastrarCategoriaViewModel, CadastrarCategoriaDto>();
        CreateMap<EditarCategoriaViewModel, EditarCategoriaDto>();
        CreateMap<DetalhesCategoriaDto, EditarCategoriaViewModel>();
        CreateMap<DetalhesCategoriaDto, ExcluirCategoriaViewModel>();
    }
}
