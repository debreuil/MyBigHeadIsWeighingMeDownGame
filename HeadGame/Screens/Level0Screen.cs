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
    public class Level0Screen : BaseScreen
    {
        public Level0Screen(V2DContent v2dContent) : base(v2dContent)
        {
        }
        public Level0Screen(SymbolImport si) : base(si)
        {
            SymbolImport = si;
        }

        public override void Initialize()
        {
            base.Initialize();
            maxScore = 3000;
        }
        public override void Activate()
        {
            base.Activate();
            ClearBoundsBody(EdgeName.Top);
            Game1.Hud.SetBackground(0);
            Game1.Hud.scoreMeter[0].SetMaxPoints(10);
            Game1.Hud.scoreMeter[1].SetMaxPoints(10);
        }

        protected override void UpdatePlayer(int playerIndex, GameTime gameTime)
        {
            if (headPlayer[playerIndex].IsOnTarget)
            {
                headPlayer[playerIndex].AddPoints(gameTime.ElapsedGameTime.Milliseconds);

                int pts = headPlayer[playerIndex].points;
                Game1.Hud.scoreMeter[playerIndex].SetScoreByRatio((float)(pts / (float)maxScore));
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}
