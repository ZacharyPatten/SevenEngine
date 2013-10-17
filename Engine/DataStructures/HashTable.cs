// This file contains the following classes:
// - HashTable
//   - HashTableListNode

// This file contains runtime values.
// All runtimes are in O-Notation. Here is a brief explanation:
// - "O(x)": the member has an upper bound of runtime equation "x"
// - "Omega(x)": the member has a lower bound of runtime equation "x"
// - "Theta(x)": the member has an upper and lower bound of runtime equation "x"
// - "EstAvg(x)": the runtime equation "x" to typically expect
// Notes: if the letter "n" is used, it typically means the current number of items within the structure

// Written by Seven (Zachary Aaron Patten)
// Last Edited on date 10-12-13
// Feel free to use this code in any manor you see fit.
// However, please site me because I put quite a bit of time into it.
// - Thanks. :)

using System;
using System.Collections;

namespace Engine.DataStructures
{
  #region HashTable

  public class HashTable<TKey, TValue>
  {
    /// <summary>A set of allowable table sizes, all of which are prime.</summary>
    private static readonly int[] _tableSizes = new int[]
    {
        107, 223, 449, 907, 1823, 3659, 7321, 14653, 29311, 58631, 117269, 234539, 
        469099, 938207, 1876417, 3752839, 7505681, 15011389, 30022781, 60045577, 
        120091177, 240182359, 480364727, 960729461, 1921458943
    };

    #region HashTableListNode

    private class HashTableListNode
    {
      //private Link<TKey, TValue> _value;
      private TKey _key;
      private TValue _value;
      private HashTableListNode _next;

      internal TKey Key { get { return _key; } set { _key = value; } }
      internal TValue Value { get { return _value; } set { _value = value; } }
      internal HashTableListNode Next { get { return _next; } set { _next = value; } }

      internal HashTableListNode(TKey key, TValue value, HashTableListNode next)
      {
        _key = key;
        _value = value;
        _next = next;
      }
    }

    #endregion

    private HashTableListNode[] _table = new HashTableListNode[107];
    private int _count = 0;
    private float _maxLoadFactor = 1.0F;
    private int _sizeIndex = 0;

    public int Count { get { return _count; } }

    public TValue this[TKey key]
    {
      get
      {
        TValue temp;
        if (TryGetValue(key, out temp))
          return temp;
        else
          throw new HashTableException("Attempting to look up a non-existing key.");
      }
      set
      {
        HashTableListNode cell = Find(key, Hash(key));
        if (cell == null)
        {
          value = default(TValue);
          throw new HashTableException("Index out of range (key not found).");
        }
        else
        {
          cell.Value = value;
        }
      }
    }

    public bool TryGetValue(TKey key, out TValue value)
    {
      HashTableListNode cell = Find(key, Hash(key));
      if (cell == null)
      {
        value = default(TValue);
        return false;
      }
      else
      {
        value = cell.Value;
        return true;
      }
    }

    private HashTableListNode Find(TKey key, int loc)
    {
      for (HashTableListNode p = _table[loc]; p != null; p = p.Next)
        if (p.Key.Equals(key))
          return p;
      return null;
    }

    private int Hash(TKey key) { return (key.GetHashCode() & 0x7fffffff) % _table.Length; }

    public void Add(TKey key, TValue value)
    {
      if (key == null)
        throw new HashTableException("\nMember: \"Add(TKey key, TValue value)\"\nThe key cannot be null.");
      int location = Hash(key);
      if (Find(key, location) == null)
      {
        if (++_count > _table.Length * _maxLoadFactor && _sizeIndex < _tableSizes.Length - 1)
        {
          HashTableListNode[] t = _table;
          _table = new HashTableListNode[_tableSizes[++_sizeIndex]];
          for (int i = 0; i < t.Length; i++)
          {
            while (t[i] != null)
            {
              HashTableListNode cell = RemoveFirst(t, i);
              Add(cell, Hash(cell.Key));
            }
          }
          location = Hash(key);
        }
        HashTableListNode p = new HashTableListNode(key, value, null);
        //p.Value = new Tuple<TKey, TValue>(key, value);
        Add(p, location);
      }
      else
        throw new HashTableException("\nMember: \"Add(TKey key, TValue value)\"\nThe key is already in the table.");
    }

    private HashTableListNode RemoveFirst(HashTableListNode[] t, int i)
    {
      HashTableListNode first = t[i];
      t[i] = first.Next;
      return first;
    }

    private void Add(HashTableListNode cell, int location)
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
      throw new HashTableException("the dictionary removal is not yet written.");
    }

