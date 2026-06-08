using ListaDeComprasWeb.WebApp.Compartilhado.Infra.Arquivos;
using ListaDeComprasWeb.WebApp.Modulos.ModuloListaCompras.Dominio;

namespace ListaDeComprasWeb.WebApp.Modulos.ModuloListaCompras.Infra;

public class RepositorioListaComprasEmArquivo : RepositorioBaseEmArquivo<ListaCompras>, IRepositorioListaCompras
{
    public RepositorioListaComprasEmArquivo(ContextoJson contexto) : base(contexto) { }

    protected override List<ListaCompras> CarregarRegistros()
    {
        return contexto.ListasCompras;
    }
}
