using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
namespace Cube
{
    class PortalManager
    {
        Model portal;
        
        int totalPortals = 0;
        public ObjectManager[] portals;
       
        int x,y,z;
        Random random = new Random(8);
       

        public PortalManager(Model model, int portalNum)
        {
            totalPortals = portalNum;
            portals = new ObjectManager[portalNum];
            portal = model;
            for (int i = 0; i < totalPortals; i++)
            {
                portals[i] = new ObjectManager();
                portals[i].model = model;

                portals[i].setPosition(new Vector3(random.Next(GameConstants.width) - 3750, 40, random.Next(GameConstants.length) - 3750));

                portals[i].boundingSphere.Center = portals[i].position;
                portals[i].boundingSphere.Radius = 100f;

                

                portals[i].boundingSphere2.Center = portals[i].position + new Vector3(0f, portals[i].position.Y + 100f,0f);
                portals[i].boundingSphere2.Radius = 50f;


                portals[i].boundingSphere3.Center = portals[i].position;
                portals[i].boundingSphere3.Radius = 150f;

            }
        }

        public void Initialize(GraphicsDeviceManager graphics, Matrix viewMatrix, Matrix projectionMatrix, GameTime gameTime)
        {
            
            for (int i = 0; i < totalPortals; i++)
            {

                //DebugShapeRenderer.AddBoundingSphere(portals[i].boundingSphere, Color.Red);
                //DebugShapeRenderer.Draw(gameTime, viewMatrix, projectionMatrix);
                //DebugShapeRenderer.AddBoundingSphere(portals[i].boundingSphere2, Color.Black);
                //DebugShapeRenderer.Draw(gameTime, viewMatrix, projectionMatrix);
                //DebugShapeRenderer.AddBoundingSphere(portals[i].boundingSphere3, Color.Aquamarine);
                //DebugShapeRenderer.Draw(gameTime, viewMatrix, projectionMatrix);
                portals[i].draw(graphics, viewMatrix, projectionMatrix);

                
                
                
            }

           

          

        }

        public void changePortal(int x, Model model)
        {
            portals[x].model = model;
        }


    }
}
