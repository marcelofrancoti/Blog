using Blog.Contracts.Dto;
using Blog.Intrastruture.Services.EntitiesService.BaseEntity;
using MediatR;

namespace Blog.Aplication.Usuario.Request
{
    public class AutenticarUsuarioRequest : IRequest<Response<UsuarioDto>>
    {
        public string Login { get; set; }
        public string Senha { get; set; }
    }
}
