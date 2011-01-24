using System;
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
using V2DRuntime.Particles;
using HeadGame.Particles;

namespace HeadGame.Screens
{
    [V2DScreenAttribute(backgroundColor = 0x000000, gravityX = 0, gravityY = 50, debugDraw = false)]
    public class Level0Screen : BaseScreen
    {
        public Sprite laserPot;

        [SpriteAttribute(depthGroup = 1000)]
        public Sprite laserPoint;

        LaserParticleEffect laserParticles;

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
            bkgScreenIndex = 0;
            maxPoints = 10;
            laserPoint.Play();
        }
        public override void Activate()
        {
            base.Activate();
            ClearBoundsBody(EdgeName.Top);
        }
        protected override void OnAddToStageComplete()
        {
            base.OnAddToStageComplete();

            point1 = new Vector2(laserPot.X / WorldScale, laserPot.Y / WorldScale);

            laserParticles = (LaserParticleEffect)CreateInstance(
                "laserParticles",
                "laserParticles",
                laserPot.X,
                laserPot.Y, 
                0);
        }

        protected override void UpdatePlayer(int playerIndex, GameTime gameTime)
        {
            if (headPlayer[playerIndex].TargetContactIndex > -1)
            {
                headPlayer[playerIndex].Points += gameTime.ElapsedGameTime.Milliseconds;
                
                int pts = headPlayer[playerIndex].Points;
                Game1.Hud.scoreMeter[playerIndex].SetScoreByRatio((float)(pts / (float)maxScore));
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            angle += .008f;
            laserPot.Rotation = angle;

            float len = 70f;
            Vector2 d = new Vector2(len * (float)Math.Cos(angle), len * (float)Math.Sin(angle));
            point2 = point1 + d;

            world.RayCast((fixture, point, normal, fraction) =>
            {
                Body body = fixture.GetBody();
                if (body != null && body.GetUserData() is Sprite)
                {
                    Sprite sp = (Sprite)body.GetUserData();
                    if (headPlayer[0].Contains(sp))
                    {
                        headPlayer[0].Electrocute();
                    }
                    else if (headPlayer[1].Contains(sp))
                    {
                        headPlayer[1].Electrocute();
                    }
                }

                hitClosest = true;
                hitPoint = point;
                hitNormal = normal;
                return fraction;
            }, point1, point2);

            Vector2 endPoint = (hitPoint == Vector2.Zero) ? point2 : hitPoint;

            laserParticles.laserDirection = angle;
            laserParticles.laserLength = Vector2.Distance(point1, endPoint) * WorldScale;

            laserPoint.X = endPoint.X * WorldScale;
            laserPoint.Y = endPoint.Y * WorldScale;
        }
        private float angle = 0;
        private Vector2 point1;
        private Vector2 point2;
        private Vector2 hitPoint = Vector2.Zero; 
        private Vector2 hitNormal = Vector2.Zero;
        private bool hitClosest = false;

        public override void Draw(SpriteBatch batch)
        {
            base.Draw(batch);


            if (useDebugDraw && world.DebugDraw != null)
            {
                if (hitClosest)
                {
                    world.DebugDraw.DrawCircle(hitPoint, .5f, new Color(0.4f, 0.9f, 0.4f));

                    world.DebugDraw.DrawSegment(point1, hitPoint, new Color(0.8f, 0.8f, 0.8f));

                    Vector2 head = hitPoint + 7f * hitNormal;
                    world.DebugDraw.DrawSegment(hitPoint, head, new Color(0.9f, 0.9f, 0.4f));
                }
                else
                {
                    world.DebugDraw.DrawSegment(point1, point2, new Color(0.8f, 0.8f, 0.8f));
                }
            }

        }
    }
}
