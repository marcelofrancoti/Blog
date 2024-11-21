using Blog.API.Hubs;
using Blog.Aplication.Postagens.Interface;
using Blog.Aplication.Postagens.Request;
using Blog.Contracts.Enum;
using Blog.Intrastruture.Services.EntitiesService.BaseEntity;
using Blog.Intrastruture.Services.Interface;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace Blog.Aplication.Postagens
{
    public class ExcluirPostagemHandler : IRequestHandler<ExcluirPostagemRequest, Response<string>>
    {
        private readonly IUsuarioQueryStore _usuarioQueryStore;
        private readonly IPostagemCommandStore _commandStore;
        private readonly IPostagemQueryStore _postagemQueryStore;
        private readonly IHubContext<PostagemHub> _hubContext;
        public ExcluirPostagemHandler(IPostagemCommandStore commandStore, IUsuarioQueryStore usuarioQueryStore, IPostagemQueryStore postagemQueryStore, IHubContext<PostagemHub> hubContext)
        {
            _commandStore = commandStore;
            _usuarioQueryStore = usuarioQueryStore;
            _postagemQueryStore = postagemQueryStore;
            _hubContext = hubContext;
        }

        public async Task<Response<string>> Handle(ExcluirPostagemRequest request, CancellationToken cancellationToken)
        {
            var verificaUsuarioTemPermissao = await _usuarioQueryStore.ObterUsuarioPorIdAsync(request.IdUsuario);
            if (!verificaUsuarioTemPermissao.TipoUsuario.Equals(TipoUsuario.adm))
            {
                return new Response<string> { Success = false, Message = "Erro úsuario não tem permissão para excluir postagem." };
            }

            var validaSeOpostDoUsuario = await _postagemQueryStore.ObterPostagemPorIdAsync(request.IdPostagem);

            if (validaSeOpostDoUsuario == null )
            {
                return new Response<string> { Success = false, Message = "Erro ao excluir postagem. Postagem Inexistente." };
            }

            if (validaSeOpostDoUsuario.IdPostagem != request.IdPostagem || validaSeOpostDoUsuario.IdUsuario != request.IdUsuario)
            {
                return new Response<string> { Success = false, Message = "Erro ao excluir postagem. Essa postagem não pertence a esse usuario" };
            }

            var success = await _commandStore.ExcluirPostagemAsync(request.IdPostagem);
   
            await _hubContext.Clients.All.SendAsync("ExcluindoPostagem", $"Postagem excluida: {validaSeOpostDoUsuario.Titulo}");
            
            return success
                ? new Response<string> { Success = true, Message = "Postagem excluída com sucesso." }
                : new Response<string> { Success = false, Message = "Erro ao excluir postagem." };
        }
    }
}
