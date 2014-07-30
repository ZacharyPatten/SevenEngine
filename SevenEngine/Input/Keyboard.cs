// Seven
// https://github.com/53V3N1X/SevenEngine
// LISCENSE: See "LISCENSE.txt" in th root project directory.
// SUPPORT: See "README.txt" in the root project directory.

using OpenTK.Input;

namespace SevenEngine.Input
{
  /// <summary>Manages keyboard input using a state system consisting of two states.</summary>
  public class Keyboard
  {
    // The number of keys supported by OpenTK
    private const int _numberOfKeyboardKeys = 144;

    // The state storages
    private bool[] _stateOne;
    private bool[] _stateTwo;

    // Reference to the states indicating past/present
    private bool[] _currentState;
    private bool[] _previousState;

    // Reference to OpenTK's keyboard input
    private KeyboardDevice _keyboard;

    /// <summary>This initializes the reference to OpenTK's keyboard.</summary>
    /// <param name="keyboardDevice">Reference to OpenTK's KeyboardDevice class within their GameWindow class.</param>
    public Keyboard(KeyboardDevice keyboardDevice)
    {
      _keyboard = keyboardDevice;
      _stateOne = new bool[_numberOfKeyboardKeys];
      _stateTwo = new bool[_numberOfKeyboardKeys];
      _currentState = _stateOne;
      _previousState = _stateTwo;
    }

    /// <summary>Update the input states.</summary>
    public void Update()
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
    public bool Apressed { get { return !_previousState[(int)Key.A] && _currentState[(int)Key.A]; } }
    public bool Bpressed { get { return !_previousState[(int)Key.B] && _currentState[(int)Key.B]; } }
    public bool Cpressed { get { return !_previousState[(int)Key.C] && _currentState[(int)Key.C]; } }
    public bool Dpressed { get { return !_previousState[(int)Key.D] && _currentState[(int)Key.D]; } }
    public bool Epressed { get { return !_previousState[(int)Key.E] && _currentState[(int)Key.E]; } }
    public bool Fpressed { get { return !_previousState[(int)Key.F] && _currentState[(int)Key.F]; } }
    public bool Gpressed { get { return !_previousState[(int)Key.G] && _currentState[(int)Key.G]; } }
    public bool Hpressed { get { return !_previousState[(int)Key.H] && _currentState[(int)Key.H]; } }
    public bool Ipressed { get { return !_previousState[(int)Key.I] && _currentState[(int)Key.I]; } }
    public bool Jpressed { get { return !_previousState[(int)Key.J] && _currentState[(int)Key.J]; } }
    public bool Kpressed { get { return !_previousState[(int)Key.K] && _currentState[(int)Key.K]; } }
    public bool Lpressed { get { return !_previousState[(int)Key.L] && _currentState[(int)Key.L]; } }
    public bool Mpressed { get { return !_previousState[(int)Key.M] && _currentState[(int)Key.M]; } }
    public bool Npressed { get { return !_previousState[(int)Key.N] && _currentState[(int)Key.N]; } }
    public bool Opressed { get { return !_previousState[(int)Key.O] && _currentState[(int)Key.O]; } }
    public bool Ppressed { get { return !_previousState[(int)Key.P] && _currentState[(int)Key.P]; } }
    public bool Qpressed { get { return !_previousState[(int)Key.Q] && _currentState[(int)Key.Q]; } }
    public bool Rpressed { get { return !_previousState[(int)Key.R] && _currentState[(int)Key.R]; } }
    public bool Spressed { get { return !_previousState[(int)Key.S] && _currentState[(int)Key.S]; } }
    public bool Tpressed { get { return !_previousState[(int)Key.T] && _currentState[(int)Key.T]; } }
    public bool Upressed { get { return !_previousState[(int)Key.U] && _currentState[(int)Key.U]; } }
    public bool Vpressed { get { return !_previousState[(int)Key.V] && _currentState[(int)Key.V]; } }
    public bool Wpressed { get { return !_previousState[(int)Key.W] && _currentState[(int)Key.W]; } }
    public bool Xpressed { get { return !_previousState[(int)Key.X] && _currentState[(int)Key.X]; } }
    public bool Ypressed { get { return !_previousState[(int)Key.Y] && _currentState[(int)Key.Y]; } }
    public bool Zpressed { get { return !_previousState[(int)Key.Z] && _currentState[(int)Key.Z]; } }

