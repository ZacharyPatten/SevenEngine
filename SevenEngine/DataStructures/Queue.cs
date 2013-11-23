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
// - Queue
//   - QueueNode
//   - QueueException

using System;
using System.Threading;
using SevenEngine.DataStructures.Interfaces;

namespace SevenEngine.DataStructures
{
  #region Queue

  /// <summary>Implements First-In-First-Out queue data structure that inherits InterfaceTraversable.</summary>
  /// <remarks>The runtimes of each public member are included in the "remarks" xml tags.</remarks>
  public class Queue<Type> : InterfaceTraversable<Type>
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

    private Object _lock;
    private int _readers;
    private int _writers;

    /// <summary>Returns the number of items in the queue.</summary>
    /// <remarks>Runtime: O(1).</remarks>
    public int Count { get { ReaderLock(); int count = _count; ReaderUnlock(); return count; } }

    /// <summary>Creates an instance of a queue.</summary>
    /// <remarks>Runtime: O(1).</remarks>
    public Queue()
    {
      _head = _tail = null;
      _count = 0;
      _lock = new Object();
    }

    /// <summary>Adds an item to the back of the queue.</summary>
    /// <param name="enqueue">The item to add to the queue.</param>
    /// <remarks>Runtime: O(1).</remarks>
    public void Enqueue(Type enqueue)
    {
      WriterLock();
      if (_tail == null)
        _head = _tail = new QueueNode(enqueue);
      else
        _tail = _tail.Next = new QueueNode(enqueue);
      _count++;
      WriterUnlock();
    }

    /// <summary>Removes the oldest item in the queue.</summary>
    /// <remarks>Runtime: O(1).</remarks>
    public Type Dequeue()
    {
      WriterLock();
      if (_head == null)
      {
        WriterUnlock();
        throw new QueueException("Attempting to remove a non-existing id value.");
      }
      Type value = _head.Value;
      if (_head == _tail)
        _tail = null;
      _head = null;
      _count--;
      WriterUnlock();
      return value;
    }

    /// <summary>Resets the queue to an empty state.</summary>
    /// <remarks>Runtime: O(1).</remarks>
    public void Clear()
    {
      WriterLock();
      _head = _tail = null;
      _count = 0;
      WriterUnlock();
    }

    public delegate bool TraversalFunction(Type node);
    /// <summary>Allows a foreach loop using a delegate.</summary>
    /// <param name="foreachFunction">The function within a foreach loop.</param>
    /// <remarks>Runtime: O(n * foreachFunction).</remarks>
    public void Traversal(Func<Type, bool> foreachFunction)
    {
      ReaderLock();
      QueueNode looper = _head;
      while (looper != null)
      {
        foreachFunction(looper.Value);
        looper = looper.Next;
      }
      ReaderUnlock();
    }

    /// <summary>Converts the list into a standard array.</summary>
    /// <returns>A standard array of all the items.</returns>
    /// /// <remarks>Runtime: Theta(n).</remarks>
    public Type[] ToArray()
    {
      ReaderLock();
      if (_count == 0)
        return null;
      Type[] array = new Type[_count];
      QueueNode looper = _head;
      for (int i = 0; i < _count; i++)
      {
        array[i] = looper.Value;
        looper = looper.Next;
      }
      ReaderUnlock();
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
    private class QueueException : Exception { public QueueException(string message) : base(message) { } }
  }

  #endregion
}