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
// - List
//   - ListNode
//   - ListException
// - ListArray
//   - ListArrayException

using System;
using System.Threading;
using SevenEngine;
using SevenEngine.DataStructures.Interfaces;

namespace SevenEngine.DataStructures
{
  public interface List<Type>
  {
    void Add(Type addition);
    void RemoveFirst(Type removal);
    int Count { get; }
    void Clear();
    bool TraverseBreakable(Func<Type, bool> traversalFunction);
    void Traverse(Action<Type> traversalAction);
  }

  #region List

  /// <summary>Implements a growing, singularly-linked list data structure that inherits InterfaceTraversable.</summary>
  /// <typeparam name="InterfaceStringId">The type of objects to be placed in the list.</typeparam>
  /// <remarks>The runtimes of each public member are included in the "remarks" xml tags.</remarks>
  public class ListLinked<ValueType, KeyType> : InterfaceTraversable<ValueType>, List<ValueType>
    //where Type : InterfaceStringId
  {
    #region ListNode

    /// <summary>This class just holds the data for each individual node of the list.</summary>
    private class ListNode
    {
      private ValueType _value;
      private ListNode _next;

      internal ValueType Value { get { return _value; } set { _value = value; } }
      internal ListNode Next { get { return _next; } set { _next = value; } }

      internal ListNode(ValueType data) { _value = data; }
    }

    #endregion

    Func<ValueType, KeyType, bool> _comparisonFunction;

    private ListNode _head;
    private ListNode _tail;
    private int _count;

    private Object _lock;
    private int _readers;
    private int _writers;

    /// <summary>Returns the number of items in the list.</summary>
    /// <remarks>Runtime: O(1).</remarks>
    public int Count { get { ReaderLock(); int count = _count; ReaderUnlock(); return count; } }

    /// <summary>Creates an instance of a stalistck.</summary>
    /// <remarks>Runtime: O(1).</remarks>
    public ListLinked(Func<ValueType, KeyType, bool> comparisonFunction)
    {
      _comparisonFunction = comparisonFunction;
      _head = _tail = null;
      _count = 0;
      _lock = new Object();
      _readers = 0;
      _writers = 0;
    }

    /// <summary>Adds an item to the list.</summary>
    /// <param name="id">The string id of the item to add to the list.</param>
    /// <param name="addition">The item to add to the list.</param>
    /// <remarks>Runtime: O(1).</remarks>
    public void Add(ValueType addition)
    {
      WriterLock();
      if (_tail == null)
        _head = _tail = new ListNode(addition);
      else
        _tail = _tail.Next = new ListNode(addition);
      _count++;
      WriterUnlock();
    }

    /// <summary>Removes an item from the list with the matching string id.</summary>
    /// <param name="removalId">The string id of the item to remove.</param>
    /// <remarks>Runtime: O(n).</remarks>
    public void Remove(KeyType removalKey)
    {
      WriterLock();
      if (_head == null)
        throw new ListException("Attempting to remove a non-existing id value.");
      if (_comparisonFunction(_head.Value, removalKey))
      {
        _head = _head.Next;
        _count--;
        WriterUnlock();
        return;
      }
      ListNode listNode = _head;
      while (listNode != null)
      {
        if (listNode.Next == null)
        {
          WriterUnlock();
          throw new ListException("Attempting to remove a non-existing id value.");
        }
        else if (_comparisonFunction(_head.Value, removalKey))
        {
          if (listNode.Next.Equals(_tail))
            _tail = listNode;
          listNode.Next = listNode.Next.Next;
          WriterUnlock();
          return;
        }
        else
          listNode = listNode.Next;
      }
      WriterUnlock();
      throw new ListException("Attempting to remove a non-existing id value.");
    }

    /// <summary>Removes the first equality by object reference.</summary>
    /// <param name="removal">The reference to the item to remove.</param>
    public void RemoveFirst(ValueType removal)
    {
      WriterLock();
      if (_head == null)
        throw new ListException("Attempting to remove a non-existing id value.");
      if (_head.Value.Equals(removal))
      {
        _head = _head.Next;
        _count--;
        WriterUnlock();
        return;
      }
      ListNode listNode = _head;
      while (listNode != null)
      {
        if (listNode.Next == null)
        {
          WriterUnlock();
          throw new ListException("Attempting to remove a non-existing id value.");
        }
        else if (_head.Value.Equals(removal))
        {
          if (listNode.Next.Equals(_tail))
            _tail = listNode;
          listNode.Next = listNode.Next.Next;
          WriterUnlock();
          return;
        }
        else
          listNode = listNode.Next;
      }
      WriterUnlock();
      throw new ListException("Attempting to remove a non-existing id value.");
    }

    /*/// <summary>Allows you to rename an entry within this list.</summary>
    /// <param name="oldName">The id of the list entry to rename.</param>
    /// <param name="newName">The new id to apply to the node.</param>
    /// <remarks>Runtime: Theta(n).</remarks>
    public void RenameEntry(KeyType oldName, KeyType newName)
    {
      WriterLock();
      ListNode looper = _head;
      ListNode rename = null;
      while (looper != null)
      {
        if (_comparisonFunction(looper.Value, newName))
        {
          WriterUnlock();
          throw new ListException("Attempting to rename a list entry to an already existing id.");
        }
        if (looper.Value.Id == oldName)
          rename = looper;
        looper = looper.Next;
      }
      rename.Value.Id = newName;
      WriterUnlock();
    }*/

    /// <summary>Resets the list to an empty state. WARNING could cause excessive garbage collection.</summary>
    public void Clear()
    {
      WriterLock();
      _head = _tail = null;
      _count = 0;
      WriterUnlock();
    }

    /// <summary>Allows a foreach loop using a delegate.</summary>
    /// <param name="traversalFunction">The function to perform on each iteration.</param>
    /// <remarks>Runtime: O(n * traversalFunction).</remarks>
    public bool TraverseBreakable(Func<ValueType, bool> traversalFunction)
    {
      ReaderLock();
      ListNode looper = _head;
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

    /// <summary>Does an imperative traversal of the structure.</summary>
    /// <param name="traversalAction">The action to perform on each iteration.</param>
    /// <remarks>Runtime: O(n * traversalAction).</remarks>
    public void Traverse(Action<ValueType> traversalAction)
    {
      ReaderLock();
      ListNode looper = _head;
      while (looper != null)
      {
        traversalAction(looper.Value);
        looper = looper.Next;
      }
      ReaderUnlock();
    }

    /// <summary>Converts the list into a standard array.</summary>
    /// <returns>A standard array of all the items.</returns>
    /// /// <remarks>Runtime: Theta(n).</remarks>
    public ValueType[] ToArray()
    {
      ReaderLock();
      if (_count == 0)
      {
        ReaderUnlock();
        return null;
      }
      ValueType[] array = new ValueType[_count];
      ListNode looper = _head;
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
    private class ListException : Exception { public ListException(string message) : base(message) { } }
  }

  #endregion

  #region ListArray
  
  /// <summary>Implements a growing list as an array (with expansions/contractions) 
  /// data structure that inherits InterfaceTraversable.</summary>
  /// <typeparam name="Type">The type of objects to be placed in the list.</typeparam>
  /// <remarks>The runtimes of each public member are included in the "remarks" xml tags.</remarks>
  public class ListArray<Type> : InterfaceTraversable<Type>, List<Type>
  {
    private Type[] _list;
    private int _count;
    private int _minimumCapacity;

    // This value determines the starting data structure size
    // at which my traversal functions will begin dynamic multithreading
    private Object _lock;
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

    /// <summary>Gets the current capacity of the list.</summary>
    /// <remarks>Runtime: O(1).</remarks>
    public int CurrentCapacity
    {
      get
      {
        ReaderLock();
        int returnValue = _list.Length;
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
        else if (value > _list.Length)
        {
          Type[] newList = new Type[value];
          _list.CopyTo(newList, 0);
          _list = newList;
        }
        else
          _minimumCapacity = value;
        WriterUnlock();
      }
    }

    /// <summary>Look-up and set an indexed item in the list.</summary>
    /// <param name="index">The index of the item to get or set.</param>
    /// <returns>The value at the given index.</returns>
    public Type this[int index]
    {
      get
      {
        ReaderLock();
        if (index < 0 || index > _count)
        {
          ReaderUnlock();
          throw new ListArrayException("Attempting an index look-up outside the ListArray's current size.");
        }
        Type returnValue = _list[index];
        ReaderUnlock();
        return returnValue;
      }
      set
      {
        WriterLock();
        if (index < 0 || index > _count)
        {
          WriterUnlock();
          throw new ListArrayException("Attempting an index assignment outside the ListArray's current size.");
        }
        _list[index] = value;
        WriterUnlock();
      }
    }

    /// <summary>Creates an instance of a ListArray, and sets it's minimum capacity.</summary>
    /// <param name="minimumCapacity">The initial and smallest array size allowed by this list.</param>
    /// <remarks>Runtime: O(1).</remarks>
    public ListArray(int minimumCapacity)
    {
      _list = new Type[minimumCapacity];
      _count = 0;
      _minimumCapacity = minimumCapacity;
      _lock = new Object();
      _readers = 0;
      _writers = 0;
    }

    /// <summary>Adds an item to the end of the list.</summary>
    /// <param name="addition">The item to be added.</param>
    /// <remarks>Runtime: O(n), EstAvg(1). </remarks>
    public void Add(Type addition)
    {
      WriterLock();
      if (_count == _list.Length)
      {
        if (_list.Length > Int32.MaxValue / 2)
          throw new ListArrayException("Your list is so large that it can no longer double itself (Int32.MaxValue barrier reached).");
        Type[] newList = new Type[_list.Length * 2];
        _list.CopyTo(newList, 0);
        _list = newList;
      }
      _list[_count++] = addition;
      WriterUnlock();
    }

    /// <summary>Removes the item at a specific index.</summary>
    /// <param name="index">The index of the item to be removed.</param>
    /// <remarks>Runtime: Theta(n - index).</remarks>
    public void Remove(int index)
    {
      WriterLock();
      if (index < 0 || index > _count)
        throw new ListArrayException("Attempting to remove an index outside the ListArray's current size.");
      if (_count < _list.Length / 4 && _list.Length / 2 > _minimumCapacity)
      {
        Type[] newList = new Type[_list.Length / 2];
        for (int i = 0; i < _count; i++)
          newList[i] = _list[i];
        _list = newList;
      }
      for (int i = index; i < _count; i++)
        _list[i] = _list[i + 1];
      _count--;
      WriterUnlock();
    }

    /// <summary>Removes the first equality by object reference.</summary>
    /// <param name="removal">The reference to the item to remove.</param>
    public void RemoveFirst(Type removal)
    {
      WriterLock();
      for (int index = 0; index < _count; index++)
        if (_list[index].Equals(removal))
        {
          if (_count < _list.Length / 4 && _list.Length / 2 > _minimumCapacity)
          {
            Type[] newList = new Type[_list.Length / 2];
            for (int i = 0; i < _count; i++)
              newList[i] = _list[i];
            _list = newList;
          }
          for (int i = index; i < _count; i++)
            _list[i] = _list[i + 1];
          _count--;
          WriterUnlock();
          return;
        }
      WriterUnlock();
      throw new ListArrayException("attempting to remove a non-existing value.");
    }

    /// <summary>Empties the list back and reduces it back to its original capacity.</summary>
    /// <remarks>Runtime: O(1).</remarks>
    public void Clear()
    {
      WriterLock();
      _list = new Type[_minimumCapacity];
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
        if (!traversalFunction(_list[i]))
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
        if (!traversalFunction(_list[i]))
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
      for (int i = 0; i < _count; i++) traversalAction(_list[i]);
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
      for (int i = start; i < end; i++) traversalAction(_list[i]);
      ReaderUnlock();
    }

    /// <summary>Converts the list array into a standard array.</summary>
    /// <returns>A standard array of all the elements.</returns>
    public Type[] ToArray()
    {
      ReaderLock();
      Type[] array = new Type[_count];
      for (int i = 0; i < _count; i++) array[i] = _list[i];
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