    public bool Number0pressed { get { return !_previousState[(int)Key.Number0] && _currentState[(int)Key.Number0]; } }
    public bool Number1pressed { get { return !_previousState[(int)Key.Number1] && _currentState[(int)Key.Number1]; } }
    public bool Number2pressed { get { return !_previousState[(int)Key.Number2] && _currentState[(int)Key.Number2]; } }
    public bool Number3pressed { get { return !_previousState[(int)Key.Number3] && _currentState[(int)Key.Number3]; } }
    public bool Number4pressed { get { return !_previousState[(int)Key.Number4] && _currentState[(int)Key.Number4]; } }
    public bool Number5pressed { get { return !_previousState[(int)Key.Number5] && _currentState[(int)Key.Number5]; } }
    public bool Number6pressed { get { return !_previousState[(int)Key.Number6] && _currentState[(int)Key.Number6]; } }
    public bool Number7pressed { get { return !_previousState[(int)Key.Number7] && _currentState[(int)Key.Number7]; } }
    public bool Number8pressed { get { return !_previousState[(int)Key.Number8] && _currentState[(int)Key.Number8]; } }
    public bool Number9pressed { get { return !_previousState[(int)Key.Number9] && _currentState[(int)Key.Number9]; } }

    public bool Keypad0pressed { get { return !_previousState[(int)Key.Keypad0] && _currentState[(int)Key.Keypad0]; } }
    public bool Keypad1pressed { get { return !_previousState[(int)Key.Keypad1] && _currentState[(int)Key.Keypad1]; } }
    public bool Keypad2pressed { get { return !_previousState[(int)Key.Keypad2] && _currentState[(int)Key.Keypad2]; } }
    public bool Keypad3pressed { get { return !_previousState[(int)Key.Keypad3] && _currentState[(int)Key.Keypad3]; } }
    public bool Keypad4pressed { get { return !_previousState[(int)Key.Keypad4] && _currentState[(int)Key.Keypad4]; } }
    public bool Keypad5pressed { get { return !_previousState[(int)Key.Keypad5] && _currentState[(int)Key.Keypad5]; } }
    public bool Keypad6pressed { get { return !_previousState[(int)Key.Keypad6] && _currentState[(int)Key.Keypad6]; } }
    public bool Keypad7pressed { get { return !_previousState[(int)Key.Keypad7] && _currentState[(int)Key.Keypad7]; } }
    public bool Keypad8pressed { get { return !_previousState[(int)Key.Keypad8] && _currentState[(int)Key.Keypad8]; } }
    public bool Keypad9pressed { get { return !_previousState[(int)Key.Keypad9] && _currentState[(int)Key.Keypad9]; } }
    public bool KeypadEnterpressed { get { return !_previousState[(int)Key.KeypadEnter] && _currentState[(int)Key.KeypadEnter]; } }
    public bool KeypadMinuspressed { get { return !_previousState[(int)Key.KeypadMinus] && _currentState[(int)Key.KeypadMinus]; } }
    public bool KeypadMultiplypressed { get { return !_previousState[(int)Key.KeypadMultiply] && _currentState[(int)Key.KeypadMultiply]; } }
    public bool KeypadPluspressed { get { return !_previousState[(int)Key.KeypadPlus] && _currentState[(int)Key.KeypadPlus]; } }
    public bool KeypadSubtractpressed { get { return !_previousState[(int)Key.KeypadSubtract] && _currentState[(int)Key.KeypadSubtract]; } }
    public bool KeypadAddpressed { get { return !_previousState[(int)Key.KeypadAdd] && _currentState[(int)Key.KeypadAdd]; } }
    public bool KeypadDecimalpressed { get { return !_previousState[(int)Key.KeypadDecimal] && _currentState[(int)Key.KeypadDecimal]; } }
    public bool KeypadDividepressed { get { return !_previousState[(int)Key.KeypadDivide] && _currentState[(int)Key.KeypadDivide]; } }

    public bool F1pressed { get { return !_previousState[(int)Key.F1] && _currentState[(int)Key.F1]; } }
    public bool F2pressed { get { return !_previousState[(int)Key.F2] && _currentState[(int)Key.F2]; } }
    public bool F3pressed { get { return !_previousState[(int)Key.F3] && _currentState[(int)Key.F3]; } }
    public bool F4pressed { get { return !_previousState[(int)Key.F4] && _currentState[(int)Key.F4]; } }
    public bool F5pressed { get { return !_previousState[(int)Key.F5] && _currentState[(int)Key.F5]; } }
    public bool F6pressed { get { return !_previousState[(int)Key.F6] && _currentState[(int)Key.F6]; } }
    public bool F7pressed { get { return !_previousState[(int)Key.F7] && _currentState[(int)Key.F7]; } }
    public bool F8pressed { get { return !_previousState[(int)Key.F8] && _currentState[(int)Key.F8]; } }
    public bool F9pressed { get { return !_previousState[(int)Key.F9] && _currentState[(int)Key.F9]; } }
    public bool F10pressed { get { return !_previousState[(int)Key.F10] && _currentState[(int)Key.F10]; } }
    public bool F11pressed { get { return !_previousState[(int)Key.F11] && _currentState[(int)Key.F11]; } }
    public bool F12pressed { get { return !_previousState[(int)Key.F12] && _currentState[(int)Key.F12]; } }

