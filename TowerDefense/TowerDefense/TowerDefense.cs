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

namespace TowerDefense
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class TowerDefense : Microsoft.Xna.Framework.Game
    {
        private GraphicsDeviceManager mGraphics;
        private ScreenManager mScreenManager;

        #region Constants

        private const int SCREEN_WIDTH = 1280;
        private const int SCREEN_HEIGHT = 720;

        #endregion //Constants

        public TowerDefense()
        {
            //Init the graphics system
            mGraphics = new GraphicsDeviceManager(this);
            mGraphics.PreferredBackBufferWidth = SCREEN_WIDTH;
            mGraphics.PreferredBackBufferHeight = SCREEN_HEIGHT;

            //Configure the content manager
            Content.RootDirectory = "Content";

            //add a gamer-services component, which is required for storage APIs
            Components.Add(new GamerServicesComponent(this));

            //TODO: Add the audio manager
            /**
             * AudioManager.Initialize(this, @"Content\Audio\RpgAudio.xgs", @"Content\Audio\Wave Bank.xwb", @"Content\Audio\Sound Bank.xsb");
             * */

            mScreenManager = new ScreenManager(this);
            Components.Add(mScreenManager);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            InputManager.initialize();

            base.Initialize();

            mScreenManager.AddScreen(new MainMenuScreen());
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            Fonts.LoadContent(Content);

            base.LoadContent();
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            Fonts.UnloadContent();

            base.UnloadContent();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            InputManager.Update();

            base.Update(gameTime);
        }

        #region Drawing
        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            mGraphics.GraphicsDevice.Clear(Color.Transparent);

            base.Draw(gameTime);
        }

        #endregion //Drawing


        #region Entry Point
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (TowerDefense game = new TowerDefense())
            {
                game.Run();
            }
        }

        #endregion //Entry Point
    }
}
