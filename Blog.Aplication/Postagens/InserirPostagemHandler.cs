using Blog.API.Hubs;
using Blog.Aplication.Postagens.Interface;
using Blog.Domain.Entities;
using Blog.Intrastruture.Services.EntitiesService;
using Blog.Intrastruture.Services.EntitiesService.BaseEntity;
using Blog.Intrastruture.Services.Interface;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace Blog.Aplication.Postagens
{
    public class InserirPostagemHandler : IRequestHandler<CriarPostagemRequest, Response<bool>>
    {
        private readonly IPostagemCommandStore _commandStore;
        private readonly IUsuarioQueryStore _queryStore;
        private readonly IHubContext<PostagemHub> _hubContext;

        public InserirPostagemHandler(IPostagemCommandStore commandStore, IUsuarioQueryStore queryStore, IHubContext<PostagemHub> hubContext)
        {
            _commandStore = commandStore;
            _queryStore = queryStore;
            _hubContext = hubContext;
        }


        public async Task<Response<bool>> Handle(CriarPostagemRequest request, CancellationToken cancellationToken)
        {
            var autenticaUsuario = await _queryStore.ObterUsuarioPorIdAsync(request.IdUsuario);
            if (autenticaUsuario == null)
            {
                return new Response<bool>
                {
                    Success = false,
                    Message = "Usuário inválido, favor inserir um usuário válido."
                };
            }

            if (string.IsNullOrEmpty(request.Titulo) || string.IsNullOrEmpty(request.Conteudo))
            {
                return new Response<bool>
                {
                    Success = false,
                    Message = "Título e conteúdo são obrigatórios."
                };
            }

            var postagem = new Postagem
            {
                Titulo = request.Titulo,
                Conteudo = request.Conteudo,
                Autor = autenticaUsuario.Nome, 
                IdUsuario = request.IdUsuario,
                DataRegistro = DateTime.UtcNow
            };

            var postagemId = await _commandStore.InserirPostagemAsync(postagem);

            if (postagemId <= 0)
            {
                return new Response<bool>
                {
                    Success = false,
                    Message = "Erro ao criar a postagem."
                };
            }

            await _hubContext.Clients.All.SendAsync("NovaPostagem", $"Nova postagem criada: {postagem.Titulo}");

            return new Response<bool>
            {
                Data = true,
                Success = true,
                Message = "Postagem criada com sucesso."
            };
        }
    }
}
