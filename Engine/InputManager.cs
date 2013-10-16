using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OpenTK.Input;

namespace Engine
{
  /// <summary>Manages input using a state system consisting of two states.</summary>
  public static class InputManager
  {
    // The number of keys supported
    private const int _numberOfKeyboardKeys = 144;

    private static bool[] _stateOne = new bool[_numberOfKeyboardKeys];
    private static bool[] _stateTwo = new bool[_numberOfKeyboardKeys];

    // Reference to the current state
    private static bool[] _currentState = _stateOne;
    private static bool[] _previousState = _stateTwo;

    // Reference to OpenTK's keyboard input
    private static KeyboardDevice _keyboard;

    /// <summary>This initializes the reference to OpenTK's keyboard.</summary>
    /// <param name="keyboardDevice">Reference to OpenTK's KeyboardDevice class within their GameWindow class.</param>
    public static void InitializeKeyboard(KeyboardDevice keyboardDevice) { _keyboard = keyboardDevice; }

    /// <summary>Update the input states.</summary>
    public static void Update()
    {
      // Swap the current input state
      if (_currentState.Equals(_stateOne))
        _currentState = _stateTwo;
      else if (_currentState.Equals(_stateTwo))
        _currentState = _stateOne;

      // Swap the old input state
      if (_previousState.Equals(_stateOne))
        _previousState = _stateTwo;
      else if (_previousState.Equals(_stateTwo))
        _previousState = _stateOne;

      // Update all the current key values
      #region Updating Current Key values
      _currentState[(int)Key.A] = _keyboard[Key.A];
      _currentState[(int)Key.B] = _keyboard[Key.B];
      _currentState[(int)Key.C] = _keyboard[Key.C];
      _currentState[(int)Key.D] = _keyboard[Key.D];
      _currentState[(int)Key.E] = _keyboard[Key.E];
      _currentState[(int)Key.F] = _keyboard[Key.F];
      _currentState[(int)Key.G] = _keyboard[Key.G];
      _currentState[(int)Key.H] = _keyboard[Key.H];
      _currentState[(int)Key.I] = _keyboard[Key.I];
      _currentState[(int)Key.J] = _keyboard[Key.J];
      _currentState[(int)Key.K] = _keyboard[Key.K];
      _currentState[(int)Key.L] = _keyboard[Key.L];
      _currentState[(int)Key.M] = _keyboard[Key.M];
      _currentState[(int)Key.N] = _keyboard[Key.N];
      _currentState[(int)Key.O] = _keyboard[Key.O];
      _currentState[(int)Key.P] = _keyboard[Key.P];
      _currentState[(int)Key.Q] = _keyboard[Key.Q];
      _currentState[(int)Key.R] = _keyboard[Key.R];
      _currentState[(int)Key.S] = _keyboard[Key.S];
      _currentState[(int)Key.T] = _keyboard[Key.T];
      _currentState[(int)Key.U] = _keyboard[Key.U];
      _currentState[(int)Key.V] = _keyboard[Key.V];
      _currentState[(int)Key.W] = _keyboard[Key.W];
      _currentState[(int)Key.X] = _keyboard[Key.X];
      _currentState[(int)Key.Y] = _keyboard[Key.Y];
      _currentState[(int)Key.Z] = _keyboard[Key.Z];

      _currentState[(int)Key.Number0] = _keyboard[Key.Number0];
      _currentState[(int)Key.Number1] = _keyboard[Key.Number1];
      _currentState[(int)Key.Number2] = _keyboard[Key.Number2];
      _currentState[(int)Key.Number3] = _keyboard[Key.Number3];
      _currentState[(int)Key.Number4] = _keyboard[Key.Number4];
      _currentState[(int)Key.Number5] = _keyboard[Key.Number5];
      _currentState[(int)Key.Number6] = _keyboard[Key.Number6];
      _currentState[(int)Key.Number7] = _keyboard[Key.Number7];
      _currentState[(int)Key.Number8] = _keyboard[Key.Number8];
      _currentState[(int)Key.Number9] = _keyboard[Key.Number9];

      _currentState[(int)Key.Keypad0] = _keyboard[Key.Keypad0];
      _currentState[(int)Key.Keypad1] = _keyboard[Key.Keypad1];
      _currentState[(int)Key.Keypad2] = _keyboard[Key.Keypad2];
      _currentState[(int)Key.Keypad3] = _keyboard[Key.Keypad3];
      _currentState[(int)Key.Keypad4] = _keyboard[Key.Keypad4];
      _currentState[(int)Key.Keypad5] = _keyboard[Key.Keypad5];
      _currentState[(int)Key.Keypad6] = _keyboard[Key.Keypad6];
      _currentState[(int)Key.Keypad7] = _keyboard[Key.Keypad7];
      _currentState[(int)Key.Keypad8] = _keyboard[Key.Keypad8];
      _currentState[(int)Key.Keypad9] = _keyboard[Key.Keypad9];
      _currentState[(int)Key.KeypadEnter] = _keyboard[Key.KeypadEnter];
      _currentState[(int)Key.KeypadMinus] = _keyboard[Key.KeypadMinus];
      _currentState[(int)Key.KeypadMultiply] = _keyboard[Key.KeypadMultiply];
      _currentState[(int)Key.KeypadPlus] = _keyboard[Key.KeypadPlus];
      _currentState[(int)Key.KeypadSubtract] = _keyboard[Key.KeypadSubtract];
      _currentState[(int)Key.KeypadAdd] = _keyboard[Key.KeypadAdd];
      _currentState[(int)Key.KeypadDecimal] = _keyboard[Key.KeypadDecimal];
      _currentState[(int)Key.KeypadDivide] = _keyboard[Key.KeypadDivide];

      _currentState[(int)Key.F1] = _keyboard[Key.F1];
      _currentState[(int)Key.F2] = _keyboard[Key.F2];
      _currentState[(int)Key.F3] = _keyboard[Key.F3];
      _currentState[(int)Key.F4] = _keyboard[Key.F4];
      _currentState[(int)Key.F5] = _keyboard[Key.F5];
      _currentState[(int)Key.F6] = _keyboard[Key.F6];
      _currentState[(int)Key.F7] = _keyboard[Key.F7];
      _currentState[(int)Key.F8] = _keyboard[Key.F8];
      _currentState[(int)Key.F9] = _keyboard[Key.F9];
      _currentState[(int)Key.F10] = _keyboard[Key.F10];
      _currentState[(int)Key.F11] = _keyboard[Key.F11];
      _currentState[(int)Key.F12] = _keyboard[Key.F12];

      _currentState[(int)Key.Down] = _keyboard[Key.Down];
      _currentState[(int)Key.Up] = _keyboard[Key.Up];
      _currentState[(int)Key.Left] = _keyboard[Key.Left];
      _currentState[(int)Key.Right] = _keyboard[Key.Right];
      
      _currentState[(int)Key.Escape] = _keyboard[Key.Escape];
      _currentState[(int)Key.ShiftLeft] = _keyboard[Key.ShiftLeft];
      _currentState[(int)Key.ShiftRight] = _keyboard[Key.ShiftRight];
      _currentState[(int)Key.Space] = _keyboard[Key.Space];
      _currentState[(int)Key.Tab] = _keyboard[Key.Tab];
      _currentState[(int)Key.Tilde] = _keyboard[Key.Tilde];
      _currentState[(int)Key.Semicolon] = _keyboard[Key.Semicolon];
      _currentState[(int)Key.PageUp] = _keyboard[Key.PageUp];
      _currentState[(int)Key.Enter] = _keyboard[Key.Enter];
      _currentState[(int)Key.LAlt] = _keyboard[Key.LAlt];
      _currentState[(int)Key.RAlt] = _keyboard[Key.RAlt];
      _currentState[(int)Key.ControlLeft] = _keyboard[Key.ControlLeft];
      _currentState[(int)Key.ControlRight] = _keyboard[Key.ControlRight];
      _currentState[(int)Key.BracketLeft] = _keyboard[Key.BracketLeft];
      _currentState[(int)Key.BracketRight] = _keyboard[Key.BracketRight];
      _currentState[(int)Key.BackSlash] = _keyboard[Key.BackSlash];
      _currentState[(int)Key.Comma] = _keyboard[Key.Comma];
      _currentState[(int)Key.Delete] = _keyboard[Key.Delete];
      _currentState[(int)Key.CapsLock] = _keyboard[Key.CapsLock];
      _currentState[(int)Key.Insert] = _keyboard[Key.Insert];
      _currentState[(int)Key.End] = _keyboard[Key.End];
      _currentState[(int)Key.Home] = _keyboard[Key.Home];
      #endregion
    }

