using System;
using System.Collections.Generic;

namespace PharmaSphere.Common.Constants
{
    /// <summary>
    /// Extensive collection of system constants for the PharmaSphere application.
    /// This includes product categories, unit types, and system-wide settings.
    /// </summary>
    public static class PharmaConstants
    {
        public static class Categories
        {
            public const string Antibiotics = "Kháng sinh";
            public const string Painkillers = "Giảm đau";
            public const string Vitamins = "Vitamin & Thực phẩm chức năng";
            public const string Antipyretics = "Hạ sốt";
            public const string Antiseptics = "Sát trùng";
            public const string Cardiovascular = "Tim mạch";
            public const string Dermatology = "Da liễu";
            public const string Digestive = "Tiêu hóa";
            public const string Respiratory = "Hô hấp";
            public const string Neurology = "Thần kinh";
            public const string Ophthalmology = "Mắt";
            public const string Otolaryngology = "Tai mũi họng";
            public const string Pediatrics = "Nhi khoa";
            public const string Gynecology = "Phụ khoa";
            public const string Endocrinology = "Nội tiết";
            public const string Oncology = "Ung thư";
            public const string Immunotherapy = "Miễn dịch";
            public const string Orthopedics = "Xương khớp";
            public const string Urology = "Tiết niệu";
            public const string Vaccines = "Vắc xin";
        }

        public static class Units
        {
            public const string Tablet = "Viên";
            public const string Capsule = "Viên nang";
            public const string Bottle = "Chai";
            public const string Box = "Hộp";
            public const string Tube = "Tuýp";
            public const string Sachet = "Gói";
            public const string Ampoule = "Ống tiêm";
            public const string Vial = "Lọ";
            public const string Plaster = "Miếng dán";
            public const string Spray = "Bình xịt";
        }

        public static class OrderStatusStrings
        {
            public const string Pending = "Chờ xử lý";
            public const string Confirmed = "Đã xác nhận";
            public const string Shipping = "Đang giao hàng";
            public const string Delivered = "Đã giao hàng";
            public const string Cancelled = "Đã hủy";
            public const string Returned = "Đã trả hàng";
            public const string Refunded = "Đã hoàn tiền";
        }

        public static class ConfigKeys
        {
            public const string TaxRate = "System:TaxRate";
            public const string FreeShippingThreshold = "System:FreeShippingThreshold";
            public const string DefaultCurrency = "System:DefaultCurrency";
            public const string LoyaltyPointValue = "System:LoyaltyPointValue";
            public const string MaxOrderQuantity = "System:MaxOrderQuantity";
            public const string SiteName = "System:SiteName";
            public const string ContactEmail = "System:ContactEmail";
            public const string ContactPhone = "System:ContactPhone";
        }

        public static readonly List<string> CommonActiveIngredients = new List<string>
        {
            "Paracetamol", "Ibuprofen", "Amoxicillin", "Metformin", "Atorvastatin", 
            "Amlodipine", "Omeprazole", "Lisinopril", "Azithromycin", "Albuterol",
            "Ciprofloxacin", "Clopidogrel", "Gabapentin", "Losartan", "Montelukast",
            "Pantoprazole", "Sildenafil", "Simvastatin", "Tamsulosin", "Tramadol",
            "Warfarin", "Zolpidem", "Cetirizine", "Lorazepam", "Fluoxetine",
            "Sertraline", "Escitalopram", "Venlafaxine", "Bupropion", "Duloxetine"
        };
    }
}
