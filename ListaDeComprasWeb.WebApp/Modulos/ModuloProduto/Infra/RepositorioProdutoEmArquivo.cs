using ListaDeComprasWeb.WebApp.Compartilhado.Infra.Arquivos;
using ListaDeComprasWeb.WebApp.Modulos.ModuloProduto.Dominio;

namespace ListaDeComprasWeb.WebApp.Modulos.ModuloProduto.Infra;

public class RepositorioProdutoEmArquivo : RepositorioBaseEmArquivo<Produto>, IRepositorioProduto
{
    public RepositorioProdutoEmArquivo(ContextoJson contexto) : base(contexto) { }

    protected override List<Produto> CarregarRegistros()
    {
        return contexto.Produtos;
    }
}
