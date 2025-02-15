﻿using AutoMapper;
using DocumentFormat.OpenXml.InkML;
using LojaProdutos.Data;
using LojaProdutos.Dto.LoginUsuario;
using LojaProdutos.Dto.Usuario;
using LojaProdutos.Models;
using LojaProdutos.Services.Autenticacao;
using LojaProdutos.Services.SessaoUsuario;
using Microsoft.EntityFrameworkCore;

namespace LojaProdutos.Services.Usuario
{
    public class UsuarioService : IUsuarioInterface
    {
        private readonly DataContext _dataContext;
        private readonly IAutenticacaoInterface _autenticacaoInterface;
        private readonly IMapper _mapper;
        private readonly ISessaoInterface _sessaoInterface;

        public UsuarioService(DataContext dataContext, 
            IAutenticacaoInterface autenticacaoInterface,
            IMapper mapper, 
            ISessaoInterface sessaoInterface)
        {
            _dataContext = dataContext;
            _autenticacaoInterface = autenticacaoInterface;
            _mapper = mapper;
            _sessaoInterface = sessaoInterface;
        }
        public async Task<UsuarioModel> BuscarUsuarioPorId(int id)
        {
            try
            {
                var usuario = await _dataContext.Usuario.Include(e => e.Endereco)
                                                        .FirstOrDefaultAsync(u => u.Id == id);
                return usuario;
            }
            catch (Exception ex) 
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<UsuarioModel>> BuscarUsuarios()
        {
            try
            {
                return await _dataContext.Usuario.Include(e => e.Endereco).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<CriarUsuarioDTO> Cadastrar(CriarUsuarioDTO criarUsuarioDTO)
        {
            try
            {
                _autenticacaoInterface.CriarSenhaHash(criarUsuarioDTO.Senha, out byte[] senhaHash, out byte[] senhaSalt);

                var usuario = new UsuarioModel
                {
                    Nome = criarUsuarioDTO.Nome,
                    Email = criarUsuarioDTO.Email,
                    Cargo = criarUsuarioDTO.Cargo,
                    SenhaHash = senhaHash,
                    SenhaSalt = senhaSalt
                };

                var endereco = new EnderecoModel
                {
                    Logradouro = criarUsuarioDTO.Logradouro,
                    Numero = criarUsuarioDTO.Numero,
                    Bairro = criarUsuarioDTO.Bairro,
                    Estado = criarUsuarioDTO.Estado,
                    Complemento = criarUsuarioDTO.Complemento,
                    CEP = criarUsuarioDTO.CEP,
                    Usuario = usuario
                };

                usuario.Endereco = endereco;

                _dataContext.Add(usuario);
                await _dataContext.SaveChangesAsync();

                return criarUsuarioDTO;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<UsuarioModel> Editar(EditarUsuarioDTO editarUsuarioDTO)
        {
            try
            {
                var usuarioBanco = await _dataContext.Usuario.Include(e => e.Endereco)
                                                             .FirstOrDefaultAsync(u => u.Id == editarUsuarioDTO.Id);
                usuarioBanco.Nome = editarUsuarioDTO.Nome;
                usuarioBanco.Cargo = editarUsuarioDTO.Cargo;
                usuarioBanco.Email = editarUsuarioDTO.Email;
                usuarioBanco.DataAlteracao = DateTime.Now;
                usuarioBanco.Endereco = _mapper.Map<EnderecoModel>(editarUsuarioDTO.Endereco);

                _dataContext.Update(usuarioBanco);
                await _dataContext.SaveChangesAsync();

                return usuarioBanco;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<UsuarioModel> Login(LoginUsuarioDTO loginUsuarioDto)
        {
            try
            {

                var usuarioBanco = await _dataContext.Usuario.FirstOrDefaultAsync(x => x.Email == loginUsuarioDto.Email);

                if (usuarioBanco == null)
                {
                    return null;
                }

                if (!_autenticacaoInterface.VerificaLogin(loginUsuarioDto.Senha, usuarioBanco.SenhaHash, usuarioBanco.SenhaSalt))
                {
                    return null;
                }

                //Criar Sessao
                _sessaoInterface.CriarSessao(usuarioBanco);

                return usuarioBanco;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> VerificaSeExisteEmail(CriarUsuarioDTO criarUsuarioDTO)
        {
            try
            {
                var usuario = await _dataContext.Usuario.FirstOrDefaultAsync(x => x.Email == criarUsuarioDTO.Email);

                if(usuario == null)
                {
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
