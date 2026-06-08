using AutoMapper;
using FluentResults;
using ListaDeComprasWeb.WebApp.Compartilhado.Apresentacao.Extensions;
using ListaDeComprasWeb.WebApp.Modulos.ModuloItemLista.Aplicacao;
using Microsoft.AspNetCore.Mvc;

namespace ListaDeComprasWeb.WebApp.Modulos.ModuloItemLista.Apresentacao;

public class ItemListaController(ServicoItemLista servicoItemLista, IMapper mapeador) : Controller
{
    [HttpGet]
    public ActionResult Listar(Guid listaId)
    {
        Result<DetalhesListaItemDto> resultadoLista = servicoItemLista.SelecionarDetalhesLista(listaId);

        if (resultadoLista.IsFailed)
        {
            TempData.AddErrorMessage(resultadoLista);

            return RedirectToAction("Listar", "ListaCompras");
        }

        List<ListarItensListaDto> dtos = servicoItemLista.SelecionarTodosPorLista(listaId);

        GerenciarItensListaViewModel gerenciarVm = new GerenciarItensListaViewModel(
            mapeador.Map<DetalhesListaItemViewModel>(resultadoLista.Value),
            mapeador.Map<List<ListarItensListaViewModel>>(dtos)
        );

        return View(gerenciarVm);
    }

    [HttpGet]
    public ActionResult Cadastrar(Guid listaId)
    {
        Result<DetalhesListaItemDto> resultadoLista = servicoItemLista.SelecionarDetalhesLista(listaId);

        if (resultadoLista.IsFailed)
        {
            TempData.AddErrorMessage(resultadoLista);

            return RedirectToAction("Listar", "ListaCompras");
        }

        CadastrarItemListaViewModel cadastrarVm = new CadastrarItemListaViewModel(
            listaId,
            Guid.Empty,
            1,
            false,
            SelecionarProdutos()
        );

        return View(cadastrarVm);
    }

    [HttpPost]
    public ActionResult Cadastrar(CadastrarItemListaViewModel cadastrarVm)
    {
        if (!ModelState.IsValid)
            return View(cadastrarVm with { Produtos = SelecionarProdutos() });

        CadastrarItemListaDto dto = mapeador.Map<CadastrarItemListaDto>(cadastrarVm);

        Result resultado = servicoItemLista.Cadastrar(dto);

        if (resultado.IsFailed)
        {
            ModelState.AddModelError(resultado);

            return View(cadastrarVm with { Produtos = SelecionarProdutos() });
        }

        if (cadastrarVm.AdicionarOutro)
            return RedirectToAction(nameof(Cadastrar), new { listaId = cadastrarVm.ListaComprasId });

        return RedirectToAction(nameof(Listar), new { listaId = cadastrarVm.ListaComprasId });
    }

    [HttpGet]
    public ActionResult Excluir(Guid id)
    {
        Result<DetalhesItemListaDto> resultado = servicoItemLista.SelecionarPorId(id);

        if (resultado.IsFailed)
        {
            TempData.AddErrorMessage(resultado);

            return RedirectToAction("Listar", "ListaCompras");
        }

        ExcluirItemListaViewModel excluirVm = mapeador.Map<ExcluirItemListaViewModel>(resultado.Value);

        return View(excluirVm);
    }

    [HttpPost]
    public ActionResult Excluir(ExcluirItemListaViewModel excluirVm)
    {
        Result resultado = servicoItemLista.Excluir(excluirVm.Id);

        if (resultado.IsFailed)
            TempData.AddErrorMessage(resultado);

        return RedirectToAction(nameof(Listar), new { listaId = excluirVm.ListaComprasId });
    }

    private List<OpcaoProdutoViewModel> SelecionarProdutos()
    {
        List<OpcaoProdutoDto> dtos = servicoItemLista.SelecionarProdutos();

        return mapeador.Map<List<OpcaoProdutoViewModel>>(dtos);
    }
}
