using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DDW.V2D;
using Microsoft.Xna.Framework.Graphics;

namespace HeadGame.Components
{
    public class Target : V2DSprite
    {
        public Target(Texture2D texture, V2DInstance instance) : base(texture, instance)
        {
        }
    }
}
