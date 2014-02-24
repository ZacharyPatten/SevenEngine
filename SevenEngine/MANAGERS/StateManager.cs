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

using System;
using SevenEngine.DataStructures;

namespace SevenEngine
{
  /// <summary>StateManager is used for is used for state management (loading, storing).</summary>
  public static class StateManager
  {
    private static Func<InterfaceGameState, InterfaceGameState, int> _comparison =
      (InterfaceGameState left, InterfaceGameState right) => { return left.Id.CompareTo(right.Id); };

    private static Func<InterfaceGameState, string, int> _keyComparison =
      (InterfaceGameState left, string right) => { return left.Id.CompareTo(right); };

    private static AvlTree<InterfaceGameState> _stateDatabase = new AvlTreeLinked<InterfaceGameState>(_comparison);

    private static InterfaceGameState _currentState = null;

    /// <summary>Calls the "Update()" function for the current state relative to the timespan since the last update in SECONDS.</summary>
    /// <param name="elapsedTime">The time since the last update call in SECONDS.</param>
    public static string Update(float elapsedTime)
    {
      if (_currentState == null) return "Don't Change States";
      return _currentState.Update(elapsedTime);
    }

    /// <summary>Calls the render function of the current state.</summary>
    public static void Render()
    {
      if (_currentState == null) return;
      _currentState.Render();
    }

    /// <summary>Gets a reference to a state.</summary>
    /// <param name="stateId">The name associated with that desired state.</param>
    /// <returns>The desired state.</returns>
    public static InterfaceGameState GetState(string stateId) { return _stateDatabase.Get<string>(stateId, _keyComparison); }

    /// <summary>Tries to get a desired state, but returns a bool rather than crashing.</summary>
    /// <param name="stateId">The name of the state to get.</param>
    /// <param name="state">The reference to the state.</param>
    /// <returns>Whether or not it could get the value.</returns>
    public static bool TryGet(string stateId, out InterfaceGameState state)
    {
       return _stateDatabase.TryGet<string>(stateId, _keyComparison, out state);

    }

    /// <summary>Adds a game state to the game</summary>
    /// <param name="stateId">What you want this state to be called so that you can access it.</param>
    /// <param name="state">The reference to the game state object to be added.</param>
    public static void AddState(InterfaceGameState state)
    {
      if (StateExists(state.Id))
      {
        Output.ClearIndent();
        Output.WriteLine("ERROR!\nStateSystem.cs\\AddState(): " + state.Id + " already exits.");
        throw new StateSystemException("ERROR!\nStateSystem.cs\\AddState(): " + state.Id + " already exits.");
      }
      else
      {
        _stateDatabase.Add(state);
        Output.WriteLine("\"" + state.Id + "\" state loaded;");
      }
    }

    /// <summary>Triggers the load method of a state.</summary>
    /// <param name="stateId">The name of the state to trigger the Load.</param>
    public static void TriggerStateLoad(string stateId)
    {
      _stateDatabase.Get<string>(stateId, _keyComparison).Load();
    }

    /// <summary>Select the current state to be updated and rendered.</summary>
    /// <param name="stateId">The name associated with the state (what you caled it when you added it).</param>
    public static void ChangeState(string stateId)
    {
      InterfaceGameState state;
      if (!TryGet(stateId, out state))
      {
        Output.ClearIndent();
        Output.WriteLine("ERROR: state \"" + stateId + "\" does not exits.");
        throw new StateSystemException("Attempting to change states to a non-existent state \"" + stateId + "\".");
      }
      else if (!state.IsReady)
      {
        Output.ClearIndent();
        Output.WriteLine("ERROR: state \"" + stateId + "\" is not ready to become the current state.");
        throw new StateSystemException("Attempting to change states to a state that has not been fully loaded \"" + stateId + "\".");
      }
      else
      {
        _currentState = _stateDatabase.Get<string>(stateId, _keyComparison);
        Output.WriteLine("\"" + stateId + "\" state selected;");
      }
    }

    /// <summary>Checks if a state exists (an example could be if a specific menu is already loaded then use it; if not then it needs to be loaded first).</summary>
    /// <param name="stateId">The name associated with the state (what you caled it when you added it).</param>
    /// <returns>"true if the state exists. "false""</returns>
    public static bool StateExists(string stateId)
    {
      return _stateDatabase.Contains<string>(stateId, _keyComparison);
    }

    /// <summary>A unique class for throwing StateSystem exceptions only.</summary>
    private class StateSystemException : Exception { public StateSystemException(string message) : base(message) { } }
  }
}