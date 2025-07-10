using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Company.DAL.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddMeetingEntitiesFixed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // 1. Create Meetings table
            migrationBuilder.CreateTable(
                name: "Meetings",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: false),
                    StartTime = table.Column<DateTime>(nullable: false),
                    EndTime = table.Column<DateTime>(nullable: false),
                    Location = table.Column<string>(nullable: false),
                    OnlineLink = table.Column<string>(nullable: false),
                    IsRecurring = table.Column<bool>(nullable: false),
                    RecurrencePattern = table.Column<string>(nullable: true),
                    CreatedById = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Meetings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Meetings_Employees_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Employees",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Cascade);
                });

            // 2. Create MeetingDepartments table
            migrationBuilder.CreateTable(
                name: "MeetingDepartments",
                columns: table => new
                {
                    MeetingId = table.Column<int>(nullable: false),
                    DepartmentId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MeetingDepartments", x => new { x.MeetingId, x.DepartmentId });
                    table.ForeignKey(
                        name: "FK_MeetingDepartments_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MeetingDepartments_Meetings_MeetingId",
                        column: x => x.MeetingId,
                        principalTable: "Meetings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            // 3. Create MeetingProjects table
            migrationBuilder.CreateTable(
                name: "MeetingProjects",
                columns: table => new
                {
                    MeetingId = table.Column<int>(nullable: false),
                    ProjectId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MeetingProjects", x => new { x.MeetingId, x.ProjectId });
                    table.ForeignKey(
                        name: "FK_MeetingProjects_Meetings_MeetingId",
                        column: x => x.MeetingId,
                        principalTable: "Meetings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MeetingProjects_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            // 4. Create MeetingParticipants table
            migrationBuilder.CreateTable(
                name: "MeetingParticipants",
                columns: table => new
                {
                    MeetingId = table.Column<int>(nullable: false),
                    EmployeeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MeetingParticipants", x => new { x.MeetingId, x.EmployeeId });
                    table.ForeignKey(
                        name: "FK_MeetingParticipants_Meetings_MeetingId",
                        column: x => x.MeetingId,
                        principalTable: "Meetings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MeetingParticipants_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Restrict);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "MeetingParticipants");
            migrationBuilder.DropTable(name: "MeetingProjects");
            migrationBuilder.DropTable(name: "MeetingDepartments");
            migrationBuilder.DropTable(name: "Meetings");
        }
    }
}
