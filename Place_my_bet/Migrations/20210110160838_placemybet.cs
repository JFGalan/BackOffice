using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Place_my_bet.Migrations
{
    public partial class placemybet : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Eventos",
                columns: table => new
                {
                    EventoId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Equipo_Local = table.Column<string>(nullable: false),
                    Equipo_Visitante = table.Column<string>(nullable: false),
                    Dia = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Eventos", x => x.EventoId);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    EmailId = table.Column<string>(nullable: false),
                    Nombre = table.Column<string>(nullable: false),
                    Apellidos = table.Column<string>(nullable: false),
                    Edad = table.Column<int>(nullable: false),
                    Fecha_Registro = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.EmailId);
                });

            migrationBuilder.CreateTable(
                name: "Mercados",
                columns: table => new
                {
                    MercadoId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Tipo_over_under = table.Column<double>(nullable: false),
                    Cuota_Over = table.Column<double>(nullable: false),
                    Cuota_Under = table.Column<double>(nullable: false),
                    Dinero_Over = table.Column<double>(nullable: false),
                    Dinero_Under = table.Column<double>(nullable: false),
                    Bloqueado = table.Column<bool>(nullable: false),
                    EventoId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mercados", x => x.MercadoId);
                    table.ForeignKey(
                        name: "FK_Mercados_Eventos_EventoId",
                        column: x => x.EventoId,
                        principalTable: "Eventos",
                        principalColumn: "EventoId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Cuentas",
                columns: table => new
                {
                    TarjetaId = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nombre_Banco = table.Column<string>(nullable: false),
                    Saldo_Actual = table.Column<double>(nullable: false),
                    UsuarioId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cuentas", x => x.TarjetaId);
                    table.ForeignKey(
                        name: "FK_Cuentas_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "EmailId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Apuestas",
                columns: table => new
                {
                    ApuestaId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Tipo_Apuesta = table.Column<string>(nullable: false),
                    Tipo_Cuota = table.Column<double>(nullable: false),
                    Dinero_Apostado = table.Column<double>(nullable: false),
                    Fecha = table.Column<DateTime>(nullable: false),
                    MercadoId = table.Column<int>(nullable: false),
                    UsuarioId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Apuestas", x => x.ApuestaId);
                    table.ForeignKey(
                        name: "FK_Apuestas_Mercados_MercadoId",
                        column: x => x.MercadoId,
                        principalTable: "Mercados",
                        principalColumn: "MercadoId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Apuestas_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "EmailId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Eventos",
                columns: new[] { "EventoId", "Dia", "Equipo_Local", "Equipo_Visitante" },
                values: new object[,]
                {
                    { 1, "2021/01/10", "Valencia C.F", "F.C Barcelona" },
                    { 2, "2021/01/10", "Cádiz C.F", "Madrid C.F" },
                    { 3, "2021/01/10", "Levante C.F", "Getafe C.F" }
                });

            migrationBuilder.InsertData(
                table: "Usuarios",
                columns: new[] { "EmailId", "Apellidos", "Edad", "Fecha_Registro", "Nombre" },
                values: new object[,]
                {
                    { "Alba@yahoo.com", "Keylin Abradelo", 33, "2020/11/12", "Alba" },
                    { "Gamelin@gmail.com", "Gamelín Prieto", 21, "2020/11/12", "Pedro" },
                    { "Leon@gmail.com", "Valiente López", 41, "2020/11/13", "León" },
                    { "Lupo@icloud.com", "Cabezali García", 24, "2020/11/13", "Lobo" },
                    { "Yelstin@icloud.com", "Huiri Kabeut", 18, "2020/11/14", "Yelstin" }
                });

            migrationBuilder.InsertData(
                table: "Cuentas",
                columns: new[] { "TarjetaId", "Nombre_Banco", "Saldo_Actual", "UsuarioId" },
                values: new object[,]
                {
                    { 5409682368847308L, "IGN", 2527.6999999999998, "Alba@yahoo.com" },
                    { 5168565334325460L, "BBVA", 299.12, "Gamelin@gmail.com" },
                    { 5203550659869059L, "Santander", 1527.7, "Leon@gmail.com" },
                    { 5219971858205527L, "Bankia", 3359.1199999999999, "Lupo@icloud.com" },
                    { 5328163982660763L, "La Caixa", 1257.22, "Yelstin@icloud.com" }
                });

            migrationBuilder.InsertData(
                table: "Mercados",
                columns: new[] { "MercadoId", "Bloqueado", "Cuota_Over", "Cuota_Under", "Dinero_Over", "Dinero_Under", "EventoId", "Tipo_over_under" },
                values: new object[,]
                {
                    { 1, false, 1.8999999999999999, 1.8999999999999999, 100.0, 100.0, 1, 2.5 },
                    { 2, false, 2.2200000000000002, 1.6599999999999999, 150.0, 200.0, 2, 3.5 },
                    { 3, false, 1.8999999999999999, 1.8999999999999999, 450.0, 450.0, 3, 1.5 },
                    { 4, false, 1.8999999999999999, 1.8999999999999999, 900.0, 900.0, 3, 1.5 }
                });

            migrationBuilder.InsertData(
                table: "Apuestas",
                columns: new[] { "ApuestaId", "Dinero_Apostado", "Fecha", "MercadoId", "Tipo_Apuesta", "Tipo_Cuota", "UsuarioId" },
                values: new object[,]
                {
                    { 1, 50.0, new DateTime(2021, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "OVER", 1.8999999999999999, "Alba@yahoo.com" },
                    { 2, 50.0, new DateTime(2021, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, "UNDER", 1.6599999999999999, "Lupo@icloud.com" },
                    { 3, 50.0, new DateTime(2021, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, "OVER", 1.8999999999999999, "Yelstin@icloud.com" },
                    { 4, 50.0, new DateTime(2021, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, "OVER", 1.8999999999999999, "Gamelin@gmail.com" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Apuestas_MercadoId",
                table: "Apuestas",
                column: "MercadoId");

            migrationBuilder.CreateIndex(
                name: "IX_Apuestas_UsuarioId",
                table: "Apuestas",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Cuentas_UsuarioId",
                table: "Cuentas",
                column: "UsuarioId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Mercados_EventoId",
                table: "Mercados",
                column: "EventoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Apuestas");

            migrationBuilder.DropTable(
                name: "Cuentas");

            migrationBuilder.DropTable(
                name: "Mercados");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropTable(
                name: "Eventos");
        }
    }
}
