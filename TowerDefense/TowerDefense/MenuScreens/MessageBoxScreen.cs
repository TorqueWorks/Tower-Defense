#region File Description
#endregion

#region Using Statements
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace TowerDefense
{
    /// <summary>
    /// A popup message box screen, used to display "are you sure?"
    /// confirmation messages.
    /// </summary>
    /// <remarks>
    /// Similar to a class found in the Game State Management sample on the 
    /// XNA Creators Club Online website (http://creators.xna.com).
    /// </remarks>
    class MessageBoxScreen : GameScreen
    {

        #region Fields

        private string mMessage;
        private Vector2 mMessagePos;

        private Texture2D mBackgroundTexture;
        private Vector2 mBackgroundPos;

        private Texture2D mLoadingBlackTexture;
        private Rectangle mLoadingBlackTextureDestination;

        private Texture2D mBackTexture;
        private Vector2 mBackPos;

        private Texture2D mSelectTexture;
        private Vector2 mSelectPosition;

        private Vector2 mConfirmPos;

        #endregion //Fields

        #region Events

        public event EventHandler<EventArgs> Accepted;
        public event EventHandler<EventArgs> Cancelled;

        #endregion //Events

        #region Initialization

        /// <summary>
        /// Constructor lets the caller specify the message.
        /// </summary>
        /// <param name="aMessage"></param>
        public MessageBoxScreen(string aMessage)
        {
            mMessage = aMessage;
            IsPopup = true;

            TransitionOnTime = TimeSpan.FromSeconds(0.2);
            TransitionOffTime = TimeSpan.FromSeconds(0.2);
        }

        /// <summary>
        /// Loads graphics lContent for this screen. This uses the shared ContentManager
        /// provided by the Game class, so the lContent will remain loaded forever.
        /// Whenever a subsequent MessageBoxScreen tries to load this same lContent, 
        /// it will just get back another reference to the already loaded data.
        /// </summary>
        public override void LoadContent()
        {
            ContentManager lContent = ScreenManager.Game.Content;

            mBackgroundTexture = lContent.Load<Texture2D>(@"Textures\MainMenu\Confirm");
            mBackTexture = lContent.Load<Texture2D>(@"Textures\Buttons\BButton");
            mSelectTexture = lContent.Load<Texture2D>(@"Textures\Buttons\AButton");
            mLoadingBlackTexture =
                lContent.Load<Texture2D>(@"Textures\GameScreens\FadeScreen");

            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
            mBackgroundPos = new Vector2(
                (viewport.Width - mBackgroundTexture.Width) / 2,
                (viewport.Height - mBackgroundTexture.Height) / 2);
            mLoadingBlackTextureDestination = new Rectangle(viewport.X, viewport.Y,
                viewport.Width, viewport.Height);

            mBackPos = mBackgroundPos + new Vector2(50f,
                mBackgroundTexture.Height - 100);
            mSelectPosition = mBackgroundPos + new Vector2(
                mBackgroundTexture.Width - 100, mBackgroundTexture.Height - 100);

            mConfirmPos.X = mBackgroundPos.X + (mBackgroundTexture.Width -
                Fonts.HeaderFont.MeasureString("Confirmation").X) / 2f;
            mConfirmPos.Y = mBackgroundPos.Y + 47;

            mMessage = Fonts.breakTextIntoLines(mMessage, 36, 10);
            mMessagePos.X = mBackgroundPos.X + (int)((mBackgroundTexture.Width -
                Fonts.GearInfoFont.MeasureString(mMessage).X) / 2);
            mMessagePos.Y = (mBackgroundPos.Y * 2) - 20;
        }
        #endregion //Initialization

        #region Handle Input

        /// <summary>
        /// Responds to user input, accepting or cancelling the message box.
        /// </summary>
        public override void HandleInput()
        {
            if (InputManager.isActionTriggered(InputManager.Action.Ok))
            {
                //Raise the accepted event, then exit the message box.
                if (Accepted != null)
                {
                    Accepted(this, EventArgs.Empty);
                }
                ExitScreen();
            }
            else if (InputManager.isActionTriggered(InputManager.Action.Back))
            {
                //Raise the cancelled event, then exit the message box
                if (Cancelled != null)
                {
                    Cancelled(this, EventArgs.Empty);
                }
                ExitScreen();
            }
        }
        #endregion //Handle Input

        #region Draw

        /// <summary>
        /// Draws the message box.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            SpriteBatch lSpriteBatch = ScreenManager.SpriteBatch;

            lSpriteBatch.Begin();

            lSpriteBatch.Draw(mLoadingBlackTexture, mLoadingBlackTextureDestination, Color.White);
            lSpriteBatch.Draw(mBackgroundTexture, mBackgroundPos, Color.White);
            lSpriteBatch.Draw(mBackTexture, mBackPos, Color.White);
            lSpriteBatch.Draw(mSelectTexture, mSelectPosition, Color.White);
            lSpriteBatch.DrawString(Fonts.ButtonNamesFont, "No",
                new Vector2(mBackPos.X + mBackTexture.Width + 5, mBackPos.Y + 5),
                Color.White);
            lSpriteBatch.DrawString(Fonts.ButtonNamesFont, "Yes",
                new Vector2(
                    mSelectPosition.X - Fonts.ButtonNamesFont.MeasureString("Yes").X,
                    mSelectPosition.Y + 5), Color.White);
            lSpriteBatch.DrawString(Fonts.HeaderFont, "Confirmation", mConfirmPos, Fonts.CountColor);
            lSpriteBatch.DrawString(Fonts.GearInfoFont, mMessage, mMessagePos, Fonts.CountColor);
            lSpriteBatch.End();
        }

        #endregion //Draw
    }
}
