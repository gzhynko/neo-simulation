using System;
using System.Drawing;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NEOSimulation.Entities;
using NEOSimulation.Types;
using Nez;
using Graphics = Nez.Graphics;
using Rectangle = Microsoft.Xna.Framework.Rectangle;
using RectangleF = Nez.RectangleF;

namespace NEOSimulation.Components.Orbital
{
    public class BodyName : Rendering.Renderable3D, IUpdatable
    {
        private BasicEffect _basicEffect;
        public Body body;
        private CelestialBody entity;
        private float textScale;
        
        public override void OnAddedToEntity()
        {
            base.OnAddedToEntity();
            
            entity = Entity as CelestialBody;
            Insist.IsNotNull(entity, "BodyName component must only be added to a CelestialBody entity");

            _basicEffect = Core.Content.LoadMonoGameEffect<BasicEffect>();
            _basicEffect.VertexColorEnabled = true;
            body = Entity.GetComponent<Body>();
            
            textScale = entity.Type == BodyType.Planet ? 2f : 1f;
            Scale = Vector3.One;
        }

        public override void Render(Batcher batcher, Camera camera)
        {
            var font = Graphics.Instance.BitmapFont;
            var text = Entity.Name;
            var arcCamera = MainScene.Instance.ArcCamera;
            var position = LocalPosition;

            var color = MainScene.Instance.SelectedBodyManager.Current == body
                ? Microsoft.Xna.Framework.Color.White
                : entity.XnaColor;
            
            var viewport = Core.GraphicsDevice.Viewport;
            var position2D = viewport.Project(position, arcCamera.Projection, arcCamera.View, WorldMatrix);

            if (position2D.Z > 1) return;
            
            var textSize = font.MeasureString(text);
            var centeredPosition = new Vector2(position2D.X - (textSize.X * textScale / 2), position2D.Y - (textSize.Y * textScale / 2));
            
            batcher.DrawString(font, text, centeredPosition, color, 0, Vector2.Zero, textScale, SpriteEffects.None, position2D.Z);
        }

        private RectangleF GetScreenBounds()
        {
            var font = Graphics.Instance.BitmapFont;
            var text = Entity.Name;
            var arcCamera = MainScene.Instance.ArcCamera;
            var position = LocalPosition;
            
            var viewport = Core.GraphicsDevice.Viewport;
            var position2D = viewport.Project(position, arcCamera.Projection, arcCamera.View, WorldMatrix);
            
            var textSize = font.MeasureString(text);
            var centeredPosition = new Vector2(position2D.X - (textSize.X * textScale / 2), position2D.Y - (textSize.Y * textScale / 2));

            return new RectangleF(centeredPosition.X, centeredPosition.Y, textSize.X * textScale,
                textSize.Y * textScale);
        }

        public void Update()
        {
            LocalPosition = new Vector3(body.LocalPosition.X / 2f, body.LocalPosition.Y / 2f,
                body.Position.Z / 2f);
        }

        public bool CheckIfClicked(Vector2 mousePos)
        {
            var bounds = GetScreenBounds();

            var mouseOver = bounds.Contains(mousePos);
            return mouseOver;
        }
    }
}