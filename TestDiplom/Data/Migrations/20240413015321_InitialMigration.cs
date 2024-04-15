using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace API.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    categoryId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    categoryName = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.categoryId);
                });

            migrationBuilder.CreateTable(
                name: "Image",
                columns: table => new
                {
                    userId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    userFirstName = table.Column<byte[]>(type: "bytea", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Image", x => x.userId);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    roleId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    roleName = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.roleId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    userId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    userFirstName = table.Column<string>(type: "text", nullable: false),
                    userSecondName = table.Column<string>(type: "text", nullable: false),
                    userThirdName = table.Column<string>(type: "text", nullable: true),
                    userLogin = table.Column<string>(type: "text", nullable: false),
                    userPassword = table.Column<string>(type: "text", nullable: false),
                    userGender = table.Column<bool>(type: "boolean", nullable: true),
                    userAge = table.Column<int>(type: "integer", nullable: true),
                    userCountry = table.Column<int>(type: "integer", nullable: true),
                    userRole = table.Column<int>(type: "integer", nullable: false),
                    userImage = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.userId);
                    table.ForeignKey(
                        name: "FK_Users_Image_userImage",
                        column: x => x.userImage,
                        principalTable: "Image",
                        principalColumn: "userId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Users_Roles_userRole",
                        column: x => x.userRole,
                        principalTable: "Roles",
                        principalColumn: "roleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Courses",
                columns: table => new
                {
                    courseId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    courseName = table.Column<string>(type: "text", nullable: false),
                    courseDesciption = table.Column<string>(type: "text", nullable: false),
                    courseData = table.Column<string>(type: "text", nullable: false),
                    courseText = table.Column<string>(type: "text", nullable: false),
                    courseTextRecom = table.Column<string>(type: "text", nullable: false),
                    courseTextWarning = table.Column<string>(type: "text", nullable: false),
                    courseCategory = table.Column<int>(type: "integer", nullable: false),
                    courseAuthor = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Courses", x => x.courseId);
                    table.ForeignKey(
                        name: "FK_Courses_Categories_courseCategory",
                        column: x => x.courseCategory,
                        principalTable: "Categories",
                        principalColumn: "categoryId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Courses_Users_courseAuthor",
                        column: x => x.courseAuthor,
                        principalTable: "Users",
                        principalColumn: "userId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Comment",
                columns: table => new
                {
                    commentId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    commentText = table.Column<string>(type: "text", nullable: false),
                    commentCourse = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comment", x => x.commentId);
                    table.ForeignKey(
                        name: "FK_Comment_Courses_commentCourse",
                        column: x => x.commentCourse,
                        principalTable: "Courses",
                        principalColumn: "courseId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Comment_commentCourse",
                table: "Comment",
                column: "commentCourse");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_courseAuthor",
                table: "Courses",
                column: "courseAuthor");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_courseCategory",
                table: "Courses",
                column: "courseCategory");

            migrationBuilder.CreateIndex(
                name: "IX_Users_userImage",
                table: "Users",
                column: "userImage");

            migrationBuilder.CreateIndex(
                name: "IX_Users_userRole",
                table: "Users",
                column: "userRole");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Comment");

            migrationBuilder.DropTable(
                name: "Courses");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Image");

            migrationBuilder.DropTable(
                name: "Roles");
        }
    }
}
