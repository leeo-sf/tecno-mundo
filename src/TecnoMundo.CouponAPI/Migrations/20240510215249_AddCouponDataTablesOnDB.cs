﻿using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TecnoMundo.CouponAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddCouponDataTablesOnDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase().Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder
                .CreateTable(
                    name: "coupon",
                    columns: table => new
                    {
                        id = table
                            .Column<long>(type: "bigint", nullable: false)
                            .Annotation(
                                "MySql:ValueGenerationStrategy",
                                MySqlValueGenerationStrategy.IdentityColumn
                            ),
                        coupon_code = table
                            .Column<string>(type: "varchar(30)", maxLength: 30, nullable: false)
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        discount_amount = table.Column<float>(type: "float", nullable: false)
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_coupon", x => x.id);
                    }
                )
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "coupon");
        }
    }
}
