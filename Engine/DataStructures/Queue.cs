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
//   - QueueException

using System;

namespace SevenEngine.DataStructures
{
  #region Queue

  /// <summary>Implements a first in last out queue data structure.</summary>
  /// <typeparam name="Type">The type of objects to be placed in the queue.</typeparam>
  /// <remarks>The runtimes of each public member are included in the "remarks" xml tags.
  /// Seven (Zachary Patten) 10-12-13.</remarks>
  public class Queue<Type>
  {
    #region QueueNode

    /// <summary>This class just holds the data for each individual node of the queue.</summary>
    private class QueueNode
    {
      private Type _value;
      private QueueNode _previous;
      private QueueNode _next;

      internal Type Value { get { return _value; } set { _value = value; } }
      internal QueueNode Previous { get { return _previous; } set { _previous = value; } }
      internal QueueNode Next { get { return _next; } set { _next = value; } }

      internal QueueNode(Type data, QueueNode previous, QueueNode next) 
      {
        _value = data;
        _previous = previous;
        _next = next;
      }
    }

    #endregion

    private QueueNode _front;
    private QueueNode _back;
    private int _count;

    /// <summary>Returns the number of items in the queue.</summary>
    /// <remarks>Runtime: O(1).</remarks>
    public int Count { get { return _count; } }

    /// <summary>Creates an instance of a queue.</summary>
    /// <remarks>Runtime: O(1).</remarks>
    public Queue()
    {
      _front = new QueueNode(default(Type), null, null);
      _back = new QueueNode(default(Type), null, null);
      _front.Next = _back;
      _back.Previous = _front;
      _count = 0;
    }

    /// <summary>Adds an item to the queue.</summary>
    /// <param name="addition">The item to add to the queue.</param>
    /// <remarks>Runtime: O(1).</remarks>
    public void Enqueue(Type addition)
    {
      QueueNode cell = new QueueNode(addition, _back.Previous, _back);
      _back.Previous = cell;
      cell.Previous.Next = cell;
      _count++;
    }

    /// <summary>Returns the oldest item in the queue without removing it.</summary>
    /// <returns>The oldest item in the queue.</returns>
    /// <remarks>Runtime: O(1).</remarks>
    public Type Peek()
    {
      if (_count == 0)
        throw new QueueException("Attempting to remove from an empty queue.");
      return _front.Next.Value;
    }

    /// <summary>Removes and returns the oldest item in the queue.</summary>
    /// <returns>The oldest item in the queue.</returns>
    /// <remarks>Runtime: O(1).</remarks>
    public Type Dequeue()
    {
      Type x = Peek();
      _front.Next = _front.Next.Next;
      _front.Next.Previous = _front;
      _count--;
      return x;
    }

    /// <summary>Clears the queue to an empty state.</summary>
    /// <remarks>Runtime: O(1), but causes considerably garbage collection.</remarks>
    public void Clear()
    {
      _front.Next = _back;
      _back.Previous = _front;
      _count = 0;
    }

    /// <summary>This is used for throwing AVL Tree exceptions only to make debugging faster.</summary>
    private class QueueException : Exception { public QueueException(string message) : base(message) { } }
  }

  #endregion
}