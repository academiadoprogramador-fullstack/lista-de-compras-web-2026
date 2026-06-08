using FluentResults;
using ListaDeComprasWeb.WebApp.Modulos.ModuloItemLista.Dominio;
using ListaDeComprasWeb.WebApp.Modulos.ModuloListaCompras.Dominio;
using ListaDeComprasWeb.WebApp.Modulos.ModuloProduto.Dominio;

namespace ListaDeComprasWeb.WebApp.Modulos.ModuloItemLista.Aplicacao;

public class ServicoItemLista
{
    private readonly IRepositorioItemLista repositorioItemLista;
    private readonly IRepositorioListaCompras repositorioListaCompras;
    private readonly IRepositorioProduto repositorioProduto;

    public ServicoItemLista(
        IRepositorioItemLista repositorioItemLista,
        IRepositorioListaCompras repositorioListaCompras,
        IRepositorioProduto repositorioProduto
    )
    {
        this.repositorioItemLista = repositorioItemLista;
        this.repositorioListaCompras = repositorioListaCompras;
        this.repositorioProduto = repositorioProduto;
    }

    public Result Cadastrar(CadastrarItemListaDto dto)
    {
        Result<(ListaCompras Lista, Produto Produto)> resultadoRelacionamentos =
            SelecionarRelacionamentos(dto.ListaComprasId, dto.ProdutoId);

        if (resultadoRelacionamentos.IsFailed)
            return Result.Fail(resultadoRelacionamentos.Errors);

        if (ExisteProdutoNaLista(dto.ListaComprasId, dto.ProdutoId))
            return Falha(nameof(dto.ProdutoId), "Este produto já foi adicionado nesta lista.");

        ItemLista novoItem = new ItemLista(
            resultadoRelacionamentos.Value.Lista,
            resultadoRelacionamentos.Value.Produto,
            dto.Quantidade
        );

        Result resultadoValidacao = ValidarEntidade(novoItem);

        if (resultadoValidacao.IsFailed)
            return resultadoValidacao;

        repositorioItemLista.Cadastrar(novoItem);

        return Result.Ok();
    }

    public Result Excluir(Guid id)
    {
        ItemLista? item = repositorioItemLista.SelecionarPorId(id);

        if (item == null)
            return Result.Fail("Item da lista não encontrado.");

        repositorioItemLista.Excluir(id);

        return Result.Ok();
    }

    public List<ListarItensListaDto> SelecionarTodosPorLista(Guid listaComprasId)
    {
        return repositorioItemLista
            .SelecionarTodos()
            .Where(i => i.ListaCompras.Id == listaComprasId)
            .Select(MapearParaListarDto)
            .ToList();
    }

    public Result<DetalhesItemListaDto> SelecionarPorId(Guid id)
    {
        ItemLista? item = repositorioItemLista.SelecionarPorId(id);

        if (item == null)
            return Result.Fail("Item da lista não encontrado.");

        return Result.Ok(MapearParaDetalhesDto(item));
    }

    public Result<DetalhesListaItemDto> SelecionarDetalhesLista(Guid listaComprasId)
    {
        ListaCompras? lista = repositorioListaCompras.SelecionarPorId(listaComprasId);

        if (lista == null)
            return Result.Fail("Lista de compras não encontrada.");

        List<ItemLista> itens = repositorioItemLista
            .SelecionarTodos()
            .Where(i => i.ListaCompras.Id == lista.Id)
            .ToList();

        return Result.Ok(new DetalhesListaItemDto(
            lista.Id,
            lista.Nome,
            lista.Status,
            itens.Count,
            itens.Sum(i => i.CalcularSubtotal())
        ));
    }

    public List<OpcaoProdutoDto> SelecionarProdutos()
    {
        return repositorioProduto
            .SelecionarTodos()
            .Select(p => new OpcaoProdutoDto(
                p.Id,
                p.Nome,
                p.Categoria.Nome,
                p.Categoria.Cor,
                p.UnidadeMedida,
                p.PrecoAproximado
            ))
            .ToList();
    }

    private Result<(ListaCompras Lista, Produto Produto)> SelecionarRelacionamentos(
        Guid listaComprasId,
        Guid produtoId
    )
    {
        ListaCompras? lista = repositorioListaCompras.SelecionarPorId(listaComprasId);

        if (lista == null)
            return Result.Fail(new Error("Selecione uma lista válida.").WithMetadata("Campo", nameof(listaComprasId)));

        Produto? produto = repositorioProduto.SelecionarPorId(produtoId);

        if (produto == null)
            return Result.Fail(new Error("Selecione um produto válido.").WithMetadata("Campo", nameof(produtoId)));

        return Result.Ok((lista, produto));
    }

    private bool ExisteProdutoNaLista(Guid listaComprasId, Guid produtoId)
    {
        return repositorioItemLista
            .SelecionarTodos()
            .Any(i =>
                i.ListaCompras.Id == listaComprasId &&
                i.Produto.Id == produtoId
            );
    }

    private static ListarItensListaDto MapearParaListarDto(ItemLista item)
    {
        return new ListarItensListaDto(
            item.Id,
            item.ListaCompras.Id,
            item.ListaCompras.Nome,
            item.Produto.Id,
            item.Produto.Nome,
            item.Produto.Categoria.Nome,
            item.Produto.Categoria.Cor,
            item.Produto.UnidadeMedida,
            item.Produto.PrecoAproximado,
            item.Quantidade,
            item.CalcularSubtotal()
        );
    }

    private static DetalhesItemListaDto MapearParaDetalhesDto(ItemLista item)
    {
        return new DetalhesItemListaDto(
            item.Id,
            item.ListaCompras.Id,
            item.ListaCompras.Nome,
            item.Produto.Id,
            item.Produto.Nome,
            item.Produto.Categoria.Nome,
            item.Produto.Categoria.Cor,
            item.Produto.UnidadeMedida,
            item.Produto.PrecoAproximado,
            item.Quantidade,
            item.CalcularSubtotal()
        );
    }

    private static Result ValidarEntidade(ItemLista item)
    {
        List<string> erros = item.Validar();

        if (erros.Count == 0)
            return Result.Ok();

        return Result.Fail(new Error(erros.First()).WithMetadata("Campo", string.Empty));
    }

    private static Result Falha(string campo, string mensagem)
    {
        return Result.Fail(new Error(mensagem).WithMetadata("Campo", campo));
    }
}

public record DetalhesListaItemDto(
    Guid Id,
    string Nome,
    StatusListaCompras Status,
    int TotalItens,
    decimal TotalEstimado
);
