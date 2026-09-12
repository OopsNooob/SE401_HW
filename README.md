# SE401 - Design Patterns in C# 🎯

Kho lưu trữ này chứa các ví dụ minh họa về Design Patterns (Mẫu thiết kế) được cài đặt bằng ngôn ngữ C#. Đây là mã nguồn và tài liệu phục vụ cho môn học **SE401**.

## 📁 Cấu trúc thư mục và tài liệu

- **`01. Gioi thieu Design Patterns.pdf`**: Tài liệu bài giảng giới thiệu tổng quan về Design Patterns, lý do sử dụng và phân loại các mẫu thiết kế cơ bản.
- **`FactoryMethod.cs`**: Mã nguồn minh họa mẫu thiết kế **Factory Method**.
- **`AbstractFactory.cs`**: Mã nguồn minh họa mẫu thiết kế **Abstract Factory**.

## 🛠️ Chi tiết các mẫu thiết kế

### 1. Factory Method (`FactoryMethod.cs`)
Mẫu thiết kế **Factory Method** được minh họa thông qua bài toán tạo các loại Pizza. 
- Thay vì khởi tạo trực tiếp các đối tượng bằng từ khóa `new` dựa trên các câu lệnh `if/else` chằng chịt, hệ thống định nghĩa một `PizzaFactory` nhận vào tham số là loại Pizza (enum) và trả về interface `IPizza` tương ứng (`HamAndMushroomPizza`, `DeluxePizza`, hoặc `SeafoodPizza`).
- **Ưu điểm**: Giúp đóng gói logic khởi tạo đối tượng, dễ dàng bảo trì và mở rộng thêm các loại Pizza mới sau này.

### 2. Abstract Factory (`AbstractFactory.cs`)
Mẫu thiết kế **Abstract Factory** được minh họa qua hệ thống tạo các loại món ăn nước (Hủ tiếu, Mỳ) với các lựa chọn nguyên liệu (Nạc, Giò).
- **Abstract Product**: `HuTieu`, `My`.
- **Concrete Product**: `HuTieuNac`, `HuTieuGio`, `MyNac`, `MyGio`.
- **Abstract Factory**: Interface `MonAnFactory` định nghĩa các thao tác `LayToHuTieu()`, `LayToMy()`.
- **Concrete Factory**: `LoaiNacFactory` (chuyên tạo các món liên quan đến nạc) và `LoaiGioFactory` (chuyên tạo các món liên quan đến giò).
- **Client**: Tương tác với hệ thống thông qua Interface của Abstract Factory để lấy món ăn, mà không cần bận tâm đến việc đối tượng cụ thể nào đang được khởi tạo.

## 🚀 Hướng dẫn chạy thử nghiệm
1. Bạn cần chuẩn bị môi trường chạy C# (ví dụ: cài đặt .NET SDK hoặc sử dụng Visual Studio).
2. Các file `FactoryMethod.cs` và `AbstractFactory.cs` đều chứa một `class Program` có hàm `Main()` độc lập bên trong.
3. Để chạy thử, bạn có thể tạo một dự án **Console Application** mới trong Visual Studio, sau đó sao chép nội dung của một trong hai file vào và chạy (Run) để xem kết quả xuất ra màn hình Console.

## 📄 Bản quyền
Dự án được phân phối dưới các điều khoản trong file `LICENSE`.
