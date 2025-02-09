using LojaProdutos.Data;
using LojaProdutos.Dto.Usuario;
using LojaProdutos.Models;
using LojaProdutos.Services.Autenticacao;
using Microsoft.EntityFrameworkCore;

namespace LojaProdutos.Services.Usuario
{
    public class UsuarioService : IUsuarioInterface
    {
        private readonly DataContext _dataContext;
        private readonly IAutenticacaoInterface _autenticacaoInterface;

        public UsuarioService(DataContext dataContext, IAutenticacaoInterface autenticacaoInterface)
        {
            _dataContext = dataContext;
            _autenticacaoInterface = autenticacaoInterface;
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