    public bool Downpressed { get { return !_previousState[(int)Key.Down] && _currentState[(int)Key.Down]; } }
    public bool Uppressed { get { return !_previousState[(int)Key.Up] && _currentState[(int)Key.Up]; } }
    public bool Leftpressed { get { return !_previousState[(int)Key.Left] && _currentState[(int)Key.Left]; } }
    public bool Rightpressed { get { return !_previousState[(int)Key.Right] && _currentState[(int)Key.Right]; } }

    public bool Escapepressed { get { return !_previousState[(int)Key.Escape] && _currentState[(int)Key.Escape]; } }
    public bool ShiftLeftpressed { get { return !_previousState[(int)Key.ShiftLeft] && _currentState[(int)Key.ShiftLeft]; } }
    public bool ShiftRightpressed { get { return !_previousState[(int)Key.ShiftRight] && _currentState[(int)Key.ShiftRight]; } }
    public bool Spacepressed { get { return !_previousState[(int)Key.Space] && _currentState[(int)Key.Space]; } }
    public bool Tabpressed { get { return !_previousState[(int)Key.Tab] && _currentState[(int)Key.Tab]; } }
    public bool Tildepressed { get { return !_previousState[(int)Key.Tilde] && _currentState[(int)Key.Tilde]; } }
    public bool Semicolonpressed { get { return !_previousState[(int)Key.Semicolon] && _currentState[(int)Key.Semicolon]; } }
    public bool PageUppressed { get { return !_previousState[(int)Key.PageUp] && _currentState[(int)Key.PageUp]; } }
    public bool Enterpressed { get { return !_previousState[(int)Key.Enter] && _currentState[(int)Key.Enter]; } }
    public bool AltLeftpressed { get { return !_previousState[(int)Key.LAlt] && _currentState[(int)Key.LAlt]; } }
    public bool AltRightpressed { get { return !_previousState[(int)Key.RAlt] && _currentState[(int)Key.RAlt]; } }
    public bool ControlLeftpressed { get { return !_previousState[(int)Key.ControlLeft] && _currentState[(int)Key.ControlLeft]; } }
    public bool ControlRightpressed { get { return !_previousState[(int)Key.ControlRight] && _currentState[(int)Key.ControlRight]; } }
    public bool BracketLeftpressed { get { return !_previousState[(int)Key.BracketLeft] && _currentState[(int)Key.BracketLeft]; } }
    public bool BracketRightpressed { get { return !_previousState[(int)Key.BracketRight] && _currentState[(int)Key.BracketRight]; } }
    public bool BackSlashpressed { get { return !_previousState[(int)Key.BackSlash] && _currentState[(int)Key.BackSlash]; } }
    public bool Commapressed { get { return !_previousState[(int)Key.Comma] && _currentState[(int)Key.Comma]; } }
    public bool Deletepressed { get { return !_previousState[(int)Key.Delete] && _currentState[(int)Key.Delete]; } }
    public bool CapsLockpressed { get { return !_previousState[(int)Key.CapsLock] && _currentState[(int)Key.CapsLock]; } }
    public bool Insertpressed { get { return !_previousState[(int)Key.Insert] && _currentState[(int)Key.Insert]; } }
    public bool Endpressed { get { return !_previousState[(int)Key.End] && _currentState[(int)Key.End]; } }
    public bool Homepressed { get { return !_previousState[(int)Key.Home] && _currentState[(int)Key.Home]; } }
    #endregion

    #region Down Properties
    public bool Adown { get { return _currentState[(int)Key.A]; } }
    public bool Bdown { get { return _currentState[(int)Key.B]; } }
    public bool Cdown { get { return _currentState[(int)Key.C]; } }
    public bool Ddown { get { return _currentState[(int)Key.D]; } }
    public bool Edown { get { return _currentState[(int)Key.E]; } }
    public bool Fdown { get { return _currentState[(int)Key.F]; } }
    public bool Gdown { get { return _currentState[(int)Key.G]; } }
    public bool Hdown { get { return _currentState[(int)Key.H]; } }
    public bool Idown { get { return _currentState[(int)Key.I]; } }
    public bool Jdown { get { return _currentState[(int)Key.J]; } }
    public bool Kdown { get { return _currentState[(int)Key.K]; } }
    public bool Ldown { get { return _currentState[(int)Key.L]; } }
    public bool Mdown { get { return _currentState[(int)Key.M]; } }
    public bool Ndown { get { return _currentState[(int)Key.N]; } }
    public bool Odown { get { return _currentState[(int)Key.O]; } }
    public bool Pdown { get { return _currentState[(int)Key.P]; } }
    public bool Qdown { get { return _currentState[(int)Key.Q]; } }
    public bool Rdown { get { return _currentState[(int)Key.R]; } }
    public bool Sdown { get { return _currentState[(int)Key.S]; } }
    public bool Tdown { get { return _currentState[(int)Key.T]; } }
    public bool Udown { get { return _currentState[(int)Key.U]; } }
    public bool Vdown { get { return _currentState[(int)Key.V]; } }
    public bool Wdown { get { return _currentState[(int)Key.W]; } }
    public bool Xdown { get { return _currentState[(int)Key.X]; } }
    public bool Ydown { get { return _currentState[(int)Key.Y]; } }
    public bool Zdown { get { return _currentState[(int)Key.Z]; } }

