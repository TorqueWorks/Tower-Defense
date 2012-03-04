#region File Description
#endregion

#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
#endregion

namespace TowerDefense
{
    /// <summary>
    /// This class handles all keyboard and gamepad actions in the game.
    /// </summary>
    public static class InputManager
    {
        #region Action Enumeration
        
        /// <summary>
        /// The actions that are possible within the game
        /// </summary>
        public enum Action
        {
            MainMenu,
            Ok,
            Back,
            CharacterManagement,
            ExitGame,
            TakeView,
            DropUnEquip,
            MoveCharacterUp,
            MoveCharacterDown,
            MoveCharacterLeft,
            MoveCharacterRight,
            CursorUp,
            CursorDown,
            DecreaseAmount,
            IncreaseAmount,
            PageLeft,
            PageRight,
            TargetUp,
            TargetDown,
            ActiveCharacterLeft,
            ActiveCharacterRight,
            TotalActionCount,
        }

        /// <summary>
        /// Readable names of each action
        /// </summary>
        
        private static readonly string[] mActionNames = 
        {
            "Main Menu",
            "Ok", 
            "Back",
            "Character Management",
            "Exit Game",
            "Take / View",
            "Drop / Unequip", 
            "Move Character - Up",
            "Move Character - Down",
            "Move Character - Left",
            "Move Character - Right",
            "Move Cursor - Up",
            "Move Cursor - Down",
            "Decrease Amount",
            "Increase Amount",
            "Page Screen Left",
            "Page Screen Right",
            "Select Target - Up",
            "Select Target - Down",
            "Select Active Character - Left",
            "Select Active Character - Right",
        };

        ///<summary>
        ///Returns the readable name of the given action.
        ///</summary>
        public static string getActionName(Action aAction)
        {
            int lIndex = (int)aAction;
            if((lIndex < 0) || (lIndex > mActionNames.Length))
            {
                throw new IndexOutOfRangeException("action");
            }
            return mActionNames[lIndex];
        }
        #endregion Action Enumeration

        #region Support Types

        ///<summary>
        ///GamePad controls expressed as one type, unified with button sematics.
        ///</summary>
        public enum GamePadButtons
        {
            Start,
            Back,
            A,
            B,
            X,
            Y,
            Up,
            Down,
            Left,
            Right,
            LeftShoulder,
            RightShoulder,
            LeftTrigger,
            RightTrigger,
        }

        ///<summary>
        ///A combination of gamepad and keyboard keys mapped to a particular action
        ///</summary>
        public class ActionMap
        {
            ///<summary>
            ///List of GamePad controls to be mapped to given action.
            ///</summary>
            public List<GamePadButtons> mGamePadButtons = new List<GamePadButtons>();

            ///<summary>
            ///List of Keyboard controls to be mapped to a particuar action.
            ///</summary>
            public List<Keys> mKeyboardKeys = new List<Keys>();
        }

        #endregion //Support Types

        #region Constants

        ///<summary>
        ///The value of an analog control that reads as a "pressed button".
        ///</summary>
        const float mAnalogLimit = 0.5f;

        #endregion //Constants

        #region Keyboard Data

        ///<summary>
        ///The state of the keyboard as of the last update.
        ///</summary>
        private static KeyboardState mCurrentKeyboardState;

        ///<summary>
        ///The state of the keyboard as of the last update
        ///</summary>
        public static KeyboardState CurrentKeyboardState
        {
            get { return mCurrentKeyboardState; }
        }

        ///<summary>
        ///The state of the keyboard as of the previous update.
        ///</summary>
        private static KeyboardState mPreviousKeyboardState;

        ///<summary>
        ///Check if a key is pressed.
        ///</summary>
        public static bool isKeyPressed(Keys aKey)
        {
            return mCurrentKeyboardState.IsKeyDown(aKey);
        }

        ///<summary>
        ///Check if a key was just pressed in the most recent update.
        ///</summary>
        public static bool isKeyTriggered(Keys aKey)
        {
            return (mCurrentKeyboardState.IsKeyDown(aKey)) &&
                (!mPreviousKeyboardState.IsKeyDown(aKey));
        }

