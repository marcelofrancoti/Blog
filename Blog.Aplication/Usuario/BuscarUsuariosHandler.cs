using Blog.Aplication.Usuario.Request;
using Blog.Contracts.Dto;
using Blog.Intrastruture.Services.EntitiesService.BaseEntity;
using Blog.Intrastruture.Services.Interface;
using MediatR;

namespace Blog.Aplication.Usuario
{
    public class BuscarUsuariosHandler : IRequestHandler<BuscarUsuariosRequest, Response<List<UsuarioDto>>>
    {
        private readonly IUsuarioQueryStore _queryStore;

        public BuscarUsuariosHandler(IUsuarioQueryStore queryStore)
        {
            _queryStore = queryStore;
        }

        public async Task<Response<List<UsuarioDto>>> Handle(BuscarUsuariosRequest request, CancellationToken cancellationToken)
        {
            var usuarios = await _queryStore.ObterUsuariosAsync(request.Nome, request.Login);

            return new Response<List<UsuarioDto>>
            {
                Success = true,
                Data = usuarios
            };
        }
    }
}
