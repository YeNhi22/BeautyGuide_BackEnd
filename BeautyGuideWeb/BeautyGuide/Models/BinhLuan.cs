using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BeautyGuide.Models
{
    public class BinhLuan
    {
        [Key]
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Nội dung bình luận không được để trống")]
        public string NoiDung { get; set; }
        
        public DateTime NgayBinhLuan { get; set; } = DateTime.Now;
        
        public bool TrangThai { get; set; } = true;
        
        // Foreign Keys
        public string ApplicationUserId { get; set; }
        
        public int BaiVietId { get; set; }
        
        // Navigation properties
        [ForeignKey("ApplicationUserId")]
        public virtual ApplicationUser NguoiBinhLuan { get; set; }
        
        [ForeignKey("BaiVietId")]
        public virtual BaiViet BaiViet { get; set; }
    }
} 