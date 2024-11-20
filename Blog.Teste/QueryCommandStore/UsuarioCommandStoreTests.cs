using Blog.Domain.Entities;
using Blog.Intrastruture.Services.IntegrationService;
using Blog.Migrations;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Blog.Teste.CommandStore
{
    public class UsuarioCommandStoreTests : IDisposable
    {
        private readonly BlogContext _context;
        private readonly UsuarioCommandStore _store;

        public UsuarioCommandStoreTests()
        {
            var options = new DbContextOptionsBuilder<BlogContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Banco em memória
                .Options;

            _context = new BlogContext(options);
            _store = new UsuarioCommandStore(_context);
        }

        [Fact]
        public async Task CriarUsuarioAsync_DeveAdicionarUsuarioERetornarId()
        {
            // Arrange
            var usuario = new Usuario
            {
                Nome = "Teste",
                Login = "teste",
                Senha = "senha123",
                TipoUsuario = 1,
                DataRegistro = DateTime.UtcNow
            };

            // Act
            var id = await _store.CriarUsuarioAsync(usuario);

            // Assert
            Assert.True(id > 0);
            var usuarioCriado = await _context.Usuarios.FindAsync(id);
            Assert.NotNull(usuarioCriado);
            Assert.Equal("Teste", usuarioCriado.Nome);
        }

        [Fact]
        public async Task AlterarSenhaAsync_DeveAtualizarSenhaDoUsuario()
        {
            // Arrange
            var usuario = new Usuario
            {
                Nome = "Teste",
                Login = "teste",
                Senha = "senha123",
                TipoUsuario = 1,
                DataRegistro = DateTime.UtcNow
            };

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            // Act
            var resultado = await _store.AlterarSenhaAsync(usuario.Id, "novaSenha456");

            // Assert
            Assert.True(resultado);
            var usuarioAlterado = await _context.Usuarios.FindAsync(usuario.Id);
            Assert.Equal("novaSenha456", usuarioAlterado.Senha);
            Assert.NotNull(usuarioAlterado.DataAlteracao);
        }

        [Fact]
        public async Task AlterarSenhaAsync_DeveRetornarFalseQuandoUsuarioNaoExistir()
        {
            // Act
            var resultado = await _store.AlterarSenhaAsync(999, "novaSenha456"); // ID inexistente

            // Assert
            Assert.False(resultado);
        }

        [Fact]
        public async Task InativarUsuarioAsync_DeveAtualizarDataAlteracao()
        {
            // Arrange
            var usuario = new Usuario
            {
                Nome = "Teste",
                Login = "teste",
                Senha = "senha123",
                TipoUsuario = 1,
                DataRegistro = DateTime.UtcNow
            };

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            // Act
            var resultado = await _store.InativarUsuarioAsync(usuario.Id);

            // Assert
            Assert.True(resultado);
            var usuarioInativado = await _context.Usuarios.FindAsync(usuario.Id);
            Assert.NotNull(usuarioInativado.DataAlteracao);
        }

        [Fact]
        public async Task InativarUsuarioAsync_DeveRetornarFalseQuandoUsuarioNaoExistir()
        {
            // Act
            var resultado = await _store.InativarUsuarioAsync(999); // ID inexistente

            // Assert
            Assert.False(resultado);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
