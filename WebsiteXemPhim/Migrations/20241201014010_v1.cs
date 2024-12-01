using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebsiteXemPhim.Migrations
{
    /// <inheritdoc />
    public partial class v1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    avatar = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    sex = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Nam",
                columns: table => new
                {
                    NamID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenNam = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Nam", x => x.NamID);
                });

            migrationBuilder.CreateTable(
                name: "QuocGia",
                columns: table => new
                {
                    QuocGiaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenQuocGia = table.Column<string>(type: "nvarchar(57)", maxLength: 57, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuocGia", x => x.QuocGiaId);
                });

            migrationBuilder.CreateTable(
                name: "TheLoai",
                columns: table => new
                {
                    TheLoaiId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenTheLoai = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TheLoai", x => x.TheLoaiId);
                });

            migrationBuilder.CreateTable(
                name: "TrangThai",
                columns: table => new
                {
                    TrangThaiId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TrangThaiPhim = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrangThai", x => x.TrangThaiId);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
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
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
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
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
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
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
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
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
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
                name: "PhimBo",
                columns: table => new
                {
                    PhimBoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenPhim = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NoiDung = table.Column<string>(type: "nvarchar(1500)", maxLength: 1500, nullable: false),
                    Anh = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    AnhNen = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    LuotXem = table.Column<int>(type: "int", nullable: false),
                    Like = table.Column<int>(type: "int", nullable: false),
                    NamID = table.Column<int>(type: "int", nullable: true),
                    QuocGiaId = table.Column<int>(type: "int", nullable: true),
                    TrangThaiId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhimBo", x => x.PhimBoId);
                    table.ForeignKey(
                        name: "FK_PhimBo_Nam_NamID",
                        column: x => x.NamID,
                        principalTable: "Nam",
                        principalColumn: "NamID");
                    table.ForeignKey(
                        name: "FK_PhimBo_QuocGia_QuocGiaId",
                        column: x => x.QuocGiaId,
                        principalTable: "QuocGia",
                        principalColumn: "QuocGiaId");
                    table.ForeignKey(
                        name: "FK_PhimBo_TrangThai_TrangThaiId",
                        column: x => x.TrangThaiId,
                        principalTable: "TrangThai",
                        principalColumn: "TrangThaiId");
                });

            migrationBuilder.CreateTable(
                name: "PhimLe",
                columns: table => new
                {
                    PhimLeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenPhim = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NoiDung = table.Column<string>(type: "nvarchar(1500)", maxLength: 1500, nullable: false),
                    Anh = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    AnhNen = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ThoiLuong = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    LuotXem = table.Column<int>(type: "int", nullable: false),
                    Like = table.Column<int>(type: "int", nullable: false),
                    Link = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Linkdb = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    NamID = table.Column<int>(type: "int", nullable: true),
                    QuocGiaId = table.Column<int>(type: "int", nullable: true),
                    TrangThaiId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhimLe", x => x.PhimLeId);
                    table.ForeignKey(
                        name: "FK_PhimLe_Nam_NamID",
                        column: x => x.NamID,
                        principalTable: "Nam",
                        principalColumn: "NamID");
                    table.ForeignKey(
                        name: "FK_PhimLe_QuocGia_QuocGiaId",
                        column: x => x.QuocGiaId,
                        principalTable: "QuocGia",
                        principalColumn: "QuocGiaId");
                    table.ForeignKey(
                        name: "FK_PhimLe_TrangThai_TrangThaiId",
                        column: x => x.TrangThaiId,
                        principalTable: "TrangThai",
                        principalColumn: "TrangThaiId");
                });

            migrationBuilder.CreateTable(
                name: "ChiTietTheLoaiPhimBo",
                columns: table => new
                {
                    ChiTietTheLoaiPhimBoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PhimBoId = table.Column<int>(type: "int", nullable: true),
                    TheLoaiId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChiTietTheLoaiPhimBo", x => x.ChiTietTheLoaiPhimBoId);
                    table.ForeignKey(
                        name: "FK_ChiTietTheLoaiPhimBo_PhimBo_PhimBoId",
                        column: x => x.PhimBoId,
                        principalTable: "PhimBo",
                        principalColumn: "PhimBoId");
                    table.ForeignKey(
                        name: "FK_ChiTietTheLoaiPhimBo_TheLoai_TheLoaiId",
                        column: x => x.TheLoaiId,
                        principalTable: "TheLoai",
                        principalColumn: "TheLoaiId");
                });

            migrationBuilder.CreateTable(
                name: "TapPhim",
                columns: table => new
                {
                    TapPhimId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Tap = table.Column<int>(type: "int", nullable: false),
                    Link = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Linkdb = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ThoiLuong = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    PhimBoId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TapPhim", x => x.TapPhimId);
                    table.ForeignKey(
                        name: "FK_TapPhim_PhimBo_PhimBoId",
                        column: x => x.PhimBoId,
                        principalTable: "PhimBo",
                        principalColumn: "PhimBoId");
                });

            migrationBuilder.CreateTable(
                name: "BinhLuan",
                columns: table => new
                {
                    BinhLuanId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PhimBoId = table.Column<int>(type: "int", nullable: true),
                    PhimLeId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    NoiDungBinhLuan = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    NgayTao = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BinhLuan", x => x.BinhLuanId);
                    table.ForeignKey(
                        name: "FK_BinhLuan_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BinhLuan_PhimBo_PhimBoId",
                        column: x => x.PhimBoId,
                        principalTable: "PhimBo",
                        principalColumn: "PhimBoId");
                    table.ForeignKey(
                        name: "FK_BinhLuan_PhimLe_PhimLeId",
                        column: x => x.PhimLeId,
                        principalTable: "PhimLe",
                        principalColumn: "PhimLeId");
                });

            migrationBuilder.CreateTable(
                name: "ChiTietTheLoaiPhimLe",
                columns: table => new
                {
                    ChiTietTheLoaiPhimLeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PhimLeId = table.Column<int>(type: "int", nullable: true),
                    TheLoaiId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChiTietTheLoaiPhimLe", x => x.ChiTietTheLoaiPhimLeId);
                    table.ForeignKey(
                        name: "FK_ChiTietTheLoaiPhimLe_PhimLe_PhimLeId",
                        column: x => x.PhimLeId,
                        principalTable: "PhimLe",
                        principalColumn: "PhimLeId");
                    table.ForeignKey(
                        name: "FK_ChiTietTheLoaiPhimLe_TheLoai_TheLoaiId",
                        column: x => x.TheLoaiId,
                        principalTable: "TheLoai",
                        principalColumn: "TheLoaiId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DanhGia",
                columns: table => new
                {
                    DanhGiaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PhimBoId = table.Column<int>(type: "int", nullable: true),
                    PhimLeId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Star = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DanhGia", x => x.DanhGiaId);
                    table.ForeignKey(
                        name: "FK_DanhGia_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DanhGia_PhimBo_PhimBoId",
                        column: x => x.PhimBoId,
                        principalTable: "PhimBo",
                        principalColumn: "PhimBoId");
                    table.ForeignKey(
                        name: "FK_DanhGia_PhimLe_PhimLeId",
                        column: x => x.PhimLeId,
                        principalTable: "PhimLe",
                        principalColumn: "PhimLeId");
                });

            migrationBuilder.CreateTable(
                name: "HopPhim",
                columns: table => new
                {
                    HopPhimId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PhimBoId = table.Column<int>(type: "int", nullable: true),
                    PhimLeId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HopPhim", x => x.HopPhimId);
                    table.ForeignKey(
                        name: "FK_HopPhim_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HopPhim_PhimBo_PhimBoId",
                        column: x => x.PhimBoId,
                        principalTable: "PhimBo",
                        principalColumn: "PhimBoId");
                    table.ForeignKey(
                        name: "FK_HopPhim_PhimLe_PhimLeId",
                        column: x => x.PhimLeId,
                        principalTable: "PhimLe",
                        principalColumn: "PhimLeId");
                });

            migrationBuilder.CreateTable(
                name: "LichSuXem",
                columns: table => new
                {
                    LichSuXemId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PhimBoId = table.Column<int>(type: "int", nullable: true),
                    PhimLeId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LichSuXem", x => x.LichSuXemId);
                    table.ForeignKey(
                        name: "FK_LichSuXem_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LichSuXem_PhimBo_PhimBoId",
                        column: x => x.PhimBoId,
                        principalTable: "PhimBo",
                        principalColumn: "PhimBoId");
                    table.ForeignKey(
                        name: "FK_LichSuXem_PhimLe_PhimLeId",
                        column: x => x.PhimLeId,
                        principalTable: "PhimLe",
                        principalColumn: "PhimLeId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

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
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_BinhLuan_PhimBoId",
                table: "BinhLuan",
                column: "PhimBoId");

            migrationBuilder.CreateIndex(
                name: "IX_BinhLuan_PhimLeId",
                table: "BinhLuan",
                column: "PhimLeId");

            migrationBuilder.CreateIndex(
                name: "IX_BinhLuan_UserId",
                table: "BinhLuan",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietTheLoaiPhimBo_PhimBoId",
                table: "ChiTietTheLoaiPhimBo",
                column: "PhimBoId");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietTheLoaiPhimBo_TheLoaiId",
                table: "ChiTietTheLoaiPhimBo",
                column: "TheLoaiId");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietTheLoaiPhimLe_PhimLeId",
                table: "ChiTietTheLoaiPhimLe",
                column: "PhimLeId");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietTheLoaiPhimLe_TheLoaiId",
                table: "ChiTietTheLoaiPhimLe",
                column: "TheLoaiId");

            migrationBuilder.CreateIndex(
                name: "IX_DanhGia_PhimBoId",
                table: "DanhGia",
                column: "PhimBoId");

            migrationBuilder.CreateIndex(
                name: "IX_DanhGia_PhimLeId",
                table: "DanhGia",
                column: "PhimLeId");

            migrationBuilder.CreateIndex(
                name: "IX_DanhGia_UserId",
                table: "DanhGia",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_HopPhim_PhimBoId",
                table: "HopPhim",
                column: "PhimBoId");

            migrationBuilder.CreateIndex(
                name: "IX_HopPhim_PhimLeId",
                table: "HopPhim",
                column: "PhimLeId");

            migrationBuilder.CreateIndex(
                name: "IX_HopPhim_UserId",
                table: "HopPhim",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_LichSuXem_PhimBoId",
                table: "LichSuXem",
                column: "PhimBoId");

            migrationBuilder.CreateIndex(
                name: "IX_LichSuXem_PhimLeId",
                table: "LichSuXem",
                column: "PhimLeId");

            migrationBuilder.CreateIndex(
                name: "IX_LichSuXem_UserId",
                table: "LichSuXem",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_PhimBo_NamID",
                table: "PhimBo",
                column: "NamID");

            migrationBuilder.CreateIndex(
                name: "IX_PhimBo_QuocGiaId",
                table: "PhimBo",
                column: "QuocGiaId");

            migrationBuilder.CreateIndex(
                name: "IX_PhimBo_TrangThaiId",
                table: "PhimBo",
                column: "TrangThaiId");

            migrationBuilder.CreateIndex(
                name: "IX_PhimLe_NamID",
                table: "PhimLe",
                column: "NamID");

            migrationBuilder.CreateIndex(
                name: "IX_PhimLe_QuocGiaId",
                table: "PhimLe",
                column: "QuocGiaId");

            migrationBuilder.CreateIndex(
                name: "IX_PhimLe_TrangThaiId",
                table: "PhimLe",
                column: "TrangThaiId");

            migrationBuilder.CreateIndex(
                name: "IX_TapPhim_PhimBoId",
                table: "TapPhim",
                column: "PhimBoId");
        }

        /// <inheritdoc />
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
                name: "BinhLuan");

            migrationBuilder.DropTable(
                name: "ChiTietTheLoaiPhimBo");

            migrationBuilder.DropTable(
                name: "ChiTietTheLoaiPhimLe");

            migrationBuilder.DropTable(
                name: "DanhGia");

            migrationBuilder.DropTable(
                name: "HopPhim");

            migrationBuilder.DropTable(
                name: "LichSuXem");

            migrationBuilder.DropTable(
                name: "TapPhim");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "TheLoai");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "PhimLe");

            migrationBuilder.DropTable(
                name: "PhimBo");

            migrationBuilder.DropTable(
                name: "Nam");

            migrationBuilder.DropTable(
                name: "QuocGia");

            migrationBuilder.DropTable(
                name: "TrangThai");
        }
    }
}
