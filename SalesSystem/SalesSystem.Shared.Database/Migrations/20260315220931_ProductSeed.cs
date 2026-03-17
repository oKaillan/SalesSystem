using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SalesSystem.Migrations
{
    /// <inheritdoc />
    public partial class ProductSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Name", "Quantity", "Price" },
                values: new object[,]
                {
            {100, "Gaming Mouse", 20, 199.90},
            {101, "Mechanical Keyboard", 15, 399.90},
            {102, "HD Webcam", 25, 149.90},
            {103, "Wireless Headphones", 30, 249.90},
            {104, "Bluetooth Speaker", 40, 189.90},
            {105, "USB-C Charger", 70, 49.90},
            {106, "Power Bank", 55, 79.90},
            {107, "HDMI Cable", 80, 19.90},
            {108, "Gaming Chair", 10, 1299.90},
            {109, "LED Desk Lamp", 35, 89.90},

            {110, "Face Moisturizer", 45, 59.90},
            {111, "Anti Aging Cream", 28, 89.90},
            {112, "Body Lotion", 50, 39.90},
            {113, "Sunscreen SPF50", 65, 49.90},
            {114, "Hair Conditioner", 40, 29.90},
            {115, "Hair Dryer", 18, 179.90},
            {116, "Hair Straightener", 22, 159.90},
            {117, "Makeup Kit", 30, 99.90},
            {118, "Nail Care Kit", 40, 49.90},
            {119, "Electric Shaver", 26, 189.90},

            {120, "Vitamin C Supplement", 50, 39.90},
            {121, "Omega 3 Capsules", 40, 49.90},
            {122, "Multivitamin Tablets", 75, 34.90},
            {123, "Digital Thermometer", 30, 59.90},
            {124, "Blood Pressure Monitor", 18, 199.90},
            {125, "First Aid Kit", 60, 69.90},
            {126, "Hand Sanitizer", 100, 14.90},
            {127, "Yoga Mat", 50, 79.90},
            {128, "Fitness Tracker", 22, 199.90},
            {129, "Protein Shaker", 80, 19.90},

            {130, "Professional Hammer", 35, 59.90},
            {131, "Screwdriver Set", 60, 29.90},
            {132, "Cordless Drill", 12, 299.90},
            {133, "Adjustable Wrench", 32, 49.90},
            {134, "Measuring Tape", 70, 24.90},
            {135, "Electric Sander", 14, 279.90},
            {136, "Circular Saw", 10, 399.90},
            {137, "Toolbox Kit", 20, 349.90},
            {138, "Safety Glasses", 55, 24.90},
            {139, "Protective Gloves", 60, 19.90},

            {140, "Router Dual Band", 22, 229.90},
            {141, "External SSD 1TB", 16, 599.90},
            {142, "Portable Hard Drive 2TB", 14, 499.90},
            {143, "Laptop Cooling Pad", 24, 89.90},
            {144, "USB Hub", 50, 49.90},
            {145, "Wireless Charger", 38, 79.90},
            {146, "Smart Light Bulb", 60, 59.90},
            {147, "Smart Plug", 70, 49.90},
            {148, "Security Camera", 18, 299.90},
            {149, "Air Humidifier", 15, 139.90},

            {150, "Massage Gun", 12, 349.90},
            {151, "Resistance Bands", 45, 39.90},
            {152, "Foam Roller", 30, 59.90},
            {153, "Running Water Bottle", 65, 29.90},
            {154, "Industrial Flashlight", 45, 59.90},
            {155, "Extension Cord 10m", 35, 44.90},
            {156, "Glue Gun", 28, 39.90},
            {157, "Instant Adhesive", 90, 9.90},
            {158, "Work Helmet", 25, 69.90},
            {159, "Smart Scale", 20, 119.90},

            {160, "Ring Light", 27, 119.90},
            {161, "Desk Microphone", 18, 149.90},
            {162, "Streaming Webcam Pro", 12, 299.90},
            {163, "Gaming Mousepad XL", 33, 59.90},
            {164, "RGB LED Strip", 48, 39.90}
                });

            migrationBuilder.InsertData(
    table: "ProductProductCategory",
    columns: new[] { "ProductsId", "CategoriesId" },
    values: new object[,]
    {
        {100,2},
        {101,2},
        {102,2},
        {103,2},
        {104,2},
        {105,2},
        {106,2},
        {107,2},
        {108,2},
        {109,2},

        {110,1},
        {111,1},
        {112,1},
        {113,1},
        {114,1},
        {115,1},
        {116,1},
        {117,1},
        {118,1},
        {119,1},

        {120,3},
        {121,3},
        {122,3},
        {123,3},
        {124,3},
        {125,3},
        {126,3},
        {127,3},
        {128,3},
        {129,3},

        {130,4},
        {131,4},
        {132,4},
        {133,4},
        {134,4},
        {135,4},
        {136,4},
        {137,4},
        {138,4},
        {139,4},

        {140,2},
        {141,2},
        {142,2},
        {143,2},
        {144,2},
        {145,2},
        {146,2},
        {147,2},
        {148,2},
        {149,3},

        {150,3},
        {151,3},
        {152,3},
        {153,3},
        {154,4},
        {155,4},
        {156,4},
        {157,4},
        {158,4},
        {159,3},

        {160,2},
        {161,2},
        {162,2},
        {163,2},
        {164,2}
    });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            for (int i = 100; i <= 164; i++)
            {
                migrationBuilder.DeleteData(
                    table: "Products",
                    keyColumn: "Id",
                    keyValue: i);
            }
        }
    }
}