    #region Pressed Properties
    public static bool Apressed { get { return !_previousState[(int)Key.A] && _currentState[(int)Key.A]; } }
    public static bool Bpressed { get { return !_previousState[(int)Key.B] && _currentState[(int)Key.B]; } }
    public static bool Cpressed { get { return !_previousState[(int)Key.C] && _currentState[(int)Key.C]; } }
    public static bool Dpressed { get { return !_previousState[(int)Key.D] && _currentState[(int)Key.D]; } }
    public static bool Epressed { get { return !_previousState[(int)Key.E] && _currentState[(int)Key.E]; } }
    public static bool Fpressed { get { return !_previousState[(int)Key.F] && _currentState[(int)Key.F]; } }
    public static bool Gpressed { get { return !_previousState[(int)Key.G] && _currentState[(int)Key.G]; } }
    public static bool Hpressed { get { return !_previousState[(int)Key.H] && _currentState[(int)Key.H]; } }
    public static bool Ipressed { get { return !_previousState[(int)Key.I] && _currentState[(int)Key.I]; } }
    public static bool Jpressed { get { return !_previousState[(int)Key.J] && _currentState[(int)Key.J]; } }
    public static bool Kpressed { get { return !_previousState[(int)Key.K] && _currentState[(int)Key.K]; } }
    public static bool Lpressed { get { return !_previousState[(int)Key.L] && _currentState[(int)Key.L]; } }
    public static bool Mpressed { get { return !_previousState[(int)Key.M] && _currentState[(int)Key.M]; } }
    public static bool Npressed { get { return !_previousState[(int)Key.N] && _currentState[(int)Key.N]; } }
    public static bool Opressed { get { return !_previousState[(int)Key.O] && _currentState[(int)Key.O]; } }
    public static bool Ppressed { get { return !_previousState[(int)Key.P] && _currentState[(int)Key.P]; } }
    public static bool Qpressed { get { return !_previousState[(int)Key.Q] && _currentState[(int)Key.Q]; } }
    public static bool Rpressed { get { return !_previousState[(int)Key.R] && _currentState[(int)Key.R]; } }
    public static bool Spressed { get { return !_previousState[(int)Key.S] && _currentState[(int)Key.S]; } }
    public static bool Tpressed { get { return !_previousState[(int)Key.T] && _currentState[(int)Key.T]; } }
    public static bool Upressed { get { return !_previousState[(int)Key.U] && _currentState[(int)Key.U]; } }
    public static bool Vpressed { get { return !_previousState[(int)Key.V] && _currentState[(int)Key.V]; } }
    public static bool Wpressed { get { return !_previousState[(int)Key.W] && _currentState[(int)Key.W]; } }
    public static bool Xpressed { get { return !_previousState[(int)Key.X] && _currentState[(int)Key.X]; } }
    public static bool Ypressed { get { return !_previousState[(int)Key.Y] && _currentState[(int)Key.Y]; } }
    public static bool Zpressed { get { return !_previousState[(int)Key.Z] && _currentState[(int)Key.Z]; } }

