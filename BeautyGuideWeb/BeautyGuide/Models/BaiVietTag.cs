using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BeautyGuide.Models
{
    public class BaiVietTag
    {
        [Key]
        public int Id { get; set; }
        
        public int BaiVietId { get; set; }
        
        public int TagId { get; set; }
        
        // Navigation properties
        [ForeignKey("BaiVietId")]
        public virtual BaiViet BaiViet { get; set; }
        
        [ForeignKey("TagId")]
        public virtual Tag Tag { get; set; }
    }
} 