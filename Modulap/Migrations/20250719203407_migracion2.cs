using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Modulap.Migrations
{
    /// <inheritdoc />
    public partial class migracion2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Buys",
                columns: table => new
                {
                    IdBuys = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AdminId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DateBuys = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Total = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Buys", x => x.IdBuys);
                    table.ForeignKey(
                        name: "FK_Buys_AspNetUsers_AdminId",
                        column: x => x.AdminId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ChickenCoops",
                columns: table => new
                {
                    IdChickenCoop = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AvailabilityStatus = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChickenCoops", x => x.IdChickenCoop);
                });

            migrationBuilder.CreateTable(
                name: "Components",
                columns: table => new
                {
                    IdComponent = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Components", x => x.IdComponent);
                });

            migrationBuilder.CreateTable(
                name: "PossibleClients",
                columns: table => new
                {
                    IdPossibleClient = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PossibleClients", x => x.IdPossibleClient);
                });

            migrationBuilder.CreateTable(
                name: "Sales",
                columns: table => new
                {
                    IdSale = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RegistrationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeliveryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sales", x => x.IdSale);
                    table.ForeignKey(
                        name: "FK_Sales_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Suppliers",
                columns: table => new
                {
                    IdSupplier = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RegistrationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Suppliers", x => x.IdSupplier);
                });

            migrationBuilder.CreateTable(
                name: "Estimates",
                columns: table => new
                {
                    IdEstimate = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ChickenCoopId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClientId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ChickenCoopLocation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    QuantityChickens = table.Column<int>(type: "int", nullable: false),
                    ChickenCoopSize = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    EnergyType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    functionalities = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    connectiontype = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhysicalInstallation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PriceTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Estimates", x => x.IdEstimate);
                    table.ForeignKey(
                        name: "FK_Estimates_AspNetUsers_ClientId",
                        column: x => x.ClientId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Estimates_ChickenCoops_ChickenCoopId",
                        column: x => x.ChickenCoopId,
                        principalTable: "ChickenCoops",
                        principalColumn: "IdChickenCoop",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    IdMessage = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClientId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AdministratorId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ChickenCoopId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ParentMessageId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TypeMessage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StatusMessage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.IdMessage);
                    table.ForeignKey(
                        name: "FK_Messages_AspNetUsers_AdministratorId",
                        column: x => x.AdministratorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Messages_AspNetUsers_ClientId",
                        column: x => x.ClientId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Messages_ChickenCoops_ChickenCoopId",
                        column: x => x.ChickenCoopId,
                        principalTable: "ChickenCoops",
                        principalColumn: "IdChickenCoop",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Messages_Messages_ParentMessageId",
                        column: x => x.ParentMessageId,
                        principalTable: "Messages",
                        principalColumn: "IdMessage",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Qualifications",
                columns: table => new
                {
                    IdQualification = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClientId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ChickenCoopId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    punctuation = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Qualifications", x => x.IdQualification);
                    table.ForeignKey(
                        name: "FK_Qualifications_AspNetUsers_ClientId",
                        column: x => x.ClientId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Qualifications_ChickenCoops_ChickenCoopId",
                        column: x => x.ChickenCoopId,
                        principalTable: "ChickenCoops",
                        principalColumn: "IdChickenCoop",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Recipes",
                columns: table => new
                {
                    IdRecipe = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ChickenCoopId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Amount = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recipes", x => x.IdRecipe);
                    table.ForeignKey(
                        name: "FK_Recipes_ChickenCoops_ChickenCoopId",
                        column: x => x.ChickenCoopId,
                        principalTable: "ChickenCoops",
                        principalColumn: "IdChickenCoop",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ComponentLosses",
                columns: table => new
                {
                    IdComponentLoss = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ComponentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Amount = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComponentLosses", x => x.IdComponentLoss);
                    table.ForeignKey(
                        name: "FK_ComponentLosses_Components_ComponentId",
                        column: x => x.ComponentId,
                        principalTable: "Components",
                        principalColumn: "IdComponent",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SaleDetails",
                columns: table => new
                {
                    IdSaleDetail = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SaleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ChickenCoopId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Amount = table.Column<int>(type: "int", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SaleDetails", x => x.IdSaleDetail);
                    table.ForeignKey(
                        name: "FK_SaleDetails_ChickenCoops_ChickenCoopId",
                        column: x => x.ChickenCoopId,
                        principalTable: "ChickenCoops",
                        principalColumn: "IdChickenCoop",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SaleDetails_Sales_SaleId",
                        column: x => x.SaleId,
                        principalTable: "Sales",
                        principalColumn: "IdSale",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ComponentLots",
                columns: table => new
                {
                    IdComponentLot = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ComponentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BuysId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SupplierId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Amount = table.Column<int>(type: "int", nullable: false),
                    AvailableQuantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComponentLots", x => x.IdComponentLot);
                    table.ForeignKey(
                        name: "FK_ComponentLots_Buys_BuysId",
                        column: x => x.BuysId,
                        principalTable: "Buys",
                        principalColumn: "IdBuys",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ComponentLots_Components_ComponentId",
                        column: x => x.ComponentId,
                        principalTable: "Components",
                        principalColumn: "IdComponent",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ComponentLots_Suppliers_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "Suppliers",
                        principalColumn: "IdSupplier",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductionLots",
                columns: table => new
                {
                    idProductionLot = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RecipeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DateProduction = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AvailableQuantity = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductionLots", x => x.idProductionLot);
                    table.ForeignKey(
                        name: "FK_ProductionLots_Recipes_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "Recipes",
                        principalColumn: "IdRecipe",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RecipeDetails",
                columns: table => new
                {
                    IdRecipeDetail = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RecipeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ComponentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ComponentQuantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecipeDetails", x => x.IdRecipeDetail);
                    table.ForeignKey(
                        name: "FK_RecipeDetails_Components_ComponentId",
                        column: x => x.ComponentId,
                        principalTable: "Components",
                        principalColumn: "IdComponent",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RecipeDetails_Recipes_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "Recipes",
                        principalColumn: "IdRecipe",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SupplierPayments",
                columns: table => new
                {
                    IdSupplierPayment = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SupplierId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ComponentLotId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupplierPayments", x => x.IdSupplierPayment);
                    table.ForeignKey(
                        name: "FK_SupplierPayments_ComponentLots_ComponentLotId",
                        column: x => x.ComponentLotId,
                        principalTable: "ComponentLots",
                        principalColumn: "IdComponentLot",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SupplierPayments_Suppliers_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "Suppliers",
                        principalColumn: "IdSupplier",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ComponentProductions",
                columns: table => new
                {
                    IdComponentProduction = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ComponentLotId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductionLotId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QuantityUsed = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComponentProductions", x => x.IdComponentProduction);
                    table.ForeignKey(
                        name: "FK_ComponentProductions_ComponentLots_ComponentLotId",
                        column: x => x.ComponentLotId,
                        principalTable: "ComponentLots",
                        principalColumn: "IdComponentLot",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ComponentProductions_ProductionLots_ProductionLotId",
                        column: x => x.ProductionLotId,
                        principalTable: "ProductionLots",
                        principalColumn: "idProductionLot",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ComponentCostings",
                columns: table => new
                {
                    IdComponentCosting = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ComponentLossId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RecipeDetailId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ComponentLotId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ComponentProductionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Entrance = table.Column<int>(type: "int", nullable: false),
                    Exit = table.Column<int>(type: "int", nullable: false),
                    Existence = table.Column<int>(type: "int", nullable: false),
                    cost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Average = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Owes = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ToHave = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Balance = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComponentCostings", x => x.IdComponentCosting);
                    table.ForeignKey(
                        name: "FK_ComponentCostings_ComponentLosses_ComponentLossId",
                        column: x => x.ComponentLossId,
                        principalTable: "ComponentLosses",
                        principalColumn: "IdComponentLoss",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ComponentCostings_ComponentLots_ComponentLotId",
                        column: x => x.ComponentLotId,
                        principalTable: "ComponentLots",
                        principalColumn: "IdComponentLot",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ComponentCostings_ComponentProductions_ComponentProductionId",
                        column: x => x.ComponentProductionId,
                        principalTable: "ComponentProductions",
                        principalColumn: "IdComponentProduction",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ComponentCostings_RecipeDetails_RecipeDetailId",
                        column: x => x.RecipeDetailId,
                        principalTable: "RecipeDetails",
                        principalColumn: "IdRecipeDetail",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Buys_AdminId",
                table: "Buys",
                column: "AdminId");

            migrationBuilder.CreateIndex(
                name: "IX_ComponentCostings_ComponentLossId",
                table: "ComponentCostings",
                column: "ComponentLossId");

            migrationBuilder.CreateIndex(
                name: "IX_ComponentCostings_ComponentLotId",
                table: "ComponentCostings",
                column: "ComponentLotId");

            migrationBuilder.CreateIndex(
                name: "IX_ComponentCostings_ComponentProductionId",
                table: "ComponentCostings",
                column: "ComponentProductionId");

            migrationBuilder.CreateIndex(
                name: "IX_ComponentCostings_RecipeDetailId",
                table: "ComponentCostings",
                column: "RecipeDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_ComponentLosses_ComponentId",
                table: "ComponentLosses",
                column: "ComponentId");

            migrationBuilder.CreateIndex(
                name: "IX_ComponentLots_BuysId",
                table: "ComponentLots",
                column: "BuysId");

            migrationBuilder.CreateIndex(
                name: "IX_ComponentLots_ComponentId",
                table: "ComponentLots",
                column: "ComponentId");

            migrationBuilder.CreateIndex(
                name: "IX_ComponentLots_SupplierId",
                table: "ComponentLots",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_ComponentProductions_ComponentLotId",
                table: "ComponentProductions",
                column: "ComponentLotId");

            migrationBuilder.CreateIndex(
                name: "IX_ComponentProductions_ProductionLotId",
                table: "ComponentProductions",
                column: "ProductionLotId");

            migrationBuilder.CreateIndex(
                name: "IX_Estimates_ChickenCoopId",
                table: "Estimates",
                column: "ChickenCoopId");

            migrationBuilder.CreateIndex(
                name: "IX_Estimates_ClientId",
                table: "Estimates",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_AdministratorId",
                table: "Messages",
                column: "AdministratorId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_ChickenCoopId",
                table: "Messages",
                column: "ChickenCoopId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_ClientId",
                table: "Messages",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_ParentMessageId",
                table: "Messages",
                column: "ParentMessageId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductionLots_RecipeId",
                table: "ProductionLots",
                column: "RecipeId");

            migrationBuilder.CreateIndex(
                name: "IX_Qualifications_ChickenCoopId",
                table: "Qualifications",
                column: "ChickenCoopId");

            migrationBuilder.CreateIndex(
                name: "IX_Qualifications_ClientId",
                table: "Qualifications",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeDetails_ComponentId",
                table: "RecipeDetails",
                column: "ComponentId");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeDetails_RecipeId",
                table: "RecipeDetails",
                column: "RecipeId");

            migrationBuilder.CreateIndex(
                name: "IX_Recipes_ChickenCoopId",
                table: "Recipes",
                column: "ChickenCoopId");

            migrationBuilder.CreateIndex(
                name: "IX_SaleDetails_ChickenCoopId",
                table: "SaleDetails",
                column: "ChickenCoopId");

            migrationBuilder.CreateIndex(
                name: "IX_SaleDetails_SaleId",
                table: "SaleDetails",
                column: "SaleId");

            migrationBuilder.CreateIndex(
                name: "IX_Sales_UserId",
                table: "Sales",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_SupplierPayments_ComponentLotId",
                table: "SupplierPayments",
                column: "ComponentLotId");

            migrationBuilder.CreateIndex(
                name: "IX_SupplierPayments_SupplierId",
                table: "SupplierPayments",
                column: "SupplierId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ComponentCostings");

            migrationBuilder.DropTable(
                name: "Estimates");

            migrationBuilder.DropTable(
                name: "Messages");

            migrationBuilder.DropTable(
                name: "PossibleClients");

            migrationBuilder.DropTable(
                name: "Qualifications");

            migrationBuilder.DropTable(
                name: "SaleDetails");

            migrationBuilder.DropTable(
                name: "SupplierPayments");

            migrationBuilder.DropTable(
                name: "ComponentLosses");

            migrationBuilder.DropTable(
                name: "ComponentProductions");

            migrationBuilder.DropTable(
                name: "RecipeDetails");

            migrationBuilder.DropTable(
                name: "Sales");

            migrationBuilder.DropTable(
                name: "ComponentLots");

            migrationBuilder.DropTable(
                name: "ProductionLots");

            migrationBuilder.DropTable(
                name: "Buys");

            migrationBuilder.DropTable(
                name: "Components");

            migrationBuilder.DropTable(
                name: "Suppliers");

            migrationBuilder.DropTable(
                name: "Recipes");

            migrationBuilder.DropTable(
                name: "ChickenCoops");
        }
    }
}
