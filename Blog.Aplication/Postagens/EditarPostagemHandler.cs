using Blog.Aplication.Postagens.Interface;
using Blog.Aplication.Postagens.Request;
using Blog.Intrastruture.Services.EntitiesService.BaseEntity;
using MediatR;

namespace Blog.Aplication.Postagens
{
    public class EditarPostagemHandler : IRequestHandler<EditarPostagemRequest, Response<string>>
    {
        private readonly IPostagemCommandStore _commandStore;

        public EditarPostagemHandler(IPostagemCommandStore commandStore)
        {
            _commandStore = commandStore;
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
