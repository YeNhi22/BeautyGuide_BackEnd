using System.Diagnostics;
using BeautyGuide.Data;
using BeautyGuide.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace BeautyGuide.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // Lấy danh mục
            var danhMucs = await _context.DanhMucs
                .Where(d => d.TrangThai)
                .ToListAsync();
            
            // Lấy bài viết mới nhất
            var baiVietMoiNhat = await _context.BaiViets
                .Include(b => b.NguoiDang)
                .Include(b => b.DanhMuc)
                .Where(b => b.TrangThai)
                .OrderByDescending(b => b.NgayDang)
                .Take(6)
                .ToListAsync();
            
            // Lấy bài viết nổi bật (có lượt xem cao nhất)
            var baiVietNoiBat = await _context.BaiViets
                .Include(b => b.NguoiDang)
                .Include(b => b.DanhMuc)
                .Where(b => b.TrangThai)
                .OrderByDescending(b => b.LuotXem)
                .Take(4)
                .ToListAsync();
            
            ViewData["DanhMucs"] = danhMucs;
            ViewData["BaiVietMoiNhat"] = baiVietMoiNhat;
            ViewData["BaiVietNoiBat"] = baiVietNoiBat;
            
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
