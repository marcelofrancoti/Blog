using Blog.Domain.Entities;
using Blog.Intrastruture.Services.IntegrationService;
using Blog.Migrations;
using Microsoft.EntityFrameworkCore;

namespace Blog.Teste.CommandStore
{
    public class PostagemCommandStoreTests
    {
        private readonly BlogContext _context;
        private readonly PostagemCommandStore _store;

        public PostagemCommandStoreTests()
        {
            var options = new DbContextOptionsBuilder<BlogContext>()
                .UseInMemoryDatabase(databaseName: "TestBlogDatabase")
                .Options;

            _context = new BlogContext(options);
            _store = new PostagemCommandStore(_context);
        }

        [Fact]
        public async Task InserirPostagemAsync_DeveAdicionarPostagemERetornarId()
        {
            // Arrange
            var postagem = new Postagem
            {
                Titulo = "Título de Teste",
                Conteudo = "Conteúdo de Teste",
                Autor = "Autor de Teste",
                DataRegistro = DateTime.UtcNow
            };

            // Act
            var id = await _store.InserirPostagemAsync(postagem);

            // Assert
            Assert.True(id > 0);
            var postagemInserida = await _context.Postagens.FindAsync(id);
            Assert.NotNull(postagemInserida);
            Assert.Equal("Título de Teste", postagemInserida.Titulo);
        }

        [Fact]
        public async Task ExcluirPostagemAsync_DeveDefinirDataExclusaoERetornarTrue()
        {
            // Arrange
            var postagem = new Postagem
            {
                Titulo = "Título de Teste",
                Conteudo = "Conteúdo de Teste",
                Autor = "Autor de Teste",
                DataRegistro = DateTime.UtcNow
            };

            _context.Postagens.Add(postagem);
            await _context.SaveChangesAsync();

            // Act
            var result = await _store.ExcluirPostagemAsync(postagem.Id);

            // Assert
            Assert.True(result);
            var postagemExcluida = await _context.Postagens.FindAsync(postagem.Id);
            Assert.NotNull(postagemExcluida);
            Assert.NotNull(postagemExcluida.DataExclusao);
        }

        [Fact]
        public async Task ExcluirPostagemAsync_DeveRetornarFalseQuandoPostagemNaoExistir()
        {
            // Act
            var result = await _store.ExcluirPostagemAsync(999);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task EditarPostagemAsync_DeveAtualizarTituloEConteudo()
        {
            // Arrange
            var postagem = new Postagem
            {
                Titulo = "Título Antigo",
                Conteudo = "Conteúdo Antigo",
                Autor = "Autor de Teste",
                DataRegistro = DateTime.UtcNow
            };

            _context.Postagens.Add(postagem);
            await _context.SaveChangesAsync();

            // Act
            var result = await _store.EditarPostagemAsync(postagem.Id, "Título Novo", "Conteúdo Novo");

            // Assert
            Assert.True(result);
            var postagemEditada = await _context.Postagens.FindAsync(postagem.Id);
            Assert.Equal("Título Novo", postagemEditada.Titulo);
            Assert.Equal("Conteúdo Novo", postagemEditada.Conteudo);
            Assert.NotNull(postagemEditada.DataAlteracao);
        }

        [Fact]
        public async Task EditarPostagemAsync_DeveRetornarFalseQuandoPostagemNaoExistir()
        {
            // Act
            var result = await _store.EditarPostagemAsync(999, "Título Novo", "Conteúdo Novo"); 

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task AtivarPostagemAsync_DeveDefinirDataExclusaoParaNullERetornarTrue()
        {
            // Arrange
            var postagem = new Postagem
            {
                Titulo = "Título de Teste",
                Conteudo = "Conteúdo de Teste",
                Autor = "Autor de Teste",
                DataRegistro = DateTime.UtcNow,
                DataExclusao = DateTime.UtcNow 
            };

            _context.Postagens.Add(postagem);
            await _context.SaveChangesAsync();

            // Act
            var result = await _store.AtivarPostagemAsync(postagem.Id);

            // Assert
            Assert.True(result);
            var postagemAtivada = await _context.Postagens.FindAsync(postagem.Id);
            Assert.NotNull(postagemAtivada.DataExclusao);
        }

        [Fact]
        public async Task AtivarPostagemAsync_DeveRetornarFalseQuandoPostagemNaoExistir()
        {
            // Act
            var result = await _store.AtivarPostagemAsync(999); 

            // Assert
            Assert.False(result);
        }
    }
}
