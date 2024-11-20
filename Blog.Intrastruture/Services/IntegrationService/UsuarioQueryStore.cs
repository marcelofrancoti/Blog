using Blog.Contracts.Dto;
using Blog.Contracts.Enum;
using Blog.Intrastruture.Services.Interface;
using Blog.Migrations;
using Microsoft.EntityFrameworkCore;

namespace Blog.Intrastruture.Services.IntegrationService
{
    public class UsuarioQueryStore : IUsuarioQueryStore
    {
        private readonly BlogContext _context;

        public UsuarioQueryStore(BlogContext context)
        {
            _context = context;
        }

        public virtual async Task<UsuarioDto?> AutenticarUsuarioAsync(string login, string senha)
        {
            var usuario = await _context.Usuarios
                .Where(u => u.Login == login && u.Senha == senha)
                .Select(u => new UsuarioDto
                {
                    Id = u.Id,
                    Nome = u.Nome,
                    Login = u.Login,
                    TipoUsuario = (TipoUsuario)u.TipoUsuario,
                    DataRegistro = u.DataRegistro
                })
                .FirstOrDefaultAsync();

            return usuario;
        }


        public virtual async Task<List<UsuarioDto>> ObterUsuariosAsync(string? nome, string? login)
        {
            var query = _context.Usuarios.AsQueryable();

            if (!string.IsNullOrEmpty(nome))
            {
                query = query.Where(u => u.Nome.Contains(nome));
            }

            if (!string.IsNullOrEmpty(login))
            {
                query = query.Where(u => u.Login.Contains(login));
            }

            return await query.Select(u => new UsuarioDto
            {
                Id = u.Id,
                Nome = u.Nome,
                Login = u.Login,
                TipoUsuario= (TipoUsuario)u.TipoUsuario,
                DataRegistro = u.DataRegistro
            }).ToListAsync();
        }

        public virtual async Task<UsuarioDto?> ObterUsuarioPorIdAsync(int id)
        {
            return await _context.Usuarios
                .Where(u => u.Id == id)
                .Select(u => new UsuarioDto
                {
                    Id = u.Id,
                    Nome = u.Nome,
                    Login = u.Login,
                    TipoUsuario = (TipoUsuario)u.TipoUsuario,
                    DataRegistro = u.DataRegistro
                })
                .FirstOrDefaultAsync();
        }
    }
}
