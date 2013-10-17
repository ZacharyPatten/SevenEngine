// This file contains the following classes:
// - HeapArrayStatic
//   - HeapArrayStaticLink
//   - PriorityHeapArrayStaticException
// - HeapArrayDynamic
//   - HeapArrayDynamicLink
//   - HeapArrayDynamicException
// External Dependencies (other than "System" from .Net Framework):
// - HeapArrayDynamic requires HashTable

// This file contains runtime and space values.
// All runtimes are in O-Notation. Here is a brief explanation:
// - "O(x)": the member has an upper bound of runtime equation "x"
// - "Omega(x)": the member has a lower bound of runtime equation "x"
// - "Theta(x)": the member has an upper and lower bound of runtime equation "x"
// - "EstAvg(x)": the runtime equation "x" to typically expect
// Notes: if the letter "n" is used, it typically means the current number of items within the structure

// Written by Seven (Zachary Aaron Patten)
// Last Edited on date 10-12-13
// Feel free to use this code in any manor you see fit.
// However, please cite me because I put quite a bit of time into it.
// - Thanks. :)

using System;

namespace Engine.DataStructures
{
  #region PriorityHeapArrayStatic

  /// <summary>Implements a mutable priority heap with static priorities using an array.</summary>
  /// <typeparam name="Type">The type of item to be stored in this priority heap.</typeparam>
  /// <remarks>The runtimes of each public member are included in the "remarks" xml tags. 
  /// Seven (Zachary Patten) 10-12-13.</remarks>
  public class HeapArrayStatic<Type>
  {
    #region HeapArrayStaticLink

    /// <summary>This is just a storage class, it stores an entry in the priority heap and its priority.</summary>
    private class HeapArrayStaticLink
    {
      private int _priority;
      private Type _value;

      internal int Priority { get { return _priority; } set { _priority = value; } }
      internal Type Value { get { return _value; } set { _value = value; } }

      internal HeapArrayStaticLink(int left, Type right)
      {
        _priority = left;
        _value = right;
      }
    }

    #endregion

    private int _count;
    private HeapArrayStaticLink[] _queueArray;

    /// <summary>The maximum items the queue can hold.</summary>
    /// <remarks>Runtime: O(1).</remarks>
    public int Capacity { get { return _queueArray.Length - 1; } }

    /// <summary>The number of items in the queue.</summary
    /// <remarks>Runtime: O(1).</remarks>
    public int Count { get { return _count; } }
    
    /// <summary>True if full, false if there is still room.</summary>
    /// <remarks>Runtime: O(1).</remarks>
    public bool IsFull { get { return _count == _queueArray.Length - 1; } }

    /// <summary>Generates a priority queue with a capacity of the parameter. Runtime O(1).</summary>
    /// <param name="capacity">The capacity you want this priority queue to have.</param>
    /// <remarks>Runtime: Theta(capacity).</remarks>
    public HeapArrayStatic(int capacity)
    {
      _queueArray = new HeapArrayStaticLink[capacity + 1];
      _queueArray[0] = new HeapArrayStaticLink(int.MaxValue, default(Type));
      for (int i = 1; i < capacity; i ++)
        _queueArray[i] = new HeapArrayStaticLink(int.MinValue, default(Type));
      _count = 0;
    }

    /// <summary>Enqueue an item into the priority queue and let it works its magic.</summary>
    /// <param name="addition">The item to be added.</param>
    /// <param name="priority">The priority of the addition. (LARGER PRIORITY -> HIGHER PRIORITY)</param>
    /// <remarks>Runtime: O(ln(n)), Omega(1), EstAvg(ln(n)).</remarks>
    public void Enqueue(Type addition, int priority)
    {
      if (!(_count < _queueArray.Length - 1))
        throw new HeapArrayStaticException("Attempting to add to a full priority queue.");
      _count++;
      // Functionality Note: imutable or mutable (next three lines)
      //_queueArray[_count + 1] = new Link<int, Type>(priority, addition);
      _queueArray[_count].Priority = priority;
      _queueArray[_count].Value = addition;
      ShiftUp(_count);
    }

    /// <summary>Dequeues the item with the highest priority.</summary>
    /// <returns>The item of the highest priority.</returns>
    /// <remarks>Runtime: Theta(ln(n)).</remarks>
    public Type Dequeue()
    {
      if (_count > 0)
      {
        Type removal = _queueArray[1].Value;
        ArraySwap(1, _count);
        _count--;
        ShiftDown(1);
        return removal;
      }
      else throw new HeapArrayStaticException("Attempting to remove from an empty priority queue.");
    }

    /// <summary>This lets you peek at the top priority WITHOUT REMOVING it.</summary>
    /// <remarks>Runtime: O(1).</remarks>
    public int Peek()
    {
      if (_count > 0)
        return _queueArray[1].Priority;
      else throw new HeapArrayStaticException("Attempting to peek at an empty priority queue.");
    }

