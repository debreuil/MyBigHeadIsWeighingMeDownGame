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
    public class Level3Screen : BaseScreen
    {
        public Level3Screen(V2DContent v2dContent) : base(v2dContent)
        {
        }
        public Level3Screen(SymbolImport si) : base(si)
        {
            SymbolImport = si;
        }

        public override void Initialize()
        {
            base.Initialize();

            maxPoints = 3;
            bkgScreenIndex = 1;
        }
        public override void Activate()
        {
            base.Activate();
            ClearBoundsBody(EdgeName.Bottom);
        }

        protected override void OnPlayerContact(object player, object objB, Fixture fixtureA, Fixture fixtureB)
        {
            base.OnPlayerContact(player, objB, fixtureA, fixtureB);
            if (!roundOver && objB is Target)
            {
                V2DSprite p = (V2DSprite)player;
                int scoredOnPlayer = p.Parent.Index == 0 ? 1 : 0;
                AddPoint(scoredOnPlayer);
            }
        }

    }
}
