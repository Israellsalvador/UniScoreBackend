using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace UniScore.API.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Evento",
                columns: table => new
                {
                    IdEvento = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NomeEvento = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DescricaoEvento = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Local = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DataInicio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataFim = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Ativo = table.Column<bool>(type: "bit", nullable: false),
                    NotaMinima = table.Column<float>(type: "real", nullable: false),
                    NotaMaxima = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Evento", x => x.IdEvento);
                });

            migrationBuilder.CreateTable(
                name: "Pessoa",
                columns: table => new
                {
                    IdPessoa = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NomePessoa = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Usuario = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Senha = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pessoa", x => x.IdPessoa);
                });

            migrationBuilder.CreateTable(
                name: "Projeto",
                columns: table => new
                {
                    IdProjeto = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NomeProjeto = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DescricaoProjeto = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Integrantes = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Turma = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    NotaFinal = table.Column<float>(type: "real", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projeto", x => x.IdProjeto);
                });

            migrationBuilder.CreateTable(
                name: "TipoUsuario",
                columns: table => new
                {
                    IdTipoUsuario = table.Column<byte>(type: "tinyint", nullable: false),
                    PapelUsuario = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoUsuario", x => x.IdTipoUsuario);
                });

            migrationBuilder.CreateTable(
                name: "CriterioAvaliacao",
                columns: table => new
                {
                    IdCriterio = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdEvento = table.Column<int>(type: "int", nullable: false),
                    NomeCriterio = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CriterioAvaliacao", x => x.IdCriterio);
                    table.ForeignKey(
                        name: "FK_CriterioAvaliacao_Evento_IdEvento",
                        column: x => x.IdEvento,
                        principalTable: "Evento",
                        principalColumn: "IdEvento",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Avaliacao",
                columns: table => new
                {
                    IdAvaliacao = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdPessoa = table.Column<int>(type: "int", nullable: false),
                    IdProjeto = table.Column<int>(type: "int", nullable: false),
                    DataAvaliacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Finalizada = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Avaliacao", x => x.IdAvaliacao);
                    table.ForeignKey(
                        name: "FK_Avaliacao_Pessoa_IdPessoa",
                        column: x => x.IdPessoa,
                        principalTable: "Pessoa",
                        principalColumn: "IdPessoa",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Avaliacao_Projeto_IdProjeto",
                        column: x => x.IdProjeto,
                        principalTable: "Projeto",
                        principalColumn: "IdProjeto",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Evento_Projeto",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdEvento = table.Column<int>(type: "int", nullable: false),
                    IdProjeto = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Evento_Projeto", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Evento_Projeto_Evento_IdEvento",
                        column: x => x.IdEvento,
                        principalTable: "Evento",
                        principalColumn: "IdEvento",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Evento_Projeto_Projeto_IdProjeto",
                        column: x => x.IdProjeto,
                        principalTable: "Projeto",
                        principalColumn: "IdProjeto",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InformacoesComplementares",
                columns: table => new
                {
                    IdInformacao = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdProjeto = table.Column<int>(type: "int", nullable: false),
                    InformacaoComplementar = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InformacoesComplementares", x => x.IdInformacao);
                    table.ForeignKey(
                        name: "FK_InformacoesComplementares_Projeto_IdProjeto",
                        column: x => x.IdProjeto,
                        principalTable: "Projeto",
                        principalColumn: "IdProjeto",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Pessoa_TipoUsuario",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdPessoa = table.Column<int>(type: "int", nullable: false),
                    IdTipoUsuario = table.Column<byte>(type: "tinyint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pessoa_TipoUsuario", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pessoa_TipoUsuario_Pessoa_IdPessoa",
                        column: x => x.IdPessoa,
                        principalTable: "Pessoa",
                        principalColumn: "IdPessoa",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Pessoa_TipoUsuario_TipoUsuario_IdTipoUsuario",
                        column: x => x.IdTipoUsuario,
                        principalTable: "TipoUsuario",
                        principalColumn: "IdTipoUsuario",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Nota",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdCriterio = table.Column<int>(type: "int", nullable: false),
                    IdAvaliacao = table.Column<int>(type: "int", nullable: false),
                    NotaProjeto = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Nota", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Nota_Avaliacao_IdAvaliacao",
                        column: x => x.IdAvaliacao,
                        principalTable: "Avaliacao",
                        principalColumn: "IdAvaliacao",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Nota_CriterioAvaliacao_IdCriterio",
                        column: x => x.IdCriterio,
                        principalTable: "CriterioAvaliacao",
                        principalColumn: "IdCriterio",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "TipoUsuario",
                columns: new[] { "IdTipoUsuario", "PapelUsuario" },
                values: new object[,]
                {
                    { (byte)1, "Coordenador" },
                    { (byte)2, "Avaliador" },
                    { (byte)3, "ProfessorAdmin" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Avaliacao_IdPessoa",
                table: "Avaliacao",
                column: "IdPessoa");

            migrationBuilder.CreateIndex(
                name: "IX_Avaliacao_IdProjeto",
                table: "Avaliacao",
                column: "IdProjeto");

            migrationBuilder.CreateIndex(
                name: "IX_CriterioAvaliacao_IdEvento",
                table: "CriterioAvaliacao",
                column: "IdEvento");

            migrationBuilder.CreateIndex(
                name: "IX_Evento_Projeto_IdEvento",
                table: "Evento_Projeto",
                column: "IdEvento");

            migrationBuilder.CreateIndex(
                name: "IX_Evento_Projeto_IdProjeto",
                table: "Evento_Projeto",
                column: "IdProjeto");

            migrationBuilder.CreateIndex(
                name: "IX_InformacoesComplementares_IdProjeto",
                table: "InformacoesComplementares",
                column: "IdProjeto");

            migrationBuilder.CreateIndex(
                name: "IX_Nota_IdAvaliacao",
                table: "Nota",
                column: "IdAvaliacao");

            migrationBuilder.CreateIndex(
                name: "IX_Nota_IdCriterio",
                table: "Nota",
                column: "IdCriterio");

            migrationBuilder.CreateIndex(
                name: "IX_Pessoa_Usuario",
                table: "Pessoa",
                column: "Usuario",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Pessoa_TipoUsuario_IdPessoa",
                table: "Pessoa_TipoUsuario",
                column: "IdPessoa");

            migrationBuilder.CreateIndex(
                name: "IX_Pessoa_TipoUsuario_IdTipoUsuario",
                table: "Pessoa_TipoUsuario",
                column: "IdTipoUsuario");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Evento_Projeto");

            migrationBuilder.DropTable(
                name: "InformacoesComplementares");

            migrationBuilder.DropTable(
                name: "Nota");

            migrationBuilder.DropTable(
                name: "Pessoa_TipoUsuario");

            migrationBuilder.DropTable(
                name: "Avaliacao");

            migrationBuilder.DropTable(
                name: "CriterioAvaliacao");

            migrationBuilder.DropTable(
                name: "TipoUsuario");

            migrationBuilder.DropTable(
                name: "Pessoa");

            migrationBuilder.DropTable(
                name: "Projeto");

            migrationBuilder.DropTable(
                name: "Evento");
        }
    }
}
