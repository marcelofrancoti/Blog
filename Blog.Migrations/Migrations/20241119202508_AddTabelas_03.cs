using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Blog.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class AddTabelas_03 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "title",
                table: "Postagems",
                newName: "Titulo");

            migrationBuilder.RenameColumn(
                name: "Body",
                table: "Postagems",
                newName: "Conteudo");

            migrationBuilder.AddColumn<string>(
                name: "Autor",
                table: "Postagems",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "DataCriacao",
                table: "Postagems",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Autor",
                table: "Postagems");

            migrationBuilder.DropColumn(
                name: "DataCriacao",
                table: "Postagems");

            migrationBuilder.RenameColumn(
                name: "Titulo",
                table: "Postagems",
                newName: "title");

            migrationBuilder.RenameColumn(
                name: "Conteudo",
                table: "Postagems",
                newName: "Body");
        }
    }
}
