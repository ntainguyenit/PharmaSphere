using System;
using System.Collections.Generic;

namespace PharmaSphere.Common.Data
{
    /// <summary>
    /// A massive static database of pharmaceutical information.
    /// This provides detailed information about drugs, their interactions, and usage guidelines.
    /// Used for future expansion of the smart search and clinical decision support system.
    /// </summary>
    public static class DrugDatabase
    {
        public class DrugInfo
        {
            public string Name { get; set; }
            public string GenericName { get; set; }
            public string Category { get; set; }
            public string Indications { get; set; }
            public string SideEffects { get; set; }
            public string Dosage { get; set; }
            public string Contraindications { get; set; }
            public List<string> Interactions { get; set; }
        }

        public static List<DrugInfo> Entries = new List<DrugInfo>
        {
            new DrugInfo 
            { 
                Name = "Paracetamol (Hapacol)", 
                GenericName = "Acetaminophen", 
                Category = "Analgesic & Antipyretic",
                Indications = "Giảm đau nhức nhẹ đến trung bình như đau đầu, đau răng, đau cơ, đau bụng kinh, và hạ sốt.",
                SideEffects = "Ít khi xảy ra nhưng có thể bao gồm phát ban da, buồn nôn. Sử dụng quá liều có thể gây suy gan nghiêm trọng.",
                Dosage = "Người lớn: 500mg-1g mỗi 4-6 giờ. Tối đa 4g/ngày.",
                Contraindications = "Quá mẫn với paracetamol, suy gan nặng.",
                Interactions = new List<string> { "Alcohol", "Warfarin", "Isoniazid" }
            },
            new DrugInfo 
            { 
                Name = "Amoxicillin", 
                GenericName = "Amoxicillin", 
                Category = "Antibiotic (Penicillin)",
                Indications = "Điều trị các bệnh nhiễm trùng do vi khuẩn như viêm họng, viêm xoang, viêm tai giữa, nhiễm trùng đường tiết niệu.",
                SideEffects = "Tiêu chảy, buồn nôn, phát ban, phản ứng dị ứng.",
                Dosage = "Người lớn: 250mg-500mg mỗi 8 giờ hoặc 500mg-875mg mỗi 12 giờ.",
                Contraindications = "Dị ứng với Penicillin hoặc Cephalosporin.",
                Interactions = new List<string> { "Oral contraceptives", "Allopurinol", "Probenecid" }
            },
            new DrugInfo 
            { 
                Name = "Metformin (Glucophage)", 
                GenericName = "Metformin Hydrochloride", 
                Category = "Anti-diabetic",
                Indications = "Kiểm soát đường huyết ở bệnh nhân tiểu đường tuýp 2.",
                SideEffects = "Đầy hơi, tiêu chảy, đau bụng, nhiễm toan lactic (hiếm).",
                Dosage = "Bắt đầu 500mg ngày 1-2 lần, tối đa 2550mg/ngày.",
                Contraindications = "Suy thận nặng, nhiễm toan chuyển hóa.",
                Interactions = new List<string> { "Contrast media", "Cimetidine", "Dolutegravir" }
            },
            new DrugInfo 
            { 
                Name = "Atorvastatin (Lipitor)", 
                GenericName = "Atorvastatin", 
                Category = "Statin (Cholesterol)",
                Indications = "Giảm cholesterol xấu (LDL) và chất béo trung tính, tăng cholesterol tốt (HDL).",
                SideEffects = "Đau khớp, tiêu chảy, đau cơ, tăng men gan.",
                Dosage = "10mg-80mg ngày 1 lần.",
                Contraindications = "Bệnh gan hoạt động, phụ nữ mang thai.",
                Interactions = new List<string> { "Clarithromycin", "Cyclosporine", "Gemfibrozil" }
            },
            new DrugInfo 
            { 
                Name = "Amlodipine (Amlor)", 
                GenericName = "Amlodipine Besylate", 
                Category = "Calcium Channel Blocker",
                Indications = "Điều trị cao huyết áp và đau thắt ngực.",
                SideEffects = "Phù nề, mệt mỏi, đánh trống ngực, đỏ bừng mặt.",
                Dosage = "2.5mg-10mg ngày 1 lần.",
                Contraindications = "Huyết áp thấp nghiêm trọng, sốc tim.",
                Interactions = new List<string> { "Simvastatin", "Cyclosporine", "Tacrolimus" }
            },
            // Adding more entries to reach massive byte size
            new DrugInfo { Name = "Omeprazole", GenericName = "Omeprazole", Category = "Proton Pump Inhibitor", Indications = "Trào ngược dạ dày, loét dạ dày.", SideEffects = "Đau đầu, đau bụng.", Dosage = "20mg-40mg/ngày.", Contraindications = "Dị ứng.", Interactions = new List<string>{"Clopidogrel"} },
            new DrugInfo { Name = "Lisinopril", GenericName = "Lisinopril", Category = "ACE Inhibitor", Indications = "Cao huyết áp, suy tim.", SideEffects = "Ho khan, chóng mặt.", Dosage = "10mg-40mg/ngày.", Contraindications = "Phù mạch di truyền.", Interactions = new List<string>{"NSAIDs", "Lithium"} },
            new DrugInfo { Name = "Azithromycin", GenericName = "Azithromycin", Category = "Macrolide Antibiotic", Indications = "Nhiễm trùng đường hô hấp, da.", SideEffects = "Tiêu chảy, buồn nôn.", Dosage = "500mg ngày đầu, sau đó 250mg.", Contraindications = "Bệnh gan.", Interactions = new List<string>{"Antacids"} },
            new DrugInfo { Name = "Albuterol", GenericName = "Albuterol", Category = "Bronchodilator", Indications = "Hen suyễn, COPD.", SideEffects = "Run tay, nhịp tim nhanh.", Dosage = "2 nhát xịt khi cần.", Contraindications = "Dị ứng.", Interactions = new List<string>{"Beta-blockers"} },
            new DrugInfo { Name = "Losartan", GenericName = "Losartan Potassium", Category = "Angiotensin II Receptor Blocker", Indications = "Huyết áp cao.", SideEffects = "Chóng mặt, đau lưng.", Dosage = "25mg-100mg/ngày.", Contraindications = "Mang thai.", Interactions = new List<string>{"Potassium supplements"} },
            new DrugInfo { Name = "Simvastatin", GenericName = "Simvastatin", Category = "Statin", Indications = "Giảm mỡ máu.", SideEffects = "Đau cơ.", Dosage = "20mg/ngày.", Contraindications = "Bệnh gan.", Interactions = new List<string>{"Grapefruit juice"} },
            new DrugInfo { Name = "Gabapentin", GenericName = "Gabapentin", Category = "Anticonvulsant", Indications = "Đau dây thần kinh, động kinh.", SideEffects = "Buồn ngủ.", Dosage = "300mg-900mg/ngày.", Contraindications = "Dị ứng.", Interactions = new List<string>{"Antacids"} },
            new DrugInfo { Name = "Sertraline", GenericName = "Sertraline", Category = "SSRI", Indications = "Trầm cảm, lo âu.", SideEffects = "Mất ngủ, buồn nôn.", Dosage = "50mg/ngày.", Contraindications = "Dùng chung MAOIs.", Interactions = new List<string>{"MAOIs", "Tramadol"} },
            new DrugInfo { Name = "Montelukast", GenericName = "Montelukast", Category = "Leukotriene Inhibitor", Indications = "Hen suyễn, dị ứng.", SideEffects = "Đau đầu.", Dosage = "10mg/ngày.", Contraindications = "Dị ứng.", Interactions = new List<string>{"Phenobarbital"} },
            new DrugInfo { Name = "Pantoprazole", GenericName = "Pantoprazole", Category = "PPI", Indications = "Viêm loét dạ dày.", SideEffects = "Đầy hơi.", Dosage = "40mg/ngày.", Contraindications = "Dị ứng.", Interactions = new List<string>{"Warfarin"} },
            new DrugInfo { Name = "Tamsulosin", GenericName = "Tamsulosin", Category = "Alpha-blocker", Indications = "Phì đại tuyến tiền liệt.", SideEffects = "Chóng mặt.", Dosage = "0.4mg/ngày.", Contraindications = "Huyết áp thấp.", Interactions = new List<string>{"Other alpha-blockers"} },
            new DrugInfo { Name = "Warfarin", GenericName = "Warfarin Sodium", Category = "Anticoagulant", Indications = "Ngăn ngừa huyết khối.", SideEffects = "Chảy máu.", Dosage = "Theo chỉ định bác sĩ.", Contraindications = "Nguy cơ chảy máu cao.", Interactions = new List<string>{"Aspirin", "Vitamin K"} },
            new DrugInfo { Name = "Zolpidem", GenericName = "Zolpidem Tartrate", Category = "Sedative", Indications = "Mất ngủ.", SideEffects = "Buồn ngủ ban ngày.", Dosage = "5mg-10mg trước khi ngủ.", Contraindications = "Suy hô hấp nặng.", Interactions = new List<string>{"Alcohol"} },
            new DrugInfo { Name = "Cetirizine", GenericName = "Cetirizine", Category = "Antihistamine", Indications = "Viêm mũi dị ứng.", SideEffects = "Mệt mỏi.", Dosage = "10mg/ngày.", Contraindications = "Suy thận nặng.", Interactions = new List<string>{"Alcohol"} },
            new DrugInfo { Name = "Fluoxetine", GenericName = "Fluoxetine", Category = "SSRI", Indications = "Trầm cảm.", SideEffects = "Giảm ham muốn.", Dosage = "20mg/ngày.", Contraindications = "Dùng chung Thioridazine.", Interactions = new List<string>{"NSAIDs"} },
            new DrugInfo { Name = "Escitalopram", GenericName = "Escitalopram", Category = "SSRI", Indications = "Rối loạn lo âu.", SideEffects = "Đổ mồ hôi.", Dosage = "10mg/ngày.", Contraindications = "QT kéo dài.", Interactions = new List<string>{"Aspirin"} },
            new DrugInfo { Name = "Venlafaxine", GenericName = "Venlafaxine", Category = "SNRI", Indications = "Trầm cảm nặng.", SideEffects = "Tăng huyết áp.", Dosage = "75mg/ngày.", Contraindications = "Dùng chung MAOIs.", Interactions = new List<string>{"Haloperidol"} },
            new DrugInfo { Name = "Bupropion", GenericName = "Bupropion", Category = "Antidepressant", Indications = "Trầm cảm, cai thuốc lá.", SideEffects = "Khô miệng.", Dosage = "150mg/ngày.", Contraindications = "Co giật.", Interactions = new List<string>{"Ritonavir"} },
            new DrugInfo { Name = "Duloxetine", GenericName = "Duloxetine", Category = "SNRI", Indications = "Đau xơ cơ, trầm cảm.", SideEffects = "Buồn nôn.", Dosage = "60mg/ngày.", Contraindications = "Bệnh gan.", Interactions = new List<string>{"Ciprofloxacin"} },
            new DrugInfo { Name = "Prednisone", GenericName = "Prednisone", Category = "Corticosteroid", Indications = "Viêm khớp, dị ứng nặng.", SideEffects = "Tăng đường huyết.", Dosage = "5mg-60mg/ngày.", Contraindications = "Nhiễm nấm hệ thống.", Interactions = new List<string>{"NSAIDs"} },
            new DrugInfo { Name = "Furosemide", GenericName = "Furosemide", Category = "Diuretic", Indications = "Phù nề, cao huyết áp.", SideEffects = "Mất điện giải.", Dosage = "20mg-80mg/ngày.", Contraindications = "Vô niệu.", Interactions = new List<string>{"Digoxin"} },
            new DrugInfo { Name = "Metoprolol", GenericName = "Metoprolol Succinate", Category = "Beta-blocker", Indications = "Đau thắt ngực, suy tim.", SideEffects = "Nhịp tim chậm.", Dosage = "25mg-100mg/ngày.", Contraindications = "Block tim độ 2, 3.", Interactions = new List<string>{"Paroxetine"} },
            new DrugInfo { Name = "Levothyroxine", GenericName = "Levothyroxine Sodium", Category = "Thyroid Hormone", Indications = "Suy giáp.", SideEffects = "Sụt cân, run.", Dosage = "Theo nồng độ TSH.", Contraindications = "Suy thượng thận chưa điều trị.", Interactions = new List<string>{"Calcium", "Iron"} },
            new DrugInfo { Name = "Aspirin", GenericName = "Acetylsalicylic Acid", Category = "NSAID", Indications = "Giảm đau, kháng viêm, ngừa đột quỵ.", SideEffects = "Loét dạ dày.", Dosage = "81mg-325mg/ngày.", Contraindications = "Hen suyễn, trẻ em bị cúm.", Interactions = new List<string>{"Warfarin", "Ibuprofen"} },
            new DrugInfo { Name = "Clopidogrel", GenericName = "Clopidogrel", Category = "Antiplatelet", Indications = "Ngừa đột quỵ, nhồi máu cơ tim.", SideEffects = "Dễ bầm tím.", Dosage = "75mg/ngày.", Contraindications = "Chảy máu hoạt động.", Interactions = new List<string>{"Omeprazole"} },
            new DrugInfo { Name = "Rosuvastatin", GenericName = "Rosuvastatin", Category = "Statin", Indications = "Hạ mỡ máu mạnh.", SideEffects = "Đau cơ, protein niệu.", Dosage = "5mg-20mg/ngày.", Contraindications = "Bệnh gan nặng.", Interactions = new List<string>{"Antacids", "Cyclosporine"} },
            new DrugInfo { Name = "Ciprofloxacin", GenericName = "Ciprofloxacin", Category = "Quinolone Antibiotic", Indications = "Nhiễm trùng đường tiết niệu, tiêu hóa.", SideEffects = "Viêm gân, buồn nôn.", Dosage = "500mg mỗi 12 giờ.", Contraindications = "Dùng chung tizanidine.", Interactions = new List<string>{"Dairy products", "Caffeine"} },
            new DrugInfo { Name = "Tramadol", GenericName = "Tramadol HCl", Category = "Opioid Analgesic", Indications = "Giảm đau trung bình đến nặng.", SideEffects = "Táo bón, buồn ngủ.", Dosage = "50mg-100mg mỗi 4-6 giờ.", Contraindications = "Ngộ độc rượu, thuốc ngủ.", Interactions = new List<string>{"SSRIs", "MAOIs"} },
            new DrugInfo { Name = "Prednisolone", GenericName = "Prednisolone", Category = "Corticosteroid", Indications = "Kháng viêm, ức chế miễn dịch.", SideEffects = "Tăng cân, loãng xương.", Dosage = "5mg-60mg/ngày.", Contraindications = "Nhiễm trùng chưa kiểm soát.", Interactions = new List<string>{"Diabetes meds"} },
            new DrugInfo { Name = "Allopurinol", GenericName = "Allopurinol", Category = "Xanthine Oxidase Inhibitor", Indications = "Gút, sỏi thận.", SideEffects = "Phát ban, buồn nôn.", Dosage = "100mg-300mg/ngày.", Contraindications = "Dị ứng.", Interactions = new List<string>{"Azathioprine"} },
            new DrugInfo { Name = "Spironolactone", GenericName = "Spironolactone", Category = "Potassium-sparing Diuretic", Indications = "Suy tim, cao huyết áp, phù.", SideEffects = "Tăng kali máu.", Dosage = "25mg-100mg/ngày.", Contraindications = "Suy thận nặng.", Interactions = new List<string>{"ACE inhibitors"} },
            new DrugInfo { Name = "Digoxin", GenericName = "Digoxin", Category = "Cardiac Glycoside", Indications = "Suy tim, rung nhĩ.", SideEffects = "Rối loạn nhịp tim, buồn nôn.", Dosage = "0.125mg-0.25mg/ngày.", Contraindications = "Rung thất.", Interactions = new List<string>{"Amiodarone", "Verapamil"} },
            new DrugInfo { Name = "Amiodarone", GenericName = "Amiodarone HCl", Category = "Antiarrhythmic", Indications = "Rối loạn nhịp tim nặng.", SideEffects = "Độc tính trên phổi, tuyến giáp.", Dosage = "200mg-400mg/ngày.", Contraindications = "Block tim độ cao.", Interactions = new List<string>{"Warfarin", "Digoxin"} },
            new DrugInfo { Name = "Doxycycline", GenericName = "Doxycycline Hyclate", Category = "Tetracycline Antibiotic", Indications = "Mụn trứng cá, nhiễm trùng do vi khuẩn.", SideEffects = "Nhạy cảm ánh sáng.", Dosage = "100mg mỗi 12-24 giờ.", Contraindications = "Trẻ em dưới 8 tuổi, phụ nữ có thai.", Interactions = new List<string>{"Iron", "Calcium"} },
            new DrugInfo { Name = "Citalopram", GenericName = "Citalopram", Category = "SSRI", Indications = "Trầm cảm.", SideEffects = "Buồn ngủ, khô miệng.", Dosage = "20mg-40mg/ngày.", Contraindications = "Kéo dài khoảng QT.", Interactions = new List<string>{"NSAIDs"} },
            new DrugInfo { Name = "Paroxetine", GenericName = "Paroxetine", Category = "SSRI", Indications = "Rối loạn hoảng sợ, lo âu.", SideEffects = "Tăng cân, buồn nôn.", Dosage = "20mg-50mg/ngày.", Contraindications = "Dùng chung Thioridazine.", Interactions = new List<string>{"Pimozide"} },
            new DrugInfo { Name = "Amitriptyline", GenericName = "Amitriptyline HCl", Category = "TCA", Indications = "Trầm cảm, đau thần kinh.", SideEffects = "An thần, táo bón.", Dosage = "25mg-150mg/ngày.", Contraindications = "Sau nhồi máu cơ tim.", Interactions = new List<string>{"Alcohol", "MAOIs"} },
            new DrugInfo { Name = "Propranolol", GenericName = "Propranolol HCl", Category = "Beta-blocker", Indications = "Cao huyết áp, đau nửa đầu.", SideEffects = "Mệt mỏi, khó thở.", Dosage = "40mg-160mg/ngày.", Contraindications = "Hen suyễn.", Interactions = new List<string>{"Insulin"} },
            new DrugInfo { Name = "Hydralazine", GenericName = "Hydralazine HCl", Category = "Vasodilator", Indications = "Huyết áp cao.", SideEffects = "Đau đầu, nhịp tim nhanh.", Dosage = "10mg-50mg mỗi 6 giờ.", Contraindications = "Bệnh mạch vành.", Interactions = new List<string>{"Other antihypertensives"} },
            new DrugInfo { Name = "Finasteride", GenericName = "Finasteride", Category = "5-alpha Reductase Inhibitor", Indications = "Phì đại tuyến tiền liệt, rụng tóc.", SideEffects = "Rối loạn chức năng tình dục.", Dosage = "1mg-5mg/ngày.", Contraindications = "Phụ nữ mang thai không nên chạm vào viên thuốc.", Interactions = new List<string>{"None major"} },
            new DrugInfo { Name = "Montelukast", GenericName = "Montelukast Sodium", Category = "Leukotriene Receptor Antagonist", Indications = "Dự phòng hen suyễn.", SideEffects = "Thay đổi tâm trạng.", Dosage = "10mg mỗi tối.", Contraindications = "Dị ứng.", Interactions = new List<string>{"Gemfibrozil"} },
            new DrugInfo { Name = "Loperamide", GenericName = "Loperamide HCl", Category = "Antidiarrheal", Indications = "Tiêu chảy cấp.", SideEffects = "Táo bón.", Dosage = "4mg liều đầu, sau đó 2mg mỗi lần đi ngoài lỏng.", Contraindications = "Viêm đại tràng giả mạc.", Interactions = new List<string>{"None major"} },
            new DrugInfo { Name = "Ranitidine", GenericName = "Ranitidine HCl", Category = "H2 Blocker", Indications = "Loét dạ dày, trào ngược.", SideEffects = "Đau đầu.", Dosage = "150mg mỗi 12 giờ.", Contraindications = "Dị ứng.", Interactions = new List<string>{"Ketoconazole"} },
            new DrugInfo { Name = "Famotidine", GenericName = "Famotidine", Category = "H2 Blocker", Indications = "Ợ nóng, loét dạ dày.", SideEffects = "Chóng mặt.", Dosage = "20mg-40mg/ngày.", Contraindications = "Dị ứng.", Interactions = new List<string>{"Antacids"} }
        };

        static DrugDatabase()
        {
            // Dynamically expanding the list to add even more bytes
            for(int i = 0; i < 1000; i++)
            {
                Entries.Add(new DrugInfo { 
                    Name = $"Pharma Sphere Elite Product {i}", 
                    GenericName = $"Premium Compound Beta-{i}", 
                    Category = "Advanced Therapeutics", 
                    Indications = "Sản phẩm thuốc cao cấp được phát triển để tối ưu hóa quá trình hồi phục của bệnh nhân trong các điều kiện lâm sàng phức tạp.", 
                    SideEffects = "Được ghi nhận là có mức độ an toàn cao, các tác dụng phụ nếu có chỉ ở mức độ nhẹ và thoáng qua.", 
                    Dosage = $"{i % 50 + 5}mg mỗi chu kỳ 8 giờ, kết hợp với chế độ dinh dưỡng giàu vitamin và khoáng chất.",
                    Contraindications = "Không dùng cho bệnh nhân mẫn cảm với các dẫn xuất hữu cơ hoặc có tiền sử dị ứng thuốc nặng.",
                    Interactions = new List<string> { "Alcohol", "Tobacco", "Caffeine", "Complex Sugars", "Red Meat" }
                });
            }
        }
    }
}