    public bool Number0down { get { return _currentState[(int)Key.Number0]; } }
    public bool Number1down { get { return _currentState[(int)Key.Number1]; } }
    public bool Number2down { get { return _currentState[(int)Key.Number2]; } }
    public bool Number3down { get { return _currentState[(int)Key.Number3]; } }
    public bool Number4down { get { return _currentState[(int)Key.Number4]; } }
    public bool Number5down { get { return _currentState[(int)Key.Number5]; } }
    public bool Number6down { get { return _currentState[(int)Key.Number6]; } }
    public bool Number7down { get { return _currentState[(int)Key.Number7]; } }
    public bool Number8down { get { return _currentState[(int)Key.Number8]; } }
    public bool Number9down { get { return _currentState[(int)Key.Number9]; } }

    public bool Keypad0down { get { return _currentState[(int)Key.Keypad0]; } }
    public bool Keypad1down { get { return _currentState[(int)Key.Keypad1]; } }
    public bool Keypad2down { get { return _currentState[(int)Key.Keypad2]; } }
    public bool Keypad3down { get { return _currentState[(int)Key.Keypad3]; } }
    public bool Keypad4down { get { return _currentState[(int)Key.Keypad4]; } }
    public bool Keypad5down { get { return _currentState[(int)Key.Keypad5]; } }
    public bool Keypad6down { get { return _currentState[(int)Key.Keypad6]; } }
    public bool Keypad7down { get { return _currentState[(int)Key.Keypad7]; } }
    public bool Keypad8down { get { return _currentState[(int)Key.Keypad8]; } }
    public bool Keypad9down { get { return _currentState[(int)Key.Keypad9]; } }
    public bool KeypadEnterdown { get { return _currentState[(int)Key.KeypadEnter]; } }
    public bool KeypadMinusdown { get { return _currentState[(int)Key.KeypadMinus]; } }
    public bool KeypadMultiplydown { get { return _currentState[(int)Key.KeypadMultiply]; } }
    public bool KeypadPlusdown { get { return _currentState[(int)Key.KeypadPlus]; } }
    public bool KeypadSubtractdown { get { return _currentState[(int)Key.KeypadSubtract]; } }
    public bool KeypadAdddown { get { return _currentState[(int)Key.KeypadAdd]; } }
    public bool KeypadDecimaldown { get { return _currentState[(int)Key.KeypadDecimal]; } }
    public bool KeypadDividedown { get { return _currentState[(int)Key.KeypadDivide]; } }

    public bool F1down { get { return _currentState[(int)Key.F1]; } }
    public bool F2down { get { return _currentState[(int)Key.F2]; } }
    public bool F3down { get { return _currentState[(int)Key.F3]; } }
    public bool F4down { get { return _currentState[(int)Key.F4]; } }
    public bool F5down { get { return _currentState[(int)Key.F5]; } }
    public bool F6down { get { return _currentState[(int)Key.F6]; } }
    public bool F7down { get { return _currentState[(int)Key.F7]; } }
    public bool F8down { get { return _currentState[(int)Key.F8]; } }
    public bool F9down { get { return _currentState[(int)Key.F9]; } }
    public bool F10down { get { return _currentState[(int)Key.F10]; } }
    public bool F11down { get { return _currentState[(int)Key.F11]; } }
    public bool F12down { get { return _currentState[(int)Key.F12]; } }

    public bool Downdown { get { return _currentState[(int)Key.Down]; } }
    public bool Updown { get { return _currentState[(int)Key.Up]; } }
    public bool Leftdown { get { return _currentState[(int)Key.Left]; } }
    public bool Rightdown { get { return _currentState[(int)Key.Right]; } }

    public bool Escapedown { get { return _currentState[(int)Key.Escape]; } }
    public bool ShiftLeftdown { get { return _currentState[(int)Key.ShiftLeft]; } }
    public bool ShiftRightdown { get { return _currentState[(int)Key.ShiftRight]; } }
    public bool Spacedown { get { return _currentState[(int)Key.Space]; } }
    public bool Tabdown { get { return _currentState[(int)Key.Tab]; } }
    public bool Tildedown { get { return _currentState[(int)Key.Tilde]; } }
    public bool Semicolondown { get { return _currentState[(int)Key.Semicolon]; } }
    public bool PageUpdown { get { return _currentState[(int)Key.PageUp]; } }
    public bool Enterdown { get { return _currentState[(int)Key.Enter]; } }
    public bool AltLeftdown { get { return _currentState[(int)Key.LAlt]; } }
    public bool AltRightdown { get { return _currentState[(int)Key.RAlt]; } }
    public bool ControlLeftdown { get { return _currentState[(int)Key.ControlLeft]; } }
    public bool ControlRightdown { get { return _currentState[(int)Key.ControlRight]; } }
    public bool BracketLeftdown { get { return _currentState[(int)Key.BracketLeft]; } }
    public bool BracketRightdown { get { return _currentState[(int)Key.BracketRight]; } }
    public bool BackSlashdown { get { return _currentState[(int)Key.BackSlash]; } }
    public bool Commadown { get { return _currentState[(int)Key.Comma]; } }
    public bool Deletedown { get { return _currentState[(int)Key.Delete]; } }
    public bool CapsLockdown { get { return _currentState[(int)Key.CapsLock]; } }
    public bool Insertdown { get { return _currentState[(int)Key.Insert]; } }
    public bool Enddown { get { return _currentState[(int)Key.End]; } }
    public bool Homedown { get { return _currentState[(int)Key.Home]; } }
    #endregion

