using Blog.Aplication.Usuario;
using Blog.Aplication.Usuario.Request;
using Blog.Intrastruture.Services.Interface;
using Moq;

namespace Blog.Teste.Handlers
{
    public class AlterarSenhaHandlerTests
    {
        private readonly Mock<IUsuarioCommandStore> _commandStoreMock;
        private readonly Mock<IUsuarioQueryStore> _queryStoreMock;
        private readonly AlterarSenhaHandler _handler;

        public AlterarSenhaHandlerTests()
        {
            _commandStoreMock = new Mock<IUsuarioCommandStore>();
            _queryStoreMock = new Mock<IUsuarioQueryStore>();
            _handler = new AlterarSenhaHandler(_commandStoreMock.Object, _queryStoreMock.Object);
        }

        [Fact]
        public async Task Handle_DeveRetornarSucessoQuandoSenhaForAlterada()
        {
            // Arrange
            var request = new AlterarSenhaRequest { IdUsuario = 1, NovaSenha = "NovaSenha123" };

            _commandStoreMock
                .Setup(c => c.AlterarSenhaAsync(request.IdUsuario, request.NovaSenha))
                .ReturnsAsync(true);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("Senha alterada com sucesso.", result.Message);
        }

        [Fact]
        public async Task Handle_DeveRetornarErroQuandoSenhaNaoForAlterada()
        {
            // Arrange
            var request = new AlterarSenhaRequest { IdUsuario = 1, NovaSenha = "NovaSenha123" };

            _commandStoreMock
                .Setup(c => c.AlterarSenhaAsync(request.IdUsuario, request.NovaSenha))
                .ReturnsAsync(false); 

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Erro ao alterar a senha. Verifique se o usuário existe.", result.Message);
        }

        [Fact]
        public async Task Handle_DeveChamarAlterarSenhaComParametrosCorretos()
        {
            // Arrange
            var request = new AlterarSenhaRequest { IdUsuario = 1, NovaSenha = "NovaSenha123" };

            _commandStoreMock
                .Setup(c => c.AlterarSenhaAsync(request.IdUsuario, request.NovaSenha))
                .ReturnsAsync(true);

            // Act
            await _handler.Handle(request, CancellationToken.None);

            // Assert
            _commandStoreMock.Verify(c => c.AlterarSenhaAsync(1, "NovaSenha123"), Times.Once);
        }
    }
}
