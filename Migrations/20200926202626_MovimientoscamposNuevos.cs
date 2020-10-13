using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApiParquimetros.Migrations
{
    public partial class MovimientoscamposNuevos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "BalanceBefore",
                table: "tbmovimientos",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<DateTime>(
                name: "CardExpirationDate",
                table: "tbmovimientos",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CardReference",
                table: "tbmovimientos",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CardScheme",
                table: "tbmovimientos",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "CuspmrPagateliaNewBalance",
                table: "tbmovimientos",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "CuspmrType",
                table: "tbmovimientos",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "DiscountAmountCurrencyId",
                table: "tbmovimientos",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "DiscountBalanceBefore",
                table: "tbmovimientos",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "DiscountBalanceCurrencyId",
                table: "tbmovimientos",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "ExternalId1",
                table: "tbmovimientos",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ExternalId2",
                table: "tbmovimientos",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ExternalId3",
                table: "tbmovimientos",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "FixedFee",
                table: "tbmovimientos",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "InsDescription",
                table: "tbmovimientos",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InsShortdesc",
                table: "tbmovimientos",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MaskedCardNumber",
                table: "tbmovimientos",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "PartialFixedFee",
                table: "tbmovimientos",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "PartialPercFee",
                table: "tbmovimientos",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "PartialVat1",
                table: "tbmovimientos",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "PercFee",
                table: "tbmovimientos",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "PercFeeTopped",
                table: "tbmovimientos",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "PercVat1",
                table: "tbmovimientos",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "PercVat2",
                table: "tbmovimientos",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<bool>(
                name: "PermitAutoRenew",
                table: "tbmovimientos",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "PermitExpiration",
                table: "tbmovimientos",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Plate10",
                table: "tbmovimientos",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Plate2",
                table: "tbmovimientos",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Plate3",
                table: "tbmovimientos",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Plate4",
                table: "tbmovimientos",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Plate45",
                table: "tbmovimientos",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Plate6",
                table: "tbmovimientos",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Plate7",
                table: "tbmovimientos",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Plate8",
                table: "tbmovimientos",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Plate9",
                table: "tbmovimientos",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "RefundAmount",
                table: "tbmovimientos",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "Sector",
                table: "tbmovimientos",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ServiceChargeTypeId",
                table: "tbmovimientos",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "ShopkeeperAmount",
                table: "tbmovimientos",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<bool>(
                name: "ShopkeeperOp",
                table: "tbmovimientos",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<double>(
                name: "ShopkeeperProfit",
                table: "tbmovimientos",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "Tariff",
                table: "tbmovimientos",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TicketNumber",
                table: "tbmovimientos",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "TotalAmount",
                table: "tbmovimientos",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<bool>(
                name: "TransStatus",
                table: "tbmovimientos",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<double>(
                name: "bonificacion",
                table: "tbmovimientos",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "tipo_vehiculo",
                table: "tbmovimientos",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "valor_sin_bonificar",
                table: "tbmovimientos",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BalanceBefore",
                table: "tbmovimientos");

            migrationBuilder.DropColumn(
                name: "CardExpirationDate",
                table: "tbmovimientos");

            migrationBuilder.DropColumn(
                name: "CardReference",
                table: "tbmovimientos");

            migrationBuilder.DropColumn(
                name: "CardScheme",
                table: "tbmovimientos");

            migrationBuilder.DropColumn(
                name: "CuspmrPagateliaNewBalance",
                table: "tbmovimientos");

            migrationBuilder.DropColumn(
                name: "CuspmrType",
                table: "tbmovimientos");

            migrationBuilder.DropColumn(
                name: "DiscountAmountCurrencyId",
                table: "tbmovimientos");

            migrationBuilder.DropColumn(
                name: "DiscountBalanceBefore",
                table: "tbmovimientos");

            migrationBuilder.DropColumn(
                name: "DiscountBalanceCurrencyId",
                table: "tbmovimientos");

            migrationBuilder.DropColumn(
                name: "ExternalId1",
                table: "tbmovimientos");

            migrationBuilder.DropColumn(
                name: "ExternalId2",
                table: "tbmovimientos");

            migrationBuilder.DropColumn(
                name: "ExternalId3",
                table: "tbmovimientos");

            migrationBuilder.DropColumn(
                name: "FixedFee",
                table: "tbmovimientos");

            migrationBuilder.DropColumn(
                name: "InsDescription",
                table: "tbmovimientos");

            migrationBuilder.DropColumn(
                name: "InsShortdesc",
                table: "tbmovimientos");

            migrationBuilder.DropColumn(
                name: "MaskedCardNumber",
                table: "tbmovimientos");

            migrationBuilder.DropColumn(
                name: "PartialFixedFee",
                table: "tbmovimientos");

            migrationBuilder.DropColumn(
                name: "PartialPercFee",
                table: "tbmovimientos");

            migrationBuilder.DropColumn(
                name: "PartialVat1",
                table: "tbmovimientos");

            migrationBuilder.DropColumn(
                name: "PercFee",
                table: "tbmovimientos");

            migrationBuilder.DropColumn(
                name: "PercFeeTopped",
                table: "tbmovimientos");

            migrationBuilder.DropColumn(
                name: "PercVat1",
                table: "tbmovimientos");

            migrationBuilder.DropColumn(
                name: "PercVat2",
                table: "tbmovimientos");

            migrationBuilder.DropColumn(
                name: "PermitAutoRenew",
                table: "tbmovimientos");

            migrationBuilder.DropColumn(
                name: "PermitExpiration",
                table: "tbmovimientos");

            migrationBuilder.DropColumn(
                name: "Plate10",
                table: "tbmovimientos");

            migrationBuilder.DropColumn(
                name: "Plate2",
                table: "tbmovimientos");

            migrationBuilder.DropColumn(
                name: "Plate3",
                table: "tbmovimientos");

            migrationBuilder.DropColumn(
                name: "Plate4",
                table: "tbmovimientos");

            migrationBuilder.DropColumn(
                name: "Plate45",
                table: "tbmovimientos");

            migrationBuilder.DropColumn(
                name: "Plate6",
                table: "tbmovimientos");

            migrationBuilder.DropColumn(
                name: "Plate7",
                table: "tbmovimientos");

            migrationBuilder.DropColumn(
                name: "Plate8",
                table: "tbmovimientos");

            migrationBuilder.DropColumn(
                name: "Plate9",
                table: "tbmovimientos");

            migrationBuilder.DropColumn(
                name: "RefundAmount",
                table: "tbmovimientos");

            migrationBuilder.DropColumn(
                name: "Sector",
                table: "tbmovimientos");

            migrationBuilder.DropColumn(
                name: "ServiceChargeTypeId",
                table: "tbmovimientos");

            migrationBuilder.DropColumn(
                name: "ShopkeeperAmount",
                table: "tbmovimientos");

            migrationBuilder.DropColumn(
                name: "ShopkeeperOp",
                table: "tbmovimientos");

            migrationBuilder.DropColumn(
                name: "ShopkeeperProfit",
                table: "tbmovimientos");

            migrationBuilder.DropColumn(
                name: "Tariff",
                table: "tbmovimientos");

            migrationBuilder.DropColumn(
                name: "TicketNumber",
                table: "tbmovimientos");

            migrationBuilder.DropColumn(
                name: "TotalAmount",
                table: "tbmovimientos");

            migrationBuilder.DropColumn(
                name: "TransStatus",
                table: "tbmovimientos");

            migrationBuilder.DropColumn(
                name: "bonificacion",
                table: "tbmovimientos");

            migrationBuilder.DropColumn(
                name: "tipo_vehiculo",
                table: "tbmovimientos");

            migrationBuilder.DropColumn(
                name: "valor_sin_bonificar",
                table: "tbmovimientos");
        }
    }
}
