using Blog.Contracts.Enum;
using Blog.Intrastruture.Services.EntitiesService.BaseEntity;
using MediatR;

namespace Blog.Aplication.Usuario.Request
{
    public class CriarUsuarioRequest : IRequest<Response<int>>
    {
        public string Login { get; set; }
        public string Nome { get; set; }
        public string Senha { get; set; }
        public TipoUsuario  TipoUsuario { get; set; }
    }
}
