using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PharmaSphere.Data;
using PharmaSphere.Models;

namespace PharmaSphere
{
    public static class DbInitializer
    {
        public static void Seed(IServiceProvider serviceProvider)
        {
            using (var context = new PharmaContext(
                serviceProvider.GetRequiredService<DbContextOptions<PharmaContext>>()))
            {
                // Ensure database is created
                context.Database.EnsureCreated();

                // Seed Customers
                if (!context.Users.Any(u => u.Role == UserRole.Customer))
                {
                    context.Users.AddRange(
                        new User { Username = "nguyenvanan", PasswordHash = "123456", FullName = "Nguyễn Văn An", Email = "an.nguyen@gmail.com", Phone = "0901234567", Role = UserRole.Customer },
                        new User { Username = "lethibinh", PasswordHash = "123456", FullName = "Lê Thị Bình", Email = "binh.le@yahoo.com", Phone = "0912345678", Role = UserRole.Customer },
                        new User { Username = "tranvanhung", PasswordHash = "123456", FullName = "Trần Văn Hùng", Email = "hung.tran@hotmail.com", Phone = "0923456789", Role = UserRole.Customer },
                        new User { Username = "phamthidieu", PasswordHash = "123456", FullName = "Phạm Thị Diệu", Email = "dieu.pham@outlook.com", Phone = "0934567890", Role = UserRole.Customer },
                        new User { Username = "hoangvanhiep", PasswordHash = "123456", FullName = "Hoàng Văn Hiệp", Email = "hiep.hoang@gmail.com", Phone = "0945678901", Role = UserRole.Customer },
                        new User { Username = "vuthithanh", PasswordHash = "123456", FullName = "Vũ Thị Thanh", Email = "thanh.vu@gmail.com", Phone = "0956789012", Role = UserRole.Customer },
                        new User { Username = "dangvandung", PasswordHash = "123456", FullName = "Đặng Văn Dũng", Email = "dung.dang@gmail.com", Phone = "0967890123", Role = UserRole.Customer },
                        new User { Username = "buithilan", PasswordHash = "123456", FullName = "Bùi Thị Lan", Email = "lan.bui@gmail.com", Phone = "0978901234", Role = UserRole.Customer },
                        new User { Username = "ngothidiem", PasswordHash = "123456", FullName = "Ngô Thị Diễm", Email = "diem.ngo@gmail.com", Phone = "0989012345", Role = UserRole.Customer },
                        new User { Username = "duongvanphuc", PasswordHash = "123456", FullName = "Dương Văn Phúc", Email = "phuc.duong@gmail.com", Phone = "0990123456", Role = UserRole.Customer },
                        new User { Username = "lythihuong", PasswordHash = "123456", FullName = "Lý Thị Hương", Email = "huong.ly@gmail.com", Phone = "0812345678", Role = UserRole.Customer },
                        new User { Username = "truongvanminh", PasswordHash = "123456", FullName = "Trương Văn Minh", Email = "minh.truong@gmail.com", Phone = "0823456789", Role = UserRole.Customer },
                        new User { Username = "dothimai", PasswordHash = "123456", FullName = "Đỗ Thị Mai", Email = "mai.do@gmail.com", Phone = "0834567890", Role = UserRole.Customer },
                        new User { Username = "phanvanthanh", PasswordHash = "123456", FullName = "Phan Văn Thành", Email = "thanh.phan@gmail.com", Phone = "0845678901", Role = UserRole.Customer },
                        new User { Username = "haothidat", PasswordHash = "123456", FullName = "Hào Thị Đạt", Email = "dat.hao@gmail.com", Phone = "0856789012", Role = UserRole.Customer },
                        new User { Username = "tonvanquy", PasswordHash = "123456", FullName = "Tôn Văn Quý", Email = "quy.ton@gmail.com", Phone = "0867890123", Role = UserRole.Customer },
                        new User { Username = "giangthituyet", PasswordHash = "123456", FullName = "Giang Thị Tuyết", Email = "tuyet.giang@gmail.com", Phone = "0878901234", Role = UserRole.Customer },
                        new User { Username = "diepvanphat", PasswordHash = "123456", FullName = "Diệp Văn Phát", Email = "phat.diep@gmail.com", Phone = "0889012345", Role = UserRole.Customer },
                        new User { Username = "canthilieu", PasswordHash = "123456", FullName = "Cấn Thị Liễu", Email = "lieu.can@gmail.com", Phone = "0890123456", Role = UserRole.Customer },
                        new User { Username = "vuongvanlong", PasswordHash = "123456", FullName = "Vương Văn Long", Email = "long.vuong@gmail.com", Phone = "0701234567", Role = UserRole.Customer }
                    );
                }

                // Seed Staff
                if (!context.Users.Any(u => u.Role == UserRole.Staff))
                {
                    context.Users.AddRange(
                        new User { Username = "staff.nguyenvana", PasswordHash = "123456", FullName = "Nhân viên Nguyễn Văn A", Email = "staff.a@pharmasphere.com", Phone = "0991112223", Role = UserRole.Staff },
                        new User { Username = "staff.lethib", PasswordHash = "123456", FullName = "Nhân viên Lê Thị B", Email = "staff.b@pharmasphere.com", Phone = "0991112224", Role = UserRole.Staff },
                        new User { Username = "staff.tranvanc", PasswordHash = "123456", FullName = "Nhân viên Trần Văn C", Email = "staff.c@pharmasphere.com", Phone = "0991112225", Role = UserRole.Staff },
                        new User { Username = "staff.phamthid", PasswordHash = "123456", FullName = "Nhân viên Phạm Thị D", Email = "staff.d@pharmasphere.com", Phone = "0991112226", Role = UserRole.Staff },
                        new User { Username = "staff.hoangvane", PasswordHash = "123456", FullName = "Nhân viên Hoàng Văn E", Email = "staff.e@pharmasphere.com", Phone = "0991112227", Role = UserRole.Staff }
                    );
                }

                // Seed Admins
                if (!context.Users.Any(u => u.Username == "admin.thanh"))
                {
                    context.Users.Add(new User { Username = "admin.thanh", PasswordHash = "admin123", FullName = "Quản trị viên Thành", Email = "admin.thanh@pharmasphere.com", Phone = "0990008888", Role = UserRole.Admin });
                }

                if (!context.Users.Any(u => u.Username == "admin.tai"))
                {
                    context.Users.Add(new User { Username = "admin.tai", PasswordHash = "admin123", FullName = "Quản trị viên Tài", Email = "admin.tai@pharmasphere.com", Phone = "0990009999", Role = UserRole.Admin });
                }

                context.SaveChanges();
            }
        }
    }
}
