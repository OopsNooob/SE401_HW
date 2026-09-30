    //Component
    public interface ICar
    {
        ICar ManufactureCar();
    }   

    //ConcreteComponent
    public class BMWCar : ICar
    {
        private string CarName = "BMW";
        public string CarBody { get; set; }
        public string CarDoor { get; set; }
        public string CarWheels { get; set; }
        public string CarGlass { get; set; }
        public string Engine { get; set; }
        public override string ToString()
        {
            return "BMWCar [CarName=" + CarName + ", CarBody=" + CarBody + ", CarDoor=" + CarDoor + ", CarWheels="
                            + CarWheels + ", CarGlass=" + CarGlass + ", Engine=" + Engine + "]";
        }
        public ICar ManufactureCar()
        {
            CarBody = "carbon fiber material";
            CarDoor = "4 car doors";
            CarWheels = "6 car glasses";
            CarGlass = "4 MRF wheels";
            return this;
        }
    }   


    //Decorator
    public abstract class CarDecorator : ICar
    {
        protected ICar car;
        public CarDecorator(ICar car)
        {
            this.car = car;
        }
        public virtual ICar ManufactureCar()
        {
            return car.ManufactureCar();
        }
    }   

    //ConcreteDecorator
    public class DieselCarDecorator : CarDecorator
    {
        public DieselCarDecorator(ICar car) : base(car)
        {
        }
        public override ICar ManufactureCar()
        {
            car.ManufactureCar();
            AddEngine(car);
            return car;
        }
        public void AddEngine(ICar car)
        {
            if (car is BMWCar)
            {
                BMWCar BMWCar = (BMWCar)car;
                BMWCar.Engine = "Diesel Engine";
                Console.WriteLine("DieselCarDecorator added Diesel Engine to the Car : " + car);
            }
        }
    }
    

    //ConcreteDecorator
    class PetrolCarDecorator : CarDecorator
    {
        public PetrolCarDecorator(ICar car) : base(car)
        {
        }
        public override ICar ManufactureCar()
        {
            car.ManufactureCar();
            AddEngine(car);
            return car;
        }
        public void AddEngine(ICar car)
        {
            if (car is BMWCar)
            {
                BMWCar BMWCar = (BMWCar)car;
                BMWCar.Engine = "Petrol Engine";
                Console.WriteLine("PetrolCarDecorator added Petrol Engine to the Car : " + car);
            }
        }
    }


    //Client
    class Client
    {
        static void Main(string[] args)
        {
            ICar bmwCar1 = new BMWCar();
            bmwCar1.ManufactureCar();
            Console.WriteLine(bmwCar1 + "\n");
            DieselCarDecorator carWithDieselEngine = new DieselCarDecorator(bmwCar1);
            carWithDieselEngine.ManufactureCar();
            Console.WriteLine();
            ICar bmwCar2 = new BMWCar();
            PetrolCarDecorator carWithPetrolEngine = new PetrolCarDecorator(bmwCar2);
            carWithPetrolEngine.ManufactureCar();
            Console.ReadKey();
        }
    }   


//1. Decorator Cổ điển (Classic Decorator - Chuẩn GoF)

```
public interface INotifier { void Send(string msg); }

// Component gốc
public class EmailNotifier : INotifier {
    public void Send(string msg) =&gt; Console.WriteLine($"Email: {msg}");
}

// 1. Base Decorator Class
public abstract class NotifierDecorator : INotifier {
    protected INotifier _wrapper;
    public NotifierDecorator(INotifier notifier) { _wrapper = notifier; }
    public virtual void Send(string msg) =&gt; _wrapper.Send(msg);
}

// 2. Concrete Decorator
public class SMSDecorator : NotifierDecorator {
    public SMSDecorator(INotifier notifier) : base(notifier) {}
    public override void Send(string msg) {
        base.Send(msg); // Gọi lớp bọc bên trong
        Console.WriteLine($"SMS: {msg}"); // Bổ sung tính năng mới
    }
}


// Bỏ qua lớp Abstract Base Decorator
public class LoggingCarDecorator : ICar {
    private readonly ICar _innerCar; // Bọc trực tiếp
    public LoggingCarDecorator(ICar car) { _innerCar = car; }

    public ICar ManufactureCar() {
        Console.WriteLine("[LOG]: Dang san xuat xe...");
        return _innerCar.ManufactureCar(); // Chuyển tiếp
    }
}


public static class FunctionalDecorator {
    // Bọc thêm tính năng Logging cho một Action
    public static Action<string> WithLogging(Action<string> action) {
        return message => {
            Console.WriteLine("[LOG START]");
            action(message); // Gọi hàm gốc
            Console.WriteLine("[LOG END]");
        };
    }
}

// Sử dụng tại Client:
Action<string> sendEmail = msg => Console.WriteLine($"Email: {msg}");
Action<string> sendWithLog = FunctionalDecorator.WithLogging(sendEmail);
sendWithLog("Thong bao hop khan");
