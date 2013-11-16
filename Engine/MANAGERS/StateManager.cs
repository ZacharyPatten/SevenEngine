// SEVENENGINE LISCENSE:
// You are free to use, modify, and distribute any or all code segments/files for any purpose
// including commercial use with the following condition: any code using or originally taken 
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
    private static AvlTree<IGameState> _stateDatabase = new AvlTree<IGameState>();

    private static IGameState _currentState = null;

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

    /// <summary>Adds a game state to the game</summary>
    /// <param name="stateId">What you want this state to be called so that you can access it.</param>
    /// <param name="state">The reference to the game state object to be added.</param>
    public static void AddState(IGameState state)
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

    /// <summary>Select the current state to be updated and rendered.</summary>
    /// <param name="stateId">The name associated with the state (what you caled it when you added it).</param>
    public static void ChangeState(string stateId)
    {
      if (!StateExists(stateId))
      {
        Output.ClearIndent();
        Output.WriteLine("ERROR!\nStateSystem.cs\\ChangeState(): " + stateId + "does not exits.");
        throw new StateSystemException("ERROR!\nStateSystem.cs\\ChangeState(): " + stateId + " does not exits.");
      }
      else
      {
        //_currentState = _stateDatabase[stateId];
        _currentState = _stateDatabase.Get(stateId);
        Output.WriteLine("\"" + stateId + "\" state selected;");
      }
    }

    /// <summary>Checks if a state exists (an example could be if a specific menu is already loaded then use it; if not then it needs to be loaded first).</summary>
    /// <param name="stateId">The name associated with the state (what you caled it when you added it).</param>
    /// <returns>"true if the state exists. "false""</returns>
    public static bool StateExists(string stateId)
    {
      //return _stateDatabase.ContainsKey(stateId);
      return _stateDatabase.Contains(stateId);
    }

    /// <summary>A unique class for throwing StateSystem exceptions only.</summary>
    private class StateSystemException : Exception { public StateSystemException(string message) : base(message) { } }
  }
}