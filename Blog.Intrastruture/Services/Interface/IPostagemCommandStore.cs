using Blog.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Aplication.Postagens.Interface
{
    public interface IPostagemCommandStore
    {
        Task<int> InserirPostagemAsync(Postagem postagem);
        Task<bool> ExcluirPostagemAsync(int id);
        Task<bool> EditarPostagemAsync(int idPostagem, string? titulo, string? conteudo);
        Task<bool> AtivarPostagemAsync(int idPostagem);
    }
}
