using Blog.Aplication.Usuario.Request;
using Blog.Intrastruture.Services.EntitiesService.BaseEntity;
using Blog.Intrastruture.Services.Interface;
using MediatR;

namespace Blog.Aplication.Usuario
{
    public class CriarUsuarioHandler : IRequestHandler<CriarUsuarioRequest, Response<int>>
    {
        private readonly IUsuarioQueryStore _queryStore;
        private readonly IUsuarioCommandStore _commandStore;

        public CriarUsuarioHandler(IUsuarioCommandStore commandStore, IUsuarioQueryStore queryStore)
        {
            _commandStore = commandStore;
            _queryStore = queryStore;
        }

        public async Task<Response<int>> Handle(CriarUsuarioRequest request, CancellationToken cancellationToken)
        {
            var verificaUsuarioExiste = await  _queryStore.ObterUsuariosAsync(request.Nome, request.Login);
            if (verificaUsuarioExiste != null && verificaUsuarioExiste.Any())
            {
                return new Response<int>
                {
                    Data = 0,
                    Success = false,
                    Message = "Nome de usuário já existe."
                };
            }

            var usuario = new Domain.Entities.Usuario
            {
                Login = request.Login,
                Nome = request.Nome,
                Senha = request.Senha,
                TipoUsuario = (int)request.TipoUsuario,
                DataRegistro = DateTime.UtcNow
            };


            var idUsuario = await _commandStore.CriarUsuarioAsync(usuario);

            if (idUsuario <= 0)
            {
                return new Response<int>
                {
                    Data = idUsuario,
                    Success = false,
                    Message = "Usuário criado com sucesso."
                };
            }

            return new Response<int>
            {
                Data = idUsuario,
                Success = true,
                Message = "Usuário criado com sucesso."
            };
        }
    }
}
