using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BeautyGuide.Models
{
    public class Tag
    {
        [Key]
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Tên tag không được để trống")]
        [StringLength(50, ErrorMessage = "Tên tag không được vượt quá 50 ký tự")]
        public string TenTag { get; set; }
        
        // Navigation properties
        public virtual ICollection<BaiViet> BaiViets { get; set; } = new List<BaiViet>();
        public virtual ICollection<BaiVietTag> BaiVietTags { get; set; } = new List<BaiVietTag>();
    }
} 