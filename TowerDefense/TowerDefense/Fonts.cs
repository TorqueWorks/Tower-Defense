using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace TowerDefense
{
    /// <summary>
    /// Static storage of SpriteFont objects and colors for use throughout the game.
    /// </summary>
    static class Fonts
    {

        #region Fonts

        private static SpriteFont mHeaderFont;
        public static SpriteFont HeaderFont
        {
            get { return mHeaderFont; }
        }

        private static SpriteFont mPlayerNameFont;
        public static SpriteFont PlayerNameFont
        {
            get { return mPlayerNameFont; }
        }

        private static SpriteFont mDebugFont;
        public static SpriteFont DebugFont
        {
            get { return mDebugFont; }
        }

        private static SpriteFont mButtonNamesFont;
        public static SpriteFont ButtonNamesFont
        {
            get { return mButtonNamesFont; }
        }

        private static SpriteFont mDescriptionFont;
        public static SpriteFont DescriptionFont
        {
            get { return mDescriptionFont; }
        }

        private static SpriteFont mGearInfoFont;
        public static SpriteFont GearInfoFont
        {
            get { return mGearInfoFont; }
        }

        private static SpriteFont mDamageFont;
        public static SpriteFont DamageFont
        {
            get { return mDamageFont; }
        }

        private static SpriteFont mPlayerStatisticsFont;
        public static SpriteFont PlayerStatisticsFont
        {
            get { return mPlayerStatisticsFont; }
        }

        private static SpriteFont mHudDetailFont;
        public static SpriteFont HudDetailFont
        {
            get { return mHudDetailFont; }
        }

        private static SpriteFont mCaptionFont;
        public static SpriteFont CaptionFont
        {
            get { return mCaptionFont; }
        }

        #endregion //Fonts


        #region Font Colors

        public static readonly Color CountColor = new Color(79, 24, 44);
        public static readonly Color TitleColor = new Color(59,18,6);
        public static readonly Color CaptionColor = new Color(228, 168, 57);
        public static readonly Color HighlightColor = new Color(223, 206, 148);
        public static readonly Color DisplayColor = new Color(68, 32, 19);
        public static readonly Color DescriptionColor = new Color(0, 0, 0);
        public static readonly Color RestrictionColor = new Color(0, 0, 0);
        public static readonly Color ModifierColor = new Color(0, 0, 0);
        public static readonly Color MenuSelectedColor = new Color(248, 218, 127);

        #endregion //Font Colors

        #region Initialization
        
        /// <summary>
        /// Load the fonts from the content pipeline.
        /// </summary>
        /// <param name="aContentManager"></param>
        public static void LoadContent(ContentManager aContentManager)
        {
            if(aContentManager == null)
            {
                throw new ArgumentNullException("contentManager");
            }

            //load each font from the content pipeline
            mButtonNamesFont = aContentManager.Load<SpriteFont>("Fonts/ButtonNamesFont");
            mCaptionFont = aContentManager.Load<SpriteFont>("Fonts/CaptionFont");
            mDamageFont = aContentManager.Load<SpriteFont>("Fonts/DamageFont");
            mDebugFont = aContentManager.Load<SpriteFont>("Fonts/DebugFont");
            mDescriptionFont = aContentManager.Load<SpriteFont>("Fonts/DescriptionFont");
            mGearInfoFont = aContentManager.Load<SpriteFont>("Fonts/GearInfoFont");
            mHeaderFont = aContentManager.Load<SpriteFont>("Fonts/HeaderFont");
            mHudDetailFont = aContentManager.Load<SpriteFont>("Fonts/HudDetailFont");
            mPlayerNameFont = aContentManager.Load<SpriteFont>("Fonts/PlayerNameFont");
            mPlayerStatisticsFont = 
                aContentManager.Load<SpriteFont>("Fonts/PlayerStatisticsFont");
        }

        /// <summary>
        /// Release all references to the fonts.
        /// </summary>
        public static void UnloadContent()
        {
            mButtonNamesFont = null;
            mCaptionFont = null;
            mDamageFont = null;
            mDebugFont = null;
            mDescriptionFont = null;
            mGearInfoFont = null;
            mHeaderFont = null;
            mHudDetailFont = null;
            mPlayerNameFont = null;
            mPlayerStatisticsFont = null;
        }
        
        #endregion //Initialization

        #region Text Helper Methods

        /// <summary>
        /// Adds newline characters to a string so that it fits within a certain size.
        /// </summary>
        /// <param name="aText">The text to be modified.</param>
        /// <param name="aMaxCharsPerLine">
        /// The maximum length of a single line of text.
        /// </param>
        /// <param name="aMaxLines">The maximum number of lines to draw.</param>
        /// <returns>The new string, with the newline characters, if needed.</returns>
        public static string breakTextIntoLines(string aText, int aMaxCharsPerLine, int aMaxLines)
        {
            if(aMaxLines <= 0)
            {
                throw new ArgumentOutOfRangeException("maxLines");
            }
            if(aMaxCharsPerLine <= 0)
            {
                throw new ArgumentOutOfRangeException("maxCharsPerLine");
            }

            //if the string is trivial, then this is really easy
            if(String.IsNullOrEmpty(aText))
            {
                return String.Empty;
            }

            //If the text is short enough to fit on one line, then this is still easy
            if(aText.Length < aMaxCharsPerLine)
            {
                return aText;
            }

            StringBuilder lStringBuilder = new StringBuilder(aText);
            int lCurLine = 0;
            int lNewLineIndex = 0;
            
            //Loop through string adding in carriage returns to split string into multiple lines
            while(((aText.Length - lNewLineIndex) > aMaxCharsPerLine) && //As long as we can't fit the rest into one line
                (lCurLine < aMaxLines))  //And we haven't hit the max lines yet
            {
                aText.IndexOf(' ', 0);
                int lNextIndex = lNewLineIndex;
                while ((lNextIndex >= 0) //While there's still spaces left...
                    && (lNextIndex < aMaxCharsPerLine)) //And we haven't hit max line length
                { //Loop through the words to find where the next carriage return goes
                    lNewLineIndex = lNextIndex;
                    lNextIndex = aText.IndexOf(' ', lNewLineIndex + 1);
                }
                //Split into separate lines!
                lStringBuilder.Replace(' ', '\n', lNewLineIndex, 1);
                ++lCurLine;
            }
            return lStringBuilder.ToString();
        }

        /// <summary>
        /// Adds new-line characters to a string to make it fit.
        /// </summary>
        /// <param name="aText">The text to be drawn.</param>
        /// <param name="aMaxCharsPerLine">
        /// The maximum length of a single line of text.
        /// </param>
        /// <returns>The new string with newline characters, if needed.</returns>
        public static string breakTextIntoLines(string aText, int aMaxCharsPerLine)
        {
            //Just use more specified version above, set max lines to length of text + 1 (since this number of
            //lines will never be reached by splitting this string) so that we achieve the goal of this method
            //which is to not have any restriction on the max lines.
            return breakTextIntoLines(aText, aMaxCharsPerLine, aText.Length + 1);
        }

        /// <summary>
        /// Break text up into separate lines to make it fit.
        /// </summary>
        /// <param name="aText">The text to be broken up.</param>
        /// <param name="aFont">The font used to measure the width of the text.</param>
        /// <param name="aRowWidth">The maximum width of each line, in pixels</param>
        /// <returns></returns>
        public static List<String> breakTextIntoList(String aText, SpriteFont aFont, int aRowWidth)
        {
            if(aFont == null)
            {
                throw new ArgumentNullException("font");
            }
            if(aRowWidth <= 0)
            {
                throw new ArgumentNullException("rowWidth");
            }

            List<string> lLines = new List<string>();

            //check for trivial text
            if(String.IsNullOrEmpty(aText))
            {
                lLines.Add(String.Empty);
                return lLines;
            }

            //Check for text that fits on a single line (also trivial)
            if(aFont.MeasureString(aText).X <= aRowWidth)
            {
                lLines.Add(aText);
                return lLines;
            }

            //break the text up into words
            string[]  lWords = aText.Split(' ');

            //Add words until they go over the length
            int lCurWord = 0;
            while(lCurWord < lWords.Length)
            { //As long as we have words to process...this will split the text into their separate lines
                int lWordsThisLine = 0;
                string lLine = String.Empty;
                while(lCurWord < lWords.Length)
                { //As long as we have words to process, this will add one line to the list
                    StringBuilder lTestLine = new StringBuilder(lLine);
                    if(lTestLine.Length < 1)
                    { //empty string, just add
                        lTestLine.Append(lWords[lCurWord]);
                    }
                    else if ((lTestLine[lTestLine.Length - 1] == '.') ||
                        (lTestLine[lTestLine.Length - 1] == '?') ||
                        (lTestLine[lTestLine.Length - 1] == '!'))
                    {
                        lTestLine.Append(' ');
                        lTestLine.Append(lWords[lCurWord]);
                    }
                    else
                    {
                        lTestLine.Append(' ');
                        lTestLine.Append(lWords[lCurWord]);
                    }
                    if((lWordsThisLine > 0) &&
                        (aFont.MeasureString(lTestLine.ToString()).X > aRowWidth))
                    { //We've reached the max row width so we're done with this line
                        break;
                    }
                    lLine = lTestLine.ToString();
                    ++lWordsThisLine;
                    ++lCurWord;
                }
                lLines.Add(lLine);
            }
            return lLines;
        }

        /// <summary>
        /// Returns a properly-formatted gold-quantity string.
        /// </summary>
        /// <param name="aGold"></param>
        /// <returns></returns>
        public static string getGoldString(int aGold)
        {
            return String.Format("{0:n0}", aGold);
        }

        #endregion //Text Helper Methods

        #region Drawing Helper Methods

        /// <summary>
        /// Draws text centered at the particular position.
        /// </summary>
        /// <param name="aSpriteBatch">The SpriteBatch object used to draw</param>
        /// <param name="aFont">The font used to draw the text</param>
        /// <param name="aText">The text to be drawn</param>
        /// <param name="aPos">The center position of the text.</param>
        /// <param name="aColor">The color of the text.</param>
        public static void drawCenteredText(SpriteBatch aSpriteBatch, SpriteFont aFont,
            string aText, Vector2 aPos, Color aColor)
        {
            if(aSpriteBatch == null)
            {
                throw new ArgumentNullException("spriteBatch");
            }
            if(aFont == null)
            {
                throw new ArgumentNullException("font");
            }

            //Check for trivial text
            if(String.IsNullOrEmpty(aText))
            {
                return;
            }

            //Calculate the centered position
            Vector2 lTextSize = aFont.MeasureString(aText);
            Vector2 lCenteredPos = new Vector2(
                aPos.X - (int)lTextSize.X / 2,
                aPos.Y - (int)lTextSize.Y / 2);

            //Draw the string
            aSpriteBatch.DrawString(aFont, aText, lCenteredPos, aColor, 0f,
                Vector2.Zero, 1f, SpriteEffects.None, 1f-aPos.Y / 720f);
        }
        #endregion //Drawing Helper Methods
    }
}
