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

// This file contains the following interfaces:
// - Heap
// This file contains the following classes:
// - HeapArrayStatic
//   - HeapArrayStaticLink
//   - HeapArrayStaticException
// - HeapArrayDynamic
//   - HeapArrayDynamicLink
//   - HeapArrayDynamicException

using System;
using System.Threading;
using SevenEngine.DataStructures.Interfaces;

namespace SevenEngine.DataStructures
{
  public interface Heap<Type> : InterfaceTraversable<Type>
  {
    int Count { get; }
    void Enqueue(Type addition, int priority);
    Type Dequeue();
    Type Peek();
    void Clear();
    bool IsEmpty { get; }
    Type[] ToArray();
  }

  #region HeapArrayStatic

  /// <summary>Implements a mutable priority heap with static priorities using an array.</summary>
  /// <typeparam name="Type">The type of item to be stored in this priority heap.</typeparam>
  /// <remarks>The runtimes of each public member are included in the "remarks" xml tags.</remarks>
  public class HeapArrayStatic<Type> : Heap<Type>
  {
    #region HeapArrayLink

    /// <summary>This is just a storage class, it stores an entry in the priority heap and its priority.</summary>
    private class HeapArrayLink
    {
      private int _priority;
      private Type _value;

      internal int Priority { get { return _priority; } set { _priority = value; } }
      internal Type Value { get { return _value; } set { _value = value; } }

      internal HeapArrayLink(int left, Type right)
      {
        _priority = left;
        _value = right;
      }
    }

    #endregion

    private HeapArrayLink[] _heapArray;
    private int _count;

    private Object _lock;
    private int _readers;
    private int _writers;

    /// <summary>The maximum items the queue can hold.</summary>
    /// <remarks>Runtime: O(1).</remarks>
    public int Capacity { get { return _heapArray.Length - 1; } }

    /// <summary>The number of items in the queue.</summary
    /// <remarks>Runtime: O(1).</remarks>
    public int Count { get { return _count; } }
    
    /// <summary>True if full, false if there is still room.</summary>
    /// <remarks>Runtime: O(1).</remarks>
    public bool IsFull { get { return _count == _heapArray.Length - 1; } }

    /// <summary>Returns true if the structure is empty.</summary>
    /// <remarks>Runtime: O(1).</remarks>
    public bool IsEmpty { get { return _count == 0; } }

    /// <summary>Generates a priority queue with a capacity of the parameter. Runtime O(1).</summary>
    /// <param name="capacity">The capacity you want this priority queue to have.</param>
    /// <remarks>Runtime: Theta(capacity).</remarks>
    public HeapArrayStatic(int capacity)
    {
      _heapArray = new HeapArrayLink[capacity + 1];
      _heapArray[0] = new HeapArrayLink(int.MaxValue, default(Type));
      for (int i = 1; i < capacity; i ++)
        _heapArray[i] = new HeapArrayLink(int.MinValue, default(Type));
      _count = 0;
      _lock = new Object();
    }

    /// <summary>Enqueue an item into the priority queue and let it works its magic.</summary>
    /// <param name="addition">The item to be added.</param>
    /// <param name="priority">The priority of the addition. (LARGER PRIORITY -> HIGHER PRIORITY)</param>
    /// <remarks>Runtime: O(ln(n)), Omega(1), EstAvg(ln(n)).</remarks>
    public void Enqueue(Type addition, int priority)
    {
      WriterLock();
      if (!(_count < _heapArray.Length - 1))
      {
        WriterUnlock();
        throw new HeapArrayStaticException("Attempting to add to a full priority queue.");
      }
      _count++;
      // Functionality Note: imutable or mutable (next three lines)
      //_queueArray[_count + 1] = new Link<int, Type>(priority, addition);
      _heapArray[_count].Priority = priority;
      _heapArray[_count].Value = addition;
      ShiftUp(_count);
      WriterUnlock();
    }

    /// <summary>Dequeues the item with the highest priority.</summary>
    /// <returns>The item of the highest priority.</returns>
    /// <remarks>Runtime: Theta(ln(n)).</remarks>
    public Type Dequeue()
    {
      WriterLock();
      if (_count > 0)
      {
        Type removal = _heapArray[1].Value;
        ArraySwap(1, _count);
        _count--;
        ShiftDown(1);
        WriterUnlock();
        return removal;
      }
      WriterUnlock();
      throw new HeapArrayStaticException("Attempting to remove from an empty priority queue.");
    }

