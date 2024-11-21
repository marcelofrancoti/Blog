using Blog.Contracts.Enum;
using Blog.Intrastruture.Services.IntegrationService;
using Blog.Migrations;
using Microsoft.EntityFrameworkCore;

namespace Blog.Teste.QueryStore
{
    public class UsuarioQueryStoreTests : IDisposable
    {
        private readonly BlogContext _context;
        private readonly UsuarioQueryStore _store;

        public UsuarioQueryStoreTests()
        {
            var options = new DbContextOptionsBuilder<BlogContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) 
                .Options;

            _context = new BlogContext(options);
            _store = new UsuarioQueryStore(_context);
        }

        [Fact]
        public async Task AutenticarUsuarioAsync_DeveRetornarUsuarioQuandoCredenciaisForemValidas()
        {
            // Arrange
            var usuario = new Domain.Entities.Usuario
            {
                Nome = "Usuário Teste",
                Login = "usuario_teste",
                Senha = "senha123",
                TipoUsuario = (int)TipoUsuario.adm,
                DataRegistro = DateTime.UtcNow
            };

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            // Act
            var resultado = await _store.AutenticarUsuarioAsync("usuario_teste", "senha123");

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(usuario.Nome, resultado.Nome);
            Assert.Equal(usuario.Login, resultado.Login);
        }

        [Fact]
        public async Task AutenticarUsuarioAsync_DeveRetornarNuloQuandoCredenciaisForemInvalidas()
        {
            // Act
            var resultado = await _store.AutenticarUsuarioAsync("usuario_invalido", "senha_errada");

            // Assert
            Assert.Null(resultado);
        }

        [Fact]
        public async Task ObterUsuariosAsync_DeveRetornarListaDeUsuariosFiltradaPorNome()
        {
            // Arrange
            var usuarios = new List<Domain.Entities.Usuario>
            {
                new Domain.Entities.Usuario
                {
                    Nome = "João Silva",
                    Login = "joao",
                    Senha = "senha123",
                    TipoUsuario = (int)TipoUsuario.simples,
                    DataRegistro = DateTime.UtcNow
                },
                new Domain.Entities.Usuario
                {
                    Nome = "Maria Oliveira",
                    Login = "maria",
                    Senha = "senha456",
                    TipoUsuario = (int)TipoUsuario.adm,
                    DataRegistro = DateTime.UtcNow
                }
            };

            _context.Usuarios.AddRange(usuarios);
            await _context.SaveChangesAsync();

            // Act
            var resultado = await _store.ObterUsuariosAsync("João", null);

            // Assert
            Assert.Single(resultado);
            Assert.Equal("João Silva", resultado.First().Nome);
        }

        [Fact]
        public async Task ObterUsuariosAsync_DeveRetornarListaDeUsuariosFiltradaPorLogin()
        {
            // Arrange
            var usuarios = new List<Domain.Entities.Usuario>
            {
                new Domain.Entities.Usuario
                {
                    Nome = "João Silva",
                    Login = "joao",
                    Senha = "senha123",
                    TipoUsuario = (int)TipoUsuario.simples,
                    DataRegistro = DateTime.UtcNow
                },
                new Domain.Entities.Usuario
                {
                    Nome = "Maria Oliveira",
                    Login = "maria",
                    Senha = "senha456",
                    TipoUsuario = (int)TipoUsuario.adm,
                    DataRegistro = DateTime.UtcNow
                }
            };

            _context.Usuarios.AddRange(usuarios);
            await _context.SaveChangesAsync();

            // Act
            var resultado = await _store.ObterUsuariosAsync(null, "maria");

            // Assert
            Assert.Single(resultado);
            Assert.Equal("Maria Oliveira", resultado.First().Nome);
        }

        [Fact]
        public async Task ObterUsuarioPorIdAsync_DeveRetornarUsuarioQuandoIdExistir()
        {
            // Arrange
            var usuario = new Domain.Entities.Usuario
            {
                Nome = "Usuário Teste",
                Login = "usuario_teste",
                Senha = "senha123",
                TipoUsuario = (int)TipoUsuario.simples,
                DataRegistro = DateTime.UtcNow
            };

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            // Act
            var resultado = await _store.ObterUsuarioPorIdAsync(usuario.Id);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(usuario.Nome, resultado.Nome);
        }

        [Fact]
        public async Task ObterUsuarioPorIdAsync_DeveRetornarNuloQuandoIdNaoExistir()
        {
            // Act
            var resultado = await _store.ObterUsuarioPorIdAsync(999); 

            // Assert
            Assert.Null(resultado);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