        #endregion //Keyboard Data

        #region GamePad Data

        ///<summary>
        ///The state of the GamePad as of the last update.
        ///</summary>
        private static GamePadState mCurrentGamePadState;

        ///<summary>
        ///The state of the GamePad as of the last update.
        ///</summary>
        public static GamePadState CurrentGamePadState
        {
            get { return mCurrentGamePadState; }
        }

        ///<summary>
        ///The state of the gamepad as of the previous update.
        ///</summary>
        private static GamePadState mPreviousGamePadState;


        #region GamePadButton Pressed Queries

        ///<summary>
        ///Check if the gamepad's Start button is pressed.
        ///</summary>
        public static bool isGamePadStartPressed()
        {
            return (mCurrentGamePadState.Buttons.Start == ButtonState.Pressed);
        }

        ///<summary>
        ///Check if the gamepad's Back button is pressed.
        ///</summary>
        public static bool isGamePadBackPressed()
        {
            return (mCurrentGamePadState.Buttons.Back == ButtonState.Pressed);
        }

        ///<summary>
        ///Check if the gamepad's A button is pressed.
        ///</summary>
        public static bool isGamePadAPressed()
        {
            return (mCurrentGamePadState.Buttons.A == ButtonState.Pressed);
        }

        /// <summary>
        /// Check if the gamepad's B button is pressed.
        /// </summary>
        /// <returns></returns>
        public static bool isGamePadBPressed()
        {
            return (mCurrentGamePadState.Buttons.B == ButtonState.Pressed);
        }

        /// <summary>
        /// Check if the gamepad's X button is pressed.
        /// </summary>
        /// <returns></returns>
        public static bool isGamePadXPressed()
        {
            return (mCurrentGamePadState.Buttons.X == ButtonState.Pressed);
        }

        /// <summary>
        /// Check if the gamepad's Y button is pressed.
        /// </summary>
        /// <returns></returns>
        public static bool isGamePadYPressed()
        {
            return (mCurrentGamePadState.Buttons.Y == ButtonState.Pressed);
        }

        /// <summary>
        /// Check if the gamepad's Right Shoulder button is pressed.
        /// </summary>
        /// <returns></returns>
        public static bool isGamePadRightShoulderPressed()
        {
            return (mCurrentGamePadState.Buttons.RightShoulder == ButtonState.Pressed);
        }

        /// <summary>
        /// Check if the gamepad's Left Shoulder button is pressed.
        /// </summary>
        /// <returns></returns>
        public static bool isGamePadLeftShoulderPressed()
        {
            return (mCurrentGamePadState.Buttons.LeftShoulder == ButtonState.Pressed);
        }

        /// <summary>
        /// Check if Up on the gamepad's directional pad is pressed.
        /// </summary>
        /// <returns></returns>
        public static bool isGamePadDPadUpPressed()
        {
            return (mCurrentGamePadState.DPad.Up == ButtonState.Pressed);
        }

        /// <summary>
        /// Check if Down on the gamepad's directional pad is pressed.
        /// </summary>
        /// <returns></returns>
        public static bool isGamePadDPadDownPressed()
        {
            return (mCurrentGamePadState.DPad.Down == ButtonState.Pressed);
        }

        /// <summary>
        /// Check if Left on the gamepad's directional pad is pressed.
        /// </summary>
        /// <returns></returns>
        public static bool isGamePadDPadLeftPressed()
        {
            return (mCurrentGamePadState.DPad.Left == ButtonState.Pressed);
        }

        /// <summary>
        /// Check if Right on the gamepad's directional pad is pressed.
        /// </summary>
        /// <returns></returns>
        public static bool isGamePadDPadRightPressed()
        {
            return (mCurrentGamePadState.DPad.Right == ButtonState.Pressed);
        }

        /// <summary>
        /// Check if the gamepad's left trigger is pressed.
        /// </summary>
        /// <returns></returns>
        public static bool isGamePadLeftTriggerPressed()
        {
            return (mCurrentGamePadState.Triggers.Left > mAnalogLimit);
        }

