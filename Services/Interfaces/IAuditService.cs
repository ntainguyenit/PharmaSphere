using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PharmaSphere.Models;

namespace PharmaSphere.Services.Interfaces
{
    public interface IAuditService
    {
        Task LogAsync(string entityName, string entityId, string action, string changes, string userId);
        Task<IEnumerable<AuditLog>> GetLogsAsync(int count = 100);
        Task<IEnumerable<AuditLog>> GetLogsByEntityAsync(string entityName, string entityId);
    }
}
