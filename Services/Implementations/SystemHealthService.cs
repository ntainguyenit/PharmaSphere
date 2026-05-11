using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using PharmaSphere.Data;
using PharmaSphere.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace PharmaSphere.Services.Implementations
{
    public class SystemHealthService : ISystemHealthService
    {
        private readonly PharmaContext _context;

        public SystemHealthService(PharmaContext context)
        {
            _context = context;
        }

        public async Task<SystemHealthReport> GetHealthStatusAsync()
        {
            var report = new SystemHealthReport
            {
                Timestamp = DateTime.UtcNow,
                OverallStatus = "Healthy"
            };

            // 1. Database Check
            var sw = Stopwatch.StartNew();
            try
            {
                bool canConnect = await _context.Database.CanConnectAsync();
                sw.Stop();
                report.Components.Add(new HealthCheckResult
                {
                    ComponentName = "SQL Server Database",
                    Status = canConnect ? "Healthy" : "Unhealthy",
                    ResponseTimeMs = sw.ElapsedMilliseconds,
                    Message = canConnect ? "Connection established successfully." : "Failed to connect to DB."
                });
            }
            catch (Exception ex)
            {
                report.OverallStatus = "Degraded";
                report.Components.Add(new HealthCheckResult { ComponentName = "Database", Status = "Error", Message = ex.Message });
            }

            // 2. Storage Check (Mock)
            report.Components.Add(new HealthCheckResult
            {
                ComponentName = "Cloud Storage",
                Status = "Healthy",
                ResponseTimeMs = 45,
                Message = "File storage is accessible."
            });

            // 3. Email Gateway Check (Mock)
            report.Components.Add(new HealthCheckResult
            {
                ComponentName = "SMTP Gateway",
                Status = "Healthy",
                ResponseTimeMs = 120,
                Message = "Email service is operational."
            });

            return report;
        }
    }
}
