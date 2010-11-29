using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DDW.V2D;
using Microsoft.Xna.Framework.Graphics;

namespace HeadGame.Components
{
    public class TimedBomb : V2DSprite
    {
        public delegate void ExplodeDelegate(TimedBomb bomb);

        public SevenSegmentDisplay[] ledSegment; 

        private int counter; // ms
        //private ExplodeDelegate Explode;
        private Random rnd = new Random((int)DateTime.Now.Ticks);

        public TimedBomb(Texture2D texture, V2DInstance instance) : base(texture, instance)
        {
        }

        public event ExplodeDelegate OnExplode;

        protected override void OnAddToStageComplete()
        {
            base.OnAddToStageComplete();
            Reset();
            ledSegment[1].SetDot(true);
        }

        public void Reset()
        {
            counter = rnd.Next(20000, 30000);
            //counter = rnd.Next(2000, 3000);
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);
            if (counter > 0)
            {
                counter -= gameTime.ElapsedGameTime.Milliseconds;

                ledSegment[0].SetValue( (uint)((counter % 1000) / 100) );
                ledSegment[1].SetValue( (uint)((counter % 10000) / 1000) );
                if (counter < 10000)
                {
                    ledSegment[2].Visible = false;
                }
                else
                {
                    ledSegment[2].SetValue((uint)((counter % 100000) / 10000));
                }

                if (counter <= 0)
                {
                    OnExplode.Invoke(this);
                }
            }
        }

    }
}
