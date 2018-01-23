using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AddressBook.Migrations.AddressBookDb
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ContactPerson",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Birthdate = table.Column<DateTime>(nullable: false),
                    Email = table.Column<string>(nullable: false),
                    GenderType = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    Phone = table.Column<string>(maxLength: 12, nullable: true),
                    Surname = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactPerson", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Address",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AddressType = table.Column<int>(nullable: false),
                    City = table.Column<string>(nullable: true),
                    ContactPersonId = table.Column<int>(nullable: true),
                    Street = table.Column<string>(maxLength: 100, nullable: false),
                    StreetAdd = table.Column<string>(nullable: true),
                    ZipCode = table.Column<string>(maxLength: 6, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Address", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Address_ContactPerson_ContactPersonId",
                        column: x => x.ContactPersonId,
                        principalTable: "ContactPerson",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Address_ContactPersonId",
                table: "Address",
                column: "ContactPersonId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Address");

            migrationBuilder.DropTable(
                name: "ContactPerson");
        }
    }
}
