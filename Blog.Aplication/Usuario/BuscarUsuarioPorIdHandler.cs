using Blog.Aplication.Usuario.Request;
using Blog.Contracts.Dto;
using Blog.Intrastruture.Services.EntitiesService.BaseEntity;
using Blog.Intrastruture.Services.Interface;
using MediatR;

namespace Blog.Aplication.Usuario
{
    public class BuscarUsuarioPorIdHandler : IRequestHandler<BuscarUsuarioPorIdRequest, Response<UsuarioDto>>
    {
        private readonly IUsuarioQueryStore _queryStore;

        public BuscarUsuarioPorIdHandler(IUsuarioQueryStore queryStore)
        {
            _queryStore = queryStore;
        }

        public async Task<Response<UsuarioDto>> Handle(BuscarUsuarioPorIdRequest request, CancellationToken cancellationToken)
        {
            var usuario = await _queryStore.ObterUsuarioPorIdAsync(request.IdUsuario);

            if (usuario == null)
            {
                return new Response<UsuarioDto>
                {
                    Success = false,
                    Message = "Usuário não encontrado."
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
