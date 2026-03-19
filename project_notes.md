# Ghi Chú Dự Án

> **Ngày bắt đầu:** 02/02/2026

---

## 📋 Thông Tin Chung

| Mục | Chi tiết |
|-----|----------|
| Tên dự án | **Capstone Project Review Registration Tool** |
| Môn học | PRN232 |
| Loại dự án | Assignment |

---

## 💡 Ý Tưởng & Mô Tả

Hệ thống quản lý đăng ký lịch Review đồ án Capstone Project, giúp:
- Sinh viên (nhóm) đăng ký slot review
- Giảng viên Review đăng ký slot tham gia chấm
- Moderator quản lý và thiết lập ràng buộc

---

## 👥 Các Vai Trò (Roles)

| Role | Mô tả |
|------|-------|
| **Sinh viên / Nhóm** | Đăng ký N slot trống có thể tham gia Review |
| **GVHD (Giảng viên Hướng dẫn)** | Không cần đăng ký, tự động được gán theo nhóm |
| **GV Review (Giảng viên Review)** | Đăng ký các slot trống để tham gia chấm |
| **Moderator / Admin** | Quản lý hệ thống, thiết lập Min-Max slot cho GV |

---

## 🗃️ Data Model (Dữ Liệu Có Sẵn Trong DB)

```
Lecturer (GVHD)
    │
    ├── Đề tài 1 ──▶ Team A (members + 1 Leader)
    ├── Đề tài 2 ──▶ Team B (members + 1 Leader)
    └── Đề tài 3 ──▶ Team C (members + 1 Leader)
```

| Entity | Mô tả |
|--------|--------|
| **Sinh viên** | Danh sách SV có sẵn |
| **Nhóm (Team)** | Thành viên + **1 Leader** |
| **Lecturer** | Danh sách GV |
| **Đề tài (Topic)** | Thuộc về Lecturer, mỗi đề tài chỉ **1 nhóm được nhận** |

> ℹ️ **Nhận biết GVHD**: Qua quan hệ **Lecturer → Đề tài → Team**

---

## ⚙️ Cấu Hình Hệ Thống

| Thông số | Giá trị |
|----------|---------|
| Thời lượng review mỗi đề tài | **Tối đa 45 phút** |
| Số slot mỗi ngày | **4 slots** |
| Số đề tài tối đa/slot | **3 đề tài** |
| Số GV Review/slot | **2 GV** |
| Số thành viên/nhóm | **5-6 người** (review theo nhóm) |
| Min slot GV Review | **2 slots** *(mặc định, Moderator có thể đổi)* |
| Max slot GV Review | **5 slots** *(mặc định, Moderator có thể đổi)* |

---

## 📌 Quy Tắc Nghiệp Vụ (Business Rules)

1. **Mỗi kì có 3 lần Review** (Review 1, Review 2, Review 3)
2. **Mỗi Slot chỉ có tối đa 3 đề tài** được review
3. **Leader của nhóm đăng ký slot** (không phải thành viên)
4. **Lecturer đăng ký slot** mà họ muốn tham gia review
5. **Moderator thiết lập Min-Max slot** (min: 2, max: 5) cho Lecturer
6. ⚠️ **Lecturer KHÔNG được review đề tài mình hướng dẫn** (conflict of interest)
7. **Thông báo qua email** đến từng SV trong nhóm + Lecturer sau khi xếp lịch
8. **Xử lý Lecturer đăng ký chưa đủ slot:**
   - **1 tuần trước deadline**: Gửi email nhắc nhở đến **toàn bộ Lecturer** (cả đã đăng ký đủ và chưa đủ)
   - **Sau deadline**: Tổng hợp danh sách Lecturer đăng ký chưa đủ min slot + **số buổi thiếu** → Gửi báo cáo cho Moderator

---

## 🤖 Quy Trình Xếp Lịch Tự Động

