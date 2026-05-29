using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Convocatorias.Infraestructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Candidatos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Apellido = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Candidatos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Convocatorias",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SedeId = table.Column<int>(type: "integer", nullable: false),
                    FacultadId = table.Column<int>(type: "integer", nullable: false),
                    CarreraId = table.Column<int>(type: "integer", nullable: false),
                    Asignatura = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Modalidad = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Convocatorias", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Periodos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Orden = table.Column<int>(type: "integer", nullable: false),
                    Cuatrimestre = table.Column<int>(type: "integer", nullable: false),
                    Anio = table.Column<int>(type: "integer", nullable: false),
                    FechaInicio = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FechaFin = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Periodos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Educaciones",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TituloGrado = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    AnioGraduacion = table.Column<int>(type: "integer", nullable: false),
                    PosgradoStatus = table.Column<int>(type: "integer", nullable: false),
                    PosgradoNombre = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    TipoFormacion = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    CandidatoId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Educaciones", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Educaciones_Candidatos_CandidatoId",
                        column: x => x.CandidatoId,
                        principalTable: "Candidatos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExperienciasDocentes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AniosExperiencia = table.Column<int>(type: "integer", nullable: false),
                    Nivel = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Institucion = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Cargo = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    DesdePeriodo = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    HastaPeriodo = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CandidatoId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExperienciasDocentes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExperienciasDocentes_Candidatos_CandidatoId",
                        column: x => x.CandidatoId,
                        principalTable: "Candidatos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExperienciasInvExt",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Tipo = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    TieneExperiencia = table.Column<bool>(type: "boolean", nullable: false),
                    ParticipoComo = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Descripcion = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    CandidatoId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExperienciasInvExt", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExperienciasInvExt_Candidatos_CandidatoId",
                        column: x => x.CandidatoId,
                        principalTable: "Candidatos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ConvocatoriaPeriodo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ConvocatoriaId = table.Column<Guid>(type: "uuid", nullable: false),
                    PeriodoId = table.Column<Guid>(type: "uuid", nullable: false),
                    EsActual = table.Column<bool>(type: "boolean", nullable: false),
                    AsignadoEn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConvocatoriaPeriodo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConvocatoriaPeriodo_Convocatorias_ConvocatoriaId",
                        column: x => x.ConvocatoriaId,
                        principalTable: "Convocatorias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Postulaciones",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ConvocatoriaId = table.Column<Guid>(type: "uuid", nullable: false),
                    CandidatoId = table.Column<Guid>(type: "uuid", nullable: false),
                    FechaPostulacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Estado = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Postulaciones", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Postulaciones_Candidatos_CandidatoId",
                        column: x => x.CandidatoId,
                        principalTable: "Candidatos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Postulaciones_Convocatorias_ConvocatoriaId",
                        column: x => x.ConvocatoriaId,
                        principalTable: "Convocatorias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Documentos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TipoDocumento = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Url = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    EducacionId = table.Column<Guid>(type: "uuid", nullable: true),
                    ExperienciaDocenteId = table.Column<Guid>(type: "uuid", nullable: true),
                    ExperienciaInvExtId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Documentos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Documentos_Educaciones_EducacionId",
                        column: x => x.EducacionId,
                        principalTable: "Educaciones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Documentos_ExperienciasDocentes_ExperienciaDocenteId",
                        column: x => x.ExperienciaDocenteId,
                        principalTable: "ExperienciasDocentes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Documentos_ExperienciasInvExt_ExperienciaInvExtId",
                        column: x => x.ExperienciaInvExtId,
                        principalTable: "ExperienciasInvExt",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ConvocatoriaPeriodo_ConvocatoriaId",
                table: "ConvocatoriaPeriodo",
                column: "ConvocatoriaId");

            migrationBuilder.CreateIndex(
                name: "IX_Documentos_EducacionId",
                table: "Documentos",
                column: "EducacionId");

            migrationBuilder.CreateIndex(
                name: "IX_Documentos_ExperienciaDocenteId",
                table: "Documentos",
                column: "ExperienciaDocenteId");

            migrationBuilder.CreateIndex(
                name: "IX_Documentos_ExperienciaInvExtId",
                table: "Documentos",
                column: "ExperienciaInvExtId");

            migrationBuilder.CreateIndex(
                name: "IX_Educaciones_CandidatoId",
                table: "Educaciones",
                column: "CandidatoId");

            migrationBuilder.CreateIndex(
                name: "IX_ExperienciasDocentes_CandidatoId",
                table: "ExperienciasDocentes",
                column: "CandidatoId");

            migrationBuilder.CreateIndex(
                name: "IX_ExperienciasInvExt_CandidatoId",
                table: "ExperienciasInvExt",
                column: "CandidatoId");

            migrationBuilder.CreateIndex(
                name: "IX_Postulaciones_CandidatoId",
                table: "Postulaciones",
                column: "CandidatoId");

            migrationBuilder.CreateIndex(
                name: "IX_Postulaciones_ConvocatoriaId_CandidatoId",
                table: "Postulaciones",
                columns: new[] { "ConvocatoriaId", "CandidatoId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConvocatoriaPeriodo");

            migrationBuilder.DropTable(
                name: "Documentos");

            migrationBuilder.DropTable(
                name: "Periodos");

            migrationBuilder.DropTable(
                name: "Postulaciones");

            migrationBuilder.DropTable(
                name: "Educaciones");

            migrationBuilder.DropTable(
                name: "ExperienciasDocentes");

            migrationBuilder.DropTable(
                name: "ExperienciasInvExt");

            migrationBuilder.DropTable(
                name: "Convocatorias");

            migrationBuilder.DropTable(
                name: "Candidatos");
        }
    }
}
