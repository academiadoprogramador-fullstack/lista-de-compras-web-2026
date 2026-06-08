using ListaDeComprasWeb.WebApp.Compartilhado.Dominio;

namespace ListaDeComprasWeb.WebApp.Modulos.ModuloListaCompras.Dominio;

public sealed class ListaCompras : EntidadeBase<ListaCompras>
{
    public string Nome { get; set; } = string.Empty;
    public DateTime DataCriacao { get; set; } = DateTime.Now;
    public StatusListaCompras Status { get; set; } = StatusListaCompras.Aberta;

    public ListaCompras() { }

    public ListaCompras(string nome, DateTime dataCriacao, StatusListaCompras status = StatusListaCompras.Aberta)
    {
        Nome = nome;
        DataCriacao = dataCriacao;
        Status = status;
    }

    public override List<string> Validar()
    {
        List<string> erros = new List<string>();

        if (string.IsNullOrWhiteSpace(Nome))
            erros.Add("O campo \"Nome\" deve ser preenchido.");

        else if (Nome.Length < 3 || Nome.Length > 100)
            erros.Add("O campo \"Nome\" deve conter entre 3 e 100 caracteres.");

        return erros;
    }

    public override void Atualizar(ListaCompras entidadeAtualizada)
    {
        Nome = entidadeAtualizada.Nome;
        Status = entidadeAtualizada.Status;
    }
}