    public static bool Number0pressed { get { return !_previousState[(int)Key.Number0] && _currentState[(int)Key.Number0]; } }
    public static bool Number1pressed { get { return !_previousState[(int)Key.Number1] && _currentState[(int)Key.Number1]; } }
    public static bool Number2pressed { get { return !_previousState[(int)Key.Number2] && _currentState[(int)Key.Number2]; } }
    public static bool Number3pressed { get { return !_previousState[(int)Key.Number3] && _currentState[(int)Key.Number3]; } }
    public static bool Number4pressed { get { return !_previousState[(int)Key.Number4] && _currentState[(int)Key.Number4]; } }
    public static bool Number5pressed { get { return !_previousState[(int)Key.Number5] && _currentState[(int)Key.Number5]; } }
    public static bool Number6pressed { get { return !_previousState[(int)Key.Number6] && _currentState[(int)Key.Number6]; } }
    public static bool Number7pressed { get { return !_previousState[(int)Key.Number7] && _currentState[(int)Key.Number7]; } }
    public static bool Number8pressed { get { return !_previousState[(int)Key.Number8] && _currentState[(int)Key.Number8]; } }
    public static bool Number9pressed { get { return !_previousState[(int)Key.Number9] && _currentState[(int)Key.Number9]; } }

    public static bool Keypad0pressed { get { return !_previousState[(int)Key.Keypad0] && _currentState[(int)Key.Keypad0]; } }
    public static bool Keypad1pressed { get { return !_previousState[(int)Key.Keypad1] && _currentState[(int)Key.Keypad1]; } }
    public static bool Keypad2pressed { get { return !_previousState[(int)Key.Keypad2] && _currentState[(int)Key.Keypad2]; } }
    public static bool Keypad3pressed { get { return !_previousState[(int)Key.Keypad3] && _currentState[(int)Key.Keypad3]; } }
    public static bool Keypad4pressed { get { return !_previousState[(int)Key.Keypad4] && _currentState[(int)Key.Keypad4]; } }
    public static bool Keypad5pressed { get { return !_previousState[(int)Key.Keypad5] && _currentState[(int)Key.Keypad5]; } }
    public static bool Keypad6pressed { get { return !_previousState[(int)Key.Keypad6] && _currentState[(int)Key.Keypad6]; } }
    public static bool Keypad7pressed { get { return !_previousState[(int)Key.Keypad7] && _currentState[(int)Key.Keypad7]; } }
    public static bool Keypad8pressed { get { return !_previousState[(int)Key.Keypad8] && _currentState[(int)Key.Keypad8]; } }
    public static bool Keypad9pressed { get { return !_previousState[(int)Key.Keypad9] && _currentState[(int)Key.Keypad9]; } }
    public static bool KeypadEnterpressed { get { return !_previousState[(int)Key.KeypadEnter] && _currentState[(int)Key.KeypadEnter]; } }
    public static bool KeypadMinuspressed { get { return !_previousState[(int)Key.KeypadMinus] && _currentState[(int)Key.KeypadMinus]; } }
    public static bool KeypadMultiplypressed { get { return !_previousState[(int)Key.KeypadMultiply] && _currentState[(int)Key.KeypadMultiply]; } }
    public static bool KeypadPluspressed { get { return !_previousState[(int)Key.KeypadPlus] && _currentState[(int)Key.KeypadPlus]; } }
    public static bool KeypadSubtractpressed { get { return !_previousState[(int)Key.KeypadSubtract] && _currentState[(int)Key.KeypadSubtract]; } }
    public static bool KeypadAddpressed { get { return !_previousState[(int)Key.KeypadAdd] && _currentState[(int)Key.KeypadAdd]; } }
    public static bool KeypadDecimalpressed { get { return !_previousState[(int)Key.KeypadDecimal] && _currentState[(int)Key.KeypadDecimal]; } }
    public static bool KeypadDividepressed { get { return !_previousState[(int)Key.KeypadDivide] && _currentState[(int)Key.KeypadDivide]; } }

    public static bool F1pressed { get { return !_previousState[(int)Key.F1] && _currentState[(int)Key.F1]; } }
    public static bool F2pressed { get { return !_previousState[(int)Key.F2] && _currentState[(int)Key.F2]; } }
    public static bool F3pressed { get { return !_previousState[(int)Key.F3] && _currentState[(int)Key.F3]; } }
    public static bool F4pressed { get { return !_previousState[(int)Key.F4] && _currentState[(int)Key.F4]; } }
    public static bool F5pressed { get { return !_previousState[(int)Key.F5] && _currentState[(int)Key.F5]; } }
    public static bool F6pressed { get { return !_previousState[(int)Key.F6] && _currentState[(int)Key.F6]; } }
    public static bool F7pressed { get { return !_previousState[(int)Key.F7] && _currentState[(int)Key.F7]; } }
    public static bool F8pressed { get { return !_previousState[(int)Key.F8] && _currentState[(int)Key.F8]; } }
    public static bool F9pressed { get { return !_previousState[(int)Key.F9] && _currentState[(int)Key.F9]; } }
    public static bool F10pressed { get { return !_previousState[(int)Key.F10] && _currentState[(int)Key.F10]; } }
    public static bool F11pressed { get { return !_previousState[(int)Key.F11] && _currentState[(int)Key.F11]; } }
    public static bool F12pressed { get { return !_previousState[(int)Key.F12] && _currentState[(int)Key.F12]; } }

    public static bool Downpressed { get { return !_previousState[(int)Key.Down] && _currentState[(int)Key.Down]; } }
    public static bool Uppressed { get { return !_previousState[(int)Key.Up] && _currentState[(int)Key.Up]; } }
    public static bool Leftpressed { get { return !_previousState[(int)Key.Left] && _currentState[(int)Key.Left]; } }
    public static bool Rightpressed { get { return !_previousState[(int)Key.Right] && _currentState[(int)Key.Right]; } }

