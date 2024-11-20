using Blog.Contracts.Dto;
using Blog.Intrastruture.Services.EntitiesService.BaseEntity;
using MediatR;

namespace Blog.Aplication.Postagens.Request
{
    public class ListarPostagemRequest : IRequest<Response<List<PostagemDto>>>
    {
        public int IdPostagem { get; set; }
        public int? IdUsuario { get;  set; }
        public string? Titulo { get; set; }
        public string? Autor { get; set; }

    }

}
