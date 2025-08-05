using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Modulap.Models;

namespace Modulap.Data
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        // Constructor 
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // relaciones AppUser

            modelBuilder.Entity<AppUser>()
                     .HasMany(u => u.MessagesAsClients)
                     .WithOne(m => m.Client)
                     .HasForeignKey(m => m.ClientId)
                     .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AppUser>()
                .HasMany(u => u.MessagesAsAdministrators)
                .WithOne(m => m.Administrator)
                .HasForeignKey(m => m.AdministratorId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AppUser>()
                .HasMany(u => u.QualificationsAsClients)
                .WithOne(q => q.Client)
                .HasForeignKey(q => q.ClientId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configuración corregida para Sales
            modelBuilder.Entity<AppUser>()
                .HasMany(u => u.Sales)
                .WithOne(s => s.User)
                .HasForeignKey(s => s.UserId)
                .OnDelete(DeleteBehavior.Restrict);

        

            modelBuilder.Entity<AppUser>()
                .HasMany(u => u.BuysAdmins)
                .WithOne(b => b.Administrator)
                .HasForeignKey(b => b.AdminId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AppUser>()
                .HasMany(u => u.Estimates)
                .WithOne(e => e.Client)
                .HasForeignKey(e => e.ClientId)
                .OnDelete(DeleteBehavior.Restrict);

            // References to ChickenCoop

            modelBuilder.Entity<ChickenCoop>()
                .HasMany(c => c.Messages)
                .WithOne(m => m.ChickenCoop)
                .HasForeignKey(m => m.ChickenCoopId);

            modelBuilder.Entity<ChickenCoop>()
                .HasMany(c => c.Qualifications)
                .WithOne(q => q.ChickenCoop)
                .HasForeignKey(q => q.ChickenCoopId);

            modelBuilder.Entity<ChickenCoop>()
                .HasMany(c => c.SaleDetail)
                .WithOne(sd => sd.ChickenCoop)
                .HasForeignKey(sd => sd.ChickenCoopId);

            modelBuilder.Entity<ChickenCoop>() //se configura la relacion desde la entidad de uno
                .HasMany(c => c.Recipes)
                .WithOne(r => r.ChickenCoop)
                .HasForeignKey(r => r.ChickenCoopId);

            modelBuilder.Entity<ChickenCoop>()
                .HasMany(c => c.Estimates) // Estations es la colección en ChickenCoop
                .WithOne(e => e.ChickenCoop) //ChickenCoop  propiedad de navegación en Estation
                .HasForeignKey(e => e.ChickenCoopId); // clave foránea en Estation

            // las propiedades de navegacion representan las relaciones que hay entre dos modelos o entidades 

            // Reference to Message
            modelBuilder.Entity<Message>()
                .HasOne(m => m.ParentMessage)
                .WithMany(m => m.ChildMessages)
                .HasForeignKey(m => m.ParentMessageId)
                .OnDelete(DeleteBehavior.Restrict);

            //Reference to Sale
            modelBuilder.Entity<Sale>()
                .HasMany(s => s.SaleDetails)
                .WithOne(sd => sd.Sale)
                .HasForeignKey(sd => sd.SaleId)
                .OnDelete(DeleteBehavior.Restrict);

            // Reference to Component
            modelBuilder.Entity<Component>()
                .HasMany(cp => cp.ComponentLots)
                .WithOne(cl => cl.Component)
                .HasForeignKey(cl => cl.ComponentId);

            modelBuilder.Entity<Component>()
                .HasMany(cp => cp.RecipeDetails)
                .WithOne(rd => rd.Component)
                .HasForeignKey(rd => rd.ComponentId);

            modelBuilder.Entity<Component>()
                .HasMany(cp => cp.ComponentLosses)
                .WithOne(cls => cls.Component)
                .HasForeignKey(cls => cls.ComponentId);

            //Refence to Buys
            modelBuilder.Entity<Buys>()
                .HasMany(b => b.ComponentLots)
                .WithOne(cl => cl.Buys)
                .HasForeignKey(cl => cl.BuysId);

            // Reference to Supplier
            modelBuilder.Entity<Supplier>()
           .HasMany(s => s.SuppliersPayments)
           .WithOne(sp => sp.Supplier)
           .HasForeignKey(sp => sp.SupplierId)
           .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Supplier>()
                .HasMany(s => s.ComponentLots)
                .WithOne(cl => cl.Supplier)
                .HasForeignKey(cl => cl.SupplierId);

            // reference to component Lot

            modelBuilder.Entity<ComponentLot>()
                .HasMany(cl => cl.SupplierPayments)
                .WithOne(sp => sp.ComponentLot)
                .HasForeignKey(sp => sp.ComponentLotId)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<ComponentLot>()
                .HasMany(cl => cl.ComponentProductions)
                .WithOne(cp => cp.ComponentLot)
                .HasForeignKey(cp => cp.ComponentLotId)
                .OnDelete(DeleteBehavior.Restrict); ;

            modelBuilder.Entity<ComponentLot>()
                .HasMany(cl => cl.ComponentCostings)
                .WithOne(cc => cc.ComponentLot)
                .HasForeignKey(cc => cc.ComponentLotId)
                .OnDelete(DeleteBehavior.Restrict);

            //  reference to component Lot

            modelBuilder.Entity<Recipe>()
                .HasMany(r => r.RecipeDetail)
                .WithOne(rd => rd.Recipe)
                .HasForeignKey(rd => rd.RecipeId);

            modelBuilder.Entity<Recipe>()
                .HasMany(r => r.ProductionLots)
                .WithOne(pl => pl.Recipe)
                .HasForeignKey(rd => rd.RecipeId);

            // reference to recipe detail
            modelBuilder.Entity<RecipeDetail>()
                .HasMany(rd => rd.ComponentCostings)
                .WithOne(cc => cc.RecipeDetail)
                .HasForeignKey(cc => cc.RecipeDetailId)
                .OnDelete(DeleteBehavior.Restrict);

            // reference to ProductionLot
            modelBuilder.Entity<ProductionLot>()
                .HasMany(pl => pl.ComponentProductions)
                .WithOne(cp => cp.ProductionLot)
                .HasForeignKey(cp => cp.ProductionLotId);

            //reference to ComponentProdutcion
            modelBuilder.Entity<ComponentProduction>()
                .HasMany(cp => cp.ComponentCostings)
                .WithOne(cc => cc.ComponentProduction)
                .HasForeignKey(cc => cc.ComponentProductionId)
                .OnDelete(DeleteBehavior.Restrict);

        // reference to ComponenLoss
        modelBuilder.Entity<ComponentLoss>()
                .HasMany(clss => clss.ComponentCostings)
                .WithOne(cc => cc.ComponentLoss)
                .HasForeignKey(cc => cc.ComponentLossId);

            // Configuración de enums
            modelBuilder.Entity<ChickenCoop>()
                .Property(c => c.AvailabilityStatus)
                .HasConversion<string>();

            modelBuilder.Entity<ComponentLoss>()
                .Property(cl => cl.Type)
                .HasConversion<string>();

            modelBuilder.Entity<Message>()
                .Property(m => m.TypeMessage)
                .HasConversion<string>();

            modelBuilder.Entity<Message>()
                .Property(m => m.StatusMessage)
                .HasConversion<string>();

            modelBuilder.Entity<ProductionLot>()
                .Property(pl => pl.Status)
                .HasConversion<string>();

            modelBuilder.Entity<Sale>()
                .Property(s => s.Type)
                .HasConversion<string>();

            modelBuilder.Entity<Estimate>()
                .Property(es => es.Status)
                .HasConversion<string>();

        }

        // DbSets para cada entidad
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<Buys> Buys { get; set; }
        public DbSet<ChickenCoop> ChickenCoops { get; set; }
        public DbSet<Component> Components { get; set; }
        public DbSet<ComponentCosting> ComponentCostings { get; set; }
        public DbSet<ComponentLoss> ComponentLosses { get; set; }
        public DbSet<ComponentLot> ComponentLots { get; set; }
        public DbSet<ComponentProduction> ComponentProductions { get; set; }
        public DbSet<Estimate> Estimates { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<PossibleClient> PossibleClients { get; set; }
        public DbSet<ProductionLot> ProductionLots { get; set; }
        public DbSet<Qualification> Qualifications { get; set; }
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<RecipeDetail> RecipeDetails { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<SaleDetail> SaleDetails { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<SupplierPayment> SupplierPayments { get; set; }
    }
}