    public static bool Escapepressed { get { return !_previousState[(int)Key.Escape] && _currentState[(int)Key.Escape]; } }
    public static bool ShiftLeftpressed { get { return !_previousState[(int)Key.ShiftLeft] && _currentState[(int)Key.ShiftLeft]; } }
    public static bool ShiftRightpressed { get { return !_previousState[(int)Key.ShiftRight] && _currentState[(int)Key.ShiftRight]; } }
    public static bool Spacepressed { get { return !_previousState[(int)Key.Space] && _currentState[(int)Key.Space]; } }
    public static bool Tabpressed { get { return !_previousState[(int)Key.Tab] && _currentState[(int)Key.Tab]; } }
    public static bool Tildepressed { get { return !_previousState[(int)Key.Tilde] && _currentState[(int)Key.Tilde]; } }
    public static bool Semicolonpressed { get { return !_previousState[(int)Key.Semicolon] && _currentState[(int)Key.Semicolon]; } }
    public static bool PageUppressed { get { return !_previousState[(int)Key.PageUp] && _currentState[(int)Key.PageUp]; } }
    public static bool Enterpressed { get { return !_previousState[(int)Key.Enter] && _currentState[(int)Key.Enter]; } }
    public static bool AltLeftpressed { get { return !_previousState[(int)Key.LAlt] && _currentState[(int)Key.LAlt]; } }
    public static bool AltRightpressed { get { return !_previousState[(int)Key.RAlt] && _currentState[(int)Key.RAlt]; } }
    public static bool ControlLeftpressed { get { return !_previousState[(int)Key.ControlLeft] && _currentState[(int)Key.ControlLeft]; } }
    public static bool ControlRightpressed { get { return !_previousState[(int)Key.ControlRight] && _currentState[(int)Key.ControlRight]; } }
    public static bool BracketLeftpressed { get { return !_previousState[(int)Key.BracketLeft] && _currentState[(int)Key.BracketLeft]; } }
    public static bool BracketRightpressed { get { return !_previousState[(int)Key.BracketRight] && _currentState[(int)Key.BracketRight]; } }
    public static bool BackSlashpressed { get { return !_previousState[(int)Key.BackSlash] && _currentState[(int)Key.BackSlash]; } }
    public static bool Commapressed { get { return !_previousState[(int)Key.Comma] && _currentState[(int)Key.Comma]; } }
    public static bool Deletepressed { get { return !_previousState[(int)Key.Delete] && _currentState[(int)Key.Delete]; } }
    public static bool CapsLockpressed { get { return !_previousState[(int)Key.CapsLock] && _currentState[(int)Key.CapsLock]; } }
    public static bool Insertpressed { get { return !_previousState[(int)Key.Insert] && _currentState[(int)Key.Insert]; } }
    public static bool Endpressed { get { return !_previousState[(int)Key.End] && _currentState[(int)Key.End]; } }
    public static bool Homepressed { get { return !_previousState[(int)Key.Home] && _currentState[(int)Key.Home]; } }
    #endregion

    #region Down Properties
    public static bool Adown { get { return _currentState[(int)Key.A]; } }
    public static bool Bdown { get { return _currentState[(int)Key.B]; } }
    public static bool Cdown { get { return _currentState[(int)Key.C]; } }
    public static bool Ddown { get { return _currentState[(int)Key.D]; } }
    public static bool Edown { get { return _currentState[(int)Key.E]; } }
    public static bool Fdown { get { return _currentState[(int)Key.F]; } }
    public static bool Gdown { get { return _currentState[(int)Key.G]; } }
    public static bool Hdown { get { return _currentState[(int)Key.H]; } }
    public static bool Idown { get { return _currentState[(int)Key.I]; } }
    public static bool Jdown { get { return _currentState[(int)Key.J]; } }
    public static bool Kdown { get { return _currentState[(int)Key.K]; } }
    public static bool Ldown { get { return _currentState[(int)Key.L]; } }
    public static bool Mdown { get { return _currentState[(int)Key.M]; } }
    public static bool Ndown { get { return _currentState[(int)Key.N]; } }
    public static bool Odown { get { return _currentState[(int)Key.O]; } }
    public static bool Pdown { get { return _currentState[(int)Key.P]; } }
    public static bool Qdown { get { return _currentState[(int)Key.Q]; } }
    public static bool Rdown { get { return _currentState[(int)Key.R]; } }
    public static bool Sdown { get { return _currentState[(int)Key.S]; } }
    public static bool Tdown { get { return _currentState[(int)Key.T]; } }
    public static bool Udown { get { return _currentState[(int)Key.U]; } }
    public static bool Vdown { get { return _currentState[(int)Key.V]; } }
    public static bool Wdown { get { return _currentState[(int)Key.W]; } }
    public static bool Xdown { get { return _currentState[(int)Key.X]; } }
    public static bool Ydown { get { return _currentState[(int)Key.Y]; } }
    public static bool Zdown { get { return _currentState[(int)Key.Z]; } }

    public static bool Number0down { get { return _currentState[(int)Key.Number0]; } }
    public static bool Number1down { get { return _currentState[(int)Key.Number1]; } }
    public static bool Number2down { get { return _currentState[(int)Key.Number2]; } }
    public static bool Number3down { get { return _currentState[(int)Key.Number3]; } }
    public static bool Number4down { get { return _currentState[(int)Key.Number4]; } }
    public static bool Number5down { get { return _currentState[(int)Key.Number5]; } }
    public static bool Number6down { get { return _currentState[(int)Key.Number6]; } }
    public static bool Number7down { get { return _currentState[(int)Key.Number7]; } }
    public static bool Number8down { get { return _currentState[(int)Key.Number8]; } }
    public static bool Number9down { get { return _currentState[(int)Key.Number9]; } }