    /// <summary>Standard priority queue algorithm for up sifting.</summary>
    /// <param name="index">The index to be up sifted.</param>
    /// <remarks>Runtime: O(ln(n)), Omega(1).</remarks>
    private void ShiftUp(int index)
    {
      // NOTE: "index * 2" is the index of the leftchild of the item at location "index"
      while (_queueArray[index].Priority > _queueArray[index / 2].Priority)
      {
        ArraySwap(index, index / 2);
        index = index / 2;
      }
    }

    /// <summary>Standard priority queue algorithm for sifting down.</summary>
    /// <param name="index">The index to be down sifted.</param>
    /// <remarks>Runtime: O(ln(n)), Omega(1).</remarks>
    private void ShiftDown(int index)
    {
      // NOTE: "index * 2" is the index of the leftchild of the item at location "index"
      while ((index * 2) <= _count)
      {
        int index2 = index * 2;
        if (((index * 2) + 1) <= _count && _queueArray[(index * 2) + 1].Priority < _queueArray[index].Priority) index2++;
        // NOTE: "(index * 2) + 1" is the index of the rightchild of the item at location "index"
        if (_queueArray[index].Priority >= _queueArray[index2].Priority) break;
        ArraySwap(index, index2);
        index = index2;
      }
    }

    /// <summary>Standard array swap method.</summary>
    /// <param name="indexOne">The first index of the swap.</param>
    /// <param name="indexTwo">The second index of the swap.</param>
    /// <remarks>Runtime: O(1).</remarks>
    private void ArraySwap(int indexOne, int indexTwo)
    {
      HeapArrayStaticLink swapStorage = _queueArray[indexTwo];
      _queueArray[indexTwo] = _queueArray[indexOne];
      _queueArray[indexOne] = swapStorage;
    }

    /// <summary>Returns this queue to an empty state.</summary>
    /// <remarks>Runtime: O(1).</remarks>
    public void Clear() { _count = 0; }

    /// <summary>This is used for throwing imutable priority queue exceptions only to make debugging faster.</summary>
    private class HeapArrayStaticException : Exception { public HeapArrayStaticException(string message) : base(message) { } }
  }

  #endregion

  #region PriorityHeapDynamic

  /// <summary>Implements a mutable priority heap with dynamic priorities using an array and a hash table.</summary>
  /// <typeparam name="Type">The type of item to be stored in this priority heap.</typeparam>
  /// <remarks>The runtimes of each public member are included in the "remarks" xml tags.
  /// Seven (Zachary Patten) 10-12-13.</remarks>
  public class HeapArrayDynamic<Type>
  {
    #region HeapArrayDynamicLink

    /// <summary>This is just a storage class, it stores an entry in the priority heap and its priority.</summary>
    private class HeapArrayDynamicLink
    {
      private int _priority;
      private Type _value;

      internal int Priority { get { return _priority; } set { _priority = value; } }
      internal Type Value { get { return _value; } set { _value = value; } }

      internal HeapArrayDynamicLink(int left, Type right)
      {
        _priority = left;
        _value = right;
      }
    }

    #endregion

    private int _count;
    private HeapArrayDynamicLink[] _queueArray;
    private HashTable<Type, int> _indexingReference;

    /// <summary>The maximum items the queue can hold.</summary>
    /// <remarks>Runtime: O(1).</remarks>
    public int Capacity { get { return _queueArray.Length - 1; } }

    /// <summary>The number of items in the queue.</summary
    /// <remarks>Runtime: O(1).</remarks>
    public int Count { get { return _count; } }

    /// <summary>True if full, false if there is still room.</summary>
    /// <remarks>Runtime: O(1).</remarks>
    public bool IsFull { get { return _count == _queueArray.Length - 1; } }

    /// <summary>Generates a priority queue with a capacity of the parameter.</summary>
    /// <param name="capacity">The capacity you want this priority queue to have.</param>
    /// <remarks>Runtime: Theta(capacity).</remarks>
    public HeapArrayDynamic(int capacity)
    {
      _indexingReference = new HashTable<Type, int>();
      _queueArray = new HeapArrayDynamicLink[capacity + 1];
      _queueArray[0] = new HeapArrayDynamicLink(int.MaxValue, default(Type));
      for (int i = 1; i < capacity; i++)
        _queueArray[0] = new HeapArrayDynamicLink(int.MinValue, default(Type));
      _count = 0;
    }

    /// <summary>Enqueue an item into the priority queue and let it works its magic.</summary>
    /// <param name="addition">The item to be added.</param>
    /// <param name="priority">The priority of the addition (LARGER PRIORITY -> HIGHER PRIORITY).</param>
    /// <remarks>Runtime: O(n), Omega(1), EstAvg(ln(n)).</remarks>
    public void Enqueue(Type addition, int priority)
    {
      if (!(_count < _queueArray.Length - 1))
        throw new HeapArrayDynamicException("Attempting to add to a full priority queue.");
      _count++;
      // Functionality Note: imutable or mutable (next three lines)
      //_queueArray[_count + 1] = new Link<int, Type>(priority, addition);
      _queueArray[_count].Priority = priority;
      _queueArray[_count].Value = addition;
      // Runtime Note: O(n) cause by hash table addition
      _indexingReference.Add(addition, _count);
      ShiftUp(_count);
    }

