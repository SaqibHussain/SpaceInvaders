/*
  File:	     	 SpaceInvader
  Description:   Space Invader Game
  Author:        Saqib Hussain S10509668
  Organisation:  School of CTN, Birmingham City University
  Email:         saqib.hussain@mail.bcu.au.uk
  Copyright:     Copyright Saqib Hussain 2011
*/

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
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace SpaceInvader
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class SpaceInvaders : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D InvaderFireImg;
        Texture2D StarfieldImg;
        Texture2D InvaderImg;
        Texture2D RocketLauncherImg;
        Texture2D MissileImg;
        Texture2D SpaceShipImg;
        int RocketXPos;
        int AlienDirection;
        int AlienSpeed;
        Invader[,] Invaders;
        double Ticks;
        Missile MissileFired;
        int PlayerScore;
        SpriteFont GameFont;
        SoundEffect ExplosionSound;
        SoundEffectInstance ExplosionSoundInstance;
        SoundEffect ShootSound;
        SoundEffectInstance ShootSoundInstance;
        SoundEffect MoveSound;
        SoundEffectInstance MoveSoundInstance;
        int GameState;
        int YPosCount;
        int SpaceShipXPos;
        string SpaceShipOn;
        int Lives;
        string InvaderOn;
        int InvaderFireXPos;
        int InvaderFireYPos;

        public SpaceInvaders()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 1024;
            graphics.PreferredBackBufferHeight = 768;
            graphics.ApplyChanges();
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            InitializeLives();
            InitialiseGameVariables();
            GameState = 1;
            base.Initialize();
        }
        //Seperate life initialisation to avoid error when invaders meet launcher.
        public void InitializeLives()
        {
            Lives = 3;
        }
        public void InitialiseGameVariables()
        {

            SpaceShipOn = "off";
            InvaderOn = "off";
            RocketXPos = 512;
            AlienDirection = -1;
            AlienSpeed = 4;
            Invaders = new Invader[5, 11];
            YPosCount = 200;
            for (int Count1 = 0; Count1 < 5; Count1++)
            {
                YPosCount = YPosCount - 25;
                int XPos = 512;
                for (int Count = 0; Count < 11; Count++)
                {

                    Invaders[Count1, Count] = new Invader();
                    Invaders[Count1, Count].SetXPos(XPos);
                    Invaders[Count1, Count].SetYPos(YPosCount);
                    XPos = XPos + 32;
                }

            }

            Ticks = 0;

            MissileFired = null;

            PlayerScore = 0;
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            StarfieldImg = Content.Load<Texture2D>("Starfield1024x768");
            InvaderImg = Content.Load<Texture2D>("inv1");
            InvaderFireImg = Content.Load<Texture2D>("InvaderFire");
            SpaceShipImg = Content.Load<Texture2D>("SpaceShip");
            RocketLauncherImg = Content.Load<Texture2D>("LaserBase");
            MissileImg = Content.Load<Texture2D>("bullet");
            GameFont = Content.Load<SpriteFont>("GameFont");
            ExplosionSound = Content.Load<SoundEffect>("explosion");
            ExplosionSoundInstance = ExplosionSound.CreateInstance();
            ShootSound = Content.Load<SoundEffect>("shoot");
            ShootSoundInstance = ShootSound.CreateInstance();
            MoveSound = Content.Load<SoundEffect>("fastinvader1");
            MoveSoundInstance = MoveSound.CreateInstance();
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

        public void UpdateStarted(GameTime currentTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                GameState = 2;
            }

        }

        public void UpdatePlaying(GameTime currentTime)
        {

            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            {
                GameState = 3;

                return;
            }

            // These statements check to see if there are any invaders remaining to shoot
            bool IsInvaderRemaining = false;
            for (int Count1 = 0; Count1 < 5; Count1++)
            {
                for (int Count = 0; Count < 11; Count++)
                {

                    if (Invaders[Count1, Count] != null)
                    {
                        IsInvaderRemaining = true;
                        break;
                    }
                }
            }

            // If there are no invaders then move to end game state
            if (!IsInvaderRemaining)
            {
                GameState = 3;

                return;
            }

            if (MissileFired != null)
            {
                MissileFired.Move();
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                MissileFired = new Missile(RocketXPos, 650);
                ShootSoundInstance.Play();
            }

            // TODO: Add your update logic here
            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                RocketXPos = RocketXPos - 4;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                RocketXPos = RocketXPos + 4;
            }

            if (RocketXPos < 100)
            {
                RocketXPos = 100;
            }

            if (RocketXPos > 924)
            {
                RocketXPos = 924;
            }


            Ticks = Ticks + currentTime.ElapsedGameTime.TotalMilliseconds;

            if (Ticks > 500)
            {
                MoveSoundInstance.Play();
                for (int Count1 = 0; Count1 < 5; Count1++)
                {
                    for (int Count = 0; Count < 11; Count++)
                    {
                        if (Invaders[Count1, Count] != null)
                        {
                            Invaders[Count1, Count].MoveHorizontal(AlienSpeed * AlienDirection);
                        }
                    }
                }

                Invader LeftMostInvader = null;
                Invader RightMostInvader = null;
                for (int Count1 = 0; Count1 < 5; Count1++)
                {
                    for (int Count = 0; Count < 11; Count++)
                    {
                        if (Invaders[Count1, Count] != null)
                        {
                            LeftMostInvader = Invaders[Count1, Count];
                            break;
                        }
                    }
                }
                for (int Count1 = 0; Count1 < 5; Count1++)
                {
                    for (int Count = 10; Count > 0; Count--)
                    {
                        if (Invaders[Count1, Count] != null)
                        {
                            RightMostInvader = Invaders[Count1, Count];
                            break;
                        }
                    }
                }

                if (LeftMostInvader.GetXPos() < 96)
                {
                    AlienDirection = +1;
                    for (int Count1 = 0; Count1 < 5; Count1++)
                    {
                        int XPos = 96;
                        for (int Count = 0; Count < 11; Count++)
                        {
                            if (Invaders[Count1, Count] != null)
                            {
                                Invaders[Count1, Count].MoveVertical(8);
                                Invaders[Count1, Count].SetXPos(XPos);
                            }
                            XPos = XPos + InvaderImg.Width;
                        }
                    }
                }

                if (RightMostInvader.GetXPos() > 924)
                {
                    AlienDirection = -1;
                    for (int Count1 = 0; Count1 < 5; Count1++)
                    {
                        int XPos = 924 - InvaderImg.Width * 10;
                        for (int Count = 0; Count < 11; Count++)
                        {
                            if (Invaders[Count1, Count] != null)
                            {
                                Invaders[Count1, Count].MoveVertical(8);
                                Invaders[Count1, Count].SetXPos(XPos);
                            }

                            XPos = XPos + InvaderImg.Width;
                        }
                    }
                }

                Ticks = 0;
            }
            //Kill invader when missile hits
            if (MissileFired != null)
            {
                Rectangle rectMissile = new Rectangle((int)MissileFired.GetPosition().X, (int)MissileFired.GetPosition().Y, MissileImg.Width, MissileImg.Height);
                //Bottom three rows worth 100 points
                for (int Count1 = 0; Count1 < 3; Count1++)
                {
                    for (int Count = 0; Count < 11; Count++)
                    {
                        if (Invaders[Count1, Count] != null)
                        {
                            Rectangle rectInvader = new Rectangle(Invaders[Count1, Count].GetXPos(), Invaders[Count1, Count].GetYPos(), InvaderImg.Width, InvaderImg.Height);

                            if (rectMissile.Intersects(rectInvader))
                            {
                                Invaders[Count1, Count] = null;
                                MissileFired = null;
                                PlayerScore = PlayerScore + 100;
                                ExplosionSoundInstance.Play();

                                break;
                            }
                        }
                    }
                }
                //Top two rows worth 200 points
                for (int Count1 = 3; Count1 < 5; Count1++)
                {
                    for (int Count = 0; Count < 11; Count++)
                    {
                        if (Invaders[Count1, Count] != null)
                        {
                            Rectangle rectInvader = new Rectangle(Invaders[Count1, Count].GetXPos(), Invaders[Count1, Count].GetYPos(), InvaderImg.Width, InvaderImg.Height);

                            if (rectMissile.Intersects(rectInvader))
                            {
                                Invaders[Count1, Count] = null;
                                MissileFired = null;
                                PlayerScore = PlayerScore + 200;
                                ExplosionSoundInstance.Play();

                                break;
                            }
                        }
                    }
                }
            }

            // End game when invaders reach lanucher
            Rectangle rectLauncher = new Rectangle((int)RocketXPos, (int)650, RocketLauncherImg.Width, RocketLauncherImg.Height);
            for (int Count1 = 0; Count1 < 5; Count1++)
            {
                for (int Count = 0; Count < 11; Count++)
                {
                    if (Invaders[Count1, Count] != null)
                    {
                        Rectangle rectInvader = new Rectangle(Invaders[Count1, Count].GetXPos(), Invaders[Count1, Count].GetYPos(), InvaderImg.Width, InvaderImg.Height);

                        if (rectLauncher.Intersects(rectInvader))
                        {

                            Lives = Lives - 1;
                            InitialiseGameVariables();
                            break;
                        }
                    }
                }
            }

            //Increase speed when Invaders at a certain X Postion
            if (AlienSpeed < 8)
            {
                for (int Count1 = 0; Count1 < 5; Count1++)
                {
                    for (int Count = 0; Count < 11; Count++)
                    {

                        if (Invaders[Count1, Count] != null)
                        {
                            if (Invaders[Count1, Count].GetYPos() > 300)
                            {
                                AlienSpeed = 8;
                                break;
                            }
                        }
                    }
                }
            }

            //Random SpaceShip
            System.Random RandNum = new System.Random();
            int RandomShip = RandNum.Next(1000);
            if (RandomShip > 500 && SpaceShipOn != "on")
            {
                SpaceShipXPos = 0 - SpaceShipImg.Width;
                SpaceShipOn = "on";
            }
            if (SpaceShipOn == "on") { SpaceShipXPos = SpaceShipXPos + 8; }
            if (SpaceShipXPos > StarfieldImg.Width + SpaceShipImg.Width)
            {
                SpaceShipOn = "off";
            }
            //SpaceShip Kill
            if (MissileFired != null && SpaceShipOn != "off")
            {
                Rectangle rectMissile = new Rectangle((int)MissileFired.GetPosition().X, (int)MissileFired.GetPosition().Y, MissileImg.Width, MissileImg.Height);
                Rectangle rectSpaceShip = new Rectangle((int)SpaceShipXPos, 10, SpaceShipImg.Width, SpaceShipImg.Height);
                if (rectMissile.Intersects(rectSpaceShip))
                {
                    SpaceShipOn = "off";
                    PlayerScore = PlayerScore + 1000;

                }
            }
            //Invader Fire
            //Pick random number
            System.Random RandNum2 = new System.Random();
            int RandomAlien = RandNum2.Next(1000);
            if (RandomAlien > 9996 && InvaderOn != "on")
            {
                //Assign random Invader
                int InvaderA = RandNum.Next(0, 5);
                int InvaderB = RandNum.Next(0, 10);
                if ((Invaders[InvaderA, InvaderB]) != null)
                {
                    InvaderFireXPos = (Invaders[InvaderA, InvaderB]).GetXPos();
                    InvaderFireYPos = (Invaders[InvaderA, InvaderB]).GetYPos();
                    InvaderOn = "on";
                }
            }
            if (InvaderOn == "on")
            {
                InvaderFireYPos = InvaderFireYPos + 8;
                Rectangle rectInvaderFire = new Rectangle((int)InvaderFireXPos, InvaderFireYPos, InvaderFireImg.Width, InvaderFireImg.Height);
                if (rectLauncher.Intersects(rectInvaderFire))
                {
                    Lives = Lives - 1;
                    InvaderOn = "off";
                }
                //Check if the bullet has gone off screen and reset
                if (InvaderFireYPos > StarfieldImg.Height) { InvaderOn = "off"; }
            }
            //End game when all lives lost - dislay end game screen.
            if (Lives == 0) { GameState = 3; }

        }


        public void UpdateEnded(GameTime currentTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.X))
            {
                this.Exit();
            }

            if (Keyboard.GetState().IsKeyDown(Keys.R))
            {
                InitialiseGameVariables();
                InitializeLives();

                GameState = 1;
            }

        }

        protected override void Update(GameTime gameTime)
        {
            switch (GameState)
            {
                case 1: UpdateStarted(gameTime);
                    break;

                case 2: UpdatePlaying(gameTime);
                    break;

                case 3: UpdateEnded(gameTime);
                    break;
            }

            base.Update(gameTime);
        }


        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>

        public void DrawStarted(GameTime currentTime)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(StarfieldImg, Vector2.Zero, Color.White);
            Vector2 StringDimensions = GameFont.MeasureString("S P A C E   I N V A D E R S!");
            int XPos = (1024 - (int)StringDimensions.X) / 2;
            spriteBatch.DrawString(GameFont, "S P A C E   I N V A D E R S!", new Vector2(XPos, 200), Color.LightGreen);
            StringDimensions = GameFont.MeasureString("P R E S S   'S'   T O    S T A R T");
            XPos = (1024 - (int)StringDimensions.X) / 2;
            spriteBatch.DrawString(GameFont, "P R E S S   'S'   T O    S T A R T", new Vector2(XPos, 300), Color.LightGreen);
            StringDimensions = GameFont.MeasureString("U S E  T H E  A R R O W  K E Y S  T O  N A V I G A T E  Y O U R  L A U N C H E R");
            XPos = (1024 - (int)StringDimensions.X) / 2;
            spriteBatch.DrawString(GameFont, "U S E  T H E  A R R O W  K E Y S  T O  N A V I G A T E  Y O U R  L A U N C H E R", new Vector2(XPos, 400), Color.LightGreen);
            StringDimensions = GameFont.MeasureString("U S E  T H E  S P A C E B A R  T O  F I R E  M I S S I L E S");
            XPos = (1024 - (int)StringDimensions.X) / 2;
            spriteBatch.DrawString(GameFont, "U S E  T H E  S P A C E B A R  T O  F I R E  M I S S I L E S", new Vector2(XPos, 450), Color.LightGreen);
            StringDimensions = GameFont.MeasureString("A N D  K I L L  T H E  I N V A D E R S");
            XPos = (1024 - (int)StringDimensions.X) / 2;
            spriteBatch.DrawString(GameFont, "A N D  K I L L  T H E  I N V A D E R S", new Vector2(XPos, 500), Color.LightGreen);
            spriteBatch.End();
        }

        public void DrawPlaying(GameTime currentTime)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(StarfieldImg, Vector2.Zero, Color.White);
            spriteBatch.Draw(RocketLauncherImg, new Vector2(RocketXPos, 650), Color.White);
            if (SpaceShipOn == "on") { spriteBatch.Draw(SpaceShipImg, new Vector2(SpaceShipXPos, 10), Color.White); }
            if (InvaderOn == "on") { spriteBatch.Draw(InvaderFireImg, new Vector2(InvaderFireXPos, InvaderFireYPos), Color.White); }
            if (MissileFired != null)
            {
                Vector2 MissilePos = new Vector2(MissileFired.GetPosition().X, MissileFired.GetPosition().Y - MissileImg.Height);
                spriteBatch.Draw(MissileImg, MissilePos, Color.White);
            }

            for (int Count1 = 0; Count1 < 5; Count1++)
            {
                for (int Count = 0; Count < 11; Count++)
                {
                    if (Invaders[Count1, Count] != null)
                    {
                        spriteBatch.Draw(InvaderImg, Invaders[Count1, Count].GetPos(), Color.White);
                    }
                }
            }

            string ScoreText = String.Format("Score = {0}", PlayerScore);
            spriteBatch.DrawString(GameFont, ScoreText, new Vector2(10, 10), Color.White);
            string LivesText = String.Format("Lives = {0}", Lives);
            spriteBatch.DrawString(GameFont, LivesText, new Vector2(150, 10), Color.White);
            spriteBatch.End();
        }

        public void DrawEnded(GameTime currentTime)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(StarfieldImg, Vector2.Zero, Color.White);
            string FinalScoreString = String.Format("Final score = {0}", PlayerScore);
            Vector2 StringDimensions = GameFont.MeasureString(FinalScoreString);
            int XPos = (1024 - (int)StringDimensions.X) / 2;
            spriteBatch.DrawString(GameFont, FinalScoreString, new Vector2(XPos, 200), Color.LightGreen);
            StringDimensions = GameFont.MeasureString("P R E S S   'R'   T O    R E S T A R T   G A M E");
            XPos = (1024 - (int)StringDimensions.X) / 2;
            spriteBatch.DrawString(GameFont, "P R E S S   'R'   T O    R E S T A R T   G A M E", new Vector2(XPos, 300), Color.LightGreen);
            StringDimensions = GameFont.MeasureString("P R E S S   'X'   T O    E X I T   G A M E");
            XPos = (1024 - (int)StringDimensions.X) / 2;
            spriteBatch.DrawString(GameFont, "P R E S S   'X'   T O    E X I T   G A M E", new Vector2(XPos, 400), Color.LightGreen);
            spriteBatch.End();
        }

        protected override void Draw(GameTime gameTime)
        {
            switch (GameState)
            {
                case 1: DrawStarted(gameTime);
                    break;

                case 2: DrawPlaying(gameTime);
                    break;

                case 3: DrawEnded(gameTime);
                    break;
            }

            base.Draw(gameTime);
        }
    }
}
