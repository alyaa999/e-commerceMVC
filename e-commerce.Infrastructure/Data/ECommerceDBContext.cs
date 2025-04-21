using e_commerce.Domain.Entites;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace e_commerce.Infrastructure.Entites;

public class ECommerceDBContext : IdentityDbContext<ApplicationUser>
{
    public ECommerceDBContext(DbContextOptions<ECommerceDBContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Tag> Tags { get; set; }
    public virtual DbSet<Address> Addresses { get; set; }

    public virtual DbSet<Cart> Carts { get; set; }

    public virtual DbSet<CartProduct> CartProducts { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderProduct> OrderProducts { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductImage> ProductImages { get; set; }

    public virtual DbSet<Return> Returns { get; set; }


    public virtual DbSet<Review> Reviews { get; set; }

    public virtual DbSet<Seller> Sellers { get; set; }

    public virtual DbSet<SubCategory> SubCategories { get; set; }

    public virtual DbSet<Wishlist> Wishlists { get; set; }



    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
       //ModelBuilderExtensions.Seed(modelBuilder);
        base.OnModelCreating(modelBuilder);
       


        modelBuilder.Entity<IdentityUserLogin<string>>(entity =>
        {
            entity.HasKey(e => e.UserId);
        });

        modelBuilder.Entity<IdentityUserRole<string>>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.RoleId });
        });

        modelBuilder.Entity<IdentityUserClaim<string>>(entity =>
        {
            entity.HasKey(e => e.Id);
        });

        modelBuilder.Entity<IdentityUserToken<string>>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });
        });




        {
            modelBuilder.Entity<Address>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__Address__3214EC27EC8F225B");

                entity.Property(e => e.IsDefault).HasDefaultValue(false);

                entity.HasOne(d => d.Customer).WithMany(p => p.Addresses)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Address__Custome__4CA06362");
            });

            modelBuilder.Entity<Cart>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__Cart__3214EC27F2436962");

                entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Customer).WithOne(p => p.Cart)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Cart__Customer_I__59FA5E80");
            });

            modelBuilder.Entity<CartProduct>(entity =>
            {
                entity.HasKey(e => new { e.CartId, e.ProductCode }).HasName("PK__Cart_Pro__9C4AF075F8E58532");

                entity.HasOne(d => d.Cart).WithMany(p => p.CartProducts)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Cart_Prod__Cart___5CD6CB2B");

                entity.HasOne(d => d.ProductCodeNavigation).WithMany(p => p.CartProducts)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Cart_Prod__produ__5DCAEF64");
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__Category__3214EC2794A91D88");
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__Customer__3214EC27B75DE14A");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__Order__3214EC27C4A690F4");

                entity.Property(e => e.OrderDate).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Customer).WithMany(p => p.Orders)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Order__Customer___5070F446");

                entity.HasOne(d => d.ShippingAddress).WithMany(p => p.Orders)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Order__Shipping___5165187F");
            });

            modelBuilder.Entity<OrderProduct>(entity =>
            {
                entity.HasKey(e => new { e.OrderId, e.ProductId }).HasName("PK__Order_Pr__48672C2257690B3E");

                entity.HasOne(d => d.Order).WithMany(p => p.OrderProducts)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK__Order_Pro__Order__5441852A");

                entity.HasOne(d => d.Product).WithMany(p => p.OrderProducts)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Order_Pro__Produ__5535A963");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__Product__3214EC278B3FF41B");

                entity.Property(e => e.Discount).HasDefaultValue(0m);

                entity.HasOne(d => d.Seller).WithMany(p => p.Products)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Product__Seller___4222D4EF");

                entity.HasOne(d => d.SubCategory).WithMany(p => p.Products)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Product__Sub_Cat__412EB0B6");
            });

            modelBuilder.Entity<ProductImage>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__Product___3214EC27769A4C12");

                entity.Property(e => e.DisplayOrder).HasDefaultValue(0);
                entity.Property(e => e.IsPrimary).HasDefaultValue(false);

                entity.HasOne(d => d.Product).WithMany(p => p.ProductImages).HasConstraintName("FK__Product_I__Produ__46E78A0C");
            });

            modelBuilder.Entity<Return>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__Returns__3214EC274E77FA1B");

                entity.Property(e => e.ReturnDate).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Order).WithMany(p => p.Returns)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Returns__Order_I__6E01572D");

                entity.HasOne(d => d.Product).WithMany(p => p.Returns)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Returns__Product__6EF57B66");
            });

           

            modelBuilder.Entity<Review>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__Review__3214EC2760B66623");

                entity.Property(e => e.ReviewDate).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Customer).WithMany(p => p.Reviews)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Review__Customer__693CA210");

                entity.HasOne(d => d.Product).WithMany(p => p.Reviews)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Review__Product___6A30C649");
            });

            modelBuilder.Entity<Seller>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__Seller__3214EC272DFE5864");
            });

            modelBuilder.Entity<SubCategory>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__Sub_Cate__3214EC27EDCAACCF");

                entity.HasOne(d => d.Category).WithMany(p => p.SubCategories)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Sub_Categ__Categ__398D8EEE");
            });

            modelBuilder.Entity<Wishlist>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__Wishlist__3214EC27DFF4E54F");

                entity.HasOne(d => d.Customer).WithMany(p => p.Wishlists)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Wishlist__Custom__60A75C0F");

                entity.HasMany(d => d.Products).WithMany(p => p.Wishlists)
                    .UsingEntity<Dictionary<string, object>>(
                        "ProductWishlist",
                        r => r.HasOne<Product>().WithMany()
                            .HasForeignKey("ProductId")
                            .OnDelete(DeleteBehavior.ClientSetNull)
                            .HasConstraintName("FK__Product_W__Produ__6477ECF3"),
                        l => l.HasOne<Wishlist>().WithMany()
                            .HasForeignKey("WishlistId")
                            .OnDelete(DeleteBehavior.ClientSetNull)
                            .HasConstraintName("FK__Product_W__Wishl__6383C8BA"),
                        j =>
                        {
                            j.HasKey("WishlistId", "ProductId").HasName("PK__Product___7FD1083A4BA20C42");
                            j.ToTable("Product_Wishlist");
                            j.IndexerProperty<int>("WishlistId").HasColumnName("Wishlist_ID");
                            j.IndexerProperty<int>("ProductId").HasColumnName("Product_ID");
                        });
            });
        }


   

    }
  
        
    }