    public static bool Keypad0down { get { return _currentState[(int)Key.Keypad0]; } }
    public static bool Keypad1down { get { return _currentState[(int)Key.Keypad1]; } }
    public static bool Keypad2down { get { return _currentState[(int)Key.Keypad2]; } }
    public static bool Keypad3down { get { return _currentState[(int)Key.Keypad3]; } }
    public static bool Keypad4down { get { return _currentState[(int)Key.Keypad4]; } }
    public static bool Keypad5down { get { return _currentState[(int)Key.Keypad5]; } }
    public static bool Keypad6down { get { return _currentState[(int)Key.Keypad6]; } }
    public static bool Keypad7down { get { return _currentState[(int)Key.Keypad7]; } }
    public static bool Keypad8down { get { return _currentState[(int)Key.Keypad8]; } }
    public static bool Keypad9down { get { return _currentState[(int)Key.Keypad9]; } }
    public static bool KeypadEnterdown { get { return _currentState[(int)Key.KeypadEnter]; } }
    public static bool KeypadMinusdown { get { return _currentState[(int)Key.KeypadMinus]; } }
    public static bool KeypadMultiplydown { get { return _currentState[(int)Key.KeypadMultiply]; } }
    public static bool KeypadPlusdown { get { return _currentState[(int)Key.KeypadPlus]; } }
    public static bool KeypadSubtractdown { get { return _currentState[(int)Key.KeypadSubtract]; } }
    public static bool KeypadAdddown { get { return _currentState[(int)Key.KeypadAdd]; } }
    public static bool KeypadDecimaldown { get { return _currentState[(int)Key.KeypadDecimal]; } }
    public static bool KeypadDividedown { get { return _currentState[(int)Key.KeypadDivide]; } }

    public static bool F1down { get { return _currentState[(int)Key.F1]; } }
    public static bool F2down { get { return _currentState[(int)Key.F2]; } }
    public static bool F3down { get { return _currentState[(int)Key.F3]; } }
    public static bool F4down { get { return _currentState[(int)Key.F4]; } }
    public static bool F5down { get { return _currentState[(int)Key.F5]; } }
    public static bool F6down { get { return _currentState[(int)Key.F6]; } }
    public static bool F7down { get { return _currentState[(int)Key.F7]; } }
    public static bool F8down { get { return _currentState[(int)Key.F8]; } }
    public static bool F9down { get { return _currentState[(int)Key.F9]; } }
    public static bool F10down { get { return _currentState[(int)Key.F10]; } }
    public static bool F11down { get { return _currentState[(int)Key.F11]; } }
    public static bool F12down { get { return _currentState[(int)Key.F12]; } }

    public static bool Downdown { get { return _currentState[(int)Key.Down]; } }
    public static bool Updown { get { return _currentState[(int)Key.Up]; } }
    public static bool Leftdown { get { return _currentState[(int)Key.Left]; } }
    public static bool Rightdown { get { return _currentState[(int)Key.Right]; } }

    public static bool Escapedown { get { return _currentState[(int)Key.Escape]; } }
    public static bool ShiftLeftdown { get { return _currentState[(int)Key.ShiftLeft]; } }
    public static bool ShiftRightdown { get { return _currentState[(int)Key.ShiftRight]; } }
    public static bool Spacedown { get { return _currentState[(int)Key.Space]; } }
    public static bool Tabdown { get { return _currentState[(int)Key.Tab]; } }
    public static bool Tildedown { get { return _currentState[(int)Key.Tilde]; } }
    public static bool Semicolondown { get { return _currentState[(int)Key.Semicolon]; } }
    public static bool PageUpdown { get { return _currentState[(int)Key.PageUp]; } }
    public static bool Enterdown { get { return _currentState[(int)Key.Enter]; } }
    public static bool AltLeftdown { get { return _currentState[(int)Key.LAlt]; } }
    public static bool AltRightdown { get { return _currentState[(int)Key.RAlt]; } }
    public static bool ControlLeftdown { get { return _currentState[(int)Key.ControlLeft]; } }
    public static bool ControlRightdown { get { return _currentState[(int)Key.ControlRight]; } }
    public static bool BracketLeftdown { get { return _currentState[(int)Key.BracketLeft]; } }
    public static bool BracketRightdown { get { return _currentState[(int)Key.BracketRight]; } }
    public static bool BackSlashdown { get { return _currentState[(int)Key.BackSlash]; } }
    public static bool Commadown { get { return _currentState[(int)Key.Comma]; } }
    public static bool Deletedown { get { return _currentState[(int)Key.Delete]; } }
    public static bool CapsLockdown { get { return _currentState[(int)Key.CapsLock]; } }
    public static bool Insertdown { get { return _currentState[(int)Key.Insert]; } }
    public static bool Enddown { get { return _currentState[(int)Key.End]; } }
    public static bool Homedown { get { return _currentState[(int)Key.Home]; } }
    #endregion

    #region Up Properties
    public static bool Aup { get { return !_currentState[(int)Key.A]; } }
    public static bool Bup { get { return !_currentState[(int)Key.B]; } }
    public static bool Cup { get { return !_currentState[(int)Key.C]; } }
    public static bool Dup { get { return !_currentState[(int)Key.D]; } }
    public static bool Eup { get { return !_currentState[(int)Key.E]; } }
    public static bool Fup { get { return !_currentState[(int)Key.F]; } }
    public static bool Gup { get { return !_currentState[(int)Key.G]; } }
    public static bool Hup { get { return !_currentState[(int)Key.H]; } }
    public static bool Iup { get { return !_currentState[(int)Key.I]; } }
    public static bool Jup { get { return !_currentState[(int)Key.J]; } }
    public static bool Kup { get { return !_currentState[(int)Key.K]; } }
    public static bool Lup { get { return !_currentState[(int)Key.L]; } }
    public static bool Mup { get { return !_currentState[(int)Key.M]; } }
    public static bool Nup { get { return !_currentState[(int)Key.N]; } }
    public static bool Oup { get { return !_currentState[(int)Key.O]; } }
    public static bool Pup { get { return !_currentState[(int)Key.P]; } }
    public static bool Qup { get { return !_currentState[(int)Key.Q]; } }
    public static bool Rup { get { return !_currentState[(int)Key.R]; } }
    public static bool Sup { get { return !_currentState[(int)Key.S]; } }
    public static bool Tup { get { return !_currentState[(int)Key.T]; } }
    public static bool Uup { get { return !_currentState[(int)Key.U]; } }
    public static bool Vup { get { return !_currentState[(int)Key.V]; } }
    public static bool Wup { get { return !_currentState[(int)Key.W]; } }
    public static bool Xup { get { return !_currentState[(int)Key.X]; } }
    public static bool Yup { get { return !_currentState[(int)Key.Y]; } }
    public static bool Zup { get { return !_currentState[(int)Key.Z]; } }

