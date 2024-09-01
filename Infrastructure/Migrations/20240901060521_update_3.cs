using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class update_3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateSequence(
                name: "outbox_messages_seq",
                incrementBy: 10);

            migrationBuilder.CreateTable(
                name: "outbox_messages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", maxLength: 200, nullable: false),
                    Type = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WhenCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    WhenProcessed = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_outbox_messages", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_outbox_messages_Id",
                table: "outbox_messages",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "outbox_messages_IDX1",
                table: "outbox_messages",
                column: "Type");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "outbox_messages");

            migrationBuilder.DropSequence(
                name: "outbox_messages_seq");
        }
    }
}
