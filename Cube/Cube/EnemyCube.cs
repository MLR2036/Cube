using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
namespace Cube
{
    class EnemyCube : PlayerCube
    {
        Random randomPos = new Random();//where to put the cube
        Random dropOff = new Random();//what to do when approching the edge
        Random portalChoice = new Random();//what to do at a portal
        GameTime updateDecisions = new GameTime();
      

        double update = 150;
        double i = 0;
        bool allowUpdate = false;
        bool allowJump = false;
        

        public EnemyCube()
        {
            
            boundingSphere.Radius = 45f;
            setPosition(new Vector3(randomPos.Next(GameConstants.width) - 3750, 40, randomPos.Next(GameConstants.length) - 3750));
           
        }

        public void updateTimer()
        {
            //control when to change direction
            if (i < update)
            {
                allowUpdate = false;
                i += updateDecisions.ElapsedGameTime.Milliseconds;
               //i = i + 1;
            }
            else if (i >= update)
            {
                allowUpdate = true;
                i = 0;
            }
        }

        public void setJump(bool jump)
        {
            this.allowJump = jump;
        }

        public void Update(GameTime gametime, bool gameInPlay, int x)
        {
            updateDecisions = gametime;
            boundingSphere.Center = position;
            

            if (gameInPlay == true)
            { //continus movement
                position += RotationMatrix.Forward * speed;
            }

            //Console.WriteLine(position);


            if (position.X >= 3800 || position.Z >= 3800 || position.X <= -3800 || position.Z <= -3800)
            {
                //The percantage of time to drop off or turn around
                int i = dropOff.Next(11);
                if (1 <= i && i <= 7)
                {
                    RotationMatrix = Matrix.Multiply(Matrix.CreateRotationY(180f), RotationMatrix);
                    Console.WriteLine("Turned around");
                    
                }
                else if (8 <= i && i <= 10)
                {
                    allowUpdate = false;

                    while (position.Y >= -10)
                    {
                        position += RotationMatrix.Down * speed;
                        Console.WriteLine("Dopping");
                       
                    }
                        if (position.Y <= -10)
                        {
                            setPosition(new Vector3(randomPos.Next(GameConstants.width) - 3750, 40, randomPos.Next(GameConstants.length) - 3750));
                            Console.WriteLine("Dropped");
                            
                        }
                    }
                
            }

            if (allowUpdate == true)
            {
                switch (x)
                {// turn in a direction based on a random number
                    case 1:
                        RotationMatrix = Matrix.Multiply(Matrix.CreateRotationY(rotation*10f), RotationMatrix);
                        break;
                    case 2:
                        RotationMatrix = Matrix.Multiply(Matrix.CreateRotationY(-rotation*10f), RotationMatrix);
                        break;
                   
                }
            }

            //land after jumping
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

            allowJump = false;

            
            
                
        }

        public void portal(int x)
        {
            //percantage of time to jump over portals or bump in to them
            if (1 <= x && x <= 6)
            {
                if (allowJump == true && hasJumped == false)
                {
                    position.Y += 20f;
                    velocity.Y = 10f;
                    Console.WriteLine("Jumped");
                    hasJumped = true;
                }               

            }
            else if (7 <= x && x <= 10 )
            {
                
                allowUpdate = false;
                
            }
        }


        }
        
  
    }

