#region File Description
#endregion

#region Using Statements
using System;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.IO;
using TowerDefenseData;
#endregion

namespace TowerDefense
{
    class GameplayScreen : GameScreen
    {

        #region Initialization

        private GameStartDescription mGameStartDescription = null;
        private SaveGameDescription mSaveGameDescription = null;

        /// <summary>
        /// Default constructor
        /// </summary>
        private GameplayScreen()
            : base()
        {
            this.Exiting += new EventHandler(GameplayScreen_Exiting);
        }

        /// <summary>
        /// Create a new GameplayScreen from a new-game description
        /// </summary>
        /// <param name="aGameStartDescription"></param>
        public GameplayScreen(GameStartDescription aGameStartDescription)
            : this()
        {
            mGameStartDescription = aGameStartDescription;
            mSaveGameDescription = null;
        }

        /// <summary>
        /// Create a new GameplayScreen from a saved-game description
        /// </summary>
        /// <param name="aSaveGameDescription"></param>
        public GameplayScreen(SaveGameDescription aSaveGameDescription)
            : this()
        {
            this.mGameStartDescription = null;
            this.mSaveGameDescription = aSaveGameDescription;
        }

        /// <summary>
        /// Handle the closing of this screen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GameplayScreen_Exiting(object sender, EventArgs e)
        {
            //make sure the session is ending
            // --EndSession must be re-entrant safe, as the EndSession may be
            // making this screen close itself
            Session.EndSession();
        }

        /// <summary>
        /// Loads graphics content for the game.
        /// </summary>
        public override void LoadContent()
        {
            if (mGameStartDescription != null)
            {
                Session.StartNewSession(mGameStartDescription, ScreenManager, this);
            }
            else if (mSaveGameDescription != null)
            {
                Session.LoadSession(mSaveGameDescription, ScreenManager, this);
            }

            //Once the load has finished, we use ResetElapsedTime to tell the game's
            //timing mechanism that we have just finished a very long frame, and that
            //it should not try to catch up
            ScreenManager.Game.ResetElapsedTime();
        }

        #endregion //Initialization

        #region Update and Draw

        /// <summary>
        /// Updates the state of the game. This method checks the GameScreen.IsActive
        /// property, so the game will stop updating when the pause menu is active, or if you tab
        /// away to a different application.
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="otherScreenHasFocus"></param>
        /// <param name="coveredByOtherScreen"></param>
        public override void Update(GameTime aGameTime, bool aOtherScreenHasFocus, bool aCoveredByOtherScreen)
        {
            base.Update(aGameTime, aOtherScreenHasFocus, aCoveredByOtherScreen);

            if (IsActive && !aCoveredByOtherScreen)
            {
                Session.Update(aGameTime);
            }
        }

        /// <summary>
        /// Lets the game respond to player input. Unlike the update method,
        /// this will only be called when the gameplay screen is active.
        /// </summary>
        public override void HandleInput()
        {
            if (InputManager.isActionTriggered(InputManager.Action.MainMenu))
            {
                ScreenManager.AddScreen(new MainMenuScreen());
                return;
            }

            if (InputManager.isActionTriggered(InputManager.Action.ExitGame))
            {
                //confirmation message box
                const string lMessage = "Are you sure you want to exit? All unsaved progress will be lost.";
                MessageBoxScreen lConfirmExitMessageBox = new MessageBoxScreen(lMessage);
                lConfirmExitMessageBox.Accepted += ConfirmExitMessageBoxAccepted;
                ScreenManager.AddScreen(lConfirmExitMessageBox);
                return;
            }
        }

        /// <summary>
        /// Event handler for when the user selects Yes on the
        /// exit confirmation box.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ConfirmExitMessageBoxAccepted(object sender, EventArgs e)
        {
            ScreenManager.Game.Exit();
        }

        /// <summary>
        /// Draws the gameplay screen.
        /// </summary>
        /// <param name="aGameTime"></param>
        public override void Draw(GameTime aGameTime)
        {
            Session.Draw(aGameTime);
        }

        #endregion //Update and Draw
    }
}
