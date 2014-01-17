using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

namespace AndroidTest
{
    public struct ScreenPadState
    {
        public ThumbSticks ThumbSticks;
        public virtualButtons Buttons;

        public ScreenPadState(ThumbSticks sticks, virtualButtons btns)
        {
            ThumbSticks = sticks;
            Buttons = btns;
        }
    }

    public struct virtualButtons
    {
        public VirtualButtonState X;
        public VirtualButtonState Y;
        public VirtualButtonState A;
        public VirtualButtonState B;

        public virtualButtons(VirtualButtonState a, VirtualButtonState b, VirtualButtonState x, VirtualButtonState y)
        {
            X = x;
            Y = y;
            A = a;
            B = b;
        }
    }

    public struct ThumbSticks
    {
        public Vector2 Left;
        public Vector2 Right;

        public ThumbSticks(Vector2 left, Vector2 right)
        {
            Left = left;
            Right = right;
        }
    }

    public enum Virtual_Button
    {
        A,
        B,
        X,
        Y,
        thumbstick
    }

    public enum VirtualButtonState
    {
        Pressed = 1,
        Released = 0
    }
}
