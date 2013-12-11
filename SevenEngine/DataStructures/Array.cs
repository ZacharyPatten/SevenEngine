// SEVENENGINE LISCENSE:
// You are free to use, modify, and distribute any or all code segments/files for any purpose
// including commercial use under the following condition: any code using or originally taken 
// from the SevenEngine project must include citation to its original author(s) located at the
// top of each source code file, or you may include a reference to the SevenEngine project as
// a whole but you must include the current SevenEngine official website URL and logo.
// - Thanks.  :)  (support: seven@sevenengine.com)

// Author(s):
// - Zachary Aaron Patten (aka Seven) seven@sevenengine.com
// Last Edited: 11-22-13

// This file contains the following interfaces:
// - Array
// This file contains the following classes:
// - ArrayStandard
//   - ArrayStandardException
// - ArrayCyclic
//   - ArrayCyclicException

using System;
using System.Threading;
using SevenEngine.DataStructures.Interfaces;

namespace SevenEngine.DataStructures
{
  public interface Array<Type> : InterfaceTraversable<Type>
  {
    Type this[int index] { get; set; }
    int Length { get; }
    bool TraverseBreakable(Func<Type, bool> traversalFunction, int start, int end);
    void Traverse(Action<Type> traversalAction, int start, int end);
  }

  #region Array

  /// <summary>Implements a standard array that inherits InterfaceTraversable.</summary>
  /// <typeparam name="Type">The generic type within the structure.</typeparam>
  public class ArrayStandard<Type> : Array<Type>
  {
    private Type[] _array;

    private Object _lock;
    private int _readers;
    private int _writers;

    /// <summary>The length of the array.</summary>
    public int Length { get { return _array.Length; } }

    /// <summary>Allows indexed access of the array.</summary>
    /// <param name="index">The index of the array to get/set.</param>
    /// <returns>The value at the desired index.</returns>
    public Type this[int index]
    {
      get
      {
        ReaderLock();
        if (index < 0 || index > _array.Length)
          throw new ArrayStandardException("index out of bounds.");
        Type returnValue = _array[index];
        ReaderUnlock();
        return returnValue;
      }
      set
      {
        WriterLock();
        if (index < 0 || index > _array.Length)
          throw new ArrayStandardException("index out of bounds.");
        _array[index] = value;
        WriterUnlock();
      }
    }

    /// <summary>Constructs an array that implements a traversal delegate function 
    /// which is an optimized "foreach" implementation.</summary>
    /// <param name="size">The length of the array in memory.</param>
    public ArrayStandard(int size)
    {
      if (size < 0)
        throw new ArrayStandardException("size of the array must be non-negative.");
      _array = new Type[size];
      _lock = new Object();
      _readers = 0;
      _writers = 0;
    }

    /// <summary>Performs a functional paradigm traversal of the array.</summary>
    /// <param name="traversalFunction">The function to perform during iteration.</param>
    /// <remarks>Runtime: O(n * traversalFunction).</remarks>
    public bool TraverseBreakable(Func<Type, bool> traversalFunction)
    {
      ReaderLock();
      for (int index = 0; index < _array.Length; index++)
      {
        if (!traversalFunction(_array[index++]))
        {
          ReaderUnlock();
          return false;
        }
      }
      ReaderUnlock();
      return true;
    }

    /// <summary>Performs a functional paradigm traversal of the array and allows for data structure optomization.</summary>
    /// <param name="traversalFunction">The function to perform during iteration.</param>
    /// <param name="start">The starting index of the traversal.</param>
    /// <param name="end">The ending index of the traversal.</param>
    /// <returns>Determines break functionality. (true = continue; false = break)</returns>
    /// <remarks>Runtime: O((end - start) * traversalFunction).</remarks>
    public bool TraverseBreakable(Func<Type, bool> traversalFunction, int start, int end)
    {
      ReaderLock();
      if (start > end || end > _array.Length || start < 0)
        throw new ArrayStandardException("start/end indeces out of bounds during traversal attempt.");
      for (int index = start; index < end; index++)
      {
        if (!traversalFunction(_array[index++]))
        {
          ReaderUnlock();
          return false;
        }
      }
      ReaderUnlock();
      return true;
    }

    /// <summary>Performs a functional paradigm traversal of the array.</summary>
    /// <param name="traversalAction">The action to perform during iteration.</param>
    /// <remarks>Runtime: O(n * traversalAction).</remarks>
    public void Traverse(Action<Type> traversalAction)
    {
      ReaderLock();
      for (int index = 0; index < _array.Length; index++)
        traversalAction(_array[index++]);
      ReaderUnlock();
    }

    /// <summary>Performs a functional paradigm traversal of the array and allows for data structure optomization.</summary>
    /// <param name="traversalAction">The action to perform during iteration.</param>
    /// <param name="start">The starting index of the traversal.</param>
    /// <param name="end">The ending index of the traversal.</param>
    /// <returns>Determines break functionality. (true = continue; false = break)</returns>
    /// <remarks>Runtime: O((end - start) * traversalAction).</remarks>
    public void Traverse(Action<Type> traversalAction, int start, int end)
    {
      ReaderLock();
      if (start > end || end > _array.Length || start < 0)
        throw new ArrayStandardException("start/end indeces out of bounds during traversal attempt.");
      for (int index = start; index < end; index++)
        traversalAction(_array[index++]);
      ReaderUnlock();
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
    private class ArrayStandardException : Exception { public ArrayStandardException(string message) : base(message) { } }
  }

