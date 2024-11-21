using Blog.Aplication.Usuario.Request;
using Blog.Intrastruture.Services.EntitiesService.BaseEntity;
using Blog.Intrastruture.Services.Interface;
using MediatR;

namespace Blog.Aplication.Usuario
{
    public class AlterarUsuarioHandler : IRequestHandler<AlterarUsuarioRequest, Response<string>>
    {
        private readonly IUsuarioQueryStore _queryStore;
        private readonly IUsuarioCommandStore _commandStore;

        public AlterarUsuarioHandler(IUsuarioCommandStore commandStore, IUsuarioQueryStore queryStore)
        {
            _commandStore = commandStore;
            _queryStore = queryStore;
        }


        public async Task<Response<string>> Handle(AlterarUsuarioRequest request, CancellationToken cancellationToken)
        {
            var sucesso = await _commandStore.AlterarUsuarioAsync(request.IdUsuario, request.TipoUsuario, request.Nome);

            if (!sucesso)
            {
                return new Response<string>
                {
                    Success = false,
                    Message = "Erro ao alterar a senha. Verifique se o usuário existe."
                };
            }

            return new Response<string>
            {
                Success = true,
                Message = "Senha alterada com sucesso."
            };
        }


    }

}
