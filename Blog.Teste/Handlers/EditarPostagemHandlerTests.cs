using Blog.Aplication.Postagens;
using Blog.Aplication.Postagens.Interface;
using Blog.Aplication.Postagens.Request;
using Blog.Contracts.Dto;
using Blog.Intrastruture.Services.Interface;
using Moq;
using Xunit;

namespace Blog.Teste.Handlers
{
    public class EditarPostagemHandlerTests
    {
        private readonly Mock<IPostagemCommandStore> _postagemCommandStoreMock;
        private readonly Mock<IPostagemQueryStore> _postagemQueryStoreMock;
        private readonly Mock<IUsuarioQueryStore> _usuarioQueryStoreMock;
        private readonly EditarPostagemHandler _handler;

        public EditarPostagemHandlerTests()
        {
            _postagemCommandStoreMock = new Mock<IPostagemCommandStore>();
            _postagemQueryStoreMock = new Mock<IPostagemQueryStore>();
            _usuarioQueryStoreMock = new Mock<IUsuarioQueryStore>();

            _handler = new EditarPostagemHandler(
                _postagemCommandStoreMock.Object,
                _usuarioQueryStoreMock.Object,
                _postagemQueryStoreMock.Object
            );
        }

        [Fact]
        public async Task Handle_DeveRetornarErroQuandoTituloEConteudoEstiveremVazios()
        {
            // Arrange
            var request = new EditarPostagemRequest
            {
                IdPostagem = 1,
                Titulo = "",
                Conteudo = ""
            };

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Título ou conteúdo devem ser fornecidos para editar a postagem.", result.Message);
        }

        [Fact]
        public async Task Handle_DeveRetornarErroQuandoUsuarioNaoExistir()
        {
            // Arrange
            var request = new EditarPostagemRequest
            {
                IdPostagem = 1,
                IdUsuario = 1,
                Titulo = "Novo Título",
                Conteudo = "Novo Conteúdo"
            };

            _usuarioQueryStoreMock
                .Setup(u => u.ObterUsuarioPorIdAsync(request.IdUsuario))
                .ReturnsAsync((UsuarioDto)null); 

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Erro ao editar a postagem. Usuario não existe.", result.Message);
        }

        [Fact]
        public async Task Handle_DeveRetornarErroQuandoPostagemNaoExistir()
        {
            // Arrange
            var request = new EditarPostagemRequest
            {
                IdPostagem = 1,
                IdUsuario = 1,
                Titulo = "Novo Título",
                Conteudo = "Novo Conteúdo"
            };

            var usuario = new UsuarioDto { Id = 1, Nome = "Usuário Teste" };

            _usuarioQueryStoreMock
                .Setup(u => u.ObterUsuarioPorIdAsync(request.IdUsuario))
                .ReturnsAsync(usuario); 

            _postagemQueryStoreMock
                .Setup(p => p.ObterPostagemPorIdAsync(request.IdPostagem)).ReturnsAsync((PostagemDto)null); 

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Erro ao editar a postagem. Post não existe.", result.Message);
        }

        [Fact]
        public async Task Handle_DeveRetornarErroQuandoPostagemNaoPertencerAoUsuario()
        {
            // Arrange
            var request = new EditarPostagemRequest
            {
                IdPostagem = 1,
                IdUsuario = 1,
                Titulo = "Novo Título",
                Conteudo = "Novo Conteúdo"
            };

            var usuario = new UsuarioDto { Id = 1, Nome = "Usuário Teste" };
            var postagem = new PostagemDto { IdPostagem = 1, IdUsuario = 2 }; 

            _usuarioQueryStoreMock
                .Setup(u => u.ObterUsuarioPorIdAsync(request.IdUsuario))
                .ReturnsAsync(usuario);

            _postagemQueryStoreMock
                .Setup(p => p.ObterPostagemPorIdAsync(request.IdPostagem))
                .ReturnsAsync(postagem);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Erro ao editar a postagem. Esse post não é desse Usuario, por favor alterar um post que seja seu.", result.Message);
        }

        [Fact]
        public async Task Handle_DeveRetornarSucessoQuandoEdicaoForBemSucedida()
        {
            // Arrange
            var request = new EditarPostagemRequest
            {
                IdPostagem = 1,
                IdUsuario = 1,
                Titulo = "Novo Título",
                Conteudo = "Novo Conteúdo"
            };

            var usuario = new UsuarioDto { Id = 1, Nome = "Usuário Teste" };
            var postagem = new PostagemDto { IdPostagem = 1, IdUsuario = 1 }; 

            _usuarioQueryStoreMock
                .Setup(u => u.ObterUsuarioPorIdAsync(request.IdUsuario))
                .ReturnsAsync(usuario);

            _postagemQueryStoreMock
                .Setup(p => p.ObterPostagemPorIdAsync(request.IdPostagem))
                .ReturnsAsync(postagem);

            _postagemCommandStoreMock
                .Setup(c => c.EditarPostagemAsync(request.IdPostagem, request.Titulo, request.Conteudo))
                .ReturnsAsync(true); 

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("Postagem editada com sucesso.", result.Message);

            _usuarioQueryStoreMock.Verify(u => u.ObterUsuarioPorIdAsync(request.IdUsuario), Times.Once);
            _postagemQueryStoreMock.Verify(p => p.ObterPostagemPorIdAsync(request.IdPostagem), Times.Once);
            _postagemCommandStoreMock.Verify(c => c.EditarPostagemAsync(request.IdPostagem, request.Titulo, request.Conteudo), Times.Once);
        }
    }
}
