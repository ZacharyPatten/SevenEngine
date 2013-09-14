using System;
using System.Collections.Generic;

namespace Engine
{
  public class StateSystem
  {
    Dictionary<string, IGameObject> _stateStore = new Dictionary<string, IGameObject>();
    IGameObject _currentState = null;

    public void Update(double elapsedTime)
    {
      if (_currentState == null)
        return; // nothing to process
      _currentState.Update(elapsedTime);
    }

    public void Render()
    {
      if (_currentState == null)
        return; // nothing to render
      _currentState.Render();
    }

    public void AddState(string stateId, IGameObject state)
    {
      if (StateExists(stateId))
      {
        Output.ClearIndent();
        Output.Print("ERROR!\nStateSystem.cs\\AddState(): " + stateId + " already exits.");
        throw new StateSystemException("ERROR!\nStateSystem.cs\\AddState(): " + stateId + " already exits.");
      }
      else
      {
        _stateStore.Add(stateId, state);
        Output.Print("\"" + stateId + "\" state loaded;");
      }
    }

    public void ChangeState(string stateId)
    {
      if (!StateExists(stateId))
      {
        Output.ClearIndent();
        Output.Print("ERROR!\nStateSystem.cs\\ChangeState(): " + stateId + "does not exits.");
        throw new StateSystemException("ERROR!\nStateSystem.cs\\ChangeState(): " + stateId + " does not exits.");
      }
      else
      {
        _currentState = _stateStore[stateId];
        Output.Print("\"" + stateId + "\" state selected;");
      }
    }

    public bool StateExists(string stateId)
    {
      return _stateStore.ContainsKey(stateId);
    }
  }

  class StateSystemException : Exception
  {
    public StateSystemException(string message) : base(message) { }
  }
}