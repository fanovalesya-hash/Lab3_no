using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Lab3_no.Models;

public partial class ShopContext : DbContext
{
    public ShopContext()
    {
    }

    public ShopContext(DbContextOptions<ShopContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Manufacturer> Manufacturers { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderArticle> OrderArticles { get; set; }

    public virtual DbSet<PickupPoint> PickupPoints { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductName> ProductNames { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Status> Statuses { get; set; }

    public virtual DbSet<Supplier> Suppliers { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Lab3_Shop_Nasenkina;Integrated Security=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Categori__3214EC0738F36EE3");

            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<Manufacturer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Manufact__3214EC0795DD4A6C");

            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Orders__3214EC0792DD4C4C");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.PickupPoint).WithMany(p => p.Orders).HasConstraintName("FK_Orders_PickupPoints");

            entity.HasOne(d => d.Status).WithMany(p => p.Orders).HasConstraintName("FK_Orders_Statuses");

            entity.HasOne(d => d.User).WithMany(p => p.Orders).HasConstraintName("FK_Orders_Users");
        });

        modelBuilder.Entity<OrderArticle>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__OrderArt__3214EC071DE01F72");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Order).WithMany(p => p.OrderArticles).HasConstraintName("FK_OrdersArticle_Orders");

            entity.HasOne(d => d.Product).WithMany(p => p.OrderArticles).HasConstraintName("FK_OrdersArticle_Products");
        });

        modelBuilder.Entity<PickupPoint>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__PickupPo__3214EC075CC8E9E9");

            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Products__3214EC07458BD246");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Category).WithMany(p => p.Products).HasConstraintName("FK_Products_Categories");

            entity.HasOne(d => d.Manufacturer).WithMany(p => p.Products).HasConstraintName("FK_Products_Manufacturers");

            entity.HasOne(d => d.Name).WithMany(p => p.Products).HasConstraintName("FK_Products_ProductNames");

            entity.HasOne(d => d.Supplier).WithMany(p => p.Products).HasConstraintName("FK_Products_Suppliers");
        });

        modelBuilder.Entity<ProductName>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ProductN__3214EC07AA656562");

            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Roles__3214EC07FA997226");

            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<Status>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Statuses__3214EC07CAF3C017");

            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<Supplier>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Supplier__3214EC079165A994");

            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Users__3214EC07E8AB188E");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.RoleNavigation).WithMany(p => p.Users).HasConstraintName("FK_Users_UserRoles");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
