using Blog.Domain.Comum;

namespace Blog.Domain.Entities
{
    public class Usuario : EntidadeBase
    {
        public string Login { get; set; }
        public string Senha { get; set; }
        public string Nome { get; set; }

        public int TipoUsuario { get; set; }
    }
}
