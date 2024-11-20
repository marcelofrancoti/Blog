using Blog.Intrastruture.Services.EntitiesService.BaseEntity;
using MediatR;

namespace Blog.Aplication.Postagens.Request
{
    public class AtivarPostagemRequest : IRequest<Response<string>>
    {
        public int IdUsuario { get; set; }
        public int IdPostagem { get; set; }
    }
}
