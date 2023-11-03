using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Violation_p2.Migrations
{
    /// <inheritdoc />
    public partial class add1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Aboutus",
                columns: table => new
                {
                    AboutuId = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    Imagepath = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    Title = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    Pargraph = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    Content = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Aboutus", x => x.AboutuId);
                });

            migrationBuilder.CreateTable(
                name: "Contacts",
                columns: table => new
                {
                    ContactId = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    Name = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    Subject = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    Message = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    Contactdate = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contacts", x => x.ContactId);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    RoleId = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    Rolename = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    RoleCategory = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.RoleId);
                });

            migrationBuilder.CreateTable(
                name: "Sliders",
                columns: table => new
                {
                    SliderId = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    Imagepath = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    Imagepath1 = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    Caption1 = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    Caption2 = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    Caption3 = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sliders", x => x.SliderId);
                });

            migrationBuilder.CreateTable(
                name: "Violationtypes",
                columns: table => new
                {
                    ViolationtypeId = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    Description = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    Basefineamount = table.Column<decimal>(type: "DECIMAL(18, 2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Violationtypes", x => x.ViolationtypeId);
                });

            migrationBuilder.CreateTable(
                name: "User1s",
                columns: table => new
                {
                    User1Id = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    Username = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    Password = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    Email = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    Imagepath = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    RoleId = table.Column<int>(type: "NUMBER(10)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User1s", x => x.User1Id);
                    table.ForeignKey(
                        name: "FK_User1s_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "RoleId");
                });

            migrationBuilder.CreateTable(
                name: "Blogs",
                columns: table => new
                {
                    BlogId = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    Title = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    Imagepath = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    Content = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    PublishDate = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    Userid = table.Column<int>(type: "NUMBER(10)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Blogs", x => x.BlogId);
                    table.ForeignKey(
                        name: "FK_Blogs_User1s_Userid",
                        column: x => x.Userid,
                        principalTable: "User1s",
                        principalColumn: "User1Id");
                });

            migrationBuilder.CreateTable(
                name: "Testimonials",
                columns: table => new
                {
                    TestimonialId = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    UserId = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    Content = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    Isapproved = table.Column<bool>(type: "NUMBER(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Testimonials", x => x.TestimonialId);
                    table.ForeignKey(
                        name: "FK_Testimonials_User1s_UserId",
                        column: x => x.UserId,
                        principalTable: "User1s",
                        principalColumn: "User1Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Vehicles",
                columns: table => new
                {
                    VehicleId = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    User1Id = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    Licenseplate = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    Model = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    Type = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    Color = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    Imagepath = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    Licenseexpirydate = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehicles", x => x.VehicleId);
                    table.ForeignKey(
                        name: "FK_Vehicles_User1s_User1Id",
                        column: x => x.User1Id,
                        principalTable: "User1s",
                        principalColumn: "User1Id");
                });

            migrationBuilder.CreateTable(
                name: "Violations",
                columns: table => new
                {
                    ViolationId = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    VehicleId = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    ViolationtypeId = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    Violationdate = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    Fineamount = table.Column<decimal>(type: "DECIMAL(18, 2)", nullable: true),
                    Ispaid = table.Column<decimal>(type: "DECIMAL(18, 2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Violations", x => x.ViolationId);
                    table.ForeignKey(
                        name: "FK_Violations_Vehicles_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "Vehicles",
                        principalColumn: "VehicleId");
                    table.ForeignKey(
                        name: "FK_Violations_Violationtypes_ViolationtypeId",
                        column: x => x.ViolationtypeId,
                        principalTable: "Violationtypes",
                        principalColumn: "ViolationtypeId");
                });

            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    PaymentId = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    ViolationId = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    VehicleId = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    Paymentdate = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    Paymentamount = table.Column<decimal>(type: "DECIMAL(18, 2)", nullable: true),
                    Paymentmethod = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.PaymentId);
                    table.ForeignKey(
                        name: "FK_Payments_Vehicles_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "Vehicles",
                        principalColumn: "VehicleId");
                    table.ForeignKey(
                        name: "FK_Payments_Violations_ViolationId",
                        column: x => x.ViolationId,
                        principalTable: "Violations",
                        principalColumn: "ViolationId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Blogs_Userid",
                table: "Blogs",
                column: "Userid");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_VehicleId",
                table: "Payments",
                column: "VehicleId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_ViolationId",
                table: "Payments",
                column: "ViolationId");

            migrationBuilder.CreateIndex(
                name: "IX_Testimonials_UserId",
                table: "Testimonials",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_User1s_RoleId",
                table: "User1s",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_User1Id",
                table: "Vehicles",
                column: "User1Id");

            migrationBuilder.CreateIndex(
                name: "IX_Violations_VehicleId",
                table: "Violations",
                column: "VehicleId");

            migrationBuilder.CreateIndex(
                name: "IX_Violations_ViolationtypeId",
                table: "Violations",
                column: "ViolationtypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Aboutus");

            migrationBuilder.DropTable(
                name: "Blogs");

            migrationBuilder.DropTable(
                name: "Contacts");

            migrationBuilder.DropTable(
                name: "Payments");

            migrationBuilder.DropTable(
                name: "Sliders");

            migrationBuilder.DropTable(
                name: "Testimonials");

            migrationBuilder.DropTable(
                name: "Violations");

            migrationBuilder.DropTable(
                name: "Vehicles");

            migrationBuilder.DropTable(
                name: "Violationtypes");

            migrationBuilder.DropTable(
                name: "User1s");

            migrationBuilder.DropTable(
                name: "Roles");
        }
    }
}
