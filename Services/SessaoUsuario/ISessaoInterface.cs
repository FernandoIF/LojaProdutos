using LojaProdutos.Models;

namespace LojaProdutos.Services.SessaoUsuario
{
    public interface ISessaoInterface
    {
        void CriarSessao(UsuarioModel usuario);
        void RemoverSessao();
        UsuarioModel BuscarSessao();
    }
}
