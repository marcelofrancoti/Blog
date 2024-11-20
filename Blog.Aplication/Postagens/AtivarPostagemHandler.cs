using Blog.Aplication.Postagens.Interface;
using Blog.Aplication.Postagens.Request;
using Blog.Contracts.Enum;
using Blog.Intrastruture.Services.EntitiesService.BaseEntity;
using Blog.Intrastruture.Services.Interface;
using MediatR;

namespace Blog.Aplication.Postagens
{
    public class AtivarPostagemHandler : IRequestHandler<AtivarPostagemRequest, Response<string>>
    {
        private readonly IUsuarioQueryStore _usuarioQueryStore;
        private readonly IPostagemCommandStore _commandStore;

        public AtivarPostagemHandler(IPostagemCommandStore commandStore, IUsuarioQueryStore usuarioQueryStore)
        {
            _commandStore = commandStore;
            _usuarioQueryStore = usuarioQueryStore;
        }

        public async Task<Response<string>> Handle(AtivarPostagemRequest request, CancellationToken cancellationToken)
        {

            var verificaUsuarioTemPermissao = await _usuarioQueryStore.ObterUsuarioPorIdAsync(request.IdUsuario);
            if (!verificaUsuarioTemPermissao.TipoUsuario.Equals(TipoUsuario.adm))
            {
                return new Response<string> { Success = false, Message = "Erro úsuario não tem permissão para ativar postagem." };
            }
            var success = await _commandStore.AtivarPostagemAsync(request.IdPostagem);

            return success
                ? new Response<string> { Success = true, Message = "Postagem ativa com sucesso." }
                : new Response<string> { Success = false, Message = "Erro ao ativar postagem." };
        }
    }
}
