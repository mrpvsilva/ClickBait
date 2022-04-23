using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClickBait.Infra.Data.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Posts",
                columns: table => new
                {
                    Id = table.Column<string>(type: "Varchar(36)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Title = table.Column<string>(type: "Varchar(100)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "Varchar(250)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Posts", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ClicksCounts",
                columns: table => new
                {
                    PostId = table.Column<string>(type: "Varchar(36)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Qtd = table.Column<long>(type: "BIGINT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClicksCounts", x => x.PostId);
                    table.ForeignKey(
                        name: "FK_ClicksCounts_Posts_PostId",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "Posts",
                columns: new[] { "Id", "Description", "Title" },
                values: new object[,]
                {
                    { "178c25b0-c01d-4e37-8a00-81f61022777a", "Dignissimos qui culpa dolorum perspiciatis laborum fugiat ut qui voluptate ut voluptatem dolorem perspiciatis et debitis non facilis eos at.", "Est qui aperiam." },
                    { "33a2c323-54bc-4c74-8651-66f0cfd3f006", "Accusamus nobis est optio quaerat sapiente consequatur vel voluptas quidem sed quasi accusantium possimus tenetur odio ipsum pariatur ducimus incidunt.", "Maxime explicabo earum." },
                    { "93b1b57f-fea6-481a-8802-9d55a0eb351b", "Dolorem natus maiores incidunt facere distinctio veniam sunt quod et dolorum aut autem laboriosam debitis quae vel voluptatum non quis.", "Asperiores ad et." },
                    { "9d4d64c6-8ae9-4950-b935-5577db3e4e87", "Incidunt praesentium accusantium aliquam fugit soluta nisi repellat repudiandae nam veritatis voluptate est et eligendi harum eligendi voluptas est maiores.", "Aut nisi qui." },
                    { "a2504a1c-2724-4b8e-9aa7-a71341626d2c", "Sit aperiam blanditiis magnam sunt delectus dolorem aliquam a soluta amet cumque ad et dolores necessitatibus qui quasi aut sunt.", "Rem occaecati inventore." },
                    { "a72fdf55-0a58-426d-b7c6-f092f765d8ad", "Quod ut totam ea fuga accusamus sed eos sapiente corrupti optio necessitatibus aut quia doloribus at et eos accusantium tenetur.", "Culpa quas est." },
                    { "b2660579-a3b8-4ae2-be7c-c16154ba4256", "Quisquam aut possimus voluptatibus quia at ea aperiam et id deleniti similique autem sit voluptas ex quidem facilis et vel.", "Consequuntur magni enim." },
                    { "c0c424c3-dfe8-4953-b025-7ba936c9bd49", "Et labore quis dolor in architecto nobis ut odit id delectus voluptas tenetur accusamus voluptas id accusantium commodi enim quis.", "Voluptatum tempore quidem." },
                    { "cc5eabc9-513e-4a8a-86ce-df794673317b", "Est quam aut voluptatem qui commodi consectetur consequatur est velit quia ut pariatur sed officia sed voluptatem dolorem eaque provident.", "Repellendus numquam dolorem." },
                    { "e9d1d8e1-3691-40c4-95ee-79afc3db7e70", "Ea eos odit est magni modi est dolorem numquam fugit exercitationem quo quo qui illo dignissimos fugiat quo aliquid non.", "Non recusandae ut." }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Posts_Title",
                table: "Posts",
                column: "Title",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClicksCounts");

            migrationBuilder.DropTable(
                name: "Posts");
        }
    }
}
