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
    private static bool _endProgramKey = false;
    /// <summary>Magic key press that shuts down the game.</summary>
    public static bool EndProgramKey { get { return _endProgramKey; } }

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
      if (_keyboard[Key.Escape])
        _endProgramKey = true;

      // Swap the current input state
      if (_currentState.Equals(_stateOne))
        _currentState = _stateTwo;
      if (_currentState.Equals(_stateTwo))
        _currentState = _stateOne;

      // Swap the old input state
      if (_previousState.Equals(_stateOne))
        _previousState = _stateTwo;
      if (_previousState.Equals(_stateTwo))
        _previousState = _stateOne;

      // Update the state values
      #region Update Input States
      if (_keyboard[Key.A])
        _currentState[(int)Key.A] = true;
      else
        _currentState[(int)Key.A] = false;
      if (_keyboard[Key.B])
        _currentState[(int)Key.B] = true;
      else
        _currentState[(int)Key.B] = false;
      if (_keyboard[Key.C])
        _currentState[(int)Key.C] = true;
      else
        _currentState[(int)Key.C] = false;
      if (_keyboard[Key.D])
        _currentState[(int)Key.D] = true;
      else
        _currentState[(int)Key.D] = false;
      if (_keyboard[Key.E])
        _currentState[(int)Key.E] = true;
      else
        _currentState[(int)Key.E] = false;
      if (_keyboard[Key.F])
        _currentState[(int)Key.F] = true;
      else
        _currentState[(int)Key.F] = false;
      if (_keyboard[Key.G])
        _currentState[(int)Key.G] = true;
      else
        _currentState[(int)Key.G] = false;
      if (_keyboard[Key.H])
        _currentState[(int)Key.H] = true;
      else
        _currentState[(int)Key.H] = false;
      if (_keyboard[Key.I])
        _currentState[(int)Key.I] = true;
      else
        _currentState[(int)Key.I] = false;
      if (_keyboard[Key.J])
        _currentState[(int)Key.J] = true;
      else
        _currentState[(int)Key.J] = false;
      if (_keyboard[Key.K])
        _currentState[(int)Key.K] = true;
      else
        _currentState[(int)Key.K] = false;
      if (_keyboard[Key.L])
        _currentState[(int)Key.L] = true;
      else
        _currentState[(int)Key.L] = false;
      if (_keyboard[Key.M])
        _currentState[(int)Key.M] = true;
      else
        _currentState[(int)Key.M] = false;
      if (_keyboard[Key.N])
        _currentState[(int)Key.N] = true;
      else
        _currentState[(int)Key.N] = false;
      if (_keyboard[Key.O])
        _currentState[(int)Key.O] = true;
      else
        _currentState[(int)Key.O] = false;
      if (_keyboard[Key.P])
        _currentState[(int)Key.P] = true;
      else
        _currentState[(int)Key.P] = false;
      if (_keyboard[Key.Q])
        _currentState[(int)Key.Q] = true;
      else
        _currentState[(int)Key.Q] = false;
      if (_keyboard[Key.R])
        _currentState[(int)Key.R] = true;
      else
        _currentState[(int)Key.R] = false;
      if (_keyboard[Key.S])
        _currentState[(int)Key.S] = true;
      else
        _currentState[(int)Key.S] = false;
      if (_keyboard[Key.T])
        _currentState[(int)Key.T] = true;
      else
        _currentState[(int)Key.T] = false;
      if (_keyboard[Key.U])
        _currentState[(int)Key.U] = true;
      else
        _currentState[(int)Key.U] = false;
      if (_keyboard[Key.V])
        _currentState[(int)Key.V] = true;
      else
        _currentState[(int)Key.V] = false;
      if (_keyboard[Key.W])
        _currentState[(int)Key.W] = true;
      else
        _currentState[(int)Key.W] = false;
      if (_keyboard[Key.X])
        _currentState[(int)Key.X] = true;
      else
        _currentState[(int)Key.X] = false;
      if (_keyboard[Key.Y])
        _currentState[(int)Key.Y] = true;
      else
        _currentState[(int)Key.Y] = false;
      if (_keyboard[Key.Z])
        _currentState[(int)Key.Z] = true;
      else
        _currentState[(int)Key.Z] = false;
