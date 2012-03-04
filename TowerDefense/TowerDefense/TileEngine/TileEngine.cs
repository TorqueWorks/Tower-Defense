#region File Description
#endregion

#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TowerDefenseData;
#endregion

namespace TowerDefense
{
    /// <summary>
    /// Static class for a tileable level
    /// </summary>
    static class TileEngine
    {
        #region Level

        private static Level mLevel = null;

        /// <summary>
        /// The level being used by the tile engine
        /// </summary>
        public static Level Level
        {
            get { return mLevel; }
        }

        /// <summary>
        /// The position of the outside 0,0 corner of the map, in pixels
        /// </summary>
        private static Vector2 mLevelOriginalPos;

        /// <summary>
        /// Calculate the screen position of a given level location (in tiles)
        /// </summary>
        /// <param name="aLevelPos">A level location, in tiles</param>
        /// <returns>The current screen position of that location</returns>
        public static Vector2 GetScreenPosition(Point aLevelPos)
        {
            return new Vector2(
                mLevelOriginalPos.X + aLevelPos.X * mLevel.TileSize.X,
                mLevelOriginalPos.Y + aLevelPos.Y * mLevel.TileSize.Y);
        }

        /// <summary>
        /// Set the level in use by the tile engine
        /// </summary>
        /// <param name="aNewLevel">The new level to use for the tile engine</param>
        public static void SetLevel(Level aNewLevel)
        {
            if (aNewLevel == null)
            {
                throw new ArgumentNullException("NewLevel");
            }

            mLevel = aNewLevel;

            //Reset the map origin, which will be recalculated on the first update
            mLevelOriginalPos = Vector2.Zero;


        }

        #endregion //Level

        #region Graphics Data

        /// <summary>
        /// The viewport that the tile engine is rendering within.
        /// </summary>
        private static Viewport mViewport;

        public static Viewport Viewport
        {
            get { return mViewport; }
            set
            {
                mViewport = value;
                mViewportCenter = new Vector2(
                    mViewport.X + mViewport.Width / 2f,
                    mViewport.Y + mViewport.Height / 2f);
            }
        }

        /// <summary>
        /// The center of the current viewport
        /// </summary>
        private static Vector2 mViewportCenter;

        #endregion //Graphics Data

        #region Updating

        /// <summary>
        /// Update the tile engine
        /// </summary>
        /// <param name="aGameTime">Snapshot of timing values</param>
        public static void Update(GameTime aGameTime)
        {
            //TODO: Do any updates here for moving around the level
        }

        #endregion //Updating

        #region Drawing

        public static void DrawLayers(SpriteBatch aSpriteBatch, bool aDrawBase, bool aDrawObject)
        {
            if (aSpriteBatch == null)
            {
                throw new ArgumentNullException("SpriteBatch");
            }
            //Not drawing anything...so let's get out early!
            if (!aDrawBase && !aDrawObject)
            {
                return;
            }

            Rectangle lDestRectangle = new Rectangle(0, 0, mLevel.TileSize.X, mLevel.TileSize.Y);

            //Loop through all the tiles for this level, drawing any which are visible
            for (int y = 0; y < mLevel.Dimensions.Y; ++y)
            {
                for (int x = 0; x < mLevel.Dimensions.X; ++x)
                {
                    lDestRectangle.X = (int)mLevelOriginalPos.X + x * mLevel.TileSize.X;
                    lDestRectangle.Y = (int)mLevelOriginalPos.Y + y * mLevel.TileSize.Y;

                    //If the tile is inside the screen...
                    if (CheckVisibility(lDestRectangle))
                    {
                        Point lMapPos = new Point(x, y);
                        if (aDrawBase)
                        {
                            Rectangle lSourceRect = mLevel.GetBaseLayerSourceRectangle(lMapPos);
                            if (lSourceRect != Rectangle.Empty)
                            {
                                aSpriteBatch.Draw(mLevel.Texture, lDestRectangle, lSourceRect, Color.White);
                            }
                        }
                        if (aDrawObject)
                        {
                            Rectangle lSourceRect = mLevel.GetObjectLayerSourceRectangle(lMapPos);
                            if (lSourceRect != Rectangle.Empty)
                            {
                                aSpriteBatch.Draw(mLevel.Texture, lDestRectangle, lSourceRect, Color.White);
                            }
                        }
                    }
                }
            }
        }

        public static bool CheckVisibility(Rectangle aScreenRect)
        {
            return ((aScreenRect.X > mViewport.X - aScreenRect.Width) &&
                (aScreenRect.Y > mViewport.Y - aScreenRect.Height) &&
                (aScreenRect.X < mViewport.X + mViewport.Width) &&
                (aScreenRect.Y < mViewport.Y + mViewport.Height));
        }
        #endregion //Drawing
    }
}
