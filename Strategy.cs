//Context
    public class ExportContext
    {
        private IExport Export;

        public ExportContext(IExport Export)
        {
            this.Export = Export;
        }
        public void SetStrategy(IExport Export)
        {
            this.Export = Export;
        }
        public void CreateArchive(string fileName)
        {
            Export.ExportFile(fileName);
        }
    }   

//Strategy
    public interface IExport
    {
        void ExportFile(string fileName);
    }   

//Concrete Strategy
    public class ExportJPG : IExport
    {
        public void ExportFile(string fileName)
        {
            Console.WriteLine("Export file: '" + fileName
                 + ".JPG' successfully");
        }
    }
    
    public class ExportPDF : IExport
    {
        public void ExportFile(string fileName)
        {
            Console.WriteLine("Export file: '" + fileName
                 + ".PDF' successfully");
        }
    }
    
    public class ExportPNG : IExport
    {
        public void ExportFile(string fileName)
        {
            Console.WriteLine("Export file: '" + fileName
                 + ".PNG' successfully");
        }
    }


//Client
    class Client
    {
        static void Main(string[] args)
        {
            ExportContext ctx = new ExportContext(new ExportPNG());
            ctx.CreateArchive("Mushroom");
            ctx.SetStrategy(new ExportJPG());
            ctx.CreateArchive("Mushroom");
            ctx.SetStrategy(new ExportPDF());
            ctx.CreateArchive("Mushroom");
            Console.Read();
        }
    }   


