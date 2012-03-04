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
    /// The main menu is the first thing displayed when the game starts up.
    /// </summary>
    class MainMenuScreen : MenuScreen
    {

        #region Graphics Data

        private Texture2D mBackgroundTexture;
        private Vector2 mBackgroundPos;

        private Texture2D mDescriptionAreaTexture;
        private Vector2 mDescriptionAreaPos;
        private Vector2 mDescriptionAreaTextPos;

        private Texture2D mIconTexture;
        private Vector2 mIconPos;

        private Texture2D mBackTexture;
        private Vector2 mBackPos;

        private Texture2D mSelectTexture;
        private Vector2 mSelectPos;

        private Texture2D mPlankTexture1, mPlankTexture2, mPlankTexture3;

        #endregion //Graphics Data

        #region Menu Entries

        private MenuEntry mNewGameMenuEntry, mExitGameMenuEntry;
        private MenuEntry mSaveGameMenuEntry, mLoadGameMenuEntry;
        private MenuEntry mControlsMenuEntry, mHelpMenuEntry;

        #endregion //Menu Entries

        #region Initialization

        /// <summary>
        /// Default constructor fills in the menu contents
        /// </summary>
        public MainMenuScreen()
            : base()
        {
            //New Game menu entry
            mNewGameMenuEntry = new MenuEntry("New Game");
            mNewGameMenuEntry.Description = "Start a New Game";
            mNewGameMenuEntry.Font = Fonts.HeaderFont;
            mNewGameMenuEntry.Position = new Vector2(715, 0f);
            mNewGameMenuEntry.Selected += NewGameMenuEntrySelected;
            MenuEntries.Add(mNewGameMenuEntry);

            //Save Game menu entry
            //Only show if the game has started
            if (Session.IsActive)
            {
                mSaveGameMenuEntry = new MenuEntry("Save Game");
                mSaveGameMenuEntry.Description = "Save the Game";
                mSaveGameMenuEntry.Font = Fonts.HeaderFont;
                mSaveGameMenuEntry.Position = new Vector2(730, 0f);
                mSaveGameMenuEntry.Selected += SaveGameMenuEntrySelected;
                MenuEntries.Add(mSaveGameMenuEntry);
            }
            else
            {
                mSaveGameMenuEntry = null;
            }

            //Load game menu entry 
            mLoadGameMenuEntry = new MenuEntry("Load Game");
            mLoadGameMenuEntry.Description = "Load the Game";
            mLoadGameMenuEntry.Font = Fonts.HeaderFont;
            mLoadGameMenuEntry.Position = new Vector2(700,0f);
            mLoadGameMenuEntry.Selected += LoadGameMenuEntrySelected;
            MenuEntries.Add(mLoadGameMenuEntry);

            //Controls menu entry
            mControlsMenuEntry = new MenuEntry("Controls");
            mControlsMenuEntry.Description = "View Game Controls";
            mControlsMenuEntry.Font = Fonts.HeaderFont;
            mControlsMenuEntry.Position = new Vector2(720, 0f);
            mControlsMenuEntry.Selected += ControlsMenuEntrySelected;
            MenuEntries.Add(mControlsMenuEntry);

            //Help menu entry
            mHelpMenuEntry = new MenuEntry("Help");
            mHelpMenuEntry.Description = "View Game Help";
            mHelpMenuEntry.Font = Fonts.HeaderFont;
            mHelpMenuEntry.Position = new Vector2(700, 0f);
            mHelpMenuEntry.Selected += HelpMenuEntrySelected;
            MenuEntries.Add(mHelpMenuEntry);

            //Exit menu entry
            mExitGameMenuEntry = new MenuEntry("Exit");
            mExitGameMenuEntry.Description = "Quit the Game";
            mExitGameMenuEntry.Font = Fonts.HeaderFont;
            mExitGameMenuEntry.Position = new Vector2(720, 0f);
            mExitGameMenuEntry.Selected += OnCancel;
            MenuEntries.Add(mExitGameMenuEntry);

            //TODO: Audio
            //AudioManager.PushMusic("MainTheme");

        }

        /// <summary>
        /// Load the graphics content for the screen
        /// </summary>
        public override void LoadContent()
        {
            //Load the textures
            ContentManager lContent = ScreenManager.Game.Content;
            mBackgroundTexture = lContent.Load<Texture2D>(@"Textures\MainMenu\MainMenu");
            mDescriptionAreaTexture = lContent.Load<Texture2D>(@"Textures\MainMenu\MainMenuInfoSpace");
            mIconTexture = lContent.Load<Texture2D>(@"Textures\MainMenu\GameLogo");
            mPlankTexture1 = lContent.Load<Texture2D>(@"Textures\MainMenu\MainMenuPlank");
            mPlankTexture2 = lContent.Load<Texture2D>(@"Textures\MainMenu\MainMenuPlank02");
            mPlankTexture3 = lContent.Load<Texture2D>(@"Textures\MainMenu\MainMenuPlank03");
            mBackTexture = lContent.Load<Texture2D>(@"Textures\Buttons\BButton");
            mSelectTexture = lContent.Load<Texture2D>(@"Textures\Buttons\AButton");

            //Calculate the texture positions
            Viewport lViewport = ScreenManager.GraphicsDevice.Viewport;
            mBackgroundPos = new Vector2(
                (lViewport.Width - mBackgroundTexture.Width) / 2,
                (lViewport.Height - mBackgroundTexture.Height) / 2);
            mDescriptionAreaPos = mBackgroundPos + new Vector2(158, 130);
            mDescriptionAreaTextPos = mBackgroundPos + new Vector2(158, 350);
            mIconPos = mBackgroundPos + new Vector2(170, 80);
            mBackPos = mBackgroundPos + new Vector2(225, 610);
            mSelectPos = mBackgroundPos + new Vector2(1120, 610);

            //Set the textures on each menu entry
            mNewGameMenuEntry.Texture = mPlankTexture3;
            if (mSaveGameMenuEntry != null)
            {
                mSaveGameMenuEntry.Texture = mPlankTexture2;
            }
            mLoadGameMenuEntry.Texture = mPlankTexture1;
            mControlsMenuEntry.Texture = mPlankTexture2;
            mHelpMenuEntry.Texture = mPlankTexture3;
            mExitGameMenuEntry.Texture = mPlankTexture1;

            //Now that they have textures, set the proper positions on the menu entries
            for (int i = 0; i < MenuEntries.Count; ++i)
            {
                MenuEntries[i].Position = new Vector2(
                    MenuEntries[i].Position.X,
                    500f - ((MenuEntries[i].Texture.Height - 10) *
                        (MenuEntries.Count - 1 - i)));
            }

            base.LoadContent();
        }
        #endregion //Initialization

        #region Updating

        /// <summary>
        /// Handles user input.
        /// </summary>
        public override void HandleInput()
        {
            if (InputManager.isActionTriggered(InputManager.Action.Back) &&
                Session.IsActive)
            {
                //TODO: Stop sound
                //AudioManager.PopMusic();
                ExitScreen();
                return;
            }

            base.HandleInput();
        }

        /// <summary>
        /// Event handler for when the New Game menu entry is selected
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NewGameMenuEntrySelected(object sender, EventArgs e)
        {
            if (Session.IsActive)
            {
                ExitScreen();
            }

            ContentManager lContent = ScreenManager.Game.Content;
            //TODO: Loading screen here
            //LoadScreen.Load(ScreenManager, true, new GameplayScreen(lContent.Load<GameStartDescription>("MainGameDescription")));
        }

        /// <summary>
        /// Event handler for when the Save Game menu entry is selected
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveGameMenuEntrySelected(object sender, EventArgs e)
        {
            //TODO: Add Save/Load screen
            //ScreenManager.AddScreen(new SaveLoadScreen(SaveLoadScreen.SaveLoadScreenMode.Save));
        }

        /// <summary>
        /// Event handler for when the Load Game menu entry is selected
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoadGameMenuEntrySelected(object sender, EventArgs e)
        {
            //TODO: Add Save/Load screen
            //SaveLoadScreen lLoadGameScreen = new SaveLoadScreen(SaveLoadScreen.SaveLoadScreenMode.Load);
            //lLoadGameScreen.LoadingSaveGame += new SaveLoadScreen.LoadingSaveGameHandler(loadGameScreen_LoadingSaveGame);
            //ScreenManager.AddScreen(lLoadGameScreen);
        }

        /* TODO: Add Save/Load screen
       
        private void loadGameScreen_LoadingSaveGame(SaveGameDescription aSaveGameDescription)
        {
            if (Session.IsActive)
            {
                ExitScreen();
            }
            LoadingScreen.Load(ScreenManager, true, new GamePlayScreen(aSaveGameDescription));
        }
         **/

        /// <summary>
        /// Event handler for when the Controls menu entry is selected.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ControlsMenuEntrySelected(object sender, EventArgs e)
        {
            ScreenManager.AddScreen(new ControlsScreen());
        }

        /// <summary>
        /// Event handler for when the Help menu entry is selected.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HelpMenuEntrySelected(object sender, EventArgs e)
        {
            //TODO: Add help screen
            //ScreenManager.AddScreen(new HelpScreen());
        }

        /// <summary>
        /// When the user cancels the main menu, or when
        /// the Exit GAme menu entry is selected
        /// </summary>
        protected override void OnCancel()
        {
            //add a confirmation message box
            string lMessage = String.Empty;
            if (Session.IsActive)
            {
                lMessage = "Are you sure you want to exit? All unsaved progress will be lost.";
            }
            else
            {
                lMessage = "Are you sure you want to exit?";
            }
            MessageBoxScreen lConfirmExitMessageBox = new MessageBoxScreen(lMessage);
            lConfirmExitMessageBox.Accepted += ConfirmExitMessageBoxAccepted;
            ScreenManager.AddScreen(lConfirmExitMessageBox);
        }

        /// <summary>
        /// Event handler for when the user selected Yes on the quit
        /// confirmation message box.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ConfirmExitMessageBoxAccepted(object sender, EventArgs e)
        {
            ScreenManager.Game.Exit();
        }

        #endregion //Updating

        #region Drawing

        public override void Draw(GameTime aGameTime)
        {
            SpriteBatch lSpriteBatch = ScreenManager.SpriteBatch;

            lSpriteBatch.Begin();

            //Draw the background images
            lSpriteBatch.Draw(mBackgroundTexture, mBackgroundPos, Color.White);

            //Draw each menu entry in turn
            for (int i = 0; i < MenuEntries.Count; ++i)
            {
                MenuEntry lMenuEntry = MenuEntries[i];
                bool lIsSelected = IsActive && (i == mSelectedEntry);
                lMenuEntry.Draw(this, lIsSelected, aGameTime);
            }

            //Draw the description text for the selected entry
            MenuEntry lSelectedMenuEntry = SelectedMenuEntry;
            if ((lSelectedMenuEntry != null) &&
                !String.IsNullOrEmpty(lSelectedMenuEntry.Description))
            {
                Vector2 lTextSize = Fonts.DescriptionFont.MeasureString(lSelectedMenuEntry.Description);
                Vector2 lTextPos = mDescriptionAreaPos + new Vector2(
                    (float)Math.Floor((mDescriptionAreaTexture.Width - lTextSize.X) / 2),
                    0f);
                lSpriteBatch.DrawString(Fonts.DescriptionFont, lSelectedMenuEntry.Description, lTextPos, Color.White);
            }

            //Draw the select instruction
            lSpriteBatch.Draw(mSelectTexture, mSelectPos, Color.White);
            lSpriteBatch.DrawString(Fonts.ButtonNamesFont, "Select",
                new Vector2(
                    mSelectPos.X - Fonts.ButtonNamesFont.MeasureString("Select").X - 5,
                    mSelectPos.Y + 5), Color.White);

            //If we are in-game, draw the back instruction
            if (Session.IsActive)
            {
                lSpriteBatch.Draw(mBackTexture, mBackPos, Color.White);
                lSpriteBatch.DrawString(Fonts.ButtonNamesFont, "Resume",
                    new Vector2(mBackPos.X + 55, mBackPos.Y + 5), Color.White);
            }

            lSpriteBatch.End();
        }

        #endregion //Drawing
    }
}
