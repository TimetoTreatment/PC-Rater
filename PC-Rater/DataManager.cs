using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace EmperorPC
{
    class DataManager
    {
        public List<Computer> ListComputer { get; private set; }
        public List<CPU> ListCPU { get; private set; }
        public List<VGA> ListVGA { get; private set; }
        public List<RAM> ListRAM { get; private set; }
        public List<SSD> ListSSD { get; private set; }

        private readonly string databasePath = "Database/";

        ////////////
        /* 생성자 */
        ////////////
        private DataManager()
        {
            if (Directory.Exists(databasePath) == false)
            {
                Directory.CreateDirectory(databasePath);
                CreateDatabase();
            }

            CheckFile(databasePath, "Computer");
            CheckFile(databasePath, "CPU");
            CheckFile(databasePath, "VGA");
            CheckFile(databasePath, "RAM");
            CheckFile(databasePath, "SSD");

            ReadFile();
            InitializeComputer();
        }

        ///////////////
        /* 파일 확인 */
        ///////////////
        private void CheckFile(string path, string type)
        {
            if (File.Exists(path + type + ".xml") == false)
            {
                string[] contents = { "<" + type + ">", "</" + type + ">" };
                File.WriteAllLines(path + type + ".xml", contents);
            }
        }

        ///////////////
        /* 파일 읽기 */
        ///////////////
        private void ReadFile()
        {
            XElement xElement;

            xElement = XElement.Parse(File.ReadAllText(databasePath + "CPU.xml"));
            ListCPU = (from item in xElement.Descendants("CPU")
                       select new CPU(
                           int.Parse(item.Element("ID").Value),
                           item.Element("Manufacturer").Value,
                           item.Element("Name").Value,
                           int.Parse(item.Element("Price").Value),
                           int.Parse(item.Element("CoreCount").Value),
                           double.Parse(item.Element("Frequency").Value)
                           )).ToList();

            xElement = XElement.Parse(File.ReadAllText(databasePath + "VGA.xml"));
            ListVGA = (from item in xElement.Descendants("VGA")
                       select new VGA(
                           int.Parse(item.Element("ID").Value),
                           item.Element("Manufacturer").Value,
                           item.Element("Name").Value,
                           int.Parse(item.Element("Price").Value),
                           int.Parse(item.Element("StreamProcessor").Value),
                           int.Parse(item.Element("VRAM").Value)
                           )).ToList();

            xElement = XElement.Parse(File.ReadAllText(databasePath + "RAM.xml"));
            ListRAM = (from item in xElement.Descendants("RAM")
                       select new RAM(
                           int.Parse(item.Element("ID").Value),
                           item.Element("Manufacturer").Value,
                           item.Element("Name").Value,
                           int.Parse(item.Element("Price").Value),
                           int.Parse(item.Element("Capacity").Value),
                           int.Parse(item.Element("Frequency").Value)
                           )).ToList();

            xElement = XElement.Parse(File.ReadAllText(databasePath + "SSD.xml"));
            ListSSD = (from item in xElement.Descendants("SSD")
                       select new SSD(
                           int.Parse(item.Element("ID").Value),
                           item.Element("Manufacturer").Value,
                           item.Element("Name").Value,
                           int.Parse(item.Element("Price").Value),
                           int.Parse(item.Element("Capacity").Value),
                           int.Parse(item.Element("Performance").Value)
                           )).ToList();

            xElement = XElement.Parse(File.ReadAllText(databasePath + "Computer.xml"));
            ListComputer = (from item in xElement.Descendants("Computer")
                            select new Computer(
                                int.Parse(item.Element("ID").Value),
                                int.Parse(item.Element("CPUID").Value),
                                int.Parse(item.Element("VGAID").Value),
                                int.Parse(item.Element("RAMID").Value),
                                int.Parse(item.Element("SSDID").Value)
                                )).ToList();
        }

        ///////////////
        /* 파일 쓰기 */
        ///////////////
        public void WriteFile()
        {
            ListComputer.Sort((left, right) => left.ID.CompareTo(right.ID));
            ListCPU.Sort((left, right) => left.ID.CompareTo(right.ID));
            ListVGA.Sort((left, right) => left.ID.CompareTo(right.ID));
            ListRAM.Sort((left, right) => left.ID.CompareTo(right.ID));
            ListSSD.Sort((left, right) => left.ID.CompareTo(right.ID));

            if (Directory.Exists(databasePath) == false)
                Directory.CreateDirectory(databasePath);

            string output;

            output = "<Computer>\n";

            foreach (var computer in ListComputer)
            {
                output += "    <Computer>\n";
                output += Tagging("ID", computer.ID, 2);
                output += Tagging("Price", computer.Price, 2);
                output += Tagging("CPUID", computer.CPUID, 2);
                output += Tagging("VGAID", computer.VGAID, 2);
                output += Tagging("RAMID", computer.RAMID, 2);
                output += Tagging("SSDID", computer.SSDID, 2);
                output += "    </Computer>\n";
            }

            output += "</Computer>\n";
            File.WriteAllText(databasePath + "Computer.xml", output);

            output = "<CPU>\n";

            foreach (var item in ListCPU)
            {
                output += "    <CPU>\n";
                output += Tagging("ID", item.ID, 2);
                output += Tagging("Manufacturer", item.Manufacturer, 2);
                output += Tagging("Name", item.Name, 2);
                output += Tagging("Price", item.Price, 2);
                output += Tagging("CoreCount", item.CoreCount, 2);
                output += Tagging("Frequency", item.Frequency, 2);
                output += "    </CPU>\n";
            }

            output += "</CPU>\n";
            File.WriteAllText(databasePath + "CPU.xml", output);

            output = "<VGA>\n";

            foreach (var item in ListVGA)
            {
                output += "    <VGA>\n";
                output += Tagging("ID", item.ID, 2);
                output += Tagging("Manufacturer", item.Manufacturer, 2);
                output += Tagging("Name", item.Name, 2);
                output += Tagging("Price", item.Price, 2);
                output += Tagging("StreamProcessor", item.StreamProcessor, 2);
                output += Tagging("VRAM", item.VRAM, 2);
                output += "    </VGA>\n";
            }

            output += "</VGA>\n";
            File.WriteAllText(databasePath + "VGA.xml", output);

            output = "<RAM>\n";

            foreach (var item in ListRAM)
            {
                output += "    <RAM>\n";
                output += Tagging("ID", item.ID, 2);
                output += Tagging("Manufacturer", item.Manufacturer, 2);
                output += Tagging("Name", item.Name, 2);
                output += Tagging("Price", item.Price, 2);
                output += Tagging("Capacity", item.Capacity, 2);
                output += Tagging("Frequency", item.Frequency, 2);
                output += "    </RAM>\n";
            }

            output += "</RAM>\n";
            File.WriteAllText(databasePath + "RAM.xml", output);

            output = "<SSD>\n";

            foreach (var item in ListSSD)
            {
                output += "    <SSD>\n";
                output += Tagging("ID", item.ID, 1);
                output += Tagging("Manufacturer", item.Manufacturer, 1);
                output += Tagging("Name", item.Name, 1);
                output += Tagging("Price", item.Price, 1);
                output += Tagging("Capacity", item.Capacity, 1);
                output += Tagging("Performance", item.Performance, 1);
                output += "    </SSD>\n";
            }

            output += "</SSD>\n";
            File.WriteAllText(databasePath + "SSD.xml", output);
        }

        ///////////////
        /* 태그 추가 */
        ///////////////
        private string Tagging(string element, object contents, int indent = 0)
        {
            return new string(' ', indent * 4) + "<" + element + ">" + contents + "</" + element + ">\n";
        }

        ////////////////////
        /* ID와 객체 연결 */
        ////////////////////
        private void InitializeComputer()
        {
            foreach (Computer computer in ListComputer)
            {
                computer.CPU = ListCPU.Find(item => item.ID == computer.CPUID);
                computer.VGA = ListVGA.Find(item => item.ID == computer.VGAID);
                computer.RAM = ListRAM.Find(item => item.ID == computer.RAMID);
                computer.SSD = ListSSD.Find(item => item.ID == computer.SSDID);

                computer.CalculatePrice();
            }
        }

        ////////////////////
        /* 모든 튜플 리셋 */
        ////////////////////
        public void CreateDatabase()
        {
            ListCPU = new();
            ListCPU.Add(new(1748, "Intel", "i5-9400F", 190000, 6, 3.9));
            ListCPU.Add(new(1995, "Intel", "i5-9600KF", 230000, 6, 4.4));
            ListCPU.Add(new(2034, "Intel", "i5-9600K", 250000, 6, 4.4));
            ListCPU.Add(new(4913, "Intel", "i7-9700K", 400000, 8, 4.4));
            ListCPU.Add(new(5078, "Intel", "G6400", 97000, 2, 3.9));
            ListCPU.Add(new(5107, "Intel", "G6605", 115000, 2, 4.1));
            ListCPU.Add(new(5232, "Intel", "i3-10100F", 110000, 4, 3.9));
            ListCPU.Add(new(5235, "Intel", "i3-10100", 120000, 4, 3.9));
            ListCPU.Add(new(5598, "Intel", "i5-11400", 270000, 6, 4.2));
            ListCPU.Add(new(6297, "Intel", "i5-11500", 280000, 6, 4.3));
            ListCPU.Add(new(7101, "Intel", "i5-11600KF", 290000, 6, 4.7));
            ListCPU.Add(new(7691, "Intel", "i7-11700", 460000, 8, 4.7));
            ListCPU.Add(new(8035, "Intel", "i9-11900K", 660000, 10, 5.1));
            ListCPU.Add(new(8068, "AMD", "4350G", 220000, 6, 4.0));
            ListCPU.Add(new(9121, "AMD", "5600X", 340000, 6, 4.4));

            ListVGA = new();
            ListVGA.Add(new(1054, "NVIDIA", "GTX 1650 UDV D6 4GB", 250000, 896, 4));
            ListVGA.Add(new(1182, "NVIDIA", "GTX 1660 D5 6GB", 320000, 1408, 6));
            ListVGA.Add(new(2560, "NVIDIA", "GTX 1070 ARMOR D5 8GB", 360000, 1920, 8));
            ListVGA.Add(new(2616, "NVIDIA", "RTX 3060 D6 12GB", 440000, 3584, 12));
            ListVGA.Add(new(2633, "NVIDIA", "RTX 3060 Ti D6 8GB", 520000, 4864, 8));
            ListVGA.Add(new(3845, "NVIDIA", "RTX 3070 Founders Edition D6 8GB", 780000, 5888, 8));
            ListVGA.Add(new(4749, "NVIDIA", "RTX 3070 TWIN Edge D6 8GB", 790000, 5888, 8));
            ListVGA.Add(new(5030, "NVIDIA", "RTX 3070 Gamer WHITE D6 8GB", 810000, 5888, 8));
            ListVGA.Add(new(5082, "NVIDIA", "RTX 3080 BLACK EDITION D6X 10GB", 1020000, 8704, 10));
            ListVGA.Add(new(6189, "NVIDIA", "RTX 3080 White D6X 10GB", 1040000, 8704, 10));
            ListVGA.Add(new(7368, "NVIDIA", "RTX 3090 D6X 24GB", 1940000, 10496, 24));
            ListVGA.Add(new(8433, "AMD", "RX 6700 XT D6 12GB", 580000, 2560, 12));
            ListVGA.Add(new(9731, "AMD", "FX 6800 OC D6 16GB", 700000, 3840, 16));

            ListRAM = new();
            ListRAM.Add(new(1077, "Samsung", "SAMSUNG DDR4-2933", 34000, 8, 2933));
            ListRAM.Add(new(1931, "Samsung", "SAMSUNG DDR4-2933", 68000, 16, 2933, true));
            ListRAM.Add(new(2558, "GeiL", "DDR4-3200 PRISTINE", 36000, 8, 3200));
            ListRAM.Add(new(2754, "GeiL", "DDR4-3200 PRISTINE", 69000, 16, 3200, true));
            ListRAM.Add(new(3725, "Micron", "Crucial DDR4-3200 CL22", 40000, 8, 3200));
            ListRAM.Add(new(4238, "Micron", "Ballistix DDR4-3200", 44000, 8, 3200));
            ListRAM.Add(new(5651, "SK Hynix", "KLEVV DDR4-3400 RGB", 120000, 16, 3400, true));
            ListRAM.Add(new(6645, "SK Hynix", "DDR4-3400 TRIDENT Z", 210000, 32, 3400, true));
            ListRAM.Add(new(7675, "G.Skill", "DDR4-3600 TRIDENT Z", 180000, 16, 3600, true));
            ListRAM.Add(new(9720, "G.Skill", "DDR4-3600 TRIDENT Z", 340000, 32, 3600, true));

            ListSSD = new();
            ListSSD.Add(new(1346, "Samsung", "870 EVO", 50000, 250, 500));
            ListSSD.Add(new(2265, "Samsung", "870 EVO", 80000, 500, 550));
            ListSSD.Add(new(2935, "Samsung", "870 EVO", 160000, 1000, 550));
            ListSSD.Add(new(4299, "Samsung", "970 EVO M.2", 100000, 500, 3000));
            ListSSD.Add(new(4495, "Samsung", "970 EVO M.2", 190000, 1000, 3000));
            ListSSD.Add(new(4957, "Samsung", "970 EVO M.2", 370000, 2000, 3000));
            ListSSD.Add(new(5502, "Samsung", "970 EVO M.2", 710000, 4000, 3000));
            ListSSD.Add(new(5948, "Samsung", "980 PRO M.2", 170000, 500, 6000));
            ListSSD.Add(new(6001, "Samsung", "980 PRO M.2", 280000, 1000, 6000));
            ListSSD.Add(new(6715, "Micron", "MX500", 45000, 250, 500));
            ListSSD.Add(new(7818, "Micron", "MX500", 70000, 500, 530));
            ListSSD.Add(new(8148, "Micron", "MX500", 130000, 1000, 530));
            ListSSD.Add(new(8573, "SK Hynix", "Gold P31", 98000, 500, 3000));
            ListSSD.Add(new(9860, "SK Hynix", "Gold P31", 198000, 1000, 3000));

            ListComputer = new();
            ListComputer.Add(new(1150, 5235, 0, 1077, 1346));
            ListComputer.Add(new(2022, 8068, 0, 1931, 1346));
            ListComputer.Add(new(2997, 5598, 0, 2558, 2265));
            ListComputer.Add(new(4157, 5598, 0, 2754, 8148));
            ListComputer.Add(new(4271, 7101, 1182, 2754, 2265));
            ListComputer.Add(new(5176, 7101, 2633, 2754, 2265));
            ListComputer.Add(new(5421, 7691, 5030, 5651, 6001));
            ListComputer.Add(new(6143, 8035, 6189, 9720, 4299));
            ListComputer.Add(new(8292, 7691, 0, 1931, 5502));
            ListComputer.Add(new(9330, 8035, 7368, 6645, 6001));

            WriteFile();
            InitializeComputer();
        }

        public void ResetDatabase() 
        {
            ListComputer.Clear();
            ListCPU.Clear();
            ListVGA.Clear();
            ListRAM.Clear();
            ListSSD.Clear();

            CreateDatabase();
        }

        /////////////////
        /* 싱글톤 패턴 */
        /////////////////
        private static readonly Lazy<DataManager> lazy
        = new(() => new DataManager());

        public static DataManager Instance => lazy.Value;
    }
}
