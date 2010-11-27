using System.Collections.Generic;
using DDW.Display;
using DDW.V2D;
using V2DRuntime.Attributes;
using V2DRuntime.V2D;
using DDW.Input;
using Microsoft.Xna.Framework.Input;

using Microsoft.Xna.Framework.Graphics;
using HeadGame.Components;
using Microsoft.Xna.Framework;
using Box2D.XNA;
using V2DRuntime.Enums;

namespace HeadGame.Screens
{
    [V2DScreenAttribute(backgroundColor = 0x000000, gravityX = 0, gravityY = 50, debugDraw = false)]
    public class Level2Screen : BaseScreen
    {
        public V2DSprite pig;

        public Level2Screen(V2DContent v2dContent) : base(v2dContent)
        {
        }
        public Level2Screen(SymbolImport si) : base(si)
        {
            SymbolImport = si;
        }

        public override void Initialize()
        {
            base.Initialize();
            Game1.Hud.SetBackground(1);
        }
        public override void Activate()
        {
            base.Activate();
            ClearBoundsBody(EdgeName.Top);
            ClearBoundsBody(EdgeName.Bottom);
            ClearBoundsBody(EdgeName.Left);
            ClearBoundsBody(EdgeName.Right);
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (pig.Y > 800)
            {
                pig.Y = 0;
            }
            System.Console.WriteLine(headPlayer[1].feet.Y);
            if (headPlayer[0].feet.Y > 635)
            {
                headPlayer[0].MoveUp(80);
            }
            if (headPlayer[1].feet.Y > 635)
            {
                headPlayer[1].MoveUp(80);
            }
        }
    }
}
