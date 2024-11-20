using Blog.Intrastruture.Services.EntitiesService.BaseEntity;
using MediatR;

namespace Blog.Aplication.Usuario.Request
{
    public class InativarUsuarioRequest : IRequest<Response<string>>
    {
        public int IdUsuario { get; }

        public InativarUsuarioRequest(int idUsuario)
        {
            IdUsuario = idUsuario;
        }
    }
}