    /// <summary>This lets you peek at the top priority WITHOUT REMOVING it.</summary>
    /// <remarks>Runtime: O(1).</remarks>
    public Type Peek()
    {
      ReaderLock();
      if (_count > 0)
      {
        ReaderUnlock();
        return _heapArray[1].Value;
      }
      ReaderUnlock();
      throw new HeapArrayStaticException("Attempting to peek at an empty priority queue.");
    }

    /// <summary>Standard priority queue algorithm for up sifting.</summary>
    /// <param name="index">The index to be up sifted.</param>
    /// <remarks>Runtime: O(ln(n)), Omega(1).</remarks>
    private void ShiftUp(int index)
    {
      // NOTE: "index * 2" is the index of the leftchild of the item at location "index"
      while (_heapArray[index].Priority > _heapArray[index / 2].Priority)
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
        if (((index * 2) + 1) <= _count && _heapArray[(index * 2) + 1].Priority < _heapArray[index].Priority) index2++;
        // NOTE: "(index * 2) + 1" is the index of the rightchild of the item at location "index"
        if (_heapArray[index].Priority >= _heapArray[index2].Priority) break;
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
      HeapArrayLink swapStorage = _heapArray[indexTwo];
      _heapArray[indexTwo] = _heapArray[indexOne];
      _heapArray[indexOne] = swapStorage;
    }

    /// <summary>Returns this queue to an empty state.</summary>
    /// <remarks>Runtime: O(1).</remarks>
    public void Clear() { WriterLock(); _count = 0; WriterUnlock(); }

    /// <summary>Traversal function for a heap. Following a pre-order traversal.</summary>
    /// <param name="traversalFunction">The function to perform per iteration.</param>
    /// <returns>A determining a break in the traversal. (true = continue, false = break)</returns>
    public bool TraverseBreakable(Func<Type, bool> traversalFunction) { return TraversalPreOrderBreakable(traversalFunction); }

    /// <summary>Traversal function for a heap. Following a pre-order traversal.</summary>
    /// <param name="traversalFunction">The function to perform per iteration.</param>
    public void Traverse(Action<Type> traversalFunction) { TraversalPreOrder(traversalFunction); }

    /// <summary>Implements an imperative traversal of the structure.</summary>
    /// <param name="traversalFunction">The function to perform per node in the traversal.</param>
    /// <remarks>Runtime: O(n * traversalFunction).</remarks>
    public bool TraversalPreOrderBreakable(Func<Type, bool> traversalFunction)
    {
      ReaderLock();
      for (int i = 0; i < _count; i++)
      {
        if (!traversalFunction(_heapArray[i].Value))
        {
          ReaderUnlock();
          return false;
        }
      }
      ReaderUnlock();
      return true;
    }

    /// <summary>Implements an imperative traversal of the structure.</summary>
    /// <param name="traversalAction">The action to perform per node in the traversal.</param>
    /// <remarks>Runtime: O(n * traversalAction).</remarks>
    public void TraversalPreOrder(Action<Type> traversalAction)
    {
      ReaderLock();
      for (int i = 0; i < _count; i++) traversalAction(_heapArray[i].Value);
      ReaderUnlock();
    }

    /// <summary>Converts the heap into an array using pre-order traversal (WARNING: items are not ordered).</summary>
    /// <returns>The array of priority-sorted items.</returns>
    public Type[] ToArray()
    {
      ReaderLock();
      Type[] array = new Type[_count];
      for (int i = 0; i < _count; i++) { array[i] = _heapArray[i].Value; }
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

    /// <summary>This is used for throwing imutable priority queue exceptions only to make debugging faster.</summary>
    private class HeapArrayStaticException : Exception { public HeapArrayStaticException(string message) : base(message) { } }
  }

  #endregion

  #region HeapArrayDynamic

  /// <summary>Implements a mutable priority heap with dynamic priorities using an array and a hash table.</summary>
  /// <typeparam name="Type">The type of item to be stored in this priority heap.</typeparam>
  /// <remarks>The runtimes of each public member are included in the "remarks" xml tags.</remarks>
  public class HeapArrayDynamic<Type> : Heap<Type>
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
    private HeapArrayDynamicLink[] _heapArray;
    private HashTableStandard<Type, int> _indexingReference;

    private Object _lock;
    private int _readers;
    private int _writers;

