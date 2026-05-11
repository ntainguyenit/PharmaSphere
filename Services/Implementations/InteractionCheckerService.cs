using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PharmaSphere.Common.Data;
using PharmaSphere.Services.Interfaces;
using PharmaSphere.Data;

namespace PharmaSphere.Services.Implementations
{
    public class InteractionCheckerService : IInteractionCheckerService
    {
        private readonly PharmaContext _context;

        public InteractionCheckerService(PharmaContext context)
        {
            _context = context;
        }

        public async Task<InteractionResult> CheckInteractionsAsync(List<int> productIds)
        {
            var result = new InteractionResult { HasConflict = false };
            
            var products = _context.Products.Where(p => productIds.Contains(p.Id)).ToList();
            var drugNames = products.Select(p => p.Name).ToList();

            foreach (var name in drugNames)
            {
                var info = DrugDatabase.Entries.FirstOrDefault(d => name.Contains(d.Name) || d.Name.Contains(name));
                if (info != null)
                {
                    foreach (var otherName in drugNames.Where(n => n != name))
                    {
                        if (info.Interactions.Any(i => otherName.Contains(i) || i.Contains(otherName)))
                        {
                            result.HasConflict = true;
                            result.Warnings.Add($"Cảnh báo: {name} có thể tương tác với {otherName}.");
                            result.Severity = "High";
                        }
                    }
                }
            }

            return await Task.FromResult(result);
        }

        public async Task<List<string>> GetDrugInfoAsync(string drugName)
        {
            var info = DrugDatabase.Entries.FirstOrDefault(d => drugName.Contains(d.Name) || d.Name.Contains(drugName));
            if (info == null) return new List<string> { "Không tìm thấy thông tin." };

            return new List<string>
            {
                $"Tên gốc: {info.GenericName}",
                $"Danh mục: {info.Category}",
                $"Chỉ định: {info.Indications}",
                $"Tác dụng phụ: {info.SideEffects}",
                $"Liều dùng: {info.Dosage}"
            };
        }
    }
}
