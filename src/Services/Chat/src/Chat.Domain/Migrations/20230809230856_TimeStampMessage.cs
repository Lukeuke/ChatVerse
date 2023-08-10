using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Chat.Domain.Migrations
{
    /// <inheritdoc />
    public partial class TimeStampMessage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "TimeStampOffset",
                table: "Messages",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TimeStampOffset",
                table: "Messages");
        }
    }
}
