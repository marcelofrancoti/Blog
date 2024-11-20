using Blog.Intrastruture.Services.EntitiesService.BaseEntity;
using MediatR;

namespace Blog.Aplication.Postagens.Request
{
    public class ExcluirPostagemRequest : IRequest<Response<string>>
    {
        public ExcluirPostagemRequest(int id, int idUsuario)
        {
            IdPostagem = id;
            IdUsuario = idUsuario;
        }

        public int IdUsuario { get; set; }
        public int IdPostagem { get; set; }

    }
}
