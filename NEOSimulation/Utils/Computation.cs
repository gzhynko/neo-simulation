using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NEOSimulation.Components.Orbital;

namespace NEOSimulation.Utils
{
    public static class Computation
    {
        public static VertexPositionColor[] CalculateEllipse(float semiMajorAxis, float eccentricity, float periapsis, float inclination, float ascNode, Color color, float scale)
        {
            var ellipseDetail = 0.01f;
            var totalVertices = 2 * Math.PI / ellipseDetail;
            var verticesArray = new VertexPositionColor[(int)totalVertices + 1];

            var index = 0;
            for (float i = 0; i <= 2 * Math.PI; i += ellipseDetail)
            {
                verticesArray[index] = new VertexPositionColor(PositionAtEccentricAnomaly(i, semiMajorAxis, eccentricity, periapsis, inclination, ascNode) * scale, color);
                index++;
            }
            
            return verticesArray;
        }

        /// <summary>
        /// The algorithm is taken from this StackExchange Space answer by 2012rcampion:
        /// https://space.stackexchange.com/a/8915
        /// </summary>
        public static Vector3 PositionAtEccentricAnomaly(float eccentricAnomaly, float semiMajorAxis, float eccentricity, float argumentOfPeriapsis, float inclination, float ascendingNode)
        {
            var xInPlane = semiMajorAxis * (Math.Cos(eccentricAnomaly) - eccentricity);
            var yInPlane = semiMajorAxis * Math.Sin(eccentricAnomaly) * Math.Sqrt(1 - Math.Pow(eccentricity, 2));
                
            var x = Math.Cos(argumentOfPeriapsis) * xInPlane - Math.Sin(argumentOfPeriapsis) * yInPlane;
            var y = Math.Sin(argumentOfPeriapsis) * xInPlane + Math.Cos(argumentOfPeriapsis) * yInPlane;
            var z = Math.Sin(inclination) * y;
            y = Math.Cos(inclination) * y;

            var tempX = x;
            x = Math.Cos(ascendingNode) * tempX - Math.Sin(ascendingNode) * y;
            y = Math.Sin(ascendingNode) * tempX + Math.Cos(ascendingNode) * y;

            return new Vector3((float) x, (float) y, (float) z);
        }

        public static double SolveKelperEquation(double meanAnomaly, double eccentricity)
        {
            var eccentricAnomaly = meanAnomaly;
            while(true) {
                var dE = (eccentricAnomaly - eccentricity * Math.Sin(eccentricAnomaly) - meanAnomaly) /
                         (1 - eccentricity * Math.Cos(eccentricAnomaly));
                eccentricAnomaly -= dE;
                
                if(Math.Abs(dE) < 1e-6) break;
            }

            return eccentricAnomaly;
        }

        public static double DistanceInAuBetweenBodies(Body first, Body second)
        {
            var scaledDistance = Vector3.Distance(first.LocalPosition, second.LocalPosition);
            return scaledDistance / Constants.OBJECT_SCALE;
        }
    }
}