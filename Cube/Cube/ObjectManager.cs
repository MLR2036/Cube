using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Cube
{
    class ObjectManager
    {
        public Model model;
        public Model playerPortal;
        public Matrix[] Transforms;

        //model position in world
        public Vector3 position = Vector3.Zero;
        //collision detection
        public BoundingSphere boundingSphere;
        public BoundingSphere boundingSphere2;
        public BoundingSphere boundingSphere3;
        public BoundingBox boundingBox;

       

        public void setPosition(Vector3 position)
        {
            this.position = position;
        }

       
        public void draw(GraphicsDeviceManager graphics, Matrix viewMatrix, Matrix projectionMatrix)
        {
            Transforms = SetupEffectTransformDefaults(model, graphics, viewMatrix, projectionMatrix);
            DrawModel(model, Matrix.CreateTranslation(position), Transforms);
 
        }

        public void DrawModel(Model model, Matrix modelTransform, Matrix[] absoluteBoneTransforms)
        {
            //Draw the model, a model can have multiple meshes, so loop
            foreach (ModelMesh mesh in model.Meshes)
            {
                //This is where the mesh orientation is set
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.World = absoluteBoneTransforms[mesh.ParentBone.Index] * modelTransform;
                }
                //Draw the mesh, will use the effects set above.
                mesh.Draw();
            }
        }


        public Matrix[] SetupEffectTransformDefaults
    (Model myModel, GraphicsDeviceManager graphics, Matrix viewMatrix, Matrix projectionMatrix)
        {
            Matrix[] absoluteTransforms = new Matrix[myModel.Bones.Count];
            myModel.CopyAbsoluteBoneTransformsTo(absoluteTransforms);           

            foreach (ModelMesh mesh in myModel.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.Projection = projectionMatrix;
                    effect.View = viewMatrix;
                }
            }
            return absoluteTransforms;
        }


    }
}
