using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DDW.V2D;
using Microsoft.Xna.Framework.Input;

namespace HeadGame.Screens
{
    public class SplashScreen : V2DScreen
    {
        public V2DSprite bkg;

        public SplashScreen(V2DContent v2dContent): base(v2dContent)
        {
        }
        public SplashScreen(SymbolImport si) : base(si)
        {
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);
            KeyboardState ks = Keyboard.GetState();
            if (ks.GetPressedKeys().Length > 0 && stage != null)
            {
                stage.NextScreen();
            }
        }
    }
}
