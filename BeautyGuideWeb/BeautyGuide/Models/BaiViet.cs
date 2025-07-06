using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BeautyGuide.Models
{
    public class BaiViet
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [StringLength(200)]
        [Display(Name = "Tiêu đề")]
        public string TieuDe { get; set; } = string.Empty;
        
        [Required]
        [Display(Name = "Nội dung")]
        public string NoiDung { get; set; } = string.Empty;
        
        [Required]
        [StringLength(500)]
        [Display(Name = "Mô tả ngắn")]
        public string MoTaNgan { get; set; } = string.Empty;
        
        [Display(Name = "Ảnh bìa")]
        public string? AnhBia { get; set; }
        
        [Display(Name = "Lượt xem")]
        public int LuotXem { get; set; } = 0;
        
        [Required]
        [Display(Name = "Người đăng")]
        public string ApplicationUserId { get; set; } = string.Empty;
        
        [Display(Name = "Danh mục")]
        public int DanhMucId { get; set; }
        
        [Display(Name = "Người đăng")]
        public ApplicationUser NguoiDang { get; set; } = null!;
        
        [Display(Name = "Danh mục")]
        public DanhMuc DanhMuc { get; set; } = null!;
        
        public ICollection<BinhLuan> BinhLuans { get; set; } = new List<BinhLuan>();
        public ICollection<BaiVietTag> BaiVietTags { get; set; } = new List<BaiVietTag>();
        public ICollection<Tag> Tags { get; set; } = new List<Tag>();
        
        [Display(Name = "Ngày đăng")]
        public DateTime NgayDang { get; set; } = DateTime.Now;
        
        [Display(Name = "Ngày cập nhật")]
        public DateTime? NgayCapNhat { get; set; }
        
        [Display(Name = "Trạng thái")]
        public bool TrangThai { get; set; } = true;
    }
} 