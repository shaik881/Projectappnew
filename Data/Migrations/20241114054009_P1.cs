using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DefaultIdentityColumnRename.Data.Migrations
{
    /// <inheritdoc />
    public partial class P1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "UserName",
                table: "AspNetUsers",
                newName: "NameUser");

            migrationBuilder.AddColumn<string>(
                name: "Custom",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Custom",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "NameUser",
                table: "AspNetUsers",
                newName: "UserName");

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
