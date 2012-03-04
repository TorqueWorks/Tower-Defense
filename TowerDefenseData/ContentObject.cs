#region File Description
#endregion

#region Using Statements
using System;
using Microsoft.Xna.Framework.Content;
#endregion

namespace TowerDefenseData
{
    /// <summary>
    /// Base type for all of the data types that load via the content pipeline
    /// </summary>
    public abstract class ContentObject
    {
        /// <summary>
        /// The name of the content pipeline asset that contained the object
        /// </summary>
        private string mAssetName;

        [ContentSerializerIgnore]
        public string AssetName
        {
            get { return mAssetName; }
            set { mAssetName = value; }
        }
    }
}
