using AutoMapper;
using FluentResults;
using ListaDeComprasWeb.WebApp.Compartilhado.Apresentacao.Extensions;
using ListaDeComprasWeb.WebApp.Modulos.ModuloListaCompras.Aplicacao;
using ListaDeComprasWeb.WebApp.Modulos.ModuloListaCompras.Dominio;
using Microsoft.AspNetCore.Mvc;

namespace ListaDeComprasWeb.WebApp.Modulos.ModuloListaCompras.Apresentacao;

public class ListaComprasController(ServicoListaCompras servicoListaCompras, IMapper mapeador) : Controller
{
    [HttpGet]
    public ActionResult Listar()
    {
        List<ListarListasComprasDto> dtos = servicoListaCompras.SelecionarTodos();
        List<ListarListasComprasViewModel> listarVms = mapeador.Map<List<ListarListasComprasViewModel>>(dtos);

        return View(listarVms);
    }

    [HttpGet]
    public ActionResult Cadastrar()
    {
        CadastrarListaComprasViewModel cadastrarVm = new CadastrarListaComprasViewModel(string.Empty);

        return View(cadastrarVm);
    }

    [HttpPost]
    public ActionResult Cadastrar(CadastrarListaComprasViewModel cadastrarVm)
    {
        if (!ModelState.IsValid)
            return View(cadastrarVm);

        CadastrarListaComprasDto dto = mapeador.Map<CadastrarListaComprasDto>(cadastrarVm);

        Result resultado = servicoListaCompras.Cadastrar(dto);

        if (resultado.IsFailed)
        {
            ModelState.AddModelError(resultado);

            return View(cadastrarVm);
        }

        return RedirectToAction(nameof(Listar));
    }

    [HttpGet]
    public ActionResult Editar(Guid id)
    {
        Result<DetalhesListaComprasDto> resultado = servicoListaCompras.SelecionarPorId(id);

        if (resultado.IsFailed)
        {
            TempData.AddErrorMessage(resultado);

            return RedirectToAction(nameof(Listar));
        }

        EditarListaComprasViewModel editarVm = mapeador.Map<EditarListaComprasViewModel>(resultado.Value);

        return View(editarVm);
    }

    [HttpPost]
    public ActionResult Editar(EditarListaComprasViewModel editarVm)
    {
        if (!ModelState.IsValid)
            return View(editarVm);

        EditarListaComprasDto dto = mapeador.Map<EditarListaComprasDto>(editarVm);

        Result resultado = servicoListaCompras.Editar(dto);

        if (resultado.IsFailed)
        {
            ModelState.AddModelError(resultado);

            return View(editarVm);
        }

        return RedirectToAction(nameof(Listar));
    }

    [HttpGet]
    public ActionResult Excluir(Guid id)
    {
        Result<DetalhesListaComprasDto> resultado = servicoListaCompras.SelecionarPorId(id);

        if (resultado.IsFailed)
        {
            TempData.AddErrorMessage(resultado);

            return RedirectToAction(nameof(Listar));
        }

        ExcluirListaComprasViewModel excluirVm = mapeador.Map<ExcluirListaComprasViewModel>(resultado.Value);

        return View(excluirVm);
    }

    [HttpPost]
    public ActionResult Excluir(ExcluirListaComprasViewModel excluirVm)
    {
        Result resultado = servicoListaCompras.Excluir(excluirVm.Id);

        if (resultado.IsFailed)
            TempData.AddErrorMessage(resultado);

        return RedirectToAction(nameof(Listar));
    }

    [HttpPost]
    public ActionResult Concluir(Guid id)
    {
        Result<DetalhesListaComprasDto> resultadoDetalhes = servicoListaCompras.SelecionarPorId(id);

        if (resultadoDetalhes.IsFailed)
        {
            TempData.AddErrorMessage(resultadoDetalhes);

            return RedirectToAction(nameof(Listar));
        }

        DetalhesListaComprasDto detalhes = resultadoDetalhes.Value;

        Result resultado = servicoListaCompras.Editar(new EditarListaComprasDto(
            detalhes.Id,
            detalhes.Nome,
            StatusListaCompras.Concluida
        ));

        if (resultado.IsFailed)
            TempData.AddErrorMessage(resultado);

        return RedirectToAction(nameof(Listar));
    }
}
