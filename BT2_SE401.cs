// =============================================================================
//  BT2 - MẪU THIẾT KẾ: COMPOSITE (chính) + SINGLETON (bổ trợ)
//  Bài toán: cơ cấu nhân sự công ty dạng cây (CEO -> VP -> Trưởng nhóm -> NV)
//  và yêu cầu tính lương GetSalary() đồng nhất cho mọi chức danh.
//
//  Cách chạy nhanh:
//     dotnet new console -n BT2 && thay nội dung Program.cs bằng file này
//     dotnet run
//  (hoặc dán thẳng vào https://dotnetfiddle.net, chọn C# / .NET 6+)
// =============================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BT2_CompanyHierarchy
{
    // =========================================================================
    // 1. COMPONENT - Employee
    //    Giao diện chung cho CẢ nhân viên đơn lẻ lẫn cấp quản lý.
    //    Client chỉ làm việc với Employee, không cần biết đối tượng là loại nào.
    // =========================================================================
    public abstract class Employee
    {
        public string Name { get; }
        public string Position { get; }
        public double BaseSalary { get; }

        /// <summary>Cấp trên trực tiếp (null nếu là CEO hoặc chưa được gán).</summary>
        public Employee Parent { get; internal set; }

        protected Employee(string name, string position, double baseSalary)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Tên nhân viên không được để trống.", nameof(name));
            if (string.IsNullOrWhiteSpace(position))
                throw new ArgumentException("Chức danh không được để trống.", nameof(position));
            if (baseSalary < 0)
                throw new ArgumentOutOfRangeException(nameof(baseSalary), "Lương cơ bản không được âm.");

            Name = name;
            Position = position;
            BaseSalary = baseSalary;
        }

        // ---- Phương thức nghiệp vụ mà đề bài yêu cầu -----------------------

        /// <summary>Lương của RIÊNG người này (không tính cấp dưới).</summary>
        public abstract double GetSalary();

        /// <summary>Tổng quỹ lương của người này + toàn bộ nhánh cấp dưới (đệ quy).</summary>
        public abstract double GetTotalSalary();

        /// <summary>Danh sách cấp dưới trực tiếp (Leaf trả về danh sách rỗng).</summary>
        public abstract List<Employee> GetSubordinates();

        // ---- Quản lý cây phân cấp: mặc định KHÔNG hỗ trợ (dành cho Leaf) ----

        public virtual void Add(Employee employee)
        {
            throw new InvalidOperationException(
                $"'{Position} - {Name}' là nhân viên cơ sở, không thể thêm cấp dưới.");
        }

        public virtual void Remove(Employee employee)
        {
            throw new InvalidOperationException(
                $"'{Position} - {Name}' là nhân viên cơ sở, không có cấp dưới để xóa.");
        }

        // ---- Tiện ích duyệt cây -------------------------------------------

        /// <summary>Duyệt toàn bộ cây (gồm chính nó) theo thứ tự trước (pre-order).</summary>
        public IEnumerable<Employee> Flatten()
        {
            yield return this;
            foreach (Employee child in GetSubordinates())
                foreach (Employee descendant in child.Flatten())
                    yield return descendant;
        }

        /// <summary>Độ sâu trong cây: CEO = 0, VP = 1, ...</summary>
        public int GetLevel()
        {
            int level = 0;
            for (Employee p = Parent; p != null; p = p.Parent) level++;
            return level;
        }

        // ---- In sơ đồ tổ chức dạng cây -------------------------------------

        public void Display()
        {
            Console.WriteLine(GetInfo());
            DisplaySubordinates("");
        }

        internal abstract string GetInfo();

        // Leaf không có cấp dưới nên mặc định không in gì thêm
        internal virtual void DisplaySubordinates(string prefix) { }
    }

    // =========================================================================
    // 2. LEAF - Staff
    //    Nhân viên cơ sở: NV Quảng cáo, NV Kế toán, NV Vận chuyển, ...
    //    Không có cấp dưới -> mọi phép tính dừng lại ở chính nó.
    // =========================================================================
    public class Staff : Employee
    {
        public Staff(string name, string position, double baseSalary)
            : base(name, position, baseSalary)
        {
        }

        public override double GetSalary() => BaseSalary;

        // Leaf: tổng quỹ lương chính là lương của bản thân
        public override double GetTotalSalary() => GetSalary();

        public override List<Employee> GetSubordinates() => new List<Employee>();

        internal override string GetInfo() =>
            $"[NV] {Position}: {Name} | Lương: {GetSalary():N0} VND";
    }

    // =========================================================================
    // 3. COMPOSITE - Manager
    //    Cấp quản lý (VP, Trưởng nhóm, ...) chứa danh sách Employee cấp dưới.
    //    Cấp dưới có thể là Staff HOẶC một Manager khác -> cây lồng nhau tùy ý.
    // =========================================================================
    public class Manager : Employee
    {
        private readonly List<Employee> _subordinates = new List<Employee>();

        /// <summary>Phụ cấp trách nhiệm tính trên mỗi cấp dưới trực tiếp.</summary>
        public double AllowancePerSubordinate { get; }

        public Manager(string name, string position, double baseSalary,
                       double allowancePerSubordinate = 2_000_000)
            : base(name, position, baseSalary)
        {
            if (allowancePerSubordinate < 0)
                throw new ArgumentOutOfRangeException(nameof(allowancePerSubordinate));
            AllowancePerSubordinate = allowancePerSubordinate;
        }

        public override void Add(Employee employee)
        {
            if (employee == null)
                throw new ArgumentNullException(nameof(employee));

            // CEO luôn là gốc của cây, không được làm cấp dưới của ai
            if (employee is CEO)
                throw new InvalidOperationException("CEO là gốc của cây, không thể làm cấp dưới.");

            // Chặn vòng lặp: không thể thêm chính mình hoặc cấp trên của mình làm cấp dưới
            for (Employee p = this; p != null; p = p.Parent)
            {
                if (ReferenceEquals(p, employee))
                    throw new InvalidOperationException(
                        $"Không thể thêm '{employee.Name}' làm cấp dưới của '{Name}' (tạo vòng lặp).");
            }

            // Mỗi người chỉ có một cấp trên trực tiếp
            if (employee.Parent != null)
                throw new InvalidOperationException(
                    $"'{employee.Name}' đã thuộc quyền quản lý của '{employee.Parent.Name}'.");

            _subordinates.Add(employee);
            employee.Parent = this;
        }

        public override void Remove(Employee employee)
        {
            if (employee != null && _subordinates.Remove(employee))
                employee.Parent = null;
        }

        // Trả về BẢN SAO để bên ngoài không sửa trực tiếp danh sách nội bộ
        public override List<Employee> GetSubordinates() => new List<Employee>(_subordinates);

        // Lương quản lý = lương cứng + phụ cấp trách nhiệm theo số cấp dưới trực tiếp
        public override double GetSalary() =>
            BaseSalary + _subordinates.Count * AllowancePerSubordinate;

        // Đệ quy ủy nhiệm: mỗi cấp dưới tự tính tổng của nhánh mình
        // (Staff trả về lương của nó, Manager lại đệ quy tiếp) -> không cần if/else kiểm tra loại
        public override double GetTotalSalary() =>
            GetSalary() + _subordinates.Sum(s => s.GetTotalSalary());

        internal override string GetInfo() =>
            $"[QL] {Position}: {Name} | Lương: {GetSalary():N0} VND | Quỹ lương nhánh: {GetTotalSalary():N0} VND";

        internal override void DisplaySubordinates(string prefix)
        {
            for (int i = 0; i < _subordinates.Count; i++)
            {
                bool isLast = i == _subordinates.Count - 1;
                Employee child = _subordinates[i];

                Console.WriteLine($"{prefix}{(isLast ? "└── " : "├── ")}{child.GetInfo()}");
                child.DisplaySubordinates(prefix + (isLast ? "    " : "│   "));
            }
        }
    }

    // =========================================================================
    // 4. SINGLETON + COMPOSITE - CEO
    //    "Hiển nhiên anh ta là CEO": cả công ty chỉ có đúng một CEO.
    //    CEO là một Manager đặc biệt (đỉnh cây) nhưng chỉ được tạo qua GetInstance().
    // =========================================================================
    public sealed class CEO : Manager
    {
        private static volatile CEO _instance;
        private static readonly object _lock = new object();

        // Constructor private: bên ngoài không thể dùng "new CEO(...)"
        private CEO(string name, double baseSalary)
            : base(name, "Tổng Giám Đốc (CEO)", baseSalary, allowancePerSubordinate: 3_000_000)
        {
        }

        /// <summary>
        /// Lấy CEO duy nhất. Lần gọi đầu tiên tạo đối tượng với tên/lương truyền vào;
        /// các lần sau trả về đúng đối tượng cũ và BỎ QUA tham số.
        /// Dùng double-checked locking để an toàn khi chạy đa luồng.
        /// </summary>
        public static CEO GetInstance(string name = "Nguyễn Văn Sáng Lập",
                                      double baseSalary = 50_000_000)
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                        _instance = new CEO(name, baseSalary);
                }
            }
            return _instance;
        }
    }

    // =========================================================================
    // 5. CLIENT - mô phỏng bài toán
    //    Chỉ dùng kiểu Employee/Manager/Staff qua giao diện chung.
    // =========================================================================
    internal static class Program
    {
        private static void PrintHeader(string title)
        {
            Console.WriteLine();
            Console.WriteLine(new string('=', 80));
            Console.WriteLine(title);
            Console.WriteLine(new string('=', 80));
        }

        private static void Main()
        {
            Console.OutputEncoding = Encoding.UTF8;

            // -----------------------------------------------------------------
            PrintHeader("BƯỚC 1: SINGLETON - CHỈ TỒN TẠI MỘT CEO");
            // -----------------------------------------------------------------
            CEO ceo = CEO.GetInstance("Trần Đình Khang", 60_000_000);
            CEO ceoKhac = CEO.GetInstance("Người Khác", 1); // tham số bị bỏ qua

            Console.WriteLine($"ceo      -> {ceo.Name}");
            Console.WriteLine($"ceoKhac  -> {ceoKhac.Name}");
            Console.WriteLine($"Hai biến trỏ cùng một đối tượng? {ReferenceEquals(ceo, ceoKhac)}");

            // -----------------------------------------------------------------
            PrintHeader("BƯỚC 2: DỰNG CÂY NHÂN SỰ (COMPOSITE)");
            // -----------------------------------------------------------------
            // Cấp 1: hai Phó Giám Đốc (VP) dưới CEO
            Manager vpMarketing = new Manager("Lê Thị Mai", "VP Marketing", 35_000_000);
            Manager vpSanXuat = new Manager("Phạm Văn Bình", "VP Sản Xuất", 35_000_000);
            ceo.Add(vpMarketing);
            ceo.Add(vpSanXuat);

            // Nhánh Marketing: Trưởng nhóm Quảng cáo (2 NV) + 1 NV Kinh doanh
            Manager truongNhomQC = new Manager("Hoàng Văn Quang", "Trưởng Nhóm Quảng Cáo", 22_000_000);
            Staff nvQuangCao1 = new Staff("Nguyễn Văn An", "NV Quảng Cáo", 12_000_000);
            Staff nvQuangCao2 = new Staff("Bùi Thị Hoa", "NV Quảng Cáo", 11_000_000);
            Staff nvKinhDoanh = new Staff("Đặng Minh Tuấn", "NV Kinh Doanh", 13_000_000);

            truongNhomQC.Add(nvQuangCao1);
            truongNhomQC.Add(nvQuangCao2);
            vpMarketing.Add(truongNhomQC);
            vpMarketing.Add(nvKinhDoanh);

            // Nhánh Sản xuất: 1 NV Kế toán + Trưởng nhóm Vận chuyển (2 NV)
            Staff nvKeToan = new Staff("Trần Thị Toán", "NV Kế Toán", 14_000_000);
            Manager truongNhomVC = new Manager("Vũ Đức Hải", "Trưởng Nhóm Vận Chuyển", 20_000_000);
            Staff nvVanChuyen1 = new Staff("Đỗ Văn Chuyển", "NV Vận Chuyển", 11_000_000);
            Staff nvVanChuyen2 = new Staff("Lý Văn Giao", "NV Vận Chuyển", 10_500_000);

            truongNhomVC.Add(nvVanChuyen1);
            truongNhomVC.Add(nvVanChuyen2);
            vpSanXuat.Add(nvKeToan);
            vpSanXuat.Add(truongNhomVC);

            Console.WriteLine("Sơ đồ tổ chức ([QL] = quản lý, [NV] = nhân viên cơ sở):\n");
            ceo.Display();

            // -----------------------------------------------------------------
            PrintHeader("BƯỚC 3: GetSalary() ĐỒNG NHẤT CHO MỌI CHỨC DANH");
            // -----------------------------------------------------------------
            // Một vòng lặp duy nhất, không if/else theo chức danh
            Console.WriteLine(string.Format("{0,-40}{1,-20}{2,15}{3,18}",
                "Chức danh", "Họ tên", "GetSalary()", "GetTotalSalary()"));
            Console.WriteLine(new string('-', 93));

            foreach (Employee e in ceo.Flatten())
            {
                string chucDanh = new string(' ', e.GetLevel() * 2) + e.Position;
                Console.WriteLine(string.Format("{0,-40}{1,-20}{2,15:N0}{3,18:N0}",
                    chucDanh, e.Name, e.GetSalary(), e.GetTotalSalary()));
            }

            // -----------------------------------------------------------------
            PrintHeader("BƯỚC 4: QUỸ LƯƠNG THEO TỪNG PHÒNG BAN (ĐỆ QUY)");
            // -----------------------------------------------------------------
            Console.WriteLine($"Marketing  ({vpMarketing.Name}): {vpMarketing.GetTotalSalary():N0} VND");
            Console.WriteLine($"Sản xuất   ({vpSanXuat.Name}): {vpSanXuat.GetTotalSalary():N0} VND");
            Console.WriteLine($"Toàn công ty: {ceo.GetTotalSalary():N0} VND");

            int tongNhanSu = ceo.Flatten().Count();
            int soNhanVien = ceo.Flatten().Count(e => e.Position.StartsWith("NV"));
            Console.WriteLine($"Tổng nhân sự: {tongNhanSu} người (trong đó {soNhanVien} nhân viên cơ sở)");

            // -----------------------------------------------------------------
            PrintHeader("BƯỚC 5: KIỂM TRA CÁC RÀNG BUỘC CỦA CÂY");
            // -----------------------------------------------------------------
            TryAction("Thêm cấp dưới cho nhân viên cơ sở (Leaf)",
                () => nvKeToan.Add(new Staff("Ai Đó", "NV Thực Tập", 5_000_000)));

            TryAction("Gán CEO làm cấp dưới của người khác",
                () => truongNhomQC.Add(ceo));

            TryAction("Gán VP Marketing làm cấp dưới của Trưởng nhóm (vòng lặp)",
                () => truongNhomQC.Add(vpMarketing));

            TryAction("Một người có hai cấp trên",
                () => vpSanXuat.Add(nvQuangCao1));

            // -----------------------------------------------------------------
            PrintHeader("BƯỚC 6: THAY ĐỔI CƠ CẤU - ĐIỀU CHUYỂN NHÂN SỰ");
            // -----------------------------------------------------------------
            double truoc = ceo.GetTotalSalary();
            Console.WriteLine($"Quỹ lương trước khi điều chuyển: {truoc:N0} VND");

            // Điều chuyển NV Kế toán từ khối Sản xuất sang khối Marketing
            vpSanXuat.Remove(nvKeToan);
            vpMarketing.Add(nvKeToan);

            // Tổng toàn công ty không đổi (hai VP cùng mức phụ cấp), nhưng quỹ lương
            // của từng phòng ban thì thay đổi -> cây tự cập nhật, không cần sửa code tính lương
            double sau = ceo.GetTotalSalary();
            Console.WriteLine($"Quỹ lương sau khi điều chuyển  : {sau:N0} VND (chênh lệch {sau - truoc:N0} VND)");
            Console.WriteLine($"  - Marketing: {vpMarketing.GetTotalSalary():N0} VND");
            Console.WriteLine($"  - Sản xuất : {vpSanXuat.GetTotalSalary():N0} VND\n");
            ceo.Display();

            Console.WriteLine();
            Console.WriteLine(new string('=', 80));
        }

        // Chạy một thao tác và in ra lỗi nghiệp vụ (nếu có) thay vì làm chương trình dừng đột ngột
        private static void TryAction(string moTa, Action action)
        {
            try
            {
                action();
                Console.WriteLine($"[OK ] {moTa}");
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"[LỖI] {moTa}\n       -> {ex.Message}");
            }
        }
    }
}
