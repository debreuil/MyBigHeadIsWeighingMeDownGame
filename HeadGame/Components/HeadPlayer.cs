using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DDW.Display;
using Box2D.XNA;
using Microsoft.Xna.Framework.Graphics;
using DDW.V2D;
using V2DRuntime.Game;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using V2DRuntime.Attributes;

namespace HeadGame.Components
{
    public class HeadPlayer : V2DSprite
    {
        [V2DSpriteAttribute(density = 1.5F, friction = 0.5F)]
        public V2DSprite head;

        public V2DSprite neck;
        public V2DSprite playerBody;

        [V2DSpriteAttribute(restitution = .8f, friction = .1f, fixedRotation = true)]
        public PlayerFeet feet;

        protected int points = 0;

        public HeadPlayer(Texture2D texture, V2DInstance instance) : base(texture, instance)
		{
        }
        public override void Initialize()
        {
            base.Initialize();
        }
        public int TargetContactIndex
        {
            get
            {
                return this.feet.targetContactIndex;
            }
        }
        
        public int Points
        {
            get
            {
                return points;
            }
            set
            {
                points = value;
            }
        }

        public void MoveTo(float xPos, float yPos)
        {
            xPos /= V2DScreen.WorldScale;
            yPos /= V2DScreen.WorldScale;

            head.body.SetLinearVelocity(Vector2.Zero);
            neck.body.SetLinearVelocity(Vector2.Zero);
            playerBody.body.SetLinearVelocity(Vector2.Zero);
            feet.body.SetLinearVelocity(Vector2.Zero);

            Vector2 orgHead = head.body.Position;
            Vector2 orgNeck = neck.body.Position;
            Vector2 orgBody = playerBody.body.Position;
            Vector2 orgFeet = feet.body.Position;

            head.body.Position = new Vector2(xPos, yPos);
            neck.body.Position = new Vector2(xPos + (orgNeck.X - orgHead.X), yPos + (orgNeck.Y - orgHead.Y));
            playerBody.body.Position = new Vector2(xPos + (orgBody.X - orgHead.X), yPos + (orgBody.Y - orgHead.Y));
            feet.body.Position = new Vector2(xPos + (orgFeet.X - orgHead.X), yPos + (orgFeet.Y - orgHead.Y));
        }
        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}