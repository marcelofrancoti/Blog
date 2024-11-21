using Blog.Intrastruture.Services.IntegrationService;
using Blog.Migrations;
using Microsoft.EntityFrameworkCore;

namespace Blog.Teste.QueryStore
{
    public class PostagemQueryStoreTests : IDisposable
    {
        private readonly BlogContext _context;
        private readonly PostagemQueryStore _store;

        public PostagemQueryStoreTests()
        {
            var options = new DbContextOptionsBuilder<BlogContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) 
                .Options;

            _context = new BlogContext(options);
            _store = new PostagemQueryStore(_context);

            // Configurando dados iniciais
            _context.Postagens.AddRange(new[]
            {
                new Domain.Entities.Postagem { Id = 1, Titulo = "Título A", Conteudo = "Conteúdo A", Autor = "Autor A", DataRegistro = DateTime.UtcNow },
                new Domain.Entities.Postagem { Id = 2, Titulo = "Título B", Conteudo = "Conteúdo B", Autor = "Autor B", DataRegistro = DateTime.UtcNow },
                new Domain.Entities.Postagem { Id = 3, Titulo = "Título C", Conteudo = "Conteúdo C", Autor = "Autor C", DataRegistro = DateTime.UtcNow, DataExclusao = DateTime.UtcNow }
            });
            _context.SaveChanges();
        }

        [Fact]
        public async Task ObterPostagensAsync_DeveRetornarTodasPostagensAtivas()
        {
            // Act
            var result = await _store.ObterPostagensAsync(null, null, null, null);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task ObterPostagensAsync_DeveFiltrarPorTitulo()
        {
            // Act
            var result = await _store.ObterPostagensAsync("Título A", null, null, null);

            // Assert
            Assert.Single(result);
            Assert.Equal("Título A", result[0].Titulo);
        }

        [Fact]
        public async Task ObterPostagensAsync_DeveFiltrarPorAutor()
        {
            // Act
            var result = await _store.ObterPostagensAsync(null, "Autor B", null, null);

            // Assert
            Assert.Single(result);
            Assert.Equal("Autor B", result[0].Autor);
        }

        [Fact]
        public async Task ObterPostagensAsync_DeveRetornarVazioQuandoNaoEncontrarPostagens()
        {
            // Act
            var result = await _store.ObterPostagensAsync("Título Z", "Autor Z", null, null);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