    /// <summary>This lets you peek at the top priority WITHOUT REMOVING it.</summary>
    /// <remarks>Runtime: O(1).</remarks>
    public int Peek()
    {
      if (_count > 0)
        return _queueArray[1].Priority;
      else throw new HeapArrayDynamicException("Attempting to peek at an empty priority queue.");
    }

    /// <summary>Dequeues the item with the highest priority.</summary>
    /// <returns>The item of the highest priority.</returns>
    /// <remarks>Runtime: O(n), Omega(ln(n)), EstAvg(ln(n)).</remarks>
    public Type Dequeue()
    {
      if (_count > 0)
      {
        Type removal = _queueArray[1].Value;
        ArraySwap(1, _count);
        _count--;
        // Runtime Note: O(n) caused by has table removal
        _indexingReference.Remove(removal);
        ShiftDown(1);
        return removal;
      }
      else throw new HeapArrayDynamicException("Attempting to dequeue from an empty priority queue.");
    }

    /// <summary>Increases the priority of an item in the queue.</summary>
    /// <param name="item">The item to have its priority increased.</param>
    /// <param name="priority">The ammount to increase the priority by (LARGER INT -> HIGHER PRIORITY).</param>
    /// <remarks>Runtime: O(n), Omega(1), EstAvg(ln(n)).</remarks>
    public void IncreasePriority(Type item, int priority)
    {
      // Runtime Note: O(n) caused by hash table look-up.
      int index = _indexingReference[item];
      // Functionality Note: imutable or mutable (next two lines)
      //_queueArray[index] = new Link<int, Type>(_queueArray[index].Left + priority, item);
      _queueArray[index].Priority += priority;
      ShiftUp(index);
    }

    /// <summary>Decreases the priority of an item in the queue.</summary>
    /// <param name="item">The item to have its priority decreased.</param>
    /// <param name="priority">The ammount to decrease the priority by (LARGER INT -> HIGHER PRIORITY).</param>
    /// <remarks>Runtime: O(n), Omega(1), EstAvg(ln(n)).</remarks>
    public void DecreasePriority(Type item, int priority)
    {
      // Runtime Note: O(n) caused by hash table look-up.
      int index = _indexingReference[item];
      // Functionality Note: imutable or mutable (next two lines)
      //_queueArray[index] = new Link<int, Type>(_queueArray[index].Left - priority, item);
      _queueArray[index].Priority -= priority;
      ShiftDown(index);
    }

    /// <summary>Standard priority queue algorithm for up sifting.</summary>
    /// <param name="index">The index to be up sifted.</param>
    /// <remarks>Runtime: O(ln(n)), Omega(1).</remarks>
    private void ShiftUp(int index)
    {
      // NOTE: "index / 2" is the index of the parent of the item at location "index"
      while (_queueArray[index].Priority > _queueArray[index / 2].Priority)
      {
        ArraySwap(index, index / 2);
        index = index / 2;
      }
    }

    /// <summary>Standard priority queue algorithm for sifting down.</summary>
    /// <param name="index">The index to be down sifted.</param>
    /// <remarks>Runtime: O(ln(n)), Omega(1).</remarks>
    private void ShiftDown(int index)
    {
      // NOTE: "index * 2" is the index of the leftchild of the item at location "index"
      while ((index * 2) <= _count)
      {
        int index2 = index * 2;
        if (((index * 2) + 1) <= _count && _queueArray[(index * 2) + 1].Priority < _queueArray[index].Priority) index2++;
        // NOTE: "(index * 2) + 1" is the index of the rightchild of the item at location "index"
        if (_queueArray[index].Priority >= _queueArray[index2].Priority) break;
        ArraySwap(index, index2);
        index = index2;
      }
    }

    /// <summary>Standard array swap method.</summary>
    /// <param name="indexOne">The first index of the swap.</param>
    /// <param name="indexTwo">The second index of the swap.</param>
    /// <remarks>Runtime: O(1).</remarks>
    private void ArraySwap(int indexOne, int indexTwo)
    {
      HeapArrayDynamicLink swapStorage = _queueArray[indexTwo];
      _queueArray[indexTwo] = _queueArray[indexOne];
      _queueArray[indexOne] = swapStorage;
      _indexingReference[_queueArray[indexOne].Value] = indexOne;
      _indexingReference[_queueArray[indexTwo].Value] = indexTwo;
    }

    /// <summary>This is used for throwing mutable priority queue exceptions only to make debugging faster.</summary>
    private class HeapArrayDynamicException : Exception { public HeapArrayDynamicException(string message) : base(message) { } }
  }

  #endregion
}