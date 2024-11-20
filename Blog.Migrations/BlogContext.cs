using Blog.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Blog.Migrations
{
    public class BlogContext : DbContext
    {
        public BlogContext(DbContextOptions<BlogContext> options) : base(options)
        {
        }
        public DbSet<Postagem> Postagens { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Postagem>(entity =>
            {
                entity.HasKey(u => u.Id);
                entity.Property(u => u.DataRegistro).HasDefaultValueSql("NOW()").HasColumnName("data_registro").ValueGeneratedOnAdd();
                entity.Property(u => u.DataAlteracao).HasColumnName("data_alteracao");
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasKey(u => u.Id);
                entity.Property(u => u.DataRegistro).HasDefaultValueSql("NOW()").HasColumnName("data_registro").ValueGeneratedOnAdd();
                entity.Property(u => u.DataAlteracao).HasColumnName("data_alteracao");
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
