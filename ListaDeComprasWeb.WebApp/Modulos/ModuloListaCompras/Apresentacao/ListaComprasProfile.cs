using AutoMapper;
using ListaDeComprasWeb.WebApp.Modulos.ModuloListaCompras.Aplicacao;
using ListaDeComprasWeb.WebApp.Modulos.ModuloListaCompras.Apresentacao;

namespace ListaDeComprasWeb.WebApp.ModuloListaCompras.Apresentacao;

public class ListaComprasProfile : Profile
{
    public ListaComprasProfile()
    {
        CreateMap<ListarListasComprasDto, ListarListasComprasViewModel>();
        CreateMap<CadastrarListaComprasViewModel, CadastrarListaComprasDto>();
        CreateMap<EditarListaComprasViewModel, EditarListaComprasDto>();
        CreateMap<DetalhesListaComprasDto, EditarListaComprasViewModel>();
    }
}
