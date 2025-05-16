using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ToDoList.Server.Migrations
{
    public partial class AddSequenceColumnToTodosTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Sequence",
                table: "Todos",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Sequence",
                table: "Todos");
        }
    }
}
