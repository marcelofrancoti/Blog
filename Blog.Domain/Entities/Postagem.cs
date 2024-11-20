using Blog.Domain.Comum;

namespace Blog.Domain.Entities
{
    public class Postagem : EntidadeBase
    {
        public int IdUsuario { get; set; }
        public string Titulo { get; set; }
        public string Conteudo { get; set; }
        public string Autor { get; set; }
        public DateTime? DataExclusao { get; set; }

    }
}
