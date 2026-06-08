using System.ComponentModel.DataAnnotations;
using ListaDeComprasWeb.WebApp.Modulos.ModuloListaCompras.Dominio;
using ListaDeComprasWeb.WebApp.Modulos.ModuloProduto.Dominio;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace ListaDeComprasWeb.WebApp.Modulos.ModuloItemLista.Apresentacao;

public record OpcaoProdutoViewModel(
    Guid Id,
    string Nome,
    string CategoriaNome,
    string CategoriaCor,
    UnidadeMedida UnidadeMedida,
    decimal PrecoAproximado
);

public record DetalhesListaItemViewModel(
    Guid Id,
    string Nome,
    StatusListaCompras Status,
    int TotalItens,
    decimal TotalEstimado
);

public record ListarItensListaViewModel(
    Guid Id,
    Guid ListaComprasId,
    string ListaComprasNome,
    Guid ProdutoId,
    string ProdutoNome,
    string CategoriaNome,
    string CategoriaCor,
    UnidadeMedida UnidadeMedida,
    decimal PrecoAproximado,
    decimal Quantidade,
    decimal Subtotal
);

public record CadastrarItemListaViewModel(
    Guid ListaComprasId,

    [Required(ErrorMessage = "O campo \"Produto\" deve ser preenchido.")]
    Guid ProdutoId,

    [Range(0.01, double.MaxValue, ErrorMessage = "O campo \"Quantidade\" deve conter um valor maior que 0.")]
    decimal Quantidade,

    [ValidateNever]
    List<OpcaoProdutoViewModel> Produtos
);

public record ExcluirItemListaViewModel(
    Guid Id,
    Guid ListaComprasId,
    string ListaComprasNome,
    Guid ProdutoId,
    string ProdutoNome,
    string CategoriaNome,
    string CategoriaCor,
    UnidadeMedida UnidadeMedida,
    decimal PrecoAproximado,
    decimal Quantidade,
    decimal Subtotal
);

public record GerenciarItensListaViewModel(
    DetalhesListaItemViewModel Lista,
    List<ListarItensListaViewModel> Itens
);
