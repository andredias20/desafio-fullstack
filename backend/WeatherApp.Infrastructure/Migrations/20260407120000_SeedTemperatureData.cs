using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WeatherApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedTemperatureData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                INSERT INTO temperature_records (""CityName"", ""Latitude"", ""Longitude"", ""TemperatureCelsius"", ""RecordedAt"") VALUES
                ('São Paulo',       -23.5505, -46.6333, 22.4,  '2026-04-04 09:15:00+00'),
                ('Rio de Janeiro',  -22.9068, -43.1729, 30.1,  '2026-04-04 11:30:00+00'),
                ('Fortaleza',        -3.7172, -38.5433, 32.7,  '2026-04-04 13:00:00+00'),
                ('Curitiba',        -25.4284, -49.2733, 16.3,  '2026-04-04 15:45:00+00'),
                ('Manaus',           -3.1190, -60.0217, 33.0,  '2026-04-04 17:00:00+00'),
                ('Belo Horizonte',  -19.9167, -43.9345, 25.8,  '2026-04-05 08:20:00+00'),
                ('Salvador',        -12.9714, -38.5014, 29.4,  '2026-04-05 10:10:00+00'),
                ('Porto Alegre',    -30.0346, -51.2177, 18.7,  '2026-04-05 12:30:00+00'),
                ('Recife',           -8.0476, -34.8770, 31.2,  '2026-04-05 14:00:00+00'),
                ('Brasília',        -15.7801, -47.9292, 27.5,  '2026-04-05 16:45:00+00'),
                ('Belém',            -1.4558, -48.4902, 32.1,  '2026-04-06 09:00:00+00'),
                ('Goiânia',         -16.6869, -49.2648, 28.9,  '2026-04-06 11:15:00+00'),
                ('Florianópolis',   -27.5954, -48.5480, 20.3,  '2026-04-06 13:30:00+00'),
                ('Maceió',           -9.6658, -35.7350, 30.6,  '2026-04-06 15:00:00+00'),
                ('Natal',            -5.7945, -35.2110, 31.8,  '2026-04-06 17:20:00+00'),
                ('São Paulo',       -23.5505, -46.6333, 19.5,  '2026-04-07 08:45:00+00'),
                ('Curitiba',        -25.4284, -49.2733, 15.2,  '2026-04-07 10:00:00+00'),
                ('Rio de Janeiro',  -22.9068, -43.1729, 28.3,  '2026-04-07 12:15:00+00'),
                ('Manaus',           -3.1190, -60.0217, 33.0,  '2026-04-07 14:30:00+00'),
                ('Fortaleza',        -3.7172, -38.5433, 31.5,  '2026-04-07 16:00:00+00');
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                DELETE FROM temperature_records
                WHERE ""RecordedAt"" >= '2026-04-04' AND ""RecordedAt"" < '2026-04-08'
                  AND ""CityName"" IN (
                    'São Paulo', 'Rio de Janeiro', 'Fortaleza', 'Curitiba', 'Manaus',
                    'Belo Horizonte', 'Salvador', 'Porto Alegre', 'Recife', 'Brasília',
                    'Belém', 'Goiânia', 'Florianópolis', 'Maceió', 'Natal'
                  );
            ");
        }
    }
}
