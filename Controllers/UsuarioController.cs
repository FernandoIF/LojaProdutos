﻿using AutoMapper;
using LojaProdutos.Dto.Endereco;
using LojaProdutos.Dto.Usuario;
using LojaProdutos.Filtros;
using LojaProdutos.Services.Usuario;
using Microsoft.AspNetCore.Mvc;

namespace LojaProdutos.Controllers
{
    [UsuarioLogado]
    [UsuarioLogadoAdm]
    public class UsuarioController : Controller
    {
        private readonly IUsuarioInterface _usuarioInterface;
        private readonly IMapper _mapper;

        public UsuarioController(IUsuarioInterface usuarioInterface, IMapper mapper)
        {
            _usuarioInterface = usuarioInterface;
            _mapper = mapper;
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

        public async Task<IActionResult> Editar(int id)
        {
            var usuario = await _usuarioInterface.BuscarUsuarioPorId(id);

            var usuarioEditado = new EditarUsuarioDTO
            {
                Nome = usuario.Nome,
                Cargo = usuario.Cargo,
                Email = usuario.Email,
                Id = id,
                Endereco = _mapper.Map<EditarEnderecoDTO>(usuario.Endereco)
            };

            return View(usuarioEditado);
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

        [HttpPost]
        public async Task<IActionResult> Editar(EditarUsuarioDTO editarUsuarioDTO)
        {
            if (ModelState.IsValid)
            {
                var usuario = await _usuarioInterface.Editar(editarUsuarioDTO);
                TempData["MensagemSucesso"] = "Usuário editado com sucesso!";

                return RedirectToAction("Index");
            }
            else
            {
                TempData["MensagemErro"] = "Verifique os dados informados";
                return View(editarUsuarioDTO);
            }
        }
    }
}
