using Blog.API.Controllers;
using Blog.Aplication.Usuario.Request;
using Blog.Contracts.Dto;
using Blog.Intrastruture.Services.EntitiesService.BaseEntity;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Blog.Teste.Controllers
{
    public class UsuarioControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly UsuarioController _controller;

        public UsuarioControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _controller = new UsuarioController(_mediatorMock.Object);
        }

        [Fact]
        public async Task AutenticarUsuario_DeveRetornarOkQuandoSucesso()
        {
            // Arrange
            var request = new AutenticarUsuarioRequest { Login = "user", Senha = "password" };
            var mockResponse = new Response<UsuarioDto>
            {
                Success = true,
                Data = new UsuarioDto { Id = 1, Nome = "Teste", Login = "user" }
            };

            _mediatorMock
                .Setup(m => m.Send(request, It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockResponse);

            // Act
            var result = await _controller.AutenticarUsuario(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var responseData = Assert.IsType<UsuarioDto>(okResult.Value);
            Assert.Equal("Teste", responseData.Nome);
        }

        [Fact]
        public async Task BuscarUsuarioPorId_DeveRetornarBadRequestQuandoNaoEncontrado()
        {
            // Arrange
            var id = 1;
            var request = new BuscarUsuarioPorIdRequest { IdUsuario = id };
            var mockResponse = new Response<UsuarioDto> { Success = false, Message = "Usuário não encontrado." };

            _mediatorMock
                .Setup(m => m.Send(It.Is<BuscarUsuarioPorIdRequest>(r => r.IdUsuario == id), It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockResponse);

            // Act
            var result = await _controller.BuscarUsuarioPorId(id);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Usuário não encontrado.", badRequestResult.Value);
        }


        [Fact]
        public async Task CriarUsuario_DeveRetornarBadRequestQuandoFalha()
        {
            // Arrange
            var request = new CriarUsuarioRequest { Nome = "Teste", Login = "user" };
            var mockResponse = new Response<int> { Success = false, Message = "Erro ao criar usuário." };

            _mediatorMock
                .Setup(m => m.Send(request, It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockResponse);

            // Act
            var result = await _controller.CriarUsuario(request);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Erro ao criar usuário.", badRequestResult.Value);
        }
    }
}