#endregion
    }

    #region Pressed Properties
    public static bool Apressed { get { return _previousState[(int)Key.A] == false && _currentState[(int)Key.A]; } }
    public static bool Bpressed { get { return _previousState[(int)Key.B] == false && _currentState[(int)Key.B]; } }
    public static bool Cpressed { get { return _previousState[(int)Key.C] == false && _currentState[(int)Key.C]; } }
    public static bool Dpressed { get { return _previousState[(int)Key.D] == false && _currentState[(int)Key.D]; } }
    public static bool Epressed { get { return _previousState[(int)Key.E] == false && _currentState[(int)Key.E]; } }
    public static bool Fpressed { get { return _previousState[(int)Key.F] == false && _currentState[(int)Key.F]; } }
    public static bool Gpressed { get { return _previousState[(int)Key.G] == false && _currentState[(int)Key.G]; } }
    public static bool Hpressed { get { return _previousState[(int)Key.H] == false && _currentState[(int)Key.H]; } }
    public static bool Ipressed { get { return _previousState[(int)Key.I] == false && _currentState[(int)Key.I]; } }
    public static bool Jpressed { get { return _previousState[(int)Key.J] == false && _currentState[(int)Key.J]; } }
    public static bool Kpressed { get { return _previousState[(int)Key.K] == false && _currentState[(int)Key.K]; } }
    public static bool Lpressed { get { return _previousState[(int)Key.L] == false && _currentState[(int)Key.L]; } }
    public static bool Mpressed { get { return _previousState[(int)Key.M] == false && _currentState[(int)Key.M]; } }
    public static bool Npressed { get { return _previousState[(int)Key.N] == false && _currentState[(int)Key.N]; } }
    public static bool Opressed { get { return _previousState[(int)Key.O] == false && _currentState[(int)Key.O]; } }
    public static bool Ppressed { get { return _previousState[(int)Key.P] == false && _currentState[(int)Key.P]; } }
    public static bool Qpressed { get { return _previousState[(int)Key.Q] == false && _currentState[(int)Key.Q]; } }
    public static bool Rpressed { get { return _previousState[(int)Key.R] == false && _currentState[(int)Key.R]; } }
    public static bool Spressed { get { return _previousState[(int)Key.S] == false && _currentState[(int)Key.S]; } }
    public static bool Tpressed { get { return _previousState[(int)Key.T] == false && _currentState[(int)Key.T]; } }
    public static bool Upressed { get { return _previousState[(int)Key.U] == false && _currentState[(int)Key.U]; } }
    public static bool Vpressed { get { return _previousState[(int)Key.V] == false && _currentState[(int)Key.V]; } }
    public static bool Wpressed { get { return _previousState[(int)Key.W] == false && _currentState[(int)Key.W]; } }
    public static bool Xpressed { get { return _previousState[(int)Key.X] == false && _currentState[(int)Key.X]; } }
    public static bool Ypressed { get { return _previousState[(int)Key.Y] == false && _currentState[(int)Key.Y]; } }
    public static bool Zpressed { get { return _previousState[(int)Key.Z] == false && _currentState[(int)Key.Z]; } }
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
    #endregion

    # region Released Properties
    public static bool Areleased { get { return _previousState[(int)Key.A] && _currentState[(int)Key.A] == false; } }
    public static bool Breleased { get { return _previousState[(int)Key.B] && _currentState[(int)Key.B] == false; } }
    public static bool Creleased { get { return _previousState[(int)Key.C] && _currentState[(int)Key.C] == false; } }
    public static bool Dreleased { get { return _previousState[(int)Key.D] && _currentState[(int)Key.D] == false; } }
    public static bool Ereleased { get { return _previousState[(int)Key.E] && _currentState[(int)Key.E] == false; } }
    public static bool Freleased { get { return _previousState[(int)Key.F] && _currentState[(int)Key.F] == false; } }
    public static bool Greleased { get { return _previousState[(int)Key.G] && _currentState[(int)Key.G] == false; } }
    public static bool Hreleased { get { return _previousState[(int)Key.H] && _currentState[(int)Key.H] == false; } }
    public static bool Ireleased { get { return _previousState[(int)Key.I] && _currentState[(int)Key.I] == false; } }
    public static bool Jreleased { get { return _previousState[(int)Key.J] && _currentState[(int)Key.J] == false; } }
    public static bool Kreleased { get { return _previousState[(int)Key.K] && _currentState[(int)Key.K] == false; } }
    public static bool Lreleased { get { return _previousState[(int)Key.L] && _currentState[(int)Key.L] == false; } }
    public static bool Mreleased { get { return _previousState[(int)Key.M] && _currentState[(int)Key.M] == false; } }
    public static bool Nreleased { get { return _previousState[(int)Key.N] && _currentState[(int)Key.N] == false; } }
    public static bool Oreleased { get { return _previousState[(int)Key.O] && _currentState[(int)Key.O] == false; } }
    public static bool Preleased { get { return _previousState[(int)Key.P] && _currentState[(int)Key.P] == false; } }
    public static bool Qreleased { get { return _previousState[(int)Key.Q] && _currentState[(int)Key.Q] == false; } }
    public static bool Rreleased { get { return _previousState[(int)Key.R] && _currentState[(int)Key.R] == false; } }
    public static bool Sreleased { get { return _previousState[(int)Key.S] && _currentState[(int)Key.S] == false; } }
    public static bool Treleased { get { return _previousState[(int)Key.T] && _currentState[(int)Key.T] == false; } }
    public static bool Ureleased { get { return _previousState[(int)Key.U] && _currentState[(int)Key.U] == false; } }
    public static bool Vreleased { get { return _previousState[(int)Key.V] && _currentState[(int)Key.V] == false; } }
    public static bool Wreleased { get { return _previousState[(int)Key.W] && _currentState[(int)Key.W] == false; } }
    public static bool Xreleased { get { return _previousState[(int)Key.X] && _currentState[(int)Key.X] == false; } }
    public static bool Yreleased { get { return _previousState[(int)Key.Y] && _currentState[(int)Key.Y] == false; } }
    public static bool Zreleased { get { return _previousState[(int)Key.Z] && _currentState[(int)Key.Z] == false; } }
    #endregion
  }
}