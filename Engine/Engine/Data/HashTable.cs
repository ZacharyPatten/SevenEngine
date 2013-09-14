using System;

namespace Engine.Data
{
  public class HashTable<TKey, TValue>
  {
    private LinkedListNode<Tuple<TKey, TValue>>[] _table
        = new LinkedListNode<Tuple<TKey, TValue>>[107];

    private int _count = 0;

    /// <summary>A set of allowable table sizes, all of which are prime.</summary>
    private int[] _tableSizes = new int[]
    {
        107, 223, 449, 907, 1823, 3659, 7321, 14653, 29311, 58631, 117269, 234539, 
        469099, 938207, 1876417, 3752839, 7505681, 15011389, 30022781, 60045577, 
        120091177, 240182359, 480364727, 960729461, 1921458943
    };

    private float _maxLoadFactor = 1.0F;
    private int _sizeIndex = 0;

    public TValue this[TKey key]
    {
      get
      {
        TValue temp;
        if (TryGetValue(key, out temp))
          return temp;
        else
          throw new HashTableException("\nMember: \"this[TKey key]\"\nIndex out of range (key not found).");
      }
      set
      {
        LinkedListNode<Tuple<TKey, TValue>> cell = Find(key, Hash(key));
        if (cell == null)
        {
          value = default(TValue);
          throw new HashTableException("\nMember: \"this[TKey key]\"\nIndex out of range (key not found).");
        }
        else
        {
          cell.Value.Right = value;
        }
      }
    }

    public int Count
    {
      get
      {
        return _count;
      }
    }

    public bool TryGetValue(TKey key, out TValue value)
    {
      LinkedListNode<Tuple<TKey, TValue>> cell = Find(key, Hash(key));
      if (cell == null)
      {
        value = default(TValue);
        return false;
      }
      else
      {
        value = cell.Value.Right;
        return true;
      }
    }

    private LinkedListNode<Tuple<TKey, TValue>> Find(TKey key, int loc)
    {
      for (LinkedListNode<Tuple<TKey, TValue>> p = _table[loc];
          p != null; p = p.Next)
      {
        if (p.Value.Left.Equals(key))
        {
          return p;
        }
      }
      return null;
    }

    private int Hash(TKey key)
    {
      return (key.GetHashCode() & 0x7fffffff) % _table.Length;
    }

    public void Add(TKey key, TValue value)
    {
      if (key == null)
      {
        throw new HashTableException("\nMember: \"Add(TKey key, TValue value)\"\nThe key cannot be null.");
      }
      int location = Hash(key);
      if (Find(key, location) == null)
      {
        if (++_count > _table.Length * _maxLoadFactor &&
            _sizeIndex < _tableSizes.Length - 1)
        {
          LinkedListNode<Tuple<TKey, TValue>>[] t = _table;
          _table = new LinkedListNode<Tuple<TKey, TValue>>[_tableSizes[++_sizeIndex]];
          for (int i = 0; i < t.Length; i++)
          {
            while (t[i] != null)
            {
              LinkedListNode<Tuple<TKey, TValue>> cell
                  = RemoveFirst(t, i);
              Add(cell, Hash(cell.Value.Left));
            }
          }
          location = Hash(key);
        }
        LinkedListNode<Tuple<TKey, TValue>> p
            = new LinkedListNode<Tuple<TKey, TValue>>(new Tuple<TKey, TValue>(key, value), null);
        //p.Value = new Tuple<TKey, TValue>(key, value);
        Add(p, location);
      }
      else
      {
        throw new HashTableException("\nMember: \"Add(TKey key, TValue value)\"\nThe key is already in the table.");
      }
    }

    private void Add(LinkedListNode<Tuple<TKey, TValue>> cell, int location)
    {
      cell.Next = _table[location];
      _table[location] = cell;
    }

    public void Remove(TKey key)
    {
      //if (key == null)
      //  throw new HashTableException("\nMember: \"Remove(TKey key)\"\nThe key cannot be null.");
      //int location = Hash(key);
      //if (Find(key, location) != null)
      //{
      //  LinkedListNode<Tuple<TKey, TValue>> p
      //      = new LinkedListNode<Tuple<TKey, TValue>>(new Tuple<TKey, TValue>(key, value), null);
      //  //p.Value = new Tuple<TKey, TValue>(key, value);
      //  Add(p, location);
      //}
      //else
      //{
      //  throw new HashTableException("\nMember: \"Remove(TKey key)\"\nThe key is not in the table.");
      //}
      throw new NotImplementedException("the dictionary removal is not yet written.");
    }

    private LinkedListNode<Tuple<TKey, TValue>>
        RemoveFirst(LinkedListNode<Tuple<TKey, TValue>>[] t, int i)
    {
      LinkedListNode<Tuple<TKey, TValue>> first = t[i];
      t[i] = first.Next;
      return first;
    }
  }

  class HashTableException : Exception
  {
    public HashTableException(string message) : base(message) { }
  }
}
