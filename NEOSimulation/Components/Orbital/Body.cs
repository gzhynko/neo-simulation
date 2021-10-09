using System;
using Microsoft.Xna.Framework;
using NEOSimulation.Entities;
using NEOSimulation.Types;
using NEOSimulation.Utils;
using Nez;
using Sphere3D = NEOSimulation.Components.Rendering.Sphere3D;

namespace NEOSimulation.Components.Orbital
{
    public class Body : Sphere3D
    {
        private CelestialBody entity;
        
        public override void OnAddedToEntity()
        {
            base.OnAddedToEntity();

            entity = Entity as CelestialBody;
            Insist.IsNotNull(entity, "Body component must only be added to a CelestialBody entity");
            
            if(entity.Type == BodyType.Planet)
                GenerateSphere(50, entity.XnaColor, (float)entity.DiameterKm * Constants.KM_TO_AU, new Vector3(Constants.OBJECT_SCALE));
        }

        /*public void ChangePositionToEpoch(double daysFromJ2000)
        {
            var centuriesFromJ2000 = daysFromJ2000 / (365.25 * 100);
            Console.WriteLine("new centuries from j2000: " + centuriesFromJ2000);

            // account for the differences between planet and NEO data
            var longitudeOfPerihelion = entity.AscendingNode + entity.ArgumentOfPerihelion;
            var meanLongitude = longitudeOfPerihelion + entity.MeanAnomaly;
            if (entity.Type == BodyType.Planet)
            {
                longitudeOfPerihelion = entity.ArgumentOfPerihelion;
                meanLongitude = entity.MeanAnomaly;
            }

            var newMeanLongitude = meanLongitude + entity.MeanMotion * (entity.Type == BodyType.Planet ? centuriesFromJ2000 : daysFromJ2000);
            var newMeanAnomaly = newMeanLongitude - longitudeOfPerihelion;
            
            // account for the differences between planet and NEO data
            var argumentOfPerihelion = entity.ArgumentOfPerihelion;
            if (entity.Type == BodyType.Planet)
            {
                argumentOfPerihelion = longitudeOfPerihelion - entity.AscendingNode;
            }

            var eccentricAnomaly = Computation.SolveKelperEquation(newMeanAnomaly, entity.Eccentricity);

            //Console.WriteLine($"anomaly: {eccentricAnomaly}, a: {entity.SemiMajorAxis}, e: {entity.Eccentricity}, peri: {argumentOfPerihelion}, incl: {entity.Inclination}, ascNode: {entity.AscendingNode}");
            //Console.WriteLine($"centuries from j2000 = {centuriesFromJ2000}");
            var newPos = Computation.PositionAtEccentricAnomaly((float) eccentricAnomaly, (float) entity.SemiMajorAxis,
                (float) entity.Eccentricity, (float) argumentOfPerihelion * Mathf.Deg2Rad, (float) entity.Inclination * Mathf.Deg2Rad,
                (float) entity.AscendingNode * Mathf.Deg2Rad);
            LocalPosition = new Vector3(newPos.X * Constants.OBJECT_SCALE, newPos.Y * Constants.OBJECT_SCALE, newPos.Z * Constants.OBJECT_SCALE);
        }*/
        
        public void ChangePositionToEpoch(DateTime date)
        {
            var daysFromOsculationDate = (date - entity.OsculationDate).TotalDays;
            var centuriesFromOsculationDate = daysFromOsculationDate / (365.25 * 100);

            var longitudeOfPerihelion = entity.AscendingNode + entity.ArgumentOfPerihelion;
            var meanLongitude = longitudeOfPerihelion + entity.MeanAnomaly;
            if (entity.Type == BodyType.Planet)
            {
                longitudeOfPerihelion = entity.ArgumentOfPerihelion;
                meanLongitude = entity.MeanAnomaly;
            }

            var newMeanLongitude = meanLongitude + entity.MeanMotion * (entity.Type == BodyType.Planet ? centuriesFromOsculationDate : daysFromOsculationDate);
            
            var newMeanAnomaly = newMeanLongitude - longitudeOfPerihelion;
            var argumentOfPerihelion = entity.ArgumentOfPerihelion;
            if (entity.Type == BodyType.Planet)
            {
                argumentOfPerihelion = longitudeOfPerihelion - entity.AscendingNode;
            }

            var eccentricAnomaly = Computation.SolveKelperEquation(newMeanAnomaly * Mathf.Deg2Rad, entity.Eccentricity);

            var newPos = Computation.PositionAtEccentricAnomaly((float) eccentricAnomaly, (float) entity.SemiMajorAxis,
                (float) entity.Eccentricity, (float) argumentOfPerihelion * Mathf.Deg2Rad, (float) entity.Inclination * Mathf.Deg2Rad,
                (float) entity.AscendingNode * Mathf.Deg2Rad);
            LocalPosition = new Vector3(newPos.X * Constants.OBJECT_SCALE, newPos.Y * Constants.OBJECT_SCALE, newPos.Z * Constants.OBJECT_SCALE);
        }

        public void OnSelected()
        {
            if(entity.Type == BodyType.Planet) return;
            
            var orbit = entity.GetComponent<Orbit>();
            orbit.InitializeEllipse();
        }
        
        public void OnDeselected()
        {
            if(entity.Type == BodyType.Planet) return;

            var orbit = entity.GetComponent<Orbit>();
            orbit.RemoveEllipse();
        }
    }
}