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

using System;
using System.Threading;
using SevenEngine.DataStructures;

namespace SevenEngine.DataStructures
{
  public interface HashTable<ValueType, KeyType> : DataStructure<ValueType>
  {
    ValueType this[KeyType key] { get; set; }
    ValueType Get(KeyType get);
    bool TryGet(KeyType get, out ValueType returnValue);
    bool Contains(KeyType containsCheck);
    void Add(KeyType key, ValueType value);
    void Remove(KeyType removalKey);
    int Count { get; }
    bool IsEmpty { get; }
    void Clear();
  }
  
  #region HashTableLinked

  public class HashTableLinked<ValueType, KeyType> : HashTable<ValueType, KeyType>
  {
    /// <summary>A set of allowable table sizes, all of which are prime.</summary>
    private static readonly int[] _tableSizes = new int[]
    {
        107, 223, 449, 907, 1823, 3659, 7321, 14653, 29311, 58631, 117269, 234539, 
        469099, 938207, 1876417, 3752839, 7505681, 15011389, 30022781, 60045577, 
        120091177, 240182359, 480364727, 960729461, 1921458943
    };

    #region HashTableLinkedNode

    private class HashTableLinkedNode
    {
      private KeyType _key;
      private ValueType _value;
      private HashTableLinkedNode _next;

      internal KeyType Key { get { return _key; } set { _key = value; } }
      internal ValueType Value { get { return _value; } set { _value = value; } }
      internal HashTableLinkedNode Next { get { return _next; } set { _next = value; } }

      internal HashTableLinkedNode(KeyType key, ValueType value, HashTableLinkedNode next)
      {
        _key = key;
        _value = value;
        _next = next;
      }
    }

    #endregion

    private const float _maxLoadFactor = 1.0f;

    private HashTableLinkedNode[] _table;
    private int _count;
    private int _sizeIndex;
    private object _lock;
    private int _readers;
    private int _writers;

    /// <summary>Returns the current number of items in the structure.</summary>
    /// <remarks>Runetime: O(1).</remarks>
    public int Count { get { return _count; } }

    /// <summary>Returns true if the structure is empty.</summary>
    /// <remarks>Runetime: O(1).</remarks>
    public bool IsEmpty { get { return _count == 0; } }

    /// <summary>Returns the current size of the actual table. You will want this if you 
    /// wish to multithread structure traversals.</summary>
    /// <remarks>Runtime: O(1).</remarks>
    public int TableSize { get { return _table.Length; } }

    /// <summary>Allows indexed look-up of the structure. (Set does not replace the Add() method)</summary>
    /// <param name="key">The "index" to access of the structure.</param>
    /// <returns>The value at the index of the requested key.</returns>
    /// <remarks>Runtime: N/A.</remarks>
    public ValueType this[KeyType key]
    {
      get
      {
        ReaderLock();
        ValueType temp;
        if (TryGet(key, out temp))
        {
          ReaderUnlock();
          return temp;
        }
        else
        {
          ReaderUnlock();
          throw new HashTableListException("Attempting to look up a non-existing key.");
        }
      }
      set
      {
        WriterLock();
        HashTableLinkedNode cell = Find(key, Hash(key));
        if (cell == null)
        {
          value = default(ValueType);
          WriterUnlock();
          throw new HashTableListException("Index out of range (key not found). This does not replace the add method.");
        }
        else
        {
          cell.Value = value;
          WriterUnlock();
        }
      }
    }

    /// <summary>Constructs a new hash table instance.</summary>
    /// <remarks>Runtime: O(1).</remarks>
    public HashTableLinked()
    {
      _table = new HashTableLinkedNode[107];
      _count = 0;
      _sizeIndex = 0;
      _lock = new object();
      _readers = 0;
      _writers = 0;
    }

    public bool Contains(KeyType key)
    {
      ReaderLock();
      HashTableLinkedNode cell = Find(key, Hash(key));
      if (cell == null)
      {
        ReaderUnlock();
        return false;
      }
      else
      {
        ReaderUnlock();
        return true;
      }
    }

    public ValueType Get(KeyType key)
    {
      ReaderLock();
      HashTableLinkedNode cell = Find(key, Hash(key));
      if (cell == null)
      {
        ReaderUnlock();
        throw new HashTableListException("attempting to get a non-existing key value.");
      }
      else
      {
        ValueType returnValue = cell.Value;
        ReaderUnlock();
        return returnValue;
      }
    }

    /// <summary>Typical try-get functionality for data structures.</summary>
    /// <param name="key">The key to look up the value for.</param>
    /// <param name="value">The return value if the value is found (returns default if not).</param>
    /// <returns>True if the requested key look up found a value.</returns>
    /// <remarks>Runtime: O(1).</remarks>
    public bool TryGet(KeyType key, out ValueType value)
    {
      ReaderLock();
      HashTableLinkedNode cell = Find(key, Hash(key));
      if (cell == null)
      {
        value = default(ValueType);
        ReaderUnlock();
        return false;
      }
      else
      {
        value = cell.Value;
        ReaderUnlock();
        return true;
      }
    }

