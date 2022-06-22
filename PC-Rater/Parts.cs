namespace EmperorPC
{
    abstract class Part
    {
        public int ID { get; init; }

        public string Manufacturer { get; init; }

        public string Name { get; init; }

        public int Price { get; init; }

        public int Score { get; init; }

        protected int BaseScore { get; init; }

        protected double CorrectionFactor { get; init; } = 1;

        public Part(int ID, string manufacturer, string name, int price)
        {
            this.ID = ID;
            Name = name;
            Manufacturer = manufacturer;
            Price = price;
        }
    }

    class CPU : Part
    {
        public double Frequency { get; init; }
        public int CoreCount { get; init; }

        public CPU(int ID, string manufacturer, string name, int price, int coreCount, double frequency) : base(ID, manufacturer, name, price)
        {
            Frequency = frequency;
            CoreCount = coreCount;

            BaseScore = 8000;

            CorrectionFactor += (coreCount - 6) * 0.15;
            CorrectionFactor += (frequency - 4.0) * 0.2;

            Score = (int)(BaseScore * CorrectionFactor);
        }
    }

    class VGA : Part
    {
        public int StreamProcessor { get; init; }
        public int VRAM { get; init; }

        public VGA(int ID, string manufacturer, string name, int price, int streamProcessor, int vram) : base(ID, manufacturer, name, price)
        {
            StreamProcessor = streamProcessor;
            VRAM = vram;

            BaseScore = 10000;

            CorrectionFactor += (streamProcessor - 3000) * 0.0003;
            CorrectionFactor += (vram - 6.0) * 0.1;

            Score = (int)(BaseScore * CorrectionFactor);
        }
    }

    class RAM : Part
    {
        public bool DualChannel { get; init; }

        public int Capacity { get; init; }

        public int Frequency { get; init; }

        public RAM(int ID, string manufacturer, string name, int price, int capacity, int frequency, bool isDualChannel = false) : base(ID, manufacturer, name, price)
        {
            DualChannel = isDualChannel;
            Capacity = capacity;
            Frequency = frequency;

            BaseScore = 5000;

            CorrectionFactor += (capacity - 8) * 0.08;
            CorrectionFactor += (frequency - 3000) * 0.0004;

            Score = (int)(BaseScore * CorrectionFactor);
        }
    }

    class SSD : Part
    {
        public int Capacity { get; init; }

        public int Performance { get; init; }

        public SSD(int ID, string manufacturer, string name, int price, int capacity, int performance) : base(ID, manufacturer, name, price)
        {
            Capacity = capacity;
            Performance = performance;

            BaseScore = 3000;

            CorrectionFactor += (capacity - 500) * 0.001;
            CorrectionFactor += (performance - 1000) * 0.0005;

            Score = (int)(BaseScore * CorrectionFactor);
        }
    }
}
