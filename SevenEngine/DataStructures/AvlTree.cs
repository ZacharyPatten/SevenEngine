// SEVENENGINE LISCENSE:
// You are free to use, modify, and distribute any or all code segments/files for any purpose
// including commercial use under the following condition: any code using or originally taken 
// from the SevenEngine project must include citation to its original author(s) located at the
// top of each source code file, or you may include a reference to the SevenEngine project as
// a whole but you must include the current SevenEngine official website URL and logo.
// - Thanks.  :)  (support: seven@sevenengine.com)

// Author(s):
// - Zachary Aaron Patten (aka Seven) seven@sevenengine.com
// Special Thanks:
// - AVL Trees were originally invented by G. M. Adelson-Velskii and E. M. Landis in 1962
// Last Edited: 11-16-13

using System;
using System.Threading;
using SevenEngine.DataStructures;

namespace SevenEngine.DataStructures
{
  public interface AvlTree<Type> : DataStructure<Type>
  {
    void Add(Type addition);
    bool TryAdd(Type addition);
    void Remove(Type removal);
    bool TryRemove(Type removal);
    int Count { get; }
    bool IsEmpty { get; }
    void Clear();

    bool Contains<Key>(Key key, Func<Type, Key, int> comparison);
    Type Get<Key>(Key get, Func<Type, Key, int> comparison);
    bool TryGet<Key>(Key get, Func<Type, Key, int> comparison, out Type item);
    void Remove<Key>(Key removal, Func<Type, Key, int> comparison);
    bool TryRemove<Key>(Key removal, Func<Type, Key, int> comparison);
  }

  #region AvlTreeLinked<Type>

  /// <summary>Implements an AVL Tree where the items are sorted by string id values.</summary>
  /// <remarks>The runtimes of each public member are included in the "remarks" xml tags.</remarks>
  [Serializable]
  public class AvlTreeLinked<Type> : AvlTree<Type>
  {
    #region AvlTreeNodeLinked

    /// <summary>This class just holds the data for each individual node of the tree.</summary>
    public class AvlTreeLinkedNode
    {
      private Type _value;
      private AvlTreeLinkedNode _leftChild;
      private AvlTreeLinkedNode _rightChild;
      private int _height;

      public Type Value { get { return _value; } set { _value = value; } }
      public AvlTreeLinkedNode LeftChild { get { return _leftChild; } set { _leftChild = value; } }
      public AvlTreeLinkedNode RightChild { get { return _rightChild; } set { _rightChild = value; } }
      public int Height { get { return _height; } set { _height = value; } }

      public AvlTreeLinkedNode(Type value)
      {
        _value = value;
        _leftChild = null;
        _rightChild = null;
        _height = 0;
      }
    }

    #endregion

    protected AvlTreeLinkedNode _avlTree;
    protected int _count;

    protected Func<Type, Type, int> _valueComparisonFunction;

    /// <summary>WARNING! Allows direct access to the tree structure. Can corrupt tree if miss-used</summary>
    public AvlTreeLinkedNode UnsafeTop { get { return _avlTree; } set { _avlTree = value; } }

    /// <summary>Gets the number of elements in the collection.</summary>
    /// <remarks>Runtime: O(1).</remarks>
    public int Count { get { return _count; } }
    /// <summary>Gets whether the binary search tree is empty.</summary>
    /// <remarks>Runtime: O(1).</remarks>
    public bool IsEmpty { get { return _avlTree == null; } }

