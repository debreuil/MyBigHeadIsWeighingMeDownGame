using System;
using System.Collections.Generic;
using DDW.Display;
using DDW.Input;
using DDW.V2D;
using HeadGame.Panels;
using V2DRuntime.Display;
using V2DRuntime.Network;

namespace HeadGame.Screens
{
    public class TitleScreen : Screen
    {
        public Sprite bkg;

        public MainMenuPanel mainMenuPanel;
        public ExitPanel exitPanel;
        public InstructionsPanel instructionsPanel;

        public Sprite buttonGuide;

        public Panel[] panels;
        private MenuState curState;
        private Stack<Panel> panelStack = new Stack<Panel>();

        public TitleScreen(V2DContent v2dContent) : base(v2dContent)
        {
        }
        public TitleScreen(SymbolImport si) : base(si)
        {
            SymbolImport = si;
        }
        public override void Initialize()
        {
            base.Initialize();
        }
        protected override void OnAddToStageComplete()
        {
            base.OnAddToStageComplete();
            
            panels = new Panel[] { mainMenuPanel, instructionsPanel, exitPanel };

            SetPanel(MenuState.MainMenu);
            mainMenuPanel.Activate();
        }
        public override void AddedToStage(EventArgs e)
        {
            base.AddedToStage(e);
        }
        public override void RemovedFromStage(EventArgs e)
        {
            SetPanel(MenuState.Empty);
            base.RemovedFromStage(e);
        }

        public override bool OnPlayerInput(int playerIndex, Move move, TimeSpan time)
        {
            if (curState != MenuState.MainMenu && move == Move.ButtonB)
            {
                SetPanel(MenuState.MainMenu);
            }
            else
            {
                panelStack.Peek().OnPlayerInput(playerIndex, move, time);
            }
            return false;
        }

        public void SetPanel(MenuState state)
        {
            switch (state)
            {
                case MenuState.Empty:
                    panelStack.Clear();
                    break;

                case MenuState.Instructions:
                    panelStack.Push(instructionsPanel);
                    break;

                case MenuState.MainMenu:
                    panelStack.Push(mainMenuPanel);
                    break;

                case MenuState.Exit:
                    panelStack.Push(exitPanel);
                    //curPanel = exitPanel;
                    break;

                case MenuState.QuickGame:
                    OnStartGame();
                    break;
            }

            Panel cp = panelStack.Count > 0 ? panelStack.Peek() : null;
            for (int i = 0; i < panels.Length; i++)
            {
                if (panels[i] == cp)
                {
                    if (!children.Contains(panels[i]))
                    {
                        AddChild(panels[i]);
                        panels[i].Activate();
                    }
                }
                else
                {
                    if (panels[i].IsOnStage)
                    {
                        panels[i].Deactivate();
                        RemoveChild(panels[i]);
                    }
                }
            }

            if (state == MenuState.MainMenu)
            {
                panelStack.Clear();
                panelStack.Push(mainMenuPanel);
            }

            curState = state;
        }
        protected void OnStartGame()
        {
            stage.NextScreen();
        }
    }
    public enum MenuState
    {
        Empty,
        Begin,
        MainMenu,
        NetworkGame,
        Lobby,
        HostGame,
        JoinGame,
        HighScores,
        Options,
        Instructions,
        UnlockTrial,
        QuickGame,
        Exit,
    }
}
