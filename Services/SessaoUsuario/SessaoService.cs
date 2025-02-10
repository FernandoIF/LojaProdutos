using LojaProdutos.Models;
using Newtonsoft.Json;

namespace LojaProdutos.Services.SessaoUsuario
{
    public class SessaoService : ISessaoInterface
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public SessaoService(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }
        public UsuarioModel BuscarSessao()
        {
            string sessaoUsuario = _contextAccessor.HttpContext.Session.GetString("usuarioSessao");

            if(string.IsNullOrEmpty(sessaoUsuario))
            {
                return null;
            }

            return JsonConvert.DeserializeObject<UsuarioModel>(sessaoUsuario);
        }

        public void CriarSessao(UsuarioModel usuario)
        {
            string usuarioJson = JsonConvert.SerializeObject(usuario);
            _contextAccessor.HttpContext.Session.SetString("usuarioSessao", usuarioJson);
        }

        public void RemoverSessao()
        {
            _contextAccessor.HttpContext.Session.Remove("usuarioSessao");
        }
    }
}
