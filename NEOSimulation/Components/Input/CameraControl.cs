using System;
using Microsoft.Xna.Framework.Input;
using NEOSimulation.Components.Orbital;
using Nez;

namespace NEOSimulation.Components.Input
{
    public class CameraControl : Component, IUpdatable
    {
        private ArcBallCamera camera;
        private SelectedBody selectedBodyManager;

        public override void OnAddedToEntity()
        {
            camera = Entity.GetComponent<ArcBallCamera>();
            selectedBodyManager = MainScene.Instance.SelectedBodyManager;
            selectedBodyManager.Current = MainScene.Instance.GetBodyByName("Sun");

            base.OnAddedToEntity();
        }

        public void Update()
        {
            HandleInput();
            UpdateTargetPosition();
        }

        private void HandleInput()
        {
            if (MainScene.Instance.InputBlocked) return;
            
            var mouseWheelDelta = Nez.Input.MouseWheelDelta;
            var scrollSpeed = camera.Distance <= 400f ? 0.05f : 0.5f;
            var shiftDown = Nez.Input.CurrentKeyboardState.IsKeyDown(Keys.LeftShift);
            var modifiedScrollSpeed = shiftDown ? 0.1f : 1f;
            
            if(mouseWheelDelta != 0f)
                camera.Move(-1f * (mouseWheelDelta * scrollSpeed * modifiedScrollSpeed));

            var viewport = Core.GraphicsDevice.Viewport;
            var mouseState = Nez.Input.CurrentMouseState;
            var previousMouseState = Nez.Input.PreviousMouseState;
            var mouseChangeX = mouseState.X - previousMouseState.X;
            var mouseChangeY = mouseState.Y - previousMouseState.Y;

            if (mouseState.LeftButton == ButtonState.Pressed && mouseState.X > 0 && (mouseState.Y > 0 &&
                mouseState.X < viewport.Width && mouseState.Y < viewport.Height))
            {
                var rate = shiftDown ? -0.001f : -0.01f;
                camera.Rotate(mouseChangeX * rate, mouseChangeY * rate);
            }
        }

        private void UpdateTargetPosition()
        {
            camera.Target = selectedBodyManager.Current.LocalPosition;
        }
    }
}