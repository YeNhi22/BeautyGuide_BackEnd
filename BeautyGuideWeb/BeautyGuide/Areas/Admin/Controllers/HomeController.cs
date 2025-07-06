using BeautyGuide.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace BeautyGuide.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // Thống kê số lượng
            ViewBag.TotalBaiViet = await _context.BaiViets.CountAsync();
            ViewBag.TotalDanhMuc = await _context.DanhMucs.CountAsync();
            ViewBag.TotalTag = await _context.Tags.CountAsync();
            ViewBag.TotalUser = await _context.Users.CountAsync();
            ViewBag.TotalBinhLuan = await _context.BinhLuans.CountAsync();

            // Lấy 5 bài viết mới nhất
            var recentBaiViets = await _context.BaiViets
                .Include(b => b.NguoiDang)
                .Include(b => b.DanhMuc)
                .OrderByDescending(b => b.NgayDang)
                .Take(5)
                .ToListAsync();

            return View(recentBaiViets);
        }
    }
} 