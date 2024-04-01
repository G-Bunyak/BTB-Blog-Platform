using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlogPlatform.Migrations
{
    /// <inheritdoc />
    public partial class DbInit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "posts",
                columns: table => new
                {
                    p_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    p_author_nickname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    p_title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    p_content = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_posts", x => x.p_id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    u_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    u_login = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    u_password = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.u_id);
                });

            migrationBuilder.CreateTable(
                name: "comments",
                columns: table => new
                {
                    c_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    c_author_nickname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    c_content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    c_post_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_comments", x => x.c_id);
                    table.ForeignKey(
                        name: "FK_comments_posts_c_post_id",
                        column: x => x.c_post_id,
                        principalTable: "posts",
                        principalColumn: "p_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_comments_c_post_id",
                table: "comments",
                column: "c_post_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "comments");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "posts");
        }
    }
}
