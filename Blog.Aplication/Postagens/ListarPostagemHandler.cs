using Blog.Aplication.Postagens.Request;
using Blog.Contracts.Dto;
using Blog.Intrastruture.Services.EntitiesService.BaseEntity;
using Blog.Intrastruture.Services.Interface;
using MediatR;

namespace Blog.Aplication.Postagens
{
    public class ListarPostagemHandler : IRequestHandler<ListarPostagemRequest, Response<List<PostagemDto>>>
    {
        private readonly IPostagemQueryStore _queryStore;

        public ListarPostagemHandler(IPostagemQueryStore queryStore)
        {
            _queryStore = queryStore;
        }

        public async Task<Response<List<PostagemDto>>> Handle(ListarPostagemRequest request, CancellationToken cancellationToken)
        {
            var postagens = await _queryStore.ObterPostagensAsync(
                request.Titulo,
                request.Autor,
                request.IdPostagem,
                request.IdUsuario);

            return new Response<List<PostagemDto>>
            {
                Data = postagens,
                Success = true
            };
        }
    }
}
