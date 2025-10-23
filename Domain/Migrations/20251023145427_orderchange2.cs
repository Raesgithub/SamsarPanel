using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Domain.Migrations
{
    /// <inheritdoc />
    public partial class orderchange2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Subjectl",
                table: "Orders",
                newName: "Subject");

            migrationBuilder.RenameColumn(
                name: "Messagel",
                table: "Orders",
                newName: "Message");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Subject",
                table: "Orders",
                newName: "Subjectl");

            migrationBuilder.RenameColumn(
                name: "Message",
                table: "Orders",
                newName: "Messagel");
        }
    }
}
