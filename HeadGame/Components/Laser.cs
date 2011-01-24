using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using DDW.V2D;
using Microsoft.Xna.Framework;
using HeadGame.Particles;
using V2DRuntime.Attributes;
using DDW.Display;
using Box2D.XNA;
using HeadGame.Screens;

namespace HeadGame.Components
{
    public class Laser : V2DSprite
    {
        public Sprite laserPot;

        [SpriteAttribute(depthGroup = 1000)]
        public Sprite laserPoint;

        LaserParticleEffect laserParticles;

        private float angle = 0;
        private Vector2 point1;
        private Vector2 point2;
        private Vector2 hitPoint = Vector2.Zero;
        private Vector2 hitNormal = Vector2.Zero;
        private bool hitClosest = false;

        BaseScreen levelScreen; 

        public Laser(Texture2D texture, V2DInstance instance) : base(texture, instance)
        {            
        }

        public override void Activate()
        {
            base.Activate();
            laserParticles.Begin();
                    }
        public override void Deactivate()
        {
            base.Deactivate();
            laserParticles.End();
        }
        protected override void OnAddToStageComplete()
        {
            base.OnAddToStageComplete();
            levelScreen = (BaseScreen)screen;

            point1 = new Vector2(laserPot.X / V2DScreen.WorldScale, laserPot.Y / V2DScreen.WorldScale);

            laserParticles = (LaserParticleEffect)CreateInstance(
                "laserParticles",
                "laserParticles",
                laserPot.X,
                laserPot.Y,
                0);
        }
        public override void RemovedFromStage(EventArgs e)
        {
            base.RemovedFromStage(e);
            levelScreen = null;
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);

            angle += .008f;
            laserPot.Rotation = angle;

            float len = 70f;
            Vector2 d = new Vector2(len * (float)Math.Cos(angle), len * (float)Math.Sin(angle));
            point2 = point1 + d;

            levelScreen.world.RayCast((fixture, point, normal, fraction) =>
            {
                Body body = fixture.GetBody();
                if (body != null && body.GetUserData() is Sprite)
                {
                    Sprite sp = (Sprite)body.GetUserData();
                    if (levelScreen.headPlayer[0].Contains(sp))
                    {
                        levelScreen.headPlayer[0].Electrocute();
                    }
                    else if (levelScreen.headPlayer[1].Contains(sp))
                    {
                        levelScreen.headPlayer[1].Electrocute();
                    }
                }

                hitClosest = true;
                hitPoint = point;
                hitNormal = normal;
                return fraction;
            }, point1, point2);

            Vector2 endPoint = (hitPoint == Vector2.Zero) ? point2 : hitPoint;

            laserParticles.laserDirection = angle;
            laserParticles.laserLength = Vector2.Distance(point1, endPoint) * V2DScreen.WorldScale;

            laserPoint.X = endPoint.X * V2DScreen.WorldScale;
            laserPoint.Y = endPoint.Y * V2DScreen.WorldScale;
        }
        public override void Draw(SpriteBatch batch)
        {
            base.Draw(batch);

            if (levelScreen.useDebugDraw && levelScreen.world.DebugDraw != null)
            {
                if (hitClosest)
                {
                    levelScreen.world.DebugDraw.DrawCircle(hitPoint, .5f, new Color(0.4f, 0.9f, 0.4f));

                    levelScreen.world.DebugDraw.DrawSegment(point1, hitPoint, new Color(0.8f, 0.8f, 0.8f));

                    Vector2 head = hitPoint + 7f * hitNormal;
                    levelScreen.world.DebugDraw.DrawSegment(hitPoint, head, new Color(0.9f, 0.9f, 0.4f));
                }
                else
                {
                    levelScreen.world.DebugDraw.DrawSegment(point1, point2, new Color(0.8f, 0.8f, 0.8f));
                }
            }
        }
    }
}
