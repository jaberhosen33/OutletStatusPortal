using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OutletStatusPortal.Migrations
{
    public partial class outletstatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NewOutletInfos",
                columns: table => new
                {
                    Sl = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OutletCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OutletName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OfficeITSetup = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CouriarStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Vendor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OutletITSetup = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LinkStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SAPID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MailID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    POSID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EPSLive = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AssignPersons = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NewOutletInfos", x => x.Sl);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    StafId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PassWord = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.StafId);
                });

            migrationBuilder.CreateTable(
                name: "Arise_Problems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Sl = table.Column<int>(type: "int", nullable: false),
                    OutletSl = table.Column<int>(type: "int", nullable: false),
                    ProblemType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateBy = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Arise_Problems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Arise_Problems_NewOutletInfos_OutletSl",
                        column: x => x.OutletSl,
                        principalTable: "NewOutletInfos",
                        principalColumn: "Sl",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DeviceSetupStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Sl = table.Column<int>(type: "int", nullable: false),
                    OutletSl = table.Column<int>(type: "int", nullable: false),
                    DeviceType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WorkStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WorkBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeviceSetupStatuses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeviceSetupStatuses_NewOutletInfos_OutletSl",
                        column: x => x.OutletSl,
                        principalTable: "NewOutletInfos",
                        principalColumn: "Sl",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "StafId", "Date", "Name", "PassWord", "Phone", "Role" },
                values: new object[] { "l53335", new DateTime(2025, 7, 7, 16, 4, 5, 297, DateTimeKind.Local).AddTicks(1352), "Jaber Hosen", "1234", "01700000001", "Admin" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "StafId", "Date", "Name", "PassWord", "Phone", "Role" },
                values: new object[] { "l54445", new DateTime(2025, 7, 7, 16, 4, 5, 297, DateTimeKind.Local).AddTicks(1355), "Sadia Akter", "jaber hosen", "01700000002", "User" });

            migrationBuilder.CreateIndex(
                name: "IX_Arise_Problems_OutletSl",
                table: "Arise_Problems",
                column: "OutletSl");

            migrationBuilder.CreateIndex(
                name: "IX_DeviceSetupStatuses_OutletSl",
                table: "DeviceSetupStatuses",
                column: "OutletSl");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Arise_Problems");

            migrationBuilder.DropTable(
                name: "DeviceSetupStatuses");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "NewOutletInfos");
        }
    }
}
