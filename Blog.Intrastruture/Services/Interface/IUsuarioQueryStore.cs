using Blog.Contracts.Dto;

namespace Blog.Intrastruture.Services.Interface
{
    public interface IUsuarioQueryStore
    {
        Task<UsuarioDto?> AutenticarUsuarioAsync(string login, string senha);
        Task<List<UsuarioDto>> ObterUsuariosAsync(string? nome, string? login);
        Task<UsuarioDto?> ObterUsuarioPorIdAsync(int id);
    }
}
