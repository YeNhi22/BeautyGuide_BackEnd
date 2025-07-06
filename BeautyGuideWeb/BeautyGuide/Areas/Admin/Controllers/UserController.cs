using BeautyGuide.Data;
using BeautyGuide.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace BeautyGuide.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // GET: Admin/User
        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.ToListAsync();
            return View(users);
        }

        // GET: Admin/User/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            // Lấy danh sách role của user
            var userRoles = await _userManager.GetRolesAsync(user);
            ViewData["UserRoles"] = userRoles;

            return View(user);
        }

        // GET: Admin/User/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            // Lấy danh sách tất cả các role
            var roles = await _roleManager.Roles.ToListAsync();
            ViewData["Roles"] = roles;

            // Lấy danh sách role của user
            var userRoles = await _userManager.GetRolesAsync(user);
            ViewData["UserRoles"] = userRoles;

            return View(user);
        }

        // POST: Admin/User/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,HoTen,Email,PhoneNumber")] ApplicationUser userModel, string[] selectedRoles)
        {
            if (id != userModel.Id)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Cập nhật thông tin cơ bản
                    user.HoTen = userModel.HoTen;
                    user.PhoneNumber = userModel.PhoneNumber;
                    
                    var result = await _userManager.UpdateAsync(user);
                    if (!result.Succeeded)
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                        return View(user);
                    }

                    // Cập nhật roles
                    var userRoles = await _userManager.GetRolesAsync(user);
                    
                    // Xóa tất cả role hiện tại
                    result = await _userManager.RemoveFromRolesAsync(user, userRoles);
                    if (!result.Succeeded)
                    {
                        ModelState.AddModelError(string.Empty, "Không thể xóa vai trò hiện tại.");
                        return View(user);
                    }

                    // Thêm các role mới được chọn
                    if (selectedRoles != null && selectedRoles.Length > 0)
                    {
                        result = await _userManager.AddToRolesAsync(user, selectedRoles);
                        if (!result.Succeeded)
                        {
                            ModelState.AddModelError(string.Empty, "Không thể thêm vai trò mới.");
                            return View(user);
                        }
                    }

                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await UserExists(user.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            // Nếu có lỗi, chuẩn bị dữ liệu để hiển thị lại form
            var roles = await _roleManager.Roles.ToListAsync();
            ViewData["Roles"] = roles;
            ViewData["UserRoles"] = await _userManager.GetRolesAsync(user);
            
            return View(user);
        }

        // GET: Admin/User/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            // Không cho phép xóa tài khoản Admin chính
            if (await _userManager.IsInRoleAsync(user, "Admin") && user.Email == "admin@beautyguide.com")
            {
                ModelState.AddModelError(string.Empty, "Không thể xóa tài khoản Admin chính.");
                return View(user);
            }

            return View(user);
        }

        // POST: Admin/User/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            
            // Không cho phép xóa tài khoản Admin chính
            if (await _userManager.IsInRoleAsync(user, "Admin") && user.Email == "admin@beautyguide.com")
            {
                ModelState.AddModelError(string.Empty, "Không thể xóa tài khoản Admin chính.");
                return View(user);
            }

            try
            {
                // Xóa tất cả bình luận của người dùng trước
                var userComments = _context.BinhLuans.Where(b => b.ApplicationUserId == id);
                _context.BinhLuans.RemoveRange(userComments);
                
                // Xóa tất cả bài viết của người dùng
                var userPosts = _context.BaiViets.Where(b => b.ApplicationUserId == id).ToList();
                foreach (var post in userPosts)
                {
                    // Trước tiên, xóa các bình luận liên quan đến bài viết
                    var postComments = _context.BinhLuans.Where(c => c.BaiVietId == post.Id);
                    _context.BinhLuans.RemoveRange(postComments);
                    
                    // Xóa các liên kết BaiVietTag
                    var postTags = _context.Set<BaiVietTag>().Where(bt => bt.BaiVietId == post.Id);
                    _context.Set<BaiVietTag>().RemoveRange(postTags);
                    
                    // Sau đó xóa bài viết
                    _context.BaiViets.Remove(post);
                }
                
                // Lưu các thay đổi vào cơ sở dữ liệu
                await _context.SaveChangesAsync();
                
                // Sau đó xóa người dùng
                var result = await _userManager.DeleteAsync(user);
                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return View(user);
                }
                
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                // Xử lý lỗi và hiển thị thông báo lỗi
                ModelState.AddModelError(string.Empty, "Không thể xóa người dùng này. Chi tiết lỗi: " + ex.Message);
                return View(user);
            }
        }

        // Phương thức kiểm tra user có tồn tại không
        private async Task<bool> UserExists(string id)
        {
            return await _userManager.FindByIdAsync(id) != null;
        }
    }
} 