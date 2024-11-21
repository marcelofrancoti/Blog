using Blog.Contracts.Dto;

namespace Blog.Intrastruture.Services.Interface
{
    public interface IPostagemQueryStore
    {
        Task<List<PostagemDto>> ObterPostagensAsync(string? titulo, string? autor, int? IdPostagem, int? IdUsuario);
        Task<PostagemDto> ObterPostagemPorIdAsync(int IdPostagem);

    }
}
