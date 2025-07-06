using BeautyGuide.Data;
using BeautyGuide.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace BeautyGuide.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class DanhMucController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<DanhMucController> _logger;

        public DanhMucController(ApplicationDbContext context, ILogger<DanhMucController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Admin/DanhMuc
        public async Task<IActionResult> Index()
        {
            return View(await _context.DanhMucs.ToListAsync());
        }

        // GET: Admin/DanhMuc/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var danhMuc = await _context.DanhMucs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (danhMuc == null)
            {
                return NotFound();
            }

            return View(danhMuc);
        }

        // GET: Admin/DanhMuc/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/DanhMuc/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TenDanhMuc,MoTa,TrangThai")] DanhMuc danhMuc)
        {
            _logger.LogInformation("Create action called with DanhMuc: {@DanhMuc}", danhMuc);
            
            if (ModelState.IsValid)
            {
                _logger.LogInformation("ModelState is valid, adding DanhMuc to context");
                _context.Add(danhMuc);
                
                try
                {
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("DanhMuc saved successfully with ID: {Id}", danhMuc.Id);
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException ex)
                {
                    _logger.LogError(ex, "Error saving DanhMuc: {Message}", ex.Message);
                    ModelState.AddModelError("", "Không thể lưu danh mục. Vui lòng thử lại sau.");
                }
            }
            else
            {
                _logger.LogWarning("ModelState is invalid: {@ModelStateErrors}", ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage));
            }
            
            return View(danhMuc);
        }

        // GET: Admin/DanhMuc/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var danhMuc = await _context.DanhMucs.FindAsync(id);
            if (danhMuc == null)
            {
                return NotFound();
            }
            return View(danhMuc);
        }

        // POST: Admin/DanhMuc/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TenDanhMuc,MoTa,TrangThai")] DanhMuc danhMuc)
        {
            if (id != danhMuc.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(danhMuc);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DanhMucExists(danhMuc.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(danhMuc);
        }

        // GET: Admin/DanhMuc/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var danhMuc = await _context.DanhMucs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (danhMuc == null)
            {
                return NotFound();
            }

            return View(danhMuc);
        }

        // POST: Admin/DanhMuc/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var danhMuc = await _context.DanhMucs.FindAsync(id);
            
            // Kiểm tra xem danh mục có bài viết không
            var hasBaiViet = await _context.BaiViets.AnyAsync(b => b.DanhMucId == id);
            if (hasBaiViet)
            {
                ModelState.AddModelError(string.Empty, "Không thể xóa danh mục này vì có bài viết liên quan.");
                return View(danhMuc);
            }
            
            _context.DanhMucs.Remove(danhMuc);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DanhMucExists(int id)
        {
            return _context.DanhMucs.Any(e => e.Id == id);
        }
    }
} 