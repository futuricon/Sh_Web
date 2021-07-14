using Microsoft.EntityFrameworkCore.Migrations;

namespace Sh.Infrastructure.Migrations
{
    public partial class init2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Dossier",
                keyColumn: "Id",
                keyValue: "20210713125256581275");

            migrationBuilder.AlterColumn<string>(
                name: "DescriptionUz",
                table: "Dossier",
                type: "nvarchar(800)",
                maxLength: 800,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "DescriptionRu",
                table: "Dossier",
                type: "nvarchar(800)",
                maxLength: 800,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Dossier",
                type: "nvarchar(800)",
                maxLength: 800,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.InsertData(
                table: "Dossier",
                columns: new[] { "Id", "Address", "CVFilePath", "CoverPhotoPath", "Description", "DescriptionRu", "DescriptionUz", "Email", "FirstName", "FirstNameRu", "FirstNameUz", "IsAboutInfo", "LastName", "LastNameRu", "LastNameUz", "PhoneNumber", "Position", "PositionRu", "PositionUz" },
                values: new object[] { "20210713133932964814", "some address", "", "", "test", "тест", "test", "test@mail.com", "Shakhlo", "Шахло", "Shaxlo", true, "Ergasheva", "Эргашева", "Ergasheva", "+998908007060", "Pupil", "Ученица", "O'quvchi" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Dossier",
                keyColumn: "Id",
                keyValue: "20210713133932964814");

            migrationBuilder.AlterColumn<string>(
                name: "DescriptionUz",
                table: "Dossier",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(800)",
                oldMaxLength: 800);

            migrationBuilder.AlterColumn<string>(
                name: "DescriptionRu",
                table: "Dossier",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(800)",
                oldMaxLength: 800);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Dossier",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(800)",
                oldMaxLength: 800);

            migrationBuilder.InsertData(
                table: "Dossier",
                columns: new[] { "Id", "Address", "CVFilePath", "CoverPhotoPath", "Description", "DescriptionRu", "DescriptionUz", "Email", "FirstName", "FirstNameRu", "FirstNameUz", "IsAboutInfo", "LastName", "LastNameRu", "LastNameUz", "PhoneNumber", "Position", "PositionRu", "PositionUz" },
                values: new object[] { "20210713125256581275", "some address", "", "", "test", "тест", "test", "test@mail.com", "Shakhlo", "Шахло", "Shaxlo", true, "Ergasheva", "Эргашева", "Ergasheva", "+998908007060", "Pupil", "Ученица", "O'quvchi" });
        }
    }
}
