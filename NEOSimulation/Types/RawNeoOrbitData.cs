using CsvHelper.Configuration.Attributes;

namespace NEOSimulation.Types
{
    public class RawNeoOrbitData
    {
        [Name("full_name")]
        public string Name { get; set; }
        
        [Name("epoch_mjd")]
        public double Epoch { get; set; }

        [Name("a")]
        public double A { get; set; }
        
        [Name("e")]
        public double E { get; set; }
        
        [Name("i")]
        public double I { get; set; }
        
        [Name("om")]
        public double Node { get; set; }

        [Name("w")]
        public double Peri { get; set; }
        
        [Name("ma")]
        public double M { get; set; }
        
        [Name("n")]
        public double N { get; set; }
        
        [Name("moid")]
        public double Moid { get; set; }

        [Name("diameter")]
        public double? Diameter { get; set; }
    }
}