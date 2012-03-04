#region File Description
#endregion

#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;
using TowerDefenseData;

#endregion

namespace TowerDefenseProcessors
{
    /// <summary>
    /// This class will be instantiated by the XNA Framework Content Pipeline to
    /// write the specified data type into binary .xnb format.
    /// 
    /// This should be part of a Content Pipeline Extension Library project
    /// </summary>
    [ContentTypeWriter]
    public class LevelWriter : TowerDefenseGameWriter<Level>
    {
        protected override void Write(ContentWriter output, Level value)
        {
            //Validate the level first
            if ((value.Dimensions.X <= 0) ||
                (value.Dimensions.Y <= 0))
            {
                throw new InvalidContentException("Invalid Map Dimensions");
            }

            int lTotalTiles = value.Dimensions.X * value.Dimensions.Y;
            if (value.BaseLayer.Length != lTotalTiles)
            {
                throw new InvalidContentException("Base Layer was " +
                    value.BaseLayer.Length.ToString() +
                    " tiles, but the dimensions specify " +
                    lTotalTiles.ToString() + ".");
            }
            if (value.ObjectLayer.Length != lTotalTiles)
            {
                throw new InvalidContentException("Object Layer was " +
                    value.ObjectLayer.Length.ToString() +
                    " tiles, but the dimensions specify " +
                    lTotalTiles.ToString() + ".");
            }

            output.Write(value.Name);
            output.WriteObject(value.Dimensions);
            output.WriteObject(value.TileSize);
            output.WriteObject(value.TextureName);
            output.WriteObject(value.BaseLayer);
            output.WriteObject(value.ObjectLayer);

        }
    }
}
