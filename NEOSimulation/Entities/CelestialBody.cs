using System;
using Microsoft.Xna.Framework;
using NEOSimulation.Types;
using Nez;

namespace NEOSimulation.Entities
{
    public class CelestialBody : Entity
    {
        public BodyType Type { get; set; }
        public double SemiMajorAxis { get; set; }
        public double Eccentricity { get; set; }
        public double Inclination { get; set; }
        public double AscendingNode { get; set; }
        public double ArgumentOfPerihelion { get; set; }
        public double Epoch { get; set; }
        public double MeanAnomaly { get; set; }
        public double MeanMotion { get; set; }
        public double DiameterKm { get; set; }
        public System.Drawing.Color Color { get; set; }
        
        public Color XnaColor => new (Color.R, Color.G, Color.B, Color.A);
        public DateTime OsculationDate { get; set; }
    }
}