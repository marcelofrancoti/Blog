using Blog.Domain.Entities;

namespace Blog.Intrastruture.Services.Interface
{
    public interface IUsuarioCommandStore
    {
        Task<int> CriarUsuarioAsync(Usuario usuario);
        Task<bool> AlterarSenhaAsync(int idUsuario, string novaSenha);
        Task<bool> AlterarUsuarioAsync(int IdUsuario, int TipoUsuario, string nome);
        Task<bool> InativarUsuarioAsync(int idUsuario);
    }
}