        /// <summary>
        /// Check if the gamepad's right trigger is pressed.
        /// </summary>
        /// <returns></returns>
        public static bool isGamePadRightTriggerPressed()
        {
            return (mCurrentGamePadState.Triggers.Right > mAnalogLimit);
        }

        /// <summary>
        /// Check if Up on the gamepad's left analog stick is pressed.
        /// </summary>
        /// <returns></returns>
        public static bool isGamePadLeftStickUpPressed()
        {
            return (mCurrentGamePadState.ThumbSticks.Left.Y > mAnalogLimit);
        }

        /// <summary>
        /// Check if Down on the gamepad's left analog stick is pressed.
        /// </summary>
        /// <returns></returns>
        public static bool isGamePadLeftStickDownPressed()
        {
            return ( -1f * mCurrentGamePadState.ThumbSticks.Left.Y > mAnalogLimit);
        }

        /// <summary>
        /// Check if Left on the gamepad's left analog stick is pressed.
        /// </summary>
        /// <returns></returns>
        public static bool isGamePadLeftStickLeftPressed()
        {
            return (-1f * mCurrentGamePadState.ThumbSticks.Left.X > mAnalogLimit);
        }

        /// <summary>
        /// Check if Right on the gamepad's left analog stick is pressed.
        /// </summary>
        /// <returns></returns>
        public static bool isGamePadLeftStickRightPressed()
        {
            return (mCurrentGamePadState.ThumbSticks.Left.X > mAnalogLimit);
        }

        /// <summary>
        /// Check if the GamePadKey value specified is pressed.
        /// </summary>
        /// <param name="aGamePadKey"></param>
        /// <returns></returns>
        private static bool isGamePadButtonPressed(GamePadButtons aGamePadKey)
        {
            switch (aGamePadKey)
            {
                case GamePadButtons.Start:
                    return isGamePadStartPressed();
                case GamePadButtons.Back:
                    return isGamePadBackPressed();
                case GamePadButtons.A:
                    return isGamePadAPressed();
                case GamePadButtons.B:
                    return isGamePadBPressed();
                case GamePadButtons.X:
                    return isGamePadXPressed();
                case GamePadButtons.Y:
                    return isGamePadYPressed();
                case GamePadButtons.LeftShoulder:
                    return isGamePadLeftShoulderPressed();
                case GamePadButtons.RightShoulder:
                    return isGamePadRightShoulderPressed();
                case GamePadButtons.LeftTrigger:
                    return isGamePadLeftTriggerPressed();
                case GamePadButtons.RightTrigger:
                    return isGamePadRightTriggerPressed();
                case GamePadButtons.Up:
                    return isGamePadDPadUpPressed() ||
                        isGamePadLeftStickUpPressed();
                case GamePadButtons.Down:
                    return isGamePadDPadDownPressed() ||
                        isGamePadLeftStickDownPressed();
                case GamePadButtons.Left:
                    return isGamePadDPadLeftPressed() || 
                        isGamePadLeftStickLeftPressed();
                case GamePadButtons.Right:
                    return isGamePadDPadRightPressed() ||
                        isGamePadLeftStickRightPressed();
            }
            return false;
        }

        #endregion //GamePadButton Pressed Queries

        #region GamePadButton Triggered Queries
        /// <summary>
        /// Check if the gamepad's Start button was just pressed.
        /// </summary>
        public static bool isGamePadStartTriggered()
        {
            return ((mCurrentGamePadState.Buttons.Start == ButtonState.Pressed) &&
              (mPreviousGamePadState.Buttons.Start == ButtonState.Released));
        }


        /// <summary>
        /// Check if the gamepad's Back button was just pressed.
        /// </summary>
        public static bool isGamePadBackTriggered()
        {
            return ((mCurrentGamePadState.Buttons.Back == ButtonState.Pressed) &&
              (mPreviousGamePadState.Buttons.Back == ButtonState.Released));
        }


        /// <summary>
        /// Check if the gamepad's A button was just pressed.
        /// </summary>
        public static bool isGamePadATriggered()
        {
            return ((mCurrentGamePadState.Buttons.A == ButtonState.Pressed) &&
              (mPreviousGamePadState.Buttons.A == ButtonState.Released));
        }


