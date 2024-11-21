using Blog.Contracts.Dto;
using Blog.Intrastruture.Services.EntitiesService.BaseEntity;
using MediatR;

namespace Blog.Aplication.Usuario.Request
{
    public class BuscarUsuariosRequest : IRequest<Response<List<UsuarioDto>>>
    {
        public string? Nome { get; set; } 
        public string? Login { get; set; } 
    }
}
