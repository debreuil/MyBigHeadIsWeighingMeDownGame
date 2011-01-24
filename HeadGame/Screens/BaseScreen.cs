using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DDW.Display;
using DDW.V2D;
using V2DRuntime.Attributes;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using V2DRuntime.Shaders;
using Box2D.XNA;
using Microsoft.Xna.Framework.Audio;
using DDW.Input;
using HeadGame.Components;
using HeadGame.Panels;
using V2DRuntime.Enums;

namespace HeadGame.Screens
{
    //public delegate void ContactDelegate(object objA, object objB);

    public abstract class BaseScreen : V2DScreen
    {
        public const string TARGET = "target";
        //public static Dictionary<Type, ContactDelegate> contactTypes = new Dictionary<Type, ContactDelegate>();

        public HudScreen hudScreen;

        //[V2DShaderAttribute(shaderType = typeof(BackgroundShader))]

        [V2DSpriteAttribute(friction = 0.5f, restitution = 0.5F, isStatic = true)]
        public V2DSprite roof;

        [V2DSpriteAttribute(restitution = .8f)]
        public List<HeadPlayer> headPlayer;

        [V2DSpriteAttribute(friction = 0.5f, restitution = 0.5F, isStatic = true)]
        public V2DSprite[] floor;

        [V2DSpriteAttribute(friction = 0.5f, restitution = 0.5F, isStatic = true)]
        public Target[] target;

        public Sprite targetAnim;

        protected int maxScore = 3;
        protected int maxPoints = 10;
        protected uint bkgScreenIndex = 0;
        public float moveForce = 250;
        public float jumpForce = 10000;

        public bool skipLevel = false;
        public bool exitToMainMenu = false;
        protected bool levelOver = false;
        protected bool playersNeedReset = false;
        protected bool roundOver = true;
        protected bool allowInput = true;

        protected Random rnd = new Random((int)DateTime.Now.Ticks);

        public BaseScreen(V2DContent v2dContent): base(v2dContent)
        {
        }
        public BaseScreen(SymbolImport si) : base(si)
        {
        }

        public override void Initialize()
        {
            base.Initialize();
        }
        protected override void OnAddToStageComplete()
        {
            base.OnAddToStageComplete();

            contactTypes.Add(headPlayer[0].feet.GetType(), OnPlayerContact);
        }
        public override void Activate()
        {
            base.Activate();

            levelOver = false;
            skipLevel = false;
            exitToMainMenu = false;

            ClearBoundsBody(EdgeName.Top);

            ResetLevel();
        }

        public override void EndContact(Contact contact)
        {
            base.EndContact(contact);
            object objA = contact.GetFixtureA().GetBody().GetUserData();
            object objB = contact.GetFixtureB().GetBody().GetUserData();

            if (objA is PlayerFeet && objB is V2DSprite)
            {
                ((PlayerFeet)objA).EndContact((V2DSprite)objB);
            }

            if (objB is PlayerFeet && objA is V2DSprite)
            {
                ((PlayerFeet)objB).EndContact((V2DSprite)objA);
            }
        }
        protected void ResetLevel()
        {
            Game1.Hud.scoreMeter[0].SetMaxPoints(maxPoints);
            Game1.Hud.scoreMeter[1].SetMaxPoints(maxPoints);
            Game1.Hud.scoreMeter[0].Reset();
            Game1.Hud.scoreMeter[1].Reset();
            Game1.Hud.SetBackground(bkgScreenIndex);
        }
        protected void SkipLevel()
        {
            skipLevel = true;
        }
        public void ExitToMainMenu()
        {
            exitToMainMenu = true;
        }

