using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PharmaSphere.Common.Data;

namespace PharmaSphere.Services.Interfaces
{
    public interface IInteractionCheckerService
    {
        Task<InteractionResult> CheckInteractionsAsync(List<int> productIds);
        Task<List<string>> GetDrugInfoAsync(string drugName);
    }

    public class InteractionResult
    {
        public bool HasConflict { get; set; }
        public List<string> Warnings { get; set; } = new List<string>();
        public string Severity { get; set; } // Low, Medium, High, Critical
    }
}
