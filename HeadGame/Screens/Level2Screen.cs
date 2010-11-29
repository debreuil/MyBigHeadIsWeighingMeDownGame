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
        public Pig pig;

        private bool pigNeedsReset;

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
            bkgScreenIndex = 1;
            maxPoints = 3;
        }
        protected override void OnAddToStageComplete()
        {
            base.OnAddToStageComplete();
            contactTypes.Add(pig.GetType(), OnPigContact);
        }
        public override void Activate()
        {
            base.Activate();
            ClearBoundsBody(EdgeName.Bottom);
            ClearBoundsBody(EdgeName.Left);
            ClearBoundsBody(EdgeName.Right);
        }

        private void ResetPig()
        {
            pig.body.SetLinearVelocity(Vector2.Zero);
            pig.X = ClientSize.X / 2 - pig.VisibleWidth / 2;
            pig.Y = 0;
        }

        protected override void OnPlayerContact(object player, object objB, Fixture fixtureA, Fixture fixtureB)
        {
            base.OnPlayerContact(player, objB, fixtureA, fixtureB);
            if (!roundOver && objB is Target)
            {
                PlayerFeet pf = (PlayerFeet)player;
                int scoredOnPlayer = pf.Parent.Index == 0 ? 1 : 0;

                V2DSprite head = ((HeadPlayer)pf.Parent).head;
                int pt = thrust[scoredOnPlayer];
                Vector2 impulse = new Vector2(rnd.Next(-pt, pt), -pt);
                head.body.ApplyLinearImpulse(impulse, head.body.GetWorldCenter());
            }
        }
        private int[] thrust = new int[] { 22000, 22000, 3000, 1000 }; // pl0, pl1, bottom hole, top ledge
        public void OnPigContact(object pigObj, object objB, Fixture fixtureA, Fixture fixtureB)
        {
            if (!roundOver && objB is Target)
            {
                Target t = (Target)objB;
                if (t.Index < 2)
                {
                    pigNeedsReset = true;
                    int scoredOnPlayer = t.Index == 0 ? 1 : 0;
                    AddPoint(scoredOnPlayer);
                }
                else
                {
                    int pt = thrust[t.Index];
                    Vector2 pigImpulse = new Vector2(rnd.Next(-pt, pt), -pt);
                    pig.body.ApplyLinearImpulse(pigImpulse, pig.body.GetWorldCenter());
                }
            }
        }

        public override void BeginContact(Contact contact)
        {
            base.BeginContact(contact);
        }

        public override void OnUpdateComplete(GameTime gameTime)
        {
            base.OnUpdateComplete(gameTime);

            if (pigNeedsReset)
            {
                pigNeedsReset = false;
                ResetPig();
            }
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}
