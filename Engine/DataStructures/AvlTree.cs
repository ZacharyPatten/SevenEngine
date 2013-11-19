// SEVENENGINE LISCENSE:
// You are free to use, modify, and distribute any or all code segments/files for any purpose
// including commercial use with the following condition: any code using or originally taken 
// from the SevenEngine project must include citation to its original author(s) located at the
// top of each source code file, or you may include a reference to the SevenEngine project as
// a whole but you must include the current SevenEngine official website URL and logo.
// - Thanks.  :)  (support: seven@sevenengine.com)

// Author(s):
// - Zachary Aaron Patten (aka Seven) seven@sevenengine.com
//   - Contribution: All but the remove method.
// - Nicholas Boen
//   - Contribution: The remove method.
// Special Thanks:
// - AVL Trees were originally invented by G. M. Adelson-Velskii and E. M. Landis in 1962
// Last Edited: 11-16-13

// This file contains the following classes:
// - AvlTree
//   - AvlTreeNode
//   - AvlTreeEnumerator
//   - AvlTreeException

// Notes:
// This class uses the Many-Readers-One-Writer multithreading pattern. This probably not 
// the most efficient multithreading pattern, but it will protect the tree if multithreaded.

using System;
using System.Threading;
using SevenEngine;

namespace SevenEngine.DataStructures
{
  #region AvlTree