        /// <summary>
        /// Check if the gamepad's B button was just pressed.
        /// </summary>
        public static bool isGamePadBTriggered()
        {
            return ((mCurrentGamePadState.Buttons.B == ButtonState.Pressed) &&
              (mPreviousGamePadState.Buttons.B == ButtonState.Released));
        }


        /// <summary>
        /// Check if the gamepad's X button was just pressed.
        /// </summary>
        public static bool isGamePadXTriggered()
        {
            return ((mCurrentGamePadState.Buttons.X == ButtonState.Pressed) &&
              (mPreviousGamePadState.Buttons.X == ButtonState.Released));
        }


        /// <summary>
        /// Check if the gamepad's Y button was just pressed.
        /// </summary>
        public static bool isGamePadYTriggered()
        {
            return ((mCurrentGamePadState.Buttons.Y == ButtonState.Pressed) &&
              (mPreviousGamePadState.Buttons.Y == ButtonState.Released));
        }


        /// <summary>
        /// Check if the gamepad's LeftShoulder button was just pressed.
        /// </summary>
        public static bool isGamePadLeftShoulderTriggered()
        {
            return (
                (mCurrentGamePadState.Buttons.LeftShoulder == ButtonState.Pressed) &&
                (mPreviousGamePadState.Buttons.LeftShoulder == ButtonState.Released));
        }


        /// <summary>
        /// Check if the gamepad's RightShoulder button was just pressed.
        /// </summary>
        public static bool isGamePadRightShoulderTriggered()
        {
            return (
                (mCurrentGamePadState.Buttons.RightShoulder == ButtonState.Pressed) &&
                (mPreviousGamePadState.Buttons.RightShoulder == ButtonState.Released));
        }


        /// <summary>
        /// Check if Up on the gamepad's directional pad was just pressed.
        /// </summary>
        public static bool isGamePadDPadUpTriggered()
        {
            return ((mCurrentGamePadState.DPad.Up == ButtonState.Pressed) &&
              (mPreviousGamePadState.DPad.Up == ButtonState.Released));
        }


        /// <summary>
        /// Check if Down on the gamepad's directional pad was just pressed.
        /// </summary>
        public static bool isGamePadDPadDownTriggered()
        {
            return ((mCurrentGamePadState.DPad.Down == ButtonState.Pressed) &&
              (mPreviousGamePadState.DPad.Down == ButtonState.Released));
        }


        /// <summary>
        /// Check if Left on the gamepad's directional pad was just pressed.
        /// </summary>
        public static bool isGamePadDPadLeftTriggered()
        {
            return ((mCurrentGamePadState.DPad.Left == ButtonState.Pressed) &&
              (mPreviousGamePadState.DPad.Left == ButtonState.Released));
        }


        /// <summary>
        /// Check if Right on the gamepad's directional pad was just pressed.
        /// </summary>
        public static bool isGamePadDPadRightTriggered()
        {
            return ((mCurrentGamePadState.DPad.Right == ButtonState.Pressed) &&
              (mPreviousGamePadState.DPad.Right == ButtonState.Released));
        }


        /// <summary>
        /// Check if the gamepad's left trigger was just pressed.
        /// </summary>
        public static bool isGamePadLeftTriggerTriggered()
        {
            return ((mCurrentGamePadState.Triggers.Left > mAnalogLimit) &&
                (mPreviousGamePadState.Triggers.Left < mAnalogLimit));
        }


        /// <summary>
        /// Check if the gamepad's right trigger was just pressed.
        /// </summary>
        public static bool isGamePadRightTriggerTriggered()
        {
            return ((mCurrentGamePadState.Triggers.Right > mAnalogLimit) &&
                (mPreviousGamePadState.Triggers.Right < mAnalogLimit));
        }


        /// <summary>
        /// Check if Up on the gamepad's left analog stick was just pressed.
        /// </summary>
        public static bool isGamePadLeftStickUpTriggered()
        {
            return ((mCurrentGamePadState.ThumbSticks.Left.Y > mAnalogLimit) &&
                (mPreviousGamePadState.ThumbSticks.Left.Y < mAnalogLimit));
        }


