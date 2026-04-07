using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WeatherApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddRecordedAtIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "ix_temperature_records_recorded_at",
                table: "temperature_records",
                column: "RecordedAt");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_temperature_records_recorded_at",
                table: "temperature_records");
        }
    }
}
