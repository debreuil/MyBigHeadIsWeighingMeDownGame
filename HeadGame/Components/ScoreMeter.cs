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

        public int maxPoints = 10;

        public ScoreMeter(Texture2D texture, V2DInstance instance) : base(texture, instance)
		{
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public void SetMaxPoints(int maxPoints)
        {
            this.maxPoints = maxPoints;
            for (int i = 0; i < levelScore.Count; i++)
            {
                levelScore[i].Visible = (i < maxPoints) ? true : false;
            }
        }
        public void SetScoreByRatio(float ratio)
        {
            int val = (int)(ratio * maxPoints);
            for (int i = 0; i < levelScore.Count; i++)
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
