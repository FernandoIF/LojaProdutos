using System.Reflection.Metadata.Ecma335;
using LojaProdutos.Dto.Produto;
using LojaProdutos.Filtros;
using LojaProdutos.Services.Categoria;
using LojaProdutos.Services.Produto;
using Microsoft.AspNetCore.Mvc;

namespace LojaProdutos.Controllers
{
    [UsuarioLogado]
    public class ProdutoController : Controller
    {
        private readonly IProdutoInterface _produtoInterface;
        private readonly ICategoriaInterface _categoriaInterface;
        public ProdutoController(IProdutoInterface produtoInterface, ICategoriaInterface categoriaInterface)
        {
            _produtoInterface = produtoInterface;
            _categoriaInterface = categoriaInterface;
        }
        [UsuarioLogadoAdm]
        public async Task<IActionResult> Index()
        {
            var produtos = await _produtoInterface.BuscarProdutos();

            return View(produtos);
        }

        [UsuarioLogadoAdm]
        public async Task<IActionResult> Cadastrar()
        {
            ViewBag.Categorias = await _categoriaInterface.BuscarCategorias();

            return View();
        }

        [UsuarioLogadoAdm]
        public async Task<IActionResult> Remover(int id)
        {
            var produto = await _produtoInterface.Remover(id);
            return RedirectToAction("Index", "Produto");
        }

        public async Task<IActionResult> Detalhes(int id)
        {
            var produto = await _produtoInterface.BuscarProdutoPorId(id);

            return View(produto);
        }

        [UsuarioLogadoAdm]
        public async Task<IActionResult> Editar(int id)
        {
            var produto = await _produtoInterface.BuscarProdutoPorId(id);

            var editarProdutoDTO = new EditarProdutoDTO
            {
                Nome = produto.Nome,
                Marca = produto.Marca,
                Valor = produto.Valor,
                CategoriaModelId = produto.CategoriaModelId,
                Foto = produto.Foto,
                QuantidadeEstoque = produto.QuantidadeEstoque
            };

            ViewBag.Categorias = await _categoriaInterface.BuscarCategorias();

            return View(editarProdutoDTO);
        }

        [HttpPost]
        [UsuarioLogadoAdm]
        public async Task<IActionResult> Cadastrar(CriarProdutoDTO criarProdutoDTO, IFormFile foto)
        {
            if (ModelState.IsValid)
            {
                var produto = await _produtoInterface.Cadastrar(criarProdutoDTO, foto);
                TempData["MensagemSucesso"] = "Produto cadastrado com sucesso!";

                return RedirectToAction("Index", "Produto");
            }
            else
            {
                ViewBag.Categorias = await _categoriaInterface.BuscarCategorias();
                TempData["MensagemErro"] = "Erro ao cadastrar Produto";

                return View(criarProdutoDTO); 
            }
        }

        [HttpPost]
        [UsuarioLogadoAdm]
        public async Task<IActionResult> Editar(EditarProdutoDTO editarProdutoDTO, IFormFile? foto)
        {
            if (ModelState.IsValid)
            {
                var produto = await _produtoInterface.Editar(editarProdutoDTO, foto);
                TempData["MensagemSucesso"] = "Produto editado com sucesso!";

                return RedirectToAction("Index", "Produto");
            }
            else
            {
                ViewBag.Categorias = await _categoriaInterface.BuscarCategorias();
                TempData["MensagemErro"] = "Erro ao editar Produto";

                return View(editarProdutoDTO);
            }
        }
    }
}
