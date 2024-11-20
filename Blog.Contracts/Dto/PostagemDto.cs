namespace Blog.Contracts.Dto
{
    public class PostagemDto
    {
        public int IdPostagem { get; set; }
        public int IdUsuario { get; set; }
        public string Titulo { get; set; }
        public string Conteudo { get; set; }
        public string Autor { get; set; }
        public DateTime DataRegistro { get; set; }
    }
}
