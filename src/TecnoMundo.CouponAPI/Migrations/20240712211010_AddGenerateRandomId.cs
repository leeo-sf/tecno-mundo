using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TecnoMundo.CouponAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddGenerateRandomId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(table: "coupon", keyColumn: "id", keyValue: 1L);

            migrationBuilder.DeleteData(table: "coupon", keyColumn: "id", keyValue: 2L);

            migrationBuilder
                .AlterColumn<int>(
                    name: "id",
                    table: "coupon",
                    type: "int",
                    nullable: false,
                    oldClrType: typeof(long),
                    oldType: "bigint"
                )
                .Annotation(
                    "MySql:ValueGenerationStrategy",
                    MySqlValueGenerationStrategy.IdentityColumn
                )
                .OldAnnotation(
                    "MySql:ValueGenerationStrategy",
                    MySqlValueGenerationStrategy.IdentityColumn
                );

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(table: "coupon", keyColumn: "id", keyValue: 104819);

            migrationBuilder.DeleteData(table: "coupon", keyColumn: "id", keyValue: 907498);

            migrationBuilder
                .AlterColumn<long>(
                    name: "id",
                    table: "coupon",
                    type: "bigint",
                    nullable: false,
                    oldClrType: typeof(int),
                    oldType: "int"
                )
                .Annotation(
                    "MySql:ValueGenerationStrategy",
                    MySqlValueGenerationStrategy.IdentityColumn
                )
                .OldAnnotation(
                    "MySql:ValueGenerationStrategy",
                    MySqlValueGenerationStrategy.IdentityColumn
                );

            migrationBuilder.InsertData(
                table: "coupon",
                columns: new[] { "id", "coupon_code", "discount_amount" },
                values: new object[,]
                {
                    { 1L, "GEEK_SHOPPING_10", 10f },
                    { 2L, "GEEK_SHOPPING_15", 15f }
                }
            );
        }
    }
}
