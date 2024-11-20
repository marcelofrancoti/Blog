using Blog.Aplication.Postagens.Interface;
using Blog.Domain.Entities;
using Blog.Migrations;

namespace Blog.Intrastruture.Services.IntegrationService
{
    public class PostagemCommandStore : IPostagemCommandStore
    {
        private readonly BlogContext _context;

        public PostagemCommandStore(BlogContext context)
        {
            _context = context;
        }

        public async Task<int> InserirPostagemAsync(Postagem postagem)
        {
            _context.Postagens.Add(postagem);
            await _context.SaveChangesAsync();
            return postagem.Id;
        }

        public async Task<bool> ExcluirPostagemAsync(int id)
        {
            var postagem = await _context.Postagens.FindAsync(id);
            if (postagem == null) return false;

            postagem.DataExclusao = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> EditarPostagemAsync(int idPostagem, string? titulo, string? conteudo)
        {
            var postagem = await _context.Postagens.FindAsync(idPostagem);

            if (postagem == null) return false;

            if (!string.IsNullOrEmpty(titulo))
            {
                postagem.Titulo = titulo;
            }

            if (!string.IsNullOrEmpty(conteudo))
            {
                postagem.Conteudo = conteudo;
            }

            postagem.DataAlteracao = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AtivarPostagemAsync(int idPostagem)
        {
            var postagem = await _context.Postagens.FindAsync(idPostagem);
            if (postagem == null) return false;

            postagem.DataExclusao = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
