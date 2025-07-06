using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace BeautyGuide.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string HoTen { get; set; } = string.Empty;
        
        public string? AnhDaiDien { get; set; }
        
        public DateTime NgayTao { get; set; } = DateTime.Now;
        
        public DateTime NgaySinh { get; set; } = new DateTime(2000, 1, 1);
        
        public ICollection<BaiViet> BaiViets { get; set; } = new List<BaiViet>();
        public ICollection<BinhLuan> BinhLuans { get; set; } = new List<BinhLuan>();
    }
} 