using Blog.Aplication.Usuario;
using Blog.Aplication.Usuario.Request;
using Blog.Contracts.Dto;
using Blog.Intrastruture.Services.Interface;
using Moq;

namespace Blog.Teste.Handlers
{
    public class BuscarUsuarioPorIdHandlerTests
    {
        private readonly Mock<IUsuarioQueryStore> _queryStoreMock;
        private readonly BuscarUsuarioPorIdHandler _handler;

        public BuscarUsuarioPorIdHandlerTests()
        {
            _queryStoreMock = new Mock<IUsuarioQueryStore>();
            _handler = new BuscarUsuarioPorIdHandler(_queryStoreMock.Object);
        }

        [Fact]
        public async Task Handle_DeveRetornarErroQuandoUsuarioNaoExistir()
        {
            // Arrange
            var request = new BuscarUsuarioPorIdRequest { IdUsuario = 1 };

            _queryStoreMock
                .Setup(q => q.ObterUsuarioPorIdAsync(It.IsAny<int>()))
                .ReturnsAsync((UsuarioDto)null); 

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Usuário não encontrado.", result.Message);
        }

        [Fact]
        public async Task Handle_DeveRetornarUsuarioQuandoExistir()
        {
            // Arrange
            var request = new BuscarUsuarioPorIdRequest { IdUsuario = 1 };

            var usuarioDto = new UsuarioDto
            {
                Id = 1,
                Nome = "Usuário Teste",
                Login = "usuario_teste",
                TipoUsuario = Contracts.Enum.TipoUsuario.adm
            };

            _queryStoreMock
                .Setup(q => q.ObterUsuarioPorIdAsync(request.IdUsuario))
                .ReturnsAsync(usuarioDto); 

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.True(result.Success);
            Assert.Equal(usuarioDto, result.Data);
        }

        [Fact]
        public async Task Handle_DeveChamarObterUsuarioPorIdComParametroCorreto()
        {
            // Arrange
            var request = new BuscarUsuarioPorIdRequest { IdUsuario = 1 };

            _queryStoreMock
                .Setup(q => q.ObterUsuarioPorIdAsync(It.IsAny<int>()))
                .ReturnsAsync(new UsuarioDto());

            // Act
            await _handler.Handle(request, CancellationToken.None);

            // Assert
            _queryStoreMock.Verify(q => q.ObterUsuarioPorIdAsync(1), Times.Once);
        }
    }
}
