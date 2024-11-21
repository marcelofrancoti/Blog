using Blog.Intrastruture.Services.EntitiesService.BaseEntity;
using MediatR;
using System.Text.Json.Serialization;

namespace Blog.Aplication.Usuario.Request
{
    public class AlterarUsuarioRequest : IRequest<Response<string>>
    {
        public string Nome { get; set; }
        public int TipoUsuario { get; set; }
        [JsonIgnore]
        public int IdUsuario { get; set; }
    }
}
