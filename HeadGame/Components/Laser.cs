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
        [V2DSpriteAttribute(depthGroup = 10, fixedRotation = true)]
        public V2DSprite laserSource;

        [V2DSpriteAttribute(depthGroup = 10, fixedRotation = true)]
        public V2DSprite laserTarget;

        [SpriteAttribute(depthGroup = 10)]
        public Sprite laserPoint;

        [SpriteAttribute(depthGroup = 5)]
        LaserParticleEffect laserParticles;

        public float maxLaserLength = 70;

        private Vector2 point1;
        private Vector2 point2;
        private Vector2 hitPoint = Vector2.Zero;
        private Vector2 hitNormal = Vector2.Zero;
        private bool hitClosest = false;

        private Vector2 sourceCenter;
        private Vector2 targetCenter;
        private Vector2 prevSourcePos;
        private Vector2 prevTargetPos;
        private BaseScreen levelScreen; 



        public Laser(Texture2D texture, V2DInstance instance) : base(texture, instance)
        {            
        }

        public override void Activate()
        {
            base.Activate();
            laserParticles.Begin();
            laserPoint.Play();
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

            sourceCenter = GetGlobalOffset(laserSource.Position);

            point1 = new Vector2(sourceCenter.X / V2DScreen.WorldScale, sourceCenter.Y / V2DScreen.WorldScale);

            laserParticles = (LaserParticleEffect)CreateInstance(
                "laserParticles",
                "laserParticles",
                sourceCenter.X,
                sourceCenter.Y,
                0);
        }

        public float Angle
        {
            get { return laserSource.Rotation; }
            set
            {
                if (laserTarget == null)
                {
                    laserSource.Rotation = value;
                }
            }
        }

        public override void RemovedFromStage(EventArgs e)
        {
            base.RemovedFromStage(e);
            levelScreen = null;
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);

            sourceCenter = GetGlobalOffset(laserSource.Position);
            laserParticles.Position = sourceCenter;
            point1 = new Vector2(sourceCenter.X / V2DScreen.WorldScale, sourceCenter.Y / V2DScreen.WorldScale);

            if (laserTarget == null)
            {
                float angle = laserSource.Rotation;

                Vector2 d = new Vector2(maxLaserLength * (float)Math.Cos(angle), maxLaserLength * (float)Math.Sin(angle));
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

                if (laserPoint != null)
                {
                    laserPoint.X = endPoint.X * V2DScreen.WorldScale - this.X;
                    laserPoint.Y = endPoint.Y * V2DScreen.WorldScale - this.Y;
                }
            }
            else
            {
                if (laserSource.Position != prevSourcePos || laserTarget.Position != prevTargetPos)
                {
                    prevSourcePos = laserSource.Position;
                    prevTargetPos = laserTarget.Position;
                    targetCenter = GetGlobalOffset(laserTarget.Position);

                    laserParticles.laserLength = (targetCenter - sourceCenter).Length();
                    float angle = (float)Math.Atan2(targetCenter.Y - sourceCenter.Y, targetCenter.X - sourceCenter.X);
                    laserParticles.laserDirection = angle;
                    laserSource.Rotation = angle;
                    laserTarget.Rotation = angle + (float)Math.PI;
                }
            }
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
