#region File Description

#endregion

#region Using Statements
using System;
using System.Xml.Serialization;
#endregion

namespace TowerDefense
{
    /// <summary>
    /// The description of a save game file.
    /// </summary>
    /// <remarks>
    /// This data is saved in a separate file, and loaded by
    /// the Load and Save Game menu screens.
    /// </remarks>
    public class SaveGameDescription
    {

        #region Fields
        private string mFilename;
        /// <summary>
        /// The name of the save file with the game data.
        /// </summary>
        public string Filename
        {
            get { return mFilename; }
            set { mFilename = value; }
        }

        private string mChapterName;
        /// <summary>
        /// The short description of how far the player has progressed in the game.
        /// </summary>
        public string ChapterName
        {
            get { return mChapterName; }
            set { mChapterName = value; }
        }

        private string mDescription;
        /// <summary>
        /// The short description of how far the player has progressed in the game.
        /// </summary>
        /// <remarks>Here, it's the time played</remarks>
        public string Description
        {
            get { return mDescription; }
            set { mDescription = value; }
        }
        #endregion //Fields
    }
}
