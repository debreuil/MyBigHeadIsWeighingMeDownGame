using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DDW.V2D;
using V2DRuntime.Attributes;
using Microsoft.Xna.Framework.Graphics;
using HeadGame.Screens;

namespace HeadGame.Components
{
    public class PlayerFeet : V2DSprite
    {
        public int contactCount;
        private float airTime;

        public bool isJumpKeyDown;
        public int targetContactIndex = -1;

        public PlayerFeet(Texture2D texture, V2DInstance instance) : base(texture, instance)
		{
        }
        public override void AddedToStage(EventArgs e)
        {
            base.AddedToStage(e);
            targetContactIndex = -1;
        }

        public bool CanJump
        {
            get
            {
                return (contactCount > 0) || (airTime < 0.2F);
            }  
        }

        public void BeginContact(V2DSprite objB)
        {
            if (objB.Parent != this.Parent)
            {
                contactCount++;
                airTime = 0;
                if(objB is Target)
                {
                    targetContactIndex = objB.Index;
                }
            }
        }

        public void EndContact(V2DSprite objB)
        {
            if (objB.Parent != this.Parent)
            {
                contactCount--;
                if (objB is Target)
                {
                    targetContactIndex = -1;
                }
            }
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);

            airTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
        }
    }
}