  /// <summary>Implements an AVL Tree where the items are sorted by string id values.</summary>
  /// <remarks>The runtimes of each public member are included in the "remarks" xml tags.</remarks>
  public class AvlTree<Type>// : System.Collections.IEnumerable
    where Type : SevenEngine.DataStructures.Interfaces.InterfaceStringId
  {
    #region AvlTreeNode

    /// <summary>This class just holds the data for each individual node of the tree.</summary>
    private class AvlTreeNode
    {
      private Type _value;
      private AvlTreeNode _leftChild;
      private AvlTreeNode _rightChild;
      private int _height;

      // private AvlTreeNode _next;
      // private AvlTreeNode _previous;

      internal Type Value { get { return _value; } set { _value = value; } }
      internal AvlTreeNode LeftChild { get { return _leftChild; } set { _leftChild = value; } }
      internal AvlTreeNode RightChild { get { return _rightChild; } set { _rightChild = value; } }
      internal int Height { get { return _height; } set { _height = value; } }

      // internal AvlTreeNode Next { get { return _next; } set { _next = value; } }
      // internal AvlTreeNode Previous { get { return _previous; } set { _previous = value; } }

      internal AvlTreeNode(Type value, int height)
      {
        _value = value;
        _leftChild = null;
        _rightChild = null;
        _height = height;
        // Note: removed threaded tree functionality
        // _next = next;
        // _previous = null;
      }
    }

    #endregion
    
    private AvlTreeNode _avlTree;
    private int _count;

    // private AvlTreeNode _head;

    private Lock _lock;
    private int _readers;
    private int _writers;
    private int _waitingWriters;

    /// <summary>Gets the number of elements in the collection.</summary>
    /// <remarks>Runtime: O(1).</remarks>
    public int Count { get { return _count; } }
    /// <summary>Gets whether the binary search tree is empty.</summary>
    /// <remarks>Runtime: O(1).</remarks>
    public bool IsEmpty { get { return _avlTree == null; } }

    /// <summary>Constructs an AVL Tree.</summary>
    /// <remarks>Runtime: O(1).</remarks>
    public AvlTree()
    {
      _avlTree = null;
      _count = 0;

      _lock = new Lock();
      _readers = 0;
      _writers = 0;
      _waitingWriters = 0;
    }

    /// <summary>Gets the item with the designated by the string.</summary>
    /// <param name="id">The string ID to look for.</param>
    /// <returns>The object with the desired string ID if it exists.</returns>
    /// <remarks>Runtime: O(ln(n)), Omega(1).</remarks>
    public Type Get(string id)
    {
      lock (_lock)
      {
        while (!(_writers == 0)) { Monitor.Wait(_lock); }
        _readers++;
      }
      #region Recursive
      // Type returnValue = Get(id, _avlTree);
      // lock (_lock)
      //{
      //  _readers--;
      //  Monitor.Pulse(_lock);
      //}
      // return returnValue;
      #endregion
      #region Iterative
      AvlTreeNode _current = _avlTree;
      while (_current != null)
      {
        int compareResult = id.CompareTo(_current.Value.Id);
        if (compareResult == 0)
        {
          lock (_lock)
          {
            _readers--;
            Monitor.Pulse(_lock);
          }
          return _current.Value;
        }
        else if (compareResult < 0)
          _current = _current.LeftChild;
        else
          _current = _current.RightChild;
      }
      throw new AvlTreeException("Attempting to get a non-existing value: " + id + ".");
      #endregion
    }

    /// <summary>Standard AVL Tree searching algorithm using recursion.</summary>
    /// <param name="id">The string ID to look for.</param>
    /// <param name="avlTree">The AVL Tree node to look in.</param>
    /// <returns>The object in the AVL Tree with the desired string ID.</returns>
    /// <remarks>Runtime: O(ln(n)), Omega(1).</remarks>
    private Type Get(string id, AvlTreeNode avlTree)
    {
      if (avlTree == null)
        throw new AvlTreeException("Attempting to get a non-existing value: " + id + ".");
      int compResult = id.CompareTo(avlTree.Value.Id);
      if (compResult == 0)
        return avlTree.Value;
      else if (compResult < 0)
        return Get(id, avlTree.LeftChild);
      else
        return Get(id, avlTree.RightChild);
    }

    /// <summary>Checks to see if the tree contains a specific key.</summary>
    /// <param name="id">The id to check for existance.</param>
    /// <returns>"true" if the key exists; "false" if the key does not exist.</returns>
    /// <remarks>Runtime: O(ln(n)).</remarks>
    public bool Contains(string id)
    {
      lock (_lock)
      {
        while (!(_writers == 0)) { Monitor.Wait(_lock); }
        _readers++;
      }
      #region Recursive
      // bool returnValue = Contains(id, _avlTree);
      // lock (_lock)
      //{
      //  _readers--;
      //  Monitor.Pulse(_lock);
      //}
      // return returnValue;
      #endregion
      #region Iterative
      AvlTreeNode _current = _avlTree;
      while (_current != null)
      {
        int compareResult = id.CompareTo(_current.Value.Id);
        if (compareResult == 0)
        {
          lock (_lock)
          {
            _readers--;
            Monitor.Pulse(_lock);
          }
          return true;
        }
        else if (compareResult < 0)
          _current = _current.LeftChild;
        else
          _current = _current.RightChild;
      }
      return false;
      #endregion
    }

    /// <summary>Checks to see if the tree contains a specific key.</summary>
    /// <param name="id">The id to check for existance.</param>
    /// <param name="avlTree">The tree to check for key existance in.</param>
    /// <returns>"true" if the key exists; "false" if the key does not exist.</returns>
    /// <remarks>Runtime: O(ln(n)).</remarks>
    private bool Contains(string id, AvlTreeNode avlTree)
    {
      if (avlTree == null)
        return false;
      int compareResult = id.CompareTo(avlTree.Value.Id);
      if (compareResult == 0)
        return true;
      else if (compareResult < 0)
        return Contains(id, avlTree.LeftChild);
      else
        return Contains(id, avlTree.RightChild);
    }

    /// <summary>Adds an object to the AVL Tree.</summary>
    /// <param name="id">The string id that you can use to look and remove up the object.</param>
    /// <param name="addition">The object to add.</param>
    /// <remarks>Runtime: Theta(ln(n)).</remarks>
    public void Add(Type addition)
    {
      lock (_lock)
      {
        _waitingWriters++;
        while (!(_writers == 0) && !(_readers == 0)) { Monitor.Wait(_lock); }
        _waitingWriters--;
        _writers++;
      }
      #region Recursive
      _avlTree = Add(addition, _avlTree);
      #endregion
      #region Iterative
      #endregion
      _count++;
      lock (_lock)
      {
        _writers--;
        Monitor.PulseAll(_lock);
      }
    }

    /// <summary>Adds an object to the AVL Tree.</summary>
    /// <param name="addition">The object to add.</param>
    /// <param name="avlTree">The binary search tree to add to.</param>
    /// <returns>The resulting tree.</returns>
    /// <remarks>Runtime: Theta(ln(n)).</remarks>
    private AvlTreeNode Add(Type addition, AvlTreeNode avlTree)
    {
      if (avlTree == null)
      {
        //if (_head == null)
        //  return _head = new AvlTreeNode(addition, 0, null);
        //_head.Previous = new AvlTreeNode(addition, 0, _head);
        //_head = _head.Previous;
        //return _head;
        return  new AvlTreeNode(addition, 0);
      }
      else
      {
        int compResult = addition.Id.CompareTo(avlTree.Value.Id);
        if (compResult == 0)
          throw new AvlTreeException("Attempting to add an already existing id exists.");
        else if (compResult < 0)
          avlTree.LeftChild = Add(addition, avlTree.LeftChild);
        else
          avlTree.RightChild = Add(addition, avlTree.RightChild);
        return Balance(avlTree);
      }
    }

    /// <summary>Removes an object from the AVL Tree.</summary>
    /// <param name="removal">The string ID of the object to remove.</param>
    /// <remarks>Runtime: Theta(ln(n)).</remarks>
    public void Remove(string removal)
    {
      lock (_lock)
      {
        _waitingWriters++;
        while (!(_writers == 0) && !(_readers == 0)) { Monitor.Wait(_lock); }
        _waitingWriters--;
        _writers++;
      }
      #region Recursive
      _avlTree = Remove(removal, _avlTree);
      #endregion
      #region Iterative
      #endregion
      _count--;
      lock (_lock)
      {
        _writers--;
        Monitor.PulseAll(_lock);
      }
    }

    /// <summary>Removes an object from the AVL Tree.</summary>
    /// <param name="removal">The string ID of the object to remove.</param>
    /// <param name="avlTree">The binary tree to remove from.</param>
    /// <returns>The resulting tree after the removal.</returns>
    /// <remarks>Runtime: Theta(ln(n)).</remarks>
    private AvlTreeNode Remove(string removal, AvlTreeNode avlTree)
    {
      if (avlTree != null)
      {
        int compResult = removal.CompareTo(avlTree.Value.Id);
        if (compResult == 0)
        {
          if (avlTree.RightChild != null)
          {
            AvlTreeNode leftMostOfRight;
            avlTree.RightChild = RemoveLeftMost(avlTree.RightChild, out leftMostOfRight);
            leftMostOfRight.RightChild = avlTree.RightChild;
            leftMostOfRight.LeftChild = avlTree.LeftChild;
            avlTree = leftMostOfRight;
          }
          else if (avlTree.LeftChild != null)
          {
            AvlTreeNode rightMostOfLeft;
            avlTree.LeftChild = RemoveRightMost(avlTree.LeftChild, out rightMostOfLeft);
            rightMostOfLeft.RightChild = avlTree.RightChild;
            rightMostOfLeft.LeftChild = avlTree.LeftChild;
            avlTree = rightMostOfLeft;
          }
          else
            return null;
          SetHeight(avlTree);
          return Balance(avlTree);
        }
        else if (compResult < 0)
          avlTree.LeftChild = Remove(removal, avlTree.LeftChild);
        else
          avlTree.RightChild = Remove(removal, avlTree.RightChild);
        SetHeight(avlTree);
        return Balance(avlTree);
      }
      else
        throw new AvlTreeException("Attempting to remove a non-existing entry.");
    }

    /// <summary>Removes the left-most child of an AVL Tree node and returns it through the out parameter.</summary>
    /// <param name="avlTree">The tree to remove the left-most child from.</param>
    /// <param name="leftMost">The left-most child of this AVL tree.</param>
    /// <returns>The updated tree with the removal.</returns>
    /// <remarks>Runtime: Theta(ln(n)).</remarks>
    private AvlTreeNode RemoveLeftMost(AvlTreeNode avlTree, out AvlTreeNode leftMost)
    {
      if (avlTree.LeftChild == null)
      {
        leftMost = avlTree;
        return null;
      }
      avlTree.LeftChild = RemoveLeftMost(avlTree.LeftChild, out leftMost);
      SetHeight(avlTree);
      return Balance(avlTree);
    }

    /// <summary>Removes the right-most child of an AVL Tree node and returns it through the out parameter.</summary>
    /// <param name="avlTree">The tree to remove the right-most child from.</param>
    /// <param name="leftMost">The right-most child of this AVL tree.</param>
    /// <returns>The updated tree with the removal.</returns>
    /// <remarks>Runtime: Theta(ln(n)).</remarks>
    private AvlTreeNode RemoveRightMost(AvlTreeNode avlTree, out AvlTreeNode rightMost)
    {
      if (avlTree.RightChild == null)
      {
        rightMost = avlTree;
        return null;
      }
      avlTree.LeftChild = RemoveLeftMost(avlTree.RightChild, out rightMost);
      SetHeight(avlTree);
      return Balance(avlTree);
    }

    /// <summary>This is just a protection against the null valued leaf nodes, which have a height of "-1".</summary>
    /// <param name="avlTree">The AVL Tree to find the hight of.</param>
    /// <returns>Returns "-1" if null (leaf) or the height property of the node.</returns>
    /// <remarks>Runtime: O(1).</remarks>
    private int Height(AvlTreeNode avlTree)
    {
      if (avlTree == null)
        return -1;
      else
        return avlTree.Height;
    }

    /// <summary>Sets the height of a tree based on its children's heights.</summary>
    /// <param name="avlTree">The tree to have its height adjusted.</param>
    /// <remarks>Runtime: O(1).</remarks>
    private void SetHeight(AvlTreeNode avlTree)
    {
      // Functionality Note: imutable or mutable (next three lines)
      if (Height(avlTree.LeftChild) < Height(avlTree.RightChild))
        // t.Root = new Link<Type, int>(t.Id, Math.Max(Height(t.LeftChild), Height(t.RightChild)) + 1);
        avlTree.Height = Math.Max(Height(avlTree.LeftChild), Height(avlTree.RightChild)) + 1;
    }

    /// <summary>Standard balancing algorithm for an AVL Tree.</summary>
    /// <param name="avlTree">The tree to check the balancing of.</param>
    /// <returns>The result of the possible balancing.</returns>
    /// <remarks>Runtime: O(1).</remarks>
    private AvlTreeNode Balance(AvlTreeNode avlTree)
    {
      if (Height(avlTree.LeftChild) == Height(avlTree.RightChild) + 2)
      {
        if (Height(avlTree.LeftChild.LeftChild) > Height(avlTree.RightChild))
          return SingleRotateRight(avlTree);
        else
          return DoubleRotateRight(avlTree);
      }
      else if (Height(avlTree.RightChild) == Height(avlTree.LeftChild) + 2)
      {
        if (Height(avlTree.RightChild.RightChild) > Height(avlTree.LeftChild))
          return SingleRotateLeft(avlTree);
        else
          return DoubleRotateLeft(avlTree);
      }
      SetHeight(avlTree);
      return avlTree;
    }

    /// <summary>Standard single rotation (to the right) algorithm for an AVL Tree.</summary>
    /// <param name="avlTree">The tree to single rotate right.</param>
    /// <returns>The resulting tree.</returns>
    /// <remarks>Runtime: O(1).</remarks>
    private AvlTreeNode SingleRotateRight(AvlTreeNode avlTree)
    {
      AvlTreeNode temp = avlTree.LeftChild;
      avlTree.LeftChild = temp.RightChild;
      temp.RightChild = avlTree;
      SetHeight(avlTree);
      SetHeight(temp);
      return temp;
    }

    /// <summary>Standard single rotation (to the left) algorithm for an AVL Tree.</summary>
    /// <param name="avlTree">The tree to single rotate left.</param>
    /// <returns>The resulting tree.</returns>
    /// <remarks>Runtime: O(1).</remarks>
    private AvlTreeNode SingleRotateLeft(AvlTreeNode avlTree)
    {
      AvlTreeNode temp = avlTree.RightChild;
      avlTree.RightChild = temp.LeftChild;
      temp.LeftChild = avlTree;
      SetHeight(avlTree);
      SetHeight(temp);
      return temp;
    }

    /// <summary>Standard double rotation (to the right) algorithm for an AVL Tree.</summary>
    /// <param name="avlTree">The tree to float rotate right.</param>
    /// <returns>The resulting tree.</returns>
    /// <remarks>Runtime: O(1).</remarks>
    private AvlTreeNode DoubleRotateRight(AvlTreeNode avlTree)
    {
      AvlTreeNode temp = avlTree.LeftChild.RightChild;
      avlTree.LeftChild.RightChild = temp.LeftChild;
      temp.LeftChild = avlTree.LeftChild;
      avlTree.LeftChild = temp.RightChild;
      temp.RightChild = avlTree;
      SetHeight(temp.LeftChild);
      SetHeight(temp.RightChild);
      SetHeight(temp);
      return temp;
    }

    /// <summary>Standard double rotation (to the left) algorithm for an AVL Tree.</summary>
    /// <param name="avlTree">The tree to float rotate left.</param>
    /// <returns>The resulting tree.</returns>
    /// <remarks>Runtime: O(1).</remarks>
    private AvlTreeNode DoubleRotateLeft(AvlTreeNode avlTree)
    {
      AvlTreeNode temp = avlTree.RightChild.LeftChild;
      avlTree.RightChild.LeftChild = temp.RightChild;
      temp.RightChild = avlTree.RightChild;
      avlTree.RightChild = temp.LeftChild;
      temp.LeftChild = avlTree;
      SetHeight(temp.LeftChild);
      SetHeight(temp.RightChild);
      SetHeight(temp);
      return temp;
    }

    /// <summary>A function to be used in a tree traversal.</summary>
    /// <param name="id">The id of the current node.</param>
    /// <param name="node">The current node of a traversal.</param>
    public delegate void TraversalFunction(Type node);

    /// <summary>Allows an alphebetical ordered traversal using a delegate.</summary>
    /// <param name="traversalFunction">The function to perform per node in the traversal.</param>
    /// <remarks>Runtime: O(n * traversalFunction).</remarks>
    public void AlphabeticalTraversal(TraversalFunction traversalFunction)
    {
      lock (_lock)
      {
        while (!(_writers == 0)) { Monitor.Wait(_lock); }
        _readers++;
      }
      AlphabeticalTraversal(traversalFunction, _avlTree);
      lock (_lock)
      {
        _readers--;
        Monitor.Pulse(_lock);
      }
    }
    private void AlphabeticalTraversal(TraversalFunction traversalFunction, AvlTreeNode avltreeNode)
    {
      if (avltreeNode != null)
      {
        AlphabeticalTraversal(traversalFunction, avltreeNode.LeftChild);
        traversalFunction(avltreeNode.Value);
        AlphabeticalTraversal(traversalFunction, avltreeNode.RightChild);
      }
    }

    /// <summary>Allows a reverse alphabetical ordered using a delegate.</summary>
    /// <param name="traversalFunction">The function to perform per node in the traversal.</param>
    /// <remarks>Runtime: O(n * traversalFunction).</remarks>
    public void ReverseAlphabeticalTraversal(TraversalFunction traversalFunction)
    {
      lock (_lock)
      {
        while (!(_writers == 0)) { Monitor.Wait(_lock); }
        _readers++;
      }
      ReverseAlphabeticalTraversal(traversalFunction, _avlTree);
      lock (_lock)
      {
        _readers--;
        Monitor.Pulse(_lock);
      }
    }
    private void ReverseAlphabeticalTraversal(TraversalFunction traversalFunction, AvlTreeNode avltreeNode)
    {
      if (avltreeNode != null)
      {
        ReverseAlphabeticalTraversal(traversalFunction, avltreeNode.RightChild);
        traversalFunction(avltreeNode.Value);
        ReverseAlphabeticalTraversal(traversalFunction, avltreeNode.LeftChild);
      }
    }

    //// The following "IEnumerable" functions allow you to use a "foreach"
    //// loop on this AvlTree class.
    //public System.Collections.IEnumerator GetEnumerator()
    //{ return new AvlTreeEnumerator(_head); }
    //// THIS COUL BE DONE MORE EFFICIENTLY! It will require stacks to 
    //// track the traversal of the tree.
    //private class AvlTreeEnumerator : System.Collections.IEnumerator
    //{
    //  private AvlTreeNode _head;
    //  private AvlTreeNode _current;
      
    //  public AvlTreeEnumerator(AvlTreeNode list) { 
    //    _current = _head = new AvlTreeNode(default(Type), 0, list); }

    //  public object Current { get { return _current.Value; } }
    //  public bool MoveNext()
    //  {
    //    if (_current.Next != null) { _current = _current.Next; return true; }
    //    return false;
    //  }
    //  public void Reset() { 
    //    _current = _head; }
    //}

    /// <summary>Puts all the items in the tree into a list by alphabetical order.</summary>
    /// <returns>The alphabetized list of items.</returns>
    /// <remarks>Runtime: Theta(n).</remarks>
    public Type[] ToArray()
    {
      lock (_lock)
      {
        while (!(_writers == 0)) { Monitor.Wait(_lock); }
        _readers++;
      }
      Type[] array = new Type[_count];
      ToArray(array, _avlTree, 0);
      lock (_lock)
      {
        _readers--;
        Monitor.Pulse(_lock);
      }
      return array;
    }
    private void ToArray(Type[] array, AvlTreeNode avltreeNode, int position)
    {
      if (avltreeNode != null)
      {
        ToArray(array, avltreeNode.RightChild, position);
        array[position++] = avltreeNode.Value;
        ToArray(array, avltreeNode.LeftChild, position);
      }
    }

    /// <summary>Represents a lockable object for multi-threading.</summary>
    private class Lock { internal Lock() { } }

    /// <summary>This is used for throwing AVL Tree exceptions only to make debugging faster.</summary>
    private class AvlTreeException : Exception { public AvlTreeException(string message) : base(message) { } }
  }

  #endregion
}