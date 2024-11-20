using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Blog.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class AddTabelas_04 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DhExclusao",
                table: "Postagems");

            migrationBuilder.AddColumn<DateTime>(
                name: "DataExclusao",
                table: "Postagems",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DataExclusao",
                table: "Postagems");

            migrationBuilder.AddColumn<DateTime>(
                name: "DhExclusao",
                table: "Postagems",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