    public static bool Number0up { get { return !_currentState[(int)Key.Number0]; } }
    public static bool Number1up { get { return !_currentState[(int)Key.Number1]; } }
    public static bool Number2up { get { return !_currentState[(int)Key.Number2]; } }
    public static bool Number3up { get { return !_currentState[(int)Key.Number3]; } }
    public static bool Number4up { get { return !_currentState[(int)Key.Number4]; } }
    public static bool Number5up { get { return !_currentState[(int)Key.Number5]; } }
    public static bool Number6up { get { return !_currentState[(int)Key.Number6]; } }
    public static bool Number7up { get { return !_currentState[(int)Key.Number7]; } }
    public static bool Number8up { get { return !_currentState[(int)Key.Number8]; } }
    public static bool Number9up { get { return !_currentState[(int)Key.Number9]; } }

    public static bool Keypad0up { get { return !_currentState[(int)Key.Keypad0]; } }
    public static bool Keypad1up { get { return !_currentState[(int)Key.Keypad1]; } }
    public static bool Keypad2up { get { return !_currentState[(int)Key.Keypad2]; } }
    public static bool Keypad3up { get { return !_currentState[(int)Key.Keypad3]; } }
    public static bool Keypad4up { get { return !_currentState[(int)Key.Keypad4]; } }
    public static bool Keypad5up { get { return !_currentState[(int)Key.Keypad5]; } }
    public static bool Keypad6up { get { return !_currentState[(int)Key.Keypad6]; } }
    public static bool Keypad7up { get { return !_currentState[(int)Key.Keypad7]; } }
    public static bool Keypad8up { get { return !_currentState[(int)Key.Keypad8]; } }
    public static bool Keypad9up { get { return !_currentState[(int)Key.Keypad9]; } }
    public static bool KeypadEnterup { get { return !_currentState[(int)Key.KeypadEnter]; } }
    public static bool KeypadMinusup { get { return !_currentState[(int)Key.KeypadMinus]; } }
    public static bool KeypadMultiplyup { get { return !_currentState[(int)Key.KeypadMultiply]; } }
    public static bool KeypadPlusup { get { return !_currentState[(int)Key.KeypadPlus]; } }
    public static bool KeypadSubtractup { get { return !_currentState[(int)Key.KeypadSubtract]; } }
    public static bool KeypadAddup { get { return !_currentState[(int)Key.KeypadAdd]; } }
    public static bool KeypadDecimalup { get { return !_currentState[(int)Key.KeypadDecimal]; } }
    public static bool KeypadDivideup { get { return !_currentState[(int)Key.KeypadDivide]; } }

    public static bool F1up { get { return !_currentState[(int)Key.F1]; } }
    public static bool F2up { get { return !_currentState[(int)Key.F2]; } }
    public static bool F3up { get { return !_currentState[(int)Key.F3]; } }
    public static bool F4up { get { return !_currentState[(int)Key.F4]; } }
    public static bool F5up { get { return !_currentState[(int)Key.F5]; } }
    public static bool F6up { get { return !_currentState[(int)Key.F6]; } }
    public static bool F7up { get { return !_currentState[(int)Key.F7]; } }
    public static bool F8up { get { return !_currentState[(int)Key.F8]; } }
    public static bool F9up { get { return !_currentState[(int)Key.F9]; } }
    public static bool F10up { get { return !_currentState[(int)Key.F10]; } }
    public static bool F11up { get { return !_currentState[(int)Key.F11]; } }
    public static bool F12up { get { return !_currentState[(int)Key.F12]; } }

    public static bool Downup { get { return !_currentState[(int)Key.Down]; } }
    public static bool Upup { get { return !_currentState[(int)Key.Up]; } }
    public static bool Leftup { get { return !_currentState[(int)Key.Left]; } }
    public static bool Rightup { get { return !_currentState[(int)Key.Right]; } }

    public static bool Escapeup { get { return !_currentState[(int)Key.Escape]; } }
    public static bool ShiftLeftup { get { return !_currentState[(int)Key.ShiftLeft]; } }
    public static bool ShiftRightup { get { return !_currentState[(int)Key.ShiftRight]; } }
    public static bool Spaceup { get { return !_currentState[(int)Key.Space]; } }
    public static bool Tabup { get { return !_currentState[(int)Key.Tab]; } }
    public static bool Tildeup { get { return !_currentState[(int)Key.Tilde]; } }
    public static bool Semicolonup { get { return !_currentState[(int)Key.Semicolon]; } }
    public static bool PageUpup { get { return !_currentState[(int)Key.PageUp]; } }
    public static bool Enterup { get { return !_currentState[(int)Key.Enter]; } }
    public static bool AltLeftup { get { return !_currentState[(int)Key.LAlt]; } }
    public static bool AltRightup { get { return !_currentState[(int)Key.RAlt]; } }
    public static bool ControlLeftup { get { return !_currentState[(int)Key.ControlLeft]; } }
    public static bool ControlRightup { get { return !_currentState[(int)Key.ControlRight]; } }
    public static bool BracketLeftup { get { return !_currentState[(int)Key.BracketLeft]; } }
    public static bool BracketRightup { get { return !_currentState[(int)Key.BracketRight]; } }
    public static bool BackSlashup { get { return !_currentState[(int)Key.BackSlash]; } }
    public static bool Commaup { get { return !_currentState[(int)Key.Comma]; } }
    public static bool Deleteup { get { return !_currentState[(int)Key.Delete]; } }
    public static bool CapsLockup { get { return !_currentState[(int)Key.CapsLock]; } }
    public static bool Insertup { get { return !_currentState[(int)Key.Insert]; } }
    public static bool Endup { get { return !_currentState[(int)Key.End]; } }
    public static bool Homeup { get { return !_currentState[(int)Key.Home]; } }
    #endregion

