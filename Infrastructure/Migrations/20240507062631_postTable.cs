using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class postTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateSequence(
                name: "postseq",
                incrementBy: 10);

            migrationBuilder.CreateSequence(
                name: "topiseq",
                incrementBy: 10);

            migrationBuilder.CreateTable(
                name: "posts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", maxLength: 200, nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WhenCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    WhenUpdated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_posts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "topic_type",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_topic_type", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "topics",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", maxLength: 200, nullable: false),
                    TopicTypeId = table.Column<int>(type: "int", nullable: false),
                    PostId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_topics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_topics_posts_PostId",
                        column: x => x.PostId,
                        principalTable: "posts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_topics_topic_type_TopicTypeId",
                        column: x => x.TopicTypeId,
                        principalTable: "topic_type",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_posts_Id",
                table: "posts",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_topic_type_Id",
                table: "topic_type",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_topics_Id",
                table: "topics",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_topics_PostId",
                table: "topics",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_topics_TopicTypeId",
                table: "topics",
                column: "TopicTypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "topics");

            migrationBuilder.DropTable(
                name: "posts");

            migrationBuilder.DropTable(
                name: "topic_type");

            migrationBuilder.DropSequence(
                name: "postseq");

            migrationBuilder.DropSequence(
                name: "topiseq");
        }
    }
}
