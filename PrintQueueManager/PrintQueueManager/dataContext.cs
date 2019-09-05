using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace PrintQueueManager.Data
{
    public partial class dataContext : DbContext
    {
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<PrintProfile> PrintProfiles { get; set; }
        public virtual DbSet<ProfileOrder> ProfileOrders { get; set; }
        public virtual DbSet<Preset> Presets { get; set; }
        public virtual DbSet<PresetPrinter> PresetPrinters { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#if DEBUG
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlCe(@"Data Source=C:\projects2\PrintQueueManager\PrintQueueManager\data.sdf");
                //optionsBuilder.UseSqlCe(@"Data Source=C:\projects2\data.sdf");
#else
                optionsBuilder.UseSqlCe(@"Data Source=data.sdf");
#endif
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>(entity =>
            {
                entity.Property(e => e.CustomerEmail).HasMaxLength(50);

                entity.Property(e => e.CustomerName).HasMaxLength(150);

                entity.Property(e => e.ImportedFilename).HasMaxLength(150);

                entity.Property(e => e.LinkedFilename).HasMaxLength(150);

                entity.Property(e => e.OrderNotes).HasMaxLength(250);

                entity.Property(e => e.Remark).HasMaxLength(50);

                entity.Property(e => e.Sku).HasMaxLength(50);
            });

            modelBuilder.Entity<PrintProfile>(entity =>
            {
                entity.Property(e => e.Eta).HasColumnName("ETA");

                entity.Property(e => e.FtpDirectory).HasMaxLength(250);

                entity.Property(e => e.FtpPassword).HasMaxLength(50);

                entity.Property(e => e.FtpServer).HasMaxLength(250);

                entity.Property(e => e.FtpUsername).HasMaxLength(50);

                entity.Property(e => e.ProfileName)
                    .IsRequired()
                    .HasMaxLength(150);
            });

            modelBuilder.Entity<ProfileOrder>(entity =>
            {
                entity.Property(e => e.Reserved1).HasMaxLength(50);

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.ProfileOrders)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProfileOrders_ToTable_1");

                entity.HasOne(d => d.Profile)
                    .WithMany(p => p.ProfileOrders)
                    .HasForeignKey(d => d.ProfileId)
                    .HasConstraintName("FK_ProfileOrders_ToTable");
            });
        }
    }
}
