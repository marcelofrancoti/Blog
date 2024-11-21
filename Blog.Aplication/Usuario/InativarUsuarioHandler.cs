using MediatR;
using Blog.Intrastruture.Services.EntitiesService.BaseEntity;
using Blog.Aplication.Usuario.Request;
using Blog.Intrastruture.Services.Interface;

namespace Blog.Aplication.Usuario
{
    public class InativarUsuarioHandler : IRequestHandler<InativarUsuarioRequest, Response<string>>
    {
        private readonly IUsuarioCommandStore _commandStore;

        public InativarUsuarioHandler(IUsuarioCommandStore commandStore)
        {
            _commandStore = commandStore;
        }

        public async Task<Response<string>> Handle(InativarUsuarioRequest request, CancellationToken cancellationToken)
        {
            var sucesso = await _commandStore.InativarUsuarioAsync(request.IdUsuario);

            if (!sucesso)
            {
                return new Response<string>
                {
                    Success = false,
                    Message = "Erro ao inativar o usuário. Verifique se o usuário existe."
                };
            }

            return new Response<string>
            {
                Success = true,
                Message = "Usuário inativado com sucesso."
            };
        }
    }
}
