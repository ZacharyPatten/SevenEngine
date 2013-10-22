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

// Written by Seven (Zachary Aaron Patten)
// Last Edited on date 10-17-13
// Feel free to use this code in any manor you see fit.
// However, please site me because I put quite a bit of time into it.
// - Thanks. :)

using System;

namespace SevenEngine.DataStructures
{
  #region List

  /// <summary>Implements a growing, singularly-linked list data structure.</summary>
  /// <typeparam name="Type">The type of objects to be placed in the list.</typeparam>
  /// <remarks>The runtimes of each public member are included in the "remarks" xml tags.
  /// Seven (Zachary Patten) 10-12-13.</remarks>
  public class List<Type>
  {
    #region ListNode

    /// <summary>This class just holds the data for each individual node of the list.</summary>
    private class ListNode
    {
      private string _id;
      private Type _value;
      private ListNode _down;

      internal string Id { get { return _id; } set { _id = value; } }
      internal Type Value { get { return _value; } set { _value = value; } }
      internal ListNode Down { get { return _down; } set { _down = value; } }

      internal ListNode(string id, Type data, ListNode down)
      {
        _id = id;
        _value = data;
        _down = down;
      }
    }

    #endregion

    private ListNode _head;
    private ListNode _currentIterator;
    private int _count;

    /// <summary>Returns the number of items in the list.</summary>
    /// <remarks>Runtime: O(1).</remarks>
    public int Count { get { return _count; } }

    /// <summary>Creates an instance of a stalistck.</summary>
    /// <remarks>Runtime: O(1).</remarks>
    public List()
    {
      _head = null;
      _count = 0;
    }

    /// <summary>Adds an item to the list.</summary>
    /// <param name="id">The string id of the item to add to the list.</param>
    /// <param name="addition">The item to add to the list.</param>
    /// <remarks>Runtime: Theta(n).</remarks>
    public void Add(string id, Type addition)
    {
      Add(id, addition, _head);
      _count++;
    }

    /// <summary>Adds an item to the list.</summary>
    /// <param name="id">The string id of the addition.</param>
    /// <param name="addition">The item to add to the list.</param>
    /// <param name="listNode">The current location during recursion.</param>
    /// <remarks>Runtime: Theta(n).</remarks>
    private void Add(string id, Type addition, ListNode listNode)
    {
      if (listNode == null)
        _head = new ListNode(id, addition, null);
      if (listNode.Id == id)
        throw new ListException("Attempting to add an already existing id value.");
      else if (listNode.Down == null)
        listNode.Down = new ListNode(id, addition, null);
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
    /// <remarks>Runtime: Theta(n).</remarks>
    private void Remove(string removalId, ListNode listNode)
    {
      if (listNode == null)
        throw new ListException("Attempting to remove a non-existing id value.");
      else if (listNode.Id == removalId)
        _head = _head.Down;
      else if (listNode.Down.Id == removalId)
        listNode.Down = listNode.Down.Down;
      else
        Remove(removalId, listNode.Down);
    }

    /// <summary>Initializes an iterator for this list. Use IterateNext to iterate through the list.</summary>
    /// <remarks>Runtime: O(1).</remarks> 
    public void IteratorInitialize() { _currentIterator = _head; }

    /// <summary>Gets the next item in the current iteration of this list.</summary>
    /// <param name="next">The next item in the current iteration of the list.</param>
    /// <returns>If there is an item to return. "false": nothing to return (end of list). "true": still iterating.</returns>
    /// <remarks>Runtime: O(1).</remarks> 
    public bool IterateGetNext(out Type next)
    {
      next = default(Type);
      if (_currentIterator == null)
        return false;
      next = _currentIterator.Value;
      _currentIterator = _currentIterator.Down;
      return true;
    }

    public void Clear()
    {
      _head = null;
      _count = 0;
    }

    /// <summary>This is used for throwing AVL Tree exceptions only to make debugging faster.</summary>
    private class ListException : Exception { public ListException(string message) : base(message) { } }
  }

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
      if (_count < _list.Length / 4 && _list.Length / 4 > _minimumCapacity)
      {
        Type[] newList = new Type[_list.Length / 2];
        _list.CopyTo(newList, 0);
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

    /// <summary>This is used for throwing AVL Tree exceptions only to make debugging faster.</summary>
    private class ListArrayException : Exception { public ListArrayException(string message) : base(message) { } }
  }

  #endregion
}