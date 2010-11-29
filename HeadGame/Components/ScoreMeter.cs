using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DDW.V2D;
using V2DRuntime.Attributes;
using Microsoft.Xna.Framework.Graphics;
using DDW.Display;

namespace HeadGame.Components
{
    public class ScoreMeter : V2DSprite
    {
        public Sprite bkg;
        public List<Sprite> levelScore;

        private int maxPoints = 10;
        private int pointOffset = 0;

        public ScoreMeter(Texture2D texture, V2DInstance instance) : base(texture, instance)
		{
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public void Reset()
        {
            SetScoreByRatio(0);
        }
        public void SetMaxPoints(int maxPoints)
        {
            this.maxPoints = maxPoints;
            if (maxPoints == 3)
            {
                GotoAndStop(1);
                pointOffset = 10;
            }
            else
            {
                GotoAndStop(0);
                pointOffset = 0;
            }
        }
        public void SetScoreByRatio(float ratio)
        {
            int val = (int)(ratio * maxPoints) + pointOffset;
            for (int i = pointOffset; i < levelScore.Count; i++)
            {
                levelScore[i].GotoAndStop(i < val ? 1u : 0u);
            }
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}
