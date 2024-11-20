using Blog.Aplication.Usuario.Request;
using Blog.Contracts.Dto;
using Blog.Intrastruture.Services.EntitiesService.BaseEntity;
using Blog.Intrastruture.Services.Interface;
using MediatR;

namespace Blog.Aplication.Usuario
{
    public class AutenticarUsuarioHandler : IRequestHandler<AutenticarUsuarioRequest, Response<UsuarioDto>>
    {
        private readonly IUsuarioQueryStore _queryStore;

        public AutenticarUsuarioHandler(IUsuarioQueryStore queryStore)
        {
            _queryStore = queryStore;
        }

        public async Task<Response<UsuarioDto>> Handle(AutenticarUsuarioRequest request, CancellationToken cancellationToken)
        {
            var usuario = await _queryStore.AutenticarUsuarioAsync(request.Login, request.Senha);

            if (usuario == null)
            {
                return new Response<UsuarioDto>
                {
                    Success = false,
                    Message = "Login ou senha inválidos."
                };
            }

            return new Response<UsuarioDto>
            {
                Success = true,
                Data = usuario
            };
        }
    }
}
