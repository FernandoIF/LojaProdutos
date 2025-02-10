using LojaProdutos.Dto.LoginUsuario;
using LojaProdutos.Dto.Produto;
using LojaProdutos.Dto.Usuario;
using LojaProdutos.Models;

namespace LojaProdutos.Services.Usuario
{
    public interface IUsuarioInterface
    {
        Task<List<UsuarioModel>> BuscarUsuarios();
        Task<UsuarioModel> BuscarUsuarioPorId(int id);
        Task<bool> VerificaSeExisteEmail(CriarUsuarioDTO criarUsuarioDTO);
        Task<CriarUsuarioDTO> Cadastrar(CriarUsuarioDTO criarUsuarioDTO);
        Task<UsuarioModel> Editar(EditarUsuarioDTO editarUsuarioDTO);
        Task<UsuarioModel> Login(LoginUsuarioDTO loginUsuarioDto);
    }
}
