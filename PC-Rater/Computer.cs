namespace EmperorPC
{
    class Computer
    {
        public record TypeVector
        {
            public double Gaming { get; set; }

            public double Office { get; set; }

            public double Workstation { get; set; }

            public double Overall { get; set; }

            public double Memory { get; set; }

            public double Physics { get; set; }

            public double Graphics { get; set; }

            public double Storage { get; set; }
        }

        public int ID { get; set; }

        public int Price { get; set; }

        public int CPUID { get; init; }

        public int VGAID { get; init; }

        public int RAMID { get; init; }

        public int SSDID { get; init; }

        public CPU CPU { get; set; }

        public VGA VGA { get; set; }

        public RAM RAM { get; set; }

        public SSD SSD { get; set; }

        public TypeVector Score { get; set; } = new();

        public Computer(int ID, int CPUID, int VGAID, int RAMID, int SSDID)
        {
            this.ID = ID;
            this.CPUID = CPUID;
            this.VGAID = VGAID;
            this.RAMID = RAMID;
            this.SSDID = SSDID;
        }

        public Computer(int ID, CPU CPU, VGA VGA, RAM RAM, SSD SSD)
        {
            this.ID = ID;
            this.CPU = CPU;
            this.VGA = VGA;
            this.RAM = RAM;
            this.SSD = SSD;

            CPUID = CPU.ID;
            VGAID = VGA == null? 0 : VGA.ID;
            RAMID = RAM.ID;
            SSDID = SSD.ID;

            CalculatePrice();
            CalculateScore();
        }

        public void CalculatePrice()
        {
            Price = 0;

            Price += CPU.Price;
            Price += RAM.Price;
            Price += SSD.Price;

            if (VGA != null)
                Price += VGA.Price;
        }

        public void CalculateScore()
        {
            Score.Office = 0;
            Score.Gaming = 0;
            Score.Workstation = 0;

            Score.Office += CPU.Score * 0.7;
            Score.Office += VGA == null ? 0 : VGA.Score * 0.1;
            Score.Office += RAM.DualChannel ? RAM.Score * 1.5 : RAM.Score;
            Score.Office += SSD.Score * 0.6;

            Score.Gaming += CPU.Score * 0.4;
            Score.Gaming += VGA == null ? 0 : VGA.Score * 3;
            Score.Gaming += RAM.DualChannel ? RAM.Score * 1.6 : RAM.Score * 0.7;
            Score.Gaming += SSD.Score * 0.2;

            Score.Workstation += CPU.Score;
            Score.Workstation += VGA == null ? 0 : VGA.Score * 0.8;
            Score.Workstation += RAM.DualChannel ? RAM.Score * 2 : RAM.Score;
            Score.Workstation += SSD.Score*1.2;
            Score.Workstation *= 0.8;

            Score.Overall = Score.Office * 0.3 + Score.Gaming * 0.4 + Score.Workstation * 0.3;

            Score.Physics = CPU.Score * 0.7 + RAM.Score * (RAM.DualChannel ? 0.3 : 0.15);
            Score.Memory = CPU.Score * 0.2 + RAM.Score * (RAM.DualChannel ? 0.8 : 0.4);
            Score.Graphics = CPU.Score * 0.2 + (VGA == null ? 0 : VGA.Score * 0.8);
            Score.Storage = RAM.Score * 0.1 + SSD.Score * 0.9;
        }
    }
}
