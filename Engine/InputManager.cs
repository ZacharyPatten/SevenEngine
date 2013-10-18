using System;

using OpenTK.Input;

namespace Engine
{
  /// <summary>Manages input using a state system consisting of two states.</summary>
  public static class InputManager
  {
    // Reference to the keyboard input
    private static Engine.Input.Keyboard _keyboard;

    private static Mouse _mouse;

    public static Engine.Input.Keyboard Keyboard { get { return _keyboard; } }

    public static Mouse Mouse { get { return _mouse; } }

    /// <summary>This initializes the reference to OpenTK's keyboard.</summary>
    /// <param name="keyboardDevice">Reference to OpenTK's KeyboardDevice class within their GameWindow class.</param>
    public static void InitializeKeyboard(KeyboardDevice keyboardDevice) { _keyboard = new Input.Keyboard(keyboardDevice); }

    public static void InitializeMouse(MouseDevice mouse) { _mouse = new Mouse(mouse); }

    /// <summary>Update the input states.</summary>
    public static void Update()
    {
      _keyboard.Update();
      //_mouse.Update();
    }
  }
}