using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TecnoMundo.Identity.Migrations
{
    /// <inheritdoc />
    public partial class FirstMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase().Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder
                .CreateTable(
                    name: "role",
                    columns: table => new
                    {
                        Id = table
                            .Column<int>(type: "int", nullable: false)
                            .Annotation(
                                "MySql:ValueGenerationStrategy",
                                MySqlValueGenerationStrategy.IdentityColumn
                            ),
                        Name = table
                            .Column<string>(type: "longtext", nullable: false)
                            .Annotation("MySql:CharSet", "utf8mb4")
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_role", x => x.Id);
                    }
                )
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder
                .CreateTable(
                    name: "user",
                    columns: table => new
                    {
                        Id = table
                            .Column<int>(type: "int", nullable: false)
                            .Annotation(
                                "MySql:ValueGenerationStrategy",
                                MySqlValueGenerationStrategy.IdentityColumn
                            ),
                        UserName = table
                            .Column<string>(type: "varchar(80)", maxLength: 80, nullable: false)
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        LastName = table
                            .Column<string>(type: "varchar(80)", maxLength: 80, nullable: false)
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        Cpf = table
                            .Column<string>(type: "varchar(11)", maxLength: 11, nullable: false)
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        PhoneNumber = table
                            .Column<string>(type: "varchar(15)", maxLength: 15, nullable: false)
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        UserEmail = table
                            .Column<string>(type: "longtext", nullable: false)
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        EmailConfirmed = table.Column<bool>(type: "tinyint(1)", nullable: false),
                        Password = table
                            .Column<string>(type: "varchar(12)", maxLength: 12, nullable: false)
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        role_id = table.Column<int>(type: "int", nullable: false)
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_user", x => x.Id);
                        table.ForeignKey(
                            name: "FK_user_role_role_id",
                            column: x => x.role_id,
                            principalTable: "role",
                            principalColumn: "Id",
                            onDelete: ReferentialAction.Cascade
                        );
                    }
                )
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(name: "IX_user_role_id", table: "user", column: "role_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "user");

            migrationBuilder.DropTable(name: "role");
        }
    }
}
