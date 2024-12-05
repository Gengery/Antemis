using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Antemis.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "persons",
                columns: table => new
                {
                    inn = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    surname = table.Column<string>(type: "text", nullable: true),
                    patronimic = table.Column<string>(type: "text", nullable: true),
                    gender = table.Column<char>(type: "character(1)", maxLength: 1, nullable: false),
                    birthdate = table.Column<DateOnly>(type: "date", nullable: true),
                    descryption = table.Column<string>(type: "text", nullable: true, defaultValueSql: "''::text")
                },
                constraints: table =>
                {
                    table.PrimaryKey("persons_pkey", x => x.inn);
                });

            migrationBuilder.CreateTable(
                name: "works",
                columns: table => new
                {
                    workid = table.Column<int>(type: "integer", nullable: false),
                    workname = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("works_pkey", x => x.workid);
                });

            migrationBuilder.CreateTable(
                name: "hotels",
                columns: table => new
                {
                    hotelid = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    hotelname = table.Column<string>(type: "text", nullable: false),
                    hotelinn = table.Column<string>(type: "text", nullable: false),
                    hoteldirectorinn = table.Column<string>(type: "text", nullable: false),
                    hotelownerinn = table.Column<string>(type: "text", nullable: false),
                    hotelimage = table.Column<string>(type: "text", nullable: true, defaultValueSql: "'hotel_icon.png'::text"),
                    hotelpassword = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("hotels_pkey", x => x.hotelid);
                    table.ForeignKey(
                        name: "hotels_hoteldirectorinn_fkey",
                        column: x => x.hoteldirectorinn,
                        principalTable: "persons",
                        principalColumn: "inn");
                    table.ForeignKey(
                        name: "hotels_hotelownerinn_fkey",
                        column: x => x.hotelownerinn,
                        principalTable: "persons",
                        principalColumn: "inn");
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    userid = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    inn = table.Column<string>(type: "text", nullable: false),
                    userlogin = table.Column<string>(type: "text", nullable: false),
                    imagename = table.Column<string>(type: "text", nullable: true, defaultValueSql: "'user_icon.png'::text"),
                    userpassword = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("users_pkey", x => x.userid);
                    table.ForeignKey(
                        name: "users_inn_fkey",
                        column: x => x.inn,
                        principalTable: "persons",
                        principalColumn: "inn");
                });

            migrationBuilder.CreateTable(
                name: "rooms",
                columns: table => new
                {
                    roomnumber = table.Column<int>(type: "integer", nullable: false),
                    hotelid = table.Column<int>(type: "integer", nullable: false),
                    roomdescryprion = table.Column<string>(type: "text", nullable: true, defaultValueSql: "'Информация не добавлена'::text"),
                    placesamount = table.Column<int>(type: "integer", nullable: true, defaultValue: 2),
                    priceforday = table.Column<int>(type: "integer", nullable: false),
                    isavaible = table.Column<bool>(type: "boolean", nullable: true, defaultValue: true),
                    imagename = table.Column<string>(type: "text", nullable: true, defaultValueSql: "'room_icon.png'::text")
                },
                constraints: table =>
                {
                    table.PrimaryKey("rooms_primary_key_check", x => new { x.hotelid, x.roomnumber });
                    table.ForeignKey(
                        name: "rooms_hotelid_fkey",
                        column: x => x.hotelid,
                        principalTable: "hotels",
                        principalColumn: "hotelid");
                });

            migrationBuilder.CreateTable(
                name: "workers",
                columns: table => new
                {
                    inn = table.Column<string>(type: "text", nullable: false),
                    hotelid = table.Column<int>(type: "integer", nullable: false),
                    imagename = table.Column<string>(type: "text", nullable: true, defaultValueSql: "'user_icon.png'::text"),
                    workid = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("workers_pkey", x => x.inn);
                    table.ForeignKey(
                        name: "workers_hotelid_fkey",
                        column: x => x.hotelid,
                        principalTable: "hotels",
                        principalColumn: "hotelid");
                    table.ForeignKey(
                        name: "workers_inn_fkey",
                        column: x => x.inn,
                        principalTable: "persons",
                        principalColumn: "inn");
                    table.ForeignKey(
                        name: "workers_workid_fkey",
                        column: x => x.workid,
                        principalTable: "works",
                        principalColumn: "workid");
                });

            migrationBuilder.CreateTable(
                name: "customers",
                columns: table => new
                {
                    customerinn = table.Column<string>(type: "text", nullable: false),
                    roomnumber = table.Column<int>(type: "integer", nullable: false),
                    hotelid = table.Column<int>(type: "integer", nullable: false),
                    arrivaldate = table.Column<DateOnly>(type: "date", nullable: false, defaultValueSql: "CURRENT_DATE"),
                    leavingdate = table.Column<DateOnly>(type: "date", nullable: false),
                    prepayment = table.Column<int>(type: "integer", nullable: true, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("customers_pkey", x => x.customerinn);
                    table.ForeignKey(
                        name: "cust_to_rooms_ref",
                        columns: x => new { x.hotelid, x.roomnumber },
                        principalTable: "rooms",
                        principalColumns: new[] { "hotelid", "roomnumber" });
                    table.ForeignKey(
                        name: "customers_customerinn_fkey",
                        column: x => x.customerinn,
                        principalTable: "persons",
                        principalColumn: "inn");
                    table.ForeignKey(
                        name: "customers_hotelid_fkey",
                        column: x => x.hotelid,
                        principalTable: "hotels",
                        principalColumn: "hotelid");
                });

            migrationBuilder.CreateTable(
                name: "reservations",
                columns: table => new
                {
                    roomnumber = table.Column<int>(type: "integer", nullable: false),
                    hotelid = table.Column<int>(type: "integer", nullable: false),
                    customerinn = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.ForeignKey(
                        name: "res_to_rooms_ref",
                        columns: x => new { x.hotelid, x.roomnumber },
                        principalTable: "rooms",
                        principalColumns: new[] { "hotelid", "roomnumber" });
                    table.ForeignKey(
                        name: "reservations_customerinn_fkey",
                        column: x => x.customerinn,
                        principalTable: "customers",
                        principalColumn: "customerinn");
                    table.ForeignKey(
                        name: "reservations_hotelid_fkey",
                        column: x => x.hotelid,
                        principalTable: "hotels",
                        principalColumn: "hotelid");
                });

            migrationBuilder.CreateIndex(
                name: "IX_customers_hotelid_roomnumber",
                table: "customers",
                columns: new[] { "hotelid", "roomnumber" });

            migrationBuilder.CreateIndex(
                name: "hotels_hotelinn_key",
                table: "hotels",
                column: "hotelinn",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_hotels_hoteldirectorinn",
                table: "hotels",
                column: "hoteldirectorinn");

            migrationBuilder.CreateIndex(
                name: "IX_hotels_hotelownerinn",
                table: "hotels",
                column: "hotelownerinn");

            migrationBuilder.CreateIndex(
                name: "IX_reservations_customerinn",
                table: "reservations",
                column: "customerinn");

            migrationBuilder.CreateIndex(
                name: "IX_reservations_hotelid_roomnumber",
                table: "reservations",
                columns: new[] { "hotelid", "roomnumber" });

            migrationBuilder.CreateIndex(
                name: "users_inn_key",
                table: "users",
                column: "inn",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_workers_hotelid",
                table: "workers",
                column: "hotelid");

            migrationBuilder.CreateIndex(
                name: "IX_workers_workid",
                table: "workers",
                column: "workid");

            migrationBuilder.CreateIndex(
                name: "works_workname_key",
                table: "works",
                column: "workname",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "reservations");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "workers");

            migrationBuilder.DropTable(
                name: "customers");

            migrationBuilder.DropTable(
                name: "works");

            migrationBuilder.DropTable(
                name: "rooms");

            migrationBuilder.DropTable(
                name: "hotels");

            migrationBuilder.DropTable(
                name: "persons");
        }
    }
}
