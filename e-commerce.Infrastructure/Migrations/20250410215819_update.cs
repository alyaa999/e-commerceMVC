﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace e_commerce.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class update : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Seller",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Seller_ApplicationUserId",
                table: "Seller",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Seller_AspNetUsers_ApplicationUserId",
                table: "Seller",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Seller_AspNetUsers_ApplicationUserId",
                table: "Seller");

            migrationBuilder.DropIndex(
                name: "IX_Seller_ApplicationUserId",
                table: "Seller");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Seller");
        }
    }
}
