//Abstract Class
        abstract class DataMiner
        {
            string file;
            public string File { get => file; set => file = value; }

            string rawData;
            public string RawData { get => rawData; set => rawData = value; }

            public virtual void openFile(string path)
            {
                Console.WriteLine("Open " + path);
            }
            public virtual void closeFile()
            {
                Console.WriteLine("Close file\n");
            }
            public abstract void extractData();
            public abstract void parseData();

            public void mine(string path)
            {
                openFile(path);
                extractData();
                parseData();
                closeFile();
            }

        }   

//Concrete Class
    class CSVDataMiner : DataMiner
    {
        public override void extractData()
        {
            Console.WriteLine("Extract CSV");
        }

        public override void parseData()
        {
            Console.WriteLine("Parse CSV");
        }
    }   
    
    class PDFDataMiner : DataMiner
    {
        public override void openFile(string path)
        {
            base.openFile(path);
            this.File = "PDF";
        }
        public override void extractData()
        {
            Console.WriteLine("Extract " + this.File);
            this.RawData = "PDF";
        }

        public override void parseData()
        {
            Console.WriteLine("Parse " + this.RawData);
        }
    }


//Client
    class Program
    {
        static void Main(string[] args)
        {
            PDFDataMiner pdfDataMiner = new PDFDataMiner();
            pdfDataMiner.mine("pdf path");

            CSVDataMiner csvDataMiner = new CSVDataMiner();
            csvDataMiner.mine("csv path");
        }

    }   
