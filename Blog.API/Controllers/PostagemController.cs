using Blog.API.Controllers.ConfigureBase;
using Blog.Aplication.Postagens.Request;
using Blog.Contracts.Dto;
using Blog.Intrastruture.Services.EntitiesService;
using Blog.Intrastruture.Services.EntitiesService.BaseEntity;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Blog.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class PostagemController : BaseController
    {
        public PostagemController(IMediator mediator) : base(mediator) { }

        [HttpGet]
        public async Task<IActionResult> ListarPostagens([FromQuery] ListarPostagemRequest request)
        {
            return await RequestService<ListarPostagemRequest, List<PostagemDto>>(
                request,
                result => Ok(result.Data),
                message => BadRequest(message)
            );
        }

        [HttpPost]
        public async Task<IActionResult> CriarPostagem([FromBody] CriarPostagemRequest request)
        {
            return await RequestService<CriarPostagemRequest, bool>(
                request,
                result => CreatedAtAction(nameof(CriarPostagem), new { id = result.Data }, result),
                message => BadRequest(message)
            );
        }

        [HttpPatch]
        public async Task<IActionResult> EditarPostagem(int idPostagem, int idUsuario, [FromBody] EditarPostagemRequest request)
        {
            if (idPostagem <= 0 && idUsuario <= 0)
            {
                return BadRequest("ID da postagem é obrigatório.");
            }

            request.IdPostagem = idPostagem;
            request.IdUsuario = idUsuario;

            return await RequestService<EditarPostagemRequest, string>(
                request,
                result => Ok(result.Data),
                message => BadRequest(message)
            );
        }

        [HttpDelete]
        public async Task<IActionResult> ExcluirPostagem(int idPostagem, int idUsuario)
        {
            if (idPostagem <= 0 || idUsuario <= 0)
            {
                return BadRequest("ID da postagem e ID do usuário são obrigatórios.");
            }

            var request = new ExcluirPostagemRequest(idPostagem, idUsuario);

            return await RequestService<ExcluirPostagemRequest, string>(
                request,
                result => Ok(result.Data),
                message => BadRequest(message)
            );
        }

        [HttpPut("AtivarPostagem")]
        public async Task<IActionResult> AtivarPostagem([FromBody] AtivarPostagemRequest request)
        {
            return await RequestService<AtivarPostagemRequest, string>(
                request,
                result => Ok(result.Data),
                message => BadRequest(message)
            );
        }
    }
}