        /// <summary>
        /// Check if Down on the gamepad's left analog stick was just pressed.
        /// </summary>
        public static bool isGamePadLeftStickDownTriggered()
        {
            return ((-1f * mCurrentGamePadState.ThumbSticks.Left.Y > mAnalogLimit) &&
                (-1f * mPreviousGamePadState.ThumbSticks.Left.Y < mAnalogLimit));
        }


        /// <summary>
        /// Check if Left on the gamepad's left analog stick was just pressed.
        /// </summary>
        public static bool isGamePadLeftStickLeftTriggered()
        {
            return ((-1f * mCurrentGamePadState.ThumbSticks.Left.X > mAnalogLimit) &&
                (-1f * mPreviousGamePadState.ThumbSticks.Left.X < mAnalogLimit));
        }


        /// <summary>
        /// Check if Right on the gamepad's left analog stick was just pressed.
        /// </summary>
        public static bool isGamePadLeftStickRightTriggered()
        {
            return ((mCurrentGamePadState.ThumbSticks.Left.X > mAnalogLimit) &&
                (mPreviousGamePadState.ThumbSticks.Left.X < mAnalogLimit));
        }


        /// <summary>
        /// Check if the GamePadKey value specified was pressed this frame.
        /// </summary>
        private static bool isGamePadButtonTriggered(GamePadButtons mGamePadKey)
        {
            switch (mGamePadKey)
            {
                case GamePadButtons.Start:
                    return isGamePadStartTriggered();

                case GamePadButtons.Back:
                    return isGamePadBackTriggered();

                case GamePadButtons.A:
                    return isGamePadATriggered();

                case GamePadButtons.B:
                    return isGamePadBTriggered();

                case GamePadButtons.X:
                    return isGamePadXTriggered();

                case GamePadButtons.Y:
                    return isGamePadYTriggered();

                case GamePadButtons.LeftShoulder:
                    return isGamePadLeftShoulderTriggered();

                case GamePadButtons.RightShoulder:
                    return isGamePadRightShoulderTriggered();

                case GamePadButtons.LeftTrigger:
                    return isGamePadLeftTriggerTriggered();

                case GamePadButtons.RightTrigger:
                    return isGamePadRightTriggerTriggered();

                case GamePadButtons.Up:
                    return isGamePadDPadUpTriggered() ||
                        isGamePadLeftStickUpTriggered();

                case GamePadButtons.Down:
                    return isGamePadDPadDownTriggered() ||
                        isGamePadLeftStickDownTriggered();

                case GamePadButtons.Left:
                    return isGamePadDPadLeftTriggered() ||
                        isGamePadLeftStickLeftTriggered();

                case GamePadButtons.Right:
                    return isGamePadDPadRightTriggered() ||
                        isGamePadLeftStickRightTriggered();
            }

            return false;
        }
        #endregion //GamePadButton Triggered Queries

        #endregion //GamePad Data

        #region Action Mapping

        /// <summary>
        /// The action mappings for the game.
        /// </summary>
        private static ActionMap[] mActionMaps;

        public static ActionMap[] ActionMaps
        {
            get { return mActionMaps; }
        }

