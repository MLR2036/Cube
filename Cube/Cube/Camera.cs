
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;





namespace Cube
{
    class Camera
    {
        //where the camara is in the world
        //what it is looking at 
        //which way is up in the world
        public Vector3 camPosition;
        public Vector3 camLookAt;
        public Vector3 worldUp;

        public Camera()
        {
            camPosition = new Vector3(0, 0, 0);
            camLookAt = new Vector3(0, 0, 0);
            worldUp = new Vector3(0, 1, 0);
        }

        public void FollowCam(Vector3 playerPosition, float x, float y ,float z)
        {
            camPosition.X = playerPosition.X + x;
            camPosition.Y = playerPosition.Y + y;
            camPosition.Z = playerPosition.Z + z;

            camLookAt.X = playerPosition.X;
            camLookAt.Y = playerPosition.Y;
            camLookAt.Z = playerPosition.Z;
 
        }

        public void ThirdPersonCam(float pRotation, Vector3 cubePostion, GraphicsDevice grapichsDevice)
        {
            Vector3 thridPersonRef= new Vector3 (0,200,-200);
            Matrix rotationMatrix = Matrix.CreateRotationY(pRotation);
            Vector3 transformRef = Vector3.Transform(thridPersonRef,rotationMatrix);

            camPosition = transformRef + cubePostion;
            camLookAt = cubePostion;


           // Matrix view = Matrix.CreateLookAt(camPosition, cubePostion, new Vector3(0.0f, 1.0f, 0.0f));

          
        }


    }
}
