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
using System.Threading;
using SevenEngine.DataStructures;

namespace SevenEngine.DataStructures
{
  public interface Queue<Type> : DataStructure<Type>
  {
    void Enqueue(Type push);
    Type Peek();
    Type Dequeue();
    int Count { get; }
    bool IsEmpty { get; }
    void Clear();
  }

  #region QueueLinked

  /// <summary>Implements First-In-First-Out queue data structure.</summary>
  /// <remarks>The runtimes of each public member are included in the "remarks" xml tags.</remarks>
  [Serializable]
  public class QueueLinked<Type> : Queue<Type>
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

    /// <summary>Returns true if this structure is in an empty state.</summary>
    public bool IsEmpty { get { return _head == null; } }

    /// <summary>Creates an instance of a queue.</summary>
    /// <remarks>Runtime: O(1).</remarks>
    public QueueLinked()
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

    public Type Peek()
    {
      if (_head == null)
        throw new QueueException("Attempting to remove a non-existing id value.");
      Type returnValue = _head.Value;
      return returnValue;
    }

    /// <summary>Resets the queue to an empty state.</summary>
    /// <remarks>Runtime: O(1).</remarks>
    public void Clear()
    {
      _head = _tail = null;
      _count = 0;
    }

    /// <summary>Performs a functional paradigm newest-to-oldest traversal of the queue.</summary>
    /// <param name="traversalFunction">The function to perform each iteration.</param>
    /// <remarks>Runtime: O(n * foreachFunction).</remarks>
    public bool TraverseBreakable(Func<Type, bool> traversalFunction)
    {
      QueueNode looper = _head;
      while (looper != null)
      {
        if (!traversalFunction(looper.Value))
          return false;
        looper = looper.Next;
      }
      return true;
    }

