using Blog.Contracts.Enum;

namespace Blog.Contracts.Dto
{
    public class UsuarioDto
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Login { get; set; }
        public DateTime? DataRegistro { get; set; }
        public TipoUsuario TipoUsuario { get; set; }
    }
}
