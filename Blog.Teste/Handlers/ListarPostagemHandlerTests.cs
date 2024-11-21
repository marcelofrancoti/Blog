using Blog.Aplication.Postagens;
using Blog.Aplication.Postagens.Request;
using Blog.Contracts.Dto;
using Blog.Intrastruture.Services.Interface;
using Moq;

namespace Blog.Teste.Handlers
{
    public class ListarPostagemHandlerTests
    {
        private readonly Mock<IPostagemQueryStore> _queryStoreMock;
        private readonly ListarPostagemHandler _handler;

        public ListarPostagemHandlerTests()
        {
            _queryStoreMock = new Mock<IPostagemQueryStore>();
            _handler = new ListarPostagemHandler(_queryStoreMock.Object);
        }

        [Fact]
        public async Task Handle_DeveRetornarPostagensQuandoExistiremResultados()
        {
            // Arrange
            var request = new ListarPostagemRequest { Titulo = "Teste", Autor = "Autor1" };
            var mockPostagens = new List<PostagemDto>
            {
                new PostagemDto { IdPostagem = 1, Titulo = "Teste", Autor = "Autor1", Conteudo = "Conteúdo 1" },
                new PostagemDto { IdPostagem = 2, Titulo = "Outro Teste", Autor = "Autor1", Conteudo = "Conteúdo 2" }
            };

            _queryStoreMock
                .Setup(q => q.ObterPostagensAsync(request.Titulo, request.Autor, request.IdPostagem, request.IdUsuario))
                .ReturnsAsync(mockPostagens);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            Assert.Equal(2, result.Data.Count);
            Assert.Equal("Teste", result.Data[0].Titulo);
        }

        [Fact]
        public async Task Handle_DeveRetornarListaVaziaQuandoNaoExistiremResultados()
        {
            // Arrange
            var request = new ListarPostagemRequest { Titulo = "Inexistente", Autor = "Desconhecido" };

            _queryStoreMock
                .Setup(q => q.ObterPostagensAsync(request.Titulo, request.Autor, request.IdPostagem, request.IdUsuario))
                .ReturnsAsync(new List<PostagemDto>());

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            Assert.Empty(result.Data);
        }

        [Fact]
        public async Task Handle_DeveChamarQueryStoreComParametrosCorretos()
        {
            // Arrange
            var request = new ListarPostagemRequest
            {
                Titulo = "Teste",
                Autor = "Autor1",
                IdPostagem = 1, 
                IdUsuario = null
            };

            _queryStoreMock
                .Setup(q => q.ObterPostagensAsync(
                    It.Is<string>(t => t == "Teste"),
                    It.Is<string>(a => a == "Autor1"),
                    It.Is<int?>(idPostagem => idPostagem == 1), 
                    It.Is<int?>(idUsuario => idUsuario == null) 
                ))
                .ReturnsAsync(new List<PostagemDto>());

            // Act
            await _handler.Handle(request, CancellationToken.None);

            // Assert
            _queryStoreMock.Verify(
                q => q.ObterPostagensAsync("Teste", "Autor1", 1, null), Times.Once);
        }
    }
}
