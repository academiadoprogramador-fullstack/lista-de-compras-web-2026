using ListaDeComprasWeb.WebApp.Compartilhado.Dominio;
using ListaDeComprasWeb.WebApp.Modulos.ModuloProduto.Dominio;

namespace ListaDeComprasWeb.WebApp.Modulos.ModuloListaCompras.Dominio;

public sealed class ItemLista : EntidadeBase<ItemLista>
{
    public ListaCompras ListaCompras { get; set; } = null!;
    public Produto Produto { get; set; } = null!;
    public decimal Quantidade { get; set; }

    public ItemLista() { }

    public ItemLista(ListaCompras listaCompras, Produto produto, decimal quantidade)
    {
        ListaCompras = listaCompras;
        Produto = produto;
        Quantidade = quantidade;
    }

    public decimal CalcularSubtotal()
    {
        return Produto.PrecoAproximado * Quantidade;
    }

    public override List<string> Validar()
    {
        List<string> erros = new List<string>();

        if (ListaCompras == null)
            erros.Add("O campo \"Lista\" deve ser preenchido.");

        if (Produto == null)
            erros.Add("O campo \"Produto\" deve ser preenchido.");

        if (Quantidade <= 0)
            erros.Add("O campo \"Quantidade\" deve conter um valor maior que 0.");

        return erros;
    }

    public override void Atualizar(ItemLista entidadeAtualizada)
    {
        ListaCompras = entidadeAtualizada.ListaCompras;
        Produto = entidadeAtualizada.Produto;
        Quantidade = entidadeAtualizada.Quantidade;
    }
}
