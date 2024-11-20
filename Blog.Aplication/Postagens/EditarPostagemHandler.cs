using Blog.Aplication.Postagens.Interface;
using Blog.Aplication.Postagens.Request;
using Blog.Intrastruture.Services.EntitiesService.BaseEntity;
using Blog.Intrastruture.Services.Interface;
using MediatR;

namespace Blog.Aplication.Postagens
{
    public class EditarPostagemHandler : IRequestHandler<EditarPostagemRequest, Response<string>>
    {
        private readonly IPostagemCommandStore _commandStore;
        private readonly IUsuarioQueryStore _commandUsuarioStore;
        private readonly IPostagemQueryStore _postagemQueryStore;
        public EditarPostagemHandler(IPostagemCommandStore commandStore, IUsuarioQueryStore usuarioQueryStore, IPostagemQueryStore postagemQueryStore)
        {
            _commandStore = commandStore;
            _commandUsuarioStore = usuarioQueryStore;
            _postagemQueryStore = postagemQueryStore;
        }

        public async Task<Response<string>> Handle(EditarPostagemRequest request, CancellationToken cancellationToken)
        {

            if (string.IsNullOrEmpty(request.Titulo) && string.IsNullOrEmpty(request.Conteudo))
            {
                return new Response<string>
                {
                    Success = false,
                    Message = "Título ou conteúdo devem ser fornecidos para editar a postagem."
                };
            }

            var validaUsuario = await _commandUsuarioStore.ObterUsuarioPorIdAsync(request.IdUsuario);
     
            if (validaUsuario == null)
            {
                return new Response<string>
                {
                    Success = false,
                    Message = "Erro ao editar a postagem. Usuario não existe."
                };
            }

            var validaPostagem = await _postagemQueryStore.ObterPostagemPorIdAsync(request.IdPostagem);

            if (validaPostagem == null)
            {
                return new Response<string>
                {
                    Success = false,
                    Message = "Erro ao editar a postagem. Post não existe."
                };
            }

            if (!validaPostagem.IdUsuario.Equals(request.IdUsuario))
            {
                return new Response<string>
                {
                    Success = false,
                    Message = "Erro ao editar a postagem. Esse post não é desse Usuario, por favor alterar um post que seja seu."
                };
            }

            var sucesso = await _commandStore.EditarPostagemAsync(request.IdPostagem, request.Titulo, request.Conteudo);

            if (!sucesso)
            {
                return new Response<string>
                {
                    Success = false,
                    Message = "Erro ao editar a postagem. Verifique se a postagem existe."
                };
            }

            return new Response<string>
            {
                Success = true,
                Message = "Postagem editada com sucesso."
            };
        }
    }
}