    private HashTableLinkedNode Find(KeyType key, int loc)
    {
      for (HashTableLinkedNode bucket = _table[loc]; bucket != null; bucket = bucket.Next)
        if (bucket.Key.Equals(key))
          return bucket;
      return null;
    }

    private int Hash(KeyType key) { return (key.GetHashCode() & 0x7fffffff) % _table.Length; }

    /// <summary>Adds a value to the hash table.</summary>
    /// <param name="key">The key value to use as the look-up reference in the hash table.</param>
    /// <param name="value">The value to store in the hash table.</param>
    /// <remarks>Runtime: O(n), Omega(1).</remarks>
    public void Add(KeyType key, ValueType value)
    {
      WriterLock();
      if (key == null)
      {
        WriterUnlock();
        throw new HashTableListException("attempting to add a null key to the structure.");
      }
      int location = Hash(key);
      if (Find(key, location) == null)
      {
        if (++_count > _table.Length * _maxLoadFactor && _sizeIndex < _tableSizes.Length - 1)
        {
          HashTableLinkedNode[] t = _table;
          _table = new HashTableLinkedNode[_tableSizes[++_sizeIndex]];
          for (int i = 0; i < t.Length; i++)
          {
            while (t[i] != null)
            {
              HashTableLinkedNode cell = RemoveFirst(t, i);
              Add(cell, Hash(cell.Key));
            }
          }
          location = Hash(key);
        }
        HashTableLinkedNode p = new HashTableLinkedNode(key, value, null);
        Add(p, location);
        WriterUnlock();
      }
      else
      {
        WriterUnlock();
        throw new HashTableListException("\nMember: \"Add(TKey key, TValue value)\"\nThe key is already in the table.");
      }
    }

    private HashTableLinkedNode RemoveFirst(HashTableLinkedNode[] t, int i)
    {
      HashTableLinkedNode first = t[i];
      t[i] = first.Next;
      return first;
    }

    private void Add(HashTableLinkedNode cell, int location)
    {
      cell.Next = _table[location];
      _table[location] = cell;
    }

    /// <summary>Removes a value from the hash table.</summary>
    /// <param name="key">The key of the value to remove.</param>
    /// <remarks>Runtime: N/A. (I'm still editing this structure)</remarks>
    public void Remove(KeyType key)
    {
      WriterLock();
      if (key == null)
      {
        WriterUnlock();
        throw new HashTableListException("attempting to remove \"null\" from the structure.");
      }
      int location = Hash(key);
      if (_table[location].Key.Equals(key))
        _table[location] = _table[location].Next;
      for (HashTableLinkedNode bucket = _table[location]; bucket != null; bucket = bucket.Next)
      {
        if (bucket.Next == null)
        {
          WriterUnlock();
          throw new HashTableListException("attempting to remove a non-existing value.");
        }
        else if (bucket.Next.Key.Equals(key))
          bucket.Next = bucket.Next.Next;
      }
      _count--;
      WriterUnlock();
    }

    public void Clear()
    {
      WriterLock();
      _table = new HashTableLinkedNode[107];
      _count = 0;
      _sizeIndex = 0;
      WriterUnlock();
    }
    
    /// <summary>Does an imperative traversal of the structure.</summary>
    /// <param name="traversalAction">The action to perform during traversal.</param>
    /// <returns>Whether or not the traversal was broken.</returns>
    /// <remarks>Runtime: O(n * traversalAction).</remarks>
    public bool TraverseBreakable(Func<ValueType, bool> traversalAction)
    {
      ReaderLock();
      for (int i = 0; i < _table.Length; i++)
      {
        if (_table[i] == null) continue;
        HashTableLinkedNode looper = _table[i];
        while (looper != null)
        {
          if (!traversalAction(looper.Value)) { ReaderUnlock(); return false; }
          looper = looper.Next;
        }
      }
      ReaderUnlock();
      return true;
    }

    /// <summary>Does an imperative traversal of the structure.</summary>
    /// <param name="traversalAction">The action to perform during the traversal.</param>
    /// <remarks>Runtime: O(n * traversalAction).</remarks>
    public void Traverse(Action<ValueType> traversalAction)
    {
      ReaderLock();
      for (int i = 0; i < _table.Length; i++)
      {
        if (_table[i] == null) continue;
        HashTableLinkedNode looper = _table[i];
        while (looper != null)
        {
          traversalAction(looper.Value);
          looper = looper.Next;
        }
      }
      ReaderUnlock();
    }

    public ValueType[] ToArray()
    {
      ReaderLock();
      ValueType[] array = new ValueType[_count];
      int index = 0;
      for (int i = 0; i < _table.Length; i++)
      {
        if (_table[i] == null) continue;
        HashTableLinkedNode looper = _table[i];
        while (looper != null)
        {
          array[index++] = looper.Value;
          looper = looper.Next;
        }
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

    /// <summary>This is used for throwing hash table exceptions only to make debugging faster.</summary>
    private class HashTableListException : Exception { public HashTableListException(string message) : base(message) { } }
  }

  #endregion
}