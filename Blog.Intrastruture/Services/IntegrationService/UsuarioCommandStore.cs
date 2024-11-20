using Blog.Domain.Entities;
using Blog.Intrastruture.Services.Interface;
using Blog.Migrations;

namespace Blog.Intrastruture.Services.IntegrationService
{
    public class UsuarioCommandStore: IUsuarioCommandStore
    {
        private readonly BlogContext _context;

        public UsuarioCommandStore(BlogContext context)
        {
            _context = context;
        }

        public async Task<int> CriarUsuarioAsync(Usuario usuario)
        {
            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();
            return usuario.Id;
        }

        public async Task<bool> AlterarSenhaAsync(int idUsuario, string novaSenha)
        {
            var usuario = await _context.Usuarios.FindAsync(idUsuario);
            if (usuario == null) return false;

            usuario.Senha = novaSenha;
            usuario.DataAlteracao = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> InativarUsuarioAsync(int idUsuario)
        {
            var usuario = await _context.Usuarios.FindAsync(idUsuario);
            if (usuario == null) return false;

            usuario.DataAlteracao = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}

