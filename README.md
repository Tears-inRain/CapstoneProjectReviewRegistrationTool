# Capstone Project Review Registration Tool v2

Hệ thống quản lý đăng ký lịch bảo vệ Đồ án Tốt nghiệp (Capstone Project) xây dựng bằng kiến trúc chuẩn 3-Layer (API, Service, Repository) trên nền tảng .NET 10 Web API và SQL Server. 

Dự án có hỗ trợ triển khai bằng Docker Compose và đính kèm bộ Test Automation khép kín qua Postman.

## 🚀 Tính Năng (Features)
- Xem danh sách Slot khả dụng (`Available Slots`).
- Nhóm sinh viên (Do Leader đại diện) đăng ký Slot Review (`Book Team`).
- Giảng viên xếp nguyện vọng gác thi (`Book Lecturer`).
- Admin / Moderator cài đặt giới hạn số Slot của Giảng viên (`MinSlot`, `MaxSlot`).
- Tự động chạy thuật toán xếp phòng (`Auto Schedule`) để liên kết Team, Lecturer và Slot.
- Quản lý vòng lặp cấp cứu lỗi Database / Data Reset Automation qua Test Endpoints.

## 🛠 Prerequisites
Bạn cần cài đặt các phần mềm sau trên máy tính của mình:
- [Git](https://git-scm.com/)
- [Docker & Docker Compose](https://www.docker.com/products/docker-desktop/) (Docker Desktop)
- [Postman](https://www.postman.com/downloads/) (Dành cho việc chạy API Automation Test)

---

## 💻 Hướng Dẫn Kéo Code (Clone & Build)

**1. Clone kho lưu trữ từ GitHub về máy:**
Mở Terminal/Command Prompt và chạy lệnh sau:
```bash
git clone <URL_GITHUB_CỦA_BẠN>
cd CapstoneProjectReviewRegistrationTool
```

**2. Khởi chạy toàn bộ hệ thống bằng Docker Compose:**
Đảm bảo Docker Desktop của bạn đang mở và sẵn sàng. Gõ lệnh sau tại thư mục gốc (chứa file `docker-compose.yml`):
```bash
docker-compose up -d --build
```

**Lệnh trên sẽ tự động:**
1. Pull image `mssql/server:2022-latest` để dựng database `CapstoneReviewDb` với tài khoản `sa` (Mật khẩu: `Capstone@PRN232_2026!`).
2. Build Image Web API .NET 10 bằng cách đọc cấu hình từ file `Dockerfile`.
3. Chờ SQL Server khởi động thành công, Ứng dụng API sẽ tự động kích hoạt chức năng Migration / EnsureCreated để cấu trúc Database ngay khi khởi chạy.

**3. Tắt hệ thống khi không dùng đến:**
```bash
docker-compose down
```

---

## 🧪 Hướng Dẫn Auto-Test & Seed Data bằng Postman

1. Mở ứng dụng **Postman**.
2. Nhấn nút **Import** góc trên bên trái, kéo thả file `postman_collection.json` (Nằm ngay trong thư mục gốc của dự án) vào Postman.
3. Chọn thẻ Collection mới tên `Capstone Project Review Registration Tool v2`.
4. Chuột phải vào tên Collection và chọn **Run Collection**.

**💡 Lưu ý cốt lõi:**
Bộ Postman Collection này đã được tích hợp **Pre-request Script**. Trước khi chạy API số 1, nó luôn tự gọi Endpoint bí mật `POST http://localhost:5000/api/test-setup/reset-db` trước để chọc thẳng file SQL `init_db_mock.sql` xóa trắng và bơm lại toàn bộ Data Mẫu của Giảng Viên, Sinh Viên, Team, và Slot trắng. Do vậy, hệ thống Test của bạn sẽ luôn Passed với tỷ lệ 100% nhờ độ toàn vẹn Data cao tuyệt đối.

## 🗂 Cấu trúc Mã Nguồn (Architecture)
Hệ thống tuân thủ chặt chẽ nguyên lý Dependency Inversion.

- `CapstoneReview.API` (Presentations): Nơi tiếp nhận HTTP, config Dependency Injection, chứa Endpoints.
- `CapstoneReview.Service` (Business Rules): Xử lý toàn vẹn Core Logic (Team Owner Validations, Check Slot Boundaries), độc lập hoàn toàn khỏi Entity Framework. Dùng DTOs.
- `CapstoneReview.Repository` (Data Access): Chứa UnitOfWork và Repositories, kết nối SQL Server thông qua DbContext.
