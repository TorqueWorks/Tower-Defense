#region File Description
#endregion

#region Using Statements
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
#endregion

namespace TowerDefense.MenuScreens
{
    /// <summary>
    /// The loading screen coordinates transitions between the menu system and the
    /// game itself. Normally one screen will transition off at the same time as
    /// the next screen is transitioning on, but for larger transitions that can
    /// take a longer time to load their data, we want the menu system to be entirely
    /// gone before we start loading the game. This is done as follows:
    /// 
    /// - Tell all the existing screens to transition off.
    /// - Activate a loading screen, which will transition on at the same time.
    /// - The loading screen watches the state of the previous screens.
    /// - When it sees they have finished transitioning off, it activates the real
    ///   next screen, which may take a long time to load its data. The loading
    ///   screen will be the only thing displayed while this load is taking place.
    /// </summary>
    /// <remarks>
    /// Similar to a class found in the Game State Management sample on the 
    /// XNA Creators Club Online website (http://creators.xna.com).
    /// </remarks>
    public class LoadingScreen : GameScreen
    {
        #region Screens Data

        bool mLoadingIsSlow;
        bool mOtherScreensAreGone;

        GameScreen[] mScreensToLoad;

        #endregion //Screens Data

        #region Graphics Data

        private Texture2D mLoadingTexture;
        private Vector2 mLoadingPos;

        private Texture2D mLoadingBlackTexture;
        private Rectangle mLoadingBlackTextureDestination;

        #endregion //Graphics Data

        #region Initialization

        /// <summary>
        /// Private constructor; loading screens should be activated via the static Load
        /// method instead
        /// </summary>
        /// <param name="aScreenManager"></param>
        /// <param name="mLoadingIsSlow"></param>
        /// <param name="aScreensToLoad"></param>
        private LoadingScreen(ScreenManager aScreenManager, bool aLoadingIsSlow, GameScreen[] aScreensToLoad)
        {
            mLoadingIsSlow = aLoadingIsSlow;
            mScreensToLoad = aScreensToLoad;

            TransitionOnTime = TimeSpan.FromSeconds(0.5);
        }

        /// <summary>
        /// Activates the loading screen. Will transition all currently active screens off.
        /// </summary>
        /// <param name="aScreenManager"></param>
        /// <param name="aLoadingIsSlow"></param>
        /// <param name="aScreensToLoad"></param>
        public static void Load(ScreenManager aScreenManager, bool aLoadingIsSlow, params GameScreen[] aScreensToLoad)
        {
            //Tell all the current screens to transition off
            foreach(GameScreen lScreen in aScreenManager.GetScreens())
            {
                lScreen.ExitScreen();
            }

            //Create and activate the loading screen
            LoadingScreen lLoadingScreen = new LoadingScreen(aScreenManager, aLoadingIsSlow, aScreensToLoad);
            aScreenManager.AddScreen(lLoadingScreen);
        }

        public override void LoadContent()
        {
            ContentManager lContent = ScreenManager.Game.Content;
            mLoadingTexture = lContent.Load<Texture2D>(@"Textures\MainMenu\LoadingPause");
            mLoadingBlackTexture =
                lContent.Load<Texture2D>(@"Textures\GameScreens\FadeScreen");
            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
            mLoadingBlackTextureDestination = new Rectangle(viewport.X, viewport.Y,
                viewport.Width, viewport.Height);
            mLoadingPos = new Vector2(
                viewport.X + (float)Math.Floor((viewport.Width -
                    mLoadingTexture.Width) / 2f),
                viewport.Y + (float)Math.Floor((viewport.Height -
                    mLoadingTexture.Height) / 2f));

            base.LoadContent();
        }

        #endregion //Initialization

        #region Update and Draw

        /// <summary>
        /// Updates the loading screen
        /// </summary>
        /// <param name="aGameTime"></param>
        /// <param name="aOtherScreenHasFocus"></param>
        /// <param name="aCoveredByOtherScreen"></param>
        public override void Update(GameTime aGameTime, bool aOtherScreenHasFocus, bool aCoveredByOtherScreen)
        {
            base.Update(aGameTime, aOtherScreenHasFocus, aCoveredByOtherScreen);

            //If all the previous screens have finished transitioning off, it
            //is time to actually perform the load
            if (mOtherScreensAreGone)
            {
                ScreenManager.RemoveScreen(this);

                foreach (GameScreen lScreen in mScreensToLoad)
                {
                    if (lScreen != null)
                    {
                        ScreenManager.AddScreen(lScreen);
                    }
                }

                //Once the load has finished, we use ResetElapsedTime to tell
                //the game timing mechanism that we have just finished a very 
                //long frame, and that it should not try and catch up
                ScreenManager.Game.ResetElapsedTime();
            }
        }

        /// <summary>
        /// Draws the loading screen
        /// </summary>
        /// <param name="aGameTime"></param>
        public override void Draw(GameTime aGameTime)
        {
            // If we are the only active screen, that means all the previous screens
            // must have finished transitioning off. We check for this in the Draw
            // method, rather than in Update, because it isn't enough just for the
            // screens to be gone: in order for the transition to look good we must
            // have actually drawn a frame without them before we perform the load.
            if ((ScreenState == ScreenState.Active) &&
                (ScreenManager.GetScreens().Length == 1))
            {
                mOtherScreensAreGone = true;
            }

            // The gameplay screen takes a while to load, so we display a loading
            // message while that is going on, but the menus load very quickly, and
            // it would look silly if we flashed this up for just a fraction of a
            // second while returning from the game to the menus. This parameter
            // tells us how long the loading is going to take, so we know whether
            // to bother drawing the message.
            if (mLoadingIsSlow)
            {
                SpriteBatch lSpriteBatch = ScreenManager.SpriteBatch;

                //Center the text in the viewport
                Viewport lViewport = ScreenManager.GraphicsDevice.Viewport;
                Vector2 lViewportSize = new Vector2(lViewport.Width, lViewport.Height);

                Color lColor = new Color(255, 255, 255, TransitionAlpha);

                lSpriteBatch.Begin();
                lSpriteBatch.Draw(mLoadingBlackTexture, mLoadingBlackTextureDestination, Color.White);
                lSpriteBatch.Draw(mLoadingTexture, mLoadingPos, Color.White);
                lSpriteBatch.End();
            }
        }
        #endregion //Update and Draw

    }
}
