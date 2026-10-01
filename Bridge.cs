    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Tao doi tuong");
            var blue_color = new Blue();
            var red_color = new Red();
            Square blue_square = new Square { color = blue_color };
            Square red_square = new Square { color = red_color };
            Circle blue_circle = new Circle { color = blue_color };
            Circle red_circle = new Circle { color = red_color };

            Console.WriteLine($"Mau hinh chu nhat xanh: {blue_square.GetColor()}");
            Console.WriteLine($"Mau hinh chu nhat do: {red_square.GetColor()}");
            Console.WriteLine($"Mau hinh tron xanh: {blue_circle.GetColor()}");
            Console.WriteLine($"Mau hinh tron do: {red_circle.GetColor()}");
        }
    }

    interface Color
    {
        string GetColor();
    }

    class Blue : Color
    {
        public string GetColor()
        {
            return "Blue";
        }
    }

    class Red : Color
    {
        public string GetColor()
        {
            return "Red";
        }
    }

    abstract class Shape
    {
        public Color color { get; set; }

        public string GetColor()
        {
            return color.GetColor();
        }
    }

    class Square : Shape
    {

    }

    class Circle : Shape
    {

    }


public interface IDevice { void TurnOn(); }
public class TV : IDevice { public void TurnOn() =&gt; Console.WriteLine("TV ON"); }

// Abstraction
public class RemoteControl {
    protected IDevice device; // Cầu nối
    public RemoteControl(IDevice device) { this.device = device; } // Injection
    public virtual void TogglePower() { device.TurnOn(); }
}


public abstract class AutoShape {
    protected Color color;
    public AutoShape(string colorType) {
        // Tự động dùng Factory để lấy Implementor phù hợp
        color = ColorFactory.CreateColor(colorType);
    }
}


public interface IWindowImp { void DevDrawLine(); }

// Hiện tại chỉ có 1 lớp cài đặt duy nhất cho Windows
public class WinWindowImp : IWindowImp {
    public void DevDrawLine() =&gt; Console.WriteLine("Draw line on Windows");
}

public class Window {
    protected IWindowImp imp = new WinWindowImp(); // Mặc định 1 lớp
}