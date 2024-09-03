using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TecnoMundo.Identity.Migrations
{
    /// <inheritdoc />
    public partial class NewSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(name: "FK_user_role_role_id", table: "user");

            migrationBuilder.DropTable(name: "role");

            migrationBuilder.DropIndex(name: "IX_user_role_id", table: "user");

            migrationBuilder.RenameColumn(name: "Id", table: "user", newName: "id");

            migrationBuilder.RenameColumn(name: "role_id", table: "user", newName: "Role");

            migrationBuilder
                .AlterColumn<Guid>(
                    name: "id",
                    table: "user",
                    type: "char(36)",
                    nullable: false,
                    collation: "ascii_general_ci",
                    oldClrType: typeof(int),
                    oldType: "int"
                )
                .OldAnnotation(
                    "MySql:ValueGenerationStrategy",
                    MySqlValueGenerationStrategy.IdentityColumn
                );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(name: "id", table: "user", newName: "Id");

            migrationBuilder.RenameColumn(name: "Role", table: "user", newName: "role_id");

            migrationBuilder
                .AlterColumn<int>(
                    name: "Id",
                    table: "user",
                    type: "int",
                    nullable: false,
                    oldClrType: typeof(Guid),
                    oldType: "char(36)"
                )
                .Annotation(
                    "MySql:ValueGenerationStrategy",
                    MySqlValueGenerationStrategy.IdentityColumn
                )
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

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

            migrationBuilder.CreateIndex(name: "IX_user_role_id", table: "user", column: "role_id");

            migrationBuilder.AddForeignKey(
                name: "FK_user_role_role_id",
                table: "user",
                column: "role_id",
                principalTable: "role",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade
            );
        }
    }
}
