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
    class PlayerCube : ObjectManager
    {
      public Matrix RotationMatrix = Matrix.Identity;
        Random random = new Random();

        bool useMaelstrom = false;
       
        protected Vector3 velocity = Vector3.Zero;

        Vector3[] corners = new Vector3[8];

        
       
        protected int totalPortals;
        protected int penelty;
        protected int finalScore;
        


        public PlayerCube()
        {
         
          boundingSphere.Radius = 45f;
          boundingSphere2.Radius = 10f;
            

        }

        //KeyboardState keybord = Keyboard.GetState();
        

       protected float speed = 10f;
       public float rotation = 0.05f;
       protected bool hasJumped = false;

        



        public void Update(GameTime gametime, bool gameInPlay)
        {

            if (position.X >= 3800 || position.Z >= 3800 || position.X <= -3800 || position.Z <= -3800)
            {
                while (position.Y >= -10)
                {
                    position += RotationMatrix.Down * speed;
                    Console.WriteLine("Player Dopping");

                }
                if (position.Y <= -10)
                {
                    setPosition(new Vector3(random.Next(GameConstants.width) - 3750, 40, random.Next(GameConstants.length) - 3750));
                    Console.WriteLine("Player Dropped");
                    setPenelty(1);

                }
            }

            boundingSphere.Center = position;
            boundingSphere2.Center = position;


            if (useMaelstrom == true) 
            {
                boundingSphere2.Radius+=5;
            }

            if (useMaelstrom == false)
            {
                boundingSphere2.Radius = 10f;
            }


            if (gameInPlay == true)
            {


                position += RotationMatrix.Forward * speed;
               
                
            }

            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                position += RotationMatrix.Left * speed; 
                //RotationMatrix = Matrix.Multiply(Matrix.CreateRotationY(rotation), RotationMatrix);

            }

            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                position += RotationMatrix.Right * speed;
                //RotationMatrix = Matrix.Multiply(Matrix.CreateRotationY(-rotation), RotationMatrix);

            }

            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                RotationMatrix = Matrix.Multiply(Matrix.CreateRotationY(rotation), RotationMatrix);
                //position += RotationMatrix.Left * speed; 
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                RotationMatrix = Matrix.Multiply(Matrix.CreateRotationY(-rotation), RotationMatrix);
               //position += RotationMatrix.Right * speed; 
            }


            if (Keyboard.GetState().IsKeyDown(Keys.Up) && hasJumped == false)
            {

                position.Y += 20f;
                velocity.Y = 10f;


                hasJumped = true;

            }

            if (hasJumped == true)
            {
                float i = 1;
                velocity.Y -= 0.25f * i;

            }

            if (position.Y <= 50.0f)
            {
                hasJumped = false;
            }


            if (hasJumped == false)
            {

                velocity.Y = 0f;



            }

            position.Y += velocity.Y;
            
           
        }

       

        public void bump()
        {

            position += RotationMatrix.Backward * speed;                 


                velocity = RotationMatrix.Backward * 0.5f;


                position += velocity;

                setPosition(new Vector3(random.Next(GameConstants.width) - 3750, 40, random.Next(GameConstants.length) - 3750));
           
           
        }

        public void setMaelstrom(bool x)
        {
            useMaelstrom = x;
        }

        public void setSpeed(float x)
        {
            speed = speed + (float)x;
        }

        public void defaultSpeed()
        {
            speed = 10f;
        
        }

        public Vector3 getPosition
        {
            get{ return position; }
        }

        public bool getMaelstrom
        {
            get { return useMaelstrom; }
        }

        public void setTotalPortals(int x)
        {
            
            
                totalPortals = totalPortals + x;
            
        }

        public void setPenelty(int x)
        {
            penelty = penelty + x;
        }

        public void setFinalScore()
        {
            finalScore = totalPortals - penelty;
        }
        public int getTotalPortals
        {
            get { return totalPortals; }
        }
        public int getPenelty
        {
            get { return penelty; }
        }
        public int getFinalScore
        {
            get { return finalScore; }
        }

        
              



        public void Draw(GameTime gameTime,GraphicsDeviceManager graphics, Matrix viewMatrix, Matrix projectionMatrix)
        {
            //DebugShapeRenderer.AddBoundingSphere(boundingSphere, Color.AliceBlue);
            //DebugShapeRenderer.Draw(gameTime, viewMatrix, projectionMatrix);
            DebugShapeRenderer.AddBoundingSphere(boundingSphere2, Color.Blue);
            DebugShapeRenderer.Draw(gameTime, viewMatrix, projectionMatrix);
            Transforms = SetupEffectTransformDefaults(model, graphics, viewMatrix, projectionMatrix);
            DrawModel(model, RotationMatrix * Matrix.CreateTranslation(position), Transforms);
            
            
            
        }

    }


}