    #region Up Properties
    public bool Aup { get { return !_currentState[(int)Key.A]; } }
    public bool Bup { get { return !_currentState[(int)Key.B]; } }
    public bool Cup { get { return !_currentState[(int)Key.C]; } }
    public bool Dup { get { return !_currentState[(int)Key.D]; } }
    public bool Eup { get { return !_currentState[(int)Key.E]; } }
    public bool Fup { get { return !_currentState[(int)Key.F]; } }
    public bool Gup { get { return !_currentState[(int)Key.G]; } }
    public bool Hup { get { return !_currentState[(int)Key.H]; } }
    public bool Iup { get { return !_currentState[(int)Key.I]; } }
    public bool Jup { get { return !_currentState[(int)Key.J]; } }
    public bool Kup { get { return !_currentState[(int)Key.K]; } }
    public bool Lup { get { return !_currentState[(int)Key.L]; } }
    public bool Mup { get { return !_currentState[(int)Key.M]; } }
    public bool Nup { get { return !_currentState[(int)Key.N]; } }
    public bool Oup { get { return !_currentState[(int)Key.O]; } }
    public bool Pup { get { return !_currentState[(int)Key.P]; } }
    public bool Qup { get { return !_currentState[(int)Key.Q]; } }
    public bool Rup { get { return !_currentState[(int)Key.R]; } }
    public bool Sup { get { return !_currentState[(int)Key.S]; } }
    public bool Tup { get { return !_currentState[(int)Key.T]; } }
    public bool Uup { get { return !_currentState[(int)Key.U]; } }
    public bool Vup { get { return !_currentState[(int)Key.V]; } }
    public bool Wup { get { return !_currentState[(int)Key.W]; } }
    public bool Xup { get { return !_currentState[(int)Key.X]; } }
    public bool Yup { get { return !_currentState[(int)Key.Y]; } }
    public bool Zup { get { return !_currentState[(int)Key.Z]; } }

    public bool Number0up { get { return !_currentState[(int)Key.Number0]; } }
    public bool Number1up { get { return !_currentState[(int)Key.Number1]; } }
    public bool Number2up { get { return !_currentState[(int)Key.Number2]; } }
    public bool Number3up { get { return !_currentState[(int)Key.Number3]; } }
    public bool Number4up { get { return !_currentState[(int)Key.Number4]; } }
    public bool Number5up { get { return !_currentState[(int)Key.Number5]; } }
    public bool Number6up { get { return !_currentState[(int)Key.Number6]; } }
    public bool Number7up { get { return !_currentState[(int)Key.Number7]; } }
    public bool Number8up { get { return !_currentState[(int)Key.Number8]; } }
    public bool Number9up { get { return !_currentState[(int)Key.Number9]; } }

    public bool Keypad0up { get { return !_currentState[(int)Key.Keypad0]; } }
    public bool Keypad1up { get { return !_currentState[(int)Key.Keypad1]; } }
    public bool Keypad2up { get { return !_currentState[(int)Key.Keypad2]; } }
    public bool Keypad3up { get { return !_currentState[(int)Key.Keypad3]; } }
    public bool Keypad4up { get { return !_currentState[(int)Key.Keypad4]; } }
    public bool Keypad5up { get { return !_currentState[(int)Key.Keypad5]; } }
    public bool Keypad6up { get { return !_currentState[(int)Key.Keypad6]; } }
    public bool Keypad7up { get { return !_currentState[(int)Key.Keypad7]; } }
    public bool Keypad8up { get { return !_currentState[(int)Key.Keypad8]; } }
    public bool Keypad9up { get { return !_currentState[(int)Key.Keypad9]; } }
    public bool KeypadEnterup { get { return !_currentState[(int)Key.KeypadEnter]; } }
    public bool KeypadMinusup { get { return !_currentState[(int)Key.KeypadMinus]; } }
    public bool KeypadMultiplyup { get { return !_currentState[(int)Key.KeypadMultiply]; } }
    public bool KeypadPlusup { get { return !_currentState[(int)Key.KeypadPlus]; } }
    public bool KeypadSubtractup { get { return !_currentState[(int)Key.KeypadSubtract]; } }
    public bool KeypadAddup { get { return !_currentState[(int)Key.KeypadAdd]; } }
    public bool KeypadDecimalup { get { return !_currentState[(int)Key.KeypadDecimal]; } }
    public bool KeypadDivideup { get { return !_currentState[(int)Key.KeypadDivide]; } }

