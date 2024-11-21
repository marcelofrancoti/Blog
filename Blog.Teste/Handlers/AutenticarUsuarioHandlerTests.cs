using Blog.Aplication.Usuario;
using Blog.Aplication.Usuario.Request;
using Blog.Contracts.Dto;
using Blog.Intrastruture.Services.Interface;
using Moq;

namespace Blog.Teste.Handlers
{
    public class AutenticarUsuarioHandlerTests
    {
        private readonly Mock<IUsuarioQueryStore> _queryStoreMock;
        private readonly AutenticarUsuarioHandler _handler;

        public AutenticarUsuarioHandlerTests()
        {
            _queryStoreMock = new Mock<IUsuarioQueryStore>();
            _handler = new AutenticarUsuarioHandler(_queryStoreMock.Object);
        }

        [Fact]
        public async Task Handle_DeveRetornarErroQuandoUsuarioNaoForEncontrado()
        {
            // Arrange
            var request = new AutenticarUsuarioRequest { Login = "usuario_teste", Senha = "senha_incorreta" };

            _queryStoreMock
                .Setup(q => q.AutenticarUsuarioAsync(request.Login, request.Senha))
                .ReturnsAsync((UsuarioDto)null); 

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Login ou senha inválidos.", result.Message);
        }

        [Fact]
        public async Task Handle_DeveRetornarUsuarioQuandoCredenciaisForemValidas()
        {
            // Arrange
            var request = new AutenticarUsuarioRequest { Login = "usuario_teste", Senha = "senha123" };

            var usuarioDto = new UsuarioDto
            {
                Id = 1,
                Nome = "Usuário Teste",
                Login = "usuario_teste",
                TipoUsuario = Contracts.Enum.TipoUsuario.adm
            };

            _queryStoreMock
                .Setup(q => q.AutenticarUsuarioAsync(request.Login, request.Senha))
                .ReturnsAsync(usuarioDto); 

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.True(result.Success);
            Assert.Equal(usuarioDto, result.Data);
        }

        [Fact]
        public async Task Handle_DeveChamarAutenticarUsuarioComParametrosCorretos()
        {
            // Arrange
            var request = new AutenticarUsuarioRequest { Login = "usuario_teste", Senha = "senha123" };

            _queryStoreMock
                .Setup(q => q.AutenticarUsuarioAsync(request.Login, request.Senha))
                .ReturnsAsync(new UsuarioDto());

            // Act
            await _handler.Handle(request, CancellationToken.None);

            // Assert
            _queryStoreMock.Verify(q => q.AutenticarUsuarioAsync("usuario_teste", "senha123"), Times.Once);
        }
    }
}
