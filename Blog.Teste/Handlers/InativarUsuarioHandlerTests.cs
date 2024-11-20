using Blog.Aplication.Usuario;
using Blog.Aplication.Usuario.Request;
using Blog.Intrastruture.Services.EntitiesService.BaseEntity;
using Blog.Intrastruture.Services.Interface;
using Moq;
using Xunit;

namespace Blog.Teste.Handlers
{
    public class InativarUsuarioHandlerTests
    {
        private readonly Mock<IUsuarioCommandStore> _commandStoreMock;
        private readonly InativarUsuarioHandler _handler;

        public InativarUsuarioHandlerTests()
        {
            _commandStoreMock = new Mock<IUsuarioCommandStore>();
            _handler = new InativarUsuarioHandler(_commandStoreMock.Object);
        }

        [Fact]
        public async Task Handle_DeveRetornarErroQuandoFalharAoInativarUsuario()
        {
            // Arrange
            var request = new InativarUsuarioRequest(1);

            _commandStoreMock
                .Setup(c => c.InativarUsuarioAsync(request.IdUsuario))
                .ReturnsAsync(false); // Simula falha na inativação

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Erro ao inativar o usuário. Verifique se o usuário existe.", result.Message);
        }

        [Fact]
        public async Task Handle_DeveRetornarSucessoQuandoUsuarioForInativado()
        {
            // Arrange
            var request = new  InativarUsuarioRequest(1);

            _commandStoreMock
                .Setup(c => c.InativarUsuarioAsync(request.IdUsuario))
                .ReturnsAsync(true); // Simula sucesso na inativação

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("Usuário inativado com sucesso.", result.Message);
        }

        [Fact]
        public async Task Handle_DeveChamarInativarUsuarioComIdCorreto()
        {
            // Arrange
            var request = new InativarUsuarioRequest (1);

            _commandStoreMock
                .Setup(c => c.InativarUsuarioAsync(It.IsAny<int>()))
                .ReturnsAsync(true);

            // Act
            await _handler.Handle(request, CancellationToken.None);

            // Assert
            _commandStoreMock.Verify(c => c.InativarUsuarioAsync(1), Times.Once);
        }
    }
}
