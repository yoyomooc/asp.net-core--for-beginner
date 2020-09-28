using Microsoft.EntityFrameworkCore.Migrations;

namespace StudentManagement.RazorPage.Services.Migrations
{
    public partial class spGetStudentById : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string procedure = @"Create Procedure spGetStudentById
                            @Id int
                            as
                            Begin
                             Select * from Students
                             Where Id = @Id
                            End";
            migrationBuilder.Sql(procedure);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            string procedure = @"Drop procedure spGetStudentById";
            migrationBuilder.Sql(procedure);
        }
    }
}