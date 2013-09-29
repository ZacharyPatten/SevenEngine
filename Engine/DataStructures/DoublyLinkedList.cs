using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.DataStructures
{
  public class DoublyLinkedList<T>
  {
    private DoublyLinkedListNode<T> _start;
    private DoublyLinkedListNode<T> _end;
    private int _count;

    /// <summary>Allows direct access of the nodes within the list. WARNING: if used
    /// incorrectly you could corrupt the structure itself.</summary>
    public DoublyLinkedListNode<T> First
    {
      get { return _start.Next; }
    }

    /// <summary>Allows direct access of the nodes within the list. WARNING: if used
    /// incorrectly you could corrupt the structure itself.</summary>
    public DoublyLinkedListNode<T> Last
    {
      get { return _end.Previous; }
    }

    public int Count { get { return _count; } }

    /// <summary>Allows indexing of the list (starting at 0). WARNING: this is incredibly inefficient
    /// due to the nature of lists, and should only really be used for debugging.</summary>
    public T this[int index]
    {
      get
      {
        if (index < 0 || index > _count - 1)
          throw new DoublyLinkedListException("\nMember: \"this[int index]\"\nIndex out of range during indexed access of structure.");
        DoublyLinkedListNode<T> temp;
        if (index < _count / 2)
        {
          temp = _start;
          for (int loop = 0; loop < index + 1; loop++) temp = temp.Next;
        }
        else
        {
          temp = _end;
          for (int loop = _count; loop > index; loop--) temp = temp.Previous;
        }
        return temp.Value;
      }
      set
      {
        if (index < 0 || index > _count - 1)
          throw new DoublyLinkedListException("\nMember: \"this[int index]\"\nIndex out of range during indexed access of structure.");
        DoublyLinkedListNode<T> temp;
        if (index < _count / 2)
        {
          temp = _start;
          for (int loop = 0; loop < index + 1; loop++) temp = temp.Next;
        }
        else
        {
          temp = _end;
          for (int loop = _count; loop > index; loop--) temp = temp.Previous;
        }
        temp.Value = value;
      }
    }

    public DoublyLinkedList()
    {
      _start = new DoublyLinkedListNode<T>(null, default(T), null);
      _end = new DoublyLinkedListNode<T>(_start, default(T), null);
      _start.Next = _end;
      _count = 0;
    }

    /// <summary>Adds a value to the current end of the structure (index = count - 1).</summary>
    public void AddToEnd(T value)
    {
      DoublyLinkedListNode<T> addition = new DoublyLinkedListNode<T>(_end.Previous, value, _end);
      addition.Previous.Next = addition;
      _end.Previous = addition;
      _count++;
    }

    public T RemoveLast()
    {
      if (_count == 0)
        throw new DoublyLinkedListException("\nMember: \"RemoveLast()\"\nAttempting to remove a node from an empty doubly linked list.");
      T temp = _end.Previous.Value;
      _end.Previous = _end.Previous.Previous;
      _end.Previous.Next = _end;
      _count--;
      return temp;
    }

    /// <summary>Traverses through the structure and removes the first node that
    /// is equal to the parameter using the ".Equals()" comparison.</summary>
    public T RemoveFirstEquality(T removal)
    {
      DoublyLinkedListNode<T> loop = _start;
      while (loop != null)
      {
        if (loop.Value.Equals(removal)) break;
        loop = loop.Next;
      }
      if (loop == null)
        throw new DoublyLinkedListException("\nMember: \"RemoveFirstEquality(T removal)\"\nCould not find an equality to remove.");
      _count--;
      return loop.Value;
    }
  }

  class DoublyLinkedListException : Exception
  {
    public DoublyLinkedListException(string message) : base(message) { }
  }
}