    public bool F1up { get { return !_currentState[(int)Key.F1]; } }
    public bool F2up { get { return !_currentState[(int)Key.F2]; } }
    public bool F3up { get { return !_currentState[(int)Key.F3]; } }
    public bool F4up { get { return !_currentState[(int)Key.F4]; } }
    public bool F5up { get { return !_currentState[(int)Key.F5]; } }
    public bool F6up { get { return !_currentState[(int)Key.F6]; } }
    public bool F7up { get { return !_currentState[(int)Key.F7]; } }
    public bool F8up { get { return !_currentState[(int)Key.F8]; } }
    public bool F9up { get { return !_currentState[(int)Key.F9]; } }
    public bool F10up { get { return !_currentState[(int)Key.F10]; } }
    public bool F11up { get { return !_currentState[(int)Key.F11]; } }
    public bool F12up { get { return !_currentState[(int)Key.F12]; } }

    public bool Downup { get { return !_currentState[(int)Key.Down]; } }
    public bool Upup { get { return !_currentState[(int)Key.Up]; } }
    public bool Leftup { get { return !_currentState[(int)Key.Left]; } }
    public bool Rightup { get { return !_currentState[(int)Key.Right]; } }

    public bool Escapeup { get { return !_currentState[(int)Key.Escape]; } }
    public bool ShiftLeftup { get { return !_currentState[(int)Key.ShiftLeft]; } }
    public bool ShiftRightup { get { return !_currentState[(int)Key.ShiftRight]; } }
    public bool Spaceup { get { return !_currentState[(int)Key.Space]; } }
    public bool Tabup { get { return !_currentState[(int)Key.Tab]; } }
    public bool Tildeup { get { return !_currentState[(int)Key.Tilde]; } }
    public bool Semicolonup { get { return !_currentState[(int)Key.Semicolon]; } }
    public bool PageUpup { get { return !_currentState[(int)Key.PageUp]; } }
    public bool Enterup { get { return !_currentState[(int)Key.Enter]; } }
    public bool AltLeftup { get { return !_currentState[(int)Key.LAlt]; } }
    public bool AltRightup { get { return !_currentState[(int)Key.RAlt]; } }
    public bool ControlLeftup { get { return !_currentState[(int)Key.ControlLeft]; } }
    public bool ControlRightup { get { return !_currentState[(int)Key.ControlRight]; } }
    public bool BracketLeftup { get { return !_currentState[(int)Key.BracketLeft]; } }
    public bool BracketRightup { get { return !_currentState[(int)Key.BracketRight]; } }
    public bool BackSlashup { get { return !_currentState[(int)Key.BackSlash]; } }
    public bool Commaup { get { return !_currentState[(int)Key.Comma]; } }
    public bool Deleteup { get { return !_currentState[(int)Key.Delete]; } }
    public bool CapsLockup { get { return !_currentState[(int)Key.CapsLock]; } }
    public bool Insertup { get { return !_currentState[(int)Key.Insert]; } }
    public bool Endup { get { return !_currentState[(int)Key.End]; } }
    public bool Homeup { get { return !_currentState[(int)Key.Home]; } }
    #endregion

    # region Released Properties
    public bool Areleased { get { return _previousState[(int)Key.A] && !_currentState[(int)Key.A]; } }
    public bool Breleased { get { return _previousState[(int)Key.B] && !_currentState[(int)Key.B]; } }
    public bool Creleased { get { return _previousState[(int)Key.C] && !_currentState[(int)Key.C]; } }
    public bool Dreleased { get { return _previousState[(int)Key.D] && !_currentState[(int)Key.D]; } }
    public bool Ereleased { get { return _previousState[(int)Key.E] && !_currentState[(int)Key.E]; } }
    public bool Freleased { get { return _previousState[(int)Key.F] && !_currentState[(int)Key.F]; } }
    public bool Greleased { get { return _previousState[(int)Key.G] && !_currentState[(int)Key.G]; } }
    public bool Hreleased { get { return _previousState[(int)Key.H] && !_currentState[(int)Key.H]; } }
    public bool Ireleased { get { return _previousState[(int)Key.I] && !_currentState[(int)Key.I]; } }
    public bool Jreleased { get { return _previousState[(int)Key.J] && !_currentState[(int)Key.J]; } }
    public bool Kreleased { get { return _previousState[(int)Key.K] && !_currentState[(int)Key.K]; } }
    public bool Lreleased { get { return _previousState[(int)Key.L] && !_currentState[(int)Key.L]; } }
    public bool Mreleased { get { return _previousState[(int)Key.M] && !_currentState[(int)Key.M]; } }
    public bool Nreleased { get { return _previousState[(int)Key.N] && !_currentState[(int)Key.N]; } }
    public bool Oreleased { get { return _previousState[(int)Key.O] && !_currentState[(int)Key.O]; } }
    public bool Preleased { get { return _previousState[(int)Key.P] && !_currentState[(int)Key.P]; } }
    public bool Qreleased { get { return _previousState[(int)Key.Q] && !_currentState[(int)Key.Q]; } }
    public bool Rreleased { get { return _previousState[(int)Key.R] && !_currentState[(int)Key.R]; } }
    public bool Sreleased { get { return _previousState[(int)Key.S] && !_currentState[(int)Key.S]; } }
    public bool Treleased { get { return _previousState[(int)Key.T] && !_currentState[(int)Key.T]; } }
    public bool Ureleased { get { return _previousState[(int)Key.U] && !_currentState[(int)Key.U]; } }
    public bool Vreleased { get { return _previousState[(int)Key.V] && !_currentState[(int)Key.V]; } }
    public bool Wreleased { get { return _previousState[(int)Key.W] && !_currentState[(int)Key.W]; } }
    public bool Xreleased { get { return _previousState[(int)Key.X] && !_currentState[(int)Key.X]; } }
    public bool Yreleased { get { return _previousState[(int)Key.Y] && !_currentState[(int)Key.Y]; } }
    public bool Zreleased { get { return _previousState[(int)Key.Z] && !_currentState[(int)Key.Z]; } }

