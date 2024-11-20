using Blog.Intrastruture.Services.EntitiesService.BaseEntity;
using MediatR;
using System.Text.Json.Serialization;

namespace Blog.Aplication.Usuario.Request
{
    public class AlterarSenhaRequest : IRequest<Response<string>>
    {
        [JsonIgnore]
        public int IdUsuario { get; set; }
        public string NovaSenha { get; set; }
    }
}