        private static void resetActionMaps()
        {
            mActionMaps = new ActionMap[(int)Action.TotalActionCount];

            mActionMaps[(int)Action.MainMenu] = new ActionMap();
            mActionMaps[(int)Action.MainMenu].mKeyboardKeys.Add(
                Keys.Tab);
            mActionMaps[(int)Action.MainMenu].mGamePadButtons.Add(
                GamePadButtons.Start);

            mActionMaps[(int)Action.Ok] = new ActionMap();
            mActionMaps[(int)Action.Ok].mKeyboardKeys.Add(
                Keys.Enter);
            mActionMaps[(int)Action.Ok].mGamePadButtons.Add(
                GamePadButtons.A);

            mActionMaps[(int)Action.Back] = new ActionMap();
            mActionMaps[(int)Action.Back].mKeyboardKeys.Add(
                Keys.Escape);
            mActionMaps[(int)Action.Back].mGamePadButtons.Add(
                GamePadButtons.B);

            mActionMaps[(int)Action.CharacterManagement] = new ActionMap();
            mActionMaps[(int)Action.CharacterManagement].mKeyboardKeys.Add(
                Keys.Space);
            mActionMaps[(int)Action.CharacterManagement].mGamePadButtons.Add(
                GamePadButtons.Y);

            mActionMaps[(int)Action.ExitGame] = new ActionMap();
            mActionMaps[(int)Action.ExitGame].mKeyboardKeys.Add(
                Keys.Escape);
            mActionMaps[(int)Action.ExitGame].mGamePadButtons.Add(
                GamePadButtons.Back);

            mActionMaps[(int)Action.TakeView] = new ActionMap();
            mActionMaps[(int)Action.TakeView].mKeyboardKeys.Add(
                Keys.LeftControl);
            mActionMaps[(int)Action.TakeView].mGamePadButtons.Add(
                GamePadButtons.Y);

            mActionMaps[(int)Action.DropUnEquip] = new ActionMap();
            mActionMaps[(int)Action.DropUnEquip].mKeyboardKeys.Add(
                Keys.D);
            mActionMaps[(int)Action.DropUnEquip].mGamePadButtons.Add(
                GamePadButtons.X);

            mActionMaps[(int)Action.MoveCharacterUp] = new ActionMap();
            mActionMaps[(int)Action.MoveCharacterUp].mKeyboardKeys.Add(
                Keys.Up);
            mActionMaps[(int)Action.MoveCharacterUp].mGamePadButtons.Add(
                GamePadButtons.Up);

            mActionMaps[(int)Action.MoveCharacterDown] = new ActionMap();
            mActionMaps[(int)Action.MoveCharacterDown].mKeyboardKeys.Add(
                Keys.Down);
            mActionMaps[(int)Action.MoveCharacterDown].mGamePadButtons.Add(
                GamePadButtons.Down);

            mActionMaps[(int)Action.MoveCharacterLeft] = new ActionMap();
            mActionMaps[(int)Action.MoveCharacterLeft].mKeyboardKeys.Add(
                Keys.Left);
            mActionMaps[(int)Action.MoveCharacterLeft].mGamePadButtons.Add(
                GamePadButtons.Left);

            mActionMaps[(int)Action.MoveCharacterRight] = new ActionMap();
            mActionMaps[(int)Action.MoveCharacterRight].mKeyboardKeys.Add(
                Keys.Right);
            mActionMaps[(int)Action.MoveCharacterRight].mGamePadButtons.Add(
                GamePadButtons.Right);

            mActionMaps[(int)Action.CursorUp] = new ActionMap();
            mActionMaps[(int)Action.CursorUp].mKeyboardKeys.Add(
                Keys.Up);
            mActionMaps[(int)Action.CursorUp].mGamePadButtons.Add(
                GamePadButtons.Up);

            mActionMaps[(int)Action.CursorDown] = new ActionMap();
            mActionMaps[(int)Action.CursorDown].mKeyboardKeys.Add(
                Keys.Down);
            mActionMaps[(int)Action.CursorDown].mGamePadButtons.Add(
                GamePadButtons.Down);

            mActionMaps[(int)Action.DecreaseAmount] = new ActionMap();
            mActionMaps[(int)Action.DecreaseAmount].mKeyboardKeys.Add(
                Keys.Left);
            mActionMaps[(int)Action.DecreaseAmount].mGamePadButtons.Add(
                GamePadButtons.Left);

            mActionMaps[(int)Action.IncreaseAmount] = new ActionMap();
            mActionMaps[(int)Action.IncreaseAmount].mKeyboardKeys.Add(
                Keys.Right);
            mActionMaps[(int)Action.IncreaseAmount].mGamePadButtons.Add(
                GamePadButtons.Right);

            mActionMaps[(int)Action.PageLeft] = new ActionMap();
            mActionMaps[(int)Action.PageLeft].mKeyboardKeys.Add(
                Keys.LeftShift);
            mActionMaps[(int)Action.PageLeft].mGamePadButtons.Add(
                GamePadButtons.LeftTrigger);

            mActionMaps[(int)Action.PageRight] = new ActionMap();
            mActionMaps[(int)Action.PageRight].mKeyboardKeys.Add(
                Keys.RightShift);
            mActionMaps[(int)Action.PageRight].mGamePadButtons.Add(
                GamePadButtons.RightTrigger);

            mActionMaps[(int)Action.TargetUp] = new ActionMap();
            mActionMaps[(int)Action.TargetUp].mKeyboardKeys.Add(
                Keys.Up);
            mActionMaps[(int)Action.TargetUp].mGamePadButtons.Add(
                GamePadButtons.Up);

            mActionMaps[(int)Action.TargetDown] = new ActionMap();
            mActionMaps[(int)Action.TargetDown].mKeyboardKeys.Add(
                Keys.Down);
            mActionMaps[(int)Action.TargetDown].mGamePadButtons.Add(
                GamePadButtons.Down);

            mActionMaps[(int)Action.ActiveCharacterLeft] = new ActionMap();
            mActionMaps[(int)Action.ActiveCharacterLeft].mKeyboardKeys.Add(
                Keys.Left);
            mActionMaps[(int)Action.ActiveCharacterLeft].mGamePadButtons.Add(
                GamePadButtons.Left);

            mActionMaps[(int)Action.ActiveCharacterRight] = new ActionMap();
            mActionMaps[(int)Action.ActiveCharacterRight].mKeyboardKeys.Add(
                Keys.Right);
            mActionMaps[(int)Action.ActiveCharacterRight].mGamePadButtons.Add(
                GamePadButtons.Right);

        }

