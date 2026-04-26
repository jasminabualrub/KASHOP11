using KASHOP11.DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP11.DAL.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DbSet <Category> categories { set; get; }
        public DbSet<CategoryTranslation> categoryTranslations { set; get; }
        public DbSet<Product> Products { set; get; }
        public DbSet<ProductTranslation> ProductTranslations { set; get; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<BrandTranslation> BrandTranslations { get; set; }
        public DbSet<Cart> Carts { set; get; }
        public DbSet<Order> Orders { set; get; }
        public DbSet<OrderItem> OrderItems { set; get; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options,IHttpContextAccessor httpContextAccessor)
            : base(options)
        {
            _httpContextAccessor= httpContextAccessor;
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ApplicationUser>().ToTable("Users");
            modelBuilder.Entity<IdentityRole>().ToTable("Roles");
            modelBuilder.Entity<IdentityUserRole<string>>().ToTable("UserRoles");
       //     modelBuilder.Entity<Category>()
       ////.HasOne(c => c.createdBy)
       ////.WithMany(u => u.Categories)
       ////.HasForeignKey(c => c.createdById)
       ////.OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Category>()
                .HasOne(p=>p.createdBy)
                .WithMany()
                .HasForeignKey(p=>p.createdById)
                .OnDelete(DeleteBehavior.Restrict)
                ;
            modelBuilder.Entity<Category>().HasOne(p => p.updatedBy)
               .WithMany()
               .HasForeignKey(p => p.updatedById)
               .OnDelete(DeleteBehavior.Restrict)
               ;
            modelBuilder.Entity<Product>()
               .HasOne(p => p.createdBy)
               .WithMany()
               .HasForeignKey(p => p.createdById)
               .OnDelete(DeleteBehavior.Restrict)
               ;
            modelBuilder.Entity<Product>().HasOne(p => p.updatedBy)
               .WithMany()
               .HasForeignKey(p => p.updatedById)
               .OnDelete(DeleteBehavior.Restrict)
               ;
            modelBuilder.Entity<Product>()
    .HasOne(p => p.Brand)
    .WithMany(b => b.Products)
    .HasForeignKey(p => p.BrandId)
    .OnDelete(DeleteBehavior.Restrict);

        }
        

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        { if(_httpContextAccessor.HttpContext != null)
            {
                var entries = ChangeTracker.Entries<AuditableEntity>();
                var currentUserId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                foreach (var entry in entries)
                {
                    if (entry.State == EntityState.Added)
                    {
                        entry.Property(x => x.createdById).CurrentValue = currentUserId;
                        entry.Property(x => x.createdOn).CurrentValue = DateTime.UtcNow;
                    }
                    if (entry.State == EntityState.Modified)
                    {
                        entry.Property(x => x.updatedById).CurrentValue = currentUserId;
                        entry.Property(x => x.updatedOn).CurrentValue = DateTime.UtcNow;

                    }
                }
            }
            
           
            return base.SaveChangesAsync(cancellationToken);
        }
        
    }
}
