using System.Collections.Generic;

namespace Engine.DataStructures
{
  public class LinkedListNode<T>
  {
    private T _value;
    private LinkedListNode<T> _next;

    public T Value
    {
      get { return _value; }
      set { _value = value; }
    }

    public LinkedListNode<T> Next
    {
      get { return _next; }
      set { _next = value; }
    }

    public LinkedListNode(T value, LinkedListNode<T> next)
    {
      _value = value;
      _next = next;
    }
  }
}