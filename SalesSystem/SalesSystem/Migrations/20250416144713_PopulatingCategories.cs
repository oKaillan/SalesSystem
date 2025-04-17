using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SalesSystem.Migrations
{
    /// <inheritdoc />
    public partial class PopulatingCategories : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData("ProductCategories", "Name", "Electronic", schema: null);
            migrationBuilder.InsertData("ProductCategories", "Name", "Beauty", schema: null);
            migrationBuilder.InsertData("ProductCategories", "Name", "Health", schema: null);
            migrationBuilder.InsertData("ProductCategories", "Name", "Tools", schema: null);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData("ProductCategories", "Name", "Electronic", schema: null);
            migrationBuilder.DeleteData("ProductCategories", "Name", "Beauty", schema: null);
            migrationBuilder.DeleteData("ProductCategories", "Name", "Health", schema: null);
            migrationBuilder.DeleteData("ProductCategories", "Name", "Tools", schema: null);
        }
    }
}