    /// <summary>Constructs an AVL Tree.</summary>
    /// <param name="valueComparisonFunction">A function that returns negative if left is less that right, 
    /// zero if they are equal, or positive if left is greater than right.</param>
    /// <param name="keyComparisonFunction">A function that returns negative if left is less that right, 
    /// zero if they are equal, or positive if left is greater than right.</param>
    /// <remarks>Runtime: O(1).</remarks>
    public AvlTreeLinked(
      Func<Type, Type, int> valueComparisonFunction)
    {
      _avlTree = null;
      _count = 0;
      _valueComparisonFunction = valueComparisonFunction;
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

    public bool Contains<Key>(Key key, Func<Type, Key, int> comparison)
    {
      // THIS THIS THE ITERATIVE VERSION OF THIS FUNCTION. THERE IS A RECURSIVE
      // VERSION IN THE "RECURSIVE" REGION SHOULD YOU WISH TO SEE IT.
      AvlTreeLinkedNode _current = _avlTree;
      while (_current != null)
      {
        int compareResult = comparison(_current.Value, key);
        if (compareResult == 0)
        {
          return true;
        }
        else if (compareResult > 0) _current = _current.LeftChild;
        else _current = _current.RightChild;
      }
      return false;
    }

    /// <summary>Gets the item with the designated by the string.</summary>
    /// <param name="id">The string ID to look for.</param>
    /// <returns>The object with the desired string ID if it exists.</returns>
    /// <remarks>Runtime: O(ln(n)), Omega(1).</remarks>
    public Type Get<Key>(Key get, Func<Type, Key, int> comparison)
    {
      // THIS THIS THE ITERATIVE VERSION OF THIS FUNCTION. THERE IS A RECURSIVE
      // VERSION IN THE "RECURSIVE" REGION SHOULD YOU WISH TO SEE IT.
      AvlTreeLinkedNode _current = _avlTree;
      while (_current != null)
      {
        int compareResult = comparison(_current.Value, get);
        if (compareResult == 0)
        {
          return _current.Value;
        }
        else if (compareResult > 0) _current = _current.LeftChild;
        else _current = _current.RightChild;
      }
      throw new AvlTreeLinkedException("Attempting to get a non-existing value: " + get.ToString() + ".");
    }

    /// <summary>Gets the item with the designated by the string.</summary>
    /// <param name="id">The string ID to look for.</param>
    /// <returns>The object with the desired string ID if it exists.</returns>
    /// <remarks>Runtime: O(ln(n)), Omega(1).</remarks>
    public bool TryGet<Key>(Key get, Func<Type, Key, int> comparison, out Type result)
    {
      // THIS THIS THE ITERATIVE VERSION OF THIS FUNCTION. THERE IS A RECURSIVE
      // VERSION IN THE "RECURSIVE" REGION SHOULD YOU WISH TO SEE IT.
      AvlTreeLinkedNode _current = _avlTree;
      while (_current != null)
      {
        int compareResult = comparison(_current.Value, get);
        if (compareResult == 0)
        {
          result = _current.Value;
          return true;
        }
        else if (compareResult > 0) _current = _current.LeftChild;
        else _current = _current.RightChild;
      }
      result = default(Type);
      return false;
    }

    /// <summary>Adds an object to the AVL Tree.</summary>
    /// <param name="addition">The object to add.</param>
    /// <remarks>Runtime: Theta(ln(n)).</remarks>
    public void Add(Type addition)
    {
      _avlTree = Add(addition, _avlTree);
      _count++;
    }

    protected AvlTreeLinkedNode Add(Type addition, AvlTreeLinkedNode avlTree)
    {
      if (avlTree == null) return new AvlTreeLinkedNode(addition);
      int compareResult = _valueComparisonFunction(avlTree.Value, addition);
      if (compareResult == 0)
        throw new AvlTreeLinkedException("Attempting to add an already existing id exists.");
      else if (compareResult > 0) avlTree.LeftChild = Add(addition, avlTree.LeftChild);
      else avlTree.RightChild = Add(addition, avlTree.RightChild);
      return Balance(avlTree);
    }

    /// <summary>Adds an object to the AVL Tree.</summary>
    /// <param name="addition">The object to add.</param>
    /// <remarks>Runtime: Theta(ln(n)).</remarks>
    public bool TryAdd(Type addition)
    {
      bool added;
      _avlTree = TryAdd(addition, _avlTree, out added);
      _count++;
      return added;
    }

    protected AvlTreeLinkedNode TryAdd(Type addition, AvlTreeLinkedNode avlTree, out bool added)
    {
      if (avlTree == null)
      {
        added = true;
        return new AvlTreeLinkedNode(addition);
      }
      int compareResult = _valueComparisonFunction(avlTree.Value, addition);
      if (compareResult == 0)
      {
        added = false;
        return avlTree;
      }
      else if (compareResult > 0) avlTree.LeftChild = TryAdd(addition, avlTree.LeftChild, out added);
      else avlTree.RightChild = TryAdd(addition, avlTree.RightChild, out added);
      return Balance(avlTree);
    }

    /// <summary>Removes an object from the AVL Tree.</summary>
    /// <param name="removal">The string ID of the object to remove.</param>
    /// <remarks>Runtime: Theta(ln(n)).</remarks>
    public void Remove(Type removal)
    {
      _avlTree = Remove(removal, _avlTree);
      _count--;
    }

    /// <summary>Removes an object from the AVL Tree.</summary>
    /// <param name="removal">The string ID of the object to remove.</param>
    /// <param name="avlTree">The binary tree to remove from.</param>
    /// <returns>The resulting tree after the removal.</returns>
    /// <remarks>Runtime: Theta(ln(n)).</remarks>
    protected AvlTreeLinkedNode Remove(Type removal, AvlTreeLinkedNode avlTree)
    {
      if (avlTree != null)
      {
        int compareResult = _valueComparisonFunction(avlTree.Value, removal);
        if (compareResult == 0)
        {
          if (avlTree.RightChild != null)
          {
            AvlTreeLinkedNode leftMostOfRight;
            avlTree.RightChild = RemoveLeftMost(avlTree.RightChild, out leftMostOfRight);
            leftMostOfRight.RightChild = avlTree.RightChild;
            leftMostOfRight.LeftChild = avlTree.LeftChild;
            avlTree = leftMostOfRight;
          }
          else if (avlTree.LeftChild != null)
          {
            AvlTreeLinkedNode rightMostOfLeft;
            avlTree.LeftChild = RemoveRightMost(avlTree.LeftChild, out rightMostOfLeft);
            rightMostOfLeft.RightChild = avlTree.RightChild;
            rightMostOfLeft.LeftChild = avlTree.LeftChild;
            avlTree = rightMostOfLeft;
          }
          else return null;
          SetHeight(avlTree);
          return Balance(avlTree);
        }
        else if (compareResult > 0) avlTree.LeftChild = Remove(removal, avlTree.LeftChild);
        else avlTree.RightChild = Remove(removal, avlTree.RightChild);
        SetHeight(avlTree);
        return Balance(avlTree);
      }
      throw new AvlTreeLinkedException("Attempting to remove a non-existing entry.");
    }

    /// <summary>Removes an object from the AVL Tree.</summary>
    /// <param name="removal">The string ID of the object to remove.</param>
    /// <remarks>Runtime: Theta(ln(n)).</remarks>
    public bool TryRemove(Type removal)
    {
      try
      {
        _avlTree = Remove(removal, _avlTree);
        _count--;
        return true;
      }
      catch
      {
        return false;
      }
    }

    /// <summary>Removes an object from the AVL Tree.</summary>
    /// <param name="removal">The string ID of the object to remove.</param>
    /// <remarks>Runtime: Theta(ln(n)).</remarks>
    public void Remove<Key>(Key removal, Func<Type, Key, int> comparison)
    {
      _avlTree = Remove(removal, comparison, _avlTree);
      _count--;
    }

    /// <summary>Removes an object from the AVL Tree.</summary>
    /// <param name="removal">The string ID of the object to remove.</param>
    /// <param name="avlTree">The binary tree to remove from.</param>
    /// <returns>The resulting tree after the removal.</returns>
    /// <remarks>Runtime: Theta(ln(n)).</remarks>
    protected AvlTreeLinkedNode Remove<Key>(Key removal, Func<Type, Key, int> comparison, AvlTreeLinkedNode avlTree)
    {
      if (avlTree != null)
      {
        int compareResult = comparison(avlTree.Value, removal);
        if (compareResult == 0)
        {
          if (avlTree.RightChild != null)
          {
            AvlTreeLinkedNode leftMostOfRight;
            avlTree.RightChild = RemoveLeftMost(avlTree.RightChild, out leftMostOfRight);
            leftMostOfRight.RightChild = avlTree.RightChild;
            leftMostOfRight.LeftChild = avlTree.LeftChild;
            avlTree = leftMostOfRight;
          }
          else if (avlTree.LeftChild != null)
          {
            AvlTreeLinkedNode rightMostOfLeft;
            avlTree.LeftChild = RemoveRightMost(avlTree.LeftChild, out rightMostOfLeft);
            rightMostOfLeft.RightChild = avlTree.RightChild;
            rightMostOfLeft.LeftChild = avlTree.LeftChild;
            avlTree = rightMostOfLeft;
          }
          else return null;
          SetHeight(avlTree);
          return Balance(avlTree);
        }
        else if (compareResult > 0) avlTree.LeftChild = Remove<Key>(removal, comparison, avlTree.LeftChild);
        else avlTree.RightChild = Remove<Key>(removal, comparison, avlTree.RightChild);
        SetHeight(avlTree);
        return Balance(avlTree);
      }
      throw new AvlTreeLinkedException("Attempting to remove a non-existing entry.");
    }

    /// <summary>Removes an object from the AVL Tree.</summary>
    /// <param name="removal">The string ID of the object to remove.</param>
    /// <remarks>Runtime: Theta(ln(n)).</remarks>
    public bool TryRemove<Key>(Key removal, Func<Type, Key, int> comparison)
    {
      try
      {
        _avlTree = Remove(removal, comparison, _avlTree);
        _count--;
        return true;
      }
      catch
      {
        return false;
      }
    }

    /// <summary>Removes the left-most child of an AVL Tree node and returns it 
    /// through the out parameter.</summary>
    /// <param name="avlTree">The tree to remove the left-most child from.</param>
    /// <param name="leftMost">The left-most child of this AVL tree.</param>
    /// <returns>The updated tree with the removal.</returns>
    /// <remarks>Runtime: Theta(ln(n)).</remarks>
    protected AvlTreeLinkedNode RemoveLeftMost(AvlTreeLinkedNode avlTree, out AvlTreeLinkedNode leftMost)
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
    protected AvlTreeLinkedNode RemoveRightMost(AvlTreeLinkedNode avlTree, out AvlTreeLinkedNode rightMost)
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
    protected int Height(AvlTreeLinkedNode avlTree)
    {
      if (avlTree == null) return -1;
      else return avlTree.Height;
    }

    /// <summary>Sets the height of a tree based on its children's heights.</summary>
    /// <param name="avlTree">The tree to have its height adjusted.</param>
    /// <remarks>Runtime: O(1).</remarks>
    protected void SetHeight(AvlTreeLinkedNode avlTree)
    {
      if (Height(avlTree.LeftChild) < Height(avlTree.RightChild))
        avlTree.Height = Math.Max(Height(avlTree.LeftChild), Height(avlTree.RightChild)) + 1;
    }

    /// <summary>Standard balancing algorithm for an AVL Tree.</summary>
    /// <param name="avlTree">The tree to check the balancing of.</param>
    /// <returns>The result of the possible balancing.</returns>
    /// <remarks>Runtime: O(1).</remarks>
    protected AvlTreeLinkedNode Balance(AvlTreeLinkedNode avlTree)
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
    protected AvlTreeLinkedNode SingleRotateRight(AvlTreeLinkedNode avlTree)
    {
      AvlTreeLinkedNode temp = avlTree.LeftChild;
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
    protected AvlTreeLinkedNode SingleRotateLeft(AvlTreeLinkedNode avlTree)
    {
      AvlTreeLinkedNode temp = avlTree.RightChild;
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
    protected AvlTreeLinkedNode DoubleRotateRight(AvlTreeLinkedNode avlTree)
    {
      AvlTreeLinkedNode temp = avlTree.LeftChild.RightChild;
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
    protected AvlTreeLinkedNode DoubleRotateLeft(AvlTreeLinkedNode avlTree)
    {
      AvlTreeLinkedNode temp = avlTree.RightChild.LeftChild;
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
    public void Clear() {_avlTree = null; _count = 0; }

    /// <summary>Performs a functional paradigm in-order traversal of the AVL tree.</summary>
    /// <param name="traversalFunction">The function to perform during iteration.</param>
    /// <remarks>Runtime: O(n * traversalFunction).</remarks>
    public bool TraverseBreakable(Func<Type, bool> traversalFunction) { return TraversalInOrder(traversalFunction); }

    /// <summary>Performs a functional paradigm in-order traversal of the AVL tree.</summary>
    /// <param name="traversalAction">The action to perform during iteration.</param>
    /// <remarks>Runtime: O(n * traversalFunction).</remarks>
    public void Traverse(Action<Type> traversalAction) { TraversalInOrder(traversalAction); }

    /// <summary>Performs a functional paradigm in-order traversal of the AVL tree.</summary>
    /// <param name="traversalFunction">The function to perform during iteration.</param>
    /// <remarks>Runtime: O(n * traversalFunction).</remarks>
    public bool TraversalInOrder(Func<Type, bool> traversalFunction)
    {
      if (!TraversalInOrder(traversalFunction, _avlTree))
      {
        return false;
      }
      return true;
    }
    protected bool TraversalInOrder(Func<Type, bool> traversalFunction, AvlTreeLinkedNode avltreeNode)
    {
      if (avltreeNode != null)
      {
        if (!TraversalInOrder(traversalFunction, avltreeNode.LeftChild)) return false;
        if (!traversalFunction(avltreeNode.Value)) return false;
        if (!TraversalInOrder(traversalFunction, avltreeNode.RightChild)) return false;
      }
      return true;
    }

    public void TraversalInOrder(Action<Type> traversalFunction)
    {
      TraversalInOrder(traversalFunction, _avlTree);
    }
    protected void TraversalInOrder(Action<Type> traversalFunction, AvlTreeLinkedNode avltreeNode)
    {
      if (avltreeNode != null)
      {
        TraversalInOrder(traversalFunction, avltreeNode.LeftChild);
        traversalFunction(avltreeNode.Value);
        TraversalInOrder(traversalFunction, avltreeNode.RightChild);
      }
    }

    /// <summary>Performs a functional paradigm post-order traversal of the AVL tree.</summary>
    /// <param name="traversalFunction">The function to perform during iteration.</param>
    /// <remarks>Runtime: O(n * traversalFunction).</remarks>
    public bool TraversalPostOrder(Func<Type, bool> traversalFunction)
    {
      if (TraversalPostOrder(traversalFunction, _avlTree))
        return false;
      return true;
    }
    protected bool TraversalPostOrder(Func<Type, bool> traversalFunction, AvlTreeLinkedNode avltreeNode)
    {
      if (avltreeNode != null)
      {
        if (!TraversalPostOrder(traversalFunction, avltreeNode.RightChild)) return false;
        if (!traversalFunction(avltreeNode.Value)) return false;
        if (!TraversalPostOrder(traversalFunction, avltreeNode.LeftChild)) return false;
      }
      return true;
    }


    /// <summary>Performs a functional paradigm pre-order traversal of the AVL tree.</summary>
    /// <param name="traversalFunction">The function to perform during iteration.</param>
    /// <remarks>Runtime: O(n * traversalFunction).</remarks>
    public bool TraversalPreOrder(Func<Type, bool> traversalFunction)
    {
      if (!TraversalPreOrder(traversalFunction, _avlTree))
        return false;
      return true;
    }
    protected bool TraversalPreOrder(Func<Type, bool> traversalFunction, AvlTreeLinkedNode avltreeNode)
    {
      if (avltreeNode != null)
      {
        if (!traversalFunction(avltreeNode.Value)) return false;
        if (!TraversalPreOrder(traversalFunction, avltreeNode.LeftChild)) return false;
        if (!TraversalPreOrder(traversalFunction, avltreeNode.RightChild)) return false;
      }
      return true;
    }

    /// <summary>Creates an array out of the values in this structure.</summary>
    /// <returns>An array containing the values in this structure.</returns>
    /// <remarks>Runtime: Theta(n),</remarks>
    public Type[] ToArray() { return ToArrayInOrder(); }

    /// <summary>Puts all the items in the tree into a list by alphabetical order.</summary>
    /// <returns>The alphabetized list of items.</returns>
    /// <remarks>Runtime: Theta(n).</remarks>
    public Type[] ToArrayInOrder()
    {
      Type[] array = new Type[_count];
      ToArrayInOrder(array, _avlTree, 0);
      return array;
    }
    protected void ToArrayInOrder(Type[] array, AvlTreeLinkedNode avltreeNode, int position)
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
      Type[] array = new Type[_count];
      ToArrayPostOrder(array, _avlTree, 0);
      return array;
    }
    protected void ToArrayPostOrder(Type[] array, AvlTreeLinkedNode avltreeNode, int position)
    {
      if (avltreeNode != null)
      {
        ToArrayInOrder(array, avltreeNode.RightChild, position);
        array[position++] = avltreeNode.Value;
        ToArrayInOrder(array, avltreeNode.LeftChild, position);
      }
    }

    /// <summary>This is used for throwing AVL Tree exceptions only to make debugging faster.</summary>
    protected class AvlTreeLinkedException : Exception { public AvlTreeLinkedException(string message) : base(message) { } }
  }

  #endregion

  #region AvlTreeLinkedThreadSafe<Type>

  /// <summary>Implements an AVL Tree where the items are sorted by string id values.</summary>
  /// <remarks>The runtimes of each public member are included in the "remarks" xml tags.</remarks>
  [Serializable]
  public class AvlTreeLinkedThreadSafe<Type> : AvlTree<Type>
  {
    #region AvlTreeNodeLinkedThreadSafe

    /// <summary>This class just holds the data for each individual node of the tree.</summary>
    public class AvlTreeLinkedNodeThreadSafe
    {
      private Type _value;
      private AvlTreeLinkedNodeThreadSafe _leftChild;
      private AvlTreeLinkedNodeThreadSafe _rightChild;
      private int _height;

      public Type Value { get { return _value; } set { _value = value; } }
      public AvlTreeLinkedNodeThreadSafe LeftChild { get { return _leftChild; } set { _leftChild = value; } }
      public AvlTreeLinkedNodeThreadSafe RightChild { get { return _rightChild; } set { _rightChild = value; } }
      public int Height { get { return _height; } set { _height = value; } }

      public AvlTreeLinkedNodeThreadSafe(Type value)
      {
        _value = value;
        _leftChild = null;
        _rightChild = null;
        _height = 0;
      }
    }

    #endregion

    protected AvlTreeLinkedNodeThreadSafe _avlTree;
    protected int _count;

    protected Func<Type, Type, int> _valueComparisonFunction;

    protected object _lock;
    protected int _readers;
    protected int _writers;

    /// <summary>WARNING! Allows direct access to the tree structure. Can corrupt tree if miss-used</summary>
    public AvlTreeLinkedNodeThreadSafe UnsafeTop { get { return _avlTree; } set { _avlTree = value; } }

    /// <summary>Gets the number of elements in the collection.</summary>
    /// <remarks>Runtime: O(1).</remarks>
    public int Count { get { ReaderLock(); int count = _count; ReaderUnlock(); return count; } }
    /// <summary>Gets whether the binary search tree is empty.</summary>
    /// <remarks>Runtime: O(1).</remarks>
    public bool IsEmpty { get { ReaderLock(); bool isEmpty = _avlTree == null; ReaderUnlock(); return isEmpty; } }

    /// <summary>Constructs an AVL Tree.</summary>
    /// <param name="valueComparisonFunction">A function that returns negative if left is less that right, 
    /// zero if they are equal, or positive if left is greater than right.</param>
    /// <param name="keyComparisonFunction">A function that returns negative if left is less that right, 
    /// zero if they are equal, or positive if left is greater than right.</param>
    /// <remarks>Runtime: O(1).</remarks>
    public AvlTreeLinkedThreadSafe(
      Func<Type, Type, int> valueComparisonFunction)
    {
      _avlTree = null;
      _count = 0;
      _lock = new object();
      _readers = 0;
      _writers = 0;
      _valueComparisonFunction = valueComparisonFunction;
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

    public bool Contains<Key>(Key key, Func<Type, Key, int> comparison)
    {
      // THIS THIS THE ITERATIVE VERSION OF THIS FUNCTION. THERE IS A RECURSIVE
      // VERSION IN THE "RECURSIVE" REGION SHOULD YOU WISH TO SEE IT.
      ReaderLock();
      AvlTreeLinkedNodeThreadSafe _current = _avlTree;
      while (_current != null)
      {
        int compareResult = comparison(_current.Value, key);
        if (compareResult == 0)
        {
          ReaderUnlock();
          return true;
        }
        else if (compareResult > 0) _current = _current.LeftChild;
        else _current = _current.RightChild;
      }
      ReaderUnlock();
      return false;
    }

    /// <summary>Gets the item with the designated by the string.</summary>
    /// <param name="id">The string ID to look for.</param>
    /// <returns>The object with the desired string ID if it exists.</returns>
    /// <remarks>Runtime: O(ln(n)), Omega(1).</remarks>
    public Type Get<Key>(Key get, Func<Type, Key, int> comparison)
    {
      // THIS THIS THE ITERATIVE VERSION OF THIS FUNCTION. THERE IS A RECURSIVE
      // VERSION IN THE "RECURSIVE" REGION SHOULD YOU WISH TO SEE IT.
      ReaderLock();
      AvlTreeLinkedNodeThreadSafe _current = _avlTree;
      while (_current != null)
      {
        int compareResult = comparison(_current.Value, get);
        if (compareResult == 0)
        {
          ReaderUnlock();
          return _current.Value;
        }
        else if (compareResult > 0) _current = _current.LeftChild;
        else _current = _current.RightChild;
      }
      ReaderUnlock();
      throw new AvlTreeLinkedException("Attempting to get a non-existing value: " + get.ToString() + ".");
    }

    /// <summary>Gets the item with the designated by the string.</summary>
    /// <param name="id">The string ID to look for.</param>
    /// <returns>The object with the desired string ID if it exists.</returns>
    /// <remarks>Runtime: O(ln(n)), Omega(1).</remarks>
    public bool TryGet<Key>(Key get, Func<Type, Key, int> comparison, out Type result)
    {
      // THIS THIS THE ITERATIVE VERSION OF THIS FUNCTION. THERE IS A RECURSIVE
      // VERSION IN THE "RECURSIVE" REGION SHOULD YOU WISH TO SEE IT.
      ReaderLock();
      AvlTreeLinkedNodeThreadSafe _current = _avlTree;
      while (_current != null)
      {
        int compareResult = comparison(_current.Value, get);
        if (compareResult == 0)
        {
          ReaderUnlock();
          result = _current.Value;
          return true;
        }
        else if (compareResult > 0) _current = _current.LeftChild;
        else _current = _current.RightChild;
      }
      ReaderUnlock();
      result = default(Type);
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

    protected AvlTreeLinkedNodeThreadSafe Add(Type addition, AvlTreeLinkedNodeThreadSafe avlTree)
    {
      if (avlTree == null) return new AvlTreeLinkedNodeThreadSafe(addition);
      int compareResult = _valueComparisonFunction(avlTree.Value, addition);
      if (compareResult == 0)
        throw new AvlTreeLinkedException("Attempting to add an already existing id exists.");
      else if (compareResult > 0) avlTree.LeftChild = Add(addition, avlTree.LeftChild);
      else avlTree.RightChild = Add(addition, avlTree.RightChild);
      return Balance(avlTree);
    }

    /// <summary>Adds an object to the AVL Tree.</summary>
    /// <param name="addition">The object to add.</param>
    /// <remarks>Runtime: Theta(ln(n)).</remarks>
    public bool TryAdd(Type addition)
    {
      WriterLock();
      bool added;
      _avlTree = TryAdd(addition, _avlTree, out added);
      _count++;
      WriterUnlock();
      return added;
    }

    protected AvlTreeLinkedNodeThreadSafe TryAdd(Type addition, AvlTreeLinkedNodeThreadSafe avlTree, out bool added)
    {
      if (avlTree == null)
      {
        added = true;
        return new AvlTreeLinkedNodeThreadSafe(addition);
      }
      int compareResult = _valueComparisonFunction(avlTree.Value, addition);
      if (compareResult == 0)
      {
        added = false;
        return avlTree;
      }
      else if (compareResult > 0) avlTree.LeftChild = TryAdd(addition, avlTree.LeftChild, out added);
      else avlTree.RightChild = TryAdd(addition, avlTree.RightChild, out added);
      return Balance(avlTree);
    }

    /// <summary>Removes an object from the AVL Tree.</summary>
    /// <param name="removal">The string ID of the object to remove.</param>
    /// <remarks>Runtime: Theta(ln(n)).</remarks>
    public void Remove(Type removal)
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
    protected AvlTreeLinkedNodeThreadSafe Remove(Type removal, AvlTreeLinkedNodeThreadSafe avlTree)
    {
      if (avlTree != null)
      {
        int compareResult = _valueComparisonFunction(avlTree.Value, removal);
        if (compareResult == 0)
        {
          if (avlTree.RightChild != null)
          {
            AvlTreeLinkedNodeThreadSafe leftMostOfRight;
            avlTree.RightChild = RemoveLeftMost(avlTree.RightChild, out leftMostOfRight);
            leftMostOfRight.RightChild = avlTree.RightChild;
            leftMostOfRight.LeftChild = avlTree.LeftChild;
            avlTree = leftMostOfRight;
          }
          else if (avlTree.LeftChild != null)
          {
            AvlTreeLinkedNodeThreadSafe rightMostOfLeft;
            avlTree.LeftChild = RemoveRightMost(avlTree.LeftChild, out rightMostOfLeft);
            rightMostOfLeft.RightChild = avlTree.RightChild;
            rightMostOfLeft.LeftChild = avlTree.LeftChild;
            avlTree = rightMostOfLeft;
          }
          else return null;
          SetHeight(avlTree);
          return Balance(avlTree);
        }
        else if (compareResult > 0) avlTree.LeftChild = Remove(removal, avlTree.LeftChild);
        else avlTree.RightChild = Remove(removal, avlTree.RightChild);
        SetHeight(avlTree);
        return Balance(avlTree);
      }
      WriterUnlock();
      throw new AvlTreeLinkedException("Attempting to remove a non-existing entry.");
    }

    /// <summary>Removes an object from the AVL Tree.</summary>
    /// <param name="removal">The string ID of the object to remove.</param>
    /// <remarks>Runtime: Theta(ln(n)).</remarks>
    public bool TryRemove(Type removal)
    {
      WriterLock();
      try
      {
        _avlTree = Remove(removal, _avlTree);
        _count--;
        WriterUnlock();
        return true;
      }
      catch
      {
        WriterUnlock();
        return false;
      }
    }

    /// <summary>Removes an object from the AVL Tree.</summary>
    /// <param name="removal">The string ID of the object to remove.</param>
    /// <remarks>Runtime: Theta(ln(n)).</remarks>
    public void Remove<Key>(Key removal, Func<Type, Key, int> comparison)
    {
      WriterLock();
      _avlTree = Remove(removal, comparison, _avlTree);
      _count--;
      WriterUnlock();
    }

    /// <summary>Removes an object from the AVL Tree.</summary>
    /// <param name="removal">The string ID of the object to remove.</param>
    /// <param name="avlTree">The binary tree to remove from.</param>
    /// <returns>The resulting tree after the removal.</returns>
    /// <remarks>Runtime: Theta(ln(n)).</remarks>
    protected AvlTreeLinkedNodeThreadSafe Remove<Key>(Key removal, Func<Type, Key, int> comparison, AvlTreeLinkedNodeThreadSafe avlTree)
    {
      if (avlTree != null)
      {
        int compareResult = comparison(avlTree.Value, removal);
        if (compareResult == 0)
        {
          if (avlTree.RightChild != null)
          {
            AvlTreeLinkedNodeThreadSafe leftMostOfRight;
            avlTree.RightChild = RemoveLeftMost(avlTree.RightChild, out leftMostOfRight);
            leftMostOfRight.RightChild = avlTree.RightChild;
            leftMostOfRight.LeftChild = avlTree.LeftChild;
            avlTree = leftMostOfRight;
          }
          else if (avlTree.LeftChild != null)
          {
            AvlTreeLinkedNodeThreadSafe rightMostOfLeft;
            avlTree.LeftChild = RemoveRightMost(avlTree.LeftChild, out rightMostOfLeft);
            rightMostOfLeft.RightChild = avlTree.RightChild;
            rightMostOfLeft.LeftChild = avlTree.LeftChild;
            avlTree = rightMostOfLeft;
          }
          else return null;
          SetHeight(avlTree);
          return Balance(avlTree);
        }
        else if (compareResult > 0) avlTree.LeftChild = Remove<Key>(removal, comparison, avlTree.LeftChild);
        else avlTree.RightChild = Remove<Key>(removal, comparison, avlTree.RightChild);
        SetHeight(avlTree);
        return Balance(avlTree);
      }
      WriterUnlock();
      throw new AvlTreeLinkedException("Attempting to remove a non-existing entry.");
    }

    /// <summary>Removes an object from the AVL Tree.</summary>
    /// <param name="removal">The string ID of the object to remove.</param>
    /// <remarks>Runtime: Theta(ln(n)).</remarks>
    public bool TryRemove<Key>(Key removal, Func<Type, Key, int> comparison)
    {
      WriterLock();
      try
      {
        _avlTree = Remove(removal, comparison, _avlTree);
        _count--;
        WriterUnlock();
        return true;
      }
      catch
      {
        WriterUnlock();
        return false;
      }
    }

    /// <summary>Removes the left-most child of an AVL Tree node and returns it 
    /// through the out parameter.</summary>
    /// <param name="avlTree">The tree to remove the left-most child from.</param>
    /// <param name="leftMost">The left-most child of this AVL tree.</param>
    /// <returns>The updated tree with the removal.</returns>
    /// <remarks>Runtime: Theta(ln(n)).</remarks>
    protected AvlTreeLinkedNodeThreadSafe RemoveLeftMost(AvlTreeLinkedNodeThreadSafe avlTree, out AvlTreeLinkedNodeThreadSafe leftMost)
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
    protected AvlTreeLinkedNodeThreadSafe RemoveRightMost(AvlTreeLinkedNodeThreadSafe avlTree, out AvlTreeLinkedNodeThreadSafe rightMost)
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
    protected int Height(AvlTreeLinkedNodeThreadSafe avlTree)
    {
      if (avlTree == null) return -1;
      else return avlTree.Height;
    }

    /// <summary>Sets the height of a tree based on its children's heights.</summary>
    /// <param name="avlTree">The tree to have its height adjusted.</param>
    /// <remarks>Runtime: O(1).</remarks>
    protected void SetHeight(AvlTreeLinkedNodeThreadSafe avlTree)
    {
      if (Height(avlTree.LeftChild) < Height(avlTree.RightChild))
        avlTree.Height = Math.Max(Height(avlTree.LeftChild), Height(avlTree.RightChild)) + 1;
    }

    /// <summary>Standard balancing algorithm for an AVL Tree.</summary>
    /// <param name="avlTree">The tree to check the balancing of.</param>
    /// <returns>The result of the possible balancing.</returns>
    /// <remarks>Runtime: O(1).</remarks>
    protected AvlTreeLinkedNodeThreadSafe Balance(AvlTreeLinkedNodeThreadSafe avlTree)
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
    protected AvlTreeLinkedNodeThreadSafe SingleRotateRight(AvlTreeLinkedNodeThreadSafe avlTree)
    {
      AvlTreeLinkedNodeThreadSafe temp = avlTree.LeftChild;
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
    protected AvlTreeLinkedNodeThreadSafe SingleRotateLeft(AvlTreeLinkedNodeThreadSafe avlTree)
    {
      AvlTreeLinkedNodeThreadSafe temp = avlTree.RightChild;
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
    protected AvlTreeLinkedNodeThreadSafe DoubleRotateRight(AvlTreeLinkedNodeThreadSafe avlTree)
    {
      AvlTreeLinkedNodeThreadSafe temp = avlTree.LeftChild.RightChild;
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
    protected AvlTreeLinkedNodeThreadSafe DoubleRotateLeft(AvlTreeLinkedNodeThreadSafe avlTree)
    {
      AvlTreeLinkedNodeThreadSafe temp = avlTree.RightChild.LeftChild;
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

    /// <summary>Performs a functional paradigm in-order traversal of the AVL tree.</summary>
    /// <param name="traversalFunction">The function to perform during iteration.</param>
    /// <remarks>Runtime: O(n * traversalFunction).</remarks>
    public bool TraverseBreakable(Func<Type, bool> traversalFunction) { return TraversalInOrder(traversalFunction); }

    /// <summary>Performs a functional paradigm in-order traversal of the AVL tree.</summary>
    /// <param name="traversalAction">The action to perform during iteration.</param>
    /// <remarks>Runtime: O(n * traversalFunction).</remarks>
    public void Traverse(Action<Type> traversalAction) { TraversalInOrder(traversalAction); }

    /// <summary>Performs a functional paradigm in-order traversal of the AVL tree.</summary>
    /// <param name="traversalFunction">The function to perform during iteration.</param>
    /// <remarks>Runtime: O(n * traversalFunction).</remarks>
    public bool TraversalInOrder(Func<Type, bool> traversalFunction)
    {
      ReaderLock();
      if (!TraversalInOrder(traversalFunction, _avlTree))
      {
        ReaderUnlock();
        return false;
      }
      ReaderUnlock();
      return true;
    }
    protected bool TraversalInOrder(Func<Type, bool> traversalFunction, AvlTreeLinkedNodeThreadSafe avltreeNode)
    {
      if (avltreeNode != null)
      {
        if (!TraversalInOrder(traversalFunction, avltreeNode.LeftChild)) return false;
        if (!traversalFunction(avltreeNode.Value)) return false;
        if (!TraversalInOrder(traversalFunction, avltreeNode.RightChild)) return false;
      }
      return true;
    }

    public void TraversalInOrder(Action<Type> traversalFunction)
    {
      ReaderLock();
      TraversalInOrder(traversalFunction, _avlTree);
      ReaderUnlock();
    }
    protected void TraversalInOrder(Action<Type> traversalFunction, AvlTreeLinkedNodeThreadSafe avltreeNode)
    {
      if (avltreeNode != null)
      {
        TraversalInOrder(traversalFunction, avltreeNode.LeftChild);
        traversalFunction(avltreeNode.Value);
        TraversalInOrder(traversalFunction, avltreeNode.RightChild);
      }
    }

    /// <summary>Performs a functional paradigm post-order traversal of the AVL tree.</summary>
    /// <param name="traversalFunction">The function to perform during iteration.</param>
    /// <remarks>Runtime: O(n * traversalFunction).</remarks>
    public bool TraversalPostOrder(Func<Type, bool> traversalFunction)
    {
      ReaderLock();
      if (TraversalPostOrder(traversalFunction, _avlTree))
      {
        ReaderUnlock();
        return false;
      }
      ReaderUnlock();
      return true;
    }
    protected bool TraversalPostOrder(Func<Type, bool> traversalFunction, AvlTreeLinkedNodeThreadSafe avltreeNode)
    {
      if (avltreeNode != null)
      {
        if (!TraversalPostOrder(traversalFunction, avltreeNode.RightChild)) return false;
        if (!traversalFunction(avltreeNode.Value)) return false;
        if (!TraversalPostOrder(traversalFunction, avltreeNode.LeftChild)) return false;
      }
      return true;
    }


    /// <summary>Performs a functional paradigm pre-order traversal of the AVL tree.</summary>
    /// <param name="traversalFunction">The function to perform during iteration.</param>
    /// <remarks>Runtime: O(n * traversalFunction).</remarks>
    public bool TraversalPreOrder(Func<Type, bool> traversalFunction)
    {
      ReaderLock();
      if (!TraversalPreOrder(traversalFunction, _avlTree))
      {
        ReaderUnlock();
        return false;
      }
      ReaderUnlock();
      return true;
    }
    protected bool TraversalPreOrder(Func<Type, bool> traversalFunction, AvlTreeLinkedNodeThreadSafe avltreeNode)
    {
      if (avltreeNode != null)
      {
        if (!traversalFunction(avltreeNode.Value)) return false;
        if (!TraversalPreOrder(traversalFunction, avltreeNode.LeftChild)) return false;
        if (!TraversalPreOrder(traversalFunction, avltreeNode.RightChild)) return false;
      }
      return true;
    }

    /// <summary>Creates an array out of the values in this structure.</summary>
    /// <returns>An array containing the values in this structure.</returns>
    /// <remarks>Runtime: Theta(n),</remarks>
    public Type[] ToArray() { return ToArrayInOrder(); }

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
    protected void ToArrayInOrder(Type[] array, AvlTreeLinkedNodeThreadSafe avltreeNode, int position)
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
    protected void ToArrayPostOrder(Type[] array, AvlTreeLinkedNodeThreadSafe avltreeNode, int position)
    {
      if (avltreeNode != null)
      {
        ToArrayInOrder(array, avltreeNode.RightChild, position);
        array[position++] = avltreeNode.Value;
        ToArrayInOrder(array, avltreeNode.LeftChild, position);
      }
    }

    /// <summary>Thread safe enterance for readers.</summary>
    protected void ReaderLock() { lock (_lock) { while (!(_writers == 0)) Monitor.Wait(_lock); _readers++; } }
    /// <summary>Thread safe exit for readers.</summary>
    protected void ReaderUnlock() { lock (_lock) { _readers--; Monitor.Pulse(_lock); } }
    /// <summary>Thread safe enterance for writers.</summary>
    protected void WriterLock() { lock (_lock) { while (!(_writers == 0) && !(_readers == 0)) Monitor.Wait(_lock); _writers++; } }
    /// <summary>Thread safe exit for readers.</summary>
    protected void WriterUnlock() { lock (_lock) { _writers--; Monitor.PulseAll(_lock); } }

    /// <summary>This is used for throwing AVL Tree exceptions only to make debugging faster.</summary>
    protected class AvlTreeLinkedException : Exception { public AvlTreeLinkedException(string message) : base(message) { } }
  }

  #endregion
}