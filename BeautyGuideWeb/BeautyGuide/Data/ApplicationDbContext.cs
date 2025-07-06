using BeautyGuide.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BeautyGuide.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<BaiViet> BaiViets { get; set; }
        public DbSet<DanhMuc> DanhMucs { get; set; }
        public DbSet<BinhLuan> BinhLuans { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<BaiVietTag> BaiVietTags { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Cấu hình khóa chính cho bảng trung gian BaiVietTag
            modelBuilder.Entity<BaiVietTag>()
                .HasKey(bt => bt.Id);

            // Cấu hình mối quan hệ giữa BaiVietTag và BaiViet
            modelBuilder.Entity<BaiVietTag>()
                .HasOne(bt => bt.BaiViet)
                .WithMany(b => b.BaiVietTags)
                .HasForeignKey(bt => bt.BaiVietId);

            // Cấu hình mối quan hệ giữa BaiVietTag và Tag
            modelBuilder.Entity<BaiVietTag>()
                .HasOne(bt => bt.Tag)
                .WithMany(t => t.BaiVietTags)
                .HasForeignKey(bt => bt.TagId);

            // Cấu hình mối quan hệ giữa BaiViet và ApplicationUser
            modelBuilder.Entity<BaiViet>()
                .HasOne(b => b.NguoiDang)
                .WithMany(u => u.BaiViets)
                .HasForeignKey(b => b.ApplicationUserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Cấu hình mối quan hệ giữa BinhLuan và ApplicationUser
            modelBuilder.Entity<BinhLuan>()
                .HasOne(c => c.NguoiBinhLuan)
                .WithMany(u => u.BinhLuans)
                .HasForeignKey(c => c.ApplicationUserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Cấu hình mối quan hệ giữa BinhLuan và BaiViet
            modelBuilder.Entity<BinhLuan>()
                .HasOne(c => c.BaiViet)
                .WithMany(b => b.BinhLuans)
                .HasForeignKey(c => c.BaiVietId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
} 