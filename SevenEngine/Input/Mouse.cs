// SEVENENGINE LISCENSE:
// You are free to use, modify, and distribute any or all code segments/files for any purpose
// including commercial use under the following condition: any code using or originally taken 
// from the SevenEngine project must include citation to its original author(s) located at the
// top of each source code file, or you may include a reference to the SevenEngine project as
// a whole but you must include the current SevenEngine official website URL and logo.
// - Thanks.  :)  (support: seven@sevenengine.com)

// Author(s):
// - Zachary Aaron Patten (aka Seven) seven@sevenengine.com
// Last Edited: 11-16-13

using OpenTK.Input;

namespace SevenEngine.Input
{
  /// <summary>Manages mouse input using a state system consisting of two states.</summary>
  public class Mouse
  {
    // The number of mouse buttons supported by OpenTK
    private const int _numberOfMouseButtons = 13;

    // State value storage
    private bool[] _stateOne;
    private bool[] _stateTwo;

    // Reference to the states indicating past/present
    private bool[] _currentState;
    private bool[] _previousState;

    // Reference to the OpenTK mouse object
    private MouseDevice _mouse;

    // Previous mouse position
    private int _previousX;
    private int _previousY;

    /// <summary>Initializes this mouse object with a pointer to the OpenTK mouse object.</summary>
    /// <param name="mouse">The reference to the OpenTK mouse object.</param>
    public Mouse(MouseDevice mouse)
    {
      _mouse = mouse;
      _stateOne = new bool[_numberOfMouseButtons];
      _stateTwo = new bool[_numberOfMouseButtons];
      _currentState = _stateOne;
      _previousState = _stateTwo;

      // Reset the mouse position
      _previousX = _mouse.X;
      _previousY = _mouse.Y;
    }

    internal void Update()
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

      #region State Updating

      // Update the mouse buttons
      _currentState[(int)MouseButton.Left] = _mouse[MouseButton.Left];
      _currentState[(int)MouseButton.Middle] = _mouse[MouseButton.Middle];
      _currentState[(int)MouseButton.Right] = _mouse[MouseButton.Right];

      _currentState[(int)MouseButton.Button1] = _mouse[MouseButton.Button1];
      _currentState[(int)MouseButton.Button2] = _mouse[MouseButton.Button2];
      _currentState[(int)MouseButton.Button3] = _mouse[MouseButton.Button3];
      _currentState[(int)MouseButton.Button4] = _mouse[MouseButton.Button4];
      _currentState[(int)MouseButton.Button5] = _mouse[MouseButton.Button5];
      _currentState[(int)MouseButton.Button6] = _mouse[MouseButton.Button6];
      _currentState[(int)MouseButton.Button7] = _mouse[MouseButton.Button7];
      _currentState[(int)MouseButton.Button8] = _mouse[MouseButton.Button8];
      _currentState[(int)MouseButton.Button9] = _mouse[MouseButton.Button9];
      _currentState[(int)MouseButton.LastButton] = _mouse[MouseButton.LastButton];

      #endregion
    }

    /// <summary>(ONLY CALL THIS ONCE PER INOUT HANDLING!!!) Gets the current delta X and updates the mouse position</summary>
    public int deltaX
    {
      get
      {
        int temp = _previousX;
        _previousX = _mouse.X;
        return _mouse.X - temp;
      }
    }

    /// <summary>(ONLY CALL THIS ONCE PER INOUT HANDLING!!!) Gets the current delta Y and updates the mouse position</summary>
    public int deltaY
    {
      get
      {
        int temp = _previousY;
        _previousY = _mouse.Y;
        return _mouse.Y - temp;
      }
    }

    #region Pressed Properties

    public bool LeftClickPressed { get { return !_previousState[(int)MouseButton.Left] && _currentState[(int)MouseButton.Left]; } }
    public bool MiddleClickPressed { get { return !_previousState[(int)MouseButton.Middle] && _currentState[(int)MouseButton.Middle]; } }
    public bool RightClickPressed { get { return !_previousState[(int)MouseButton.Right] && _currentState[(int)MouseButton.Right]; } }

    public bool Button1pressed { get { return !_previousState[(int)MouseButton.Button1] && _currentState[(int)MouseButton.Button1]; } }
    public bool Button2pressed { get { return !_previousState[(int)MouseButton.Button2] && _currentState[(int)MouseButton.Button2]; } }
    public bool Button3pressed { get { return !_previousState[(int)MouseButton.Button3] && _currentState[(int)MouseButton.Button3]; } }
    public bool Button4pressed { get { return !_previousState[(int)MouseButton.Button4] && _currentState[(int)MouseButton.Button4]; } }
    public bool Button5pressed { get { return !_previousState[(int)MouseButton.Button5] && _currentState[(int)MouseButton.Button5]; } }
    public bool Button6pressed { get { return !_previousState[(int)MouseButton.Button6] && _currentState[(int)MouseButton.Button6]; } }
    public bool Button7pressed { get { return !_previousState[(int)MouseButton.Button7] && _currentState[(int)MouseButton.Button7]; } }
    public bool Button8pressed { get { return !_previousState[(int)MouseButton.Button8] && _currentState[(int)MouseButton.Button8]; } }
    public bool Button9pressed { get { return !_previousState[(int)MouseButton.Button9] && _currentState[(int)MouseButton.Button9]; } }
    public bool LastButtonpressed { get { return !_previousState[(int)MouseButton.LastButton] && _currentState[(int)MouseButton.LastButton]; } }