    /// <summary>The maximum items the queue can hold.</summary>
    /// <remarks>Runtime: O(1).</remarks>
    public int Capacity { get { ReaderLock(); int capacity = _heapArray.Length - 1; ReaderUnlock(); return capacity; } }

    /// <summary>The number of items in the queue.</summary
    /// <remarks>Runtime: O(1).</remarks>
    public int Count { get { ReaderLock(); int count = _count; ReaderUnlock(); return count; } }

    /// <summary>True if full, false if there is still room.</summary>
    /// <remarks>Runtime: O(1).</remarks>
    public bool IsFull { get { ReaderLock(); bool isFull = _count == _heapArray.Length - 1; ReaderUnlock(); return isFull; } }

    /// <summary>Returns true if the structure is empty.</summary>
    /// <remarks>Runtime: O(1).</remarks>
    public bool IsEmpty { get { return _count == 0; } }

    /// <summary>Generates a priority queue with a capacity of the parameter.</summary>
    /// <param name="capacity">The capacity you want this priority queue to have.</param>
    /// <remarks>Runtime: Theta(capacity).</remarks>
    public HeapArrayDynamic(int capacity)
    {
      _indexingReference = new HashTableStandard<Type, int>();
      _heapArray = new HeapArrayDynamicLink[capacity + 1];
      _heapArray[0] = new HeapArrayDynamicLink(int.MaxValue, default(Type));
      for (int i = 1; i < capacity; i++)
        _heapArray[0] = new HeapArrayDynamicLink(int.MinValue, default(Type));
      _count = 0;
      _lock = new Object();
    }

    /// <summary>Enqueue an item into the priority queue and let it works its magic.</summary>
    /// <param name="addition">The item to be added.</param>
    /// <param name="priority">The priority of the addition (LARGER PRIORITY -> HIGHER PRIORITY).</param>
    /// <remarks>Runtime: O(n), Omega(1), EstAvg(ln(n)).</remarks>
    public void Enqueue(Type addition, int priority)
    {
      WriterLock();
      if (!(_count < _heapArray.Length - 1))
      {
        WriterUnlock();
        throw new HeapArrayDynamicException("Attempting to add to a full priority queue.");
      }
      _count++;
      // Functionality Note: imutable or mutable (next three lines)
      //_queueArray[_count + 1] = new Link<int, Type>(priority, addition);
      _heapArray[_count].Priority = priority;
      _heapArray[_count].Value = addition;
      // Runtime Note: O(n) cause by hash table addition
      _indexingReference.Add(addition, _count);
      ShiftUp(_count);
      WriterUnlock();
    }

    /// <summary>This lets you peek at the top priority WITHOUT REMOVING it.</summary>
    /// <remarks>Runtime: O(1).</remarks>
    public Type Peek()
    {
      ReaderLock();
      if (_count > 0)
      {
        Type peek = _heapArray[1].Value;
        ReaderUnlock();
        return peek;
      }
      ReaderUnlock();
      throw new HeapArrayDynamicException("Attempting to peek at an empty priority queue.");
    }

    /// <summary>Dequeues the item with the highest priority.</summary>
    /// <returns>The item of the highest priority.</returns>
    /// <remarks>Runtime: O(n), Omega(ln(n)), EstAvg(ln(n)).</remarks>
    public Type Dequeue()
    {
      WriterLock();
      if (_count > 0)
      {
        Type removal = _heapArray[1].Value;
        ArraySwap(1, _count);
        _count--;
        // Runtime Note: O(n) caused by has table removal
        _indexingReference.Remove(removal);
        ShiftDown(1);
        WriterUnlock();
        return removal;
      }
      WriterUnlock();
      throw new HeapArrayDynamicException("Attempting to dequeue from an empty priority queue.");
    }

    /// <summary>Increases the priority of an item in the queue.</summary>
    /// <param name="item">The item to have its priority increased.</param>
    /// <param name="priority">The ammount to increase the priority by (LARGER INT -> HIGHER PRIORITY).</param>
    /// <remarks>Runtime: O(n), Omega(1), EstAvg(ln(n)).</remarks>
    public void IncreasePriority(Type item, int priority)
    {
      WriterLock();
      // Runtime Note: O(n) caused by hash table look-up.
      int index = _indexingReference[item];
      // Functionality Note: imutable or mutable (next two lines)
      //_queueArray[index] = new Link<int, Type>(_queueArray[index].Left + priority, item);
      _heapArray[index].Priority += priority;
      ShiftUp(index);
      WriterUnlock();
    }

