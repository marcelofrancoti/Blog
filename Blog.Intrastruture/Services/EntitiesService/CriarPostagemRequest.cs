using Blog.Intrastruture.Services.EntitiesService.BaseEntity;
using MediatR;

namespace Blog.Intrastruture.Services.EntitiesService
{
    public class CriarPostagemRequest : IRequest<Response<bool>>
    {
        public string Titulo { get; set; }
        public string Conteudo { get; set; }
        public int IdUsuario { get; set; }
    }
}