    #endregion

    #region Down Properties

    public bool LeftClickdown { get { return _currentState[(int)MouseButton.Left]; } }
    public bool MiddleClickdown { get { return _currentState[(int)MouseButton.Middle]; } }
    public bool RightClickdown { get { return _currentState[(int)MouseButton.Right]; } }

    public bool Button1down { get { return _currentState[(int)MouseButton.Button1]; } }
    public bool Button2down { get { return _currentState[(int)MouseButton.Button2]; } }
    public bool Button3down { get { return _currentState[(int)MouseButton.Button3]; } }
    public bool Button4down { get { return _currentState[(int)MouseButton.Button4]; } }
    public bool Button5down { get { return _currentState[(int)MouseButton.Button5]; } }
    public bool Button6down { get { return _currentState[(int)MouseButton.Button6]; } }
    public bool Button7down { get { return _currentState[(int)MouseButton.Button7]; } }
    public bool Button8down { get { return _currentState[(int)MouseButton.Button8]; } }
    public bool Button9down { get { return _currentState[(int)MouseButton.Button9]; } }
    public bool LastButtondown { get { return _currentState[(int)MouseButton.LastButton]; } }

    #endregion

    #region Up Properties

    public bool LeftClickup { get { return !_currentState[(int)MouseButton.Left]; } }
    public bool MiddleClickup { get { return !_currentState[(int)MouseButton.Middle]; } }
    public bool RightClickup { get { return !_currentState[(int)MouseButton.Right]; } }

    public bool Button1up { get { return !_currentState[(int)MouseButton.Button1]; } }
    public bool Button2up { get { return !_currentState[(int)MouseButton.Button2]; } }
    public bool Button3up { get { return !_currentState[(int)MouseButton.Button3]; } }
    public bool Button4up { get { return !_currentState[(int)MouseButton.Button4]; } }
    public bool Button5up { get { return !_currentState[(int)MouseButton.Button5]; } }
    public bool Button6up { get { return !_currentState[(int)MouseButton.Button6]; } }
    public bool Button7up { get { return !_currentState[(int)MouseButton.Button7]; } }
    public bool Button8up { get { return !_currentState[(int)MouseButton.Button8]; } }
    public bool Button9up { get { return !_currentState[(int)MouseButton.Button9]; } }
    public bool LastButtonup { get { return !_currentState[(int)MouseButton.LastButton]; } }

    #endregion

    #region Released Properties

    public bool LeftClickreleased { get { return _previousState[(int)MouseButton.Left] && !_currentState[(int)MouseButton.Left]; } }
    public bool MiddleClickreleased { get { return _previousState[(int)MouseButton.Middle] && !_currentState[(int)MouseButton.Middle]; } }
    public bool RightClickreleased { get { return _previousState[(int)MouseButton.Right] && !_currentState[(int)MouseButton.Right]; } }

    public bool Button1released { get { return _previousState[(int)MouseButton.Button1] && !_currentState[(int)MouseButton.Button1]; } }
    public bool Button2released { get { return _previousState[(int)MouseButton.Button2] && !_currentState[(int)MouseButton.Button2]; } }
    public bool Button3released { get { return _previousState[(int)MouseButton.Button3] && !_currentState[(int)MouseButton.Button3]; } }
    public bool Button4released { get { return _previousState[(int)MouseButton.Button4] && !_currentState[(int)MouseButton.Button4]; } }
    public bool Button5released { get { return _previousState[(int)MouseButton.Button5] && !_currentState[(int)MouseButton.Button5]; } }
    public bool Button6released { get { return _previousState[(int)MouseButton.Button6] && !_currentState[(int)MouseButton.Button6]; } }
    public bool Button7released { get { return _previousState[(int)MouseButton.Button7] && !_currentState[(int)MouseButton.Button7]; } }
    public bool Button8released { get { return _previousState[(int)MouseButton.Button8] && !_currentState[(int)MouseButton.Button8]; } }
    public bool Button9released { get { return _previousState[(int)MouseButton.Button9] && !_currentState[(int)MouseButton.Button9]; } }
    public bool LastButtonreleased { get { return _previousState[(int)MouseButton.LastButton] && !_currentState[(int)MouseButton.LastButton]; } }

    #endregion
  }
}