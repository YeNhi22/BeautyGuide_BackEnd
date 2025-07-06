using BeautyGuide.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BeautyGuide.Data
{
    public static class DbInitializer
    {
        public static async Task InitializeAsync(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            context.Database.EnsureCreated();

            // Kiểm tra nếu đã có dữ liệu
            if (context.Users.Any())
            {
                return;   // DB đã được khởi tạo
            }

            // Tạo roles
            var roles = new[] { "Admin", "User" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            // Tạo user admin
            var adminUser = new ApplicationUser
            {
                UserName = "admin@beautyguide.com",
                Email = "admin@beautyguide.com",
                HoTen = "Admin",
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(adminUser, "Admin@123");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }

            // Tạo user thường
            var normalUser = new ApplicationUser
            {
                UserName = "user@example.com",
                Email = "user@example.com",
                HoTen = "Người dùng",
                EmailConfirmed = true
            };

            result = await userManager.CreateAsync(normalUser, "User@123");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(normalUser, "User");
            }

            // Tạo danh mục
            var danhMucs = new DanhMuc[]
            {
                new DanhMuc { TenDanhMuc = "Chăm sóc da", MoTa = "Các bài viết về chăm sóc da", TrangThai = true },
                new DanhMuc { TenDanhMuc = "Trang điểm", MoTa = "Các bài viết về trang điểm", TrangThai = true },
                new DanhMuc { TenDanhMuc = "Chăm sóc tóc", MoTa = "Các bài viết về chăm sóc tóc", TrangThai = true },
                new DanhMuc { TenDanhMuc = "Làm đẹp tự nhiên", MoTa = "Các bài viết về làm đẹp tự nhiên", TrangThai = true }
            };

            await context.DanhMucs.AddRangeAsync(danhMucs);
            await context.SaveChangesAsync();

            // Tạo tags
            var tags = new Tag[]
            {
                new Tag { TenTag = "Skincare" },
                new Tag { TenTag = "Makeup" },
                new Tag { TenTag = "Haircare" },
                new Tag { TenTag = "Natural" },
                new Tag { TenTag = "Mặt nạ" },
                new Tag { TenTag = "Dưỡng ẩm" },
                new Tag { TenTag = "Son môi" },
                new Tag { TenTag = "Chống nắng" }
            };

            await context.Tags.AddRangeAsync(tags);
            await context.SaveChangesAsync();

            // Tạo bài viết mẫu
            var baiViets = new BaiViet[]
            {
                new BaiViet
                {
                    TieuDe = "Cách chăm sóc da mặt hàng ngày",
                    NoiDung = "<p>Chăm sóc da mặt hàng ngày là một phần quan trọng trong việc duy trì làn da khỏe mạnh và rạng rỡ. Dưới đây là các bước cơ bản để chăm sóc da mặt hàng ngày:</p><h3>Bước 1: Làm sạch</h3><p>Sử dụng sữa rửa mặt phù hợp với loại da của bạn để loại bỏ bụi bẩn, dầu thừa và lớp trang điểm. Rửa mặt vào buổi sáng và buổi tối.</p><h3>Bước 2: Toner</h3><p>Sử dụng toner để cân bằng độ pH của da và làm se lỗ chân lông.</p><h3>Bước 3: Serum</h3><p>Sử dụng serum chứa các thành phần hoạt tính như vitamin C, hyaluronic acid hoặc retinol tùy thuộc vào nhu cầu của da.</p><h3>Bước 4: Kem dưỡng ẩm</h3><p>Sử dụng kem dưỡng ẩm phù hợp với loại da để giữ ẩm và bảo vệ da.</p><h3>Bước 5: Kem chống nắng (buổi sáng)</h3><p>Sử dụng kem chống nắng có SPF ít nhất 30 để bảo vệ da khỏi tác hại của tia UV.</p>",
                    MoTaNgan = "Hướng dẫn chi tiết các bước chăm sóc da mặt hàng ngày để có làn da khỏe mạnh và rạng rỡ.",
                    NgayDang = DateTime.Now.AddDays(-10),
                    LuotXem = 150,
                    TrangThai = true,
                    ApplicationUserId = adminUser.Id,
                    DanhMucId = danhMucs[0].Id
                },
                new BaiViet
                {
                    TieuDe = "Các bước trang điểm cơ bản cho người mới bắt đầu",
                    NoiDung = "<p>Trang điểm có thể là một kỹ năng khó khăn đối với người mới bắt đầu. Dưới đây là hướng dẫn từng bước để tạo nên một lớp trang điểm cơ bản:</p><h3>Bước 1: Kem lót</h3><p>Sử dụng kem lót để tạo nền và giúp lớp trang điểm bền màu hơn.</p><h3>Bước 2: Kem nền</h3><p>Sử dụng kem nền phù hợp với tông da để tạo nền đều màu.</p><h3>Bước 3: Che khuyết điểm</h3><p>Sử dụng concealer để che các khuyết điểm như quầng thâm, mụn.</p><h3>Bước 4: Phấn phủ</h3><p>Sử dụng phấn phủ để cố định lớp trang điểm và giảm độ bóng.</p><h3>Bước 5: Phấn má hồng</h3><p>Sử dụng phấn má hồng để tạo sắc hồng tự nhiên cho gương mặt.</p><h3>Bước 6: Trang điểm mắt</h3><p>Sử dụng phấn mắt, kẻ mắt và mascara để làm nổi bật đôi mắt.</p><h3>Bước 7: Son môi</h3><p>Hoàn thiện với son môi màu phù hợp.</p>",
                    MoTaNgan = "Hướng dẫn chi tiết các bước trang điểm cơ bản dành cho người mới bắt đầu.",
                    NgayDang = DateTime.Now.AddDays(-7),
                    LuotXem = 120,
                    TrangThai = true,
                    ApplicationUserId = normalUser.Id,
                    DanhMucId = danhMucs[1].Id
                },
                new BaiViet
                {
                    TieuDe = "Cách chăm sóc tóc khô và hư tổn",
                    NoiDung = "<p>Tóc khô và hư tổn là vấn đề phổ biến mà nhiều người gặp phải. Dưới đây là một số cách để chăm sóc và phục hồi tóc khô và hư tổn:</p><h3>1. Sử dụng dầu gội và dầu xả dành cho tóc khô</h3><p>Chọn các sản phẩm dành riêng cho tóc khô và hư tổn, chứa các thành phần dưỡng ẩm như argan oil, keratin, hoặc protein.</p><h3>2. Hạn chế sử dụng nhiệt</h3><p>Giảm thiểu việc sử dụng máy sấy, máy duỗi, và máy uốn tóc. Nếu cần sử dụng, hãy dùng sản phẩm bảo vệ nhiệt trước khi tạo kiểu.</p><h3>3. Đắp mặt nạ dưỡng tóc</h3><p>Sử dụng mặt nạ dưỡng tóc 1-2 lần mỗi tuần để cung cấp độ ẩm và dưỡng chất cho tóc.</p><h3>4. Cắt tỉa thường xuyên</h3><p>Cắt tỉa ngọn tóc thường xuyên để loại bỏ phần tóc chẻ ngọn và hư tổn.</p><h3>5. Sử dụng dầu dưỡng tóc</h3><p>Thoa dầu dưỡng tóc như dầu dừa, dầu argan, hoặc dầu jojoba lên tóc để cung cấp độ ẩm và làm mềm tóc.</p><h3>6. Tránh gội đầu quá thường xuyên</h3><p>Gội đầu quá thường xuyên có thể làm mất đi dầu tự nhiên của tóc. Hãy gội đầu 2-3 lần mỗi tuần.</p>",
                    MoTaNgan = "Hướng dẫn chi tiết cách chăm sóc và phục hồi tóc khô và hư tổn.",
                    NgayDang = DateTime.Now.AddDays(-5),
                    LuotXem = 90,
                    TrangThai = true,
                    ApplicationUserId = adminUser.Id,
                    DanhMucId = danhMucs[2].Id
                },
                new BaiViet
                {
                    TieuDe = "Các mặt nạ tự nhiên cho da mụn",
                    NoiDung = "<p>Da mụn có thể được cải thiện bằng các mặt nạ tự nhiên làm từ các nguyên liệu có sẵn trong nhà bếp. Dưới đây là một số công thức mặt nạ tự nhiên hiệu quả cho da mụn:</p><h3>1. Mặt nạ mật ong và quế</h3><p><strong>Nguyên liệu:</strong></p><ul><li>2 thìa mật ong</li><li>1/2 thìa bột quế</li></ul><p><strong>Cách làm:</strong></p><ol><li>Trộn mật ong và bột quế thành hỗn hợp sệt.</li><li>Thoa lên mặt và để trong 10-15 phút.</li><li>Rửa sạch với nước ấm.</li></ol><p>Mật ong có tính kháng khuẩn tự nhiên, trong khi quế giúp kích thích lưu thông máu và giảm viêm.</p><h3>2. Mặt nạ trà xanh</h3><p><strong>Nguyên liệu:</strong></p><ul><li>2 thìa bột trà xanh</li><li>Nước ấm vừa đủ để tạo hỗn hợp sệt</li></ul><p><strong>Cách làm:</strong></p><ol><li>Trộn bột trà xanh với nước ấm để tạo thành hỗn hợp sệt.</li><li>Thoa lên mặt và để trong 15-20 phút.</li><li>Rửa sạch với nước ấm.</li></ol><p>Trà xanh giàu chất chống oxy hóa và có tính kháng khuẩn, giúp giảm viêm và làm dịu da mụn.</p><h3>3. Mặt nạ nghệ và sữa chua</h3><p><strong>Nguyên liệu:</strong></p><ul><li>1 thìa bột nghệ</li><li>2 thìa sữa chua không đường</li></ul><p><strong>Cách làm:</strong></p><ol><li>Trộn bột nghệ và sữa chua thành hỗn hợp đồng nhất.</li><li>Thoa lên mặt và để trong 15-20 phút.</li><li>Rửa sạch với nước ấm.</li></ol><p>Nghệ có tính kháng khuẩn và chống viêm, trong khi sữa chua chứa axit lactic giúp làm sạch và làm mềm da.</p>",
                    MoTaNgan = "Hướng dẫn làm các loại mặt nạ tự nhiên hiệu quả cho da mụn từ các nguyên liệu có sẵn trong nhà bếp.",
                    NgayDang = DateTime.Now.AddDays(-3),
                    LuotXem = 80,
                    TrangThai = true,
                    ApplicationUserId = normalUser.Id,
                    DanhMucId = danhMucs[3].Id
                }
            };

            await context.BaiViets.AddRangeAsync(baiViets);
            await context.SaveChangesAsync();

            // Tạo liên kết bài viết - tag
            var baiVietTags = new BaiVietTag[]
            {
                new BaiVietTag { BaiVietId = baiViets[0].Id, TagId = tags[0].Id },
                new BaiVietTag { BaiVietId = baiViets[0].Id, TagId = tags[5].Id },
                new BaiVietTag { BaiVietId = baiViets[0].Id, TagId = tags[7].Id },
                new BaiVietTag { BaiVietId = baiViets[1].Id, TagId = tags[1].Id },
                new BaiVietTag { BaiVietId = baiViets[1].Id, TagId = tags[6].Id },
                new BaiVietTag { BaiVietId = baiViets[2].Id, TagId = tags[2].Id },
                new BaiVietTag { BaiVietId = baiViets[3].Id, TagId = tags[3].Id },
                new BaiVietTag { BaiVietId = baiViets[3].Id, TagId = tags[4].Id }
            };

            await context.BaiVietTags.AddRangeAsync(baiVietTags);
            await context.SaveChangesAsync();

            // Tạo bình luận
            var binhLuans = new BinhLuan[]
            {
                new BinhLuan
                {
                    NoiDung = "Bài viết rất hữu ích, cảm ơn tác giả!",
                    NgayBinhLuan = DateTime.Now.AddDays(-9),
                    TrangThai = true,
                    ApplicationUserId = normalUser.Id,
                    BaiVietId = baiViets[0].Id
                },
                new BinhLuan
                {
                    NoiDung = "Tôi đã thử phương pháp này và thấy hiệu quả rõ rệt.",
                    NgayBinhLuan = DateTime.Now.AddDays(-8),
                    TrangThai = true,
                    ApplicationUserId = adminUser.Id,
                    BaiVietId = baiViets[0].Id
                },
                new BinhLuan
                {
                    NoiDung = "Hướng dẫn rất chi tiết và dễ hiểu.",
                    NgayBinhLuan = DateTime.Now.AddDays(-6),
                    TrangThai = true,
                    ApplicationUserId = normalUser.Id,
                    BaiVietId = baiViets[1].Id
                },
                new BinhLuan
                {
                    NoiDung = "Tôi sẽ thử áp dụng những phương pháp này.",
                    NgayBinhLuan = DateTime.Now.AddDays(-4),
                    TrangThai = true,
                    ApplicationUserId = adminUser.Id,
                    BaiVietId = baiViets[2].Id
                },
                new BinhLuan
                {
                    NoiDung = "Mặt nạ trà xanh thực sự hiệu quả!",
                    NgayBinhLuan = DateTime.Now.AddDays(-2),
                    TrangThai = true,
                    ApplicationUserId = normalUser.Id,
                    BaiVietId = baiViets[3].Id
                }
            };

            await context.BinhLuans.AddRangeAsync(binhLuans);
            await context.SaveChangesAsync();
        }
    }
} 