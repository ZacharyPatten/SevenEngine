// SEVENENGINE LISCENSE:
// You are free to use, modify, and distribute any or all code segments/files for any purpose
// including commercial use with the following condition: any code using or originally taken 
// from the SevenEngine project must include citation to its original author(s) located at the
// top of each source code file, or you may include a reference to the SevenEngine project as
// a whole but you must include the current SevenEngine official website URL and logo.
// - Thanks.  :)  (support: seven@sevenengine.com)

// Author(s):
// - Zachary Aaron Patten (aka Seven) seven@sevenengine.com
// Last Edited: 10-26-13

using System;
using OpenTK.Input;

namespace SevenEngine
{
  /// <summary>Manages input using a state system consisting of two states.</summary>
  public static class InputManager
  {
    // Reference to the keyboard input
    private static SevenEngine.Input.Keyboard _keyboard;

    private static Mouse _mouse;

    public static SevenEngine.Input.Keyboard Keyboard { get { return _keyboard; } }

    public static Mouse Mouse { get { return _mouse; } }

    /// <summary>This initializes the reference to OpenTK's keyboard.</summary>
    /// <param name="keyboardDevice">Reference to OpenTK's KeyboardDevice class within their GameWindow class.</param>
    public static void InitializeKeyboard(KeyboardDevice keyboardDevice) { _keyboard = new Input.Keyboard(keyboardDevice); }

    public static void InitializeMouse(MouseDevice mouse) { _mouse = new Mouse(mouse); }

    /// <summary>Update the input states.</summary>
    public static void Update()
    {
      _keyboard.Update();
      _mouse.Update();
    }
  }
}