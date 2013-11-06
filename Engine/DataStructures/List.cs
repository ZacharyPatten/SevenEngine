// SEVENENGINE LISCENSE:
// You are free to use, modify, and distribute any or all code segments/files for any purpose
// including commercial use with the following condition: any code using or originally taken from the 
// SevenEngine project must include citation to its original author(s) located at the top of each
// source code file. Alternatively, you may include a reference to the SevenEngine project as a whole,
// but you must include the current SevenEngine official website URL and logo.
// - Thanks.  :)  (support: seven@sevenengine.com)

// Author(s):
// - Zachary Aaron Patten (aka Seven) seven@sevenengine.com
// Last Edited: 10-26-13

// This file contains the following classes:
// - List
//   - ListNode
//   - ListException
// - ListArray
//   - ListArrayException
// This file has no external dependencies (other than "System" from .Net Framework).

// This file contains runtime values.
// All runtimes are in O-Notation. Here is a brief explanation:
// - "O(x)": the member has an upper bound of runtime equation "x"
// - "Omega(x)": the member has a lower bound of runtime equation "x"
// - "Theta(x)": the member has an upper and lower bound of runtime equation "x"
// - "EstAvg(x)": the runtime equation "x" to typically expect
// Notes: if the letter "n" is used, it typically means the current number of items within the structure

using System;
using System.Runtime.CompilerServices;
using SevenEngine.DataStructures.Interfaces;

namespace SevenEngine.DataStructures
{
  #region List

  #region InterfaceStringId
  //// This interface is in the "Interfaces" folder, but here it is if you
  //// can't find it:
  // public interface InterfaceStringId
  // {
  //   string Id { get; set; }
  // }
  #endregion

  /// <summary>Implements a growing, singularly-linked list data structure.</summary>
  /// <typeparam name="InterfaceStringId">The type of objects to be placed in the list.</typeparam>
  /// <remarks>The runtimes of each public member are included in the "remarks" xml tags.
  /// Seven (Zachary Patten) 10-12-13.</remarks>
  public class List<Type> where Type : InterfaceStringId
  {
    #region ListNode

    /// <summary>This class just holds the data for each individual node of the list.</summary>
    private class ListNode
    {
      private Type _value;
      private ListNode _next;

      internal Type Value { get { return _value; } set { _value = value; } }
      internal ListNode Next { get { return _next; } set { _next = value; } }

      internal ListNode(Type data) { _value = data; }
    }

    #endregion

    private ListNode _head;
    private ListNode _tail;
    private int _count;

    /// <summary>Returns the number of items in the list.</summary>
    /// <remarks>Runtime: O(1).</remarks>
    public int Count { get { return _count; } }

    /// <summary>Creates an instance of a stalistck.</summary>
    /// <remarks>Runtime: O(1).</remarks>
    public List()
    {
      _head = _tail = null;
      _count = 0;
    }

    /// <summary>Adds an item to the list.</summary>
    /// <param name="id">The string id of the item to add to the list.</param>
    /// <param name="addition">The item to add to the list.</param>
    /// <remarks>Runtime: O(1).</remarks>
    public void Add(Type addition)
    {
      if (_tail == null)
        _head = _tail = new ListNode(addition);
      else
        _tail = _tail.Next = new ListNode(addition);
      _count++;
    }

    /// <summary>Removes an item from the list with the matching string id.</summary>
    /// <param name="removalId">The string id of the item to remove.</param>
    /// <remarks>Runtime: O(n).</remarks>
    public void Remove(string removalId)
    {
      if (_head == null)
        throw new ListException("Attempting to remove a non-existing id value.");
      if (_head.Value.Id == removalId)
      {
        _head = _head.Next;
        _count--;
        return;
      }
      ListNode listNode = _head;
      while (listNode != null)
      {
        if (listNode.Next == null)
          throw new ListException("Attempting to remove a non-existing id value.");
        else if (listNode.Next.Value.Id == removalId)
        {
          if (listNode.Next.Equals(_tail))
            _tail = listNode;
          listNode.Next = listNode.Next.Next;
        }
        else
          listNode = listNode.Next;
      }
      throw new ListException("Attempting to remove a non-existing id value.");
    }

