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
// - Queue
//   - QueueNode
//   - QueueEnumerator
//   - QueueException

using System;

namespace SevenEngine.DataStructures
{
  #region Queue

  /// <summary>Implements First-In-First-Out queue data structure.</summary>
  /// <typeparam name="InterfaceStringId">The type of objects to be placed in the list.</typeparam>
  /// <remarks>The runtimes of each public member are included in the "remarks" xml tags.
  /// Seven (Zachary Patten) 10-12-13.</remarks>
  public class Queue<Type> : System.Collections.IEnumerable
  {
    #region QueueNode

    /// <summary>This class just holds the data for each individual node of the list.</summary>
    private class QueueNode
    {
      private Type _value;
      private QueueNode _next;

      internal Type Value { get { return _value; } set { _value = value; } }
      internal QueueNode Next { get { return _next; } set { _next = value; } }

      internal QueueNode(Type data) { _value = data; }
    }

    #endregion

    private QueueNode _head;
    private QueueNode _tail;
    private int _count;

    /// <summary>Returns the number of items in the queue.</summary>
    /// <remarks>Runtime: O(1).</remarks>
    public int Count { get { return _count; } }

    /// <summary>Creates an instance of a queue.</summary>
    /// <remarks>Runtime: O(1).</remarks>
    public Queue()
    {
      _head = _tail = null;
      _count = 0;
    }

    /// <summary>Adds an item to the back of the queue.</summary>
    /// <param name="enqueue">The item to add to the queue.</param>
    /// <remarks>Runtime: O(1).</remarks>
    public void Enqueue(Type enqueue)
    {
      if (_tail == null)
        _head = _tail = new QueueNode(enqueue);
      else
        _tail = _tail.Next = new QueueNode(enqueue);
      _count++;
    }

    /// <summary>Removes the oldest item in the queue.</summary>
    /// <remarks>Runtime: O(1).</remarks>
    public Type Dequeue()
    {
      if (_head == null)
        throw new QueueException("Attempting to remove a non-existing id value.");
      Type value = _head.Value;
      if (_head == _tail)
        _tail = null;
      _head = null;
      _count--;
      return value;
    }

    /// <summary>Resets the queue to an empty state.</summary>
    public void Clear()
    {
      _head = _tail = null;
      _count = 0;
    }

    /// <summary>A function to be used in a foreach loop.</summary>
    /// <param name="id">The id of the current node.</param>
    /// <param name="node">The current node of a foreach loop.</param>
    public delegate void ForeachFunction(Type node);
    /// <summary>Allows a foreach loop using a delegate.</summary>
    /// <param name="foreachFunction">The function within a foreach loop.</param>
    /// <remarks>Runtime: O(n * foreachFunction).</remarks>
    public void Foreach(ForeachFunction foreachFunction)
    {
      QueueNode looper = _head;
      while (looper != null)
      {
        foreachFunction(looper.Value);
        looper = looper.Next;
      }
    }

    // The following "IEnumerable" functions allow you to use a "foreach"
    // loop on this AvlTree class.
    public System.Collections.IEnumerator GetEnumerator()
    { return new ListEnumerator(_head); }
    private class ListEnumerator : System.Collections.IEnumerator
    {
      private QueueNode _head;
      private QueueNode _current;

      public ListEnumerator(QueueNode node)
      {
        _head = node;
        _current = new QueueNode(default(Type));
        _current.Next = _head;
      }

      public object Current { get { return _current.Value; } }
      public void Reset() { _current = _head; }
      public bool MoveNext()
      { if (_current.Next != null) { _current = _current.Next; return true; } return false; }
    }

    /// <summary>Converts the list into a standard array.</summary>
    /// <returns>A standard array of all the items.</returns>
    /// /// <remarks>Runtime: Theta(n).</remarks>
    public Type[] ToArray()
    {
      if (_count == 0)
        return null;
      Type[] array = new Type[_count];
      QueueNode looper = _head;
      for (int i = 0; i < _count; i++)
      {
        array[i] = looper.Value;
        looper = looper.Next;
      }
      return array;
    }

    /// <summary>This is used for throwing AVL Tree exceptions only to make debugging faster.</summary>
    private class QueueException : Exception { public QueueException(string message) : base(message) { } }
  }

  #endregion
}