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
//   - StackEnumerator
//   - StackException

using System;

namespace SevenEngine.DataStructures
{
  #region Stack

  /// <summary>Implements a First-In-Last-Out stack data structure.</summary>
  /// <typeparam name="Type">The type of objects to be placed in the stack.</typeparam>
  /// <remarks>The runtimes of each public member are included in the "remarks" xml tags.
  /// Seven (Zachary Patten) 10-12-13.</remarks>
  public class Stack<Type> : System.Collections.IEnumerable
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

    // The following "IEnumerable" functions allow you to use a "foreach"
    // loop on this AvlTree class.
    public System.Collections.IEnumerator GetEnumerator()
    { return new StackEnumerator(_top); }
    private class StackEnumerator : System.Collections.IEnumerator
    {
      private StackNode _head;
      private StackNode _current;

      public StackEnumerator(StackNode node) { _head = node; _current = new StackNode(default(Type), _head); }

      public object Current { get { return _current.Value; } }
      public void Reset() { _current = _head; }
      public bool MoveNext()
      { if (_current.Down != null) { _current = _current.Down; return true; } return false; }
    }

    /// <summary>Converts the list into a standard array.</summary>
    /// <returns>A standard array of all the items.</returns>
    /// /// <remarks>Runtime: Theta(n).</remarks>
    public Type[] ToArray()
    {
      if (_count == 0)
        return null;
      Type[] array = new Type[_count];
       StackNode looper = _top;
      for (int i = 0; i < _count; i++)
      {
        array[i] = looper.Value;
        looper = looper.Down;
      }
      return array;
    }

    /// <summary>This is used for throwing AVL Tree exceptions only to make debugging faster.</summary>
    private class StackException : Exception { public StackException(string message) : base(message) { } }
  }

  #endregion
}