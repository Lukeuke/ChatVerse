using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Chat.Domain.Migrations
{
    /// <inheritdoc />
    public partial class SenderFullName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SenderFullName",
                table: "Messages",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SenderFullName",
                table: "Messages");
        }
    }
}
