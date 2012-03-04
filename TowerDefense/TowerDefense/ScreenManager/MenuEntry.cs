#region File Description
#endregion //File Description

#region Using Statements
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
#endregion //Using Statements

namespace TowerDefense
{
    /// <summary>
    /// Helper class represents a single entry in a MenuScreen. By default this
    /// just draws the entry text string, but it can be customized to display menu
    /// entries in different ways. This also provides an event that will be raised
    /// when the menu entry is selected.
    /// </summary>
    /// <remarks>
    /// Similar to a class found in the Game State Management sample on the 
    /// XNA Creators Club Online website (http://creators.xna.com).
    /// </remarks>
    class MenuEntry
    {

        #region Fields

        /// <summary>
        /// The text rendered for this entry.
        /// </summary>
        private string mText;

        /// <summary>
        /// The font used for this menu item.
        /// </summary>
        private SpriteFont mFont;

        /// <summary>
        /// The position of this menu item on the screen.
        /// </summary>
        private Vector2 mPos;

        /// <summary>
        /// A description of the function of the button.
        /// </summary>
        private string mDescription;

        /// <summary>
        /// An optional texture drawn with the text.
        /// </summary>
        /// <remarks>
        /// If present, the text will be centered on the texture.
        /// </remarks>
        private Texture2D mTexture;

        #endregion //Fields

        #region Properties

        /// <summary>
        /// The text display for this menu entry.
        /// </summary>
        public string Text
        {
            get { return mText; }
            set { mText = value; }
        }

        /// <summary>
        /// The font used to draw this menu entry
        /// </summary>
        public SpriteFont Font
        {
            get { return mFont; }
            set { mFont = value; }
        }

        /// <summary>
        /// The position at which to draw this menu entry.
        /// </summary>
        public Vector2 Position
        {
            get { return mPos; }
            set { mPos = value; }
        }

        /// <summary>
        /// The description of the function of this menu entry.
        /// </summary>
        public string Description
        {
            get { return mDescription; }
            set { mDescription = value; }
        }

        /// <summary>
        /// An option texture drawn with the text
        /// </summary>
        /// <remarks>
        /// If present, the text will be centered on the texture.
        /// </remarks>
        public Texture2D Texture
        {
            get { return mTexture; }
            set { mTexture = value; }
        }

        #endregion //Properties

        #region Events

        /// <summary>
        /// Event raised when the menu entry is selected.
        /// </summary>
        public event EventHandler<EventArgs> Selected;

        /// <summary>
        /// Method for raiding the Selected event.
        /// </summary>
        protected internal virtual void OnSelectEntry()
        {
            if (Selected != null)
            {
                Selected(this, EventArgs.Empty);
            }
        }

        #endregion //Events

        #region Initialization

        /// <summary>
        /// Constructs a new menu entry with the specified text.
        /// </summary>
        /// <param name="aText"></param>
        public MenuEntry(string aText)
        {
            this.mText = aText;
        }

        #endregion //Initialization

        #region Update and Draw

        /// <summary>
        /// Updates the menu entry.
        /// </summary>
        /// <param name="aScreen"></param>
        /// <param name="aIsSelected"></param>
        /// <param name="aGameTime"></param>
        public virtual void Update(MenuScreen aScreen, bool aIsSelected, GameTime aGameTime) { }

        /// <summary>
        /// Draws the menu entry. This can be overridden to customize the appearance.
        /// </summary>
        /// <param name="aScreen"></param>
        /// <param name="aIsSelected"></param>
        /// <param name="aGameTime"></param>
        public virtual void Draw(MenuScreen aScreen, bool aIsSelected, GameTime aGameTime)
        {
            //Draw selected entries in yellow, otherwise white
            Color lTextColor = aIsSelected ? Fonts.MenuSelectedColor : Fonts.TitleColor;

            //Draw text, ceneted on the middle of each line.
            ScreenManager lScreenManager = aScreen.ScreenManager;
            SpriteBatch lSpriteBatch = lScreenManager.SpriteBatch;

            
            if (mTexture != null)
            { //If we have a texture draw it and center the text on that texture
                lSpriteBatch.Draw(mTexture, mPos, Color.White);
                if ((mFont != null) && !String.IsNullOrEmpty(mText))
                {
                    Vector2 lTextSize = mFont.MeasureString(mText);
                    Vector2 lTextPos = mPos + new Vector2(
                        (float)Math.Floor((mTexture.Width - lTextSize.X) / 2),
                        (float)Math.Floor((mTexture.Height - lTextSize.Y) / 2));
                    lSpriteBatch.DrawString(mFont, mText, lTextPos, lTextColor);
                }
            }
            else if ((mFont != null) && !String.IsNullOrEmpty(mText))
            { //Otherwise just draw the text at the specified position
                lSpriteBatch.DrawString(mFont, mText, mPos, lTextColor);
            }
        }

        /// <summary>
        /// Queries how much vertical space this menu entry requires.
        /// </summary>
        /// <param name="aScreen"></param>
        /// <returns></returns>
        public virtual int GetHeight(MenuScreen aScreen)
        {
            return mFont.LineSpacing;
        }

        #endregion //Update and Draw
    }
}
