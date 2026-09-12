//Abstract Factory
interface MonAnFactory
    {
        HuTieu LayToHuTieu();
        My LayToMy();
    }

//Abstract Product
   interface HuTieu
    {
        string GetModelDetails();
    }

   interface My
    {
        string GetModelDetails();
    }

//Client
  class Client
    {
        HuTieu hutieu;
        My my;
    
        public Client(MonAnFactory factory)
        {
            hutieu = factory.LayToHuTieu();
            my = factory.LayToMy();
        }

        public string GetHuTieuDetails()
        {
            return hutieu.GetModelDetails();
        }

        public string GetMyDetails()
        {
            return my.GetModelDetails();
        }
    }

//Concrete Factory
 class LoaiGioFactory : MonAnFactory
    {
        public HuTieu LayToHuTieu()
        {
            return new HuTieuGio();
        }

        public My LayToMy()
        {
            return new MyGio();
        }
    }


 class LoaiNacFactory : MonAnFactory
    {
        public HuTieu LayToHuTieu()
        {
            return new HuTieuNac();
        }

        public My LayToMy()
        {
            return new MyNac();
        }
    }

//Product
  class HuTieuNac : HuTieu
    {
        public string GetModelDetails()
        {
            return "HU TIEU NAC cua em day";
        }
    }
    
     class HuTieuGio : HuTieu
    {
        public string GetModelDetails()
        {
            return "HU TIEU GIO cua em day";
        }
    }

    class MyNac : My
    {
        public string GetModelDetails()
        {
            return "MY NAC cua em day";
        }
    }
    
    class MyGio : My
    {
        public string GetModelDetails()
        {
            return "MY GIO cua em day";
        }
    }

//Hàm main
 class Program
    {
        static void Main(string[] args)
        {
            MonNuocFactory loaiNac = new LoaiNacFactory();
            Client NacClient = new Client(loaiNac);
            MonNuocFactory loaiGio = new LoaiGioFactory();
            Client GioClient = new Client(loaiGio);

            Console.WriteLine("********* HU TIEU **********");
            Console.WriteLine(NacClient.GetHuTieuDetails());
            Console.WriteLine(GioClient.GetHuTieuDetails());

            Console.WriteLine("******* MY **********");
            Console.WriteLine(NacClient.GetMyDetails());
            Console.WriteLine(GioClient.GetMyDetails());

            Console.ReadKey();
        }
    }