    # region Released Properties
    public static bool Areleased { get { return _previousState[(int)Key.A] && !_currentState[(int)Key.A]; } }
    public static bool Breleased { get { return _previousState[(int)Key.B] && !_currentState[(int)Key.B]; } }
    public static bool Creleased { get { return _previousState[(int)Key.C] && !_currentState[(int)Key.C]; } }
    public static bool Dreleased { get { return _previousState[(int)Key.D] && !_currentState[(int)Key.D]; } }
    public static bool Ereleased { get { return _previousState[(int)Key.E] && !_currentState[(int)Key.E]; } }
    public static bool Freleased { get { return _previousState[(int)Key.F] && !_currentState[(int)Key.F]; } }
    public static bool Greleased { get { return _previousState[(int)Key.G] && !_currentState[(int)Key.G]; } }
    public static bool Hreleased { get { return _previousState[(int)Key.H] && !_currentState[(int)Key.H]; } }
    public static bool Ireleased { get { return _previousState[(int)Key.I] && !_currentState[(int)Key.I]; } }
    public static bool Jreleased { get { return _previousState[(int)Key.J] && !_currentState[(int)Key.J]; } }
    public static bool Kreleased { get { return _previousState[(int)Key.K] && !_currentState[(int)Key.K]; } }
    public static bool Lreleased { get { return _previousState[(int)Key.L] && !_currentState[(int)Key.L]; } }
    public static bool Mreleased { get { return _previousState[(int)Key.M] && !_currentState[(int)Key.M]; } }
    public static bool Nreleased { get { return _previousState[(int)Key.N] && !_currentState[(int)Key.N]; } }
    public static bool Oreleased { get { return _previousState[(int)Key.O] && !_currentState[(int)Key.O]; } }
    public static bool Preleased { get { return _previousState[(int)Key.P] && !_currentState[(int)Key.P]; } }
    public static bool Qreleased { get { return _previousState[(int)Key.Q] && !_currentState[(int)Key.Q]; } }
    public static bool Rreleased { get { return _previousState[(int)Key.R] && !_currentState[(int)Key.R]; } }
    public static bool Sreleased { get { return _previousState[(int)Key.S] && !_currentState[(int)Key.S]; } }
    public static bool Treleased { get { return _previousState[(int)Key.T] && !_currentState[(int)Key.T]; } }
    public static bool Ureleased { get { return _previousState[(int)Key.U] && !_currentState[(int)Key.U]; } }
    public static bool Vreleased { get { return _previousState[(int)Key.V] && !_currentState[(int)Key.V]; } }
    public static bool Wreleased { get { return _previousState[(int)Key.W] && !_currentState[(int)Key.W]; } }
    public static bool Xreleased { get { return _previousState[(int)Key.X] && !_currentState[(int)Key.X]; } }
    public static bool Yreleased { get { return _previousState[(int)Key.Y] && !_currentState[(int)Key.Y]; } }
    public static bool Zreleased { get { return _previousState[(int)Key.Z] && !_currentState[(int)Key.Z]; } }

    public static bool Number0released { get { return _previousState[(int)Key.Number0] && !_currentState[(int)Key.Number0]; } }
    public static bool Number1released { get { return _previousState[(int)Key.Number1] && !_currentState[(int)Key.Number1]; } }
    public static bool Number2released { get { return _previousState[(int)Key.Number2] && !_currentState[(int)Key.Number2]; } }
    public static bool Number3released { get { return _previousState[(int)Key.Number3] && !_currentState[(int)Key.Number3]; } }
    public static bool Number4released { get { return _previousState[(int)Key.Number4] && !_currentState[(int)Key.Number4]; } }
    public static bool Number5released { get { return _previousState[(int)Key.Number5] && !_currentState[(int)Key.Number5]; } }
    public static bool Number6released { get { return _previousState[(int)Key.Number6] && !_currentState[(int)Key.Number6]; } }
    public static bool Number7released { get { return _previousState[(int)Key.Number7] && !_currentState[(int)Key.Number7]; } }
    public static bool Number8released { get { return _previousState[(int)Key.Number8] && !_currentState[(int)Key.Number8]; } }
    public static bool Number9released { get { return _previousState[(int)Key.Number9] && !_currentState[(int)Key.Number9]; } }

    public static bool Keypad0released { get { return _previousState[(int)Key.Keypad0] && !_currentState[(int)Key.Keypad0]; } }
    public static bool Keypad1released { get { return _previousState[(int)Key.Keypad1] && !_currentState[(int)Key.Keypad1]; } }
    public static bool Keypad2released { get { return _previousState[(int)Key.Keypad2] && !_currentState[(int)Key.Keypad2]; } }
    public static bool Keypad3released { get { return _previousState[(int)Key.Keypad3] && !_currentState[(int)Key.Keypad3]; } }
    public static bool Keypad4released { get { return _previousState[(int)Key.Keypad4] && !_currentState[(int)Key.Keypad4]; } }
    public static bool Keypad5released { get { return _previousState[(int)Key.Keypad5] && !_currentState[(int)Key.Keypad5]; } }
    public static bool Keypad6released { get { return _previousState[(int)Key.Keypad6] && !_currentState[(int)Key.Keypad6]; } }
    public static bool Keypad7released { get { return _previousState[(int)Key.Keypad7] && !_currentState[(int)Key.Keypad7]; } }
    public static bool Keypad8released { get { return _previousState[(int)Key.Keypad8] && !_currentState[(int)Key.Keypad8]; } }
    public static bool Keypad9released { get { return _previousState[(int)Key.Keypad9] && !_currentState[(int)Key.Keypad9]; } }
    public static bool KeypadEnterreleased { get { return _previousState[(int)Key.KeypadEnter] && !_currentState[(int)Key.KeypadEnter]; } }
    public static bool KeypadMinusreleased { get { return _previousState[(int)Key.KeypadMinus] && !_currentState[(int)Key.KeypadMinus]; } }
    public static bool KeypadMultiplyreleased { get { return _previousState[(int)Key.KeypadMultiply] && !_currentState[(int)Key.KeypadMultiply]; } }
    public static bool KeypadPlusreleased { get { return _previousState[(int)Key.KeypadPlus] && !_currentState[(int)Key.KeypadPlus]; } }
    public static bool KeypadSubtractreleased { get { return _previousState[(int)Key.KeypadSubtract] && !_currentState[(int)Key.KeypadSubtract]; } }
    public static bool KeypadAddreleased { get { return _previousState[(int)Key.KeypadAdd] && !_currentState[(int)Key.KeypadAdd]; } }
    public static bool KeypadDecimalreleased { get { return _previousState[(int)Key.KeypadDecimal] && !_currentState[(int)Key.KeypadDecimal]; } }
    public static bool KeypadDividereleased { get { return _previousState[(int)Key.KeypadDivide] && !_currentState[(int)Key.KeypadDivide]; } }

