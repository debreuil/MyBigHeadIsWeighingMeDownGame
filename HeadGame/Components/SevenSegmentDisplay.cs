using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DDW.V2D;
using V2DRuntime.Attributes;
using Microsoft.Xna.Framework.Graphics;
using HeadGame.Screens;
using DDW.Display;

namespace HeadGame.Components
{
    public class SevenSegmentDisplay : V2DSprite
    {
        protected Sprite[] segment;

        protected uint currentValue;

        private bool[][] digitTable = new bool[][]
        {
            new bool[]{true,    true,   true,   true,   true,   true,   false}, //  0
			new	bool[]{false,	true,	true,	false,	false,	false,	false},	//	1
			new	bool[]{true,	true,	false,	true,	true,	false,	true},	//	2
			new	bool[]{true,	true,	true,	true,	false,	false,	true},	//	3
			new	bool[]{false,	true,	true,	false,	false,	true,	true},	//	4
			new	bool[]{true,	false,	true,	true,	false,	true,	true},	//	5
			new	bool[]{true,	false,	true,	true,	true,	true,	true},	//	6
			new	bool[]{true,	true,	true,	false,	false,	false,	false},	//	7
			new	bool[]{true,	true,	true,	true,	true,	true,	true},	//	8
			new	bool[]{true,	true,	true,	true,	false,	true,	true},	//	9
			new	bool[]{true,	true,	true,	false,	true,	true,	true},	//	A
			new	bool[]{false,	false,	true,	true,	true,	true,	true},	//	B
			new	bool[]{true,	false,	false,	true,	true,	true,	false},	//	C
			new	bool[]{false,	true,	true,	true,	true,	false,	true},	//	D
			new	bool[]{true,	false,	false,	true,	true,	true,	true},	//	E
			new	bool[]{true,	false,	false,	false,	true,	true,	true},	//	F
        };

        public SevenSegmentDisplay(Texture2D texture, V2DInstance instance) : base(texture, instance)
		{
        }

        protected override void OnAddToStageComplete()
        {
            base.OnAddToStageComplete();
            SetDot(false);
        }

        public void SetValue(uint val)
        {
            if(val >= 0 && val < digitTable.Length)
            {
                currentValue = val;
                bool[] displayVal = digitTable[currentValue];

                for (int i = 0; i < 7; i++)
                {
                    segment[i].Visible = displayVal[i];
                }
            }
        }

        public void SetDot(bool val)
        {
            segment[7].Visible = val;
        }
    }
}