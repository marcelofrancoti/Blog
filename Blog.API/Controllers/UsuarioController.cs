using Blog.API.Controllers.ConfigureBase;
using Blog.Aplication.Usuario.Request;
using Blog.Contracts.Dto;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Blog.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UsuarioController : BaseController
    {
        public UsuarioController(IMediator mediator) : base(mediator) { }

        [HttpPost("autenticar")]
        public async Task<IActionResult> AutenticarUsuario([FromBody] AutenticarUsuarioRequest request)
        {
            return await RequestService<AutenticarUsuarioRequest, UsuarioDto>(
                request,
                result => result.Success ? Ok(result.Data) : Unauthorized(result.Message),
                message => Unauthorized(message)
            );
        }

        [HttpGet]
        public async Task<IActionResult> BuscarUsuarios([FromQuery] BuscarUsuariosRequest request)
        {
            return await RequestService<BuscarUsuariosRequest, List<UsuarioDto>>(
                request,
                result => Ok(result.Data),
                message => BadRequest(message)
            );
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> BuscarUsuarioPorId(int id)
        {
            if (id <= 0)
            {
                return BadRequest("ID do usuário é obrigatório.");
            }

            var request = new BuscarUsuarioPorIdRequest { IdUsuario = id };

            return await RequestService<BuscarUsuarioPorIdRequest, UsuarioDto>(
                request,
                result => result.Success ? Ok(result.Data) : NotFound(result.Message),
                message => BadRequest(message)
            );
        }

        [HttpPost]
        public async Task<IActionResult> CriarUsuario([FromBody] CriarUsuarioRequest request)
        {
            return await RequestService<CriarUsuarioRequest, int>(
                request,
                result => CreatedAtAction(nameof(CriarUsuario), new { id = result.Data }, result),
                message => BadRequest(message)
            );
        }

        [HttpPatch("{id}/alterar-senha")]
        public async Task<IActionResult> AlterarSenha(int id, [FromBody] AlterarSenhaRequest request)
        {
            if (id <= 0)
            {
                return BadRequest("ID do usuário é obrigatório.");
            }

            request.IdUsuario = id;

            return await RequestService<AlterarSenhaRequest, string>(
                request,
                result => Ok(result.Data),
                message => BadRequest(message)
            );
        }


        [HttpPatch("{id}")]
        public async Task<IActionResult> AlterarDados(int id, [FromBody] AlterarUsuarioRequest request)
        {
            if (id <= 0)
            {
                return BadRequest("ID do usuário é obrigatório.");
            }
            request.IdUsuario = id;

            return await RequestService<AlterarUsuarioRequest, string>(
                request,
                result => Ok(result.Data),
                message => BadRequest(message)
            );
        }

        [HttpDelete]
        public async Task<IActionResult> InativarUsuario(int id)
        {
            if (id <= 0)
            {
                return BadRequest("ID do usuário é obrigatório.");
            }

            var request = new InativarUsuarioRequest(id);

            return await RequestService<InativarUsuarioRequest, string>(
                request,
                result => Ok(result.Data),
                message => BadRequest(message)
            );
        }
    }
}
