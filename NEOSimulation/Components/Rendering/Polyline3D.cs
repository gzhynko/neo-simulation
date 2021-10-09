using System;
using System.Linq;
using Microsoft.Xna.Framework.Graphics;
using Nez;

namespace NEOSimulation.Components.Rendering
{
    public class Polyline3D : Renderable3D
    {
        protected VertexPositionColor[] _vertices;
        
        VertexBuffer _vertexBuffer;
        BasicEffect _basicEffect;
        
        protected void InitializePolyline()
        {
            if (_vertexBuffer != null)
                _vertexBuffer.Dispose();

            // create a vertex buffer, and copy our vertex data into it.
            _vertexBuffer = new VertexBuffer(Core.GraphicsDevice, typeof(VertexPositionColor), _vertices.Length,
                BufferUsage.None);
            _vertexBuffer.SetData(_vertices.ToArray());
        }

        public override void OnAddedToEntity()
        {
            base.OnAddedToEntity();

            _basicEffect = Entity.Scene.Content.LoadMonoGameEffect<BasicEffect>();
            _basicEffect.VertexColorEnabled = true;
        }

        #region IDisposable

        ~Polyline3D()
        {
            Dispose(false);
        }

        /// <summary>
        /// frees resources used by this object.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// frees resources used by this object.
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_vertexBuffer != null)
                    _vertexBuffer.Dispose();
                
                if (_basicEffect != null)
                    _basicEffect.Dispose();
            }
        }

        #endregion

        
        public override void Render(Batcher batcher, Camera camera)
        {
            if(_vertices == null || _vertices.Length <= 0) return;
            
            var arcCamera = MainScene.Instance.ArcCamera;
            
            // flush the 2D batch so we render appropriately depth-wise
            batcher.FlushBatch();

            Core.GraphicsDevice.BlendState = BlendState.Opaque;
            Core.GraphicsDevice.DepthStencilState = DepthStencilState.Default;

            // Set BasicEffect parameters.
            _basicEffect.World = WorldMatrix;
            _basicEffect.View = arcCamera.View;
            _basicEffect.Projection = arcCamera.Projection;
            _basicEffect.DiffuseColor = Color.ToVector3();

            // Set our vertex declaration and vertex buffer.
            Core.GraphicsDevice.SetVertexBuffer(_vertexBuffer);

            _basicEffect.CurrentTechnique.Passes[0].Apply();
            Core.GraphicsDevice.DrawUserPrimitives(PrimitiveType.LineStrip, _vertices, 0, _vertices.Length - 1);
        }
    }
}