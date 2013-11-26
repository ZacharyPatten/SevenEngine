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

// This file contains the following classes:
// - Stack
//   - StackNode
//   - StackException

using System;
using System.Threading;
using SevenEngine.DataStructures.Interfaces;

namespace SevenEngine.DataStructures
{
  #region Stack

  /// <summary>Implements a First-In-Last-Out stack data structure that inherits InterfaceTraversable.</summary>
  /// <typeparam name="Type">The generic type within the structure.</typeparam>
  /// <remarks>The runtimes of each public member are included in the "remarks" xml tags.
  /// Seven (Zachary Patten) 10-12-13.</remarks>
  public class Stack<Type> : InterfaceTraversable<Type>
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

    private Object _lock;
    private int _readers;
    private int _writers;

    /// <summary>Returns the number of items in the stack.</summary>
    /// <remarks>Runtime: O(1).</remarks>
    public int Count { get { ReaderLock(); int count = _count; ReaderUnlock(); return count; } }

    /// <summary>Creates an instance of a stack.</summary>
    /// <remarks>Runtime: O(1).</remarks>
    public Stack()
    {
      _top = null;
      _count = 0;
      _lock = new Object();
    }

    /// <summary>Adds an item to the top of the stack.</summary>
    /// <param name="addition">The item to add to the stack.</param>
    /// <remarks>Runtime: O(1).</remarks>
    public void Push(Type addition)
    {
      WriterLock();
      _top = new StackNode(addition, _top);
      _count++;
      WriterUnlock();
    }

    /// <summary>Returns the most recent addition to the stack.</summary>
    /// <returns>The most recent addition to the stack.</returns>
    /// <remarks>Runtime: O(1).</remarks>
    public Type Peek()
    {
      ReaderLock();
      if (_top == null)
      {
        ReaderUnlock();
        throw new StackException("Attempting to remove from an empty queue.");
      }
      Type peek = _top.Value;
      ReaderUnlock();
      return peek;
    }

    /// <summary>Removes and returns the most recent addition to the stack.</summary>
    /// <returns>The most recent addition to the stack.</returns>
    /// <remarks>Runtime: O(1).</remarks>
    public Type Pop()
    {
      WriterLock();
      Type x = _top.Value;
      _top = _top.Down;
      _count--;
      WriterUnlock();
      return x;
    }

    /// <summary>Clears the stack to an empty state.</summary>
    /// <remarks>Runtime: O(1). Note: causes considerable garbage collection.</remarks>
    public void Clear()
    {
      WriterLock();
      _top = null;
      _count = 0;
      WriterUnlock();
    }

    /// <summary>Performs a functional paradigm top-to-bottom traversal of the stack.</summary>
    /// <param name="traversalFunction">The function to perform each iteration.</param>
    /// <remarks>Runtime: O(n * foreachFunction).</remarks>
    public bool Traversal(Func<Type, bool> traversalFunction)
    {
      ReaderLock();
      StackNode looper = _top;
      while (looper != null)
      {
        if (!traversalFunction(looper.Value))
        {
          ReaderUnlock();
          return false;
        }
        looper = looper.Down;
      }
      ReaderUnlock();
      return true;
    }

    /// <summary>Converts the list into a standard array.</summary>
    /// <returns>A standard array of all the items.</returns>
    /// <remarks>Runtime: Theta(n).</remarks>
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

    /// <summary>Thread safe enterance for readers.</summary>
    private void ReaderLock() { lock (_lock) { while (!(_writers == 0)) Monitor.Wait(_lock); _readers++; } }
    /// <summary>Thread safe exit for readers.</summary>
    private void ReaderUnlock() { lock (_lock) { _readers--; Monitor.Pulse(_lock); } }
    /// <summary>Thread safe enterance for writers.</summary>
    private void WriterLock() { lock (_lock) { while (!(_writers == 0) && !(_readers == 0)) Monitor.Wait(_lock); _writers++; } }
    /// <summary>Thread safe exit for readers.</summary>
    private void WriterUnlock() { lock (_lock) { _writers--; Monitor.PulseAll(_lock); } }

    /// <summary>This is used for throwing AVL Tree exceptions only to make debugging faster.</summary>
    private class StackException : Exception { public StackException(string message) : base(message) { } }
  }

  #endregion
}