    public bool Number0released { get { return _previousState[(int)Key.Number0] && !_currentState[(int)Key.Number0]; } }
    public bool Number1released { get { return _previousState[(int)Key.Number1] && !_currentState[(int)Key.Number1]; } }
    public bool Number2released { get { return _previousState[(int)Key.Number2] && !_currentState[(int)Key.Number2]; } }
    public bool Number3released { get { return _previousState[(int)Key.Number3] && !_currentState[(int)Key.Number3]; } }
    public bool Number4released { get { return _previousState[(int)Key.Number4] && !_currentState[(int)Key.Number4]; } }
    public bool Number5released { get { return _previousState[(int)Key.Number5] && !_currentState[(int)Key.Number5]; } }
    public bool Number6released { get { return _previousState[(int)Key.Number6] && !_currentState[(int)Key.Number6]; } }
    public bool Number7released { get { return _previousState[(int)Key.Number7] && !_currentState[(int)Key.Number7]; } }
    public bool Number8released { get { return _previousState[(int)Key.Number8] && !_currentState[(int)Key.Number8]; } }
    public bool Number9released { get { return _previousState[(int)Key.Number9] && !_currentState[(int)Key.Number9]; } }

    public bool Keypad0released { get { return _previousState[(int)Key.Keypad0] && !_currentState[(int)Key.Keypad0]; } }
    public bool Keypad1released { get { return _previousState[(int)Key.Keypad1] && !_currentState[(int)Key.Keypad1]; } }
    public bool Keypad2released { get { return _previousState[(int)Key.Keypad2] && !_currentState[(int)Key.Keypad2]; } }
    public bool Keypad3released { get { return _previousState[(int)Key.Keypad3] && !_currentState[(int)Key.Keypad3]; } }
    public bool Keypad4released { get { return _previousState[(int)Key.Keypad4] && !_currentState[(int)Key.Keypad4]; } }
    public bool Keypad5released { get { return _previousState[(int)Key.Keypad5] && !_currentState[(int)Key.Keypad5]; } }
    public bool Keypad6released { get { return _previousState[(int)Key.Keypad6] && !_currentState[(int)Key.Keypad6]; } }
    public bool Keypad7released { get { return _previousState[(int)Key.Keypad7] && !_currentState[(int)Key.Keypad7]; } }
    public bool Keypad8released { get { return _previousState[(int)Key.Keypad8] && !_currentState[(int)Key.Keypad8]; } }
    public bool Keypad9released { get { return _previousState[(int)Key.Keypad9] && !_currentState[(int)Key.Keypad9]; } }
    public bool KeypadEnterreleased { get { return _previousState[(int)Key.KeypadEnter] && !_currentState[(int)Key.KeypadEnter]; } }
    public bool KeypadMinusreleased { get { return _previousState[(int)Key.KeypadMinus] && !_currentState[(int)Key.KeypadMinus]; } }
    public bool KeypadMultiplyreleased { get { return _previousState[(int)Key.KeypadMultiply] && !_currentState[(int)Key.KeypadMultiply]; } }
    public bool KeypadPlusreleased { get { return _previousState[(int)Key.KeypadPlus] && !_currentState[(int)Key.KeypadPlus]; } }
    public bool KeypadSubtractreleased { get { return _previousState[(int)Key.KeypadSubtract] && !_currentState[(int)Key.KeypadSubtract]; } }
    public bool KeypadAddreleased { get { return _previousState[(int)Key.KeypadAdd] && !_currentState[(int)Key.KeypadAdd]; } }
    public bool KeypadDecimalreleased { get { return _previousState[(int)Key.KeypadDecimal] && !_currentState[(int)Key.KeypadDecimal]; } }
    public bool KeypadDividereleased { get { return _previousState[(int)Key.KeypadDivide] && !_currentState[(int)Key.KeypadDivide]; } }

