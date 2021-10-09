using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace NEOSimulation.Types
{
    public class RawPlanetaryOrbitData
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        
        [JsonProperty("a")]
        public double A { get; set; }
        
        [JsonProperty("e")]
        public double E { get; set; }
        
        [JsonProperty("i")]
        public double I { get; set; }
        
        [JsonProperty("M")]
        public double M { get; set; }
        
        [JsonProperty("Mdot")]
        public double MDot { get; set; }
        
        [JsonProperty("peri")]
        public double Peri { get; set; }
        
        [JsonProperty("node")]
        public double Node { get; set; }
        
        [JsonProperty("epoch")]
        public double Epoch { get; set; }
        
        [JsonProperty("diameter")]
        public double Diameter { get; set; }
        
        [JsonProperty("htmlColor")]
        public string HtmlColor { get; set; }
    }
}