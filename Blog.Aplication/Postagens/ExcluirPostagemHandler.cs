﻿using Blog.Aplication.Postagens.Interface;
using Blog.Aplication.Postagens.Request;
using Blog.Contracts.Enum;
using Blog.Intrastruture.Services.EntitiesService.BaseEntity;
using Blog.Intrastruture.Services.IntegrationService;
using Blog.Intrastruture.Services.Interface;
using MediatR;

namespace Blog.Aplication.Postagens
{
    public class ExcluirPostagemHandler : IRequestHandler<ExcluirPostagemRequest, Response<string>>
    {
        private readonly IUsuarioQueryStore _usuarioQueryStore;
        private readonly IPostagemCommandStore _commandStore;

        public ExcluirPostagemHandler(IPostagemCommandStore commandStore, IUsuarioQueryStore usuarioQueryStore)
        {
            _commandStore = commandStore;
            _usuarioQueryStore = usuarioQueryStore;
        }

        public async Task<Response<string>> Handle(ExcluirPostagemRequest request, CancellationToken cancellationToken)
        {
            var verificaUsuarioTemPermissao = await _usuarioQueryStore.ObterUsuarioPorIdAsync(request.IdUsuario);
            if (!verificaUsuarioTemPermissao.TipoUsuario.Equals(TipoUsuario.adm))
            {
                return new Response<string> { Success = false, Message = "Erro úsuario não tem permissão para excluir postagem." };
            }

            var success = await _commandStore.ExcluirPostagemAsync(request.IdPostagem);

            return success
                ? new Response<string> { Success = true, Message = "Postagem excluída com sucesso." }
                : new Response<string> { Success = false, Message = "Erro ao excluir postagem." };
        }
    }
}
