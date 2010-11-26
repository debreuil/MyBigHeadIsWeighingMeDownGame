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

        [V2DSpriteAttribute(restitution = .1f, friction = .1f, fixedRotation = true)]
        public PlayerFeet feet;

        public int points = 0;

        public HeadPlayer(Texture2D texture, V2DInstance instance) : base(texture, instance)
		{
        }
        public override void Initialize()
        {
            base.Initialize();
        }
        public bool IsOnTarget
        {
            get
            {
                return this.feet.isOnTarget;
            }
        }

        public void AddPoints(int amount)
        {
            points += amount;
        }
        public void MoveUp(int amount)
        {
            //Vector2 org = this.Position;
            //this.Position = new Vector2(org.X, org.Y - amount);

            Vector2 orgHead = head.body.Position;
            Vector2 orgNeck = neck.body.Position;
            Vector2 orgBody = feet.body.Position;
            Console.WriteLine(orgHead);
            head.body.Position = new Vector2(orgHead.X, orgHead.Y - amount);
            neck.body.Position = new Vector2(orgNeck.X, orgNeck.Y - amount);
            feet.body.Position = new Vector2(orgBody.X, orgBody.Y - amount);
        }
        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}