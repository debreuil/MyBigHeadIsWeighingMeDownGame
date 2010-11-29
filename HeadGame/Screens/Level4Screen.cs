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
    public class Level4Screen : BaseScreen
    {
        public TimedBomb bomb;

        private int deadTimeCounter;
        private bool hasExploded;

        public Level4Screen(V2DContent v2dContent) : base(v2dContent)
        {
        }
        public Level4Screen(SymbolImport si) : base(si)
        {
            SymbolImport = si;
        }

        public override void Initialize()
        {
            base.Initialize();
            bkgScreenIndex = 1;
        }

        public override void Activate()
        {
            base.Activate();

            //bomb.OnExplode -= new TimedBomb.ExplodeDelegate(OnBombReadyToExplode); // guard
            bomb.OnExplode += new TimedBomb.ExplodeDelegate(OnBombReadyToExplode);
            hasExploded = false;
        }

        void OnBombReadyToExplode(TimedBomb bomb)
        {
            allowInput = false;
            bomb.OnExplode -= new TimedBomb.ExplodeDelegate(OnBombReadyToExplode);
        }

        protected override void OnAddToStageComplete()
        {
            base.OnAddToStageComplete();
            deadTimeCounter = 4000;
        }
        private void Explode()
        {
            hasExploded = true;

            if (headPlayer[0].feet.body.GetPosition().X < headPlayer[1].feet.body.GetPosition().X)
            {
                Game1.Hud.endPanel.GotoAndStop(0);
                Game1.Hud.endPanel.Visible = true;
            }
            else
            {
                Game1.Hud.endPanel.GotoAndStop(1);
                Game1.Hud.endPanel.Visible = true;
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (!allowInput && !hasExploded)
            {
                deadTimeCounter -= gameTime.ElapsedGameTime.Milliseconds;
                if (deadTimeCounter < 0)
                {
                    Explode();
                }
            }
        }
    }
}
