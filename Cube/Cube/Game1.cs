using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Cube
{
    /// <summary>
    /// Cube Portals is 3D game for PC where the player is challenged to close portals 
    /// while enemy cubes re-open them. In order to progress to the next level the player must close the 
    /// required amount of portals in the given time limit.
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Matrix projectionMatrix;
        Matrix viewMatrix;
        Camera camera = new Camera();
        Matrix RotationMatrix = Matrix.Identity;

        Random enemyMoves = new Random();
        Random coneChoice = new Random();

        Random enemyMoves2 = new Random();
        Random coneChoice2 = new Random();

        bool gameInPlay = true;
        bool cheat1 = false;
        bool cheat2 = false;
        bool cheat3 = false;
        bool cheat4 = false;
        bool story = false;
        
        ObjectManager ground = new ObjectManager();
        ObjectManager spaceSkybox = new ObjectManager();
        PortalManager portals;
        
        Model groundMap;
        Model portal;
        
        

        PlayerCube cube = new PlayerCube();
        EnemyCube eCube = new EnemyCube();
        EnemyCube eCube2 = new EnemyCube();
        
        
        float aspectRatio;



        Vector3 skyboxPosition = Vector3.Zero;
        Vector3 GroundPosition = Vector3.Zero;
        Vector3 modelPosition = new Vector3(0.0f, 50.0f, 0.0f);
        Vector3 velocity = Vector3.Zero;

        KeyboardState lastkey = Keyboard.GetState();


        Texture2D timerBar;
        Texture2D maelstromBar;
        Texture2D maelstromCharge;
        public SpriteFont kootenay;
        Vector2 textPos = new Vector2(500, 250);
        Vector2 enemyScorePos = new Vector2(500, 350);
        Texture2D controls;
        Texture2D storyImg;
        public enum GameState
        { STARTSCREEN, CONTROLS, PREPARELEVEL, PLAYING, PAUSE, GAMEOVER }

        GameState gameState = GameState.STARTSCREEN;

        int level;
        int numberOfPortals;
        int numberToClose;
        int Closed = 0;
        int score = 0;
        int penalty = 0;
       

        double timeUp = 90000f;
        double maelstrom = 5000f;
        double recharge = 40000f;
        double charge = 0f;

        SoundEffect portalClose;
        SoundEffect portalEntry;
        
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            //graphics.IsFullScreen = true;
            graphics.PreferredBackBufferHeight = 500;
            graphics.PreferredBackBufferWidth = 1200;
            
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {

            Window.Title = "Cube Portals";
            aspectRatio = (float)graphics.PreferredBackBufferWidth / graphics.PreferredBackBufferHeight;
            projectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45.0f), aspectRatio,1.0f, 10000.0f);
            DebugShapeRenderer.Initialize(graphics.GraphicsDevice);
           
            
           
            
            
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            spaceSkybox.model = Content.Load<Model>("Models\\spaceSkybox");
            cube.model = Content.Load<Model>("Models\\cubeS");
            groundMap = Content.Load<Model>("Models\\tileGroundMap2");
            portal = Content.Load<Model>("Models\\portal2");
            cube.playerPortal = Content.Load<Model>("Models\\portal1");
            eCube.model = Content.Load<Model>("Models\\cube");
            eCube.playerPortal = Content.Load<Model>("Models\\portal2");
            eCube2.model = Content.Load<Model>("Models\\cube");
            eCube2.playerPortal = Content.Load<Model>("Models\\portal2");

            aspectRatio = graphics.GraphicsDevice.Viewport.AspectRatio;
            ground.model = groundMap;
            timerBar = Content.Load<Texture2D>("textures\\TimeBar");
            maelstromBar = Content.Load<Texture2D>("textures\\TimeBar");
            maelstromCharge = Content.Load<Texture2D>("textures\\charge");
           kootenay = Content.Load<SpriteFont>("Fonts\\Kootaney");
           controls = Content.Load<Texture2D>("textures\\Controls");
           storyImg = Content.Load<Texture2D>("textures\\Story");

           portalClose = Content.Load<SoundEffect>("Audio\\hyperspace_activate");
           portalEntry = Content.Load<SoundEffect>("Audio\\tx0_fire1");
           
          
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            
           KeyboardState keybord = Keyboard.GetState();
            
            
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || keybord.IsKeyDown(Keys.Escape))
            {
                
                this.Exit();
            }

            int incrementLevel = 1;

            if (gameState == GameState.STARTSCREEN)
            {
                if(keybord.IsKeyDown(Keys.Space))
                {
                    story = true;
                }
                if(keybord.IsKeyUp(Keys.Space))
                {
                    story = false;
                }
               
                if(keybord.IsKeyDown(Keys.Enter) && !(lastkey.IsKeyDown(Keys.Enter)))
                {
                    level = 0;
                    numberOfPortals = 10;
                    numberToClose = 6;
                    Closed = 0;
                    portals = new PortalManager(portal, numberOfPortals);
                    score = 0;
                    penalty = 0;
                    cheat1 = false;
                    cheat2 = false;
                    cheat3 = false;
                    cheat4 = false;
                    cube.setTotalPortals(-cube.getTotalPortals);
                    cube.setPenelty(-cube.getPenelty);
                    cube.setFinalScore();
                    cube.defaultSpeed();
                    eCube.defaultSpeed();
                    eCube2.defaultSpeed();
                    cube.setPosition(new Vector3(0.0f,50.0f,0.0f));
                   
                    gameState = GameState.PLAYING;
                    
                }

                if (keybord.IsKeyDown(Keys.C) && !(lastkey.IsKeyDown(Keys.C)))
                {
                    gameState = GameState.CONTROLS; 
                }

                
            }

            if (gameState == GameState.CONTROLS)
            {
                if (keybord.IsKeyDown(Keys.B) && !(lastkey.IsKeyDown(Keys.B)))
                {
                    gameState = GameState.STARTSCREEN;
                }
                
            }

            if (gameState == GameState.PREPARELEVEL)
            {
               
                if (keybord.IsKeyDown(Keys.Enter) && !(lastkey.IsKeyDown(Keys.Enter)))
                {
                    level = level + incrementLevel;
                    numberOfPortals = numberOfPortals + 5;
                    numberToClose = numberToClose + 4;
                    Closed = 0;
                    cheat2 = false;
                    cheat4 = false;
                    cube.setTotalPortals(-cube.getTotalPortals);
                    portals = new PortalManager(portal, numberOfPortals);
                    cube.setSpeed(2f);
                    eCube.setSpeed(3f);
                    eCube2.setSpeed(3f);
                    timeUp = 90000f;
                    gameState = GameState.PLAYING;
                }

 
            }

           

            if (gameState == GameState.PAUSE)
            {
                //cheats for debuging///////////////////////////////////////////////////// 
                if (keybord.IsKeyDown(Keys.OemPlus) && !(lastkey.IsKeyDown(Keys.OemPlus)))
                {
                    maelstrom = maelstrom + 10000000f;//add 2.8 hours of maelstrom to create an infinet maelstrom
                    cheat1 = true;  //booleans to notify draw method
                    cheat3 = false;
                }

                if (keybord.IsKeyDown(Keys.OemMinus) && !(lastkey.IsKeyDown(Keys.OemMinus)))
                {
                    timeUp = 2000f; // set levle time to 2 seconds
                    cheat2 = true;
                    cheat4 = false;
                }
                if (keybord.IsKeyDown(Keys.R) && !(lastkey.IsKeyDown(Keys.R)))
                {
                    maelstrom = 5000f; 
                    cheat1 = false;
                    cheat3 = true;
                }
                if (keybord.IsKeyDown(Keys.T) && !(lastkey.IsKeyDown(Keys.T)))
                {
                    timeUp = timeUp + 10000f; //add 10 seconds of extra time each time pressed
                    cheat2 = false;
                    cheat4 = true;
                }
                /////////////////////////////////////////////////////////////////////////////
                if (keybord.IsKeyDown(Keys.Enter) && !(lastkey.IsKeyDown(Keys.Enter)))
                {
                    gameState = GameState.PLAYING;
                }
            }

           

            if (gameState == GameState.PLAYING)
            {
                timeUp -= gameTime.ElapsedGameTime.Milliseconds;
               // Console.Out.WriteLine(timeUp);

                spaceSkybox.position = cube.position;

                if (keybord.IsKeyDown(Keys.P) && !(lastkey.IsKeyDown(Keys.P)))
                {
                    gameState = GameState.PAUSE;
                }

                   
                

                if (charge >= recharge) 
                {
                    maelstrom = 5000f;
                    charge = 0f;
                }

                if (maelstrom <= 0)
                {
                    charge += gameTime.ElapsedGameTime.Milliseconds;
                    cube.boundingSphere2.Radius = 10;
                }

                if (!(maelstrom <= 0))
                {

                    if (keybord.IsKeyDown(Keys.Space) && (cube.getMaelstrom == false))
                    {

                        cube.setMaelstrom(true);

                    }
                }

                if (cube.getMaelstrom == true)
                {
                    maelstrom -= gameTime.ElapsedGameTime.Milliseconds;
                   // Console.Out.WriteLine(maelstrom + "maelstorm");
                }

                if (keybord.IsKeyUp(Keys.Space) && (cube.getMaelstrom == true))
                {                    
                    cube.setMaelstrom(false);
                }

                int x = 0;
                if (timeUp > 0)
                {
                    for (int j = 0; j < portals.portals.Length; j++)
                    {               

                        if (portals.portals[j].model.Equals(cube.playerPortal))
                        {
                            x = x + 1;
                            Closed = x;

                        }

                    }

                }

                cube.Update(gameTime, gameInPlay);

                eCube.updateTimer();

                eCube.Update(gameTime, gameInPlay, enemyMoves.Next(4));

                eCube2.updateTimer();

                eCube2.Update(gameTime, gameInPlay, enemyMoves.Next(4));

                for (int i = 0; i < portals.portals.Length; i++)
                {
                    if (cube.boundingSphere.Intersects(portals.portals[i].boundingSphere) && !(portals.portals[i].model.Equals(cube.playerPortal)))
                    {
                        portalEntry.Play();
                        cube.bump();
                        cube.setPenelty(GameConstants.bump);
                    }

                }

                for (int i = 0; i < portals.portals.Length; i++)
                {
                    if (eCube.boundingSphere.Intersects(portals.portals[i].boundingSphere3))
                    {

                        eCube.setJump(true);
                        eCube.portal(coneChoice.Next(11));//will the ai jump over the portal or enter it
                    }

                }

                for (int i = 0; i < portals.portals.Length; i++)
                {
                    if (eCube2.boundingSphere.Intersects(portals.portals[i].boundingSphere3))
                    {

                        eCube2.setJump(true);
                        eCube2.portal(coneChoice.Next(11));//will the ai jump over the portal or enter it
                    }

                }



                for (int i = 0; i < portals.portals.Length; i++)
                {
                    if (eCube.boundingSphere.Intersects(portals.portals[i].boundingSphere)&&!(portals.portals[i].model.Equals(cube.playerPortal)))
                    {
                        //Console.WriteLine("Bumped");
                        eCube.bump();
                        eCube.setPenelty(GameConstants.bump);
                    }

                }

                for (int i = 0; i < portals.portals.Length; i++)
                {
                    if (eCube2.boundingSphere.Intersects(portals.portals[i].boundingSphere) && !(portals.portals[i].model.Equals(cube.playerPortal)))
                    {
                       // Console.WriteLine("Bumped");
                        eCube2.bump();
                        eCube2.setPenelty(GameConstants.bump);
                    }

                }

                for (int i = 0; i < portals.portals.Length; i++)
                {
                    if (cube.boundingSphere.Intersects(portals.portals[i].boundingSphere2))
                    {
                        if (!(portals.portals[i].model.Equals(cube.playerPortal)))
                        {
                            portalClose.Play();
                        }
                        portals.changePortal(i, cube.playerPortal);
                        portals.portals[i].boundingSphere2.Radius = 100f;

                    }

                }

                for (int i = 0; i < portals.portals.Length; i++)
                {
                    if (cube.boundingSphere2.Intersects(portals.portals[i].boundingSphere2))
                    {
                        portals.changePortal(i, cube.playerPortal);
                        portals.portals[i].boundingSphere2.Radius = 100f;

                    }

                }

                for (int i = 0; i < portals.portals.Length; i++)
                {
                    if (eCube.boundingSphere.Intersects(portals.portals[i].boundingSphere2))
                    {
                        portals.changePortal(i, eCube.playerPortal);
                        portals.portals[i].boundingSphere2.Radius = 50f;
                    }

                }

                for (int i = 0; i < portals.portals.Length; i++)
                {
                    if (eCube2.boundingSphere.Intersects(portals.portals[i].boundingSphere2))
                    {
                        portals.changePortal(i, eCube2.playerPortal);
                        portals.portals[i].boundingSphere2.Radius = 50f;
                    }

                }


                if (timeUp <= 0)
                {

                    for (int j = 0; j < portals.portals.Length; j++)
                    {

                        if (portals.portals[j].model.Equals(cube.playerPortal))
                        {
                            cube.setTotalPortals(GameConstants.change);
                            cube.setFinalScore();

                        }
                    }

                    for (int j = 0; j < portals.portals.Length; j++)
                    {
                        if (portals.portals[j].model.Equals(eCube.playerPortal))
                        {
                            eCube.setTotalPortals(GameConstants.change);
                            eCube.setFinalScore();

                        }
                    }

                    if (cube.getTotalPortals >= numberToClose)
                    {
                        gameState = GameState.PREPARELEVEL;
                        score = score + cube.getFinalScore;
                        penalty = penalty + cube.getPenelty;
                    }
                    else
                    {

                        gameState = GameState.GAMEOVER;
                        score = score + cube.getFinalScore;
                        penalty = penalty + cube.getPenelty;

                    }

                   
                } 

               

            }

            if (gameState == GameState.GAMEOVER)
            {
                Console.Out.WriteLine("GAMEOVER STATE");

                if (keybord.IsKeyDown(Keys.Enter) && !(lastkey.IsKeyDown(Keys.Enter)))
                {
                  
                    timeUp = 90000f;
                    charge = 40000f;
                    gameState = GameState.STARTSCREEN;
                }
            }

            lastkey = keybord;
            base.Update(gameTime);
 
      }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {

            if (gameState == GameState.STARTSCREEN)
            {
                graphics.GraphicsDevice.Clear(Color.RoyalBlue);

                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
                spriteBatch.DrawString(kootenay, "Press Enter To Start\n\nPress C to view Controls\n\nHold space bar to read story", textPos, Color.Black);                
                spriteBatch.End();

                if (story == true)
                {

                    spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
                    spriteBatch.Draw(storyImg, new Vector2(300,20), null, Color.RoyalBlue);                    
                    spriteBatch.End();
                    
                }

            }

            if (gameState == GameState.CONTROLS)
            {
                graphics.GraphicsDevice.Clear(Color.White);
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
                spriteBatch.Draw(controls, Vector2.Zero, null, Color.White);
                spriteBatch.DrawString(kootenay, "Press B to go Back", new Vector2(300, 4), Color.Black);
                spriteBatch.End();
 
            }

            if (gameState == GameState.PAUSE) 
            {
                
                graphics.GraphicsDevice.Clear(Color.Firebrick);
                if (cheat1 == true)
                {
                    spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
                    spriteBatch.DrawString(kootenay, "infinte maelstrom activated", new Vector2(500,10), Color.Black);
                    spriteBatch.End();  
                }
                if (cheat2 == true)
                {
                    spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
                    spriteBatch.DrawString(kootenay, "Level time reduced to 2 seconds", new Vector2(500, 40), Color.Black);
                    spriteBatch.End();
                }
                if (cheat3 == true)
                {
                    spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
                    spriteBatch.DrawString(kootenay, "Maelstrom has been reset", new Vector2(500, 10), Color.Black);
                    spriteBatch.End();
                }
                if (cheat4 == true)
                {
                    spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
                    spriteBatch.DrawString(kootenay, "Press T again to add another 10 seconds", new Vector2(500, 40), Color.Black);
                    spriteBatch.End();
                }

                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);                
                spriteBatch.DrawString(kootenay, "Press Enter to continue", textPos, Color.Black);
                spriteBatch.End();

                GraphicsDevice.BlendState = BlendState.Opaque;
                GraphicsDevice.DepthStencilState = DepthStencilState.Default;
                GraphicsDevice.RasterizerState = RasterizerState.CullCounterClockwise;
                GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;
            }

            if(gameState == GameState.PREPARELEVEL)
            {
                graphics.GraphicsDevice.Clear(Color.RoyalBlue);
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
                spriteBatch.DrawString(kootenay, "Press Enter To Start Next Level", textPos, Color.Black);
                spriteBatch.End();

                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
                spriteBatch.DrawString(kootenay, "\nYour score so far \n" + score + "\nPenelty for entering portals & Drops: " + penalty, textPos, Color.Black);
                spriteBatch.End();


            }

            if(gameState == GameState.PLAYING)
            {
            GraphicsDevice.Clear(Color.Black);
            spaceSkybox.draw(graphics, viewMatrix, projectionMatrix);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
            spriteBatch.DrawString(kootenay, "Level: " + level +"\nNumber of Portals " + numberOfPortals+
                "\nYou need to Close "+ numberToClose+": Closed "+Closed,
                Vector2.Zero, Color.White);

            spriteBatch.DrawString(kootenay,"MALESTROM:", new Vector2(690,1), Color.White);

            float scalef = (float)((timeUp) / 90000f);
            spriteBatch.Draw(timerBar, new Vector2(100, 3), null, Color.White, 0.0f, new Vector2(0, 0), new Vector2(scalef, 1f),
                SpriteEffects.None, 0f);
            if (maelstrom > 0)
            {
                float scalef1 = (float)((maelstrom) / 5000f);
                spriteBatch.Draw(maelstromBar, new Vector2(800, 3), null, Color.White, 0.0f, new Vector2(0, 0), new Vector2(scalef1, 1f),
                    SpriteEffects.None, 0f);
            }           
           else if (maelstrom <= 0)
            {
                float scalef2 = (float)((charge) / recharge);
                Console.WriteLine(scalef2);
                spriteBatch.Draw(maelstromBar, new Vector2(800, 3), null, Color.Silver, 0.0f, new Vector2(0, 0), new Vector2(scalef2, 1f),
                    SpriteEffects.None, 0f);
            }
           
            spriteBatch.End();

           

            GraphicsDevice.BlendState = BlendState.Opaque;
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            GraphicsDevice.RasterizerState = RasterizerState.CullCounterClockwise;
            GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;

            

            
           
            camera.FollowCam(cube.getPosition, 0, 200f, 800f);
            //camera.ThirdPersonCam(cube.rotation, cube.position, GraphicsDevice);

            viewMatrix = Matrix.CreateLookAt(camera.camPosition, camera.camLookAt, camera.worldUp);
           
            cube.Draw(gameTime, graphics, viewMatrix, projectionMatrix);
            eCube.Draw(gameTime, graphics, viewMatrix, projectionMatrix);
            eCube2.Draw(gameTime, graphics, viewMatrix, projectionMatrix);

            portals.Initialize(graphics, viewMatrix, projectionMatrix,gameTime);
            ground.draw(graphics, viewMatrix, projectionMatrix);         

           

            
            }

            if (gameState == GameState.GAMEOVER)
            {
                graphics.GraphicsDevice.Clear(Color.RoyalBlue);
                
                
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
                spriteBatch.DrawString(kootenay, "Final score\n" + score + "\nPenelty for entering portals & Drops: "
                    + penalty + "\n\n press Enter to go to Start Screen", textPos, Color.Black);
                spriteBatch.End();               

                GraphicsDevice.BlendState = BlendState.Opaque;
                GraphicsDevice.DepthStencilState = DepthStencilState.Default;
                GraphicsDevice.RasterizerState = RasterizerState.CullCounterClockwise;
                GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;
                
                

            }
           


            base.Draw(gameTime);
        }
    }
}
