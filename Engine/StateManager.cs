using System;
using System.Collections.Generic;

namespace Engine
{
  public static class StateManager
  {
    private static Dictionary<string, IGameState> _stateDatabase = new Dictionary<string, IGameState>();

    private static IGameState _currentState = null;

    /// <summary>Calls the "Update()" function for the current state relative to the timespan since the last update in SECONDS.</summary>
    /// <param name="elapsedTime">The time since the last update call in SECONDS.</param>
    public static void Update(double elapsedTime)
    {
      if (_currentState == null) return;
      _currentState.Update(elapsedTime);
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
    public static void AddState(string stateId, IGameState state)
    {
      if (StateExists(stateId))
      {
        Output.ClearIndent();
        Output.Write("ERROR!\nStateSystem.cs\\AddState(): " + stateId + " already exits.");
        throw new StateSystemException("ERROR!\nStateSystem.cs\\AddState(): " + stateId + " already exits.");
      }
      else
      {
        _stateDatabase.Add(stateId, state);
        Output.Write("\"" + stateId + "\" state loaded;");
      }
    }

    /// <summary>Select the current state to be updated and rendered.</summary>
    /// <param name="stateId">The name associated with the state (what you caled it when you added it).</param>
    public static void ChangeState(string stateId)
    {
      if (!StateExists(stateId))
      {
        Output.ClearIndent();
        Output.Write("ERROR!\nStateSystem.cs\\ChangeState(): " + stateId + "does not exits.");
        throw new StateSystemException("ERROR!\nStateSystem.cs\\ChangeState(): " + stateId + " does not exits.");
      }
      else
      {
        _currentState = _stateDatabase[stateId];
        Output.Write("\"" + stateId + "\" state selected;");
      }
    }

    /// <summary>Checks if a state exists (an example could be if a specific menu is already loaded then use it; if not then it needs to be loaded first).</summary>
    /// <param name="stateId">The name associated with the state (what you caled it when you added it).</param>
    /// <returns>"true if the state exists. "false""</returns>
    public static bool StateExists(string stateId)
    {
      return _stateDatabase.ContainsKey(stateId);
    }
  }

  internal class StateSystemException : Exception
  {
    public StateSystemException(string message) : base(message) { }
  }
}