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

        #region Map

        private string mMapContentName;
        /// <summary>
        /// The content name of the map for the new game.
        /// </summary>
        public string MapContentName
        {
            get { return mMapContentName; }
            set { mMapContentName = value; }
        }

        #endregion //Map

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

                aDesc.MapContentName = input.ReadString();

                return aDesc;
            }
        }

        #endregion //Content Type Reader
    }
}
