using Blog.API.Hubs;
using Blog.Aplication.Postagens;
using Blog.Aplication.Postagens.Interface;
using Blog.Contracts.Dto;
using Blog.Domain.Entities;
using Blog.Intrastruture.Services.EntitiesService;
using Blog.Intrastruture.Services.Interface;
using Microsoft.AspNetCore.SignalR;
using Moq;

namespace Blog.Teste.Handlers
{
    public class InserirPostagemHandlerTests
    {
        private readonly Mock<IUsuarioQueryStore> _queryStoreMock;
        private readonly Mock<IPostagemCommandStore> _commandStoreMock;
        private readonly Mock<IHubContext<PostagemHub>> _hubContextMock;
        private readonly Mock<IClientProxy> _clientProxyMock;
        private readonly Mock<IHubClients> _hubClientsMock;
        private readonly InserirPostagemHandler _handler;

        public InserirPostagemHandlerTests()
        {
            _queryStoreMock = new Mock<IUsuarioQueryStore>();
            _commandStoreMock = new Mock<IPostagemCommandStore>();
            _hubContextMock = new Mock<IHubContext<PostagemHub>>();
            _clientProxyMock = new Mock<IClientProxy>();
            _hubClientsMock = new Mock<IHubClients>();

            _hubClientsMock.Setup(c => c.All).Returns(_clientProxyMock.Object);
            _hubContextMock.Setup(c => c.Clients).Returns(_hubClientsMock.Object);

            _handler = new InserirPostagemHandler(_commandStoreMock.Object, _queryStoreMock.Object, _hubContextMock.Object);
        }

        [Fact]
        public async Task Handle_DeveRetornarErroQuandoUsuarioNaoExistir()
        {
            // Arrange
            var request = new CriarPostagemRequest { IdUsuario = 1, Titulo = "Teste", Conteudo = "Conteúdo" };

            _queryStoreMock
                .Setup(q => q.ObterUsuarioPorIdAsync(It.IsAny<int>()))
                .ReturnsAsync((UsuarioDto?)null); 

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Usuário inválido, favor inserir um usuário válido.", result.Message);
        }

        [Fact]
        public async Task Handle_DeveRetornarErroQuandoTituloOuConteudoNaoForemInformados()
        {
            // Arrange
            var request = new CriarPostagemRequest { IdUsuario = 1, Titulo = "", Conteudo = "" };

            _queryStoreMock
                .Setup(q => q.ObterUsuarioPorIdAsync(It.IsAny<int>()))
                .ReturnsAsync(new UsuarioDto { Id = 1, Nome = "Teste" });

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Título e conteúdo são obrigatórios.", result.Message);
        }

        [Fact]
        public async Task Handle_DeveRetornarErroQuandoNaoConseguirCriarPostagem()
        {
            // Arrange
            var request = new CriarPostagemRequest { IdUsuario = 1, Titulo = "Teste", Conteudo = "Conteúdo" };

            _queryStoreMock
                .Setup(q => q.ObterUsuarioPorIdAsync(It.IsAny<int>()))
                .ReturnsAsync(new UsuarioDto { Id = 1, Nome = "Teste" });

            _commandStoreMock
                .Setup(c => c.InserirPostagemAsync(It.IsAny<Postagem>()))
                .ReturnsAsync(0); 

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Erro ao criar a postagem.", result.Message);
        }

        [Fact]
        public async Task Handle_DeveRetornarSucessoEEnviarMensagemNoHub()
        {
            // Arrange
            var request = new CriarPostagemRequest { IdUsuario = 1, Titulo = "Teste", Conteudo = "Conteúdo" };

            _queryStoreMock
                .Setup(q => q.ObterUsuarioPorIdAsync(It.IsAny<int>()))
                .ReturnsAsync(new UsuarioDto { Id = 1, Nome = "Teste" });

            _commandStoreMock
                .Setup(c => c.InserirPostagemAsync(It.IsAny<Postagem>()))
                .ReturnsAsync(1); 

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("Postagem criada com sucesso.", result.Message);

            _clientProxyMock.Verify(
                c => c.SendCoreAsync("NovaPostagem", It.Is<object[]>(o => o[0].ToString() == "Nova postagem criada: Teste"), default),
                Times.Once 
            );
        }
    }

}
