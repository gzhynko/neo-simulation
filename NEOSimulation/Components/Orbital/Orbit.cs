using System;
using System.Drawing;
using Microsoft.Xna.Framework;
using NEOSimulation.Components.Rendering;
using NEOSimulation.Entities;
using NEOSimulation.Types;
using NEOSimulation.Utils;
using Nez;

namespace NEOSimulation.Components.Orbital
{
    public class Orbit : Polyline3D
    {
        private CelestialBody entity;
        
        public override void OnAddedToEntity()
        {
            base.OnAddedToEntity();
            
            entity = Entity as CelestialBody;
            Insist.IsNotNull(entity, "Orbit component must only be added to a CelestialBody entity");

            if (entity.Type == BodyType.Planet)
            {
                InitializeEllipse();
            }

            Scale = new Vector3(1f);
        }

        public void InitializeEllipse()
        {
            // account for the differences between planet and NEO data
            var argumentOfPerihelion = entity.ArgumentOfPerihelion;
            
            if (entity.Type == BodyType.Planet)
            {
                var longitudeOfPerihelion = entity.ArgumentOfPerihelion;
                argumentOfPerihelion = longitudeOfPerihelion - entity.AscendingNode;
            }
            
            //Console.WriteLine($"anomaly: 1, a: {entity.SemiMajorAxis}, e: {entity.Eccentricity}, peri: {argumentOfPerihelion}, incl: {entity.Inclination}, ascNode: {entity.AscendingNode}");
            
            _vertices = Computation
                .CalculateEllipse((float) entity.SemiMajorAxis, (float) entity.Eccentricity,
                    (float) argumentOfPerihelion * Mathf.Deg2Rad, (float) entity.Inclination * Mathf.Deg2Rad,
                    (float) entity.AscendingNode * Mathf.Deg2Rad, entity.XnaColor, Constants.OBJECT_SCALE);

            InitializePolyline();
        }

        public void RemoveEllipse()
        {
            _vertices = null;
        }
    }
}