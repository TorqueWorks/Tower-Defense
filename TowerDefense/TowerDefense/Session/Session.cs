#region File Description

#endregion

#region Using Statements
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;
using TowerDefenseData;
#endregion

namespace TowerDefense
{
    class Session
    {

        #region Singleton

        /// <summary>
        /// The single Session instance that can be active at a time.
        /// </summary>
        private static Session mSingleton;

        #endregion //Singleton

        #region Level

        /// <summary>
        /// Change the current level.
        /// </summary>
        /// <param name="aContentName">The asset name of the new level.</param>
        public static void ChangeLevel(string aContentName)
        {
            string lLevelContentName = aContentName;
            if (!lLevelContentName.StartsWith(@"Levels\"))
            { //All levels should be in the Levels content location
                lLevelContentName = Path.Combine(@"Levels", lLevelContentName);
            }

            //Load the level
            ContentManager lContent = mSingleton.mScreenManager.Game.Content;
            Level lLevel = lContent.Load<Level>(lLevelContentName).Clone() as Level;

            TileEngine.SetLevel(lLevel);
        }

        #endregion //Level

        #region User Interface Data

        /// <summary>
        /// The ScreenManager used to manage all UI's in the game.
        /// </summary>
        private ScreenManager mScreenManager;

        /// <summary>
        /// The ScreenManager used to manage all UI's in the game.
        /// </summary>
        public static ScreenManager ScreenManager
        {
            get { return (mSingleton == null ? null : mSingleton.mScreenManager); }
        }

        /// <summary>
        /// The GameplayScreen object which was created for this session
        /// </summary>
        private GameplayScreen mGameplayScreen;

        /// <summary>
        /// The heads-up-display menu shown on the map.
        /// </summary>
        private Hud mHud;

        /// <summary>
        /// The heads-up-display menu shown on the map
        /// </summary>
        public static Hud Hud
        {
            get { return (mSingleton == null ? null : mSingleton.mHud); }
        }

        #endregion //User Interface Data

        #region State Data

        /// <summary>
        /// Returns true if there is an active session.
        /// </summary>
        public static bool IsActive
        {
            get { return mSingleton != null; }
        }

        #endregion //State Data

        #region Initialization

        /// <summary>
        /// Private constructor for Session
        /// </summary>
        /// <param name="aScreenManager"></param>
        /// <param name="aGameplayScreen"></param>
        /// <remarks>
        /// The lack of a public constructor forces the singleton model.
        /// </remarks>
        private Session(ScreenManager aScreenManager, GameplayScreen aGameplayScreen)
        {
            if (aScreenManager == null)
            {
                throw new ArgumentNullException("screenManager");
            }
            if (aGameplayScreen == null)
            {
                throw new ArgumentNullException("gameplayScreen");
            }

            mScreenManager = aScreenManager;
            mGameplayScreen = aGameplayScreen;
            mHud = new Hud(mScreenManager);
            mHud.LoadContent();
        }

        #endregion //Initialization

        #region Updating

        /// <summary>
        /// Update the session for this frame.
        /// </summary>
        /// <param name="aGameTime"></param>
        /// <remarks>
        /// This should only be called if there are no menus in use.
        /// </remarks>
        public static void Update(GameTime aGameTime)
        {
            if (mSingleton == null)
            {
                return;
            }
        }

        #endregion //Updating

        #region Drawing

        /// <summary>
        /// Draws the session environment to the screen.
        /// </summary>
        /// <param name="aGameTime"></param>
        public static void Draw(GameTime aGameTime)
        {
            //TODO: Draw screen
            mSingleton.DrawScreen(aGameTime);

            mSingleton.mHud.Draw();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aGameTime"></param>
        private void DrawScreen(GameTime aGameTime)
        {
            SpriteBatch lSpriteBatch = mScreenManager.SpriteBatch;

            lSpriteBatch.Begin();
            //Draw the background/ground tiles
            if (TileEngine.Level.Texture != null)
            {
                TileEngine.DrawLayers(lSpriteBatch, true, false);
            }
            //Draw the foreground objects
            if (TileEngine.Level.Texture != null)
            {
                TileEngine.DrawLayers(lSpriteBatch, false, true);
            }
            lSpriteBatch.End();
        }

        #endregion //Drawing

        #region Starting a New Session

        public static void StartNewSession(GameStartDescription aGameStartDescription,
            ScreenManager aScreenManager, GameplayScreen aGameplayScreen)
        {
            if (aGameStartDescription == null)
            {
                throw new ArgumentNullException("gameStartDescription");
            }
            if (aScreenManager == null)
            {
                throw new ArgumentNullException("screenManager");
            }
            if (aGameplayScreen == null)
            {
                throw new ArgumentNullException("gameplayScreen");
            }

            //End any existing session
            EndSession();

            //Create new singleton
            mSingleton = new Session(aScreenManager, aGameplayScreen);

            //Set up initial level
            ChangeLevel(aGameStartDescription.LevelContentName);

        }

        #endregion //Starting a New Session

        #region Ending a Session

        public static void EndSession()
        {
            //exit the gameplay screen
            if (mSingleton != null)
            {
                GameplayScreen lGameplayScreen = mSingleton.mGameplayScreen;
                mSingleton.mGameplayScreen = null;

                //TODO: Add music
                //pop the music
                //AudioManager.PopMusic();

                mSingleton = null;

                if (lGameplayScreen != null)
                {
                    lGameplayScreen.ExitScreen();
                }
            }
        }

        #endregion //Ending a session

        #region Loading a Session

        /// <summary>
        /// Start a new session, using the data in the given save game
        /// </summary>
        /// <param name="aSaveGameDescription">The description of the save game</param>
        /// <param name="aScreenManager">The screen manager for the new session</param>
        /// <param name="aGameplayScreen"></param>
        public static void LoadSession(SaveGameDescription aSaveGameDescription,
            ScreenManager aScreenManager, GameplayScreen aGameplayScreen)
        {
            if (aSaveGameDescription == null)
            {
                throw new ArgumentNullException("saveGameDescription");
            }
            if (aScreenManager == null)
            {
                throw new ArgumentNullException("screenManager");
            }
            if (aGameplayScreen == null)
            {
                throw new ArgumentNullException("gameplayScreen");
            }

            //Exit any existing session
            EndSession();

            //Create the new session
            mSingleton = new Session(aScreenManager, aGameplayScreen);

            //Get the storage device and load the session
            GetStorageDevice(
                delegate(StorageDevice aStorageDevice)
                {
                    LoadSessionResult(aStorageDevice, aSaveGameDescription);
                });
        }

        /// <summary>
        /// Receives the storage device and starts a new session, using
        /// the data in the given save game.
        /// </summary>
        /// <remarks>
        /// The new Session is created in LoadSessionResult
        /// </remarks>
        /// <param name="aStorageDevice">The chosen storage device.</param>
        /// <param name="aSaveGameDescription">The description of the save game.</param>
        public static void LoadSessionResult(StorageDevice aStorageDevice, SaveGameDescription aSaveGameDescription)
        {
            if (aSaveGameDescription == null)
            {
                throw new ArgumentNullException("saveGameDescription");
            }
            if (aStorageDevice == null || !aStorageDevice.IsConnected)
            {
                return;
            }

            //TODO: Load saved game
        }

        #endregion //Loading a Session

        #region Saving a Session

        /// <summary>
        /// Save the current state of the session.
        /// </summary>
        /// <param name="aOverwriteDescription">The description of the save game to over-write, if any.</param>
        public static void SaveSession(SaveGameDescription aOverwriteDescription)
        {
            //Retrieve the storage device, asynchronously
            GetStorageDevice(delegate(StorageDevice aStorageDevice)
            {
                SaveSessionResult(aStorageDevice, aOverwriteDescription);
            });

        }

        /// <summary>
        /// Save the current state of the session, with the given storage device.
        /// </summary>
        /// <param name="aStorageDevice">The chosen storage device.</param>
        /// <param name="aOverwriteDescription">The description of the game to over-write, if any</param>
        private static void SaveSessionResult(StorageDevice aStorageDevice, SaveGameDescription aOverwriteDescription)
        {
            if ((aStorageDevice == null) || !aStorageDevice.IsConnected)
            {
                return;
            }


            //TODO: Save session data here
        }
        #endregion //Saving a Session

        #region Deleting a Save Game

        /// <summary>
        /// Delete the saved game specified by the description
        /// </summary>
        /// <param name="aSaveGameDescription"></param>
        public static void DeleteSaveGame(SaveGameDescription aSaveGameDescription)
        {
            if (aSaveGameDescription == null)
            {
                throw new ArgumentNullException("saveGameDescription");
            }

            //Get the storage device to delete the saved game
            GetStorageDevice(
                delegate(StorageDevice aStorageDevice)
                {
                    DeleteSaveGameResult(aStorageDevice, aSaveGameDescription);
                });
        }

        /// <summary>
        /// Delete the saved game specified by the description.
        /// </summary>
        /// <param name="aStorageDevice">The chosen storage device</param>
        /// <param name="aSaveGameDescription">The description of the saved game.</param>
        public static void DeleteSaveGameResult(StorageDevice aStorageDevice,
            SaveGameDescription aSaveGameDescription)
        {
            if (aSaveGameDescription == null)
            {
                throw new ArgumentNullException("saveGameDescription");
            }
            if ((aStorageDevice == null) || !aStorageDevice.IsConnected)
            {
                return;
            }

            //TODO: Delete saved game file here
        }

        #endregion //Deleting a Save Game

        #region Save Game Descriptions

        /// <summary>
        /// Save game descriptions for the current set of saved games.
        /// </summary>
        private static List<SaveGameDescription> mSaveGameDescriptions = null;

        /// <summary>
        /// Saved game descriptions for the current set of saved games.
        /// </summary>
        public static List<SaveGameDescription> SaveGameDescriptions
        {
            get { return mSaveGameDescriptions; }
        }

        /// <summary>
        /// The maximum number of saved-game descriptions that the list may hold
        /// </summary>
        public const int MAX_SAVED_GAMES = 5;

        /// <summary>
        /// XML serializer for SaveGameDescription objects.
        /// </summary>
        private static XmlSerializer mSaveGameDescriptionSerializer =
            new XmlSerializer(typeof(SaveGameDescription));

        /// <summary>
        /// Refreshes the list of saved-game descriptions
        /// </summary>
        public static void RefreshSaveGameDescriptions()
        {
            mSaveGameDescriptions = null;

            //Retrieve the storage device, asynchronously
            GetStorageDevice(RefreshSaveGameDescriptionsResult);
        }

        /// <summary>
        /// Asynchronous storage-device callback for refreshing the
        /// save-game description.
        /// </summary>
        /// <param name="aStorageDevice"></param>
        private static void RefreshSaveGameDescriptionsResult(StorageDevice aStorageDevice)
        {
            if ((aStorageDevice == null) || !aStorageDevice.IsConnected)
            {
                return;
            }

            //TODO: Get the list of saved games
        }

        #endregion //Save Game Descriptions

        #region Storage

        /// <summary>
        /// The stored StorageDevice object.
        /// </summary>
        private static StorageDevice mStorageDevice;

        /// <summary>
        /// The container name used for saved games.
        /// </summary>
        public static string SaveGameContainerName = "TowerDefense";

        /// <summary>
        /// A delegate for receiving StorageDeviceObjects.
        /// </summary>
        /// <param name="aStorageDevice"></param>
        public delegate void StorageDeviceDelegate(StorageDevice aStorageDevice);

        public static void GetStorageDevice(StorageDeviceDelegate aRetrievalDelegate)
        {
            if (aRetrievalDelegate == null)
            {
                throw new ArgumentNullException("retrievalDelegate");
            }

            //Check the storage storage device
            if ((mStorageDevice != null) && mStorageDevice.IsConnected)
            {
                aRetrievalDelegate(mStorageDevice);
                return;
            }

            //The storage device must be retrieved
            if (!Guide.IsVisible)
            {
                //Reset the device
                mStorageDevice = null;
                StorageDevice.BeginShowSelector(GetStorageDeviceResult, aRetrievalDelegate);
            }
        }

        /// <summary>
        /// Asynchronous callback to the guide's BeginShowStorageDeviceSelector call.
        /// </summary>
        /// <param name="aResult"></param>
        private static void GetStorageDeviceResult(IAsyncResult aResult)
        {
            if ((aResult == null) || !aResult.IsCompleted)
            {
                return;
            }

            //Retrieve and store the storage device
            mStorageDevice = StorageDevice.EndShowSelector(aResult);

            //Check the new storage device
            if ((mStorageDevice != null) && mStorageDevice.IsConnected)
            {
                //it passes; call the stored delegate
                StorageDeviceDelegate lFunc = aResult.AsyncState as StorageDeviceDelegate;
                if (lFunc != null)
                {
                    lFunc(mStorageDevice);
                }
            }
        }

        /// <summary>
        /// Synchronously opens storage container
        /// </summary>
        /// <param name="aStorageDevice"></param>
        /// <returns></returns>
        private static StorageContainer OpenContainer(StorageDevice aStorageDevice)
        {
            IAsyncResult aResult = aStorageDevice.BeginOpenContainer(Session.SaveGameContainerName, null, null);

            //Wait for the WaitHandle to become signaled
            aResult.AsyncWaitHandle.WaitOne();

            StorageContainer lContainer = aStorageDevice.EndOpenContainer(aResult);

            //close the wait handle
            aResult.AsyncWaitHandle.Close();

            return lContainer;
        }

        #endregion //Storage

        #region Random

        /// <summary>
        /// The random-number generator used with game events.
        /// </summary>
        private static Random mRandom = new Random();

        /// <summary>
        /// The random-number generator used with game events.
        /// </summary>
        public static Random Random
        {
            get { return mRandom; }
        }

        #endregion //Random
    }
}
