﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Xyzies.Notification.Data.Migrations
{
    public partial class CreateMessageTemplateTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TypeOfMessages",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Type = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TypeOfMessages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MessageTemplates",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    CreateOn = table.Column<DateTime>(nullable: false),
                    Cause = table.Column<string>(nullable: false),
                    Subject = table.Column<string>(nullable: true),
                    MessageBody = table.Column<string>(nullable: true),
                    TypeOfMessageId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MessageTemplates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MessageTemplates_TypeOfMessages_TypeOfMessageId",
                        column: x => x.TypeOfMessageId,
                        principalTable: "TypeOfMessages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MessageTemplates_TypeOfMessageId",
                table: "MessageTemplates",
                column: "TypeOfMessageId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MessageTemplates");

            migrationBuilder.DropTable(
                name: "TypeOfMessages");
        }
    }
}
