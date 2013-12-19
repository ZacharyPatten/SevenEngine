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
    void Remove(Type removal);
    int Count { get; }
    bool IsEmpty { get; }
    void Clear();
  }

  public interface RedBlackTree<ValueType, KeyType> : RedBlackTree<ValueType>
  {
    ValueType this[KeyType key] { get; }
    ValueType Get(KeyType get);
    bool TryGet(KeyType get, out ValueType returnValue);
    bool Contains(KeyType containsCheck);
    void Remove(KeyType removalKey);
    bool TraversalInOrderBreakable(Func<ValueType, bool> traversalFunction, KeyType minimum, KeyType maximum);
    void TraversalInOrder(Action<ValueType> traversalFunction, KeyType minimum, KeyType maximum);
  }

  public interface RedBlackTree<ValueType, FirstKeyType, SecondKeyType> : RedBlackTree<ValueType, FirstKeyType>
  {
    // THESE MUST BE NAMED DIFFERENTLY FROM THE INHERITED INTERFACE ACCORDING TO 13.4.6 (as of 12.16.13 in C# 5.0)
    // OTHERWISE INTERFACE RE-IMPLEMENTATION WILL OVERRIDE METHODS IF THE GENERIC TYPES ARE EQUAL
    ValueType GetSecondGeneric(SecondKeyType get);
    bool TryGetSecondGeneric(SecondKeyType get, out ValueType returnValue);
    bool ContainsSecondGeneric(SecondKeyType containsCheck);
    void RemoveSecondGeneric(SecondKeyType removalKey);
    bool TraversalInOrderBreakableSecondGeneric(Func<ValueType, bool> traversalFunction, SecondKeyType minimum, SecondKeyType maximum);
    void TraversalInOrderSecondGeneric(Action<ValueType> traversalFunction, SecondKeyType minimum, SecondKeyType maximum);
  }

  #region RedBlackTreeLinked

  public class RedBlackTreeLinked<ValueType> : RedBlackTree<ValueType>
  {
    protected const bool Red = true;
    protected const bool Black = false;

    #region RedBlackTreeNode

    protected class RedBlackLinkedNode
    {
      private bool _color;
      private ValueType _value;
      private RedBlackLinkedNode _leftChild;
      private RedBlackLinkedNode _rightChild;
      private RedBlackLinkedNode _parent;

      internal bool Color { get { return _color; } set { _color = value; } }
      internal ValueType Value { get { return _value; } set { _value = value; } }
      internal RedBlackLinkedNode LeftChild { get { return _leftChild; } set { _leftChild = value; } }
      internal RedBlackLinkedNode RightChild { get { return _rightChild; } set { _rightChild = value; } }
      internal RedBlackLinkedNode Parent { get { return _parent; } set { _parent = value; } }

      internal RedBlackLinkedNode()
      {
        _color = Red;
      }
    }

    #endregion

    protected Func<ValueType, ValueType, int> _valueComparisonFunction;

    protected int _count;
    protected RedBlackLinkedNode _redBlackTree;
    protected static RedBlackLinkedNode _sentinelNode;

    protected object _lock;
    protected int _readers;
    protected int _writers;

    public int Count { get { ReaderLock(); int count = _count; ReaderUnlock(); return count; } }

    public bool IsEmpty { get { ReaderLock(); bool isEmpty = _redBlackTree == null; ReaderUnlock(); return isEmpty; } }

    public RedBlackTreeLinked(
      Func<ValueType, ValueType, int> valueComparisonFunction)
    {
      _sentinelNode = new RedBlackLinkedNode();
      _sentinelNode.Color = Black;
      _redBlackTree = _sentinelNode;
      _valueComparisonFunction = valueComparisonFunction;
      _lock = new object();
      _readers = 0;
      _writers = 0;
    }

    public void Add(ValueType data)
    {
      WriterLock();
      if (data == null)
      {
        WriterUnlock();
        throw (new RedBlackLinkedException("RedBlackNode key and data must not be null"));
      }
      int result = 0;
      RedBlackLinkedNode addition = new RedBlackLinkedNode();
      RedBlackLinkedNode temp = _redBlackTree;
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

    public ValueType GetMin()
    {
      ReaderLock();
      RedBlackLinkedNode treeNode = _redBlackTree;
      if (treeNode == null || treeNode == _sentinelNode)
      {
        ReaderUnlock();
        throw new RedBlackLinkedException("attempting to get the minimum value from an empty tree.");
      }
      while (treeNode.LeftChild != _sentinelNode)
        treeNode = treeNode.LeftChild;
      ValueType returnValue = treeNode.Value;
      ReaderUnlock();
      return returnValue;
    }

    public ValueType GetMax()
    {
      ReaderLock();
      RedBlackLinkedNode treeNode = _redBlackTree;
      if (treeNode == null || treeNode == _sentinelNode)
      {
        ReaderUnlock();
        throw (new RedBlackLinkedException("attempting to get the maximum value from an empty tree."));
      }
      while (treeNode.RightChild != _sentinelNode)
        treeNode = treeNode.RightChild;
      ValueType returnValue = treeNode.Value;
      ReaderUnlock();
      return returnValue;
    }

    public void Remove(ValueType value)
    {
      WriterLock();
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
      WriterUnlock();
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

    public void Clear() { _redBlackTree = _sentinelNode; _count = 0; }

    /// <summary>Performs a functional paradigm in-order traversal of the AVL tree.</summary>
    /// <param name="traversalFunction">The function to perform during iteration.</param>
    /// <remarks>Runtime: O(n * traversalFunction).</remarks>
    public bool TraverseBreakable(Func<ValueType, bool> traversalFunction) { return TraversalInOrder(traversalFunction); }

    /// <summary>Performs a functional paradigm in-order traversal of the AVL tree.</summary>
    /// <param name="traversalAction">The action to perform during iteration.</param>
    /// <remarks>Runtime: O(n * traversalFunction).</remarks>
    public void Traverse(Action<ValueType> traversalAction) { TraversalInOrder(traversalAction); }

    /// <summary>Performs a functional paradigm in-order traversal of the AVL tree.</summary>
    /// <param name="traversalFunction">The function to perform during iteration.</param>
    /// <remarks>Runtime: O(n * traversalFunction).</remarks>
    public bool TraversalInOrder(Func<ValueType, bool> traversalFunction)
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
    private bool TraversalInOrder(Func<ValueType, bool> traversalFunction, RedBlackLinkedNode avltreeNode)
    {
      if (avltreeNode != null)
      {
        if (!TraversalInOrder(traversalFunction, avltreeNode.LeftChild)) return false;
        if (!traversalFunction(avltreeNode.Value)) return false;
        if (!TraversalInOrder(traversalFunction, avltreeNode.RightChild)) return false;
      }
      return true;
    }

    public void TraversalInOrder(Action<ValueType> traversalFunction)
    {
      ReaderLock();
      TraversalInOrder(traversalFunction, _redBlackTree);
      ReaderUnlock();
    }
    protected void TraversalInOrder(Action<ValueType> traversalFunction, RedBlackLinkedNode avltreeNode)
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
    public bool TraversalPostOrder(Func<ValueType, bool> traversalFunction)
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
    protected bool TraversalPostOrder(Func<ValueType, bool> traversalFunction, RedBlackLinkedNode avltreeNode)
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
    public bool TraversalPreOrder(Func<ValueType, bool> traversalFunction)
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
    protected bool TraversalPreOrder(Func<ValueType, bool> traversalFunction, RedBlackLinkedNode avltreeNode)
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
    public ValueType[] ToArray() { return ToArrayInOrder(); }

    /// <summary>Puts all the items in the tree into an array in order.</summary>
    /// <returns>The alphabetized list of items.</returns>
    /// <remarks>Runtime: Theta(n).</remarks>
    public ValueType[] ToArrayInOrder()
    {
      ReaderLock();
      ValueType[] array = new ValueType[_count];
      ToArrayInOrder(array, _redBlackTree, 0);
      ReaderUnlock();
      return array;
    }
    protected void ToArrayInOrder(ValueType[] array, RedBlackLinkedNode avltreeNode, int position)
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
    public ValueType[] ToArrayPostOrder()
    {
      ReaderLock();
      ValueType[] array = new ValueType[_count];
      ToArrayPostOrder(array, _redBlackTree, 0);
      ReaderUnlock();
      return array;
    }
    protected void ToArrayPostOrder(ValueType[] array, RedBlackLinkedNode avltreeNode, int position)
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

  public class RedBlackTreeLinked<ValueType, KeyType> : RedBlackTreeLinked<ValueType>, RedBlackTree<ValueType, KeyType>
  {
    protected Func<ValueType, KeyType, int> _keyComparisonFunction;

    public ValueType this[KeyType key]
    {
      get
      {
        ReaderLock();
        int comparison;
        RedBlackLinkedNode treeNode = _redBlackTree;
        while (treeNode != _sentinelNode)
        {
          comparison = _keyComparisonFunction(treeNode.Value, key);
          if (comparison == 0)
          {
            ReaderUnlock();
            return treeNode.Value;
          }
          if (comparison > 0)
            treeNode = treeNode.LeftChild;
          else
            treeNode = treeNode.RightChild;
        }
        ReaderUnlock();
        throw new RedBlackLinkedException("attempting to get a non-existing value.");
      }
    }

    public RedBlackTreeLinked(
      Func<ValueType, ValueType, int> valueComparisonFunction,
      Func<ValueType, KeyType, int> keyComparisonFunction)
      : base(valueComparisonFunction)
    {
      _keyComparisonFunction = keyComparisonFunction;
    }

    public ValueType Get(KeyType key)
    {
      ReaderLock();
      int comparison;
      RedBlackLinkedNode treeNode = _redBlackTree;
      while (treeNode != _sentinelNode)
      {
        comparison = _keyComparisonFunction(treeNode.Value, key);
        if (comparison == 0)
        {
          ReaderUnlock();
          return treeNode.Value;
        }
        if (comparison > 0)
          treeNode = treeNode.LeftChild;
        else
          treeNode = treeNode.RightChild;
      }
      ReaderUnlock();
      throw new RedBlackLinkedException("attempting to get a non-existing value.");
    }

    public bool TryGet(KeyType key, out ValueType returnValue)
    {
      ReaderLock();
      int comparison;
      RedBlackLinkedNode treeNode = _redBlackTree;
      while (treeNode != _sentinelNode)
      {
        comparison = _keyComparisonFunction(treeNode.Value, key);
        if (comparison == 0)
        {
          returnValue = treeNode.Value;
          ReaderUnlock();
          return true;
        }
        if (comparison > 0)
          treeNode = treeNode.LeftChild;
        else
          treeNode = treeNode.RightChild;
      }
      returnValue = default(ValueType);
      ReaderUnlock();
      return false;
    }

    public bool Contains(KeyType key)
    {
      ReaderLock();
      int comparison;
      RedBlackLinkedNode treeNode = _redBlackTree;
      while (treeNode != _sentinelNode)
      {
        comparison = _keyComparisonFunction(treeNode.Value, key);
        if (comparison == 0)
        {
          ReaderUnlock();
          return true;
        }
        if (comparison > 0)
          treeNode = treeNode.LeftChild;
        else
          treeNode = treeNode.RightChild;
      }
      ReaderUnlock();
      return false;
    }

    public void Remove(KeyType key)
    {
      WriterLock();
      if (key == null)
      {
        WriterUnlock();
        throw (new RedBlackLinkedException("attempting to remove a null key from the tree."));
      }
      int result;
      RedBlackLinkedNode node;
      node = _redBlackTree;
      while (node != _sentinelNode)
      {
        result = _keyComparisonFunction(node.Value, key);
        if (result == 0) break;
        if (result > 0) node = node.LeftChild;
        else node = node.RightChild;
      } 
      if (node == _sentinelNode) return;
      Remove(node);
      _count = _count - 1;
      WriterUnlock();
    }
    
    /// <summary>Performs a functional paradigm in-order traversal of the AVL tree.</summary>
    /// <param name="traversalFunction">The function to perform during iteration.</param>
    /// <remarks>Runtime: O(n * traversalFunction).</remarks>
    public bool TraversalInOrderBreakable(Func<ValueType, bool> traversalFunction, KeyType minimum, KeyType maximum)
    {
      ReaderLock();
      if (!TraversalInOrderBreakable(traversalFunction, _redBlackTree, minimum, maximum))
      {
        ReaderUnlock();
        return false;
      }
      ReaderUnlock();
      return true;
    }
    protected bool TraversalInOrderBreakable(Func<ValueType, bool> traversalFunction, RedBlackLinkedNode avltreeNode, KeyType minimum, KeyType maximum)
    {
      if (avltreeNode != null)
      {
        if (_keyComparisonFunction(avltreeNode.LeftChild.Value, minimum) >= 0)
          if (!TraversalInOrderBreakable(traversalFunction, avltreeNode.LeftChild, minimum, maximum)) return false;
        if (_keyComparisonFunction(avltreeNode.Value, minimum) >= 0 && _keyComparisonFunction(avltreeNode.Value, maximum) <= 0)
          if (!traversalFunction(avltreeNode.Value)) return false;
        if (_keyComparisonFunction(avltreeNode.RightChild.Value, maximum) <= 0)
          if (!TraversalInOrderBreakable(traversalFunction, avltreeNode.RightChild, minimum, maximum)) return false;
      }
      return true;
    }

    /// <summary>Performs a functional paradigm in-order traversal of the AVL tree.</summary>
    /// <param name="traversalFunction">The function to perform during iteration.</param>
    /// <remarks>Runtime: O(n * traversalFunction).</remarks>
    public void TraversalInOrder(Action<ValueType> traversalFunction, KeyType minimum, KeyType maximum)
    {
      ReaderLock();
      TraversalInOrder(traversalFunction, _redBlackTree, minimum, maximum);
      ReaderUnlock();
    }
    protected void TraversalInOrder(Action<ValueType> traversalFunction, RedBlackLinkedNode avltreeNode, KeyType minimum, KeyType maximum)
    {
      if (avltreeNode != null)
      {
        if (_keyComparisonFunction(avltreeNode.LeftChild.Value, minimum) >= 0)
          TraversalInOrder(traversalFunction, avltreeNode.LeftChild, minimum, maximum);
        if (_keyComparisonFunction(avltreeNode.Value, minimum) >= 0 && _keyComparisonFunction(avltreeNode.Value, maximum) <= 0)
          traversalFunction(avltreeNode.Value);
        if (_keyComparisonFunction(avltreeNode.RightChild.Value, maximum) <= 0)
          TraversalInOrder(traversalFunction, avltreeNode.RightChild, minimum, maximum);
      }
    }
  }

  public class RedBlackTreeLinked<ValueType, FirstKeyType, SecondKeyType> : RedBlackTreeLinked<ValueType, FirstKeyType>, RedBlackTree<ValueType, FirstKeyType, SecondKeyType>
  {
    private Func<ValueType, SecondKeyType, int> _secondkeyComparisonFunction;
    
    public RedBlackTreeLinked(
      Func<ValueType, ValueType, int> valueComparisonFunction,
      Func<ValueType, FirstKeyType, int> firstkeyComparisonFunction,
      Func<ValueType, SecondKeyType, int> secondKeycomparisonFunction)
      : base(valueComparisonFunction, firstkeyComparisonFunction)
    {
      _secondkeyComparisonFunction = secondKeycomparisonFunction;
    }

    public ValueType GetSecondGeneric(SecondKeyType key)
    {
      ReaderLock();
      int comparison;
      ValueType returnValue;
      RedBlackLinkedNode treeNode = _redBlackTree;
      while (treeNode != _sentinelNode)
      {
        comparison = _secondkeyComparisonFunction(treeNode.Value, key);
        if (comparison == 0)
        {
          returnValue = treeNode.Value;
          ReaderUnlock();
          return returnValue;
        }
        if (comparison > 0)
          treeNode = treeNode.LeftChild;
        else
          treeNode = treeNode.RightChild;
      }
      returnValue = default(ValueType);
      ReaderUnlock();
      return returnValue;
    }

    public bool TryGetSecondGeneric(SecondKeyType key, out ValueType returnValue)
    {
      ReaderLock();
      int comparison;
      RedBlackLinkedNode treeNode = _redBlackTree;
      while (treeNode != _sentinelNode)
      {
        comparison = _secondkeyComparisonFunction(treeNode.Value, key);
        if (comparison == 0)
        {
          returnValue = treeNode.Value;
          ReaderUnlock();
          return true;
        }
        if (comparison > 0)
          treeNode = treeNode.LeftChild;
        else
          treeNode = treeNode.RightChild;
      }
      returnValue = default(ValueType);
      ReaderUnlock();
      return false;
    }

    public bool ContainsSecondGeneric(SecondKeyType key)
    {
      ReaderLock();
      int comparison;
      RedBlackLinkedNode treeNode = _redBlackTree;
      while (treeNode != _sentinelNode)
      {
        comparison = _secondkeyComparisonFunction(treeNode.Value, key);
        if (comparison == 0)
        {
          ReaderUnlock();
          return true;
        }
        if (comparison > 0)
          treeNode = treeNode.LeftChild;
        else
          treeNode = treeNode.RightChild;
      }
      ReaderUnlock();
      return false;
    }

    public void RemoveSecondGeneric(SecondKeyType key)
    {
      WriterLock();
      if (key == null)
      {
        WriterUnlock();
        throw (new RedBlackLinkedException("attempting to remove a null key from the tree."));
      }
      int result;
      RedBlackLinkedNode node;
      node = _redBlackTree;
      while (node != _sentinelNode)
      {
        result = _secondkeyComparisonFunction(node.Value, key);
        if (result == 0) break;
        if (result > 0) node = node.LeftChild;
        else node = node.RightChild;
      }
      if (node == _sentinelNode) return;
      Remove(node);
      _count = _count - 1;
      WriterUnlock();
    }

    /// <summary>Performs a functional paradigm in-order traversal of the AVL tree.</summary>
    /// <param name="traversalFunction">The function to perform during iteration.</param>
    /// <remarks>Runtime: O(n * traversalFunction).</remarks>
    public bool TraversalInOrderBreakableSecondGeneric(Func<ValueType, bool> traversalFunction, SecondKeyType minimum, SecondKeyType maximum)
    {
      ReaderLock();
      if (!TraversalInOrderBreakableSecondGeneric(traversalFunction, _redBlackTree, minimum, maximum))
      {
        ReaderUnlock();
        return false;
      }
      ReaderUnlock();
      return true;
    }
    private bool TraversalInOrderBreakableSecondGeneric(Func<ValueType, bool> traversalFunction, RedBlackLinkedNode avltreeNode, SecondKeyType minimum, SecondKeyType maximum)
    {
      if (avltreeNode != null)
      {
        if (_secondkeyComparisonFunction(avltreeNode.LeftChild.Value, minimum) >= 0)
          if (!TraversalInOrderBreakableSecondGeneric(traversalFunction, avltreeNode.LeftChild, minimum, maximum)) return false;
        if (_secondkeyComparisonFunction(avltreeNode.Value, minimum) >= 0 && _secondkeyComparisonFunction(avltreeNode.Value, maximum) <= 0)
          if (!traversalFunction(avltreeNode.Value)) return false;
        if (_secondkeyComparisonFunction(avltreeNode.RightChild.Value, maximum) <= 0)
          if (!TraversalInOrderBreakableSecondGeneric(traversalFunction, avltreeNode.RightChild, minimum, maximum)) return false;
      }
      return true;
    }

    /// <summary>Performs a functional paradigm in-order traversal of the AVL tree.</summary>
    /// <param name="traversalFunction">The function to perform during iteration.</param>
    /// <remarks>Runtime: O(n * traversalFunction).</remarks>
    public void TraversalInOrderSecondGeneric(Action<ValueType> traversalFunction, SecondKeyType minimum, SecondKeyType maximum)
    {
      ReaderLock();
      TraversalInOrderSecondGeneric(traversalFunction, _redBlackTree, minimum, maximum);
      ReaderUnlock();
    }
    private void TraversalInOrderSecondGeneric(Action<ValueType> traversalFunction, RedBlackLinkedNode avltreeNode, SecondKeyType minimum, SecondKeyType maximum)
    {
      if (avltreeNode != null)
      {
        if (_secondkeyComparisonFunction(avltreeNode.LeftChild.Value, minimum) >= 0)
          TraversalInOrderSecondGeneric(traversalFunction, avltreeNode.LeftChild, minimum, maximum);
        if (_secondkeyComparisonFunction(avltreeNode.Value, minimum) >= 0 && _secondkeyComparisonFunction(avltreeNode.Value, maximum) <= 0)
          traversalFunction(avltreeNode.Value);
        if (_secondkeyComparisonFunction(avltreeNode.RightChild.Value, maximum) <= 0)
          TraversalInOrderSecondGeneric(traversalFunction, avltreeNode.RightChild, minimum, maximum);
      }
    }
  }
  #endregion
}
