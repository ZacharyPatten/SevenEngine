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
  public interface RedBlackTree<Type> : DataStructure<Type>
  {
    void Add(Type addition);
    //bool TryAdd(Type addition);
    void Remove(Type removal);
    //bool TryRemove(Type removal);
    int Count { get; }
    bool IsEmpty { get; }
    void Clear();

    //bool Contains<Key>(Key key, Func<Type, Key, int> comparison);
    //Type Get<Key>(Key get, Func<Type, Key, int> comparison);
    //bool TryGet<Key>(Key get, Func<Type, Key, int> comparison, out Type item);
    //void Remove<Key>(Key removal, Func<Type, Key, int> comparison);
    //bool TryRemove<Key>(Key removal, Func<Type, Key, int> comparison);
  }

  #region RedBlackTreeLinked<Type>

  [Serializable]
  public class RedBlackTreeLinked<Type> : RedBlackTree<Type>
  {
    protected const bool Red = true;
    protected const bool Black = false;

    #region RedBlackTreeNode

    protected class RedBlackLinkedNode
    {
      private bool _color;
      private Type _value;
      private RedBlackLinkedNode _leftChild;
      private RedBlackLinkedNode _rightChild;
      private RedBlackLinkedNode _parent;

      internal bool Color { get { return _color; } set { _color = value; } }
      internal Type Value { get { return _value; } set { _value = value; } }
      internal RedBlackLinkedNode LeftChild { get { return _leftChild; } set { _leftChild = value; } }
      internal RedBlackLinkedNode RightChild { get { return _rightChild; } set { _rightChild = value; } }
      internal RedBlackLinkedNode Parent { get { return _parent; } set { _parent = value; } }

      internal RedBlackLinkedNode()
      {
        _color = Red;
      }
    }

    #endregion

    protected Func<Type, Type, int> _valueComparisonFunction;

    protected int _count;
    protected RedBlackLinkedNode _redBlackTree;
    protected static RedBlackLinkedNode _sentinelNode;

    public int Count { get { return _count; } }

    public bool IsEmpty { get { return _redBlackTree == null; } }

    public RedBlackTreeLinked(
      Func<Type, Type, int> valueComparisonFunction)
    {
      _sentinelNode = new RedBlackLinkedNode();
      _sentinelNode.Color = Black;
      _redBlackTree = _sentinelNode;
      _valueComparisonFunction = valueComparisonFunction;
    }

    public Type Get<Key>(Key key, Func<Type, Key, int> comparison)
    {
      int compareResult;
      RedBlackLinkedNode treeNode = _redBlackTree;
      while (treeNode != _sentinelNode)
      {
        compareResult = comparison(treeNode.Value, key);
        if (compareResult == 0)
          return treeNode.Value;
        if (compareResult > 0)
          treeNode = treeNode.LeftChild;
        else
          treeNode = treeNode.RightChild;
      }
      throw new RedBlackLinkedException("attempting to get a non-existing value.");
    }

    public bool TryGet<Key>(Key key, Func<Type, Key, int> comparison, out Type returnValue)
    {
      int compareResult;
      RedBlackLinkedNode treeNode = _redBlackTree;
      while (treeNode != _sentinelNode)
      {
        compareResult = comparison(treeNode.Value, key);
        if (compareResult == 0)
        {
          returnValue = treeNode.Value;
          return true;
        }
        if (compareResult > 0)
          treeNode = treeNode.LeftChild;
        else
          treeNode = treeNode.RightChild;
      }
      returnValue = default(Type);
      return false;
    }

    public bool Contains(Type item)
    {
      int compareResult;
      RedBlackLinkedNode treeNode = _redBlackTree;
      while (treeNode != _sentinelNode)
      {
        compareResult = _valueComparisonFunction(treeNode.Value, item);
        if (compareResult == 0)
          return true;
        if (compareResult > 0)
          treeNode = treeNode.LeftChild;
        else
          treeNode = treeNode.RightChild;
      }
      return false;
    }

    public bool Contains<Key>(Key key, Func<Type, Key, int> comparison)
    {
      int compareResult;
      RedBlackLinkedNode treeNode = _redBlackTree;
      while (treeNode != _sentinelNode)
      {
        compareResult = comparison(treeNode.Value, key);
        if (compareResult == 0)
          return true;
        if (compareResult > 0)
          treeNode = treeNode.LeftChild;
        else
          treeNode = treeNode.RightChild;
      }
      return false;
    }

    public void Add(Type data)
    {
      if (data == null)
        throw (new RedBlackLinkedException("RedBlackNode key and data must not be null"));
      int result = 0;
      RedBlackLinkedNode addition = new RedBlackLinkedNode();
      RedBlackLinkedNode temp = _redBlackTree;
      while (temp != _sentinelNode)
      {
        addition.Parent = temp;
        result = _valueComparisonFunction(data, temp.Value);
        if (result == 0)
          throw (new RedBlackLinkedException("A Node with the same key already exists"));
        if (result > 0)
          temp = temp.RightChild;
        else
          temp = temp.LeftChild;
      }
      addition.Value = data;
      addition.LeftChild = _sentinelNode;
      addition.RightChild = _sentinelNode;
      if (addition.Parent != null)
      {
        result = _valueComparisonFunction(addition.Value, addition.Parent.Value);
        if (result > 0)
          addition.Parent.RightChild = addition;
        else
          addition.Parent.LeftChild = addition;
      }
      else
        _redBlackTree = addition;
      BalanceAddition(addition);
      _count = _count + 1;
    }

    protected void BalanceAddition(RedBlackLinkedNode balancing)
    {
      RedBlackLinkedNode temp;
      while (balancing != _redBlackTree && balancing.Parent.Color == Red)
      {
        if (balancing.Parent == balancing.Parent.Parent.LeftChild)
        {
          temp = balancing.Parent.Parent.RightChild;
          if (temp != null && temp.Color == Red)
          {
            balancing.Parent.Color = Black;
            temp.Color = Black;
            balancing.Parent.Parent.Color = Red;
            balancing = balancing.Parent.Parent;
          }
          else
          {
            if (balancing == balancing.Parent.RightChild)
            {
              balancing = balancing.Parent;
              RotateLeft(balancing);
            }
            balancing.Parent.Color = Black;
            balancing.Parent.Parent.Color = Red;
            RotateRight(balancing.Parent.Parent);
          }
        }
        else
        {
          temp = balancing.Parent.Parent.LeftChild;
          if (temp != null && temp.Color == Red)
          {
            balancing.Parent.Color = Black;
            temp.Color = Black;
            balancing.Parent.Parent.Color = Red;
            balancing = balancing.Parent.Parent;
          }
          else
          {
            if (balancing == balancing.Parent.LeftChild)
            {
              balancing = balancing.Parent;
              RotateRight(balancing);
            }
            balancing.Parent.Color = Black;
            balancing.Parent.Parent.Color = Red;
            RotateLeft(balancing.Parent.Parent);
          }
        }
      }
      _redBlackTree.Color = Black;
    }

    protected void RotateLeft(RedBlackLinkedNode redBlackTree)
    {
      RedBlackLinkedNode temp = redBlackTree.RightChild;
      redBlackTree.RightChild = temp.LeftChild;
      if (temp.LeftChild != _sentinelNode)
        temp.LeftChild.Parent = redBlackTree;
      if (temp != _sentinelNode)
        temp.Parent = redBlackTree.Parent;
      if (redBlackTree.Parent != null)
      {
        if (redBlackTree == redBlackTree.Parent.LeftChild)
          redBlackTree.Parent.LeftChild = temp;
        else
          redBlackTree.Parent.RightChild = temp;
      }
      else
        _redBlackTree = temp;
      temp.LeftChild = redBlackTree;
      if (redBlackTree != _sentinelNode)
        redBlackTree.Parent = temp;
    }

    protected void RotateRight(RedBlackLinkedNode redBlacktree)
    {
      RedBlackLinkedNode temp = redBlacktree.LeftChild;
      redBlacktree.LeftChild = temp.RightChild;
      if (temp.RightChild != _sentinelNode)
        temp.RightChild.Parent = redBlacktree;
      if (temp != _sentinelNode)
        temp.Parent = redBlacktree.Parent;
      if (redBlacktree.Parent != null)
      {
        if (redBlacktree == redBlacktree.Parent.RightChild)
          redBlacktree.Parent.RightChild = temp;
        else
          redBlacktree.Parent.LeftChild = temp;
      }
      else
        _redBlackTree = temp;
      temp.RightChild = redBlacktree;
      if (redBlacktree != _sentinelNode)
        redBlacktree.Parent = temp;
    }

    public Type GetMin()
    {
      RedBlackLinkedNode treeNode = _redBlackTree;
      if (treeNode == null || treeNode == _sentinelNode)
        throw new RedBlackLinkedException("attempting to get the minimum value from an empty tree.");
      while (treeNode.LeftChild != _sentinelNode)
        treeNode = treeNode.LeftChild;
      Type returnValue = treeNode.Value;
      return returnValue;
    }

    public Type GetMax()
    {
      RedBlackLinkedNode treeNode = _redBlackTree;
      if (treeNode == null || treeNode == _sentinelNode)
      {
        throw (new RedBlackLinkedException("attempting to get the maximum value from an empty tree."));
      }
      while (treeNode.RightChild != _sentinelNode)
        treeNode = treeNode.RightChild;
      Type returnValue = treeNode.Value;
      return returnValue;
    }

    public void Remove(Type value)
    {
      if (value is object)
        if (((object)value) == null)
          throw new RedBlackLinkedException("Attempting to remove a null value from the tree.");
      int result;
      RedBlackLinkedNode node;
      node = _redBlackTree;
      while (node != _sentinelNode)
      {
        result = _valueComparisonFunction(node.Value, value);
        if (result == 0) break;
        if (result > 0) node = node.LeftChild;
        else node = node.RightChild;
      }
      if (node == _sentinelNode) return;
      Remove(node);
      _count = _count - 1;
    }

    protected void Remove(RedBlackLinkedNode removal)
    {
      RedBlackLinkedNode x = new RedBlackLinkedNode();
      RedBlackLinkedNode temp;
      if (removal.LeftChild == _sentinelNode || removal.RightChild == _sentinelNode)
        temp = removal;
      else
      {
        temp = removal.RightChild;
        while (temp.LeftChild != _sentinelNode)
          temp = temp.LeftChild;
      }
      if (temp.LeftChild != _sentinelNode)
        x = temp.LeftChild;
      else
        x = temp.RightChild;
      x.Parent = temp.Parent;
      if (temp.Parent != null)
        if (temp == temp.Parent.LeftChild)
          temp.Parent.LeftChild = x;
        else
          temp.Parent.RightChild = x;
      else
        _redBlackTree = x;
      if (temp != removal)
        removal.Value = temp.Value;
      if (temp.Color == Black) BalanceRemoval(x);
    }

    protected void BalanceRemoval(RedBlackLinkedNode balancing)
    {
      RedBlackLinkedNode temp;
      while (balancing != _redBlackTree && balancing.Color == Black)
      {
        if (balancing == balancing.Parent.LeftChild)
        {
          temp = balancing.Parent.RightChild;
          if (temp.Color == Red)
          {
            temp.Color = Black;
            balancing.Parent.Color = Red;
            RotateLeft(balancing.Parent);
            temp = balancing.Parent.RightChild;
          }
          if (temp.LeftChild.Color == Black && temp.RightChild.Color == Black)
          {
            temp.Color = Red;
            balancing = balancing.Parent;
          }
          else
          {
            if (temp.RightChild.Color == Black)
            {
              temp.LeftChild.Color = Black;
              temp.Color = Red;
              RotateRight(temp);
              temp = balancing.Parent.RightChild;
            }
            temp.Color = balancing.Parent.Color;
            balancing.Parent.Color = Black;
            temp.RightChild.Color = Black;
            RotateLeft(balancing.Parent);
            balancing = _redBlackTree;
          }
        }
        else
        {
          temp = balancing.Parent.LeftChild;
          if (temp.Color == Red)
          {
            temp.Color = Black;
            balancing.Parent.Color = Red;
            RotateRight(balancing.Parent);
            temp = balancing.Parent.LeftChild;
          }
          if (temp.RightChild.Color == Black && temp.LeftChild.Color == Black)
          {
            temp.Color = Red;
            balancing = balancing.Parent;
          }
          else
          {
            if (temp.LeftChild.Color == Black)
            {
              temp.RightChild.Color = Black;
              temp.Color = Red;
              RotateLeft(temp);
              temp = balancing.Parent.LeftChild;
            }
            temp.Color = balancing.Parent.Color;
            balancing.Parent.Color = Black;
            temp.LeftChild.Color = Black;
            RotateRight(balancing.Parent);
            balancing = _redBlackTree;
          }
        }
      }
      balancing.Color = Black;
    }

    public void Clear()
    {
      _redBlackTree = _sentinelNode;
      _count = 0;
    }

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
      if (!TraversalInOrder(traversalFunction, _redBlackTree))
        return false;
      return true;
    }
    private bool TraversalInOrder(Func<Type, bool> traversalFunction, RedBlackLinkedNode avltreeNode)
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
      TraversalInOrder(traversalFunction, _redBlackTree);
    }
    protected void TraversalInOrder(Action<Type> traversalFunction, RedBlackLinkedNode avltreeNode)
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
      if (TraversalPostOrder(traversalFunction, _redBlackTree))
        return false;
      return true;
    }
    protected bool TraversalPostOrder(Func<Type, bool> traversalFunction, RedBlackLinkedNode avltreeNode)
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
      if (!TraversalPreOrder(traversalFunction, _redBlackTree))
        return false;
      return true;
    }
    protected bool TraversalPreOrder(Func<Type, bool> traversalFunction, RedBlackLinkedNode avltreeNode)
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

    /// <summary>Puts all the items in the tree into an array in order.</summary>
    /// <returns>The alphabetized list of items.</returns>
    /// <remarks>Runtime: Theta(n).</remarks>
    public Type[] ToArrayInOrder()
    {
      Type[] array = new Type[_count];
      ToArrayInOrder(array, _redBlackTree, 0);
      return array;
    }
    protected void ToArrayInOrder(Type[] array, RedBlackLinkedNode avltreeNode, int position)
    {
      if (avltreeNode != null)
      {
        ToArrayInOrder(array, avltreeNode.LeftChild, position);
        array[position++] = avltreeNode.Value;
        ToArrayInOrder(array, avltreeNode.RightChild, position);
      }
    }

    /// <summary>Puts all the items in the tree into an array in reverse order.</summary>
    /// <returns>The alphabetized list of items.</returns>
    /// <remarks>Runtime: Theta(n).</remarks>
    public Type[] ToArrayPostOrder()
    {
      Type[] array = new Type[_count];
      ToArrayPostOrder(array, _redBlackTree, 0);
      return array;
    }
    protected void ToArrayPostOrder(Type[] array, RedBlackLinkedNode avltreeNode, int position)
    {
      if (avltreeNode != null)
      {
        ToArrayInOrder(array, avltreeNode.RightChild, position);
        array[position++] = avltreeNode.Value;
        ToArrayInOrder(array, avltreeNode.LeftChild, position);
      }
    }

    /// <summary>This is used for throwing RedBlackTree exceptions only to make debugging faster.</summary>
    protected class RedBlackLinkedException : Exception { public RedBlackLinkedException(string message) : base(message) { } }
  }

  #endregion

  #region RedBlackTreeLinkedThreadSafe<Type>

  [Serializable]
  public class RedBlackTreeLinkedThreadSafe<Type> : RedBlackTree<Type>
  {
    protected const bool Red = true;
    protected const bool Black = false;

    #region RedBlackTreeNode

    protected class RedBlackLinkedThreadSafeNode
    {
      private bool _color;
      private Type _value;
      private RedBlackLinkedThreadSafeNode _leftChild;
      private RedBlackLinkedThreadSafeNode _rightChild;
      private RedBlackLinkedThreadSafeNode _parent;

      internal bool Color { get { return _color; } set { _color = value; } }
      internal Type Value { get { return _value; } set { _value = value; } }
      internal RedBlackLinkedThreadSafeNode LeftChild { get { return _leftChild; } set { _leftChild = value; } }
      internal RedBlackLinkedThreadSafeNode RightChild { get { return _rightChild; } set { _rightChild = value; } }
      internal RedBlackLinkedThreadSafeNode Parent { get { return _parent; } set { _parent = value; } }

      internal RedBlackLinkedThreadSafeNode()
      {
        _color = Red;
      }
    }

    #endregion

    protected Func<Type, Type, int> _valueComparisonFunction;

    protected int _count;
    protected RedBlackLinkedThreadSafeNode _redBlackTree;
    protected static RedBlackLinkedThreadSafeNode _sentinelNode;

    protected object _lock;
    protected int _readers;
    protected int _writers;

    public int Count { get { ReaderLock(); int count = _count; ReaderUnlock(); return count; } }

    public bool IsEmpty { get { ReaderLock(); bool isEmpty = _redBlackTree == null; ReaderUnlock(); return isEmpty; } }

    public RedBlackTreeLinkedThreadSafe(
      Func<Type, Type, int> valueComparisonFunction)
    {
      _sentinelNode = new RedBlackLinkedThreadSafeNode();
      _sentinelNode.Color = Black;
      _redBlackTree = _sentinelNode;
      _valueComparisonFunction = valueComparisonFunction;
      _lock = new object();
      _readers = 0;
      _writers = 0;
    }

    public Type Get<Key>(Key key, Func<Type, Key, int> comparison)
    {
      ReaderLock();
      int compareResult;
      RedBlackLinkedThreadSafeNode treeNode = _redBlackTree;
      while (treeNode != _sentinelNode)
      {
        compareResult = comparison(treeNode.Value, key);
        if (compareResult == 0)
        {
          ReaderUnlock();
          return treeNode.Value;
        }
        if (compareResult > 0)
          treeNode = treeNode.LeftChild;
        else
          treeNode = treeNode.RightChild;
      }
      ReaderUnlock();
      throw new RedBlackLinkedException("attempting to get a non-existing value.");
    }

    public bool TryGet<Key>(Key key, Func<Type, Key, int> comparison, out Type returnValue)
    {
      ReaderLock();
      int compareResult;
      RedBlackLinkedThreadSafeNode treeNode = _redBlackTree;
      while (treeNode != _sentinelNode)
      {
        compareResult = comparison(treeNode.Value, key);
        if (compareResult == 0)
        {
          returnValue = treeNode.Value;
          ReaderUnlock();
          return true;
        }
        if (compareResult > 0)
          treeNode = treeNode.LeftChild;
        else
          treeNode = treeNode.RightChild;
      }
      returnValue = default(Type);
      ReaderUnlock();
      return false;
    }

    public bool Contains(Type item)
    {
      ReaderLock();
      int compareResult;
      RedBlackLinkedThreadSafeNode treeNode = _redBlackTree;
      while (treeNode != _sentinelNode)
      {
        compareResult = _valueComparisonFunction(treeNode.Value, item);
        if (compareResult == 0)
        {
          ReaderUnlock();
          return true;
        }
        if (compareResult > 0)
          treeNode = treeNode.LeftChild;
        else
          treeNode = treeNode.RightChild;
      }
      ReaderUnlock();
      return false;
    }

    public bool Contains<Key>(Key key, Func<Type, Key, int> comparison)
    {
      ReaderLock();
      int compareResult;
      RedBlackLinkedThreadSafeNode treeNode = _redBlackTree;
      while (treeNode != _sentinelNode)
      {
        compareResult = comparison(treeNode.Value, key);
        if (compareResult == 0)
        {
          ReaderUnlock();
          return true;
        }
        if (compareResult > 0)
          treeNode = treeNode.LeftChild;
        else
          treeNode = treeNode.RightChild;
      }
      ReaderUnlock();
      return false;
    }

    public void Add(Type data)
    {
      WriterLock();
      if (data == null)
      {
        WriterUnlock();
        throw (new RedBlackLinkedException("RedBlackNode key and data must not be null"));
      }
      int result = 0;
      RedBlackLinkedThreadSafeNode addition = new RedBlackLinkedThreadSafeNode();
      RedBlackLinkedThreadSafeNode temp = _redBlackTree;
      while (temp != _sentinelNode)
      {
        addition.Parent = temp;
        result = _valueComparisonFunction(data, temp.Value);
        if (result == 0)
        {
          WriterUnlock();
          throw (new RedBlackLinkedException("A Node with the same key already exists"));
        }
        if (result > 0)
          temp = temp.RightChild;
        else
          temp = temp.LeftChild;
      }
      addition.Value = data;
      addition.LeftChild = _sentinelNode;
      addition.RightChild = _sentinelNode;
      if (addition.Parent != null)
      {
        result = _valueComparisonFunction(addition.Value, addition.Parent.Value);
        if (result > 0)
          addition.Parent.RightChild = addition;
        else
          addition.Parent.LeftChild = addition;
      }
      else
        _redBlackTree = addition;
      BalanceAddition(addition);
      _count = _count + 1;
      WriterUnlock();
    }

    protected void BalanceAddition(RedBlackLinkedThreadSafeNode balancing)
    {
      RedBlackLinkedThreadSafeNode temp;
      while (balancing != _redBlackTree && balancing.Parent.Color == Red)
      {
        if (balancing.Parent == balancing.Parent.Parent.LeftChild)
        {
          temp = balancing.Parent.Parent.RightChild;
          if (temp != null && temp.Color == Red)
          {
            balancing.Parent.Color = Black;
            temp.Color = Black;
            balancing.Parent.Parent.Color = Red;
            balancing = balancing.Parent.Parent;
          }
          else
          {
            if (balancing == balancing.Parent.RightChild)
            {
              balancing = balancing.Parent;
              RotateLeft(balancing);
            }
            balancing.Parent.Color = Black;
            balancing.Parent.Parent.Color = Red;
            RotateRight(balancing.Parent.Parent);
          }
        }
        else
        {
          temp = balancing.Parent.Parent.LeftChild;
          if (temp != null && temp.Color == Red)
          {
            balancing.Parent.Color = Black;
            temp.Color = Black;
            balancing.Parent.Parent.Color = Red;
            balancing = balancing.Parent.Parent;
          }
          else
          {
            if (balancing == balancing.Parent.LeftChild)
            {
              balancing = balancing.Parent;
              RotateRight(balancing);
            }
            balancing.Parent.Color = Black;
            balancing.Parent.Parent.Color = Red;
            RotateLeft(balancing.Parent.Parent);
          }
        }
      }
      _redBlackTree.Color = Black;
    }

    protected void RotateLeft(RedBlackLinkedThreadSafeNode redBlackTree)
    {
      RedBlackLinkedThreadSafeNode temp = redBlackTree.RightChild;
      redBlackTree.RightChild = temp.LeftChild;
      if (temp.LeftChild != _sentinelNode)
        temp.LeftChild.Parent = redBlackTree;
      if (temp != _sentinelNode)
        temp.Parent = redBlackTree.Parent;
      if (redBlackTree.Parent != null)
      {
        if (redBlackTree == redBlackTree.Parent.LeftChild)
          redBlackTree.Parent.LeftChild = temp;
        else
          redBlackTree.Parent.RightChild = temp;
      }
      else
        _redBlackTree = temp;
      temp.LeftChild = redBlackTree;
      if (redBlackTree != _sentinelNode)
        redBlackTree.Parent = temp;
    }

    protected void RotateRight(RedBlackLinkedThreadSafeNode redBlacktree)
    {
      RedBlackLinkedThreadSafeNode temp = redBlacktree.LeftChild;
      redBlacktree.LeftChild = temp.RightChild;
      if (temp.RightChild != _sentinelNode)
        temp.RightChild.Parent = redBlacktree;
      if (temp != _sentinelNode)
        temp.Parent = redBlacktree.Parent;
      if (redBlacktree.Parent != null)
      {
        if (redBlacktree == redBlacktree.Parent.RightChild)
          redBlacktree.Parent.RightChild = temp;
        else
          redBlacktree.Parent.LeftChild = temp;
      }
      else
        _redBlackTree = temp;
      temp.RightChild = redBlacktree;
      if (redBlacktree != _sentinelNode)
        redBlacktree.Parent = temp;
    }

    public Type GetMin()
    {
      ReaderLock();
      RedBlackLinkedThreadSafeNode treeNode = _redBlackTree;
      if (treeNode == null || treeNode == _sentinelNode)
      {
        ReaderUnlock();
        throw new RedBlackLinkedException("attempting to get the minimum value from an empty tree.");
      }
      while (treeNode.LeftChild != _sentinelNode)
        treeNode = treeNode.LeftChild;
      Type returnValue = treeNode.Value;
      ReaderUnlock();
      return returnValue;
    }

    public Type GetMax()
    {
      ReaderLock();
      RedBlackLinkedThreadSafeNode treeNode = _redBlackTree;
      if (treeNode == null || treeNode == _sentinelNode)
      {
        ReaderUnlock();
        throw (new RedBlackLinkedException("attempting to get the maximum value from an empty tree."));
      }
      while (treeNode.RightChild != _sentinelNode)
        treeNode = treeNode.RightChild;
      Type returnValue = treeNode.Value;
      ReaderUnlock();
      return returnValue;
    }

    public void Remove(Type value)
    {
      WriterLock();
      if (value is object)
        if (((object)value) == null)
          throw new RedBlackLinkedException("Attempting to remove a null value from the tree.");
      int result;
      RedBlackLinkedThreadSafeNode node;
      node = _redBlackTree;
      while (node != _sentinelNode)
      {
        result = _valueComparisonFunction(node.Value, value);
        if (result == 0) break;
        if (result > 0) node = node.LeftChild;
        else node = node.RightChild;
      }
      if (node == _sentinelNode) return;
      Remove(node);
      _count = _count - 1;
      WriterUnlock();
    }

    protected void Remove(RedBlackLinkedThreadSafeNode removal)
    {
      RedBlackLinkedThreadSafeNode x = new RedBlackLinkedThreadSafeNode();
      RedBlackLinkedThreadSafeNode temp;
      if (removal.LeftChild == _sentinelNode || removal.RightChild == _sentinelNode)
        temp = removal;
      else
      {
        temp = removal.RightChild;
        while (temp.LeftChild != _sentinelNode)
          temp = temp.LeftChild;
      }
      if (temp.LeftChild != _sentinelNode)
        x = temp.LeftChild;
      else
        x = temp.RightChild;
      x.Parent = temp.Parent;
      if (temp.Parent != null)
        if (temp == temp.Parent.LeftChild)
          temp.Parent.LeftChild = x;
        else
          temp.Parent.RightChild = x;
      else
        _redBlackTree = x;
      if (temp != removal)
        removal.Value = temp.Value;
      if (temp.Color == Black) BalanceRemoval(x);
    }

    protected void BalanceRemoval(RedBlackLinkedThreadSafeNode balancing)
    {
      RedBlackLinkedThreadSafeNode temp;
      while (balancing != _redBlackTree && balancing.Color == Black)
      {
        if (balancing == balancing.Parent.LeftChild)
        {
          temp = balancing.Parent.RightChild;
          if (temp.Color == Red)
          {
            temp.Color = Black;
            balancing.Parent.Color = Red;
            RotateLeft(balancing.Parent);
            temp = balancing.Parent.RightChild;
          }
          if (temp.LeftChild.Color == Black && temp.RightChild.Color == Black)
          {
            temp.Color = Red;
            balancing = balancing.Parent;
          }
          else
          {
            if (temp.RightChild.Color == Black)
            {
              temp.LeftChild.Color = Black;
              temp.Color = Red;
              RotateRight(temp);
              temp = balancing.Parent.RightChild;
            }
            temp.Color = balancing.Parent.Color;
            balancing.Parent.Color = Black;
            temp.RightChild.Color = Black;
            RotateLeft(balancing.Parent);
            balancing = _redBlackTree;
          }
        }
        else
        {
          temp = balancing.Parent.LeftChild;
          if (temp.Color == Red)
          {
            temp.Color = Black;
            balancing.Parent.Color = Red;
            RotateRight(balancing.Parent);
            temp = balancing.Parent.LeftChild;
          }
          if (temp.RightChild.Color == Black && temp.LeftChild.Color == Black)
          {
            temp.Color = Red;
            balancing = balancing.Parent;
          }
          else
          {
            if (temp.LeftChild.Color == Black)
            {
              temp.RightChild.Color = Black;
              temp.Color = Red;
              RotateLeft(temp);
              temp = balancing.Parent.LeftChild;
            }
            temp.Color = balancing.Parent.Color;
            balancing.Parent.Color = Black;
            temp.LeftChild.Color = Black;
            RotateRight(balancing.Parent);
            balancing = _redBlackTree;
          }
        }
      }
      balancing.Color = Black;
    }

    public void Clear()
    {
      _redBlackTree = _sentinelNode;
      _count = 0;
    }

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
      if (!TraversalInOrder(traversalFunction, _redBlackTree))
      {
        ReaderUnlock();
        return false;
      }
      ReaderUnlock();
      return true;
    }
    private bool TraversalInOrder(Func<Type, bool> traversalFunction, RedBlackLinkedThreadSafeNode avltreeNode)
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
      TraversalInOrder(traversalFunction, _redBlackTree);
      ReaderUnlock();
    }
    protected void TraversalInOrder(Action<Type> traversalFunction, RedBlackLinkedThreadSafeNode avltreeNode)
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
      if (TraversalPostOrder(traversalFunction, _redBlackTree))
      {
        ReaderUnlock();
        return false;
      }
      ReaderUnlock();
      return true;
    }
    protected bool TraversalPostOrder(Func<Type, bool> traversalFunction, RedBlackLinkedThreadSafeNode avltreeNode)
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
      if (!TraversalPreOrder(traversalFunction, _redBlackTree))
      {
        ReaderUnlock();
        return false;
      }
      ReaderUnlock();
      return true;
    }
    protected bool TraversalPreOrder(Func<Type, bool> traversalFunction, RedBlackLinkedThreadSafeNode avltreeNode)
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

    /// <summary>Puts all the items in the tree into an array in order.</summary>
    /// <returns>The alphabetized list of items.</returns>
    /// <remarks>Runtime: Theta(n).</remarks>
    public Type[] ToArrayInOrder()
    {
      ReaderLock();
      Type[] array = new Type[_count];
      ToArrayInOrder(array, _redBlackTree, 0);
      ReaderUnlock();
      return array;
    }
    protected void ToArrayInOrder(Type[] array, RedBlackLinkedThreadSafeNode avltreeNode, int position)
    {
      if (avltreeNode != null)
      {
        ToArrayInOrder(array, avltreeNode.LeftChild, position);
        array[position++] = avltreeNode.Value;
        ToArrayInOrder(array, avltreeNode.RightChild, position);
      }
    }

    /// <summary>Puts all the items in the tree into an array in reverse order.</summary>
    /// <returns>The alphabetized list of items.</returns>
    /// <remarks>Runtime: Theta(n).</remarks>
    public Type[] ToArrayPostOrder()
    {
      ReaderLock();
      Type[] array = new Type[_count];
      ToArrayPostOrder(array, _redBlackTree, 0);
      ReaderUnlock();
      return array;
    }
    protected void ToArrayPostOrder(Type[] array, RedBlackLinkedThreadSafeNode avltreeNode, int position)
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

    /// <summary>This is used for throwing RedBlackTree exceptions only to make debugging faster.</summary>
    protected class RedBlackLinkedException : Exception { public RedBlackLinkedException(string message) : base(message) { } }
  }

  #endregion
}
