using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TowerDefense
{
    /// <summary>
    /// Base class for screens that contain a menu of options. The user can
    /// move up and down to select an entry, or cancel to back out of the screen.
    /// </summary>
    /// <remarks>
    /// Similar to a class found in the Game State Management sample on the 
    /// XNA Creators Club Online website (http://creators.xna.com).
    /// </remarks>
    abstract class MenuScreen : GameScreen
    {

        #region Fields

        private List<MenuEntry> mMenuEntries = new List<MenuEntry>();
        protected int mSelectedEntry = 0;

        #endregion //Fields

        #region Properties

        /// <summary>
        /// Gets the list of menu entries, so derived classes can add
        /// or change the menu contents.
        /// </summary>
        protected IList<MenuEntry> MenuEntries
        {
            get { return mMenuEntries; }
        }

        /// <summary>
        /// Gets the currently selected menu entry if one exists.
        /// </summary>
        protected MenuEntry SelectedMenuEntry
        {
            get
            {
                if ((mSelectedEntry < 0) || (mSelectedEntry >= mMenuEntries.Count))
                {
                    return null;
                }
                return mMenuEntries[mSelectedEntry];
            }
        }
        #endregion //Properties

        #region Initialization


        /// <summary>
        /// Default Constructor
        /// </summary>
        public MenuScreen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(0.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
        }

        #endregion //Initialization

        #region Handle Input

        public override void  HandleInput()
        {
            int lOldSelectedEntry = mSelectedEntry;

            //Move to the previous menu entry?
            if (InputManager.isActionTriggered(InputManager.Action.CursorUp))
            {
                --mSelectedEntry;
                if (mSelectedEntry < 0)
                {
                    mSelectedEntry = mMenuEntries.Count - 1;
                }
            }

            //Move to the next menu entry?
            if(InputManager.isActionTriggered(InputManager.Action.CursorDown))
            {
                ++mSelectedEntry;
                if (mSelectedEntry >= mMenuEntries.Count)
                {
                    mSelectedEntry = 0;
                }
            }

            //Accept or cancel the menu?
            if (InputManager.isActionTriggered(InputManager.Action.Ok))
            {
                //TODO: Play sound here
                //AudioManager.PlayCue("Continue");
                OnSelectEntry();
            }
            else if (InputManager.isActionTriggered(InputManager.Action.Back) ||
                InputManager.isActionTriggered(InputManager.Action.ExitGame))
            {
                OnCancel();
            }
            else if (mSelectedEntry != lOldSelectedEntry)
            {
                //TODO: Play menu move sound here
                //AudioManager.PlayCue("MenuMove");
            }

        }

        /// <summary>
        /// Handler for when the user has chosen a menu entry.
        /// </summary>
        protected virtual void OnSelectEntry()
        {
            mMenuEntries[mSelectedEntry].OnSelectEntry();
        }

        /// <summary>
        /// Handler for when the user has cancelled the menu.
        /// </summary>
        protected virtual void OnCancel()
        {
            ExitScreen();
        }

        /// <summary>
        /// Helper overload makes it easy to use OnCancel as a MenuEntry event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnCancel(object sender, EventArgs e)
        {
            OnCancel();
        }

        #endregion //Handle Input

        #region Update and Draw

        public override void Update(GameTime aGameTime, bool aOtherScreenHasFocus, bool aCoveredByOtherScreen)
        {
            base.Update(aGameTime, aOtherScreenHasFocus, aCoveredByOtherScreen);

            //Update each of the MenuEntry objects
            for (int i = 0; i < mMenuEntries.Count; ++i)
            {
                bool lIsSelected = IsActive && (i == mSelectedEntry);

                mMenuEntries[i].Update(this, lIsSelected, aGameTime);
            }
        }

        /// <summary>
        /// Draws the menu.
        /// </summary>
        /// <param name="aGameTime"></param>
        public override void Draw(GameTime aGameTime)
        {
            SpriteBatch lSpriteBatch = ScreenManager.SpriteBatch;

            lSpriteBatch.Begin();

            //Draw each menu in turn
            for (int i = 0; i < mMenuEntries.Count; ++i)
            {
                MenuEntry lMenuEntry = mMenuEntries[i];
                bool lIsSelected = IsActive && (i == mSelectedEntry);

                lMenuEntry.Draw(this, lIsSelected, aGameTime);
            }

            lSpriteBatch.End();
        }
        #endregion //Update and Draw
    }
}
