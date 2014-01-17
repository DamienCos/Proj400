using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace AndroidTest
{
    public class InputAction
    {
        public int Id { get; private set; }
#if WINDOWS || XBOX
        public Buttons? GamePadButton { get; set; }
        public MouseButtons? MouseButton { get; set; }
        public Keys? KeyButton { get; set; }
        public TriggerState TriggerButtonState { get; set; }

#endif
        public Vector2 ThumbstickPosition { get; set; }
        public VirtualButtonState TriggerButtonState { get; set; }
        public Virtual_Button S_Pad_Button { get; set; }

        public bool IsTriggered { get; set; }

        public InputAction(int id, VirtualButtonState triggerButtonState, Vector2 thumbstickPosition)
        {
            Id = id;
            TriggerButtonState = triggerButtonState;
            ThumbstickPosition = thumbstickPosition;
        }

        public InputAction(int id, VirtualButtonState triggerButtonState)
        {
            Id = id;
            TriggerButtonState = triggerButtonState;
            
        }
    }


    static class S_Pad_StateEx
    {
        public static bool IsButtonDown(this ScreenPadState S_pad_State, Virtual_Button button)
        {
            return GetButtonState(S_pad_State, button) == VirtualButtonState.Pressed;
        }

        public static bool IsButtonUp(this ScreenPadState S_pad_State, Virtual_Button button)
        {
            return GetButtonState(S_pad_State, button) == VirtualButtonState.Released;
        }

        private static VirtualButtonState GetButtonState(ScreenPadState S_pad_State, Virtual_Button button)
        {
            switch (button)
            {
                case Virtual_Button.A:
                    return S_pad_State.Buttons.A;
                case Virtual_Button.B:
                    return S_pad_State.Buttons.B;
                case Virtual_Button.X:
                    return S_pad_State.Buttons.X;
                case Virtual_Button.Y:
                    return S_pad_State.Buttons.Y;
                //case Virtual_Button.thumbstick:
                   // return S_pad_State.ThumbSticks.Left;
            }

            return VirtualButtonState.Released;
        }
    }



    public class InputManager
    {
        private readonly Dictionary<int, InputAction> _actions = new Dictionary<int, InputAction>();
#if WINDOWS || XBOX
        public GamePadState CurrentGamepadState { get; private set; }
        public GamePadState OldGamepadState { get; private set; }

        public MouseState CurrentMouseState { get; private set; }
        public MouseState OldMouseState { get; private set; }

        public KeyboardState CurrentKeyboardState { get; private set; }
        public KeyboardState OldKeyboardState { get; private set; }
#else
        public ScreenPadState CurrentScreenPadState { get;  set; }
        public ScreenPadState OldScreenPadState { get; private set; }
        public ScreenPad screenPad { get;  set; }///////////////////////////////////////////////////////////////////////////ADD this
#endif

        public void MapAction(InputAction action)
        {
            _actions[action.Id] = action;
        }

        public InputAction GetAction(int id)
        {
            return _actions.ContainsKey(id) ? _actions[id] : null;
        }

        public bool IsActionTriggered(int id)
        {
            return _actions.ContainsKey(id) && _actions[id].IsTriggered;
        }

        public void Update()
        {
#if WINDOWS || XBOX
            OldGamepadState = CurrentGamepadState;
            OldKeyboardState = CurrentKeyboardState;
            OldMouseState = CurrentMouseState;

            CurrentGamepadState = GamePad.GetState(PlayerIndex.One);
            CurrentKeyboardState = Keyboard.GetState();
            CurrentMouseState = Mouse.GetState();
#else
            OldScreenPadState = CurrentScreenPadState;
            CurrentScreenPadState = screenPad.GetState();
#endif

            foreach (var inputAction in _actions.Values)
            {
                inputAction.IsTriggered = false;
#if WINDOWS || XBOX
                if (inputAction.TriggerButtonState == TriggerState.Down)
                {
                    //Check Gamepad
                    if (inputAction.GamePadButton.HasValue &&
                        CurrentGamepadState.IsButtonDown(inputAction.GamePadButton.Value))
                    {
                        inputAction.IsTriggered = true;
                    }

                        //Check Keyboard
                    else if (inputAction.KeyButton.HasValue &&
                             CurrentKeyboardState.IsKeyDown(inputAction.KeyButton.Value))
                    {
                        inputAction.IsTriggered = true;
                    }

                        //Check Mouse
                    else if (inputAction.MouseButton.HasValue &&
                             CurrentMouseState.IsButtonDown(inputAction.MouseButton.Value))
                    {
                        inputAction.IsTriggered = true;
                    }

                }
                else if (inputAction.TriggerButtonState == TriggerState.Pressed)
                {

                    //Check Gamepad
                    if (inputAction.GamePadButton.HasValue &&
                        CurrentGamepadState.IsButtonDown(inputAction.GamePadButton.Value) &&
                         OldGamepadState.IsButtonUp(inputAction.GamePadButton.Value))
                    {
                        inputAction.IsTriggered = true;
                    }

                        //Check Keyboard
                    else if (inputAction.KeyButton.HasValue &&
                             CurrentKeyboardState.IsKeyDown(inputAction.KeyButton.Value) &&
                         OldKeyboardState.IsKeyUp(inputAction.KeyButton.Value))
                    {
                        inputAction.IsTriggered = true;
                    }

                        //Check Mouse
                    else if (inputAction.MouseButton.HasValue &&
                             CurrentMouseState.IsButtonDown(inputAction.MouseButton.Value) &&
                         OldMouseState.IsButtonUp(inputAction.MouseButton.Value))
                    {
                        inputAction.IsTriggered = true;
                    }

                }
                else if (inputAction.TriggerButtonState == TriggerState.Released)
                {
                    //Check Gamepad
                    if (inputAction.GamePadButton.HasValue &&
                        (CurrentGamepadState.IsButtonUp(inputAction.GamePadButton.Value) &&
                         OldGamepadState.IsButtonDown(inputAction.GamePadButton.Value)))
                    {
                        inputAction.IsTriggered = true;
                    }

                        //Check Keyboard
                    else if (inputAction.KeyButton.HasValue &&
                             (CurrentKeyboardState.IsKeyUp(inputAction.KeyButton.Value) &&
                              OldKeyboardState.IsKeyDown(inputAction.KeyButton.Value)))
                    {
                        inputAction.IsTriggered = true;
                    }

                        //Check Mouse
                    else if (inputAction.MouseButton.HasValue &&
                             (CurrentMouseState.IsButtonUp(inputAction.MouseButton.Value) &&
                              OldMouseState.IsButtonDown(inputAction.MouseButton.Value)))
                    {
                        inputAction.IsTriggered = true;
                    }
                }
                else inputAction.IsTriggered = true;
#endif
                if (inputAction.TriggerButtonState == VirtualButtonState.Pressed)
                {
                    inputAction.IsTriggered = true;
                }
                //else
                //    inputAction.IsTriggered = true;

                if (inputAction.ThumbstickPosition != Vector2.Zero)
                {
                    inputAction.IsTriggered = true;
                }
            }
        }
    }
}
