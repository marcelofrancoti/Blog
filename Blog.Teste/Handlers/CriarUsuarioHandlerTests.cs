using Blog.Aplication.Usuario;
using Blog.Aplication.Usuario.Request;
using Blog.Contracts.Dto;
using Blog.Contracts.Enum;
using Blog.Intrastruture.Services.Interface;
using Moq;

namespace Blog.Teste.Handlers
{
    public class CriarUsuarioHandlerTests
    {
        private readonly Mock<IUsuarioQueryStore> _queryStoreMock;
        private readonly Mock<IUsuarioCommandStore> _commandStoreMock;
        private readonly CriarUsuarioHandler _handler;

        public CriarUsuarioHandlerTests()
        {
            _queryStoreMock = new Mock<IUsuarioQueryStore>();
            _commandStoreMock = new Mock<IUsuarioCommandStore>();
            _handler = new CriarUsuarioHandler(_commandStoreMock.Object, _queryStoreMock.Object);
        }

        [Fact]
        public async Task Handle_DeveRetornarErroQuandoUsuarioJaExiste()
        {
            // Arrange
            var request = new CriarUsuarioRequest
            {
                Nome = "Usuário Existente",
                Login = "usuario_existente",
                Senha = "123456",
                TipoUsuario = TipoUsuario.adm
            };

            var usuariosMock = new List<UsuarioDto>
            {
                new UsuarioDto { Id = 1, Nome = "Usuário Existente", Login = "usuario_existente" }
            };

            _queryStoreMock
                .Setup(q => q.ObterUsuariosAsync(request.Nome, request.Login))
                .ReturnsAsync(usuariosMock);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Nome de usuário já existe.", result.Message);
        }

        [Fact]
        public async Task Handle_DeveRetornarErroQuandoFalhaAoCriarUsuario()
        {
            // Arrange
            var request = new CriarUsuarioRequest
            {
                Nome = "Novo Usuário",
                Login = "novo_usuario",
                Senha = "123456",
                TipoUsuario = TipoUsuario.simples
            };

            _queryStoreMock
                .Setup(q => q.ObterUsuariosAsync(request.Nome, request.Login))
                .ReturnsAsync(new List<UsuarioDto>());

            _commandStoreMock
                .Setup(c => c.CriarUsuarioAsync(It.IsAny<Domain.Entities.Usuario>()))
                .ReturnsAsync(0); 

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.False(result.Success);
            Assert.Equal(0, result.Data);
        }

        [Fact]
        public async Task Handle_DeveCriarUsuarioComSucesso()
        {
            // Arrange
            var request = new CriarUsuarioRequest
            {
                Nome = "Novo Usuário",
                Login = "novo_usuario",
                Senha = "123456",
                TipoUsuario = TipoUsuario.adm
            };

            _queryStoreMock
                .Setup(q => q.ObterUsuariosAsync(request.Nome, request.Login))
                .ReturnsAsync(new List<UsuarioDto>());

            _commandStoreMock
                .Setup(c => c.CriarUsuarioAsync(It.IsAny<Domain.Entities.Usuario>()))
                .ReturnsAsync(1); 

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.True(result.Success);
            Assert.Equal(1, result.Data);
            Assert.Equal("Usuário criado com sucesso.", result.Message);
        }

        [Fact]
        public async Task Handle_DeveChamarObterUsuariosComParametrosCorretos()
        {
            // Arrange
            var request = new CriarUsuarioRequest
            {
                Nome = "Teste",
                Login = "teste_login"
            };

            _queryStoreMock
                .Setup(q => q.ObterUsuariosAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(new List<UsuarioDto>());

            // Act
            await _handler.Handle(request, CancellationToken.None);

            // Assert
            _queryStoreMock.Verify(q => q.ObterUsuariosAsync("Teste", "teste_login"), Times.Once);
        }
    }
}
