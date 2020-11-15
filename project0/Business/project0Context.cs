using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Business
{
    public partial class project0Context : DbContext
    {
        public project0Context()
        {
        }

        public project0Context(DbContextOptions<project0Context> options)
            : base(options)
        {
        }

        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Item> Items { get; set; }
        public virtual DbSet<Location> Locations { get; set; }
        public virtual DbSet<LocationItem> LocationItems { get; set; }
        public virtual DbSet<OrderItem> OrderItems { get; set; }
        public virtual DbSet<Sorder> Sorders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>(entity =>
            {
                entity.ToTable("Customer", "Store");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(150);
            });

            modelBuilder.Entity<Item>(entity =>
            {
                entity.ToTable("Item", "Store");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(150);
            });

            modelBuilder.Entity<Location>(entity =>
            {
                entity.ToTable("Location", "Store");

                entity.Property(e => e.Name).HasMaxLength(150);
            });

            modelBuilder.Entity<LocationItem>(entity =>
            {
                entity.HasKey(e => new { e.LocationId, e.ItemId })
                    .HasName("PK__Location__40D94CAF02EA25FE");

                entity.ToTable("LocationItem", "Store");

                entity.HasOne(d => d.Item)
                    .WithMany(p => p.LocationItems)
                    .HasForeignKey(d => d.ItemId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__LocationI__ItemI__6A30C649");

                entity.HasOne(d => d.Location)
                    .WithMany(p => p.LocationItems)
                    .HasForeignKey(d => d.LocationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__LocationI__Locat__693CA210");
            });

            modelBuilder.Entity<OrderItem>(entity =>
            {
                entity.HasKey(e => new { e.OrderId, e.ItemId })
                    .HasName("PK__OrderIte__64B7B3F793438D22");

                entity.ToTable("OrderItem", "Store");

                entity.HasOne(d => d.Item)
                    .WithMany(p => p.OrderItems)
                    .HasForeignKey(d => d.ItemId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__OrderItem__ItemI__76969D2E");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.OrderItems)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__OrderItem__Order__75A278F5");
            });

            modelBuilder.Entity<Sorder>(entity =>
            {
                entity.HasKey(e => e.OrderId)
                    .HasName("PK__SOrder__C3905BCFA72531DF");

                entity.ToTable("SOrder", "Store");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Sorders)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__SOrder__Customer__6FE99F9F");

                entity.HasOne(d => d.Location)
                    .WithMany(p => p.Sorders)
                    .HasForeignKey(d => d.LocationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__SOrder__Location__6EF57B66");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
