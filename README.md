# Capstone Project Review Registration Tool v2.7

Hệ thống quản lý đăng ký lịch bảo vệ Đồ án Tốt nghiệp (Capstone Project) xây dựng bằng kiến trúc chuẩn 3-Layer (API, Service, Repository) trên nền tảng .NET 10 Web API và Entity Framework Core SQL Server. 

Dự án có hỗ trợ triển khai bằng Docker Compose và đính kèm bộ Test Automation khép kín qua Postman với dữ liệu chuẩn chỉ.

## 🚀 Tính Năng (Features) & Flow Dự Án
- Lọc danh sách Nền tảng (**Master Data**): Lấy danh sách Sinh viên, Tham chiếu Giảng viên (`Lecturer`) và Nhóm Đề tài (`Team`).
- Khởi tạo Slot bảo vệ: Moderator có quyền setup các Slot trống theo lịch trình.
- Đăng ký Slot (Sinh viên): Nhóm sinh viên (Do Leader đại diện) đăng ký các Slot tự do.
- Đăng ký Slot (Giảng viên): Giảng viên xếp nguyện vọng gác thi.
- Phân quyền Trọng tài: Admin / Moderator cài đặt giới hạn Block / MinSlot / MaxSlot của từng Giảng viên.
- Auto Xếp Lịch (**Auto Schedule**): Cỗ máy tính toán Greedy tự động ghép Nối Giảng Viên, Nhóm và Slot sao cho thoả mãn chống Trùng Lặp GVHD và đạt hạn mức tối thiểu.
- Tự động Reset DB (**Test Automation**): Phục vụ việc Testing nhanh gọn 100% tỷ lệ Passed.

## 🛠 Danh sách HTTP API Endpoints

Hệ thống cung cấp mảng REST API hoàn chỉnh phục vụ React Frontend:

**1. Khởi tạo & Dữ liệu Nền (Master Data)**
- `POST /api/test-setup/reset-db`: Xóa trắng và nạp lại Database mẫu.
- `GET /api/Lecturer`: Lấy toàn bộ danh sách Giảng viên.
- `GET /api/Team`: Lấy toàn bộ danh sách Nhóm.
- `POST /api/Slot`: Moderator tạo Slot mới cho đợt bảo vệ.

**2. Đăng ký & Lịch trình (Booking)**
- `GET /api/Slot/available`: Liệt kê các Slot còn trống chưa bị đầy giới hạn.
- `POST /api/Booking/team`: Đăng ký Slot cho Team (Leader bắt buộc).
- `POST /api/Booking/lecturer`: Giảng viên đăng ký Slot tham gia chấm thi.

**3. Quản trị & Thuật toán Xếp Lịch (Moderator & Algorithm)**
- `PUT /api/Moderator/lecturer-config/{lecturerId}`: Cấu hình Max/Min Slot cho GV.
- `POST /api/Schedule/auto-schedule`: Chạy thuật toán tự động điền lịch `Greedy`. Cần body `reviewRound`.
- `GET /api/Schedule/{reviewRound}`: Trả về kết xuất toàn bộ Lịch bảo vệ, Mapping sẵn Tên Nhóm, GV, Room.

---

## 💻 Hướng Dẫn Kéo Code & Chạy Server (Docker)

**1. Clone kho lưu trữ từ GitHub về máy:**
Mở Terminal/Command Prompt và chạy lệnh sau:
```bash
git clone <URL_GITHUB_CỦA_BẠN>
cd CapstoneProjectReviewRegistrationTool
```

**2. Khởi chạy toàn bộ hệ thống bằng Docker Compose:**
Đảm bảo Docker Desktop của bạn đang mở và sẵn sàng. Gõ lệnh sau tại thư mục gốc (chứa file `docker-compose.yml`):
```bash
docker compose up -d --build
```

**Lệnh trên sẽ tự động:**
1. Cấu hình container `mssql_db` với SQL Server 2022 (Mật khẩu: `Capstone@PRN232_2026!`).
2. Build và Hosting Web API .NET 10 tại cổng `http://localhost:5000`.
3. Entity Framework Core tự động Migration/EnsureCreated thiết lập bảng dữ liệu khi khởi chạy.

**3. Lấy Public URL (Cloudflare Tunnel) cho Frontend:**
Dự án đã được tích hợp sẵn hệ thống Public URL tự động bằng Cloudflare Tunnel. Sau khi khởi chạy `docker-compose up -d`, đường hầm sẽ được mở. Để lấy Public URL cung cấp cho màn hình Đăng nhập của React Frontend, bạn mở Terminal và xem log của Tunnel:
```bash
docker logs capstone_tunnel
```
Lướt tìm những dòng cuối cùng, bạn sẽ thấy 1 đường link màu xanh có dạng `https://xxxx-xxxx-xxxx.trycloudflare.com`. 
**Lưu ý:** Lõi C# Backend đã cấu hình mở khóa 100% Policy CORS (`builder.Services.AddCors(...)`), do đó bạn yên tâm sử dụng Link Cloudflare này để fetch bằng Axios mà không bị rào cản.

**4. Tắt hệ thống khi không dùng đến:**
```bash
docker compose down
```

---

## 🧪 Hướng Dẫn Kịch Bản Test Tự Động (Postman Collection)

1. Mở ứng dụng **Postman**.
2. Nhấn nút **Import**, kéo thả file `postman_collection.json` vào Postman.
3. Chọn thẻ Collection mới tên `Capstone Project Review Registration Tool v2`.
4. Chuột phải vào tên Collection và chọn **Run Collection**.

**💡 Cấu trúc Pipeline của bộ Test:**
Khác với kiến trúc cũ, bộ Postman này chạy dạng **Pipeline Đồng bộ**. 
Ngay tại **Test Request số 0 (Zero-Step)**, Postman sẽ gọi API Reset Database để dọn sạch Database, Seed lại Data Mẫu (5 Team, 2 GV, 3 Slot).
Sau đó nó mới liên hoàn nạp 9 Request đăng ký (Booking, Master Data, Auto Schedule, Config) tiếp nối đè lồng lên nhau mà không gây ra bất cứ một Race Condition nào!

## 🗂 Cấu trúc Mã Nguồn (Clean Architecture 3-Layer)
- `CapstoneReview.API` (Presentations Level): Interface cho client, config Dependency Injection, HTTP Endpoints, Data Annotations.
- `CapstoneReview.Service` (Business Rules Level): Lõi logic xử lý Đăng ký / Xếp Lịch. Thuật toán phân vùng không rò rỉ ngoại lệ DB.
- `CapstoneReview.Repository` (Data Access Level): UnitOfWork Pattern gom cụm các Repository (Team, Lecturer, Slot) tương tác EF Core SQL Server.
