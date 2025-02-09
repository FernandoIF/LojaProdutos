using LojaProdutos.Dto.Usuario;
using LojaProdutos.Services.Usuario;
using Microsoft.AspNetCore.Mvc;

namespace LojaProdutos.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly IUsuarioInterface _usuarioInterface;

        public UsuarioController(IUsuarioInterface usuarioInterface)
        {
            _usuarioInterface = usuarioInterface;
        }
        public async Task<IActionResult> Index()
        {
            var usuarios = await _usuarioInterface.BuscarUsuarios();
            return View(usuarios);
        }

        public IActionResult Cadastrar()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Cadastrar(CriarUsuarioDTO criarUsuarioDTO)
        {
            if(ModelState.IsValid)
            {
                if(await _usuarioInterface.VerificaSeExisteEmail(criarUsuarioDTO))
                {
                    TempData["MensagemErro"] = "Já existe usuário cadastrado com esse E-mail";
                    return View(criarUsuarioDTO);
                }

                var usuario = await _usuarioInterface.Cadastrar(criarUsuarioDTO);

                TempData["MensagemSucesso"] = "Cadastro Realizado com sucesso!";

                return RedirectToAction("Index");
            }
            else
            {
                TempData["MensagemErro"] = "Verifique os dados informados!";
                return View(criarUsuarioDTO);
            }
        }
    }
}
