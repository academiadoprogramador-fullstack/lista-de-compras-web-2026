using AutoMapper;
using ListaDeComprasWeb.WebApp.Modulos.ModuloItemLista.Aplicacao;

namespace ListaDeComprasWeb.WebApp.Modulos.ModuloItemLista.Apresentacao;

public class ItemListaProfile : Profile
{
    public ItemListaProfile()
    {
        CreateMap<OpcaoProdutoDto, OpcaoProdutoViewModel>();
        CreateMap<DetalhesListaItemDto, DetalhesListaItemViewModel>();
        CreateMap<ListarItensListaDto, ListarItensListaViewModel>();
        CreateMap<CadastrarItemListaViewModel, CadastrarItemListaDto>();
        CreateMap<DetalhesItemListaDto, ExcluirItemListaViewModel>();
    }
}