    /// <summary>Allows you to rename an entry within this list.</summary>
    /// <param name="oldName">The id of the list entry to rename.</param>
    /// <param name="newName">The new id to apply to the node.</param>
    /// <remarks>Runtime: Theta(n).</remarks>
    public void RenameEntry(string oldName, string newName)
    {
      ListNode looper = _head;
      ListNode rename = null;
      while (looper != null)
      {
        if (looper.Value.Id == newName)
          throw new ListException("Attempting to rename a list entry to an already existing id.");
        if (looper.Value.Id == oldName)
          rename = looper;
        looper = looper.Next;
      }
      rename.Value.Id = newName;
    }

    /// <summary>Resets the list to an empty state. WARNING could cause excessive garbage collection.</summary>
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
      ListNode looper = _head;
      while (looper != null)
      {
        foreachFunction(looper.Value);
        looper = looper.Next;
      }
    }

    /// <summary>How how want to clone each item during an entire list clone.</summary>
    /// <param name="currentid">The current id of the node.</param>
    /// <param name="node">A single node in teh list.</param>
    /// <param name="newId">The desired new id in the cloned list.</param>
    /// <param name="newNode">The cloned item to be used in the cloned list.</param>
    public delegate Type CloneFunction(Type node);
    /// <summary>Allows the user to clone the list however they choose (reference clone vs value clone).</summary>
    /// <param name="cloneFunction">he function to perform on each node during cloning.</param>
    /// <returns>The resulting cloned list.</returns>
    /// <remarks>Runtime: O(n * cloneFunction).</remarks>
    public List<Type> Clone(CloneFunction cloneFunction)
    {
      List<Type> listClone = new List<Type>();
      ListNode looper = _head;
      while (looper != null)
      {
        listClone.Add(cloneFunction(looper.Value));
        looper = looper.Next;
      }
      return listClone;
    }

