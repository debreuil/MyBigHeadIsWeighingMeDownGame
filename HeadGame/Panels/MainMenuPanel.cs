using System;
using DDW.V2D;
using Microsoft.Xna.Framework.Graphics;
using HeadGame.Screens;
using V2DRuntime.Components;
using V2DRuntime.Display;

namespace HeadGame.Panels
{
    public class MainMenuPanel : Panel
    {
        public ButtonTabGroup menuButtons;
        private MenuState[] buttonTargets = new MenuState[]
        {
            MenuState.QuickGame,
            MenuState.Instructions,
            MenuState.Exit,
        };
        public MainMenuPanel(Texture2D texture, V2DInstance inst) : base(texture, inst) { }

        void menuButtons_OnClick(Button sender, int playerIndex, TimeSpan time)
        {
            MenuState ms = buttonTargets[sender.Index];
            ((TitleScreen)parent).SetPanel(ms);
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