using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.DataStructures
{
  public class SevenQueueNode<T>
  {
    private T _value;
    private SevenQueueNode<T> _next;

    public T Value
    {
      get { return _value; }
      set { _value = value; }
    }

    public SevenQueueNode<T> Next
    {
      get { return _next; }
      set { _next = value; }
    }

    public SevenQueueNode(T value, SevenQueueNode<T> indexIncrement)
    {
      _value = value;
      _next = indexIncrement;
    }
  }
}