        /// <summary>
        /// Check if an action has been pressed.
        /// </summary>
        /// <param name="aAction"></param>
        /// <returns></returns>
        public static bool isActionPressed(Action aAction)
        {
            return isActionMapPressed(mActionMaps[(int)aAction]);
        }

        /// <summary>
        /// Check if an action was just performed in the most recent update.
        /// </summary>
        /// <param name="aAction"></param>
        /// <returns></returns>
        public static bool isActionTriggered(Action aAction)
        {
            return isActionMapTriggered(mActionMaps[(int)aAction]);
        }

        /// <summary>
        /// Check if an action map has been pressed.
        /// </summary>
        /// <param name="aActionMap"></param>
        /// <returns></returns>
        private static bool isActionMapPressed(ActionMap aActionMap)
        {
            for (int i = 0; i < aActionMap.mKeyboardKeys.Count; ++i)
            {
                if (isKeyPressed(aActionMap.mKeyboardKeys[i]))
                {
                    return true;
                }
            }
            if (mCurrentGamePadState.IsConnected)
            {
                for (int i = 0; i < aActionMap.mGamePadButtons.Count; ++i)
                {
                    if (isGamePadButtonPressed(aActionMap.mGamePadButtons[i]))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Check if an action map has been triggered this frame
        /// </summary>
        /// <param name="aActionMap"></param>
        /// <returns></returns>
        private static bool isActionMapTriggered(ActionMap aActionMap)
        {
            for (int i = 0; i < aActionMap.mKeyboardKeys.Count; ++i)
            {
                if (isKeyTriggered(aActionMap.mKeyboardKeys[i]))
                {
                    return true;
                }
            }
            if (mCurrentGamePadState.IsConnected)
            {
                for (int i = 0; i < aActionMap.mGamePadButtons.Count; ++i)
                {
                    if (isGamePadButtonTriggered(aActionMap.mGamePadButtons[i]))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        #endregion //Action Mapping

        #region Initialization

        /// <summary>
        /// Initializes the default control keys for all actions.
        /// </summary>
        public static void initialize()
        {
            resetActionMaps();
        }

        #endregion //Initialization

        #region Updating

        public static void Update()
        {
            //update the keyboard state
            mPreviousKeyboardState = mCurrentKeyboardState;
            mCurrentKeyboardState = Keyboard.GetState();

            //update the gamepad state
            mPreviousGamePadState = mCurrentGamePadState;
            mCurrentGamePadState = GamePad.GetState(PlayerIndex.One);
        }
        #endregion //Updating
    }
}
