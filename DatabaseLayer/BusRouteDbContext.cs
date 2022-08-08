using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using BusRouteApi.DatabaseLayer.Models;

namespace BusRouteApi.DatabaseLayer
{
    public partial class BusRouteDbContext : DbContext
    {
        private readonly IConfiguration _configuration;
        public BusRouteDbContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public BusRouteDbContext(DbContextOptions<BusRouteDbContext> options, IConfiguration configuration)
            : base(options)
        {
            _configuration = configuration;
        }

        public virtual DbSet<Bus> Buses { get; set; } = null!;
        public virtual DbSet<BusRoute> BusRoutes { get; set; } = null!;
        public virtual DbSet<OilPrice> OilPrices { get; set; } = null!;
        public virtual DbSet<Payee> Payees { get; set; } = null!;
        public virtual DbSet<DatabaseLayer.Models.Route> Routes { get; set; } = null!;
        public virtual DbSet<RoutePrice> RoutePrices { get; set; } = null!;
        public virtual DbSet<Shift> Shifts { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<Vendor> Vendors { get; set; } = null!;
        public virtual DbSet<VendorPayee> VendorPayees { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql(_configuration.GetSection("AppSettings:Database").Value);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresEnum("Role", new[] { "Manager", "CarCenter", "Clerk", "Admin", "Guest" })
                .HasPostgresEnum("RouteType", new[] { "General", "Special" });

            modelBuilder.Entity<Bus>(entity =>
            {
                entity.ToTable("Bus");

                entity.Property(e => e.Id).UseIdentityAlwaysColumn();

                entity.Property(e => e.BusNumber).HasMaxLength(10);

                entity.HasOne(d => d.Payee)
                    .WithMany(p => p.Bus)
                    .HasForeignKey(d => d.PayeeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PayeeId_Ref_Payee_Id");
            });

            modelBuilder.Entity<BusRoute>(entity =>
            {
                entity.ToTable("BusRoute");

                entity.Property(e => e.Id).UseIdentityAlwaysColumn();

                entity.HasOne(d => d.Bus)
                    .WithMany(p => p.BusRoutes)
                    .HasForeignKey(d => d.BusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("BusId_Ref_Bus_Id");

                entity.HasOne(d => d.OilPrice)
                    .WithMany(p => p.BusRoutes)
                    .HasForeignKey(d => d.OilPriceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("OilPriceId_Ref_OilPrice_Id");

                entity.HasOne(d => d.RoutePrice)
                    .WithMany(p => p.BusRoutes)
                    .HasForeignKey(d => d.RoutePriceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("RoutePriceId_Ref_RoutePrice_Id");

                entity.HasOne(d => d.Shift)
                    .WithMany(p => p.BusRoutes)
                    .HasForeignKey(d => d.ShiftId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("ShiftId_Ref_Shift_Id");

                entity.HasOne(d => d.Vendor)
                    .WithMany(p => p.BusRoutes)
                    .HasForeignKey(d => d.VendorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("VendorId_Ref_Vendor_Id");
            });

            modelBuilder.Entity<OilPrice>(entity =>
            {
                entity.ToTable("OilPrice");

                entity.Property(e => e.Id).UseIdentityAlwaysColumn();
            });

            modelBuilder.Entity<Payee>(entity =>
            {
                entity.ToTable("Payee");

                entity.Property(e => e.Id).UseIdentityAlwaysColumn();

                entity.Property(e => e.Name).HasMaxLength(100);
            });

            modelBuilder.Entity<DatabaseLayer.Models.Route>(entity =>
            {
                entity.ToTable("Route");

                entity.Property(e => e.Id).UseIdentityAlwaysColumn();

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.RouteType).HasMaxLength(15);
            });

            modelBuilder.Entity<RoutePrice>(entity =>
            {
                entity.ToTable("RoutePrice");

                entity.Property(e => e.Id).UseIdentityAlwaysColumn();

                entity.HasOne(d => d.Route)
                    .WithMany(p => p.RoutePrices)
                    .HasForeignKey(d => d.RouteId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("RouteId_Ref_Route_Id");
            });

            modelBuilder.Entity<Shift>(entity =>
            {
                entity.ToTable("Shift");

                entity.Property(e => e.Id).UseIdentityAlwaysColumn();

                entity.Property(e => e.Name).HasMaxLength(20);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");

                entity.Property(e => e.Id).UseIdentityAlwaysColumn();

                entity.Property(e => e.Name).HasMaxLength(70);

                entity.Property(e => e.Password).HasMaxLength(200);

                entity.Property(e => e.Role).HasMaxLength(15);

                entity.Property(e => e.Username).HasMaxLength(50);
            });

            modelBuilder.Entity<Vendor>(entity =>
            {
                entity.ToTable("Vendor");

                entity.Property(e => e.Id).UseIdentityAlwaysColumn();

                entity.Property(e => e.Name).HasMaxLength(100);
            });

            modelBuilder.Entity<VendorPayee>(entity =>
            {
                entity.ToTable("VendorPayee");

                entity.Property(e => e.Id).UseIdentityAlwaysColumn();

                entity.HasOne(d => d.Payee)
                    .WithMany(p => p.VendorPayees)
                    .HasForeignKey(d => d.PayeeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PayeeId_Ref_Payee_Id");

                entity.HasOne(d => d.Vendor)
                    .WithMany(p => p.VendorPayees)
                    .HasForeignKey(d => d.VendorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("VendorId_Ref_Vendor_Id");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