### Luồng xếp lịch:
```
Team + Lecturer đăng ký slot
        ↓
  Deadline đăng ký
        ↓
  Hệ thống chạy thuật toán xếp lịch
        ↓
  Gửi kết quả cho Moderator duyệt
        ↓
  Moderator phê duyệt / điều chỉnh
        ↓
  Publish lịch chính thức
```

### Quy tắc xếp lịch:
1. **Ưu tiên theo thời gian đăng ký** - Ai đăng ký sớm được ưu tiên
2. **Kiểm tra conflict** - Lecturer không được xếp vào slot có đề tài mình hướng dẫn
3. **Đảm bảo Min-Max** - Mỗi Lecturer phải có ít nhất min slot, tối đa max slot (Moderator cấu hình)
4. **Tối ưu hóa** - Cố gắng xếp đủ 3 đề tài/slot
5. **Xử lý overflow** - Nếu không xếp đủ (quá nhiều team/thiếu lecturer) → **Thông báo Moderator**

### Vai trò Moderator:
- Thiết lập **Min-Max slot** cho Lecturer
- **Duyệt lịch** sau khi hệ thống xếp tự động
- Có thể điều chỉnh thủ công nếu cần

---

## 🎯 Mục Tiêu

- [ ] Xây dựng hệ thống đăng ký slot Review
- [ ] Quản lý nhóm, GV, và lịch Review
- [ ] Tự động kiểm tra conflict (GVHD ≠ GV Review)
- [ ] Hỗ trợ ràng buộc Min-Max slot cho GV

---

## 🛠️ Công Nghệ & Kiến Trúc

### Yêu Cầu Môn Học PRN232
> ⚠️ **Bắt buộc**: FE và BE phải tách riêng, kết nối qua API

### Ngôn ngữ & Framework
- **Backend:** C# + ASP.NET Core Web API
- **Frontend:** React + JavaScript + Axios
- **Database:** SQL Server
- **Real-time:** SignalR (thông báo/reminder)

### Deployment: Docker
> 🐳 **Tất cả FE, BE, DB đều chạy trên Docker**

```yaml
# docker-compose.yml (dự kiến)
services:
  frontend:       # React app (port 3000)
  backend:        # ASP.NET Core API (port 5000)
  database:       # SQL Server (port 1433)
```

---

### Kiến trúc: 3-Layer Architecture

```
┌─────────────────────────────────────┐
│         API Layer (Controllers)     │  ← Xử lý HTTP Request/Response
├─────────────────────────────────────┤
│         Service Layer (BLL)         │  ← Business Logic, Validation
├─────────────────────────────────────┤
│       Repository Layer (DAL)        │  ← Data Access, Entity Framework
├─────────────────────────────────────┤
│            Database                 │  ← SQL Server
└─────────────────────────────────────┘
```

---

### Cấu Trúc Backend
```
CapstoneReview/
├── CapstoneReview.API/
│   ├── Controllers/
│   ├── Models/
│   │   ├── RequestModels/
│   │   └── ResponseModels/
│   └── Extensions/              # DI registration helpers
│       ├── ServiceExtensions.cs
│       └── RepositoryExtensions.cs
├── CapstoneReview.Service/
│   ├── Services/                # ⚠️ Tất cả Business Rules xử lý ở đây
│   └── Interfaces/
└── CapstoneReview.Repository/
    ├── Data/
    │   ├── DbContext/           # ApplicationDbContext
    │   └── Migrations/          # EF Core Migrations
    ├── Entities/                # Entity classes (full DB mapping)
    ├── Models/                  # Partial data từ Entities cho Service
    ├── Mappers/                 # Map Entities ↔ Models
    ├── Interfaces/              # Repository interfaces
    └── Repositories/            # Repository implementations
```

---

