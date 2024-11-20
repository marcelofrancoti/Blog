using Blog.Aplication.Usuario;
using Blog.Aplication.Usuario.Request;
using Blog.Contracts.Dto;
using Blog.Intrastruture.Services.EntitiesService.BaseEntity;
using Blog.Intrastruture.Services.Interface;
using Moq;
using Xunit;

namespace Blog.Teste.Handlers
{
    public class BuscarUsuariosHandlerTests
    {
        private readonly Mock<IUsuarioQueryStore> _queryStoreMock;
        private readonly BuscarUsuariosHandler _handler;

        public BuscarUsuariosHandlerTests()
        {
            _queryStoreMock = new Mock<IUsuarioQueryStore>();
            _handler = new BuscarUsuariosHandler(_queryStoreMock.Object);
        }

        [Fact]
        public async Task Handle_DeveRetornarUsuariosQuandoExistirem()
        {
            // Arrange
            var request = new BuscarUsuariosRequest { Nome = "Teste", Login = "teste_login" };

            var usuariosMock = new List<UsuarioDto>
            {
                new UsuarioDto { Id = 1, Nome = "Usuário 1", Login = "usuario1", TipoUsuario = Contracts.Enum.TipoUsuario.adm },
                new UsuarioDto { Id = 2, Nome = "Usuário 2", Login = "usuario2", TipoUsuario = Contracts.Enum.TipoUsuario.simples }
            };

            _queryStoreMock
                .Setup(q => q.ObterUsuariosAsync(request.Nome, request.Login))
                .ReturnsAsync(usuariosMock);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            Assert.Equal(2, result.Data.Count);
            Assert.Equal("Usuário 1", result.Data[0].Nome);
        }

        [Fact]
        public async Task Handle_DeveRetornarListaVaziaQuandoNaoExistiremUsuarios()
        {
            // Arrange
            var request = new BuscarUsuariosRequest { Nome = "Inexistente", Login = "inexistente_login" };

            _queryStoreMock
                .Setup(q => q.ObterUsuariosAsync(request.Nome, request.Login))
                .ReturnsAsync(new List<UsuarioDto>()); // Simula nenhum usuário encontrado

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            Assert.Empty(result.Data);
        }

        [Fact]
        public async Task Handle_DeveChamarObterUsuariosComParametrosCorretos()
        {
            // Arrange
            var request = new BuscarUsuariosRequest { Nome = "Teste", Login = "teste_login" };

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
