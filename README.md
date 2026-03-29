# 🎬 Website Xem Phim - Nền Tảng Xem Phim Trực Tuyến Hiện Đại

![Banner](https://img.shields.io/badge/Main_Language-C%23-blue)
![Framework](https://img.shields.io/badge/Framework-.NET%206-512bd4)
![Database](https://img.shields.io/badge/Database-MS%20SQL%20Server-red)
![Status](https://img.shields.io/badge/Status-Completed-success)

**Website Xem Phim** không chỉ là một trang web xem phim thông thường, mà là một hệ thống quản lý nội dung số toàn diện được xây dựng trên nền tảng **ASP.NET Core MVC**.

---

## 🚀 Tính Năng Chính

### 🌐 Dành cho Người dùng (User)
- **Đăng ký/Đăng nhập:** Hỗ trợ bảo mật tài khoản cá nhân, tích hợp đăng nhập qua Gmail.
- **Tìm kiếm thông minh:** Bộ lọc đa năng theo thể loại, quốc gia, năm phát hành và tên phim.
- **Trải nghiệm xem phim:** Trình phát video mượt mà, hỗ trợ cả phim lẻ (Movie) và phim bộ (Series).
- **Tương tác:** Đánh giá sao (Rating), bình luận (Comment) chia sẻ cảm nhận.
- **Cá nhân hóa:**
  - **Hộp phim:** Lưu trữ những bộ phim yêu thích.
  - **Lịch sử:** Theo dõi lại các phim đã xem gần đây.
- **Thông báo:** Nhận email thông báo khi có tập phim mới hoặc cập nhật hệ thống.

### 🛡️ Dành cho Quản trị viên (Admin)
- **Dashboard:** Thống kê lượt xem, số lượng phim và người dùng.
- **Quản lý nội dung (CRUD):** Thêm, sửa, xóa phim, tập phim, thể loại và quốc gia.
- **Kiểm soát người dùng:** Quản lý bình luận và trạng thái hoạt động của các tài khoản.
- **Đồng bộ hóa:** Quản lý dữ liệu tập trung qua giao diện Admin thân thiện.
---

## 🛠 Công Nghệ Sử Dụng

| Thành phần | Công nghệ |
| :--- | :--- |
| **Backend** | .NET 6 (ASP.NET Core MVC), C# |
| **Frontend** | Razor View, JavaScript (ES6), HTML5, CSS3 |
| **UI Framework** | Bootstrap 5, FontAwesome Icons |
| **Database** | Microsoft SQL Server |
| **ORM** | Entity Framework Core |
| **API/Khác** | SMTP Email Service|

---

## 📂 Cấu Trúc Dự Án (MVC)

```text
📁 WebsiteXemPhim
├── 📁 Controllers     # Xử lý logic nghiệp vụ
├── 📁 Models          # Định nghĩa dữ liệu và Database Schema
├── 📁 Views           # Giao diện người dùng (Razor Pages)
├── 📁 wwwroot         # Tài nguyên tĩnh (CSS, JS, Images)
└── appsettings.json   # Cấu hình chuỗi kết nối (Connection String)
