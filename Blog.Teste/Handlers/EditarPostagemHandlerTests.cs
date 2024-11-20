using Blog.Aplication.Postagens;
using Blog.Aplication.Postagens.Interface;
using Blog.Aplication.Postagens.Request;
using Blog.Intrastruture.Services.EntitiesService.BaseEntity;
using Moq;
using Xunit;

namespace Blog.Teste.Handlers
{
    public class EditarPostagemHandlerTests
    {
        private readonly Mock<IPostagemCommandStore> _commandStoreMock;
        private readonly EditarPostagemHandler _handler;

        public EditarPostagemHandlerTests()
        {
            _commandStoreMock = new Mock<IPostagemCommandStore>();
            _handler = new EditarPostagemHandler(_commandStoreMock.Object);
        }

        [Fact]
        public async Task Handle_DeveRetornarErroQuandoTituloEConteudoEstiveremVazios()
        {
            // Arrange
            var request = new EditarPostagemRequest
            {
                IdPostagem = 1,
                Titulo = "",
                Conteudo = ""
            };

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Título ou conteúdo devem ser fornecidos para editar a postagem.", result.Message);
        }

        [Fact]
        public async Task Handle_DeveRetornarErroQuandoEdicaoFalhar()
        {
            // Arrange
            var request = new EditarPostagemRequest
            {
                IdPostagem = 1,
                Titulo = "Novo Título",
                Conteudo = "Novo Conteúdo"
            };

            _commandStoreMock
                .Setup(c => c.EditarPostagemAsync(request.IdPostagem, request.Titulo, request.Conteudo))
                .ReturnsAsync(false); // Simula falha na edição

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Erro ao editar a postagem. Verifique se a postagem existe.", result.Message);
        }

        [Fact]
        public async Task Handle_DeveRetornarSucessoQuandoEdicaoForBemSucedida()
        {
            // Arrange
            var request = new EditarPostagemRequest
            {
                IdPostagem = 1,
                Titulo = "Novo Título",
                Conteudo = "Novo Conteúdo"
            };

            _commandStoreMock
                .Setup(c => c.EditarPostagemAsync(request.IdPostagem, request.Titulo, request.Conteudo))
                .ReturnsAsync(true); // Simula sucesso na edição

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("Postagem editada com sucesso.", result.Message);
        }
    }
}
