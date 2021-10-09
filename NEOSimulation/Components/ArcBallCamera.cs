using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nez;

namespace NEOSimulation.Components
{
    public class ArcBallCamera : Component, IUpdatable
    {
        [NotInspectable]
        public Matrix View { get; set; }
        
        [NotInspectable]
        public Matrix Projection { get; set; }
        
        // Rotation around the two axes
        public float RotationX { get; set; }
        public float RotationY { get; set; }

        // Y axis rotation limits (radians)
        public float MinRotationY { get; set; }
        public float MaxRotationY { get; set; }

        // Distance between the target and camera
        public float Distance { get; set; }

        // Distance limits
        public float MinDistance { get; set; }
        public float MaxDistance { get; set; }

        // Calculated position and specified target
        public Vector3 Position { get; private set; }
        public Vector3 Target { get; set; }

        public ArcBallCamera(Vector3 Target, float RotationX,float RotationY, float MinRotationY, float MaxRotationY,float Distance, float MinDistance, float MaxDistance)
        {
            GeneratePerspectiveProjectionMatrix(MathHelper.ToRadians(45));
            
            this.Target = Target;

            this.MinRotationY = MinRotationY;
            this.MaxRotationY = MaxRotationY;

            // Lock the y axis rotation between the min and max values
            this.RotationY = MathHelper.Clamp(RotationY, MinRotationY, MaxRotationY);

            this.RotationX = RotationX;

            this.MinDistance = MinDistance;
            this.MaxDistance = MaxDistance;

            // Lock the distance between the min and max values
            this.Distance = MathHelper.Clamp(Distance, MinDistance, MaxDistance);
        }
        
        private void GeneratePerspectiveProjectionMatrix(float fieldOfView)
        {
            PresentationParameters pp = Core.GraphicsDevice.PresentationParameters;

            float aspectRatio = (float)pp.BackBufferWidth / (float)pp.BackBufferHeight;
            this.Projection = Matrix.CreatePerspectiveFieldOfView(fieldOfView, aspectRatio, 0.1f, 1000000.0f);
        }
        
        public void Move(float DistanceChange)
        {
           this.Distance += DistanceChange;

           this.Distance = MathHelper.Clamp(Distance, MinDistance, MaxDistance);
        }

        public void Rotate(float RotationXChange, float RotationYChange)
        {
           this.RotationX += RotationXChange;
           this.RotationY += -RotationYChange;

           this.RotationY = MathHelper.Clamp(RotationY, MinRotationY, MaxRotationY);
        }

        public void Translate(Vector3 PositionChange)
        {
           this.Position += PositionChange;
        }

        public void Update()
        {
            // Calculate rotation matrix from rotation values
            Matrix rotation = Matrix.CreateRotationX(RotationY) * Matrix.CreateRotationZ(RotationX);

            // Translate down the Z axis by the desired distance
            // between the camera and object, then rotate that
            // vector to find the camera offset from the target
            Vector3 translation = new Vector3(0, 0, Distance);
            translation = Vector3.Transform(translation, rotation);
            
            Position = Target + translation;

            // Calculate the up vector from the rotation matrix
            Vector3 up = Vector3.Transform(Vector3.Up, rotation);

            View = Matrix.CreateLookAt(Position, Target, up);
        }
    }
}