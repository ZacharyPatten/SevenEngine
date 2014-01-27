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
  public interface Stack<Type> : DataStructure<Type>
  {
    void Push(Type push);
    Type Peek();
    Type Pop();
  }

  #region StackLinked

  /// <summary>Implements a First-In-Last-Out stack data structure that inherits InterfaceTraversable.</summary>
  /// <typeparam name="Type">The generic type within the structure.</typeparam>
  /// <remarks>The runtimes of each public member are included in the "remarks" xml tags.
  /// Seven (Zachary Patten) 10-12-13.</remarks>
  [Serializable]
  public class StackLinked<Type> : Stack<Type>
  {
    #region StackLinkedNode

    /// <summary>This class just holds the data for each individual node of the stack.</summary>
    private class StackLinkedNode
    {
      private Type _value;
      private StackLinkedNode _down;

      internal Type Value { get { return _value; } set { _value = value; } }
      internal StackLinkedNode Down { get { return _down; } set { _down = value; } }

      internal StackLinkedNode(Type data, StackLinkedNode down) 
      {
        _value = data;
        _down = down;
      }
    }

    #endregion

    private StackLinkedNode _top;
    private int _count;

    private object _lock;
    private int _readers;
    private int _writers;

    /// <summary>Returns the number of items in the stack.</summary>
    /// <remarks>Runtime: O(1).</remarks>
    public int Count { get { ReaderLock(); int count = _count; ReaderUnlock(); return count; } }

    /// <summary>Creates an instance of a stack.</summary>
    /// <remarks>Runtime: O(1).</remarks>
    public StackLinked()
    {
      _top = null;
      _count = 0;
      _lock = new object();
    }

    /// <summary>Adds an item to the top of the stack.</summary>
    /// <param name="addition">The item to add to the stack.</param>
    /// <remarks>Runtime: O(1).</remarks>
    public void Push(Type addition)
    {
      WriterLock();
      _top = new StackLinkedNode(addition, _top);
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
        throw new StackLinkedException("Attempting to remove from an empty queue.");
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
    public bool TraverseBreakable(Func<Type, bool> traversalFunction)
    {
      ReaderLock();
      StackLinkedNode looper = _top;
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

    public void Traverse(Action<Type> traversalFunction)
    {
      ReaderLock();
      StackLinkedNode looper = _top;
      while (looper != null)
      {
        traversalFunction(looper.Value);
        looper = looper.Down;
      }
      ReaderUnlock();
    }

    /// <summary>Converts the list into a standard array.</summary>
    /// <returns>A standard array of all the items.</returns>
    /// <remarks>Runtime: Theta(n).</remarks>
    public Type[] ToArray()
    {
      if (_count == 0)
        return null;
      Type[] array = new Type[_count];
       StackLinkedNode looper = _top;
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
    private class StackLinkedException : Exception { public StackLinkedException(string message) : base(message) { } }
  }

  #endregion

  #region StackArray

  /// <summary>Implements a growing stack as an array (with expansions/contractions) data structure.</summary>
  /// <typeparam name="Type">The type of objects to be placed in the list.</typeparam>
  /// <remarks>The runtimes of each public member are included in the "remarks" xml tags.</remarks>
  [Serializable]
  public class StackArray<Type> : Stack<Type>
  {
    private Type[] _stack;
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
        int returnValue = _stack.Length;
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
        else if (value > _stack.Length)
        {
          Type[] newList = new Type[value];
          _stack.CopyTo(newList, 0);
          _stack = newList;
        }
        else
          _minimumCapacity = value;
        WriterUnlock();
      }
    }

    /// <summary>Creates an instance of a ListArray, and sets it's minimum capacity.</summary>
    /// <param name="minimumCapacity">The initial and smallest array size allowed by this list.</param>
    /// <remarks>Runtime: O(1).</remarks>
    public StackArray(int minimumCapacity)
    {
      _stack = new Type[minimumCapacity];
      _count = 0;
      _minimumCapacity = minimumCapacity;
      _lock = new object();
      _readers = 0;
      _writers = 0;
    }

    /// <summary>Adds an item to the end of the list.</summary>
    /// <param name="addition">The item to be added.</param>
    /// <remarks>Runtime: O(n), EstAvg(1). </remarks>
    public void Push(Type addition)
    {
      WriterLock();
      if (_count == _stack.Length)
      {
        if (_stack.Length > Int32.MaxValue / 2)
        {
          WriterUnlock();
          throw new ListArrayException("your queue is so large that it can no longer double itself (Int32.MaxValue barrier reached).");
        }
        Type[] newStack = new Type[_stack.Length * 2];
        for (int i = 0; i < _count; i++)
          newStack[i] = _stack[i];
        _stack = newStack;
      }
      _stack[_count++] = addition;
      WriterUnlock();
    }

    /// <summary>Removes the item at a specific index.</summary>
    /// <remarks>Runtime: Theta(n - index).</remarks>
    public Type Pop()
    {
      WriterLock();
      if (_count == 0)
        throw new ListArrayException("attempting to dequeue from an empty queue.");
      if (_count < _stack.Length / 4 && _stack.Length / 2 > _minimumCapacity)
      {
        Type[] newQueue = new Type[_stack.Length / 2];
        for (int i = 0; i < _count; i++)
          newQueue[i] = _stack[i];
        _stack = newQueue;
      }
      Type returnValue = _stack[--_count];
      WriterUnlock();
      return returnValue;
    }

    public Type Peek()
    {
      ReaderLock();
      Type returnValue = _stack[_count - 1];
      ReaderUnlock();
      return returnValue;
    }

    /// <summary>Empties the list back and reduces it back to its original capacity.</summary>
    /// <remarks>Runtime: O(1).</remarks>
    public void Clear()
    {
      WriterLock();
      _stack = new Type[_minimumCapacity];
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
        if (!traversalFunction(_stack[i]))
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
        if (!traversalFunction(_stack[i]))
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
      for (int i = 0; i < _count; i++) traversalAction(_stack[i]);
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
      for (int i = start; i < end; i++) traversalAction(_stack[i]);
      ReaderUnlock();
    }

    /// <summary>Converts the list array into a standard array.</summary>
    /// <returns>A standard array of all the elements.</returns>
    public Type[] ToArray()
    {
      ReaderLock();
      Type[] array = new Type[_count];
      for (int i = 0; i < _count; i++) array[i] = _stack[i];
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