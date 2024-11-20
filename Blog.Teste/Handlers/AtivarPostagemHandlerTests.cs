using Blog.Aplication.Postagens;
using Blog.Aplication.Postagens.Interface;
using Blog.Aplication.Postagens.Request;
using Blog.Contracts.Dto;
using Blog.Contracts.Enum;
using Blog.Intrastruture.Services.Interface;
using Moq;

namespace Blog.Teste.Handlers
{
    public class AtivarPostagemHandlerTests
    {
        private readonly Mock<IUsuarioQueryStore> _usuarioQueryStoreMock;
        private readonly Mock<IPostagemCommandStore> _postagemCommandStoreMock;
        private readonly AtivarPostagemHandler _handler;

        public AtivarPostagemHandlerTests()
        {
            _usuarioQueryStoreMock = new Mock<IUsuarioQueryStore>();
            _postagemCommandStoreMock = new Mock<IPostagemCommandStore>();
            _handler = new AtivarPostagemHandler(_postagemCommandStoreMock.Object, _usuarioQueryStoreMock.Object);
        }

        [Fact]
        public async Task Handle_DeveRetornarErroQuandoUsuarioNaoTemPermissao()
        {
            // Arrange
            var request = new AtivarPostagemRequest { IdUsuario = 1, IdPostagem = 123 };

            _usuarioQueryStoreMock
                .Setup(u => u.ObterUsuarioPorIdAsync(request.IdUsuario))
                .ReturnsAsync(new UsuarioDto { Id = 1, Nome = "Teste", TipoUsuario = TipoUsuario.simples });

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Erro úsuario não tem permissão para ativar postagem.", result.Message);
        }

        [Fact]
        public async Task Handle_DeveRetornarErroQuandoAtivacaoFalha()
        {
            // Arrange
            var request = new AtivarPostagemRequest { IdUsuario = 1, IdPostagem = 123 };

            _usuarioQueryStoreMock
                .Setup(u => u.ObterUsuarioPorIdAsync(request.IdUsuario))
                .ReturnsAsync(new UsuarioDto { Id = 1, Nome = "Teste", TipoUsuario = TipoUsuario.adm });

            _postagemCommandStoreMock
                .Setup(p => p.AtivarPostagemAsync(request.IdPostagem))
                .ReturnsAsync(false); // Simula falha na ativação

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Erro ao ativar postagem.", result.Message);
        }

        [Fact]
        public async Task Handle_DeveRetornarSucessoQuandoAtivacaoForBemSucedida()
        {
            // Arrange
            var request = new AtivarPostagemRequest { IdUsuario = 1, IdPostagem = 123 };

            _usuarioQueryStoreMock
                .Setup(u => u.ObterUsuarioPorIdAsync(request.IdUsuario))
                .ReturnsAsync(new UsuarioDto { Id = 1, Nome = "Teste", TipoUsuario = TipoUsuario.adm });

            _postagemCommandStoreMock
                .Setup(p => p.AtivarPostagemAsync(request.IdPostagem))
                .ReturnsAsync(true); // Simula sucesso na ativação

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("Postagem ativa com sucesso.", result.Message);
        }
    }
}
