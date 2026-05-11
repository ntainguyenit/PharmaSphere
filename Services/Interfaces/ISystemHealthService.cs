using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PharmaSphere.Data;
using Microsoft.EntityFrameworkCore;

namespace PharmaSphere.Services.Interfaces
{
    public interface ISystemHealthService
    {
        Task<SystemHealthReport> GetHealthStatusAsync();
    }

    public class SystemHealthReport
    {
        public string OverallStatus { get; set; }
        public DateTime Timestamp { get; set; }
        public List<HealthCheckResult> Components { get; set; } = new List<HealthCheckResult>();
    }

    public class HealthCheckResult
    {
        public string ComponentName { get; set; }
        public string Status { get; set; }
        public string Message { get; set; }
        public long ResponseTimeMs { get; set; }
    }
}
