using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TecnoMundo.CouponAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddSchemaGuidId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(table: "coupon", keyColumn: "id", keyValue: 104819);

            migrationBuilder.DeleteData(table: "coupon", keyColumn: "id", keyValue: 907498);

            migrationBuilder
                .AlterColumn<Guid>(
                    name: "id",
                    table: "coupon",
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
            migrationBuilder
                .AlterColumn<int>(
                    name: "id",
                    table: "coupon",
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

            migrationBuilder.InsertData(
                table: "coupon",
                columns: new[] { "id", "coupon_code", "discount_amount" },
                values: new object[,]
                {
                    { 104819, "GEEK_SHOPPING_10", 10f },
                    { 907498, "GEEK_SHOPPING_15", 15f }
                }
            );
        }
    }
}
