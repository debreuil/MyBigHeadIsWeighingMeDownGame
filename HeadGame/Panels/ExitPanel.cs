using System;
using DDW.V2D;
using Microsoft.Xna.Framework.Graphics;
using HeadGame.Screens;
using V2DRuntime.Components;
using V2DRuntime.Display;

namespace HeadGame.Panels
{
	public class ExitPanel : Panel
	{
		public ButtonTabGroup menuButtons;
		private enum ExitButton { Exit, Back };

		public ExitPanel(Texture2D texture, V2DInstance inst) : base(texture, inst) { }

		void menuButtons_OnClick(Button sender, int playerIndex, TimeSpan time)
		{
			switch ((ExitButton)sender.Index)
			{
				case ExitButton.Exit:
					Game1.instance.ExitGame();
					break;
                case ExitButton.Back:
                    ((TitleScreen)parent).SetPanel(MenuState.MainMenu);
					break;
			}
		}
        public override void Activate()
        {
            base.Activate();
            menuButtons.SetFocus(0);
            menuButtons.OnClick += new ButtonEventHandler(menuButtons_OnClick);
        }
        public override void Deactivate()
        {
            base.Deactivate();
            menuButtons.OnClick -= new ButtonEventHandler(menuButtons_OnClick);
        }

	}
}
