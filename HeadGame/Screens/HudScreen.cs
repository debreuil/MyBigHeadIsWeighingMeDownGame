using System;
using DDW.V2D;
using Microsoft.Xna.Framework.Graphics;
using HeadGame.Screens;
using V2DRuntime.Components;
using V2DRuntime.Display;
using HeadGame.Components;
using V2DRuntime.Attributes;
using DDW.Display;
using V2DRuntime.V2D;

namespace HeadGame.Screens
{
    [ScreenAttribute(depthGroup = -10, isPersistantScreen = true)]
    public class HudScreen : V2DScreen
    {
        public Sprite bkg;

        [V2DSpriteAttribute(friction = 0.5f, restitution = 0.5F, isStatic = true)]
        public V2DSprite roof;

        public ScoreMeter[] scoreMeter;
        
        public HudScreen(V2DContent v2dContent): base(v2dContent)
        {
        }
        public HudScreen(SymbolImport si) : base(si)
        {
        }

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
        public void SetBackground(uint index)
        {
            bkg.GotoAndStop(index);
        }
    }
}