    private class HashTableException : Exception { public HashTableException(string message) : base(message) { } }
  }

  #endregion

  #region HashTable Attempt Two

  public class Hashtable2
  {
    private static readonly float _loadFactor = 0.72f;
    private static readonly int[] _primeNumbers = 
    {
      3, 7, 11, 17, 23, 29, 37, 47, 59, 71, 89, 107, 131, 163, 197, 239, 293, 353, 431, 521, 631, 761, 919,
      1103, 1327, 1597, 1931, 2333, 2801, 3371, 4049, 4861, 5839, 7013, 8419, 10103, 12143, 14591,
      17519, 21023, 25229, 30293, 36353, 43627, 52361, 62851, 75431, 90523, 108631, 130363, 156437,
      187751, 225307, 270371, 324449, 389357, 467237, 560689, 672827, 807403, 968897, 1162687, 1395263,
      1674319, 2009191, 2411033, 2893249, 3471899, 4166287, 4999559, 5999471, 7199369
    };

    /// <summary>Determines whether a number is prime or not.</summary>
    /// <param name="candidate">The number to determine prime status.</param>
    /// <returns>True if prime, false if not prime.</returns>
    private static bool IsPrime(int candidate)
    {
      if (candidate == 2) return true;
      for (int divisor = 3; divisor <= Math.Sqrt(candidate); divisor += 2)
        if ((candidate % divisor) == 0)
          return false;
      return true;
    }

    /// <summary>Gets the first prime number greater than or equal to the parameter.</summary>
    /// <param name="minimum">The minimum bound on a prime number request (return value >= minimum). 
    /// If this value is negative or too large for Int32 support an exception is thrown.</param>
    /// <returns>The first prime number greater than or equal to the parameter.</returns>
    private static int GetNextPrime(int minimum)
    {
      if (minimum < 3)
        if (minimum == 2) return minimum;
        else if (minimum == 1 || minimum == 0) return 3;
        else throw new HashTableException("Requesting a prime number with a negative minimim.");
      // Try to use the hard coded chart for speed
      if (minimum < _primeNumbers[_primeNumbers.Length - 1])
        for (int i = 0; i < _primeNumbers.Length; i++)
          if (_primeNumbers[i] >= minimum) return _primeNumbers[i];
      // Run calculations to find the next prime number
      for (; minimum <= Int32.MaxValue; minimum += 2)
        if (IsPrime(minimum)) break;
      if (minimum == Int32.MaxValue)
        throw new HashTableException("There is no prime number before the maximum Int32 value.");
      return minimum;
    }

    // Deleted entries have their key set to buckets

    // The hash table data.
    // This cannot be serialised
    private struct bucket
    {
      public Object key;
      public Object value;
      public int hashCollision;   // Store hash code; sign bit means there was a collision.
    }

    private bucket[] _buckets;
    private int _count;
    private int _occupancy;
    private int _loadsize;
    private int _capacity;

    /// <summary>Returns the number of entries in this table.</summary>
    public virtual int Count { get { return _count; } }

    /// <summary>Creates a hash table of the provided size.</summary>
    /// <param name="capacity">The maximum size of the table.</param>
    public Hashtable2(int capacity)
    {
      if (capacity < 0)
        throw new HashTableException("Attempting to create a hash table with < 0 capacity.");
      int hashsize = GetNextPrime((int)(capacity / _loadFactor));
      if (hashsize < 11) hashsize = 11;
      _buckets = new bucket[hashsize];
      _loadsize = (int)(_loadFactor * hashsize);
      _capacity = capacity;
    }

    /// <summary>Computes the hash function:  H(key, i) = h1(key) + i*h2(key, hashSize).</summary>
    /// <param name="key">The object computing the hash code for.</param>
    /// <param name="hashsize">The hash size.</param>
    /// <param name="seed">"h1(key)" from the function: H(key, i) = h1(key) + i*h2(key, hashSize).</param>
    /// <param name="incr">"h2(key, hashSiz.)" from the function: H(key, i) = h1(key) + i*h2(key, hashSize).</param>
    /// <returns>The computed hash code.</returns>
    private uint ComputeHash(Object key, int hashsize, out uint seed, out uint incr)
    {
      uint hashcode = (uint)GetHash(key) & 0x7FFFFFFF;
      seed = (uint)hashcode;
      incr = (uint)(1 + (((seed >> 5) + 1) % ((uint)hashsize - 1)));
      return hashcode;
    }

    // Removes all entries from this hashtable.
    public virtual void Clear()
    {
      if (_count == 0) return;

      for (int i = 0; i < _buckets.Length; i++)
      {
        _buckets[i].hashCollision = 0;
        _buckets[i].key = null;
        _buckets[i].value = null;
      }

      _count = 0;
      _occupancy = 0;
    }

    /// <summary>Creates a copy of this hash table.</summary>
    /// <returns>A copy of the current hash table.</returns>
    public virtual Object Clone()
    {
      Hashtable2 hashTable = new Hashtable2(_capacity);
      for (int i = _buckets.Length - 1; i >= 0; i--)
        if (_buckets[i].key != null && _buckets[i].key != _buckets)
          hashTable[_buckets[i].key] = _buckets[i].value;
      return hashTable;
    }

    // Checks if this hashtable contains the given key.
    public virtual bool Contains(Object key)
    {
      return ContainsKey(key);
    }

    // Checks if this hashtable contains an entry with the given key.  This is
    // an O(1) operation.
    //
    public virtual bool ContainsKey(Object key)
    {
      if (key == null)
        throw new HashTableException("Attempting run a contains check with a null item. (you gave ContainsKey() a null)");

      uint seed;
      uint incr;
      // Take a snapshot of buckets, in case another thread resizes table
      bucket[] lbuckets = _buckets;
      uint hashcode = ComputeHash(key, lbuckets.Length, out seed, out incr);
      int ntry = 0;

      bucket b;
      int bucketNumber = (int)(seed % (uint)lbuckets.Length);
      do
      {
        b = lbuckets[bucketNumber];
        if (b.key == null)
        {
          return false;
        }
        if (((b.hashCollision & 0x7FFFFFFF) == hashcode) &&
            KeyEquals(b.key, key))
          return true;
        bucketNumber = (int)(((long)bucketNumber + incr) % (uint)lbuckets.Length);
      } while (b.hashCollision < 0 && ++ntry < lbuckets.Length);
      return false;
    }



    // Checks if this hashtable contains an entry with the given value. The
    // values of the entries of the hashtable are compared to the given value
    // using the Object.Equals method. This method performs a linear
    // search and is thus be substantially slower than the ContainsKey
    // method.
    //
    public virtual bool ContainsValue(Object value)
    {
      if (value == null)
        for (int i = _buckets.Length; --i >= 0; )
          if (_buckets[i].key != null && _buckets[i].key != _buckets && _buckets[i].value == null)
            return true;
          else
            // I had to use "j" vs "i" here because the compiler apparently cant determine scope without brackets
            for (int j = _buckets.Length; --j >= 0; )
              if (_buckets[j].value != null && _buckets[j].value.Equals(value))
                return true;
      return false;
    }

    // Copies the keys of this hashtable to a given array starting at a given
    // index. This method is used by the implementation of the CopyTo method in
    // the KeyCollection class.
    private void CopyKeys(Array array, int arrayIndex)
    {
      bucket[] lbuckets = _buckets;
      for (int i = lbuckets.Length; --i >= 0; )
      {
        Object keyv = lbuckets[i].key;
        if ((keyv != null) && (keyv != _buckets))
        {
          array.SetValue(keyv, arrayIndex++);
        }
      }
    }

    // Copies the keys of this hashtable to a given array starting at a given
    // index. This method is used by the implementation of the CopyTo method in
    // the KeyCollection class.
    private void CopyEntries(Array array, int arrayIndex)
    {
      bucket[] lbuckets = _buckets;
      for (int i = lbuckets.Length; --i >= 0; )
      {
        Object keyv = lbuckets[i].key;
        if ((keyv != null) && (keyv != _buckets))
        {
          DictionaryEntry entry = new DictionaryEntry(keyv, lbuckets[i].value);
          array.SetValue(entry, arrayIndex++);
        }
      }
    }

    /// <summary>Copies the values in this hash table into an array (note that this is only values, not keys).</summary>
    /// <param name="array">The array to copy the values to.</param>
    /// <param name="arrayIndex">The startig index of the copy.</param>
    public virtual void CopyTo(Array array, int arrayIndex)
    {
      if (array == null)
        throw new HashTableException("Attempting to copy the items in this hash table into a null array.");
      if (array.Rank != 1)
        throw new HashTableException("Attempting to copy the items in this hash table into an array of wrong demension size.");
      if (arrayIndex < 0)
        throw new HashTableException("Attempting to copy the items in this hash table, but the startig index provided is negative.");
      if (array.Length - arrayIndex < _count)
        throw new HashTableException("Attempting to copy the items in this hash table, but there is not enough room in the array if we start at " + arrayIndex + ".");
      CopyEntries(array, arrayIndex);
    }


    // Copies the values of this hashtable to a given array starting at a given
    // index. This method is used by the implementation of the CopyTo method in
    // the ValueCollection class.
    private void CopyValues(Array array, int arrayIndex)
    {
      bucket[] lbuckets = _buckets;
      for (int i = lbuckets.Length; --i >= 0; )
      {
        Object keyv = lbuckets[i].key;
        if ((keyv != null) && (keyv != _buckets))
        {
          array.SetValue(lbuckets[i].value, arrayIndex++);
        }
      }
    }

    // Returns the value associated with the given key. If an entry with the
    // given key is not found, the returned value is null.

    /// <summary>Standard hash table indexing by key.</summary>
    /// <param name="key">The "index" at which to store a value.</param>
    /// <returns>The value at the index key </returns>
    public virtual Object this[Object key]
    {
      get
      {
        if (key == null)
          throw new HashTableException("Attempting an indexed look up with a null key.");
        uint seed;
        uint incr;

        uint hashcode = ComputeHash(key, _buckets.Length, out seed, out incr);
        int ntry = 0;

        bucket b;
        int bucketNumber = (int)(seed % (uint)_buckets.Length);
        do
        {
          b = _buckets[bucketNumber];
          if (b.key == null)
          {
            return null;
          }
          if (((b.hashCollision & 0x7FFFFFFF) == hashcode) &&
              KeyEquals(b.key, key))
            return b.value;
          bucketNumber = (int)(((long)bucketNumber + incr) % (uint)_buckets.Length);
        } while (b.hashCollision < 0 && ++ntry < _buckets.Length);
        return null;
      }
      set { Insert(key, value, false); }
    }

    // Increases the bucket count of this hashtable. This method is called from
    // the Insert method when the actual load factor of the hashtable reaches
    // the upper limit specified when the hashtable was constructed. The number
    // of buckets in the hashtable is increased to the smallest prime number
    // that is larger than twice the current number of buckets, and the entries
    // in the hashtable are redistributed into the new buckets using the cached
    // hashcodes.
    private void expand()
    {

      int rawsize = GetNextPrime(_buckets.Length * 2);    // buckets.Length*2 will not overflow
      rehash(rawsize);
    }

    /// <summary>Re-hashes all entries in a new bucket array. Called occasionally due to large occupancy or when exanding.</summary>
    /// <param name="newsize">The size of the new bucket array.</param>
    private void rehash(int newsize)
    {
      _occupancy = 0;
      bucket[] newBuckets = new bucket[newsize];
      // Rehash table into a new bucket array
      for (int i = 0; i < _buckets.Length; i++)
        if ((_buckets[i].key != null) && (_buckets[i].key != _buckets))
          putEntry(newBuckets, _buckets[i].key, _buckets[i].value, _buckets[i].hashCollision & 0x7FFFFFFF);
      _buckets = newBuckets;
      _loadsize = (int)(_loadFactor * newsize);
      return;
    }

    // Internal method to get the hash code for an Object.  This will call
    // GetHashCode() on each object if you haven't provided an IHashCodeProvider
    // instance.  Otherwise, it calls hcp.GetHashCode(obj).
    protected virtual int GetHash(Object key) { return key.GetHashCode(); }

    // Internal method to compare two keys.  If you have provided an IComparer
    // instance in the constructor, this method will call comparer.Compare(item, key).
    // Otherwise, it will call item.Equals(key).
    //
    protected virtual bool KeyEquals(Object item, Object key)
    {
      if (Object.ReferenceEquals(_buckets, item))
      {
        return false;
      }

      return item == null ? false : item.Equals(key);
    }

    /// <summary>Adds an entry with the given key and value to this hashtable.</summary>
    /// <param name="key">The left value of the entry.</param>
    /// <param name="value">The right value of the entry.</param>
    public virtual void Add(Object key, Object value) { Insert(key, value, true); }

    // Inserts an entry into this hashtable. This method is called from the Set
    // and Add methods. If the add parameter is true and the given key already
    // exists in the hashtable, an exception is thrown.
    private void Insert(Object key, Object nvalue, bool add)
    {
      if (key == null)
        throw new HashTableException("Attempting to insert a null object to the table.");
      if (_count >= _loadsize)
        expand();
      else if (_occupancy > _loadsize && _count > 100)
        rehash(_buckets.Length);
      uint seed;
      uint incr;
      uint hashcode = ComputeHash(key, _buckets.Length, out seed, out incr);
      int ntry = 0;
      int emptySlotNumber = -1;
      int bucketNumber = (int)(seed % (uint)_buckets.Length);
      do
      {
        if (emptySlotNumber == -1 && (_buckets[bucketNumber].key == _buckets) && (_buckets[bucketNumber].hashCollision < 0))//(((buckets[bucketNumber].hash_coll & unchecked(0x80000000))!=0)))
          emptySlotNumber = bucketNumber;
        if ((_buckets[bucketNumber].key == null) || (_buckets[bucketNumber].key == _buckets && ((_buckets[bucketNumber].hashCollision & unchecked(0x80000000)) == 0)))
        {
          if (emptySlotNumber != -1)
            bucketNumber = emptySlotNumber;
          _buckets[bucketNumber].value = nvalue;
          _buckets[bucketNumber].key = key;
          _buckets[bucketNumber].hashCollision |= (int)hashcode;
          _count++;
          return;
        }
        if (((_buckets[bucketNumber].hashCollision & 0x7FFFFFFF) == hashcode) && KeyEquals(_buckets[bucketNumber].key, key))
        {
          if (add)
            throw new ArgumentException();
          _buckets[bucketNumber].value = nvalue;
          return;
        }
        if (emptySlotNumber == -1 && _buckets[bucketNumber].hashCollision >= 0)
        {
          _buckets[bucketNumber].hashCollision |= unchecked((int)0x80000000);
          _occupancy++;
        }
        bucketNumber = (int)(((long)bucketNumber + incr) % (uint)_buckets.Length);
      } while (++ntry < _buckets.Length);

      // This code is here if and only if there were no buckets without a collision bit set in the entire table
      if (emptySlotNumber != -1)
      {
        // We pretty much have to insert in this order.  Don't set hash
        // code until the value & key are set appropriately.
        _buckets[emptySlotNumber].value = nvalue;
        _buckets[emptySlotNumber].key = key;
        _buckets[emptySlotNumber].hashCollision |= (int)hashcode;
        _count++;
        return;
      }
      throw new HashTableException("There is a glitch in my hash table if you see this. Sorry...");
    }

    private void putEntry(bucket[] newBuckets, Object key, Object nvalue, int hashcode)
    {
      uint seed = (uint)hashcode;
      uint incr = (uint)(1 + (((seed >> 5) + 1) % ((uint)newBuckets.Length - 1)));
      int bucketNumber = (int)(seed % (uint)newBuckets.Length);
      do
      {

        if ((newBuckets[bucketNumber].key == null) || (newBuckets[bucketNumber].key == _buckets))
        {
          newBuckets[bucketNumber].value = nvalue;
          newBuckets[bucketNumber].key = key;
          newBuckets[bucketNumber].hashCollision |= hashcode;
          return;
        }

        if (newBuckets[bucketNumber].hashCollision >= 0)
        {
          newBuckets[bucketNumber].hashCollision |= unchecked((int)0x80000000);
          _occupancy++;
        }
        bucketNumber = (int)(((long)bucketNumber + incr) % (uint)newBuckets.Length);
      } while (true);
    }

    // Removes an entry from this hashtable. If an entry with the specified
    // key exists in the hashtable, it is removed. An ArgumentException is
    // thrown if the key is null.
    public virtual void Remove(Object key)
    {
      if (key == null)
        throw new HashTableException("Attempting to remove null from hash table.");

      uint seed;
      uint incr;
      // Assuming only one concurrent writer, write directly into buckets.
      uint hashcode = ComputeHash(key, _buckets.Length, out seed, out incr);
      int ntry = 0;

      int bn = (int)(seed % (uint)_buckets.Length);  // bucketNumber
      do
      {
        if (((_buckets[bn].hashCollision & 0x7FFFFFFF) == hashcode) && KeyEquals(_buckets[bn].key, key))
        {
          // Clear hash_coll field, then key, then value
          _buckets[bn].hashCollision &= unchecked((int)0x80000000);
          if (_buckets[bn].hashCollision != 0)
            _buckets[bn].key = _buckets;
          else
            _buckets[bn].key = null;
          _buckets[bn].value = null;  // Free object references sooner & simplify ContainsValue.
          _count--;
          return;
        }
        bn = (int)(((long)bn + incr) % (uint)_buckets.Length);
      } while (_buckets[bn].hashCollision < 0 && ++ntry < _buckets.Length);
      throw new HashTableException("Attempting to remove a non-existing key.");
    }

    /// <summary>This is used for throwing imutable priority queue exceptions only to make debugging faster.</summary>
    private class HashTableException : Exception { public HashTableException(string message) : base(message) { } }
  }

  #endregion
}