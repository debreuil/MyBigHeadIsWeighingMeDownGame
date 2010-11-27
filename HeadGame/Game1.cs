using DDW.V2D;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using HeadGame.Screens;
using DDW.Display;
using System.Reflection;
using System;

namespace HeadGame
{
    public class Game1 : V2DGame
	{
		public Game1() : base()
		{
        }
        protected override void LoadContent()
        {
            base.LoadContent();
            FontManager.Instance.AddFont("Arial Black", V2DGame.contentManager.Load<SpriteFont>(@"ArialBlack"));
        }

        private static HudScreen hud;
        public static HudScreen Hud
        {
            get
            {
                if (hud == null)
                {
                    SymbolImport si = new SymbolImport("gameScreens", "hudScreen");
                    hud = new HudScreen(si);
                    hud.Visible = false;
                    stage.AddScreen(hud);
                }
                return hud;
            }
        }

		public override bool HasCursor { get { return true; } }
        protected override void CreateScreens()
        {
            //stage.AddScreen(new TitleScreen(new SymbolImport("gameScreens", "titleScreens"))); 
            hud = new HudScreen(new SymbolImport("gameScreens", "hudScreen"));
            stage.AddScreen(hud);
            stage.AddScreen(new Level0Screen(new SymbolImport("gameScreens", "level0Screen")));
            stage.AddScreen(new Level1Screen(new SymbolImport("gameScreens", "level1Screen")));
            stage.AddScreen(new Level2Screen(new SymbolImport("gameScreens", "level2Screen")));
            stage.AddScreen(new Level3Screen(new SymbolImport("gameScreens", "level3Screen")));
            stage.AddScreen(new Level4Screen(new SymbolImport("gameScreens", "level4Screen"))); 
        }

		protected override void Update(Microsoft.Xna.Framework.GameTime gameTime)
		{
			base.Update(gameTime);
#if !XBOX
            KeyboardState ks = Keyboard.GetState();
            if (!keyDown && ks.IsKeyDown(Keys.Y))
            {
                keyDown = true;
                stage.NextScreen();
            }

            keyDown = !ks.IsKeyUp(Keys.Y);
#endif
		}
        protected override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Draw(gameTime);

            //GraphicsDevice.RenderState.DepthBufferEnable = true;
        }
	}
}
