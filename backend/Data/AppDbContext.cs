using Microsoft.EntityFrameworkCore;
using backend.Entity;

namespace backend.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Size> Sizes { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<ProductInventory> ProductInventories { get; set; }
    public DbSet<ProductImage> ProductImages { get; set; }
    public DbSet<Cart> Carts { get; set; }
    public DbSet<CartItem> CartItems { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<Review> Reviews { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Quan hệ 1-1 giữa User và Cart
        modelBuilder.Entity<User>()
            .HasOne(u => u.Cart)
            .WithOne(c => c.User)
            .HasForeignKey<Cart>(c => c.UserId);

        // Quan hệ 1-n giữa Category và Product
        modelBuilder.Entity<Category>()
            .HasMany(c => c.Products)
            .WithOne(p => p.Category)
            .HasForeignKey(p => p.CategoryId);

        // Quan hệ 1-n giữa Size và ProductInventory
        modelBuilder.Entity<Size>()
            .HasMany(s => s.CartItems)
            .WithOne(ci => ci.Size)
            .HasForeignKey(ci => ci.SizeId);

        // Quan hệ 1-n giữa Product và ProductInventory
        modelBuilder.Entity<Product>()
            .HasMany(p => p.Sizes)
            .WithOne(pi => pi.Product)
            .HasForeignKey(pi => pi.ProductId);

        // Quan hệ 1-n giữa Product và ProductImage
        modelBuilder.Entity<Product>()
            .HasMany(p => p.ProductImages)
            .WithOne(c => c.Product)
            .HasForeignKey(pi => pi.ProductId);

        // Quan hệ 1-n giữa Product và CartItem
        modelBuilder.Entity<Product>()
            .HasMany(p => p.CartItems)
            .WithOne(ci => ci.Product)
            .HasForeignKey(ci => ci.ProductId);

        // Quan hệ 1-n giữa Product và Review
        modelBuilder.Entity<Product>()
            .HasMany(p => p.Reviews)
            .WithOne(r => r.Product)
            .HasForeignKey(r => r.ProductId);

        // Quan hệ 1-n giữa User và Order
        modelBuilder.Entity<User>()
            .HasMany(u => u.Orders)
            .WithOne(o => o.User)
            .HasForeignKey(o => o.UserId);

        // Quan hệ 1-n giữa User và Review
        modelBuilder.Entity<User>()
            .HasMany(u => u.Reviews)
            .WithOne(r => r.User)
            .HasForeignKey(r => r.UserId);

        // Quan hệ 1-n giữa Order và OrderItem
        modelBuilder.Entity<Order>()
            .HasMany(o => o.OrderItems)
            .WithOne(oi => oi.Order)
            .HasForeignKey(oi => oi.OrderId);

        // Quan hệ 1-n giữa Product và OrderItem
        modelBuilder.Entity<Product>()
            .HasMany(p => p.OrderItems)
            .WithOne(oi => oi.Product)
            .HasForeignKey(oi => oi.ProductId)
            .OnDelete(DeleteBehavior.SetNull)
            .IsRequired(false);

        // Quan hệ 1-n giữa Cart và CartItem
        modelBuilder.Entity<Cart>()
            .HasMany(c => c.CartItems)
            .WithOne(ci => ci.Cart)
            .HasForeignKey(ci => ci.CartId);
    }
}
