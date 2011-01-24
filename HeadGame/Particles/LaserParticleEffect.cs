using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using V2DRuntime.Particles;
using Microsoft.Xna.Framework.Graphics;
using DDW.V2D;
using V2DRuntime.Tween;
using Microsoft.Xna.Framework;

namespace HeadGame.Particles
{
    public class LaserParticleEffect : ParticleEffect
    {
        public float laserDirection = 0;
        public float laserLength = 500;
        //private float cycle = 0;
        private float jiggleDist = 40;

		public LaserParticleEffect()
		{
		}
        public LaserParticleEffect(Texture2D texture, V2DInstance inst) : base(texture, inst)
		{
		}
		public override void Initialize()
		{
			base.Initialize();
			AutoStart = true;
			maxT = 1;
			steps = 6000;
            particleCount = 90;
		}

		protected override void EffectSetup(Microsoft.Xna.Framework.GameTime gameTime)
		{
			base.EffectSetup(gameTime);
			textureCount = 8;
		}
        float pulse = 0f;
        float flow = 0f;
		protected override void BatchUpdate(Microsoft.Xna.Framework.GameTime gameTime)
		{
            base.BatchUpdate(gameTime);
            particleCount = (int)(laserLength / 7f);
            //cycle += laserLength / 100f;
            pulse = (pulse + .01f) % 1;
            flow = (flow > 0) ? flow - .1f : 1;
            jiggleDist = Math.Abs(pulse - .5f) * 20f + 0;
            seed = (int)(r0 * 10001);
		}
		protected override void BatchDraw(Microsoft.Xna.Framework.Graphics.SpriteBatch batch)
		{
			base.BatchDraw(batch);
		}
		protected override void ParticleDraw(int index, Microsoft.Xna.Framework.Graphics.SpriteBatch batch)
		{
            sourceRectangle.X = (index % 8) * particleWidth;
            //float d = (laserLength * ((float)index / particleCount) + cycle) % laserLength;
            float tp = (float)index / particleCount;
            float d = laserLength * tp;
            pRotation = r2 * Easing.twoPi;
            float ti = index / 60f;
            pColor = r1 < .07f ? Color.White : new Color(Vector4.Lerp(new Vector4(1.0f, .6f, .0f, 1), new Vector4(1.0f, .1f, .0f, 1), ti));
            pScale.X = ((ti + 0f) * 5 * (1 + flow)) % 1.9f;
            pScale.Y = (index < 20) ? (index / 20f) + .04f : 1;
            //pColor = new Color(ti * ti + .5f, ti * ti + .1f, .0f, 1f);

            pPosition.X = (float)Math.Cos(laserDirection) * d + X + r0 * jiggleDist - jiggleDist / 2f;
            pPosition.Y = (float)Math.Sin(laserDirection) * d + Y + r1 * jiggleDist - jiggleDist / 2f;

            base.ParticleDraw(index, batch);
		}
    }
}
