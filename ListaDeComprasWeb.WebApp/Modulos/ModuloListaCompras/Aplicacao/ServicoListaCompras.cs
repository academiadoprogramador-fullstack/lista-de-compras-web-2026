using FluentResults;
using ListaDeComprasWeb.WebApp.Modulos.ModuloListaCompras.Dominio;

namespace ListaDeComprasWeb.WebApp.Modulos.ModuloListaCompras.Aplicacao;

public class ServicoListaCompras
{
    private readonly IRepositorioListaCompras repositorioListaCompras;
    private readonly IRepositorioItemLista repositorioItemLista;

    public ServicoListaCompras(
        IRepositorioListaCompras repositorioListaCompras,
        IRepositorioItemLista repositorioItemLista
    )
    {
        this.repositorioListaCompras = repositorioListaCompras;
        this.repositorioItemLista = repositorioItemLista;
    }

    public Result Cadastrar(CadastrarListaComprasDto dto)
    {
        ListaCompras novaLista = new ListaCompras(dto.Nome, DateTime.Now);

        Result resultadoValidacao = ValidarEntidade(novaLista);

        if (resultadoValidacao.IsFailed)
            return resultadoValidacao;

        repositorioListaCompras.Cadastrar(novaLista);

        return Result.Ok();
    }

    public Result Editar(EditarListaComprasDto dto)
    {
        ListaCompras? lista = repositorioListaCompras.SelecionarPorId(dto.Id);

        if (lista == null)
            return Result.Fail("Lista de compras não encontrada.");

        ListaCompras listaAtualizada = new ListaCompras(dto.Nome, lista.DataCriacao, dto.Status);

        Result resultadoValidacao = ValidarEntidade(listaAtualizada);

        if (resultadoValidacao.IsFailed)
            return resultadoValidacao;

        repositorioListaCompras.Editar(dto.Id, listaAtualizada);

        return Result.Ok();
    }

    public List<ListarListasComprasDto> SelecionarTodos()
    {
        return repositorioListaCompras
            .SelecionarTodos()
            .Select(l =>
            {
                List<ItemLista> itens = SelecionarItensDaLista(l.Id);

                return new ListarListasComprasDto(
                    l.Id,
                    l.Nome,
                    l.DataCriacao,
                    l.Status,
                    itens.Count,
                    itens.Sum(i => i.CalcularSubtotal())
                );
            })
            .ToList();
    }

    public Result<DetalhesListaComprasDto> SelecionarPorId(Guid id)
    {
        ListaCompras? lista = repositorioListaCompras.SelecionarPorId(id);

        if (lista == null)
            return Result.Fail("Lista de compras não encontrada.");

        List<ItemLista> itens = SelecionarItensDaLista(lista.Id);

        return Result.Ok(new DetalhesListaComprasDto(
            lista.Id,
            lista.Nome,
            lista.DataCriacao,
            lista.Status,
            itens.Count,
            itens.Sum(i => i.CalcularSubtotal())
        ));
    }

    private List<ItemLista> SelecionarItensDaLista(Guid listaId)
    {
        return repositorioItemLista
            .Filtrar(i => i.ListaCompras.Id == listaId);
    }

    private static Result ValidarEntidade(ListaCompras lista)
    {
        List<string> erros = lista.Validar();

        if (erros.Count == 0)
            return Result.Ok();

        return Result.Fail(new Error(erros.First()).WithMetadata("Campo", string.Empty));
    }
}
