using Blog.API.Controllers;
using Blog.Aplication.Postagens.Request;
using Blog.Contracts.Dto;
using Blog.Intrastruture.Services.EntitiesService;
using Blog.Intrastruture.Services.EntitiesService.BaseEntity;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Blog.Teste.Controllers
{
    public class PostagemControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly PostagemController _controller;

        public PostagemControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _controller = new PostagemController(_mediatorMock.Object);
        }

        [Fact]
        public async Task ListarPostagens_DeveRetornarOkComListaDePostagens()
        {
            // Arrange
            var request = new ListarPostagemRequest();
            var mockResponse = new Response<List<PostagemDto>>
            {
                Success = true,
                Data = new List<PostagemDto>
                {
                    new PostagemDto { IdPostagem = 1, Titulo = "Postagem 1", Conteudo = "Conteúdo 1" },
                    new PostagemDto { IdPostagem = 2, Titulo = "Postagem 2", Conteudo = "Conteúdo 2" }
                }
            };

            _mediatorMock
                .Setup(m => m.Send(request, It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockResponse);

            // Act
            var result = await _controller.ListarPostagens(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var responseData = Assert.IsType<List<PostagemDto>>(okResult.Value);
            Assert.Equal(2, responseData.Count);
        }

        [Fact]
        public async Task CriarPostagem_DeveRetornarCreatedQuandoSucesso()
        {
            // Arrange
            var request = new CriarPostagemRequest { Titulo = "Nova Postagem", Conteudo = "Conteúdo" };
            var mockResponse = new Response<bool> { Success = true, Data = true };

            _mediatorMock
                .Setup(m => m.Send(request, It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockResponse);

            // Act
            var result = await _controller.CriarPostagem(request);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);

            var response = Assert.IsType<Response<bool>>(createdResult.Value);
            Assert.True(response.Data); 
        }


        [Fact]
        public async Task EditarPostagem_DeveRetornarBadRequestQuandoIdInvalido()
        {
            // Arrange
            var id = 0;
            var idUsuario = 1;
            var request = new EditarPostagemRequest();

            // Act
            var result = await _controller.EditarPostagem(id,idUsuario, request);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Erro ao processar a requisição", badRequestResult.Value);
        }
    }
}
