using Microsoft.EntityFrameworkCore.Migrations;

namespace CGBlockMarket.Migrations
{
    public partial class addIsdelete : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name:"IsDeleted",
                table:"User",
                defaultValue:0
                );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
