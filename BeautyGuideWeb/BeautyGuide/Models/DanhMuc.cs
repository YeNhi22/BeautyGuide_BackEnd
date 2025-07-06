using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BeautyGuide.Models
{
    public class DanhMuc
    {
        [Key]
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Tên danh mục không được để trống")]
        [StringLength(100, ErrorMessage = "Tên danh mục không được vượt quá 100 ký tự")]
        public string TenDanhMuc { get; set; }
        
        [StringLength(200)]
        public string MoTa { get; set; }
        
        public bool TrangThai { get; set; } = true;
        
        // Navigation properties
        public virtual ICollection<BaiViet> BaiViets { get; set; }
    }
} 