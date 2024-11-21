using Blog.API.Hubs;
using Blog.Aplication.Postagens;
using Blog.Aplication.Postagens.Interface;
using Blog.Aplication.Postagens.Request;
using Blog.Contracts.Dto;
using Blog.Contracts.Enum;
using Blog.Intrastruture.Services.Interface;
using Microsoft.AspNetCore.SignalR;
using Moq;

namespace Blog.Teste.Handlers
{
    public class ExcluirPostagemHandlerTests
    {
        private readonly Mock<IUsuarioQueryStore> _usuarioQueryStoreMock;
        private readonly Mock<IPostagemCommandStore> _postagemCommandStoreMock;
        private readonly Mock<IPostagemQueryStore> _postagemQueryStoreMock;
        private readonly ExcluirPostagemHandler _handler;
        private readonly Mock<IHubClients> _hubClientsMock;
        private readonly Mock<IHubContext<PostagemHub>> _hubContextMock;

        public ExcluirPostagemHandlerTests()
        {
            _hubContextMock = new Mock<IHubContext<PostagemHub>>();
            _hubClientsMock = new Mock<IHubClients>();
            _postagemQueryStoreMock = new Mock<IPostagemQueryStore>();
            _usuarioQueryStoreMock = new Mock<IUsuarioQueryStore>();
            _postagemCommandStoreMock = new Mock<IPostagemCommandStore>();
            _handler = new ExcluirPostagemHandler(_postagemCommandStoreMock.Object, _usuarioQueryStoreMock.Object, _postagemQueryStoreMock.Object, _hubContextMock.Object);
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
            var request = new ExcluirPostagemRequest(1, 123);

            _usuarioQueryStoreMock
                .Setup(u => u.ObterUsuarioPorIdAsync(request.IdUsuario))
                .ReturnsAsync(new UsuarioDto { Id = 1, Nome = "Teste", TipoUsuario = TipoUsuario.adm });

            _postagemCommandStoreMock
                .Setup(p => p.ExcluirPostagemAsync(request.IdPostagem))
                .ReturnsAsync(false); 

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Erro ao excluir postagem. Postagem Inexistente.", result.Message);
        }

        [Fact]
        public async Task Handle_DeveRetornarSucessoQuandoExclusaoForBemSucedida()
        {
            // Arrange
            var request = new ExcluirPostagemRequest(1, 123);

            var usuario = new UsuarioDto
            {
                Id = 123,
                Nome = "Teste",
                TipoUsuario = TipoUsuario.adm
            };

            var postagem = new PostagemDto
            {
                IdPostagem = 1,
                IdUsuario = 123,
                Titulo = "Título de Teste"
            };

            _usuarioQueryStoreMock
                .Setup(u => u.ObterUsuarioPorIdAsync(request.IdUsuario))
                .ReturnsAsync(usuario);

            _postagemQueryStoreMock
                .Setup(p => p.ObterPostagemPorIdAsync(request.IdPostagem))
                .ReturnsAsync(postagem); 

            _postagemCommandStoreMock
                .Setup(p => p.ExcluirPostagemAsync(request.IdPostagem))
                .ReturnsAsync(true); 

            var clientProxyMock = new Mock<IClientProxy>();
            clientProxyMock
                .Setup(c => c.SendCoreAsync("ExcluindoPostagem", It.IsAny<object[]>(), default))
                .Returns(Task.CompletedTask); 

            _hubClientsMock
                .Setup(c => c.All)
                .Returns(clientProxyMock.Object); 

            _hubContextMock
                .Setup(h => h.Clients)
                .Returns(_hubClientsMock.Object); 

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("Postagem excluída com sucesso.", result.Message);

            _usuarioQueryStoreMock.Verify(u => u.ObterUsuarioPorIdAsync(request.IdUsuario), Times.Once);
            _postagemQueryStoreMock.Verify(p => p.ObterPostagemPorIdAsync(request.IdPostagem), Times.Once);
            _postagemCommandStoreMock.Verify(p => p.ExcluirPostagemAsync(request.IdPostagem), Times.Once);
            clientProxyMock.Verify(c => c.SendCoreAsync("ExcluindoPostagem", It.Is<object[]>(o => o[0].ToString() == $"Postagem excluida: {postagem.Titulo}"), default), Times.Once);
        }


    }
}