    /// <summary>Decreases the priority of an item in the queue.</summary>
    /// <param name="item">The item to have its priority decreased.</param>
    /// <param name="priority">The ammount to decrease the priority by (LARGER INT -> HIGHER PRIORITY).</param>
    /// <remarks>Runtime: O(n), Omega(1), EstAvg(ln(n)).</remarks>
    public void DecreasePriority(Type item, int priority)
    {
      WriterLock();
      // Runtime Note: O(n) caused by hash table look-up.
      int index = _indexingReference[item];
      // Functionality Note: imutable or mutable (next two lines)
      //_queueArray[index] = new Link<int, Type>(_queueArray[index].Left - priority, item);
      _heapArray[index].Priority -= priority;
      ShiftDown(index);
      WriterUnlock();
    }

    /// <summary>Standard priority queue algorithm for up sifting.</summary>
    /// <param name="index">The index to be up sifted.</param>
    /// <remarks>Runtime: O(ln(n)), Omega(1).</remarks>
    private void ShiftUp(int index)
    {
      // NOTE: "index / 2" is the index of the parent of the item at location "index"
      while (_heapArray[index].Priority > _heapArray[index / 2].Priority)
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
        if (((index * 2) + 1) <= _count && _heapArray[(index * 2) + 1].Priority < _heapArray[index].Priority) index2++;
        // NOTE: "(index * 2) + 1" is the index of the rightchild of the item at location "index"
        if (_heapArray[index].Priority >= _heapArray[index2].Priority) break;
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
      HeapArrayDynamicLink swapStorage = _heapArray[indexTwo];
      _heapArray[indexTwo] = _heapArray[indexOne];
      _heapArray[indexOne] = swapStorage;
      _indexingReference[_heapArray[indexOne].Value] = indexOne;
      _indexingReference[_heapArray[indexTwo].Value] = indexTwo;
    }

    /// <summary>Returns this queue to an empty state.</summary>
    /// <remarks>Runtime: O(1).</remarks>
    public void Clear() { WriterLock(); _indexingReference.Clear(); _count = 0; WriterUnlock(); }

    /// <summary>Traversal function for a heap. Following a pre-order traversal.</summary>
    /// <param name="traversalFunction">The function to perform per iteration.</param>
    /// <returns>A determining a break in the traversal. (true = continue, false = break)</returns>
    public bool TraverseBreakable(Func<Type, bool> traversalFunction) { return TraversalPreOrderBreakable(traversalFunction); }

    /// <summary>Traversal function for a heap. Following a pre-order traversal.</summary>
    /// <param name="traversalFunction">The function to perform per iteration.</param>
    public void Traverse(Action<Type> traversalFunction) { TraversalPreOrder(traversalFunction); }

    /// <summary>Implements an imperative traversal of the structure.</summary>
    /// <param name="traversalFunction">The function to perform per node in the traversal.</param>
    /// <remarks>Runtime: O(n * traversalFunction).</remarks>
    public bool TraversalPreOrderBreakable(Func<Type, bool> traversalFunction)
    {
      ReaderLock();
      for (int i = 0; i < _count; i++)
      {
        if (!traversalFunction(_heapArray[i].Value))
        {
          ReaderUnlock();
          return false;
        }
      }
      ReaderUnlock();
      return true;
    }

    /// <summary>Implements an imperative traversal of the structure.</summary>
    /// <param name="traversalAction">The action to perform per node in the traversal.</param>
    /// <remarks>Runtime: O(n * traversalAction).</remarks>
    public void TraversalPreOrder(Action<Type> traversalAction)
    {
      ReaderLock();
      for (int i = 0; i < _count; i++) traversalAction(_heapArray[i].Value);
      ReaderUnlock();
    }

    /// <summary>Gets all the items in the heap. WARNING: the return items are NOT ordered.</summary>
    /// <returns>The items in the heap in random order.</returns>
    public Type[] ToArray()
    {
      ReaderLock();
      Type[] array = new Type[_count];
      for (int i = 0; i < _count; i++)
        array[i] = _heapArray[i].Value;
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

    /// <summary>This is used for throwing mutable priority queue exceptions only to make debugging faster.</summary>
    private class HeapArrayDynamicException : Exception { public HeapArrayDynamicException(string message) : base(message) { } }
  }

  #endregion
}