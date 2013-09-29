using System.Collections.Generic;

namespace Engine.DataStructures
{
  public class DoublyLinkedListNode<T>
  {
    private DoublyLinkedListNode<T> _previous;
    private T _value;
    private DoublyLinkedListNode<T> _next;

    public DoublyLinkedListNode<T> Previous
    {
      get { return _previous; }
      set { _previous = value; }
    }

    public T Value
    {
      get { return _value; }
      set { _value = value; }
    }

    public DoublyLinkedListNode<T> Next
    {
      get { return _next; }
      set { _next = value; }
    }

    public DoublyLinkedListNode(DoublyLinkedListNode<T> indexDecrement,
        T value, DoublyLinkedListNode<T> indexIncrement)
    {
      _previous = indexDecrement;
      _value = value;
      _next = indexIncrement;
    }
  }
}
