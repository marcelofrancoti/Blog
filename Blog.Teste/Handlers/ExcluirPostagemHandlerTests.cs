using Blog.Aplication.Postagens;
using Blog.Aplication.Postagens.Interface;
using Blog.Aplication.Postagens.Request;
using Blog.Contracts.Dto;
using Blog.Contracts.Enum;
using Blog.Intrastruture.Services.EntitiesService.BaseEntity;
using Blog.Intrastruture.Services.Interface;
using Moq;
using Xunit;

namespace Blog.Teste.Handlers
{
    public class ExcluirPostagemHandlerTests
    {
        private readonly Mock<IUsuarioQueryStore> _usuarioQueryStoreMock;
        private readonly Mock<IPostagemCommandStore> _postagemCommandStoreMock;
        private readonly ExcluirPostagemHandler _handler;

        public ExcluirPostagemHandlerTests()
        {
            _usuarioQueryStoreMock = new Mock<IUsuarioQueryStore>();
            _postagemCommandStoreMock = new Mock<IPostagemCommandStore>();
            _handler = new ExcluirPostagemHandler(_postagemCommandStoreMock.Object, _usuarioQueryStoreMock.Object);
        }

        [Fact]
        public async Task Handle_DeveRetornarErroQuandoUsuarioNaoTemPermissao()
        {
            // Arrange
            var request = new ExcluirPostagemRequest(1, 123);

            _usuarioQueryStoreMock
                .Setup(u => u.ObterUsuarioPorIdAsync(request.IdUsuario))
                .ReturnsAsync(new UsuarioDto { Id = 1, Nome = "Teste", TipoUsuario = TipoUsuario.simples });

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Erro úsuario não tem permissão para excluir postagem.", result.Message);
        }

        [Fact]
        public async Task Handle_DeveRetornarErroQuandoExclusaoFalha()
        {
            // Arrange
            var request = new ExcluirPostagemRequest( 1, 123);

            _usuarioQueryStoreMock
                .Setup(u => u.ObterUsuarioPorIdAsync(request.IdUsuario))
                .ReturnsAsync(new UsuarioDto { Id = 1, Nome = "Teste", TipoUsuario = TipoUsuario.adm });

            _postagemCommandStoreMock
                .Setup(p => p.ExcluirPostagemAsync(request.IdPostagem))
                .ReturnsAsync(false); // Simula falha na exclusão

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Erro ao excluir postagem.", result.Message);
        }

        [Fact]
        public async Task Handle_DeveRetornarSucessoQuandoExclusaoForBemSucedida()
        {
            // Arrange
            var request = new ExcluirPostagemRequest(1, 123);

            _usuarioQueryStoreMock
                .Setup(u => u.ObterUsuarioPorIdAsync(request.IdUsuario))
                .ReturnsAsync(new UsuarioDto { Id = 1, Nome = "Teste", TipoUsuario = TipoUsuario.adm });

            _postagemCommandStoreMock
                .Setup(p => p.ExcluirPostagemAsync(request.IdPostagem))
                .ReturnsAsync(true); // Simula sucesso na exclusão

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("Postagem excluída com sucesso.", result.Message);
        }
    }
}
