using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace WebApiParquimetros.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tbcatopciones",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    str_opcion = table.Column<string>(nullable: true),
                    bit_status = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbcatopciones", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tbciudades",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    created_by = table.Column<string>(maxLength: 250, nullable: true),
                    created_date = table.Column<DateTime>(nullable: false),
                    last_modified_by = table.Column<string>(maxLength: 250, nullable: true),
                    last_modified_date = table.Column<DateTime>(nullable: false),
                    bit_status = table.Column<bool>(nullable: false),
                    str_ciudad = table.Column<string>(maxLength: 200, nullable: true),
                    str_latitud = table.Column<string>(maxLength: 50, nullable: true),
                    str_longitud = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbciudades", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tbconcesiones",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    str_clave = table.Column<string>(maxLength: 100, nullable: true),
                    str_razon_social = table.Column<string>(nullable: true),
                    str_domicilio = table.Column<string>(nullable: true),
                    str_nombre_cliente = table.Column<string>(nullable: true),
                    str_telefono = table.Column<string>(maxLength: 15, nullable: true),
                    str_email = table.Column<string>(nullable: true),
                    str_rfc = table.Column<string>(nullable: true),
                    str_notas = table.Column<string>(nullable: true),
                    str_poligono = table.Column<string>(nullable: true),
                    int_licencias = table.Column<int>(nullable: false),
                    dbl_costo_licencia = table.Column<double>(nullable: false),
                    dtm_fecha_ingreso = table.Column<DateTime>(nullable: false),
                    dtm_fecha_activacion_licencia = table.Column<DateTime>(nullable: false),
                    str_tipo = table.Column<string>(nullable: true),
                    intidciudad = table.Column<int>(nullable: false),
                    bit_status = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbconcesiones", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tbdetallemulta",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    int_id_multa = table.Column<int>(nullable: false),
                    bit_status = table.Column<bool>(nullable: false),
                    dtmFecha = table.Column<DateTime>(nullable: false),
                    str_usuario = table.Column<string>(nullable: true),
                    flt_monto = table.Column<double>(nullable: false),
                    str_comentarios = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbdetallemulta", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tbopciones",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    str_opcion = table.Column<string>(maxLength: 50, nullable: true),
                    bit_status = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbopciones", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tbtarifas",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    fltTarifa = table.Column<double>(nullable: false),
                    fltIVA = table.Column<double>(nullable: false),
                    fltImpuestos = table.Column<double>(nullable: false),
                    dtmVigencia = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbtarifas", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tbtiposusuarios",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    strTipoUsuario = table.Column<string>(nullable: true),
                    bit_status = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbtiposusuarios", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    RoleId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tbcomerciantes",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    str_apellido_ap = table.Column<string>(maxLength: 50, nullable: true),
                    str_apellido_mat = table.Column<string>(maxLength: 250, nullable: true),
                    str_nombre = table.Column<string>(maxLength: 50, nullable: true),
                    str_comerciante = table.Column<string>(maxLength: 50, nullable: true),
                    str_telefono = table.Column<string>(maxLength: 50, nullable: true),
                    created_by = table.Column<string>(maxLength: 250, nullable: true),
                    created_date = table.Column<DateTime>(nullable: false),
                    last_modified_by = table.Column<string>(maxLength: 250, nullable: true),
                    last_modified_date = table.Column<DateTime>(nullable: false),
                    bit_status = table.Column<bool>(nullable: false),
                    intidconcesion_id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbcomerciantes", x => x.id);
                    table.ForeignKey(
                        name: "FK_tbcomerciantes_tbconcesiones_intidconcesion_id",
                        column: x => x.intidconcesion_id,
                        principalTable: "tbconcesiones",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tbcomisiones",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    created_by = table.Column<string>(maxLength: 250, nullable: true),
                    created_date = table.Column<DateTime>(nullable: false),
                    last_modified_by = table.Column<string>(maxLength: 250, nullable: true),
                    last_modified_date = table.Column<DateTime>(nullable: false),
                    bit_status = table.Column<bool>(nullable: false),
                    dcm_porcentaje = table.Column<double>(nullable: false),
                    dcm_valor_fijo = table.Column<double>(nullable: false),
                    intidconcesion_id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbcomisiones", x => x.id);
                    table.ForeignKey(
                        name: "FK_tbcomisiones_tbconcesiones_intidconcesion_id",
                        column: x => x.intidconcesion_id,
                        principalTable: "tbconcesiones",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tbparametros",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    bolUsarNomenclaturaCajones = table.Column<bool>(nullable: false),
                    intTimepoAviso = table.Column<int>(nullable: false),
                    flt_Tarifa_minima = table.Column<double>(nullable: false),
                    flt_intervalo_tarifa = table.Column<double>(nullable: false),
                    int_intervalo_estacionamiento = table.Column<int>(nullable: false),
                    int_minimo_estacionamiento = table.Column<int>(nullable: false),
                    int_maximo_estacionamiento = table.Column<int>(nullable: false),
                    intidconcesion_id = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbparametros", x => x.id);
                    table.ForeignKey(
                        name: "FK_tbparametros_tbconcesiones_intidconcesion_id",
                        column: x => x.intidconcesion_id,
                        principalTable: "tbconcesiones",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbpcionesconcesion",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    int_id_opcion = table.Column<int>(nullable: true),
                    int_id_concesion = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbpcionesconcesion", x => x.id);
                    table.ForeignKey(
                        name: "FK_tbpcionesconcesion_tbconcesiones_int_id_concesion",
                        column: x => x.int_id_concesion,
                        principalTable: "tbconcesiones",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbpcionesconcesion_tbcatopciones_int_id_opcion",
                        column: x => x.int_id_opcion,
                        principalTable: "tbcatopciones",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbresumendiario",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    int_id_consecion = table.Column<int>(nullable: false),
                    dtm_fecha = table.Column<DateTime>(nullable: false),
                    int_dia = table.Column<int>(nullable: false),
                    int_mes = table.Column<int>(nullable: false),
                    int_anio = table.Column<int>(nullable: false),
                    str_dia_semama = table.Column<string>(nullable: true),
                    dtm_dia_anterior = table.Column<DateTime>(nullable: false),
                    str_dia_sem_ant = table.Column<string>(nullable: true),
                    int_ios = table.Column<int>(nullable: false),
                    int_ant_ios = table.Column<int>(nullable: false),
                    int_por_ios = table.Column<int>(nullable: false),
                    dec_ios = table.Column<decimal>(nullable: false),
                    dec_ant_ios = table.Column<decimal>(nullable: false),
                    dec_por_ios = table.Column<decimal>(nullable: false),
                    int_andriod = table.Column<int>(nullable: false),
                    int_ant_andriod = table.Column<int>(nullable: false),
                    int_por_andriod = table.Column<int>(nullable: false),
                    dec_andriod = table.Column<decimal>(nullable: false),
                    dec_ant_andriod = table.Column<decimal>(nullable: false),
                    dec_por_andriod = table.Column<decimal>(nullable: false),
                    int_total = table.Column<int>(nullable: false),
                    int_total_ant = table.Column<int>(nullable: false),
                    int_por_ant_total = table.Column<int>(nullable: false),
                    dec_total = table.Column<decimal>(nullable: false),
                    dec_total_ant = table.Column<decimal>(nullable: false),
                    dec_por_ent_total = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbresumendiario", x => x.id);
                    table.ForeignKey(
                        name: "FK_tbresumendiario_tbconcesiones_int_id_consecion",
                        column: x => x.int_id_consecion,
                        principalTable: "tbconcesiones",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tbresumenmensual",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    int_id_consecion = table.Column<int>(nullable: false),
                    dtm_fecha_inicio = table.Column<DateTime>(nullable: false),
                    dtm_fecha_fin = table.Column<DateTime>(nullable: false),
                    str_mes = table.Column<string>(nullable: true),
                    int_anio = table.Column<int>(nullable: false),
                    dtm_mes_anterior = table.Column<DateTime>(nullable: false),
                    int_mes_ios = table.Column<int>(nullable: false),
                    int_mes_ant_ios = table.Column<int>(nullable: false),
                    int_mes_por_ios = table.Column<int>(nullable: false),
                    dec_mes_ios = table.Column<decimal>(nullable: false),
                    dec_mes_ant_ios = table.Column<decimal>(nullable: false),
                    dec_mes_por_ios = table.Column<decimal>(nullable: false),
                    int_mes_andriod = table.Column<int>(nullable: false),
                    int_mes_ant_andriod = table.Column<int>(nullable: false),
                    int_mes_por_andriod = table.Column<int>(nullable: false),
                    dec_mes_andriod = table.Column<decimal>(nullable: false),
                    dec_mes_ant_andriod = table.Column<decimal>(nullable: false),
                    dec_mes_por_andriod = table.Column<decimal>(nullable: false),
                    int_mes_total = table.Column<int>(nullable: false),
                    int_mes_total_ant = table.Column<int>(nullable: false),
                    int_mes_por_total = table.Column<int>(nullable: false),
                    dec_mes_total = table.Column<decimal>(nullable: false),
                    dec_mes_total_ant = table.Column<decimal>(nullable: false),
                    dec_mes_por_total = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbresumenmensual", x => x.id);
                    table.ForeignKey(
                        name: "FK_tbresumenmensual_tbconcesiones_int_id_consecion",
                        column: x => x.int_id_consecion,
                        principalTable: "tbconcesiones",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tbresumensemanal",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    int_id_consecion = table.Column<int>(nullable: false),
                    dtm_fecha_inicio = table.Column<DateTime>(nullable: false),
                    dtm_fecha_fin = table.Column<DateTime>(nullable: false),
                    int_semana = table.Column<int>(nullable: false),
                    int_anio = table.Column<int>(nullable: false),
                    int_semana_ant = table.Column<int>(nullable: false),
                    int_sem_ios = table.Column<int>(nullable: false),
                    int_sem_ant_ios = table.Column<int>(nullable: false),
                    int_sem_por_ios = table.Column<int>(nullable: false),
                    dec_sem_ios = table.Column<decimal>(nullable: false),
                    dec_sem_ant_ios = table.Column<decimal>(nullable: false),
                    dec_sem_por_ios = table.Column<decimal>(nullable: false),
                    int_sem_andriod = table.Column<int>(nullable: false),
                    int_sem_ant_andriod = table.Column<int>(nullable: false),
                    int_sem_por_andriod = table.Column<int>(nullable: false),
                    dec_sem_andriod = table.Column<decimal>(nullable: false),
                    dec_sem_ant_andriod = table.Column<decimal>(nullable: false),
                    dec_sem_por_andriod = table.Column<decimal>(nullable: false),
                    int_sem_total = table.Column<int>(nullable: false),
                    int_sem_total_ant = table.Column<int>(nullable: false),
                    int_sem_por_ant = table.Column<int>(nullable: false),
                    dec_sem_total = table.Column<decimal>(nullable: false),
                    dec_sem_total_ant = table.Column<decimal>(nullable: false),
                    dec_sem_por_total = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbresumensemanal", x => x.id);
                    table.ForeignKey(
                        name: "FK_tbresumensemanal_tbconcesiones_int_id_consecion",
                        column: x => x.int_id_consecion,
                        principalTable: "tbconcesiones",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tbusersconcesiones",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    str_email = table.Column<string>(nullable: true),
                    str_pwd = table.Column<string>(nullable: true),
                    int_id_concesion = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbusersconcesiones", x => x.id);
                    table.ForeignKey(
                        name: "FK_tbusersconcesiones_tbconcesiones_int_id_concesion",
                        column: x => x.int_id_concesion,
                        principalTable: "tbconcesiones",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tbzonas",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    created_by = table.Column<string>(maxLength: 250, nullable: true),
                    created_date = table.Column<DateTime>(nullable: false),
                    last_modified_by = table.Column<string>(maxLength: 250, nullable: true),
                    last_modified_date = table.Column<DateTime>(nullable: false),
                    bit_status = table.Column<bool>(nullable: false),
                    str_descripcion = table.Column<string>(maxLength: 200, nullable: true),
                    str_latitud = table.Column<string>(maxLength: 50, nullable: true),
                    str_longitud = table.Column<string>(maxLength: 50, nullable: true),
                    str_color = table.Column<string>(nullable: true),
                    str_poligono = table.Column<string>(nullable: true),
                    int_id_ciudad_id = table.Column<int>(nullable: false),
                    intidconcesion_id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbzonas", x => x.id);
                    table.ForeignKey(
                        name: "FK_tbzonas_tbciudades_int_id_ciudad_id",
                        column: x => x.int_id_ciudad_id,
                        principalTable: "tbciudades",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tbzonas_tbconcesiones_intidconcesion_id",
                        column: x => x.intidconcesion_id,
                        principalTable: "tbconcesiones",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tbpermisos",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    id_rol = table.Column<int>(nullable: false),
                    id_opcion = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbpermisos", x => x.id);
                    table.ForeignKey(
                        name: "FK_tbpermisos_tbopciones_id_opcion",
                        column: x => x.id_opcion,
                        principalTable: "tbopciones",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tbpermisos_tbtiposusuarios_id_rol",
                        column: x => x.id_rol,
                        principalTable: "tbtiposusuarios",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    UserName = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    created_by = table.Column<string>(nullable: true),
                    created_date = table.Column<DateTime>(nullable: false),
                    last_modified_by = table.Column<string>(nullable: true),
                    last_modified_date = table.Column<DateTime>(nullable: false),
                    bit_status = table.Column<bool>(nullable: false),
                    strNombre = table.Column<string>(nullable: true),
                    strApellidos = table.Column<string>(nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: false),
                    str_rfc = table.Column<string>(nullable: true),
                    str_razon_social = table.Column<string>(nullable: true),
                    str_direccion = table.Column<string>(nullable: true),
                    str_cp = table.Column<string>(nullable: true),
                    intidconcesion_id = table.Column<int>(nullable: true),
                    intIdTipoUsuario = table.Column<int>(nullable: false),
                    intidciudad = table.Column<int>(nullable: true),
                    intidzona = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_tbtiposusuarios_intIdTipoUsuario",
                        column: x => x.intIdTipoUsuario,
                        principalTable: "tbtiposusuarios",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_tbciudades_intidciudad",
                        column: x => x.intidciudad,
                        principalTable: "tbciudades",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_tbconcesiones_intidconcesion_id",
                        column: x => x.intidconcesion_id,
                        principalTable: "tbconcesiones",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_tbzonas_intidzona",
                        column: x => x.intidzona,
                        principalTable: "tbzonas",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbagentes",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    created_by = table.Column<string>(maxLength: 250, nullable: true),
                    created_date = table.Column<DateTime>(nullable: false),
                    last_modified_by = table.Column<string>(maxLength: 250, nullable: true),
                    last_modified_date = table.Column<DateTime>(nullable: false),
                    bit_status = table.Column<bool>(nullable: false),
                    str_nombre = table.Column<string>(maxLength: 250, nullable: true),
                    intidzona_id = table.Column<int>(nullable: false),
                    intidconcesion_id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbagentes", x => x.id);
                    table.ForeignKey(
                        name: "FK_tbagentes_tbzonas_intidzona_id",
                        column: x => x.intidzona_id,
                        principalTable: "tbzonas",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tbespacios",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    str_clave = table.Column<string>(nullable: true),
                    str_latitud = table.Column<string>(nullable: true),
                    str_longitud = table.Column<string>(nullable: true),
                    str_marcador = table.Column<string>(nullable: true),
                    str_color = table.Column<string>(nullable: true),
                    bit_status = table.Column<bool>(nullable: false),
                    bit_ocupado = table.Column<bool>(nullable: false),
                    id_zona = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbespacios", x => x.id);
                    table.ForeignKey(
                        name: "FK_tbespacios_tbzonas_id_zona",
                        column: x => x.id_zona,
                        principalTable: "tbzonas",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tblugares",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    created_by = table.Column<string>(maxLength: 225, nullable: true),
                    created_date = table.Column<DateTime>(nullable: false),
                    last_modified_by = table.Column<string>(maxLength: 225, nullable: true),
                    last_modified_date = table.Column<DateTime>(nullable: false),
                    bit_status = table.Column<bool>(nullable: false),
                    str_latitud = table.Column<string>(maxLength: 50, nullable: true),
                    str_longitud = table.Column<string>(maxLength: 50, nullable: true),
                    str_lugar = table.Column<string>(maxLength: 50, nullable: true),
                    int_id_zona_id = table.Column<int>(nullable: false),
                    intidconcesion_id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblugares", x => x.id);
                    table.ForeignKey(
                        name: "FK_tblugares_tbzonas_int_id_zona_id",
                        column: x => x.int_id_zona_id,
                        principalTable: "tbzonas",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tblugares_tbconcesiones_intidconcesion_id",
                        column: x => x.intidconcesion_id,
                        principalTable: "tbconcesiones",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tbsecciones",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    str_seccion = table.Column<string>(nullable: true),
                    str_color = table.Column<string>(nullable: true),
                    str_poligono = table.Column<string>(nullable: true),
                    bit_status = table.Column<bool>(nullable: false),
                    intidzona_id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbsecciones", x => x.id);
                    table.ForeignKey(
                        name: "FK_tbsecciones_tbzonas_intidzona_id",
                        column: x => x.intidzona_id,
                        principalTable: "tbzonas",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    UserId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(nullable: false),
                    ProviderKey = table.Column<string>(nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    RoleId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    LoginProvider = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tbsaldo",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    created_by = table.Column<string>(maxLength: 225, nullable: true),
                    created_date = table.Column<DateTime>(nullable: false),
                    last_modified_by = table.Column<string>(maxLength: 225, nullable: true),
                    last_modified_date = table.Column<DateTime>(nullable: false),
                    bit_status = table.Column<bool>(nullable: false),
                    dtmfecha = table.Column<DateTime>(nullable: false),
                    flt_monto_final = table.Column<double>(nullable: false),
                    flt_monto_inicial = table.Column<double>(nullable: false),
                    str_forma_pago = table.Column<string>(maxLength: 50, nullable: false),
                    str_tipo_recarga = table.Column<string>(maxLength: 20, nullable: true),
                    int_id_usuario_id = table.Column<string>(nullable: true),
                    int_id_usuario_trans = table.Column<string>(nullable: true),
                    intidconcesion_id = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbsaldo", x => x.id);
                    table.ForeignKey(
                        name: "FK_tbsaldo_AspNetUsers_int_id_usuario_id",
                        column: x => x.int_id_usuario_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbsaldo_tbconcesiones_intidconcesion_id",
                        column: x => x.intidconcesion_id,
                        principalTable: "tbconcesiones",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbtarjetas",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    created_by = table.Column<string>(maxLength: 225, nullable: true),
                    created_date = table.Column<DateTime>(nullable: false),
                    last_modified_by = table.Column<string>(maxLength: 225, nullable: true),
                    last_modified_date = table.Column<DateTime>(nullable: false),
                    bit_status = table.Column<bool>(nullable: false),
                    dc_mano_vigencia = table.Column<long>(nullable: false),
                    dcm_mes_vigencia = table.Column<long>(nullable: false),
                    str_referencia_tarjeta = table.Column<string>(maxLength: 50, nullable: true),
                    str_sistema_tarjeta = table.Column<string>(maxLength: 50, nullable: true),
                    str_tarjeta = table.Column<string>(maxLength: 50, nullable: true),
                    str_titular = table.Column<string>(maxLength: 200, nullable: true),
                    int_id_usuario_id = table.Column<string>(nullable: true),
                    intidconcesion_id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbtarjetas", x => x.id);
                    table.ForeignKey(
                        name: "FK_tbtarjetas_AspNetUsers_int_id_usuario_id",
                        column: x => x.int_id_usuario_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbtarjetas_tbconcesiones_intidconcesion_id",
                        column: x => x.intidconcesion_id,
                        principalTable: "tbconcesiones",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tbvehiculos",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    created_by = table.Column<string>(maxLength: 225, nullable: true),
                    created_date = table.Column<DateTime>(nullable: false),
                    last_modified_by = table.Column<string>(maxLength: 255, nullable: true),
                    last_modified_date = table.Column<DateTime>(nullable: false),
                    bit_status = table.Column<bool>(nullable: false),
                    str_color = table.Column<string>(maxLength: 50, nullable: true),
                    str_modelo = table.Column<string>(maxLength: 200, nullable: true),
                    str_placas = table.Column<string>(maxLength: 20, nullable: false),
                    int_id_usuario_id = table.Column<string>(nullable: true),
                    intidconcesion_id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbvehiculos", x => x.id);
                    table.ForeignKey(
                        name: "FK_tbvehiculos_AspNetUsers_int_id_usuario_id",
                        column: x => x.int_id_usuario_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbvehiculos_tbconcesiones_intidconcesion_id",
                        column: x => x.intidconcesion_id,
                        principalTable: "tbconcesiones",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tbdetallesaldo",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    dtm_fecha = table.Column<DateTime>(nullable: false),
                    flt_monto = table.Column<double>(nullable: false),
                    str_tipo = table.Column<string>(nullable: true),
                    str_forma_pago = table.Column<string>(nullable: true),
                    int_id_saldo = table.Column<int>(nullable: false),
                    int_id_usuario = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbdetallesaldo", x => x.id);
                    table.ForeignKey(
                        name: "FK_tbdetallesaldo_tbsaldo_int_id_saldo",
                        column: x => x.int_id_saldo,
                        principalTable: "tbsaldo",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tbdetallesaldo_AspNetUsers_int_id_usuario",
                        column: x => x.int_id_usuario,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbmovimientos",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    created_by = table.Column<string>(maxLength: 225, nullable: true),
                    created_date = table.Column<DateTime>(nullable: true),
                    last_modified_by = table.Column<string>(maxLength: 225, nullable: true),
                    last_modified_date = table.Column<DateTime>(nullable: false),
                    bit_status = table.Column<bool>(nullable: false),
                    str_placa = table.Column<string>(nullable: true),
                    str_latitud = table.Column<string>(nullable: true),
                    str_longitud = table.Column<string>(nullable: true),
                    boolean_auto_recarga = table.Column<bool>(nullable: false),
                    boolean_multa = table.Column<bool>(nullable: false),
                    dt_hora_inicio = table.Column<DateTime>(nullable: false),
                    dtm_fecha_insercion_descuento = table.Column<DateTime>(nullable: true),
                    dtm_fecha_descuento = table.Column<DateTime>(nullable: true),
                    dtm_hora_fin = table.Column<DateTime>(nullable: false),
                    int_tiempo = table.Column<int>(nullable: false),
                    flt_moneda_saldo_previo_descuento = table.Column<double>(nullable: false),
                    flt_monto = table.Column<double>(nullable: false),
                    flt_saldo_previo_descuento = table.Column<double>(nullable: true),
                    flt_valor_descuento = table.Column<double>(nullable: true),
                    flt_valor_devuelto = table.Column<double>(nullable: true),
                    flt_valor_final_descuento = table.Column<double>(nullable: true),
                    str_cambio_descuento = table.Column<string>(maxLength: 50, nullable: true),
                    str_codigo_autorizacion = table.Column<string>(maxLength: 50, nullable: true),
                    str_codigo_transaccion = table.Column<string>(maxLength: 50, nullable: true),
                    str_comentarios = table.Column<string>(maxLength: 200, nullable: true),
                    str_hash_tarjeta = table.Column<string>(maxLength: 50, nullable: true),
                    str_instalacion = table.Column<string>(maxLength: 50, nullable: true),
                    str_instalacion_abrv = table.Column<string>(maxLength: 50, nullable: true),
                    str_moneda_valor_descuento = table.Column<string>(maxLength: 50, nullable: true),
                    str_referencia_operacion = table.Column<string>(maxLength: 50, nullable: true),
                    str_so = table.Column<string>(maxLength: 50, nullable: true),
                    str_tipo = table.Column<string>(maxLength: 50, nullable: true),
                    str_versionapp = table.Column<string>(maxLength: 50, nullable: true),
                    int_id_espacio = table.Column<int>(nullable: true),
                    int_id_saldo_id = table.Column<int>(nullable: false),
                    int_id_usuario_id = table.Column<string>(nullable: true),
                    int_id_vehiculo_id = table.Column<int>(nullable: false),
                    intidconcesion_id = table.Column<int>(nullable: false),
                    int_id_multa = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbmovimientos", x => x.id);
                    table.ForeignKey(
                        name: "FK_tbmovimientos_tbespacios_int_id_espacio",
                        column: x => x.int_id_espacio,
                        principalTable: "tbespacios",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbmovimientos_tbsaldo_int_id_saldo_id",
                        column: x => x.int_id_saldo_id,
                        principalTable: "tbsaldo",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tbmovimientos_AspNetUsers_int_id_usuario_id",
                        column: x => x.int_id_usuario_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbmovimientos_tbvehiculos_int_id_vehiculo_id",
                        column: x => x.int_id_vehiculo_id,
                        principalTable: "tbvehiculos",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tbmovimientos_tbconcesiones_intidconcesion_id",
                        column: x => x.intidconcesion_id,
                        principalTable: "tbconcesiones",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tbdetallemovimientos",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    int_idmovimiento = table.Column<int>(nullable: false),
                    int_idespacio = table.Column<int>(nullable: true),
                    int_id_usuario_id = table.Column<string>(nullable: true),
                    int_id_zona = table.Column<int>(nullable: true),
                    int_duracion = table.Column<int>(nullable: false),
                    dtm_horaInicio = table.Column<DateTime>(nullable: false),
                    dtm_horaFin = table.Column<DateTime>(nullable: false),
                    flt_importe = table.Column<double>(nullable: false),
                    flt_descuentos = table.Column<double>(nullable: false),
                    flt_saldo_anterior = table.Column<double>(nullable: false),
                    flt_saldo_fin = table.Column<double>(nullable: false),
                    str_observaciones = table.Column<string>(nullable: true),
                    str_latitud = table.Column<string>(nullable: true),
                    str_longitud = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbdetallemovimientos", x => x.id);
                    table.ForeignKey(
                        name: "FK_tbdetallemovimientos_AspNetUsers_int_id_usuario_id",
                        column: x => x.int_id_usuario_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbdetallemovimientos_tbzonas_int_id_zona",
                        column: x => x.int_id_zona,
                        principalTable: "tbzonas",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbdetallemovimientos_tbespacios_int_idespacio",
                        column: x => x.int_idespacio,
                        principalTable: "tbespacios",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbdetallemovimientos_tbmovimientos_int_idmovimiento",
                        column: x => x.int_idmovimiento,
                        principalTable: "tbmovimientos",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tbmultas",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    created_by = table.Column<string>(maxLength: 225, nullable: true),
                    created_date = table.Column<DateTime>(nullable: false),
                    last_modified_by = table.Column<string>(maxLength: 225, nullable: true),
                    last_modified_date = table.Column<DateTime>(nullable: false),
                    bit_status = table.Column<bool>(nullable: false),
                    dtm_fecha = table.Column<DateTime>(nullable: false),
                    flt_monto = table.Column<double>(nullable: false),
                    str_motivo = table.Column<string>(maxLength: 200, nullable: true),
                    str_folio_multa = table.Column<string>(nullable: true),
                    str_placa = table.Column<string>(nullable: true),
                    str_Estado = table.Column<string>(nullable: true),
                    str_marca = table.Column<string>(nullable: true),
                    str_modelo = table.Column<string>(nullable: true),
                    str_color = table.Column<string>(nullable: true),
                    str_ubicacion = table.Column<string>(nullable: true),
                    str_fundamento = table.Column<string>(nullable: true),
                    str_articulo = table.Column<string>(nullable: true),
                    str_categoria = table.Column<string>(nullable: true),
                    str_clave = table.Column<string>(nullable: true),
                    str_tipo_pago = table.Column<string>(nullable: true),
                    str_documento_garantia = table.Column<string>(nullable: true),
                    str_tipo_multa = table.Column<string>(nullable: true),
                    str_clave_candado = table.Column<string>(nullable: true),
                    dtm_fecha_multafisica = table.Column<DateTime>(nullable: false),
                    str_no_parquimetro = table.Column<string>(nullable: true),
                    str_id_agente_id = table.Column<string>(nullable: true),
                    int_id_movimiento_id = table.Column<int>(nullable: true),
                    int_id_saldo_id = table.Column<int>(nullable: true),
                    int_id_vehiculo_id = table.Column<int>(nullable: true),
                    intidconcesion_id = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbmultas", x => x.id);
                    table.ForeignKey(
                        name: "FK_tbmultas_tbmovimientos_int_id_movimiento_id",
                        column: x => x.int_id_movimiento_id,
                        principalTable: "tbmovimientos",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbmultas_tbsaldo_int_id_saldo_id",
                        column: x => x.int_id_saldo_id,
                        principalTable: "tbsaldo",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbmultas_tbvehiculos_int_id_vehiculo_id",
                        column: x => x.int_id_vehiculo_id,
                        principalTable: "tbvehiculos",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbmultas_tbconcesiones_intidconcesion_id",
                        column: x => x.intidconcesion_id,
                        principalTable: "tbconcesiones",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbmultas_AspNetUsers_str_id_agente_id",
                        column: x => x.str_id_agente_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_intIdTipoUsuario",
                table: "AspNetUsers",
                column: "intIdTipoUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_intidciudad",
                table: "AspNetUsers",
                column: "intidciudad");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_intidconcesion_id",
                table: "AspNetUsers",
                column: "intidconcesion_id");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_intidzona",
                table: "AspNetUsers",
                column: "intidzona");

            migrationBuilder.CreateIndex(
                name: "IX_tbagentes_intidzona_id",
                table: "tbagentes",
                column: "intidzona_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbcomerciantes_intidconcesion_id",
                table: "tbcomerciantes",
                column: "intidconcesion_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbcomisiones_intidconcesion_id",
                table: "tbcomisiones",
                column: "intidconcesion_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbdetallemovimientos_int_id_usuario_id",
                table: "tbdetallemovimientos",
                column: "int_id_usuario_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbdetallemovimientos_int_id_zona",
                table: "tbdetallemovimientos",
                column: "int_id_zona");

            migrationBuilder.CreateIndex(
                name: "IX_tbdetallemovimientos_int_idespacio",
                table: "tbdetallemovimientos",
                column: "int_idespacio");

            migrationBuilder.CreateIndex(
                name: "IX_tbdetallemovimientos_int_idmovimiento",
                table: "tbdetallemovimientos",
                column: "int_idmovimiento");

            migrationBuilder.CreateIndex(
                name: "IX_tbdetallesaldo_int_id_saldo",
                table: "tbdetallesaldo",
                column: "int_id_saldo");

            migrationBuilder.CreateIndex(
                name: "IX_tbdetallesaldo_int_id_usuario",
                table: "tbdetallesaldo",
                column: "int_id_usuario");

            migrationBuilder.CreateIndex(
                name: "IX_tbespacios_id_zona",
                table: "tbespacios",
                column: "id_zona");

            migrationBuilder.CreateIndex(
                name: "IX_tblugares_int_id_zona_id",
                table: "tblugares",
                column: "int_id_zona_id");

            migrationBuilder.CreateIndex(
                name: "IX_tblugares_intidconcesion_id",
                table: "tblugares",
                column: "intidconcesion_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbmovimientos_int_id_espacio",
                table: "tbmovimientos",
                column: "int_id_espacio");

            migrationBuilder.CreateIndex(
                name: "IX_tbmovimientos_int_id_saldo_id",
                table: "tbmovimientos",
                column: "int_id_saldo_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbmovimientos_int_id_usuario_id",
                table: "tbmovimientos",
                column: "int_id_usuario_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbmovimientos_int_id_vehiculo_id",
                table: "tbmovimientos",
                column: "int_id_vehiculo_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbmovimientos_intidconcesion_id",
                table: "tbmovimientos",
                column: "intidconcesion_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbmultas_int_id_movimiento_id",
                table: "tbmultas",
                column: "int_id_movimiento_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbmultas_int_id_saldo_id",
                table: "tbmultas",
                column: "int_id_saldo_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbmultas_int_id_vehiculo_id",
                table: "tbmultas",
                column: "int_id_vehiculo_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbmultas_intidconcesion_id",
                table: "tbmultas",
                column: "intidconcesion_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbmultas_str_id_agente_id",
                table: "tbmultas",
                column: "str_id_agente_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbparametros_intidconcesion_id",
                table: "tbparametros",
                column: "intidconcesion_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbpcionesconcesion_int_id_concesion",
                table: "tbpcionesconcesion",
                column: "int_id_concesion");

            migrationBuilder.CreateIndex(
                name: "IX_tbpcionesconcesion_int_id_opcion",
                table: "tbpcionesconcesion",
                column: "int_id_opcion");

            migrationBuilder.CreateIndex(
                name: "IX_tbpermisos_id_opcion",
                table: "tbpermisos",
                column: "id_opcion");

            migrationBuilder.CreateIndex(
                name: "IX_tbpermisos_id_rol",
                table: "tbpermisos",
                column: "id_rol");

            migrationBuilder.CreateIndex(
                name: "IX_tbresumendiario_int_id_consecion",
                table: "tbresumendiario",
                column: "int_id_consecion");

            migrationBuilder.CreateIndex(
                name: "IX_tbresumenmensual_int_id_consecion",
                table: "tbresumenmensual",
                column: "int_id_consecion");

            migrationBuilder.CreateIndex(
                name: "IX_tbresumensemanal_int_id_consecion",
                table: "tbresumensemanal",
                column: "int_id_consecion");

            migrationBuilder.CreateIndex(
                name: "IX_tbsaldo_int_id_usuario_id",
                table: "tbsaldo",
                column: "int_id_usuario_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbsaldo_intidconcesion_id",
                table: "tbsaldo",
                column: "intidconcesion_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbsecciones_intidzona_id",
                table: "tbsecciones",
                column: "intidzona_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbtarjetas_int_id_usuario_id",
                table: "tbtarjetas",
                column: "int_id_usuario_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbtarjetas_intidconcesion_id",
                table: "tbtarjetas",
                column: "intidconcesion_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbusersconcesiones_int_id_concesion",
                table: "tbusersconcesiones",
                column: "int_id_concesion");

            migrationBuilder.CreateIndex(
                name: "IX_tbvehiculos_int_id_usuario_id",
                table: "tbvehiculos",
                column: "int_id_usuario_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbvehiculos_intidconcesion_id",
                table: "tbvehiculos",
                column: "intidconcesion_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbzonas_int_id_ciudad_id",
                table: "tbzonas",
                column: "int_id_ciudad_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbzonas_intidconcesion_id",
                table: "tbzonas",
                column: "intidconcesion_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "tbagentes");

            migrationBuilder.DropTable(
                name: "tbcomerciantes");

            migrationBuilder.DropTable(
                name: "tbcomisiones");

            migrationBuilder.DropTable(
                name: "tbdetallemovimientos");

            migrationBuilder.DropTable(
                name: "tbdetallemulta");

            migrationBuilder.DropTable(
                name: "tbdetallesaldo");

            migrationBuilder.DropTable(
                name: "tblugares");

            migrationBuilder.DropTable(
                name: "tbmultas");

            migrationBuilder.DropTable(
                name: "tbparametros");

            migrationBuilder.DropTable(
                name: "tbpcionesconcesion");

            migrationBuilder.DropTable(
                name: "tbpermisos");

            migrationBuilder.DropTable(
                name: "tbresumendiario");

            migrationBuilder.DropTable(
                name: "tbresumenmensual");

            migrationBuilder.DropTable(
                name: "tbresumensemanal");

            migrationBuilder.DropTable(
                name: "tbsecciones");

            migrationBuilder.DropTable(
                name: "tbtarifas");

            migrationBuilder.DropTable(
                name: "tbtarjetas");

            migrationBuilder.DropTable(
                name: "tbusersconcesiones");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "tbmovimientos");

            migrationBuilder.DropTable(
                name: "tbcatopciones");

            migrationBuilder.DropTable(
                name: "tbopciones");

            migrationBuilder.DropTable(
                name: "tbespacios");

            migrationBuilder.DropTable(
                name: "tbsaldo");

            migrationBuilder.DropTable(
                name: "tbvehiculos");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "tbtiposusuarios");

            migrationBuilder.DropTable(
                name: "tbzonas");

            migrationBuilder.DropTable(
                name: "tbciudades");

            migrationBuilder.DropTable(
                name: "tbconcesiones");
        }
    }
}
