﻿using Blog.Contracts.Dto;
using Blog.Intrastruture.Services.Interface;
using Blog.Migrations;
using Microsoft.EntityFrameworkCore;

namespace Blog.Intrastruture.Services.IntegrationService
{
    public class PostagemQueryStore : IPostagemQueryStore
    {
        private readonly BlogContext _context;

        public PostagemQueryStore(BlogContext context)
        {
            _context = context;
        }

        public async Task<PostagemDto> ObterPostagemPorIdAsync(int IdPostagem)
        {
            var query = _context.Postagens.AsQueryable().Where(p => p.Id == IdPostagem);

            query = query.Where(p => p.DataExclusao == null);

            return  query
                .Select(p => new PostagemDto
                {
                    IdPostagem = p.Id,
                    Titulo = p.Titulo,
                    Conteudo = p.Conteudo,
                    Autor = p.Autor,
                    DataRegistro = p.DataRegistro.Value,
                    IdUsuario = p.IdUsuario,
                }).First();
        }

        public async Task<List<PostagemDto>> ObterPostagensAsync(string? titulo, string? autor, int? IdPostagem, int? IdUsuario)
        {
            var query = _context.Postagens.AsQueryable();

            if (!string.IsNullOrEmpty(titulo))
                query = query.Where(p => p.Titulo.Contains(titulo));

            if (!string.IsNullOrEmpty(autor))
                query = query.Where(p => p.Autor.Contains(autor));

            if (IdPostagem > 0)
                query = query.Where(p => p.Id == IdPostagem);

            if (IdUsuario > 0)
                query = query.Where(p => p.IdUsuario == IdUsuario);

            query = query.Where(p => p.DataExclusao == null);

            return await query
                .Select(p => new PostagemDto
                {
                    IdPostagem = p.Id,
                    Titulo = p.Titulo,
                    Conteudo = p.Conteudo,
                    Autor = p.Autor,
                    IdUsuario = p.IdUsuario,
                    DataRegistro = p.DataRegistro.Value,
                })
                .ToListAsync();
        }
    }
}
