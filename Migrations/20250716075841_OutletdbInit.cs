using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OutletStatusPortal.Migrations
{
    public partial class OutletdbInit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AfterOutletSetups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OutletName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OfficeITSetup = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CourierStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NetworkVendor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OutletITSetup = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LinkStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SapId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MailId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PosId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EPSLive = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AssignedPersons = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AfterOutletSetups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StockItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VendorName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OutletName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StockType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Pos = table.Column<int>(type: "int", nullable: false),
                    Om = table.Column<int>(type: "int", nullable: false),
                    Server = table.Column<int>(type: "int", nullable: false),
                    Router = table.Column<int>(type: "int", nullable: false),
                    Scanner = table.Column<int>(type: "int", nullable: false),
                    Icmo = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockItems", x => x.Id);
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
                name: "BeforeOutletSetUps",
                columns: table => new
                {
                    Sl = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OutletCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OutletName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StockType = table.Column<int>(type: "int", nullable: false),
                    StockItemId = table.Column<int>(type: "int", nullable: false),
                    Pos = table.Column<int>(type: "int", nullable: false),
                    Om = table.Column<int>(type: "int", nullable: false),
                    Server = table.Column<int>(type: "int", nullable: false),
                    Router = table.Column<int>(type: "int", nullable: false),
                    Scanner = table.Column<int>(type: "int", nullable: false),
                    Icmo = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BeforeOutletSetUps", x => x.Sl);
                    table.ForeignKey(
                        name: "FK_BeforeOutletSetUps_StockItems_StockItemId",
                        column: x => x.StockItemId,
                        principalTable: "StockItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StockTransactions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StockItemId = table.Column<int>(type: "int", nullable: false),
                    OperationType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OperationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PerformedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    OutletCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Pos = table.Column<int>(type: "int", nullable: false),
                    Om = table.Column<int>(type: "int", nullable: false),
                    Server = table.Column<int>(type: "int", nullable: false),
                    Router = table.Column<int>(type: "int", nullable: false),
                    Scanner = table.Column<int>(type: "int", nullable: false),
                    Icmo = table.Column<int>(type: "int", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StockTransactions_StockItems_StockItemId",
                        column: x => x.StockItemId,
                        principalTable: "StockItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DeviceSetupStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Sl = table.Column<int>(type: "int", nullable: false),
                    DeviceType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WorkStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WorkBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeviceSetupStatuses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeviceSetupStatuses_BeforeOutletSetUps_Sl",
                        column: x => x.Sl,
                        principalTable: "BeforeOutletSetUps",
                        principalColumn: "Sl",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "StafId", "Date", "Name", "PassWord", "Phone", "Role" },
                values: new object[] { "l53335", new DateTime(2025, 7, 16, 13, 58, 41, 85, DateTimeKind.Local).AddTicks(9634), "Jaber Hosen", "1234", "01700000001", "Admin" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "StafId", "Date", "Name", "PassWord", "Phone", "Role" },
                values: new object[] { "l54445", new DateTime(2025, 7, 16, 13, 58, 41, 85, DateTimeKind.Local).AddTicks(9637), "Sadia Akter", "jaber hosen", "01700000002", "User" });

            migrationBuilder.CreateIndex(
                name: "IX_BeforeOutletSetUps_StockItemId",
                table: "BeforeOutletSetUps",
                column: "StockItemId");

            migrationBuilder.CreateIndex(
                name: "IX_DeviceSetupStatuses_Sl",
                table: "DeviceSetupStatuses",
                column: "Sl");

            migrationBuilder.CreateIndex(
                name: "IX_StockTransactions_StockItemId",
                table: "StockTransactions",
                column: "StockItemId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AfterOutletSetups");

            migrationBuilder.DropTable(
                name: "DeviceSetupStatuses");

            migrationBuilder.DropTable(
                name: "StockTransactions");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "BeforeOutletSetUps");

            migrationBuilder.DropTable(
                name: "StockItems");
        }
    }
}
