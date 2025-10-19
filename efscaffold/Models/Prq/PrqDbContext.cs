using Microsoft.EntityFrameworkCore;

namespace efscaffold.Models.Prq
{
    public class PrqDbContext : DbContext
    {
        public PrqDbContext(DbContextOptions<PrqDbContext> options) : base(options) { }

        public DbSet<PrqAutomovil> PRQ_Automoviles { get; set; } = null!;
        public DbSet<PrqParqueo> PRQ_Parqueo { get; set; } = null!;
        public DbSet<PrqIngresoAutomovil> PRQ_IngresoAutomoviles { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<PrqAutomovil>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK_PRQ_Automoviles");
                entity.Property(e => e.Manufacturer).HasMaxLength(100).IsRequired();
                entity.Property(e => e.Color).HasMaxLength(50).IsRequired();
                entity.Property(e => e.Type).HasMaxLength(20).IsRequired();
                entity.HasMany(e => e.Ingresos).WithOne(i => i.Automovil).HasForeignKey(i => i.PrqAutomovilId).OnDelete(DeleteBehavior.Cascade).HasConstraintName("fk_prq_ingreso_automovil");
            });

            modelBuilder.Entity<PrqParqueo>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK_PRQ_Parqueo");
                entity.Property(e => e.ProvinceName).HasMaxLength(100).IsRequired();
                entity.Property(e => e.Name).HasMaxLength(150).IsRequired();
                entity.Property(e => e.PricePerHour).HasColumnType("decimal(10,2)").HasDefaultValue(0.00m);
                entity.HasMany(e => e.Ingresos).WithOne(i => i.Parqueo).HasForeignKey(i => i.PrqParqueoId).OnDelete(DeleteBehavior.Cascade).HasConstraintName("fk_prq_ingreso_parqueo");
            });

            modelBuilder.Entity<PrqIngresoAutomovil>(entity =>
            {
                entity.HasKey(e => e.Consecutive).HasName("PK_PRQ_IngresoAutomoviles");
                entity.Property(e => e.EntryDateTime).IsRequired();
                entity.Property(e => e.ExitDateTime).IsRequired(false);
                entity.HasIndex(e => e.PrqParqueoId).HasDatabaseName("idx_prq_ingreso_parqueo");
                entity.HasIndex(e => e.PrqAutomovilId).HasDatabaseName("idx_prq_ingreso_automovil");
            });
        }
    }
}