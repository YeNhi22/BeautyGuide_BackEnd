using BeautyGuide.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace BeautyGuide.ViewComponents
{
    public class DanhMucMenuViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public DanhMucMenuViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var danhMucs = await _context.DanhMucs
                .Where(d => d.TrangThai)
                .ToListAsync();
            
            return View(danhMucs);
        }
    }
} 