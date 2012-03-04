#region File Description

#endregion

#region Using Statements
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using TowerDefenseData;
#endregion

namespace TowerDefense
{
    /// <summary>
    /// Displays the in-game controls to the user
    /// </summary>
    class ControlsScreen : GameScreen
    {
        #region Private Types

        /// <summary>
        /// Holds the GamePad control info to display
        /// </summary>
        private struct GamePadInfo
        {
            public string mText;
            public Vector2 mTextPos;
        }

        /// <summary>
        /// Holds the Keyboard control info to display
        /// </summary>
        private struct KeyboardInfo
        {
            public InputManager.ActionMap[] mTotalActionList;
            public int mSelectedIndex;
        }

        #endregion //Private Types

        #region Graphics Data

        private Texture2D mBackgroundTexture;
        private Texture2D mPlankTexture;

        private Vector2 mPlankPos;
        private Vector2 mTitlePos;
        private Vector2 mActionPos;
        private Vector2 mKey1Pos;
        private Vector2 mKey2Pos;

        private Texture2D mBaseBorderTexture;
        private Vector2 mBaseBorderPos = new Vector2(200, 570);
        
        private Texture2D mScrollUpTexture;
        private Texture2D mScrollDownTexture;
        private Vector2 mScrollUpPos = new Vector2(990, 235);
        private Vector2 mScrollDownPos = new Vector2(990, 490);
        
        private Texture2D mRightTriggerButton;
        private Texture2D mLeftTriggerButton;
        private Vector2 mRightTriggerPos;
        private Vector2 mLeftTriggerPos;

        private Texture2D mControlPadTexture;
        private Vector2 mControlPadPos = new Vector2(450, 180);

        private Texture2D mKeyboardTexture;
        private Vector2 mKeyboardPos = new Vector2(305, 185);

        private float mChartLine1Pos;
        private float mChartLine2Pos;
        private float mChartLine3Pos;
        private float mChartLine4Pos;

        private Texture2D mBackTexture;
        private readonly Vector2 mBackPosition = new Vector2(225, 610);

        #endregion //Graphics Data

        #region Control Display Data

        private bool mIsShowControlPad;

        private GamePadInfo[] mLeftStrings = new GamePadInfo[7];
        private GamePadInfo[] mRightStrings = new GamePadInfo[6];
        private KeyboardInfo mKeyboardInfo;

        private int mStartIndex = 0;
        private const int mMaxActionDisplay = 6;

        #endregion //Control Display Data

        #region Initialization

        /// <summary>
        /// Creates a new ControlsScreenObject
        /// </summary>
        public ControlsScreen()
            : base()
        {
            TransitionOnTime = TimeSpan.FromSeconds(0.5);

            mChartLine1Pos = mKeyboardPos.X + 30;
            mChartLine2Pos = mKeyboardPos.X + 340;
            mChartLine3Pos = mKeyboardPos.X + 510;
            mChartLine4Pos = mKeyboardPos.X + 670;

            mIsShowControlPad = true;
        }

