using System;
using DDW.V2D;
using Microsoft.Xna.Framework.Graphics;
using HeadGame.Screens;
using V2DRuntime.Components;
using V2DRuntime.Display;
using HeadGame.Components;
using V2DRuntime.Attributes;

namespace HeadGame.Panels
{
    public class OverlayPanel : Panel
    {
        [V2DSpriteAttribute(friction = 0.5f, restitution = 0.5F, isStatic = true)]
        public V2DSprite roof;

        public ScoreMeter[] scoreMeter;

        public OverlayPanel(Texture2D texture, V2DInstance inst) : base(texture, inst) { }

        public override void Initialize()
        {
            base.Initialize();
        }
        public override void Activate()
        {
            base.Activate();
        }
        public override void Deactivate()
        {
            base.Deactivate();
        }
    }
}