    public void Traverse(Action<Type> traversalFunction)
    {
      QueueNode looper = _head;
      while (looper != null)
      {
        traversalFunction(looper.Value);
        looper = looper.Next;
      }
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

  #region QueueLinkedThreadSafe

  /// <summary>Implements First-In-First-Out queue data structure that inherits InterfaceTraversable. Thread-safe version.</summary>
  /// <remarks>The runtimes of each public member are included in the "remarks" xml tags.</remarks>
  [Serializable]
  public class QueueLinkedThreadSafe<Type> : Queue<Type>
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

    private object _lock;
    private int _readers;
    private int _writers;

    /// <summary>Returns the number of items in the queue.</summary>
    /// <remarks>Runtime: O(1).</remarks>
    public int Count { get { ReaderLock(); int count = _count; ReaderUnlock(); return count; } }

    public bool IsEmpty { get { ReaderLock(); bool isEmpty = _head == null; ReaderUnlock(); return isEmpty; } }

    /// <summary>Creates an instance of a queue.</summary>
    /// <remarks>Runtime: O(1).</remarks>
    public QueueLinkedThreadSafe()
    {
      _head = _tail = null;
      _count = 0;
      _lock = new object();
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

    public Type Peek()
    {
      ReaderLock();
      if (_head == null)
      {
        ReaderUnlock();
        throw new QueueException("Attempting to remove a non-existing id value.");
      }
      Type returnValue = _head.Value;
      ReaderUnlock();
      return returnValue;
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

    /// <summary>Performs a functional paradigm newest-to-oldest traversal of the queue.</summary>
    /// <param name="traversalFunction">The function to perform each iteration.</param>
    /// <remarks>Runtime: O(n * foreachFunction).</remarks>
    public bool TraverseBreakable(Func<Type, bool> traversalFunction)
    {
      ReaderLock();
      QueueNode looper = _head;
      while (looper != null)
      {
        if (!traversalFunction(looper.Value))
        {
          ReaderUnlock();
          return false;
        }
        looper = looper.Next;
      }
      ReaderUnlock();
      return true;
    }

    public void Traverse(Action<Type> traversalFunction)
    {
      ReaderLock();
      QueueNode looper = _head;
      while (looper != null)
      {
        traversalFunction(looper.Value);
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

  #region QueueArray

  /// <summary>Implements a growing list as an array (with expansions/contractions) 
  /// data structure that inherits InterfaceTraversable.</summary>
  /// <typeparam name="Type">The type of objects to be placed in the list.</typeparam>
  /// <remarks>The runtimes of each public member are included in the "remarks" xml tags.</remarks>
  [Serializable]
  public class QueueArray<Type> : Queue<Type>
  {
    private Type[] _queue;
    private int _start;
    private int _count;
    private int _minimumCapacity;

    /// <summary>Gets the number of items in the list.</summary>
    /// <remarks>Runtime: O(1).</remarks>
    public int Count
    {
      get
      {
        int returnValue = _count;
        return returnValue;
      }
    }

    /// <summary>Returns true if the structure is empty.</summary>
    /// <remarks>Runtime: O(1).</remarks>
    public bool IsEmpty { get { return _count == 0; } }

    /// <summary>Gets the current capacity of the list.</summary>
    /// <remarks>Runtime: O(1).</remarks>
    public int CurrentCapacity
    {
      get
      {
        int returnValue = _queue.Length;
        return returnValue;
      }
    }

    /// <summary>Allows you to adjust the minimum capacity of this list.</summary>
    /// <remarks>Runtime: O(n), Omega(1).</remarks>
    public int MinimumCapacity
    {
      get
      {
        int returnValue = _minimumCapacity;
        return returnValue;
      }
      set
      {
        if (value < 1)
          throw new ListArrayException("Attempting to set a minimum capacity to a negative or zero value.");
        else if (value > _queue.Length)
        {
          Type[] newList = new Type[value];
          _queue.CopyTo(newList, 0);
          _queue = newList;
        }
        else
          _minimumCapacity = value;
      }
    }

    /// <summary>Creates an instance of a ListArray, and sets it's minimum capacity.</summary>
    /// <param name="minimumCapacity">The initial and smallest array size allowed by this list.</param>
    /// <remarks>Runtime: O(1).</remarks>
    public QueueArray(int minimumCapacity)
    {
      _queue = new Type[minimumCapacity];
      _count = 0;
      _minimumCapacity = minimumCapacity;
    }

    /// <summary>Adds an item to the end of the list.</summary>
    /// <param name="addition">The item to be added.</param>
    /// <remarks>Runtime: O(n), EstAvg(1). </remarks>
    public void Enqueue(Type addition)
    {
      if (_count == _queue.Length)
      {
        if (_queue.Length > Int32.MaxValue / 2)
        {
          throw new ListArrayException("your queue is so large that it can no longer double itself (Int32.MaxValue barrier reached).");
        }
        Type[] newQueue = new Type[_queue.Length * 2];
        for (int i = 0; i < _count; i++)
          newQueue[i] = _queue[(i + _start) % _queue.Length];
        _start = 0;
        _queue = newQueue;
      }
      _queue[(_start + _count++) % _queue.Length] = addition;
    }

    /// <summary>Removes the item at a specific index.</summary>
    /// <remarks>Runtime: Theta(n - index).</remarks>
    public Type Dequeue()
    {
      if (_count == 0)
        throw new ListArrayException("attempting to dequeue from an empty queue.");
      if (_count < _queue.Length / 4 && _queue.Length / 2 > _minimumCapacity)
      {
        Type[] newQueue = new Type[_queue.Length / 2];
        for (int i = 0; i < _count; i++)
          newQueue[i] = _queue[(i + _start) % _queue.Length];
        _start = 0;
        _queue = newQueue;
      }
      Type returnValue = _queue[_start++];
      _count--;
      if (_count == 0)
        _start = 0;
      return returnValue;
    }

    public Type Peek()
    {
      Type returnValue = _queue[_start];
      return returnValue;
    }

    /// <summary>Empties the list back and reduces it back to its original capacity.</summary>
    /// <remarks>Runtime: O(1).</remarks>
    public void Clear()
    {
      _queue = new Type[_minimumCapacity];
      _count = 0;
    }

    /// <summary>Traverses the structure and performs a function on each entry.</summary>
    /// <param name="traversalFunction">The function within a foreach loop.</param>
    /// <remarks>Runtime: O(n * traversalFunction).</remarks>
    public bool TraverseBreakable(Func<Type, bool> traversalFunction)
    {
      for (int i = 0; i < _count; i++)
        if (!traversalFunction(_queue[i]))
          return false;
      return true;
    }

    /// <summary>Traverses the structure and performs a function on each entry.</summary>
    /// <param name="traversalFunction">The function within a foreach loop.</param>
    /// <param name="start">The index to start the traversal from.</param>
    /// <param name="end">The index to end the traversal at.</param>
    /// <remarks>Runtime: O((end - start) * traversalFunction).</remarks>
    public bool TraverseBreakable(Func<Type, bool> traversalFunction, int start, int end)
    {
      if (start < 0 || start < end || end > _count)
        throw new ListArrayException("invalid index parameters on traversal");
      for (int i = start; i < end; i++)
        if (!traversalFunction(_queue[i]))
          return false;
      return true;
    }

    /// <summary>Traverses the structure and performs an action on each entry.</summary>
    /// <param name="traversalAction">The action within a foreach loop.</param>
    /// <remarks>Runtime: O(n * traversalAction).</remarks>
    public void Traverse(Action<Type> traversalAction)
    {
      for (int i = 0; i < _count; i++) traversalAction(_queue[i]);
    }

    /// <summary>Traverses the structure and performs a function on each entry.</summary>
    /// <param name="traversalAction">The action within a foreach loop.</param>
    /// <param name="start">The index to start the traversal from.</param>
    /// <param name="end">The index to end the traversal at.</param>
    /// <remarks>Runtime: O((end - start) * traversalAction).</remarks>
    public void Traverse(Action<Type> traversalAction, int start, int end)
    {
      if (start < 0 || start < end || end > _count)
        throw new ListArrayException("invalid index parameters on traversal");
      for (int i = start; i < end; i++) traversalAction(_queue[i]);
    }

    /// <summary>Converts the list array into a standard array.</summary>
    /// <returns>A standard array of all the elements.</returns>
    public Type[] ToArray()
    {
      Type[] array = new Type[_count];
      for (int i = 0; i < _count; i++) array[i] = _queue[i];
      return array;
    }

    /// <summary>This is used for throwing AVL Tree exceptions only to make debugging faster.</summary>
    private class ListArrayException : Exception { public ListArrayException(string message) : base(message) { } }
  }

  #endregion

  #region QueueArrayThreadSafe

  /// <summary>Implements a growing list as an array (with expansions/contractions) 
  /// data structure that inherits InterfaceTraversable.</summary>
  /// <typeparam name="Type">The type of objects to be placed in the list.</typeparam>
  /// <remarks>The runtimes of each public member are included in the "remarks" xml tags.</remarks>
  [Serializable]
  public class QueueArrayThreadSafe<Type> : Queue<Type>
  {
    private Type[] _queue;
    private int _start;
    private int _count;
    private int _minimumCapacity;

    // This value determines the starting data structure size
    // at which my traversal functions will begin dynamic multithreading
    private object _lock;
    private int _readers;
    private int _writers;

    /// <summary>Gets the number of items in the list.</summary>
    /// <remarks>Runtime: O(1).</remarks>
    public int Count
    {
      get
      {
        ReaderLock();
        int returnValue = _count;
        ReaderUnlock();
        return returnValue;
      }
    }

    /// <summary>Returns true if the structure is empty.</summary>
    /// <remarks>Runtime: O(1).</remarks>
    public bool IsEmpty { get { return _count == 0; } }

    /// <summary>Gets the current capacity of the list.</summary>
    /// <remarks>Runtime: O(1).</remarks>
    public int CurrentCapacity
    {
      get
      {
        ReaderLock();
        int returnValue = _queue.Length;
        ReaderUnlock();
        return returnValue;
      }
    }

    /// <summary>Allows you to adjust the minimum capacity of this list.</summary>
    /// <remarks>Runtime: O(n), Omega(1).</remarks>
    public int MinimumCapacity
    {
      get
      {
        ReaderLock();
        int returnValue = _minimumCapacity;
        ReaderUnlock();
        return returnValue;
      }
      set
      {
        WriterLock();
        if (value < 1)
          throw new ListArrayException("Attempting to set a minimum capacity to a negative or zero value.");
        else if (value > _queue.Length)
        {
          Type[] newList = new Type[value];
          _queue.CopyTo(newList, 0);
          _queue = newList;
        }
        else
          _minimumCapacity = value;
        WriterUnlock();
      }
    }

    /// <summary>Creates an instance of a ListArray, and sets it's minimum capacity.</summary>
    /// <param name="minimumCapacity">The initial and smallest array size allowed by this list.</param>
    /// <remarks>Runtime: O(1).</remarks>
    public QueueArrayThreadSafe(int minimumCapacity)
    {
      _queue = new Type[minimumCapacity];
      _count = 0;
      _minimumCapacity = minimumCapacity;
      _lock = new object();
      _readers = 0;
      _writers = 0;
    }

    /// <summary>Adds an item to the end of the list.</summary>
    /// <param name="addition">The item to be added.</param>
    /// <remarks>Runtime: O(n), EstAvg(1). </remarks>
    public void Enqueue(Type addition)
    {
      WriterLock();
      if (_count == _queue.Length)
      {
        if (_queue.Length > Int32.MaxValue / 2)
        {
          WriterUnlock();
          throw new ListArrayException("your queue is so large that it can no longer double itself (Int32.MaxValue barrier reached).");
        }
        Type[] newQueue = new Type[_queue.Length * 2];
        for (int i = 0; i < _count; i++)
          newQueue[i] = _queue[(i + _start) % _queue.Length];
        _start = 0;
        _queue = newQueue;
      }
      _queue[(_start + _count++) % _queue.Length] = addition;
      WriterUnlock();
    }

    /// <summary>Removes the item at a specific index.</summary>
    /// <remarks>Runtime: Theta(n - index).</remarks>
    public Type Dequeue()
    {
      WriterLock();
      if (_count == 0)
        throw new ListArrayException("attempting to dequeue from an empty queue.");
      if (_count < _queue.Length / 4 && _queue.Length / 2 > _minimumCapacity)
      {
        Type[] newQueue = new Type[_queue.Length / 2];
        for (int i = 0; i < _count; i++)
          newQueue[i] = _queue[(i + _start) % _queue.Length];
        _start = 0;
        _queue = newQueue;
      }
      Type returnValue = _queue[_start++];
      _count--;
      if (_count == 0)
        _start = 0;
      WriterUnlock();
      return returnValue;
    }

    public Type Peek()
    {
      ReaderLock();
      Type returnValue = _queue[_start];
      ReaderUnlock();
      return returnValue;
    }

    /// <summary>Empties the list back and reduces it back to its original capacity.</summary>
    /// <remarks>Runtime: O(1).</remarks>
    public void Clear()
    {
      WriterLock();
      _queue = new Type[_minimumCapacity];
      _count = 0;
      WriterUnlock();
    }

    /// <summary>Traverses the structure and performs a function on each entry.</summary>
    /// <param name="traversalFunction">The function within a foreach loop.</param>
    /// <remarks>Runtime: O(n * traversalFunction).</remarks>
    public bool TraverseBreakable(Func<Type, bool> traversalFunction)
    {
      ReaderLock();
      for (int i = 0; i < _count; i++)
        if (!traversalFunction(_queue[i]))
        {
          ReaderUnlock();
          return false;
        }
      ReaderUnlock();
      return true;
    }

    /// <summary>Traverses the structure and performs a function on each entry.</summary>
    /// <param name="traversalFunction">The function within a foreach loop.</param>
    /// <param name="start">The index to start the traversal from.</param>
    /// <param name="end">The index to end the traversal at.</param>
    /// <remarks>Runtime: O((end - start) * traversalFunction).</remarks>
    public bool TraverseBreakable(Func<Type, bool> traversalFunction, int start, int end)
    {
      if (start < 0 || start < end || end > _count)
        throw new ListArrayException("invalid index parameters on traversal");
      ReaderLock();
      for (int i = start; i < end; i++)
        if (!traversalFunction(_queue[i]))
        {
          ReaderUnlock();
          return false;
        }
      ReaderUnlock();
      return true;
    }

    /// <summary>Traverses the structure and performs an action on each entry.</summary>
    /// <param name="traversalAction">The action within a foreach loop.</param>
    /// <remarks>Runtime: O(n * traversalAction).</remarks>
    public void Traverse(Action<Type> traversalAction)
    {
      ReaderLock();
      for (int i = 0; i < _count; i++) traversalAction(_queue[i]);
      ReaderUnlock();
    }

    /// <summary>Traverses the structure and performs a function on each entry.</summary>
    /// <param name="traversalAction">The action within a foreach loop.</param>
    /// <param name="start">The index to start the traversal from.</param>
    /// <param name="end">The index to end the traversal at.</param>
    /// <remarks>Runtime: O((end - start) * traversalAction).</remarks>
    public void Traverse(Action<Type> traversalAction, int start, int end)
    {
      if (start < 0 || start < end || end > _count)
        throw new ListArrayException("invalid index parameters on traversal");
      ReaderLock();
      for (int i = start; i < end; i++) traversalAction(_queue[i]);
      ReaderUnlock();
    }

    /// <summary>Converts the list array into a standard array.</summary>
    /// <returns>A standard array of all the elements.</returns>
    public Type[] ToArray()
    {
      ReaderLock();
      Type[] array = new Type[_count];
      for (int i = 0; i < _count; i++) array[i] = _queue[i];
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
    private class ListArrayException : Exception { public ListArrayException(string message) : base(message) { } }
  }

  #endregion
}