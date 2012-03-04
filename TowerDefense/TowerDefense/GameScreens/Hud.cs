using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TowerDefense
{
    class Hud 
    {

        private ScreenManager mScreenManager; 

        #region Initialization

        public Hud(ScreenManager aScreenManager)
        {
            if (aScreenManager == null)
            {
                throw new ArgumentNullException("screenManager");
            }

            mScreenManager = aScreenManager;
        }

        /// <summary>
        /// Load the graphics content from the content manager.
        /// </summary>
        public void LoadContent()
        {

        }

        #endregion //Initialization


        #region Drawing

        /// <summary>
        /// Draw the screen.
        /// </summary>
        public void Draw()
        {

        }
        #endregion //Drawing
    }
}
