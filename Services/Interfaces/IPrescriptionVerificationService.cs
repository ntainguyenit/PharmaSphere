using System;
using System.Threading.Tasks;
using PharmaSphere.Models;

namespace PharmaSphere.Services.Interfaces
{
    /// <summary>
    /// Professional service for verifying medical prescriptions using AI-based OCR and validation.
    /// </summary>
    public interface IPrescriptionVerificationService
    {
        Task<PrescriptionVerificationResult> VerifyAsync(int prescriptionId);
        Task<bool> IsValidForProductAsync(int productId, int prescriptionId);
    }

    public class PrescriptionVerificationResult
    {
        public bool IsValid { get; set; }
        public float ConfidenceScore { get; set; }
        public string ExtractedDoctorName { get; set; }
        public string ExtractedPatientName { get; set; }
        public string Reason { get; set; }
    }
}
