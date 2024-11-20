using Blog.Aplication.Usuario.Request;
using Blog.Intrastruture.Services.EntitiesService.BaseEntity;
using Blog.Intrastruture.Services.Interface;
using MediatR;

namespace Blog.Aplication.Usuario
{
    public class AlterarSenhaHandler : IRequestHandler<AlterarSenhaRequest, Response<string>>
    {
        private readonly IUsuarioQueryStore _queryStore;
        private readonly IUsuarioCommandStore _commandStore;

        public AlterarSenhaHandler(IUsuarioCommandStore commandStore, IUsuarioQueryStore queryStore)
        {
            _commandStore = commandStore;
            _queryStore = queryStore;
        }

        public async Task<Response<string>> Handle(AlterarSenhaRequest request, CancellationToken cancellationToken)
        {
            var sucesso = await _commandStore.AlterarSenhaAsync(request.IdUsuario, request.NovaSenha);

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