### Cấu Trúc Frontend (React)
```
capstone-review-fe/
├── public/
├── src/
│   ├── api/                    # Axios instances, API calls
│   │   ├── axiosClient.js      # Axios config + interceptors
│   │   ├── authApi.js          # Login, refresh token
│   │   ├── slotApi.js          # Slot CRUD
│   │   └── ...
│   ├── components/             # Reusable UI components
│   │   ├── common/             # Button, Modal, Table...
│   │   └── layout/             # Header, Sidebar, Footer
│   ├── pages/                  # Page components
│   │   ├── auth/               # Login, Register
│   │   ├── student/            # Student dashboard, slot booking
│   │   ├── lecturer/           # Lecturer slot registration
│   │   └── moderator/          # Admin panel, schedule approval
│   ├── contexts/               # React Context (Auth, Theme...)
│   ├── hooks/                  # Custom hooks
│   ├── utils/                  # Helper functions
│   ├── routes/                 # Route configs
│   ├── App.js
│   └── index.js
├── .env                        # API URL, configs
└── package.json
```

---

### Authentication Strategy

| Giai đoạn | Phương thức |
|-----------|-------------|
| **Testing** | Tài khoản ảo tạo trên DB |
| **Production** | Google OAuth (@fpt.edu.vn) |

**JWT Token Strategy:**
| Token Type | Expiry | Storage |
|------------|--------|---------|
| **Access Token** | 15-30 phút | Frontend (memory/localStorage) |
| **Refresh Token** | 7-30 ngày | Database |

**Luồng hoạt động:**
```
Login → Nhận Access Token + Refresh Token
    ↓
API Request (Bearer Access Token)
    ↓
Access Token hết hạn → Gọi /refresh với Refresh Token
    ↓
Nhận Access Token mới → Tiếp tục sử dụng
```

---

## 📝 Ghi Chú Thảo Luận

### Buổi 1 - 02/02/2026

**Nội dung đã thảo luận:**
- Xác định ý tưởng: Capstone Project Review Registration Tool
- Liệt kê 4 vai trò: Sinh viên, GVHD, GV Review, Moderator
- Xác định 6 quy tắc nghiệp vụ chính

---

## ❓ Câu Hỏi Cần Làm Rõ

