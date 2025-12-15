using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetBlaze.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAttributesInUserDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CertificatePassword",
                table: "UserDetails");

            migrationBuilder.DropColumn(
                name: "DeviceNumber",
                table: "UserDetails");

            migrationBuilder.RenameColumn(
                name: "Key",
                table: "UserDetails",
                newName: "AaGuid");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "UserDetails",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn)
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "UserDetails",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CredType",
                table: "UserDetails",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<byte[]>(
                name: "CredentialId",
                table: "UserDetails",
                type: "longblob",
                nullable: false);

            migrationBuilder.AddColumn<string>(
                name: "DeviceInfo",
                table: "UserDetails",
                type: "varchar(500)",
                maxLength: 500,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "IpAddress",
                table: "UserDetails",
                type: "varchar(50)",
                maxLength: 50,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "UserDetails",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<byte[]>(
                name: "PublicKey",
                table: "UserDetails",
                type: "longblob",
                nullable: false);

            migrationBuilder.AddColumn<string>(
                name: "RevokeReason",
                table: "UserDetails",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "RevokedAt",
                table: "UserDetails",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RevokedBy",
                table: "UserDetails",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<uint>(
                name: "SignatureCounter",
                table: "UserDetails",
                type: "int unsigned",
                nullable: false,
                defaultValue: 0u);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "UserDetails",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserAgent",
                table: "UserDetails",
                type: "varchar(200)",
                maxLength: 200,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "UserDetails");

            migrationBuilder.DropColumn(
                name: "CredType",
                table: "UserDetails");

            migrationBuilder.DropColumn(
                name: "CredentialId",
                table: "UserDetails");

            migrationBuilder.DropColumn(
                name: "DeviceInfo",
                table: "UserDetails");

            migrationBuilder.DropColumn(
                name: "IpAddress",
                table: "UserDetails");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "UserDetails");

            migrationBuilder.DropColumn(
                name: "PublicKey",
                table: "UserDetails");

            migrationBuilder.DropColumn(
                name: "RevokeReason",
                table: "UserDetails");

            migrationBuilder.DropColumn(
                name: "RevokedAt",
                table: "UserDetails");

            migrationBuilder.DropColumn(
                name: "RevokedBy",
                table: "UserDetails");

            migrationBuilder.DropColumn(
                name: "SignatureCounter",
                table: "UserDetails");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "UserDetails");

            migrationBuilder.DropColumn(
                name: "UserAgent",
                table: "UserDetails");

            migrationBuilder.RenameColumn(
                name: "AaGuid",
                table: "UserDetails",
                newName: "Key");

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "UserDetails",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn)
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddColumn<string>(
                name: "CertificatePassword",
                table: "UserDetails",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "DeviceNumber",
                table: "UserDetails",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
