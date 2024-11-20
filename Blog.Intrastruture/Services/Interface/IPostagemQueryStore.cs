using Blog.Contracts.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Intrastruture.Services.Interface
{
    public interface IPostagemQueryStore
    {
        Task<List<PostagemDto>> ObterPostagensAsync(string? titulo, string? autor, int? IdPostagem, int? IdUsuario);
        Task<PostagemDto> ObterPostagemPorIdAsync(int IdPostagem);

    }
}