  #endregion

  #region ArrayCyclic

  /// <summary>Implements a cyclic array (allows overwriting) that inherits InterfaceTraversable.</summary>
  /// <typeparam name="Type">The generic type within the structure.</typeparam>
  public class ArrayCyclic<Type> : Array<Type>
  {
    private Type[] _array;
    int _start;
    int _count;

    private Object _lock;
    private int _readers;
    private int _writers;

    /// <summary>The length of the array.</summary>
    public int Length { get { return _array.Length; } }

    /// <summary>Allows indexed access of the array.</summary>
    /// <param name="index">The index of the array to get/set.</param>
    /// <returns>The value at the desired index.</returns>
    public Type this[int index]
    {
      get
      {
        ReaderLock();
        if (index < 0 || index > _count)
        {
          ReaderUnlock();
          throw new ArrayCyclicException("index out of bounds.");
        }
        Type returnValue = _array[(index + _start) % _array.Length];
        ReaderUnlock();
        return returnValue;
      }
      set
      {
        WriterLock();
        if (index < 0 || index > _count ||
          (index > _start + _count && index < ((_start + _count)) % _array.Length))
        {
          WriterUnlock();
          throw new ArrayCyclicException("index out of bounds.");
        }
        _array[(index + _start) % _array.Length] = value;
        WriterUnlock();
      }
    }

    /// <summary>Constructs an array that implements a traversal delegate function 
    /// which is an optimized "foreach" implementation.</summary>
    /// <param name="size">The length of the array in memory.</param>
    public ArrayCyclic(int size)
    {
      if (size < 0)
        throw new ArrayCyclicException("size of the array must be non-negative.");
      _array = new Type[size];
      _start = 0;
      _count = 0;
      _lock = new Object();
      _readers = 0;
      _writers = 0;
    }

    /// <summary>Adds a value to the current end of the cyclic array, will overwrite the begining of the array if it is full.</summary>
    /// <param name="addition">The value to be added.</param>
    public void Add(Type addition)
    {
      WriterLock();
      if (_count == _array.Length)
      {
        _array[_start++] = addition;
        _start %= _array.Length;
      }
      else
        _array[(_start + _count++) % _array.Length] = addition;
      WriterUnlock();
    }

    public void Remove(int index)
    {
      throw new NotImplementedException();
    }

    /// <summary>Performs a functional paradigm traversal of the array.</summary>
    /// <param name="traversalFunction">The function to perform during iteration.</param>
    /// <remarks>Runtime: O(n * traversalFunction).</remarks>
    public bool TraverseBreakable(Func<Type, bool> traversalFunction)
    {
      ReaderLock();
      for (int i = 0; i < _count; i++)
        if (!traversalFunction(_array[(i + _start) % _array.Length]))
        {
          ReaderUnlock();
          return false;
        }
      ReaderUnlock();
      return true;
    }

    /// <summary>Performs a functional paradigm traversal of the array and allows for data structure optomization.</summary>
    /// <param name="traversalFunction">The function to perform during iteration.</param>
    /// <param name="start">The starting index of the traversal.</param>
    /// <param name="end">The ending index of the traversal.</param>
    /// <returns>Determines break functionality. (true = continue; false = break)</returns>
    /// <remarks>Runtime: O(n * traversalFunction).</remarks>
    public bool TraverseBreakable(Func<Type, bool> traversalFunction, int start, int end)
    {
      ReaderLock();
      if (start > end || end > _array.Length || start < 0)
        throw new ArrayCyclicException("start/end indeces out of bounds during traversal attempt.");
      for (int i = start; i < end; i++)
        if (!traversalFunction(_array[(i + _start) % _array.Length]))
        {
          ReaderUnlock();
          return false;
        }
      ReaderUnlock();
      return true;
    }

    
    public void Traverse(Action<Type> traversalAction)
    {
      ReaderLock();
      for (int i = 0; i < _count; i++)
        traversalAction(_array[(i + _start) % _array.Length]);
      ReaderUnlock();
    }

    /// <summary>Performs a functional paradigm traversal of the array and allows for data structure optomization.</summary>
    /// <param name="traversalAction">The function to perform during iteration.</param>
    /// <param name="start">The starting index of the traversal.</param>
    /// <param name="end">The ending index of the traversal.</param>
    /// <returns>Determines break functionality. (true = continue; false = break)</returns>
    /// <remarks>Runtime: O(n * traversalAction).</remarks>
    public void Traverse(Action<Type> traversalAction, int start, int end)
    {
      ReaderLock();
      if (start > end || end > _array.Length || start < 0)
        throw new ArrayCyclicException("start/end indeces out of bounds during traversal attempt.");
      for (int i = start; i < end; i++)
        traversalAction(_array[(i + _start) % _array.Length]);
      ReaderUnlock();
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
    private class ArrayCyclicException : Exception { public ArrayCyclicException(string message) : base(message) { } }
  }

  #endregion
}