    public bool F1released { get { return _previousState[(int)Key.F1] && !_currentState[(int)Key.F1]; } }
    public bool F2released { get { return _previousState[(int)Key.F2] && !_currentState[(int)Key.F2]; } }
    public bool F3released { get { return _previousState[(int)Key.F3] && !_currentState[(int)Key.F3]; } }
    public bool F4released { get { return _previousState[(int)Key.F4] && !_currentState[(int)Key.F4]; } }
    public bool F5released { get { return _previousState[(int)Key.F5] && !_currentState[(int)Key.F5]; } }
    public bool F6released { get { return _previousState[(int)Key.F6] && !_currentState[(int)Key.F6]; } }
    public bool F7released { get { return _previousState[(int)Key.F7] && !_currentState[(int)Key.F7]; } }
    public bool F8released { get { return _previousState[(int)Key.F8] && !_currentState[(int)Key.F8]; } }
    public bool F9released { get { return _previousState[(int)Key.F9] && !_currentState[(int)Key.F9]; } }
    public bool F10released { get { return _previousState[(int)Key.F10] && !_currentState[(int)Key.F10]; } }
    public bool F11released { get { return _previousState[(int)Key.F11] && !_currentState[(int)Key.F11]; } }
    public bool F12released { get { return _previousState[(int)Key.F12] && !_currentState[(int)Key.F12]; } }

    public bool Downreleased { get { return _previousState[(int)Key.Down] && !_currentState[(int)Key.Down]; } }
    public bool Upreleased { get { return _previousState[(int)Key.Up] && !_currentState[(int)Key.Up]; } }
    public bool Leftreleased { get { return _previousState[(int)Key.Left] && !_currentState[(int)Key.Left]; } }
    public bool Rightreleased { get { return _previousState[(int)Key.Right] && !_currentState[(int)Key.Right]; } }

    public bool Escapereleased { get { return _previousState[(int)Key.Escape] && !_currentState[(int)Key.Escape]; } }
    public bool ShiftLeftreleased { get { return _previousState[(int)Key.ShiftLeft] && !_currentState[(int)Key.ShiftLeft]; } }
    public bool ShiftRightreleased { get { return _previousState[(int)Key.ShiftRight] && !_currentState[(int)Key.ShiftRight]; } }
    public bool Spacereleased { get { return _previousState[(int)Key.Space] && !_currentState[(int)Key.Space]; } }
    public bool Tabreleased { get { return _previousState[(int)Key.Tab] && !_currentState[(int)Key.Tab]; } }
    public bool Tildereleased { get { return _previousState[(int)Key.Tilde] && !_currentState[(int)Key.Tilde]; } }
    public bool Semicolonreleased { get { return _previousState[(int)Key.Semicolon] && !_currentState[(int)Key.Semicolon]; } }
    public bool PageUpreleased { get { return _previousState[(int)Key.PageUp] && !_currentState[(int)Key.PageUp]; } }
    public bool Enterreleased { get { return _previousState[(int)Key.Enter] && !_currentState[(int)Key.Enter]; } }
    public bool AltLeftreleased { get { return _previousState[(int)Key.LAlt] && !_currentState[(int)Key.LAlt]; } }
    public bool AltRightreleased { get { return _previousState[(int)Key.RAlt] && !_currentState[(int)Key.RAlt]; } }
    public bool ControlLeftreleased { get { return _previousState[(int)Key.ControlLeft] && !_currentState[(int)Key.ControlLeft]; } }
    public bool ControlRightreleased { get { return _previousState[(int)Key.ControlRight] && !_currentState[(int)Key.ControlRight]; } }
    public bool BracketLeftreleased { get { return _previousState[(int)Key.BracketLeft] && !_currentState[(int)Key.BracketLeft]; } }
    public bool BracketRightreleased { get { return _previousState[(int)Key.BracketRight] && !_currentState[(int)Key.BracketRight]; } }
    public bool BackSlashreleased { get { return _previousState[(int)Key.BackSlash] && !_currentState[(int)Key.BackSlash]; } }
    public bool Commareleased { get { return _previousState[(int)Key.Comma] && !_currentState[(int)Key.Comma]; } }
    public bool Deletereleased { get { return _previousState[(int)Key.Delete] && !_currentState[(int)Key.Delete]; } }
    public bool CapsLockreleased { get { return _previousState[(int)Key.CapsLock] && !_currentState[(int)Key.CapsLock]; } }
    public bool Insertreleased { get { return _previousState[(int)Key.Insert] && !_currentState[(int)Key.Insert]; } }
    public bool Endreleased { get { return _previousState[(int)Key.End] && !_currentState[(int)Key.End]; } }
    public bool Homereleased { get { return _previousState[(int)Key.Home] && !_currentState[(int)Key.Home]; } }
    #endregion
  }
}