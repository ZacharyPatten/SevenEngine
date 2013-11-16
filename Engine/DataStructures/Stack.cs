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

// This file contains the following classes:
// - Stack
//   - StackNode
//   - StackException
// This file has no external dependencies (other than "System" from .Net Framework).

// This file contains runtime values.
// All runtimes are in O-Notation. Here is a brief explanation:
// - "O(x)": the member has an upper bound of runtime equation "x"
// - "Omega(x)": the member has a lower bound of runtime equation "x"
// - "Theta(x)": the member has an upper and lower bound of runtime equation "x"
// - "EstAvg(x)": the runtime equation "x" to typically expect
// Notes: if the letter "n" is used, it typically means the current number of items within the structure

using System;

namespace SevenEngine.DataStructures
{
  #region Stack

  /// <summary>Implements a first in first out stack data structure.</summary>
  /// <typeparam name="Type">The type of objects to be placed in the stack.</typeparam>
  /// <remarks>The runtimes of each public member are included in the "remarks" xml tags.
  /// Seven (Zachary Patten) 10-12-13.</remarks>
  public class Stack<Type>
  {
    #region StackNode

    /// <summary>This class just holds the data for each individual node of the stack.</summary>
    private class StackNode
    {
      private Type _value;
      private StackNode _down;

      internal Type Value { get { return _value; } set { _value = value; } }
      internal StackNode Down { get { return _down; } set { _down = value; } }

      internal StackNode(Type data, StackNode down) 
      {
        _value = data;
        _down = down;
      }
    }

    #endregion

    private StackNode _top;
    private int _count;

    /// <summary>Returns the number of items in the stack.</summary>
    /// <remarks>Runtime: O(1).</remarks>
    public int Count { get { return _count; } }

    /// <summary>Creates an instance of a stack.</summary>
    /// <remarks>Runtime: O(1).</remarks>
    public Stack()
    {
      _top = null;
      _count = 0;
    }

    /// <summary>Adds an item to the top of the stack.</summary>
    /// <param name="addition">The item to add to the stack.</param>
    /// <remarks>Runtime: O(1).</remarks>
    public void Push(Type addition)
    {
      _top = new StackNode(addition, _top);
      _count++;
    }

    /// <summary>Returns the most recent addition to the stack.</summary>
    /// <returns>The most recent addition to the stack.</returns>
    /// <remarks>Runtime: O(1).</remarks>
    public Type Peek()
    {
      if (_top == null)
        throw new StackException("Attempting to remove from an empty queue.");
      return _top.Value;
    }

    /// <summary>Removes and returns the most recent addition to the stack.</summary>
    /// <returns>The most recent addition to the stack.</returns>
    /// <remarks>Runtime: O(1).</remarks>
    public Type Pop()
    {
      Type x = _top.Value;
      _top = _top.Down;
      _count--;
      return x;
    }

    /// <summary>Clears the stack to an empty state.</summary>
    /// <remarks>Runtime: O(1). Note: causes considerable garbage collection.</remarks>
    public void Clear()
    {
      _top = null;
      _count = 0;
    }

    /// <summary>This is used for throwing AVL Tree exceptions only to make debugging faster.</summary>
    private class StackException : Exception { public StackException(string message) : base(message) { } }
  }

  #endregion
}