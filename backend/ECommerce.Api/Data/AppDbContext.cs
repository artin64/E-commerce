using ECommerce.Api.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Api.Data;

public sealed class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<UserEntity> Users => Set<UserEntity>();
    public DbSet<StoreEntity> Stores => Set<StoreEntity>();
    public DbSet<CategoryEntity> Categories => Set<CategoryEntity>();
    public DbSet<ProductEntity> Products => Set<ProductEntity>();
    public DbSet<AdEntity> Ads => Set<AdEntity>();
    public DbSet<OrderEntity> Orders => Set<OrderEntity>();
    public DbSet<OrderItemEntity> OrderItems => Set<OrderItemEntity>();
    public DbSet<ReviewEntity> Reviews => Set<ReviewEntity>();
    public DbSet<WishlistItemEntity> WishlistItems => Set<WishlistItemEntity>();
    public DbSet<GiftCardEntity> GiftCards => Set<GiftCardEntity>();
    public DbSet<NotificationEntity> Notifications => Set<NotificationEntity>();
    public DbSet<ChatMessageEntity> ChatMessages => Set<ChatMessageEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<UserEntity>(b =>
        {
            b.HasKey(x => x.Id);
            b.HasIndex(x => x.Email).IsUnique();
            b.Property(x => x.Email).HasMaxLength(320);
            b.Property(x => x.Name).HasMaxLength(200);
        });

        modelBuilder.Entity<StoreEntity>(b =>
        {
            b.HasKey(x => x.Id);
            b.HasIndex(x => x.StoreKey).IsUnique();
            b.Property(x => x.StoreKey).HasMaxLength(80);
            b.Property(x => x.Name).HasMaxLength(200);

            b.HasOne(x => x.OwnerUser)
                .WithOne()
                .HasForeignKey<StoreEntity>(x => x.OwnerUserId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<CategoryEntity>(b =>
        {
            b.HasKey(x => x.Id);
            b.HasIndex(x => new { x.StoreId, x.Name }).IsUnique();
            b.Property(x => x.Name).HasMaxLength(140);
            b.HasOne(x => x.Store).WithMany(s => s.Categories).HasForeignKey(x => x.StoreId);
        });

        modelBuilder.Entity<ProductEntity>(b =>
        {
            b.HasKey(x => x.Id);
            b.HasIndex(x => new { x.StoreId, x.Name });
            b.Property(x => x.Name).HasMaxLength(200);
            b.HasOne(x => x.Store).WithMany(s => s.Products).HasForeignKey(x => x.StoreId);
            b.HasOne(x => x.Category).WithMany().HasForeignKey(x => x.CategoryId).OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<OrderEntity>(b =>
        {
            b.HasKey(x => x.Id);
            b.HasIndex(x => new { x.StoreId, x.CreatedAtUtc });
            b.HasOne(x => x.Store).WithMany().HasForeignKey(x => x.StoreId);
            b.HasOne(x => x.BuyerUser).WithMany().HasForeignKey(x => x.BuyerUserId);
            b.Property(x => x.BuyerEmail).HasMaxLength(320);
        });

        modelBuilder.Entity<OrderItemEntity>(b =>
        {
            b.HasKey(x => x.Id);
            b.HasOne(x => x.Order).WithMany(o => o.Items).HasForeignKey(x => x.OrderId);
            b.HasOne(x => x.Product).WithMany().HasForeignKey(x => x.ProductId);
        });

        modelBuilder.Entity<GiftCardEntity>(b =>
        {
            b.HasKey(x => x.Id);
            b.HasIndex(x => new { x.StoreId, x.Code }).IsUnique();
            b.Property(x => x.Code).HasMaxLength(64);
            b.HasOne(x => x.Store).WithMany(s => s.GiftCards).HasForeignKey(x => x.StoreId);
        });

        modelBuilder.Entity<WishlistItemEntity>(b =>
        {
            b.HasKey(x => x.Id);
            b.HasIndex(x => new { x.StoreId, x.BuyerUserId, x.ProductId }).IsUnique();
        });

        modelBuilder.Entity<ReviewEntity>(b =>
        {
            b.HasKey(x => x.Id);
            b.HasIndex(x => new { x.StoreId, x.ProductId, x.CreatedAtUtc });
        });

        modelBuilder.Entity<NotificationEntity>(b =>
        {
            b.HasKey(x => x.Id);
            b.HasIndex(x => new { x.UserId, x.CreatedAtUtc });
            b.Property(x => x.Type).HasMaxLength(30);
        });

        modelBuilder.Entity<ChatMessageEntity>(b =>
        {
            b.HasKey(x => x.Id);
            b.HasIndex(x => new { x.StoreId, x.ProductId, x.CreatedAtUtc });
            b.Property(x => x.Sender).HasMaxLength(10);
        });
    }
}

