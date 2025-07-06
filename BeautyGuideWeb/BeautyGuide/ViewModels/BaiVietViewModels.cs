using BeautyGuide.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BeautyGuide.ViewModels
{
    public class BaiVietCreateViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Tiêu đề không được để trống")]
        [StringLength(200, ErrorMessage = "Tiêu đề không được vượt quá 200 ký tự")]
        [Display(Name = "Tiêu đề")]
        public string TieuDe { get; set; } = string.Empty;

        [Required(ErrorMessage = "Nội dung không được để trống")]
        [Display(Name = "Nội dung")]
        public string NoiDung { get; set; } = string.Empty;

        [Required(ErrorMessage = "Mô tả ngắn không được để trống")]
        [StringLength(500, ErrorMessage = "Mô tả ngắn không được vượt quá 500 ký tự")]
        [Display(Name = "Mô tả ngắn")]
        public string MoTaNgan { get; set; } = string.Empty;

        [Display(Name = "Ảnh bìa")]
        public IFormFile? AnhBiaFile { get; set; }

        [Required(ErrorMessage = "Danh mục không được để trống")]
        [Display(Name = "Danh mục")]
        public int DanhMucId { get; set; }

        [Display(Name = "Tags")]
        public List<int> TagIds { get; set; } = new List<int>();
    }
    
    public class BaiVietEditViewModel
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Tiêu đề không được để trống")]
        [StringLength(200, ErrorMessage = "Tiêu đề không được vượt quá 200 ký tự")]
        [Display(Name = "Tiêu đề")]
        public string TieuDe { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Nội dung không được để trống")]
        [Display(Name = "Nội dung")]
        public string NoiDung { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Mô tả ngắn không được để trống")]
        [StringLength(500, ErrorMessage = "Mô tả ngắn không được vượt quá 500 ký tự")]
        [Display(Name = "Mô tả ngắn")]
        public string MoTaNgan { get; set; } = string.Empty;
        
        [Display(Name = "Ảnh bìa hiện tại")]
        public string AnhBiaHienTai { get; set; } = string.Empty;
        
        [Display(Name = "Ảnh bìa mới")]
        public IFormFile? AnhBiaFile { get; set; }
        
        [Required(ErrorMessage = "Danh mục không được để trống")]
        [Display(Name = "Danh mục")]
        public int DanhMucId { get; set; }
        
        [Display(Name = "Tags")]
        public List<int> TagIds { get; set; } = new List<int>();
    }
    
    public class BaiVietDetailViewModel
    {
        public BaiViet BaiViet { get; set; } = new BaiViet();
        public BinhLuan BinhLuan { get; set; } = new BinhLuan();
        public List<BaiViet> BaiVietLienQuan { get; set; } = new List<BaiViet>();
    }
} 