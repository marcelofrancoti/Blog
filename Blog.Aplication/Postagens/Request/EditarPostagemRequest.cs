using Blog.Intrastruture.Services.EntitiesService.BaseEntity;
using MediatR;
using System.Text.Json.Serialization;

namespace Blog.Aplication.Postagens.Request
{
    public class EditarPostagemRequest : IRequest<Response<string>>
    {
        [JsonIgnore]
        public int IdPostagem { get; set; }
        [JsonIgnore]
        public int IdUsuario { get; set; }
        public string? Titulo { get; set; } // Permite alterar o título
        public string? Conteudo { get; set; } // Permite alterar o conteúdo
    }
}