### ✅ Đã Trả Lời
- ~~Thời lượng review mỗi đề tài?~~ → **Tối đa 45 phút**
- ~~Số thành viên/nhóm?~~ → **5-6 người**
- ~~Số đề tài tối đa/slot?~~ → **3 đề tài**
- ~~Min-Max slot cho Lecturer?~~ → **min: 2, max: 5**
- ~~Ai xếp lịch cuối cùng?~~ → **Hệ thống tự động**, ưu tiên đăng ký sớm
- ~~Khi nào chạy thuật toán xếp lịch?~~ → **Sau deadline đăng ký**
- ~~Moderator làm gì?~~ → **Duyệt lịch** sau khi hệ thống xếp tự động
- ~~PRN232 yêu cầu gì?~~ → **FE-BE tách riêng, kết nối qua API**
- ~~Frontend sử dụng gì?~~ → **React + JavaScript + Axios**
- ~~Authentication?~~ → **Test: tài khoản ảo DB | Production: Google OAuth @fpt.edu.vn**
- ~~Thư viện bên ngoài?~~ → **✅ Được phép sử dụng**
- ~~Deadline?~~ → **Còn dài, không cần lo**
- ~~Thông báo/reminder?~~ → **✅ Có, real-time với SignalR**
- ~~Không xếp đủ lịch?~~ → **Thông báo Moderator để xử lý**
- ~~Nhận biết GVHD?~~ → **Qua quan hệ Lecturer → Đề tài → Team**
- ~~Lecturer đăng ký đề tài hướng dẫn?~~ → **Không cần**, đã có sẵn trong DB
- ~~SV đăng ký/chọn đề tài?~~ → **Không cần**, mỗi đề tài đã gán sẵn 1 nhóm
- ~~Số GV Review/slot?~~ → **2 GV**
- ~~Số slot mỗi ngày?~~ → **4 slots**
- ~~Đổi/hủy slot sau đăng ký?~~ → **Được đổi trước Deadline**, sau khi có lịch chính thức thì không được đổi
- ~~SV/GV vắng mặt buổi review?~~ → **Moderator xử lý**, có quyền xếp lịch lại và phân GV
- ~~Cần điểm danh?~~ → **Không cần**, phần mềm chỉ dùng để đăng ký xếp lịch
- ~~Lecturer không đăng ký đủ min slot?~~ → **1 tuần trước deadline**: gửi email nhắc nhở toàn bộ Lecturer (kể cả đã đăng ký đủ). **Sau deadline**: tổng hợp danh sách Lecturer thiếu + số buổi thiếu cho Moderator
- ~~Lưu kết quả chấm điểm/feedback?~~ → **Không cần**, nằm ngoài phạm vi phần mềm đăng ký lịch
- ~~Biên bản review?~~ → **Không cần**, nằm ngoài phạm vi phần mềm đăng ký lịch
- ~~Reminder trước buổi review?~~ → **Trước 1 ngày**, nếu lịch quá gần deadline thì remind ngay khi có lịch
- ~~Thông báo khi Moderator điều chỉnh lịch?~~ → **Thông báo ngay lập tức** cho team bị điều chỉnh và GV Review
- ~~Kênh thông báo?~~ → **Cả Email và in-app notification**
- ~~Leader ủy quyền cho thành viên khác đăng ký?~~ → **Không được phép**, chỉ Leader mới có quyền đăng ký
- ~~Nhiều Moderator?~~ → **Chỉ có 1 tài khoản Moderator**
- ~~Moderator ép assign GV vào slot?~~ → **Có quyền** phân GV vào slot họ không đăng ký
- ~~Team đăng ký nhiều slot?~~ → **Có**, team đăng ký tất cả các slot rảnh trong khoảng thời gian review, hệ thống sẽ xếp 1 buổi
- ~~Nhóm không đăng ký slot trước deadline?~~ → **Tổng hợp danh sách gửi Moderator** để xử lý
- ~~Moderator gia hạn deadline?~~ → **Có quyền** gia hạn thời gian đăng ký review cho team/toàn bộ
- ~~Giao diện đăng ký slot?~~ → **Calendar view**
- ~~Dashboard Moderator?~~ → **Có**, gồm: Overview cards, Tiến độ Review, Calendar slot (trạng thái slot), Alerts, Quick Actions
- ~~Dark mode?~~ → **Không cần**
- ~~Mobile responsive hay chỉ Desktop?~~ → **Chạy trên web** (Desktop)
- ~~Lịch sử review của từng nhóm?~~ → **Có**, hệ thống lưu lịch đăng ký review của từng nhóm + lịch review của Lecturer theo từng kỳ, khi cần tra cứu thì lấy dữ liệu từ DB xuất ra
- ~~Báo cáo tổng hợp cuối kỳ?~~ → **Có**, hệ thống tự động lấy dữ liệu đã lưu, xuất báo cáo tổng hợp theo từng kỳ và lưu lại

### ❌ Chưa Trả Lời

**Tính năng bổ sung:**
1. Có cần export lịch (PDF, Excel)?

**Cấu trúc BE:**
2. Có cần Middlewares (Exception handling, Logging)?
3. Enums (ReviewRound, SlotStatus) để ở đâu?
4. Custom Exceptions để ở đâu?

---

**Edge Cases & Ngoại lệ:**
18. Nếu **không xếp đủ lịch** do thiếu GV/slot thì sao?

---

**Lịch sử & Audit:**
26. Có cần **log lịch sử** các thay đổi không? (ai đăng ký/hủy slot lúc nào)
27. Có cần **lịch sử chỉnh sửa** của Moderator không?

---