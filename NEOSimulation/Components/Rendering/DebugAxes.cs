using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nez;

namespace NEOSimulation.Components.Rendering
{
    public class DebugAxes : Renderable3D
    {
        private ArcBallCamera _camera;
        private VertexPositionColor[] _vertices;
        
        VertexBuffer _vertexBuffer;
        BasicEffect _basicEffect;
        
        public override void OnEnabled()
        {
            _camera = MainScene.Instance.ArcCamera;
            
            SetVertices();
            _basicEffect = Entity.Scene.Content.LoadMonoGameEffect<BasicEffect>();
            _basicEffect.VertexColorEnabled = true;
            
            // create a vertex buffer, and copy our vertex data into it.
            _vertexBuffer = new VertexBuffer(Core.GraphicsDevice, typeof(VertexPositionColor), _vertices.Length,
                BufferUsage.None);
            _vertexBuffer.SetData(_vertices.ToArray());
            
            base.OnEnabled();
        }
        
        private void SetVertices()
        {
            _vertices = new VertexPositionColor[6];

            _vertices[0] = new VertexPositionColor(new Vector3(10,0,0), Color.Red);
            _vertices[1] = new VertexPositionColor(new Vector3(-10,0,0), Color.Red);
            _vertices[2] = new VertexPositionColor(new Vector3(0,10,0), Color.Green);
            _vertices[3] = new VertexPositionColor(new Vector3(0,-10,0), Color.Green);
            _vertices[4] = new VertexPositionColor(new Vector3(0,0,10), Color.Blue);
            _vertices[5] = new VertexPositionColor(new Vector3(0,0,-10), Color.Blue);
        }

        public override void Render(Batcher batcher, Camera camera)
        {
            // flush the 2D batch so we render appropriately depth-wise
            batcher.FlushBatch();

            Core.GraphicsDevice.BlendState = BlendState.Opaque;
            Core.GraphicsDevice.DepthStencilState = DepthStencilState.Default;

            // Set BasicEffect parameters.
            _basicEffect.World = WorldMatrix;
            _basicEffect.View = _camera.View;
            _basicEffect.Projection = _camera.Projection;
            _basicEffect.DiffuseColor = Color.White.ToVector3();

            // Set our vertex declaration and vertex buffer.
            Core.GraphicsDevice.SetVertexBuffer(_vertexBuffer);

            _basicEffect.CurrentTechnique.Passes[0].Apply();
            Core.GraphicsDevice.DrawUserPrimitives(PrimitiveType.LineList, _vertices, 0, _vertices.Length - 1);
        }
    }
}