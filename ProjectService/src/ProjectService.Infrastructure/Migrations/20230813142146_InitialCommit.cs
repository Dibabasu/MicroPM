using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCommit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "projects",
                columns: table => new
                {
                    projectid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    projectName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    projectDescription = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ownerid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    workflowid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    projectstatus = table.Column<int>(type: "int", maxLength: 20, nullable: false),
                    created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    createdby = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    modified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    modifiedby = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_projects", x => x.projectid);
                });

            migrationBuilder.CreateTable(
                name: "components",
                columns: table => new
                {
                    componentid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    projectid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    componentName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    componentDescription = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    createdby = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    modified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    modifiedby = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_components", x => new { x.componentid, x.projectid });
                    table.ForeignKey(
                        name: "FK_components_projects_projectid",
                        column: x => x.projectid,
                        principalTable: "projects",
                        principalColumn: "projectid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "projectusers",
                columns: table => new
                {
                    userid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    projectid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    userrole = table.Column<int>(type: "int", nullable: false),
                    created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    createdby = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    modified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    modifiedby = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_projectusers", x => new { x.userid, x.projectid });
                    table.ForeignKey(
                        name: "FK_projectusers_projects_projectid",
                        column: x => x.projectid,
                        principalTable: "projects",
                        principalColumn: "projectid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "idx_components_projectid",
                table: "components",
                column: "projectid");

            migrationBuilder.CreateIndex(
                name: "idx_projects_projectstatus",
                table: "projects",
                column: "projectstatus");

            migrationBuilder.CreateIndex(
                name: "idx_projects_workflowid",
                table: "projects",
                column: "workflowid");

            migrationBuilder.CreateIndex(
                name: "idx_projectusers_projectid",
                table: "projectusers",
                column: "projectid");

            migrationBuilder.CreateIndex(
                name: "idx_projectusers_userid",
                table: "projectusers",
                column: "userid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "components");

            migrationBuilder.DropTable(
                name: "projectusers");

            migrationBuilder.DropTable(
                name: "projects");
        }
    }
}
