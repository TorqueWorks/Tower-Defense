#region File Description
#endregion

#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
#endregion

namespace TowerDefenseData
{
    public class GameStartDescription
    {

        #region Level

        private string mLevelContentName;
        /// <summary>
        /// The content name of the level for the new game.
        /// </summary>
        public string LevelContentName
        {
            get { return mLevelContentName; }
            set { mLevelContentName = value; }
        }

        #endregion //Level

        #region Content Type Reader

        /// <summary>
        /// Read a GameStartDescription object from the content pipeline
        /// </summary>
        public class GameStartDescriptionReader : ContentTypeReader<GameStartDescription>
        {
            protected override GameStartDescription Read(ContentReader input, GameStartDescription existingInstance)
            {
                GameStartDescription aDesc = existingInstance;
                if (aDesc == null)
                {
                    aDesc = new GameStartDescription();
                }

                aDesc.LevelContentName = input.ReadString();

                return aDesc;
            }
        }

        #endregion //Content Type Reader
    }
}
