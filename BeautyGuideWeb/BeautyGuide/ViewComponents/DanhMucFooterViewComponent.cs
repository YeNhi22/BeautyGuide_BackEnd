using Microsoft.AspNetCore.Mvc;
using BeautyGuide.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;

namespace BeautyGuide.ViewComponents
{
    public class DanhMucFooterViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public DanhMucFooterViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var danhMucs = await _context.DanhMucs
                .OrderByDescending(d => d.BaiViets.Count)
                .Take(5)
                .ToListAsync();
            
            return View(danhMucs);
        }
    }
} 