    /// <summary>This is used for throwing AVL Tree exceptions only to make debugging faster.</summary>
    private class ListException : Exception { public ListException(string message) : base(message) { } }
  }

  #endregion

  #region List

  /// <summary>Implements a growing, singularly-linked list data structure.</summary>
  /// <typeparam name="Type">The type of objects to be placed in the list.</typeparam>
  /// <remarks>The runtimes of each public member are included in the "remarks" xml tags.
  /// Seven (Zachary Patten) 10-12-13.</remarks>
  /*public class List<Type>
  {
    #region ListNode

    /// <summary>This class just holds the data for each individual node of the list.</summary>
    private class ListNode
    {
      private string _id;
      private Type _value;
      private ListNode _next;

      internal string Id { get { return _id; } set { _id = value; } }
      internal Type Value { get { return _value; } set { _value = value; } }
      internal ListNode Next { get { return _next; } set { _next = value; } }

      internal ListNode(string id, Type data, ListNode down)
      {
        _id = id;
        _value = data;
        _next = down;
      }
    }

    #endregion

    private ListNode _head;
    private ListNode _tail;
    // This iterator node is no longer used (it was used for the commented function at the end of this class)
    //private ListNode _currentIterator;
    private int _count;

    /// <summary>Returns the number of items in the list.</summary>
    /// <remarks>Runtime: O(1).</remarks>
    public int Count { get { return _count; } }

    /// <summary>Creates an instance of a stalistck.</summary>
    /// <remarks>Runtime: O(1).</remarks>
    public List()
    {
      _head = null;
      _tail = null;
      _count = 0;
    }

    /// <summary>Adds an item to the list.</summary>
    /// <param name="id">The string id of the item to add to the list.</param>
    /// <param name="addition">The item to add to the list.</param>
    /// <remarks>Runtime: O(1).</remarks>
    public void Add(string id, Type addition)
    {
      if (_tail == null)
        _tail = new ListNode(id, addition, null);
      else
      {
        _tail.Next = new ListNode(id, addition, null);
        _tail = _tail.Next;
      }
      if (_head == null)
        _head = _tail;
      _count++;
    }

    /// <summary>Removes an item from the list with the matching string id.</summary>
    /// <param name="removalId">The string id of the item to remove.</param>
    /// <remarks>Runtime: O(n).</remarks>
    public void Remove(string removalId)
    {
      Remove(removalId, _head);
      _count--;
    }

    /// <summary>Removes an item from the list with the matching string id.</summary>
    /// <param name="removalId">The string id of the item to remove.</param>
    /// <param name="listNode">The current location during recursion.</param>
    /// <remarks>Runtime: O(n).</remarks>
    private void Remove(string removalId, ListNode listNode)
    {
      if (listNode == null)
        throw new ListException("Attempting to remove a non-existing id value.");
      else if (listNode.Id == removalId)
        _head = _head.Next;
      else if (listNode.Next == null)
        throw new ListException("Attempting to remove a non-existing id value.");
      else if (listNode.Next.Id == removalId)
      {
        if (listNode.Next.Equals(_tail))
          _tail = listNode;
        listNode.Next = listNode.Next.Next;
      }
      else
        Remove(removalId, listNode.Next);
    }

    /// <summary>Allows you to rename an entry within this list.</summary>
    /// <param name="oldName">The id of the list entry to rename.</param>
    /// <param name="newName">The new id to apply to the node.</param>
    /// <remarks>Runtime: Theta(n).</remarks>
    public void RenameEntry(string oldName, string newName)
    {
      ListNode looper = _head;
      ListNode rename = null;
      while (looper != null)
      {
        if (looper.Id == newName)
          throw new ListException("Attempting to rename a list entry to an already existing id.");
        if (looper.Id == oldName)
          rename = looper;
        looper = looper.Next;
      }
      rename.Id = newName;
    }

    /// <summary>Resets the list to an empty state. WARNING could cause excessive garbage collection.</summary>
    public void Clear()
    {
      _head = null;
      _tail = null;
      _count = 0;
    }

    /// <summary>A function to be used in a foreach loop.</summary>
    /// <param name="id">The id of the current node.</param>
    /// <param name="node">The current node of a foreach loop.</param>
    public delegate void ForeachFunction(string id, Type node);
    /// <summary>Allows a foreach loop using a delegate.</summary>
    /// <param name="foreachFunction">The function within a foreach loop.</param>
    /// <remarks>Runtime: O(n * foreachFunction).</remarks>
    public void Foreach(ForeachFunction foreachFunction)
    {
      ListNode looper = _head;
      while (looper != null)
      {
        foreachFunction(looper.Id, looper.Value);
        looper = looper.Next;
      }
    }

    /// <summary>How how want to clone each item during an entire list clone.</summary>
    /// <param name="currentid">The current id of the node.</param>
    /// <param name="node">A single node in teh list.</param>
    /// <param name="newId">The desired new id in the cloned list.</param>
    /// <param name="newNode">The cloned item to be used in the cloned list.</param>
    public delegate void CloneFunction(string currentid, Type node, out string newId, out Type newNode);
    /// <summary>Allows the user to clone the list however they choose (reference clone vs value clone).</summary>
    /// <param name="cloneFunction">he function to perform on each node during cloning.</param>
    /// <returns>The resulting cloned list.</returns>
    /// <remarks>Runtime: O(n * cloneFunction).</remarks>
    public List<Type> Clone(CloneFunction cloneFunction)
    {
      List<Type> listClone = new List<Type>();
      ListNode looper = _head;
      while (looper != null)
      {
        string cloneId;
        Type cloneValue;
        cloneFunction(looper.Id, looper.Value, out cloneId, out cloneValue);
        listClone.Add(cloneId, cloneValue);
        looper = looper.Next;
      }
      return listClone;
    }

    // The following commented code is an alternative to the delegate functions. I am keeping this here
    // for educational purposes to show a good use for delegates.

    ///// <summary>Initializes an iterator for this list. Use IterateNext to iterate through the list.</summary>
    ///// <remarks>Runtime: O(1).</remarks> 
    //public void IteratorInitialize() { _currentIterator = _head; }
    ///// <summary>Gets the next item in the current iteration of this list.</summary>
    ///// <param name="next">The next item in the current iteration of the list.</param>
    ///// <returns>If there is an item to return. "false": nothing to return (end of list). "true": still iterating.</returns>
    ///// <remarks>Runtime: O(1).</remarks> 
    //public bool IteratorGetNext(out Type next)
    //{
    //  next = default(Type);
    //  if (_currentIterator == null)
    //    return false;
    //  next = _currentIterator.Value;
    //  _currentIterator = _currentIterator.Next;
    //  return true;
    //}

    /// <summary>This is used for throwing AVL Tree exceptions only to make debugging faster.</summary>
    private class ListException : Exception { public ListException(string message) : base(message) { } }
  }*/

  #endregion

  #region ListArray

  /// <summary>Implements a growing list as an array, so there is possible expansions/contractions.</summary>
  /// <typeparam name="Type">The type of objects to be placed in the list.</typeparam>
  /// <remarks>The runtimes of each public member are included in the "remarks" xml tags.
  /// Seven (Zachary Patten) 10-12-13.</remarks>
  public class ListArray<Type>
  {
    private Type[] _list;
    private int _count;
    private int _minimumCapacity;

    /// <summary>Gets the number of items in the list.</summary>
    /// <remarks>Runtime: O(1).</remarks>
    public int Count { get { return _count; } }

    /// <summary>Gets the current capacity of the list.</summary>
    /// <remarks>Runtime: O(1).</remarks>
    public int CurrentCapacity { get { return _list.Length; } }

    /// <summary>Allows you to adjust the minimum capacity of this list.</summary>
    /// <remarks>Runtime: O(n), Omega(1).</remarks>
    public int MinimumCapacity
    {
      get { return _minimumCapacity; }
      set
      {
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
      }
    }

    /// <summary>Look-up and set an indexed item in the list.</summary>
    /// <param name="index">The index of the item to get or set.</param>
    /// <returns>The value at the given index.</returns>
    public Type this[int index]
    {
      get
      {
        if (index < 0 || index > _count)
          throw new ListArrayException("Attempting an index look-up outside the ListArray's current size.");
        return _list[index];
      }
      set
      {
        if (index < 0 || index > _count)
          throw new ListArrayException("Attempting an index assignment outside the ListArray's current size.");
        _list[index] = value;
      }
    }

    /// <summary>Creates an instance of a ListArray, and sets it's minimum capacity.</summary>
    /// <param name="minimumCapacity">The initial and smallest array size allowed by this list.</param>
    /// <remarks>Runtime: O(1).</remarks>
    public ListArray(int minimumCapacity)
    {
      _list = new Type[minimumCapacity];
      _count = 0;
    }

    /// <summary>Adds an item to the end of the list.</summary>
    /// <param name="addition">The item to be added.</param>
    /// <remarks>Runtime: O(n), EstAvg(1). </remarks>
    public void Add(Type addition)
    {
      if (_count == _list.Length)
      {
        if (_list.Length > Int32.MaxValue / 2)
          throw new ListArrayException("Your list is so large that it can no longer double itself (Int32.MaxValue barrier reached).");
        Type[] newList = new Type[_list.Length * 2];
        _list.CopyTo(newList, 0);
        _list = newList;
      }
      _list[_count++] = addition;
    }

    /// <summary>Removes the item at a specific index.</summary>
    /// <param name="index">The index of the item to be removed.</param>
    /// <remarks>Runtime: Theta(n - index).</remarks>
    public void Remove(int index)
    {
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
    }

    /// <summary>Empties the list back and reduces it back to its original capacity.</summary>
    /// <remarks>Runtime: O(1). Warning: causes considerable garbage collection.</remarks>
    public void Clear()
    {
      _list = new Type[_minimumCapacity];
      _count = 0;
    }

    /// <summary>A function to be used in a foreach loop.</summary>
    /// <param name="node">The current node of a foreach loop.</param>
    public delegate void ForeachFunction(Type node);
    /// <summary>does a function on every item in the list.</summary>
    /// <param name="foreachFunction">The funtion to perform on every item in the list.</param>
    /// <remarks>Runtime: O(n * foreachFunction).</remarks>
    public void Foreach(ForeachFunction foreachFunction)
    {
      for (int i = 0; i < _count; i++)
        foreachFunction(_list[i]);
    }

    /// <summary>How how want to clone each item during an entire list clone.</summary>
    /// <param name="node">The current list node in the cloning traversal.</param>
    public delegate Type CloneFunction(Type node);
    /// <summary>Allows the user to clone the list however they choose (reference clone vs value clone).</summary>
    /// <param name="cloneFunction">he function to perform on each node during cloning.</param>
    /// <returns>The resulting cloned list.</returns>
    /// <remarks>Runtime: O(n * cloneFunction).</remarks>
    public ListArray<Type> Clone(CloneFunction cloneFunction)
    {
      ListArray<Type> listClone = new ListArray<Type>(_list.Length);
      for (int i = 0; i < _count; i++)
        listClone.Add(cloneFunction(_list[i]));
      return listClone;
    }

    /// <summary>This is used for throwing AVL Tree exceptions only to make debugging faster.</summary>
    private class ListArrayException : Exception { public ListArrayException(string message) : base(message) { } }
  }

  #endregion
}