        public override void OnUpdateComplete(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.OnUpdateComplete(gameTime);

            if (playersNeedReset)
            {
                playersNeedReset = false;
                ResetPlayers();
            }

            roundOver = false;

            if (skipLevel)
            {
                skipLevel = false;
                stage.NextScreen();
            }
            else if (exitToMainMenu)
            {
                exitToMainMenu = false;
                stage.SetScreen("titleScreens");
            }
            else if (levelOver)
            {
                //                gameOverlay.EndRound();
            }
        }
        protected virtual void OnPlayerContact(object player, object objB, Fixture fixtureA, Fixture fixtureB)
        {
            if (objB is V2DSprite)
            {
                ((PlayerFeet)player).BeginContact((V2DSprite)objB);
            }
        }

        protected virtual void AddPoint(int playerIndex)
        {
            roundOver = true;
            playersNeedReset = true;
            headPlayer[playerIndex].Points += 1;
            Game1.Hud.scoreMeter[playerIndex].SetScoreByRatio(headPlayer[playerIndex].Points / (float)maxScore);
        }

        private void CheckTargets()
        {
            if (target != null)
            {
                uint targOwner = 0;
                int targIndex = headPlayer[0].TargetContactIndex;
                if (targIndex == -1)
                {
                    targOwner = 1;
                    targIndex = headPlayer[1].TargetContactIndex;
                }

                for (int i = 0; i < target.Length; i++)
                {
                    target[i].GotoAndStop(targIndex == i ? 1u + targOwner : 0u);
                }
            }
        }

        protected virtual void CheckForWin()
        {
            int winnerIndex = (headPlayer[0].Points >= maxScore) ? 0 : (headPlayer[1].Points >= maxScore) ? 1 : -1;
            if (winnerIndex >= 0)
            {
                Game1.Hud.endPanel.GotoAndStop((uint)winnerIndex);
                Game1.Hud.endPanel.Visible = true;
                stage.RemoveScreen(this);
            }
        }

        protected virtual void UpdatePlayer(int playerIndex, GameTime gameTime)
        {
        }

        protected virtual void ResetPlayers()
        {
            headPlayer[0].MoveTo(300, 150);
            headPlayer[1].MoveTo(750, 150);
        }

        private void HandlePlayerInput(int playerIndex, Keys left, Keys right, Keys jump, Keys kick)
        {
            if (allowInput)
            {
                KeyboardState ks = Keyboard.GetState();
                PlayerFeet pb = headPlayer[playerIndex].feet;
                Body b = headPlayer[playerIndex].playerBody.body;

                if (ks.IsKeyDown(left))
                {
                    b.ApplyLinearImpulse(new Vector2(-moveForce, 0), b.GetWorldCenter());
                }
                else if (ks.IsKeyDown(right))
                {
                    b.ApplyLinearImpulse(new Vector2(moveForce, 0), b.GetWorldCenter());
                }

                
                if (ks.IsKeyDown(kick))
                {
                    headPlayer[playerIndex].PlayerState = HeadPlayerState.Kick;
                    if (pb.CanJump)
                    {
                        b.ApplyLinearImpulse(new Vector2(0, -jumpForce / 10), b.GetWorldCenter());
                    }
                }
                else
                {
                    headPlayer[playerIndex].PlayerState = HeadPlayerState.Normal;
                }

                if (ks.IsKeyDown(jump))
                {
                    if (!pb.isJumpKeyDown && pb.CanJump)
                    {
                        b.ApplyLinearImpulse(new Vector2(0, -jumpForce), b.GetWorldCenter());
                    }
                    pb.isJumpKeyDown = true;
                }
                else
                {
                    pb.isJumpKeyDown = false;
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            //HandlePlayerInput(0, Keys.J, Keys.L, Keys.I, Keys.K);
            HandlePlayerInput(0, Keys.A, Keys.D, Keys.W, Keys.S);
            HandlePlayerInput(1, Keys.Left, Keys.Right, Keys.Up, Keys.Down);
            base.Update(gameTime);

            UpdatePlayer(0, gameTime);
            UpdatePlayer(1, gameTime);
            CheckTargets();
            CheckForWin();
        }
    }
}
