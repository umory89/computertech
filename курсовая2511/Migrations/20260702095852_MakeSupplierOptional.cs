using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace курсовая2511.Migrations
{
    /// <inheritdoc />
    public partial class MakeSupplierOptional : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Actiontype",
                table: "AssignmentHistories",
                newName: "ActionType");

            migrationBuilder.AlterColumn<Guid>(
                name: "SupplierId",
                table: "Equipment",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.UpdateData(
                table: "EquipmentTypes",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                column: "Description",
                value: "Приставка");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ActionType",
                table: "AssignmentHistories",
                newName: "Actiontype");

            migrationBuilder.AlterColumn<Guid>(
                name: "SupplierId",
                table: "Equipment",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "EquipmentTypes",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                column: "Description",
                value: "Коммутатор");
        }
    }
}