        /// <summary>
        /// Loads the graphics content required for the screen
        /// </summary>
        public override void LoadContent()
        {
            Viewport lViewport = ScreenManager.GraphicsDevice.Viewport;
            mKeyboardInfo.mTotalActionList = InputManager.ActionMaps;
            mKeyboardInfo.mSelectedIndex = 0;

            const int lLeftStringsPos = 450;
            const int lRightStringsPos = 818;

            float lHeight = Fonts.DescriptionFont.LineSpacing - 5;

            //Set the data for gamepad control to display

            mLeftStrings[0].mText = "Page Left";
            mLeftStrings[0].mTextPos = new Vector2(lLeftStringsPos - Fonts.DescriptionFont.MeasureString(mLeftStrings[0].mText).X, 170);

            mLeftStrings[1].mText = "N/A";
            mLeftStrings[1].mTextPos = new Vector2(lLeftStringsPos -
                Fonts.DescriptionFont.MeasureString(mLeftStrings[1].mText).X, 220);

            mLeftStrings[2].mText = "Main Menu";
            mLeftStrings[2].mTextPos = new Vector2(lLeftStringsPos -
                Fonts.DescriptionFont.MeasureString(mLeftStrings[2].mText).X, 290);

            mLeftStrings[3].mText = "Exit Game";
            mLeftStrings[3].mTextPos = new Vector2(lLeftStringsPos -
                Fonts.DescriptionFont.MeasureString(mLeftStrings[3].mText).X, 340);

            mLeftStrings[4].mText = "Navigation";
            mLeftStrings[4].mTextPos = new Vector2(lLeftStringsPos -
                Fonts.DescriptionFont.MeasureString(mLeftStrings[4].mText).X, 400);

            mLeftStrings[5].mText = "Navigation";
            mLeftStrings[5].mTextPos = new Vector2(lLeftStringsPos -
                Fonts.DescriptionFont.MeasureString(mLeftStrings[5].mText).X, 455);

            mLeftStrings[6].mText = "N/A";
            mLeftStrings[6].mTextPos = new Vector2(lLeftStringsPos -
                Fonts.DescriptionFont.MeasureString(mLeftStrings[6].mText).X, 510);


            mRightStrings[0].mText = "Page Right";
            mRightStrings[0].mTextPos = new Vector2(lRightStringsPos, 170);

            mRightStrings[1].mText = "N/A";
            mRightStrings[1].mTextPos = new Vector2(lRightStringsPos, 230);

            mRightStrings[2].mText = "Character Management";
            mRightStrings[2].mTextPos = new Vector2(lRightStringsPos, 295);

            mRightStrings[3].mText = "Back";
            mRightStrings[3].mTextPos = new Vector2(lRightStringsPos, 355);

            mRightStrings[4].mText = "OK";
            mRightStrings[4].mTextPos = new Vector2(lRightStringsPos, 435);

            mRightStrings[5].mText = "Drop Gear";
            mRightStrings[5].mTextPos = new Vector2(lRightStringsPos, 510);

            ContentManager content = ScreenManager.Game.Content;
            mBackgroundTexture =
                content.Load<Texture2D>(@"Textures\MainMenu\MainMenu");
            mKeyboardTexture =
                content.Load<Texture2D>(@"Textures\MainMenu\KeyboardBkgd");
            mPlankTexture =
                content.Load<Texture2D>(@"Textures\MainMenu\MainMenuPlank03");
            mBackTexture =
                content.Load<Texture2D>(@"Textures\Buttons\BButton");
            mBaseBorderTexture =
                content.Load<Texture2D>(@"Textures\GameScreens\LineBorder");
            mControlPadTexture =
                content.Load<Texture2D>(@"Textures\MainMenu\ControlJoystick");
            mScrollUpTexture =
                content.Load<Texture2D>(@"Textures\GameScreens\ScrollUp");
            mScrollDownTexture =
                content.Load<Texture2D>(@"Textures\GameScreens\ScrollDown");
            mRightTriggerButton =
                content.Load<Texture2D>(@"Textures\Buttons\RightTriggerButton");
            mLeftTriggerButton =
                content.Load<Texture2D>(@"Textures\Buttons\LeftTriggerButton");

            mPlankPos.X = mBackgroundTexture.Width / 2 - mPlankTexture.Width / 2;
            mPlankPos.Y = 60;

            mRightTriggerPos.X = 900;
            mRightTriggerPos.Y = 50;

            mLeftTriggerPos.X = 320;
            mLeftTriggerPos.Y = 50;

            base.LoadContent();
        }
        #endregion //Initialization

        #region Updating

        /// <summary>
        /// Handles user input
        /// </summary>
        public override void HandleInput()
        {
            //Exit the screen
            if (InputManager.isActionTriggered(InputManager.Action.Back))
            {
                ExitScreen();
            }
#if !XBOX
            //toggle between keyboard and gamepad controls
            else if (InputManager.isActionTriggered(InputManager.Action.PageLeft) ||
                InputManager.isActionTriggered(InputManager.Action.PageRight))
            {
                mIsShowControlPad = !mIsShowControlPad;
            }
            //scroll through keyboard controls
            if (mIsShowControlPad == false)
            {
                if (InputManager.isActionTriggered(InputManager.Action.CursorDown))
                {
                    if (mStartIndex < mKeyboardInfo.mTotalActionList.Length - mMaxActionDisplay)
                    {
                        ++mStartIndex;
                        ++mKeyboardInfo.mSelectedIndex;
                    }
                }
                if (InputManager.isActionTriggered(InputManager.Action.CursorUp))
                {
                    if (mStartIndex > 0)
                    {
                        --mStartIndex;
                        --mKeyboardInfo.mSelectedIndex;
                    }
                }
            }
#endif
        }

