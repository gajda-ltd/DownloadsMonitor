namespace DownloadsMonitor.Migrations
{
    using System;
    using System.Diagnostics.Contracts;

    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            Contract.Assert(migrationBuilder != null);
            migrationBuilder.CreateTable(
                name: "Entries",
                columns: table => new
                {
                    Id = table.Column<Guid>(),
                    FileName = table.Column<string>(maxLength: 256),
                    Length = table.Column<long>(),
                    MD5 = table.Column<string>(maxLength: 48)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Entries", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            Contract.Assert(migrationBuilder != null);
            migrationBuilder.DropTable(name: "Entries");
        }
    }
}