    public static bool F1released { get { return _previousState[(int)Key.F1] && !_currentState[(int)Key.F1]; } }
    public static bool F2released { get { return _previousState[(int)Key.F2] && !_currentState[(int)Key.F2]; } }
    public static bool F3released { get { return _previousState[(int)Key.F3] && !_currentState[(int)Key.F3]; } }
    public static bool F4released { get { return _previousState[(int)Key.F4] && !_currentState[(int)Key.F4]; } }
    public static bool F5released { get { return _previousState[(int)Key.F5] && !_currentState[(int)Key.F5]; } }
    public static bool F6released { get { return _previousState[(int)Key.F6] && !_currentState[(int)Key.F6]; } }
    public static bool F7released { get { return _previousState[(int)Key.F7] && !_currentState[(int)Key.F7]; } }
    public static bool F8released { get { return _previousState[(int)Key.F8] && !_currentState[(int)Key.F8]; } }
    public static bool F9released { get { return _previousState[(int)Key.F9] && !_currentState[(int)Key.F9]; } }
    public static bool F10released { get { return _previousState[(int)Key.F10] && !_currentState[(int)Key.F10]; } }
    public static bool F11released { get { return _previousState[(int)Key.F11] && !_currentState[(int)Key.F11]; } }
    public static bool F12released { get { return _previousState[(int)Key.F12] && !_currentState[(int)Key.F12]; } }

    public static bool Downreleased { get { return _previousState[(int)Key.Down] && !_currentState[(int)Key.Down]; } }
    public static bool Upreleased { get { return _previousState[(int)Key.Up] && !_currentState[(int)Key.Up]; } }
    public static bool Leftreleased { get { return _previousState[(int)Key.Left] && !_currentState[(int)Key.Left]; } }
    public static bool Rightreleased { get { return _previousState[(int)Key.Right] && !_currentState[(int)Key.Right]; } }

    public static bool Escapereleased { get { return _previousState[(int)Key.Escape] && !_currentState[(int)Key.Escape]; } }
    public static bool ShiftLeftreleased { get { return _previousState[(int)Key.ShiftLeft] && !_currentState[(int)Key.ShiftLeft]; } }
    public static bool ShiftRightreleased { get { return _previousState[(int)Key.ShiftRight] && !_currentState[(int)Key.ShiftRight]; } }
    public static bool Spacereleased { get { return _previousState[(int)Key.Space] && !_currentState[(int)Key.Space]; } }
    public static bool Tabreleased { get { return _previousState[(int)Key.Tab] && !_currentState[(int)Key.Tab]; } }
    public static bool Tildereleased { get { return _previousState[(int)Key.Tilde] && !_currentState[(int)Key.Tilde]; } }
    public static bool Semicolonreleased { get { return _previousState[(int)Key.Semicolon] && !_currentState[(int)Key.Semicolon]; } }
    public static bool PageUpreleased { get { return _previousState[(int)Key.PageUp] && !_currentState[(int)Key.PageUp]; } }
    public static bool Enterreleased { get { return _previousState[(int)Key.Enter] && !_currentState[(int)Key.Enter]; } }
    public static bool AltLeftreleased { get { return _previousState[(int)Key.LAlt] && !_currentState[(int)Key.LAlt]; } }
    public static bool AltRightreleased { get { return _previousState[(int)Key.RAlt] && !_currentState[(int)Key.RAlt]; } }
    public static bool ControlLeftreleased { get { return _previousState[(int)Key.ControlLeft] && !_currentState[(int)Key.ControlLeft]; } }
    public static bool ControlRightreleased { get { return _previousState[(int)Key.ControlRight] && !_currentState[(int)Key.ControlRight]; } }
    public static bool BracketLeftreleased { get { return _previousState[(int)Key.BracketLeft] && !_currentState[(int)Key.BracketLeft]; } }
    public static bool BracketRightreleased { get { return _previousState[(int)Key.BracketRight] && !_currentState[(int)Key.BracketRight]; } }
    public static bool BackSlashreleased { get { return _previousState[(int)Key.BackSlash] && !_currentState[(int)Key.BackSlash]; } }
    public static bool Commareleased { get { return _previousState[(int)Key.Comma] && !_currentState[(int)Key.Comma]; } }
    public static bool Deletereleased { get { return _previousState[(int)Key.Delete] && !_currentState[(int)Key.Delete]; } }
    public static bool CapsLockreleased { get { return _previousState[(int)Key.CapsLock] && !_currentState[(int)Key.CapsLock]; } }
    public static bool Insertreleased { get { return _previousState[(int)Key.Insert] && !_currentState[(int)Key.Insert]; } }
    public static bool Endreleased { get { return _previousState[(int)Key.End] && !_currentState[(int)Key.End]; } }
    public static bool Homereleased { get { return _previousState[(int)Key.Home] && !_currentState[(int)Key.Home]; } }
    #endregion
  }
}