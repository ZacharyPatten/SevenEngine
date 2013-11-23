// SEVENENGINE LISCENSE:
// You are free to use, modify, and distribute any or all code segments/files for any purpose
// including commercial use under the following condition: any code using or originally taken 
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
//   - AvlTreeException

using System;
using System.Threading;
using SevenEngine.DataStructures.Interfaces;

namespace SevenEngine.DataStructures
{
  #region AvlTree

  /// <summary>Implements an AVL Tree where the items are sorted by string id values.</summary>
  /// <remarks>The runtimes of each public member are included in the "remarks" xml tags.</remarks>
  public class AvlTree<Type> : InterfaceTraversable<Type>
    where Type : InterfaceStringId
  {
    #region AvlTreeNode

    /// <summary>This class just holds the data for each individual node of the tree.</summary>
    private class AvlTreeNode
    {
      private Type _value;
      private AvlTreeNode _leftChild;
      private AvlTreeNode _rightChild;
      private int _height;

      internal Type Value { get { return _value; } set { _value = value; } }
      internal AvlTreeNode LeftChild { get { return _leftChild; } set { _leftChild = value; } }
      internal AvlTreeNode RightChild { get { return _rightChild; } set { _rightChild = value; } }
      internal int Height { get { return _height; } set { _height = value; } }

      internal AvlTreeNode(Type value)
      {
        _value = value;
        _leftChild = null;
        _rightChild = null;
        _height = 0;
      }
    }

    #endregion

    private AvlTreeNode _avlTree;
    private int _count;

    private Object _lock;
    private int _readers;
    private int _writers;

    /// <summary>Gets the number of elements in the collection.</summary>
    /// <remarks>Runtime: O(1).</remarks>
    public int Count { get { ReaderLock(); int count = _count; ReaderUnlock(); return _count; } }
    /// <summary>Gets whether the binary search tree is empty.</summary>
    /// <remarks>Runtime: O(1).</remarks>
    public bool IsEmpty { get { ReaderLock(); bool isEmpty = _avlTree == null; ReaderUnlock(); return isEmpty; } }

    /// <summary>Constructs an AVL Tree.</summary>
    /// <remarks>Runtime: O(1).</remarks>
    public AvlTree()
    {
      _avlTree = null;
      _count = 0;
      _lock = new Object();
      _readers = 0;
      _writers = 0;
    }

    #region Recursive Versions

    // THE FOLLOWING FUNCTIONS ARE RECURSIVE VERSIONS OF THE EXISTING 
    // MEMBERS WITHIN THIS CLASS. THEY HAVE THE SAME FUNCTIONALITY.

    //public Type Get(string id)
    //{
    //  ReaderLock();
    //   Type returnValue = Get(id, _avlTree);
    //   ReaderUnlock();
    //   return returnValue;
    //}

    //private Type Get(string id, AvlTreeNode avlTree)
    //{
    //  if (avlTree == null)
    //    throw new AvlTreeException("Attempting to get a non-existing value: " + id + ".");
    //  int compResult = id.CompareTo(avlTree.Value.Id);
    //  if (compResult == 0)
    //    return avlTree.Value;
    //  else if (compResult < 0)
    //    return Get(id, avlTree.LeftChild);
    //  else
    //    return Get(id, avlTree.RightChild);
    //}

    //public bool Contains(string id)
    //{
    //   ReaderLock();
    //   bool returnValue = Contains(id, _avlTree);
    //   ReaderUnlock();
    //   return returnValue;
    //}

    //private bool Contains(string id, AvlTreeNode avlTree)
    //{
    //  if (avlTree == null) return false;
    //  int compareResult = id.CompareTo(avlTree.Value.Id);
    //  if (compareResult == 0) return true;
    //  else if (compareResult < 0) return Contains(id, avlTree.LeftChild);
    //  else return Contains(id, avlTree.RightChild);
    //}

    #endregion

    /// <summary>Gets the item with the designated by the string.</summary>
    /// <param name="id">The string ID to look for.</param>
    /// <returns>The object with the desired string ID if it exists.</returns>
    /// <remarks>Runtime: O(ln(n)), Omega(1).</remarks>
    public Type Get(string id)
    {
      // THIS THIS THE ITERATIVE VERSION OF THIS FUNCTION. THERE IS A RECURSIVE
      // VERSION IN THE "RECURSIVE" REGION SHOULD YOU WISH TO SEE IT.
      ReaderLock();
      AvlTreeNode _current = _avlTree;
      while (_current != null)
      {
        int compareResult = id.CompareTo(_current.Value.Id);
        if (compareResult == 0)
        {
          ReaderUnlock();
          return _current.Value;
        }
        else if (compareResult < 0) _current = _current.LeftChild;
        else _current = _current.RightChild;
      }
      ReaderUnlock();
      throw new AvlTreeException("Attempting to get a non-existing value: " + id + ".");
    }

    /// <summary>Checks to see if the tree contains a specific key.</summary>
    /// <param name="id">The id to check for existance.</param>
    /// <returns>"true" if the key exists; "false" if the key does not exist.</returns>
    /// <remarks>Runtime: O(ln(n)).</remarks>
    public bool Contains(string id)
    {
      // THIS THIS THE ITERATIVE VERSION OF THIS FUNCTION. THERE IS A RECURSIVE
      // VERSION IN THE "RECURSIVE" REGION SHOULD YOU WISH TO SEE IT.
      ReaderLock();
      AvlTreeNode _current = _avlTree;
      while (_current != null)
      {
        int compareResult = id.CompareTo(_current.Value.Id);
        if (compareResult == 0)
        {
          ReaderUnlock();
          return true;
        }
        else if (compareResult < 0) _current = _current.LeftChild;
        else _current = _current.RightChild;
      }
      ReaderUnlock();
      return false;
    }

    /// <summary>Adds an object to the AVL Tree.</summary>
    /// <param name="addition">The object to add.</param>
    /// <remarks>Runtime: Theta(ln(n)).</remarks>
    public void Add(Type addition)
    {
      WriterLock();
      _avlTree = Add(addition, _avlTree);
      _count++;
      WriterUnlock();
    }

    private AvlTreeNode Add(Type addition, AvlTreeNode avlTree)
    {
      if (avlTree == null) return new AvlTreeNode(addition);
      int compResult = addition.Id.CompareTo(avlTree.Value.Id);
      if (compResult == 0)
        throw new AvlTreeException("Attempting to add an already existing id exists.");
      else if (compResult < 0) avlTree.LeftChild = Add(addition, avlTree.LeftChild);
      else avlTree.RightChild = Add(addition, avlTree.RightChild);
      return Balance(avlTree);
    }

    /// <summary>Removes an object from the AVL Tree.</summary>
    /// <param name="removal">The string ID of the object to remove.</param>
    /// <remarks>Runtime: Theta(ln(n)).</remarks>
    public void Remove(string removal)
    {
      WriterLock();
      _avlTree = Remove(removal, _avlTree);
      _count--;
      WriterUnlock();
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
          else return null;
          SetHeight(avlTree);
          return Balance(avlTree);
        }
        else if (compResult < 0) avlTree.LeftChild = Remove(removal, avlTree.LeftChild);
        else avlTree.RightChild = Remove(removal, avlTree.RightChild);
        SetHeight(avlTree);
        return Balance(avlTree);
      }
      WriterUnlock();
      throw new AvlTreeException("Attempting to remove a non-existing entry.");
    }

    /// <summary>Removes the left-most child of an AVL Tree node and returns it 
    /// through the out parameter.</summary>
    /// <param name="avlTree">The tree to remove the left-most child from.</param>
    /// <param name="leftMost">The left-most child of this AVL tree.</param>
    /// <returns>The updated tree with the removal.</returns>
    /// <remarks>Runtime: Theta(ln(n)).</remarks>
    private AvlTreeNode RemoveLeftMost(AvlTreeNode avlTree, out AvlTreeNode leftMost)
    {
      if (avlTree.LeftChild == null) { leftMost = avlTree; return null; }
      avlTree.LeftChild = RemoveLeftMost(avlTree.LeftChild, out leftMost);
      SetHeight(avlTree);
      return Balance(avlTree);
    }

    /// <summary>Removes the right-most child of an AVL Tree node and returns it 
    /// through the out parameter.</summary>
    /// <param name="avlTree">The tree to remove the right-most child from.</param>
    /// <param name="leftMost">The right-most child of this AVL tree.</param>
    /// <returns>The updated tree with the removal.</returns>
    /// <remarks>Runtime: Theta(ln(n)).</remarks>
    private AvlTreeNode RemoveRightMost(AvlTreeNode avlTree, out AvlTreeNode rightMost)
    {
      if (avlTree.RightChild == null) { rightMost = avlTree; return null; }
      avlTree.LeftChild = RemoveLeftMost(avlTree.RightChild, out rightMost);
      SetHeight(avlTree);
      return Balance(avlTree);
    }

    /// <summary>This is just a protection against the null valued leaf nodes, 
    /// which have a height of "-1".</summary>
    /// <param name="avlTree">The AVL Tree to find the hight of.</param>
    /// <returns>Returns "-1" if null (leaf) or the height property of the node.</returns>
    /// <remarks>Runtime: O(1).</remarks>
    private int Height(AvlTreeNode avlTree)
    {
      if (avlTree == null) return -1;
      else return avlTree.Height;
    }

    /// <summary>Sets the height of a tree based on its children's heights.</summary>
    /// <param name="avlTree">The tree to have its height adjusted.</param>
    /// <remarks>Runtime: O(1).</remarks>
    private void SetHeight(AvlTreeNode avlTree)
    {
      if (Height(avlTree.LeftChild) < Height(avlTree.RightChild))
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
        else return DoubleRotateRight(avlTree);
      }
      else if (Height(avlTree.RightChild) == Height(avlTree.LeftChild) + 2)
      {
        if (Height(avlTree.RightChild.RightChild) > Height(avlTree.LeftChild))
          return SingleRotateLeft(avlTree);
        else return DoubleRotateLeft(avlTree);
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

    /// <summary>Returns the tree to an iterative state.</summary>
    public void Clear() { WriterLock(); _avlTree = null; _count = 0; WriterUnlock(); }

    public delegate bool TraversalFunction(Type node);
    /// <summary>Implements a "foreach" loop using delegates. "break" is possible by returning false.
    /// The foreach is done in alphabetical order.</summary>
    /// <param name="traversalFunction">The function to perform per node in the traversal.</param>
    /// <remarks>Runtime: O(n * traversalFunction).</remarks>
    public void Traversal(Func<Type, bool> traversalFunction)
    {
      ReaderLock();
      Traversal(traversalFunction, _avlTree);
      ReaderUnlock();
    }
    private bool Traversal(Func<Type, bool> traversalFunction, AvlTreeNode avltreeNode)
    {
      if (avltreeNode != null)
      {
        if (!Traversal(traversalFunction, avltreeNode.LeftChild)) return false;
        if (!traversalFunction(avltreeNode.Value)) return false;
        if (!Traversal(traversalFunction, avltreeNode.RightChild)) return false;
      }
      return true;
    }

    /// <summary>Implements a "foreach" loop using delegates. "break" is possible by returning false.
    /// The foreach is done in alphabetical order.</summary>
    /// <param name="traversalFunction">The function to perform per node in the traversal.</param>
    /// <remarks>Runtime: O(n * traversalFunction).</remarks>
    public void TraversalInOrder(TraversalFunction traversalFunction)
    {
      ReaderLock();
      TraversalInOrder(traversalFunction, _avlTree);
      ReaderUnlock();
    }
    private bool TraversalInOrder(TraversalFunction traversalFunction, AvlTreeNode avltreeNode)
    {
      if (avltreeNode != null)
      {
        if (!TraversalInOrder(traversalFunction, avltreeNode.LeftChild)) return false;
        if (!traversalFunction(avltreeNode.Value)) return false;
        if (!TraversalInOrder(traversalFunction, avltreeNode.RightChild)) return false;
      }
      return true;
    }

    /// <summary>Implements a "foreach" loop using delegates. "break" is possible by returning false.
    /// The foreach is done in reverse alphabetical order.</summary>
    /// <param name="traversalFunction">The function to perform per node in the traversal.</param>
    /// <remarks>Runtime: O(n * traversalFunction).</remarks>
    public void TraversalPostOrder(TraversalFunction traversalFunction)
    {
      ReaderLock();
      TraversalPostOrder(traversalFunction, _avlTree);
      ReaderUnlock();
    }
    private bool TraversalPostOrder(TraversalFunction traversalFunction, AvlTreeNode avltreeNode)
    {
      if (avltreeNode != null)
      {
        if (!TraversalPostOrder(traversalFunction, avltreeNode.RightChild)) return false;
        if (!traversalFunction(avltreeNode.Value)) return false;
        if (!TraversalPostOrder(traversalFunction, avltreeNode.LeftChild)) return false;
      }
      return true;
    }

    /// <summary>Puts all the items in the tree into a list by alphabetical order.</summary>
    /// <returns>The alphabetized list of items.</returns>
    /// <remarks>Runtime: Theta(n).</remarks>
    public Type[] ToArrayInOrder()
    {
      ReaderLock();
      Type[] array = new Type[_count];
      ToArrayInOrder(array, _avlTree, 0);
      ReaderUnlock();
      return array;
    }
    private void ToArrayInOrder(Type[] array, AvlTreeNode avltreeNode, int position)
    {
      if (avltreeNode != null)
      {
        ToArrayInOrder(array, avltreeNode.LeftChild, position);
        array[position++] = avltreeNode.Value;
        ToArrayInOrder(array, avltreeNode.RightChild, position);
      }
    }

    /// <summary>Puts all the items in the tree into a list by reverse alphabetical order.</summary>
    /// <returns>The alphabetized list of items.</returns>
    /// <remarks>Runtime: Theta(n).</remarks>
    public Type[] ToArrayPostOrder()
    {
      ReaderLock();
      Type[] array = new Type[_count];
      ToArrayPostOrder(array, _avlTree, 0);
      ReaderUnlock();
      return array;
    }
    private void ToArrayPostOrder(Type[] array, AvlTreeNode avltreeNode, int position)
    {
      if (avltreeNode != null)
      {
        ToArrayInOrder(array, avltreeNode.RightChild, position);
        array[position++] = avltreeNode.Value;
        ToArrayInOrder(array, avltreeNode.LeftChild, position);
      }
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
    private class AvlTreeException : Exception { public AvlTreeException(string message) : base(message) { } }
  }

  #endregion
}