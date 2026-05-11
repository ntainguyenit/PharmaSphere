using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PharmaSphere.Data;
using PharmaSphere.Models;
using PharmaSphere.Services.Interfaces;

namespace PharmaSphere.Services.Implementations
{
    public class AuditService : IAuditService
    {
        private readonly PharmaContext _context;

        public AuditService(PharmaContext context)
        {
            _context = context;
        }

        public async Task LogAsync(string entityName, string entityId, string action, string changes, string userId)
        {
            var log = new AuditLog
            {
                EntityName = entityName,
                EntityId = entityId,
                Action = action,
                Changes = changes,
                UserId = userId,
                Timestamp = DateTime.Now
            };

            _context.AuditLogs.Add(log);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<AuditLog>> GetLogsAsync(int count = 100)
        {
            return await _context.AuditLogs
                .OrderByDescending(l => l.Timestamp)
                .Take(count)
                .ToListAsync();
        }

        public async Task<IEnumerable<AuditLog>> GetLogsByEntityAsync(string entityName, string entityId)
        {
            return await _context.AuditLogs
                .Where(l => l.EntityName == entityName && l.EntityId == entityId)
                .OrderByDescending(l => l.Timestamp)
                .ToListAsync();
        }
    }
}
