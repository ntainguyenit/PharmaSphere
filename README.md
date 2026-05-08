[English Below]

# PharmaSphere - Hệ thống Quản lý & Thương mại điện tử Nhà thuốc

**PharmaSphere** là một ứng dụng web hiện đại được xây dựng trên nền tảng ASP.NET Core MVC, cung cấp giải pháp toàn diện cho việc quản lý nhà thuốc và kinh doanh dược phẩm trực tuyến. Hệ thống được thiết kế với giao diện cao cấp, trải nghiệm người dùng mượt mà và kiến trúc bảo mật phân tầng.

## Các tính năng nổi bật

### Hệ thống Xác thực Kép (Dual Authentication)
Hệ thống sử dụng cơ chế bảo mật độc lập hoàn toàn cho hai nhóm người dùng:
- **Cổng Khách hàng**: Cho phép người dùng đăng nhập, quản lý giỏ hàng và đặt hàng.
- **Cổng Quản trị (Admin Portal)**: Giao diện tối ưu dành riêng cho nhân viên và quản lý để kiểm soát hệ thống, được bảo vệ bởi các chính sách bảo mật nghiêm ngặt.

### Trải nghiệm Mua sắm (Customer Experience)
- **Tìm kiếm thông minh**: Tìm kiếm sản phẩm theo tên, danh mục hoặc thương hiệu.
- **Chi tiết sản phẩm**: Hiển thị thông tin thành phần, công dụng và sản phẩm liên quan.
- **Giỏ hàng (Shopping Cart)**: Quản lý giỏ hàng realtime sử dụng Session state.
- **Thanh toán (Checkout)**: Quy trình đặt hàng đơn giản với nhiều phương thức thanh toán.

### Quản trị Hệ thống (Admin Management)
- **Dashboard**: Thống kê doanh thu và sản phẩm bán chạy.
- **Quản lý kho**: CRUD Sản phẩm, Danh mục, Thương hiệu.
- **Quản lý đơn hàng**: Tiếp nhận và xử lý hóa đơn của khách hàng.
- **Quản lý nhân sự**: Phân quyền Admin/Staff và quản lý tài khoản người dùng.

## Công nghệ sử dụng
- **Backend**: ASP.NET Core MVC 9.0, Entity Framework Core.
- **Database**: Microsoft SQL Server.
- **Frontend**: Vanilla CSS (Modern Design), JavaScript, Lucide Icons.
- **Architecture**: N-Tier Architecture, Dual Authentication Schemes.

---

# PharmaSphere - Pharmacy Management & E-commerce System

**PharmaSphere** is a modern web application built on the ASP.NET Core MVC platform, providing a comprehensive solution for pharmacy management and online pharmaceutical business. The system is designed with a premium interface, smooth user experience, and tiered security architecture.

## Key Features

### Dual Authentication System
The system uses completely independent security mechanisms for two user groups:
- **Customer Portal**: Allows users to log in, manage shopping carts, and place orders.
- **Admin Portal**: Optimized interface specifically for staff and management to control the system, protected by strict security policies.

### Customer Experience
- **Smart Search**: Search for products by name, category, or brand.
- **Product Details**: Displays information on ingredients, uses, and related products.
- **Shopping Cart**: Real-time cart management using Session state.
- **Checkout**: Simple ordering process with multiple payment methods.

### System Administration
- **Dashboard**: Revenue statistics and top-selling products.
- **Inventory Management**: CRUD Products, Categories, Brands.
- **Order Management**: Receive and process customer invoices.
- **Human Resource Management**: Admin/Staff role assignment and user account management.

## Technology Stack
- **Backend**: ASP.NET Core MVC 9.0, Entity Framework Core.
- **Database**: Microsoft SQL Server.
- **Frontend**: Vanilla CSS (Modern Design), JavaScript, Lucide Icons.
- **Architecture**: N-Tier Architecture, Dual Authentication Schemes.
