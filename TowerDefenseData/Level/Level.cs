#region File Description
#endregion

#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
#endregion

namespace TowerDefenseData
{
    /// <summary>
    /// A level which the game is played on
    /// </summary>
    public class Level : ContentObject
#if WINDOWS
, ICloneable
#endif
    {
        #region Fields


        private string mName;

        /// <summary>
        /// Name of the map
        /// </summary>
        public string Name
        {
            get { return mName; }
            set { mName = value; }
        }

        private Point mDimensions;

        /// <summary>
        /// The dimensions of the level, in tiles
        /// </summary>
        public Point Dimensions
        {
            get { return mDimensions; }
            set { mDimensions = value; }
        }

        private Point mTileSize;

        /// <summary>
        /// The size of the tiles for this map, in pixels
        /// </summary>
        public Point TileSize
        {
            get { return mTileSize; }
            set { mTileSize = value; }
        }

        private int mTilesPerRow;

        /// <summary>
        /// The number of tiles in a row of the map texture
        /// </summary>
        /// <remarks>Used to determine the source rectangle from the map layer value</remarks>
        public int TilesPerRow
        {
            get { return mTilesPerRow; }
        }
        #endregion //Fields

        #region Graphics Data

        private string mTextureName;

        /// <summary>
        /// The content name of the texture that contains the tiles for this map
        /// </summary>
        public string TextureName
        {
            get { return mTextureName; }
            set { mTextureName = value; }
        }

        private Texture2D mTexture;

        /// <summary>
        /// The texture that contains the tiles for the map
        /// </summary>
        [ContentSerializerIgnore]
        public Texture2D Texture
        {
            get { return mTexture; }
            set { mTexture = value; }
        }


        #endregion //Graphics Data

        #region Level Layers

        #region Base Layer

        private int[] mBaseLayer;
        
        /// <summary>
        /// Spatial array for the ground tiles for this level
        /// </summary>
        public int[] BaseLayer
        {
            get { return mBaseLayer; }
            set { mBaseLayer = value; }
        }

        /// <summary>
        /// Retrieves the base layer value for the given map position
        /// </summary>
        /// <param name="aMapPos">The coordinates of the tile we want</param>
        /// <returns></returns>
        public int GetBaseLayerValue(Point aMapPos)
        {
            if ((aMapPos.X < 0) || (aMapPos.Y >= mDimensions.X) ||
                (aMapPos.Y < 0) || (aMapPos.Y >= mDimensions.Y))
            {
                throw new ArgumentOutOfRangeException("MapPos");
            }

            return mBaseLayer[aMapPos.Y * mDimensions.X + aMapPos.X];
        }

        /// <summary>
        /// Retrieves the source rectangle for the tile in the given position in the
        /// base layer.
        /// </summary>
        /// <param name="aMapPos">The coordinates of the tile</param>
        /// <returns>The source rectangle for the tile</returns>
        /// <remarks>This method allows out-of-bound (blocked) positions</remarks>
        public Rectangle GetBaseLayerSourceRectangle(Point aMapPos)
        {
            //Bounds check, but out-of-bounds if non-fatal
            if ((aMapPos.X < 0) || (aMapPos.Y >= mDimensions.X) ||
                 (aMapPos.Y < 0) || (aMapPos.Y >= mDimensions.Y))
            {
                return Rectangle.Empty;
            }

            int lBaseLayerValue = GetBaseLayerValue(aMapPos);
            if (lBaseLayerValue < 0)
            {
                return Rectangle.Empty;
            }

            return new Rectangle(
                (lBaseLayerValue % mTilesPerRow) * mTileSize.X,
                (lBaseLayerValue / mTilesPerRow) * mTileSize.Y,
                mTileSize.X,
                mTileSize.Y);
        }
        #endregion //Base Layer

        #region Object Layer

        private int[] mObjectLayer;

        /// <summary>
        /// Spatial array for the object images on this level
        /// </summary>
        public int[] ObjectLayer
        {
            get { return mObjectLayer; }
            set { mObjectLayer = value; }
        }

        /// <summary>
        /// Retrieves the object layer value for the given map position
        /// </summary>
        /// <param name="aMapPos">The coordinates of the tile to retrieve the value of</param>
        /// <returns>The object layer value for the tile at the specified coordinates</returns>
        public int GetObjectLayerValue(Point aMapPos)
        {
            if ((aMapPos.X < 0) || (aMapPos.X >= mDimensions.X)
                || (aMapPos.Y < 0) || (aMapPos.Y >= mDimensions.Y))
            {
                throw new ArgumentOutOfRangeException("MapPos");
            }

            return mObjectLayer[aMapPos.Y * mDimensions.X + aMapPos.X];
        }

        /// <summary>
        /// Retrieves the source rectangle for the tile in the given position in the
        /// object layer.
        /// </summary>
        /// <param name="aMapPos">The coordinates of the tile</param>
        /// <returns>The source rectangle for the tile</returns>
        /// <remarks>This method allows out-of-bound (blocked) positions</remarks>
        public Rectangle GetObjectLayerSourceRectangle(Point aMapPos)
        {
            //Bounds check, but out-of-bounds if non-fatal
            if ((aMapPos.X < 0) || (aMapPos.Y >= mDimensions.X) ||
                 (aMapPos.Y < 0) || (aMapPos.Y >= mDimensions.Y))
            {
                return Rectangle.Empty;
            }

            int lObjectLayerValue = GetObjectLayerValue(aMapPos);
            if (lObjectLayerValue < 0)
            {
                return Rectangle.Empty;
            }

            return new Rectangle(
                (lObjectLayerValue % mTilesPerRow) * mTileSize.X,
                (lObjectLayerValue / mTilesPerRow) * mTileSize.Y,
                mTileSize.X,
                mTileSize.Y);
        }


        #endregion //Object Layer

        #endregion //Level Layers

        #region ICloneable Members

        /// <summary>
        /// Creates a deep copy of the Level object
        /// </summary>
        /// <returns></returns>
        public Object Clone()
        {
         
            Level lLevel = new Level();

            lLevel.AssetName = AssetName;
            lLevel.mBaseLayer = BaseLayer.Clone() as int[];
            lLevel.mObjectLayer = ObjectLayer.Clone() as int[];
            lLevel.mDimensions = Dimensions;
            lLevel.mName = Name;
            lLevel.mTexture = Texture;
            lLevel.mTextureName = TextureName;
            lLevel.mTileSize = TileSize;
            lLevel.mTilesPerRow = mTilesPerRow;

            return lLevel;

        }
        #endregion //ICloneable Members

        #region Content Type Reader

        /// <summary>
        /// Read a Level object from the content pipeline
        /// </summary>
        public class LevelReader : ContentTypeReader<Level>
        {
            protected override Level Read(ContentReader input, Level existingInstance)
            {
                Level lLevel = existingInstance;
                if (lLevel == null)
                {
                    lLevel = new Level();
                }

                lLevel.AssetName = input.AssetName;

                lLevel.Name = input.ReadString();
                lLevel.Dimensions = input.ReadObject<Point>();
                lLevel.TileSize = input.ReadObject<Point>();
                lLevel.TextureName = input.ReadString();
                lLevel.Texture = input.ContentManager.Load<Texture2D>(
                    System.IO.Path.Combine(@"Textures\Levels", lLevel.TextureName));
                lLevel.mTilesPerRow = lLevel.Texture.Width / lLevel.TileSize.X;
                lLevel.BaseLayer = input.ReadObject<int[]>();
                lLevel.ObjectLayer = input.ReadObject<int[]>();

                return lLevel;
            }
        }
        #endregion //Content Type Reader
    }
}
