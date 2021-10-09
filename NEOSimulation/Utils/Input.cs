using System.Collections.Generic;
using Microsoft.Xna.Framework;
using NEOSimulation.Components;
using NEOSimulation.Components.Orbital;

namespace NEOSimulation.Utils
{
    public class Input
    {
        public static List<BodyName> SortNamesByDistanceToCamera(ArcBallCamera camera, List<BodyName> names)
        {
            var result = names;
            
            result.Sort((first, second) =>
            {
                var firstCamDistance = Vector3.Distance(first.LocalPosition, camera.Position);
                var secondCamDistance = Vector3.Distance(second.LocalPosition, camera.Position);
                return (int) (firstCamDistance - secondCamDistance);
            });

            return result;
        }
    }
}