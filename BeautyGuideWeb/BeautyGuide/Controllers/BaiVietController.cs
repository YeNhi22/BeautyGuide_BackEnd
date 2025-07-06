using BeautyGuide.Data;
using BeautyGuide.Helpers;
using BeautyGuide.Models;
using BeautyGuide.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BeautyGuide.Controllers
{
    public class BaiVietController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public BaiVietController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: BaiViet
        public async Task<IActionResult> Index(string searchString, int? danhMucId, int? tagId, int? page)
        {
            int pageSize = 6;
            int pageNumber = page ?? 1;

            var baiViets = _context.BaiViets
                .Include(b => b.NguoiDang)
                .Include(b => b.DanhMuc)
                .Where(b => b.TrangThai) // Chỉ hiển thị bài viết có trạng thái true
                .OrderByDescending(b => b.NgayDang)
                .AsQueryable();

            // Lọc theo từ khóa tìm kiếm
            if (!string.IsNullOrEmpty(searchString))
            {
                baiViets = baiViets.Where(b => b.TieuDe.Contains(searchString) || b.NoiDung.Contains(searchString));
            }

            // Lọc theo danh mục
            if (danhMucId.HasValue)
            {
                baiViets = baiViets.Where(b => b.DanhMucId == danhMucId.Value);
            }

            // Lọc theo tag
            if (tagId.HasValue)
            {
                baiViets = baiViets.Where(b => b.BaiVietTags.Any(bt => bt.TagId == tagId.Value));
            }

            ViewData["DanhMucs"] = await _context.DanhMucs.ToListAsync();
            ViewData["Tags"] = await _context.Tags.ToListAsync();
            ViewData["CurrentSearchString"] = searchString;
            ViewData["CurrentDanhMucId"] = danhMucId;
            ViewData["CurrentTagId"] = tagId;

            var model = await PaginatedList<BaiViet>.CreateAsync(baiViets, pageNumber, pageSize);

            return View(model);
        }

        // GET: BaiViet/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var baiViet = await _context.BaiViets
                .Include(b => b.NguoiDang)
                .Include(b => b.DanhMuc)
                .Include(b => b.BinhLuans)
                    .ThenInclude(c => c.NguoiBinhLuan)
                .Include(b => b.BaiVietTags)
                    .ThenInclude(bt => bt.Tag)
                .FirstOrDefaultAsync(m => m.Id == id && m.TrangThai); // Chỉ hiển thị bài viết có trạng thái true

            if (baiViet == null)
            {
                return NotFound();
            }

            // Tăng lượt xem
            baiViet.LuotXem += 1;
            await _context.SaveChangesAsync();

            // Lấy các bài viết liên quan cùng danh mục
            var relatedPosts = await _context.BaiViets
                .Where(b => b.DanhMucId == baiViet.DanhMucId && b.Id != baiViet.Id && b.TrangThai)
                .OrderByDescending(b => b.NgayDang)
                .Take(3)
                .ToListAsync();

            // Tạo view model
            var viewModel = new BaiVietDetailViewModel
            {
                BaiViet = baiViet,
                BinhLuan = new BinhLuan(),
                BaiVietLienQuan = relatedPosts
            };

            return View(viewModel);
        }

        // POST: BaiViet/Comment
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Comment(int baiVietId, string noiDung)
        {
            if (string.IsNullOrEmpty(noiDung))
            {
                TempData["ErrorMessage"] = "Nội dung bình luận không được để trống";
                return RedirectToAction(nameof(Details), new { id = baiVietId });
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            var binhLuan = new BinhLuan
            {
                BaiVietId = baiVietId,
                ApplicationUserId = user.Id,
                NoiDung = noiDung,
                NgayBinhLuan = DateTime.Now
            };

            _context.Add(binhLuan);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { id = baiVietId });
        }
    }
} 