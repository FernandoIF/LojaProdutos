using LojaProdutos.Services.Estoque;
using Microsoft.AspNetCore.Mvc;

namespace LojaProdutos.Controllers
{
    public class EstoqueController : Controller
    {
        private readonly IEstoqueInterface _estoqueInterface;

        public IActionResult Index()
        {
            var registros = _estoqueInterface.ListagemRegistros();
            return View(registros);
        }

        public EstoqueController(IEstoqueInterface estoqueInterface)
        {
            _estoqueInterface = estoqueInterface;
        }

        [HttpPost]
        public async Task<IActionResult> BaixarEstoque(int id)
        {
            if (ModelState.IsValid)
            {
                var produtoBaixado = await _estoqueInterface.CriarRegistro(id);
                TempData["MensagemSucesso"] = "Compra realizada com sucesso!";

                return RedirectToAction("Index", "Home");
            }
            else
            {
                TempData["MensagemErro"] = "Erro a realizar a compra :(";
                return RedirectToAction("Index", "Home");
            }       
        }
    }
}
