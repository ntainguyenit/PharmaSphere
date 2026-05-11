using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PharmaSphere.Data;
using PharmaSphere.Models;
using PharmaSphere.Services.Interfaces;

namespace PharmaSphere.Services.Implementations
{
    public class PrescriptionVerificationService : IPrescriptionVerificationService
    {
        private readonly PharmaContext _context;

        public PrescriptionVerificationService(PharmaContext context)
        {
            _context = context;
        }

        public async Task<PrescriptionVerificationResult> VerifyAsync(int prescriptionId)
        {
            var prescription = await _context.Prescriptions.FindAsync(prescriptionId);
            if (prescription == null) 
                return new PrescriptionVerificationResult { IsValid = false, Reason = "Không tìm thấy đơn thuốc." };

            // Mock AI verification logic
            await Task.Delay(2000); // Simulate AI processing time

            bool isValid = !string.IsNullOrEmpty(prescription.ImageUrl);
            
            return new PrescriptionVerificationResult
            {
                IsValid = isValid,
                ConfidenceScore = 0.95f,
                ExtractedDoctorName = prescription.DoctorName,
                ExtractedPatientName = prescription.PatientName,
                Reason = isValid ? "Đơn thuốc hợp lệ." : "Ảnh đơn thuốc không rõ ràng."
            };
        }

        public async Task<bool> IsValidForProductAsync(int productId, int prescriptionId)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null || !product.IsPrescription) return true;

            var result = await VerifyAsync(prescriptionId);
            return result.IsValid;
        }
    }
}
