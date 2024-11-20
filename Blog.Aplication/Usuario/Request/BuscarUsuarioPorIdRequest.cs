using Blog.Contracts.Dto;
using Blog.Intrastruture.Services.EntitiesService.BaseEntity;
using MediatR;

namespace Blog.Aplication.Usuario.Request
{
    public class BuscarUsuarioPorIdRequest : IRequest<Response<UsuarioDto>>
    {
        public int IdUsuario { get; set; }
    }
}
