using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskTimeTracker.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class TaskItem_IsCompleted_Added_And_Other_Changes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsCompleted",
                table: "Tasks",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCompleted",
                table: "Tasks");
        }
    }
}