        #endregion //Updating

        #region Drawing

        /// <summary>
        /// Draws the control screen
        /// </summary>
        /// <param name="aGameTime">Provides a snapshot of timing values</param>
        public override void Draw(GameTime aGameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            Vector2 textPosition = Vector2.Zero;

            spriteBatch.Begin();

            // Draw the background texture
            spriteBatch.Draw(mBackgroundTexture, Vector2.Zero, Color.White);

            // Draw the back icon and text
            spriteBatch.Draw(mBackTexture, mBackPosition, Color.White);
            spriteBatch.DrawString(Fonts.ButtonNamesFont, "Back",
                new Vector2(mBackPosition.X + 55, mBackPosition.Y + 5), Color.White);

            // Draw the plank
            spriteBatch.Draw(mPlankTexture, mPlankPos, Color.White);

#if !XBOX
            // Draw the trigger buttons
            spriteBatch.Draw(mLeftTriggerButton, mLeftTriggerPos, Color.White);
            spriteBatch.Draw(mRightTriggerButton, mRightTriggerPos, Color.White);
#endif

            // Draw the base border
            spriteBatch.Draw(mBaseBorderTexture, mBaseBorderPos, Color.White);

            // draw the control pad screen
            if (mIsShowControlPad)
            {
                spriteBatch.Draw(mControlPadTexture, mControlPadPos,
                    Color.White);

                for (int i = 0; i < mLeftStrings.Length; i++)
                {
                    spriteBatch.DrawString(Fonts.DescriptionFont, mLeftStrings[i].mText,
                        mLeftStrings[i].mTextPos, Color.Black);
                }

                for (int i = 0; i < mRightStrings.Length; i++)
                {
                    spriteBatch.DrawString(Fonts.DescriptionFont, mRightStrings[i].mText,
                        mRightStrings[i].mTextPos, Color.Black);
                }

#if !XBOX
                // Near left trigger
                spriteBatch.DrawString(Fonts.PlayerStatisticsFont, "Keyboard",
                    new Vector2(mLeftTriggerPos.X + (mLeftTriggerButton.Width -
                    Fonts.PlayerStatisticsFont.MeasureString("Keyboard").X) / 2,
                    mRightTriggerPos.Y + 85),
                    Color.Black);

                // Near right trigger
                spriteBatch.DrawString(Fonts.PlayerStatisticsFont, "Keyboard",
                    new Vector2(mRightTriggerPos.X + (mRightTriggerButton.Width -
                    Fonts.PlayerStatisticsFont.MeasureString("Keyboard").X) / 2,
                    mRightTriggerPos.Y + 85),
                    Color.Black);
#endif

                // Draw the title text
                mTitlePos.X = mPlankPos.X + (mPlankTexture.Width -
                    Fonts.HeaderFont.MeasureString("Gamepad").X) / 2;
                mTitlePos.Y = mPlankPos.Y + (mPlankTexture.Height -
                    Fonts.HeaderFont.MeasureString("Gamepad").Y) / 2;
                spriteBatch.DrawString(Fonts.HeaderFont, "Gamepad", mTitlePos,
                    Fonts.TitleColor);
            }
            else // draws the keyboard screen
            {
                const float spacing = 47;
                string keyboardString;

                spriteBatch.Draw(mKeyboardTexture, mKeyboardPos, Color.White);
                for (int j = 0, i = mStartIndex; i < mStartIndex + mMaxActionDisplay;
                    i++, j++)
                {
                    keyboardString = InputManager.getActionName((InputManager.Action)i);
                    textPosition.X = mChartLine1Pos +
                        ((mChartLine2Pos - mChartLine1Pos) -
                        Fonts.DescriptionFont.MeasureString(keyboardString).X) / 2;
                    textPosition.Y = 253 + (spacing * j);

                    // Draw the action
                    spriteBatch.DrawString(Fonts.DescriptionFont, keyboardString,
                        textPosition, Color.Black);

                    // Draw the key one
                    keyboardString =
                        mKeyboardInfo.mTotalActionList[i].mKeyboardKeys[0].ToString();
                    textPosition.X = mChartLine2Pos +
                        ((mChartLine3Pos - mChartLine2Pos) -
                        Fonts.DescriptionFont.MeasureString(keyboardString).X) / 2;
                    spriteBatch.DrawString(Fonts.DescriptionFont, keyboardString,
                        textPosition, Color.Black);

                    // Draw the key two
                    if (mKeyboardInfo.mTotalActionList[i].mKeyboardKeys.Count > 1)
                    {
                        keyboardString = mKeyboardInfo.mTotalActionList[i].
                            mKeyboardKeys[1].ToString();
                        textPosition.X = mChartLine3Pos +
                            ((mChartLine4Pos - mChartLine3Pos) -
                        Fonts.DescriptionFont.MeasureString(keyboardString).X) / 2;
                        spriteBatch.DrawString(Fonts.DescriptionFont, keyboardString,
                            textPosition, Color.Black);
                    }
                    else
                    {
                        textPosition.X = mChartLine3Pos +
                            ((mChartLine4Pos - mChartLine3Pos) -
                            Fonts.DescriptionFont.MeasureString("---").X) / 2;
                        spriteBatch.DrawString(Fonts.DescriptionFont, "---",
                            textPosition, Color.Black);
                    }
                }

                // Draw the Action
                mActionPos.X = mChartLine1Pos +
                    ((mChartLine2Pos - mChartLine1Pos) -
                        Fonts.CaptionFont.MeasureString("Action").X) / 2;
                mActionPos.Y = 200;
                spriteBatch.DrawString(Fonts.CaptionFont, "Action", mActionPos,
                    Fonts.CaptionColor);

                // Draw the Key 1
                mKey1Pos.X = mChartLine2Pos +
                    ((mChartLine3Pos - mChartLine2Pos) -
                    Fonts.CaptionFont.MeasureString("Key 1").X) / 2;
                mKey1Pos.Y = 200;
                spriteBatch.DrawString(Fonts.CaptionFont, "Key 1", mKey1Pos,
                    Fonts.CaptionColor);

                // Draw the Key 2
                mKey2Pos.X = mChartLine3Pos +
                    ((mChartLine4Pos - mChartLine3Pos) -
                    Fonts.CaptionFont.MeasureString("Key 2").X) / 2;
                mKey2Pos.Y = 200;
                spriteBatch.DrawString(Fonts.CaptionFont, "Key 2", mKey2Pos,
                    Fonts.CaptionColor);

                // Near left trigger
                spriteBatch.DrawString(Fonts.PlayerStatisticsFont, "Gamepad",
                    new Vector2(mLeftTriggerPos.X + (mLeftTriggerButton.Width -
                    Fonts.PlayerStatisticsFont.MeasureString("Gamepad").X) / 2,
                    mRightTriggerPos.Y + 85), Color.Black);

                // Near right trigger
                spriteBatch.DrawString(Fonts.PlayerStatisticsFont, "Gamepad",
                    new Vector2(mRightTriggerPos.X + (mRightTriggerButton.Width -
                    Fonts.PlayerStatisticsFont.MeasureString("Gamepad").X) / 2,
                    mRightTriggerPos.Y + 85), Color.Black);

                // Draw the title text
                mTitlePos.X = mPlankPos.X + (mPlankTexture.Width -
                    Fonts.HeaderFont.MeasureString("Keyboard").X) / 2;
                mTitlePos.Y = mPlankPos.Y + (mPlankTexture.Height -
                    Fonts.HeaderFont.MeasureString("Keyboard").Y) / 2;
                spriteBatch.DrawString(Fonts.HeaderFont, "Keyboard", mTitlePos,
                    Fonts.TitleColor);

                // Draw the scroll textures
                spriteBatch.Draw(mScrollUpTexture, mScrollUpPos, Color.White);
                spriteBatch.Draw(mScrollDownTexture, mScrollDownPos, Color.White);
            }

            spriteBatch.End();
        }
        #endregion //Drawing
    }
}
