using BeautyGuide.Data;
using BeautyGuide.Helpers;
using BeautyGuide.Models;
using BeautyGuide.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BeautyGuide.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class BaiVietController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public BaiVietController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Admin/BaiViet
        public async Task<IActionResult> Index(int? page)
        {
            int pageSize = 10;
            int pageNumber = page ?? 1;

            var baiViets = _context.BaiViets
                .Include(b => b.NguoiDang)
                .Include(b => b.DanhMuc)
                .OrderByDescending(b => b.NgayDang)
                .AsQueryable();

            var model = await PaginatedList<BaiViet>.CreateAsync(baiViets, pageNumber, pageSize);
            
            return View(model);
        }

        // GET: Admin/BaiViet/Details/5
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
                .FirstOrDefaultAsync(m => m.Id == id);

            if (baiViet == null)
            {
                return NotFound();
            }

            return View(baiViet);
        }

        // GET: Admin/BaiViet/Create
        public async Task<IActionResult> Create()
        {
            ViewData["DanhMucs"] = await _context.DanhMucs.ToListAsync();
            ViewData["Tags"] = await _context.Tags.ToListAsync();
            return View();
        }

        // POST: Admin/BaiViet/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BaiVietCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return NotFound();
                }

                var baiViet = new BaiViet
                {
                    TieuDe = model.TieuDe,
                    NoiDung = model.NoiDung,
                    MoTaNgan = model.MoTaNgan,
                    DanhMucId = model.DanhMucId,
                    ApplicationUserId = user.Id,
                    NgayDang = DateTime.Now,
                    TrangThai = true
                };

                // Xử lý upload ảnh bìa nếu có
                if (model.AnhBiaFile != null && model.AnhBiaFile.Length > 0)
                {
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "baiviet");
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + model.AnhBiaFile.FileName;
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.AnhBiaFile.CopyToAsync(fileStream);
                    }

                    baiViet.AnhBia = "/images/baiviet/" + uniqueFileName;
                }

                _context.Add(baiViet);
                await _context.SaveChangesAsync();

                // Xử lý tags nếu có
                if (model.TagIds != null && model.TagIds.Any())
                {
                    foreach (var tagId in model.TagIds)
                    {
                        var baiVietTag = new BaiVietTag
                        {
                            BaiVietId = baiViet.Id,
                            TagId = tagId
                        };
                        _context.Add(baiVietTag);
                    }
                    await _context.SaveChangesAsync();
                }

                return RedirectToAction(nameof(Index));
            }

            ViewData["DanhMucs"] = await _context.DanhMucs.ToListAsync();
            ViewData["Tags"] = await _context.Tags.ToListAsync();
            return View(model);
        }

        // GET: Admin/BaiViet/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var baiViet = await _context.BaiViets.FindAsync(id);
            if (baiViet == null)
            {
                return NotFound();
            }

            var selectedTagIds = await _context.BaiVietTags
                .Where(bt => bt.BaiVietId == id)
                .Select(bt => bt.TagId)
                .ToListAsync();

            var model = new BaiVietEditViewModel
            {
                Id = baiViet.Id,
                TieuDe = baiViet.TieuDe,
                NoiDung = baiViet.NoiDung,
                MoTaNgan = baiViet.MoTaNgan,
                DanhMucId = baiViet.DanhMucId,
                AnhBiaHienTai = baiViet.AnhBia,
                TagIds = selectedTagIds
            };

            ViewData["DanhMucs"] = await _context.DanhMucs.ToListAsync();
            ViewData["Tags"] = await _context.Tags.ToListAsync();

            return View(model);
        }

        // POST: Admin/BaiViet/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, BaiVietEditViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var baiViet = await _context.BaiViets.FindAsync(id);
                    if (baiViet == null)
                    {
                        return NotFound();
                    }

                    baiViet.TieuDe = model.TieuDe;
                    baiViet.NoiDung = model.NoiDung;
                    baiViet.MoTaNgan = model.MoTaNgan;
                    baiViet.DanhMucId = model.DanhMucId;
                    baiViet.NgayCapNhat = DateTime.Now;

                    // Xử lý upload ảnh bìa mới nếu có
                    if (model.AnhBiaFile != null && model.AnhBiaFile.Length > 0)
                    {
                        var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "baiviet");
                        if (!Directory.Exists(uploadsFolder))
                        {
                            Directory.CreateDirectory(uploadsFolder);
                        }

                        var uniqueFileName = Guid.NewGuid().ToString() + "_" + model.AnhBiaFile.FileName;
                        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await model.AnhBiaFile.CopyToAsync(fileStream);
                        }

                        baiViet.AnhBia = "/images/baiviet/" + uniqueFileName;
                    }

                    _context.Update(baiViet);

                    // Cập nhật tags
                    var existingTags = await _context.BaiVietTags
                        .Where(bt => bt.BaiVietId == id)
                        .ToListAsync();

                    _context.BaiVietTags.RemoveRange(existingTags);

                    if (model.TagIds != null && model.TagIds.Any())
                    {
                        foreach (var tagId in model.TagIds)
                        {
                            var baiVietTag = new BaiVietTag
                            {
                                BaiVietId = baiViet.Id,
                                TagId = tagId
                            };
                            _context.Add(baiVietTag);
                        }
                    }

                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BaiVietExists(model.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            ViewData["DanhMucs"] = await _context.DanhMucs.ToListAsync();
            ViewData["Tags"] = await _context.Tags.ToListAsync();
            return View(model);
        }

        // GET: Admin/BaiViet/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var baiViet = await _context.BaiViets
                .Include(b => b.NguoiDang)
                .Include(b => b.DanhMuc)
                .FirstOrDefaultAsync(m => m.Id == id);
                
            if (baiViet == null)
            {
                return NotFound();
            }

            return View(baiViet);
        }

        // POST: Admin/BaiViet/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var baiViet = await _context.BaiViets.FindAsync(id);
            if (baiViet == null)
            {
                return NotFound();
            }

            // Xóa các liên kết với tag
            var baiVietTags = await _context.BaiVietTags.Where(bt => bt.BaiVietId == id).ToListAsync();
            _context.BaiVietTags.RemoveRange(baiVietTags);

            // Xóa các bình luận
            var binhLuans = await _context.BinhLuans.Where(c => c.BaiVietId == id).ToListAsync();
            _context.BinhLuans.RemoveRange(binhLuans);

            // Xóa bài viết
            _context.BaiViets.Remove(baiViet);
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // POST: Admin/BaiViet/ToggleStatus/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleStatus(int id)
        {
            var baiViet = await _context.BaiViets.FindAsync(id);
            if (baiViet == null)
            {
                return NotFound();
            }

            // Đảo trạng thái
            baiViet.TrangThai = !baiViet.TrangThai;
            _context.Update(baiViet);
            await _context.SaveChangesAsync();
            
            return RedirectToAction(nameof(Index));
        }

        private bool BaiVietExists(int id)
        {
            return _context.BaiViets.Any(e => e.Id == id);
        }
    }
} 