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

namespace HeadGame.Screens
{
    //public delegate void ContactDelegate(object objA, object objB);

    public abstract class BaseScreen : V2DScreen
    {
        public const string TARGET = "target";
        //public static Dictionary<Type, ContactDelegate> contactTypes = new Dictionary<Type, ContactDelegate>();

        public HudScreen hudScreen;

        //[V2DShaderAttribute(shaderType = typeof(BackgroundShader))]

        [V2DSpriteAttribute(restitution = .8f)]
        public List<HeadPlayer> headPlayer;

        [V2DSpriteAttribute(friction = 0.5f, restitution = 0.5F, isStatic = true)]
        public V2DSprite[] floor;

        [V2DSpriteAttribute(friction = 0.5f, restitution = 0.5F, isStatic = true)]
        public V2DSprite target;

        public Sprite targetAnim;

        protected int maxScore = 3;
        public float moveForce = 250;
        public float jumpForce = 15000;

        public bool restartLevel = false;
        public bool skipLevel = false;
        public bool exitToMainMenu = false;
        protected bool levelOver = false;

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

            restartLevel = false;
            levelOver = false;
            skipLevel = false;
            exitToMainMenu = false;
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
            restartLevel = true;
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

            if (skipLevel)
            {
                skipLevel = false;
                stage.NextScreen();
            }
            else if (restartLevel)
            {
                restartLevel = false;
                stage.SetScreen(this.instanceName);
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

        public override void Update(GameTime gameTime)
        {
            HandlePlayerInput(0, Keys.J, Keys.L, Keys.I, Keys.K);
            HandlePlayerInput(1, Keys.Left, Keys.Right, Keys.Up, Keys.Down);
            base.Update(gameTime);

            UpdatePlayer(0, gameTime);
            UpdatePlayer(1, gameTime);
            CheckTargets();
            CheckForWin();
        }

        private void CheckTargets()
        {
            if (target != null)
            {
                var onTarget = headPlayer[0].IsOnTarget || headPlayer[1].IsOnTarget;
                if (onTarget)
                {
                    target.GotoAndStop(1);
                }
                else
                {
                    target.GotoAndStop(0);
                }
            }
        }

        protected virtual void CheckForWin()
        {
            if (headPlayer[0].points >= maxScore || headPlayer[1].points >= maxScore)
            {
                stage.NextScreen();
            }
        }

        protected virtual void UpdatePlayer(int playerIndex, GameTime gameTime)
        {
        }

        private void HandlePlayerInput(int playerIndex, Keys left, Keys right, Keys jump, Keys kick)
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
                headPlayer[playerIndex].playerBody.GotoAndStop(1);
                if (pb.CanJump)
                {
                    b.ApplyLinearImpulse(new Vector2(0, -jumpForce / 10), b.GetWorldCenter());
                }
            }
            else
            {
                headPlayer[playerIndex].playerBody.GotoAndStop(0);
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
}
