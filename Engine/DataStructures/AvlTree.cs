//// This file contains the following classes:
//// - AvlTreeArray

//// This file contains runtime values.
//// All runtimes are in O-Notation. Here is a brief explanation:
//// - "O(x)": the member has an upper bound of runtime equation "x"
//// - "Omega(x)": the member has a lower bound of runtime equation "x"
//// - "Theta(x)": the member has an upper and lower bound of runtime equation "x"
//// - "EstAvg(x)": the runtime equation "x" to typically expect
////   (THIS IS MY PERSONAL ESTIMATION, AND CONSIDERING I WROTE THE CODE YOU SHOULD PROBABLY TRUST ME)
//// Note that if the letter "n" is used, it typically means the current number of items within the set.

//// Written by Seven (Zachary Aaron Patten)
//// Last Edited on date 10-12-13
//// Feel free to use this code in any manor you see fit.
//// However, please site me because I put quite a bit of time into it.
//// Special thanks to Rodney Howell, my previous data structures professor.
//// - Thanks. :)

//using System;

//using Engine.DataStructures.Interfaces;

//namespace Engine.DataStructures
//{
//  #region AvlTreeArray

//  public class AvlTreeArray
//  {
//    private Link<IStringId, int>[] _avlTree;
//    private int _count;
//    private int _capacity;

//    public int Count { get { return _count; } }
//    public int Capacity { get { return _capacity; } }
//    public bool IsEmpty { get { return _count == 0; } }

//    public IStringId Find(string name) { return Find(name, 1); }

//    public AvlTreeArray(int capacity)
//    {
//      _capacity = capacity;
//      // Due to the nature of an AVL tree, the actual capacity must be a power of 2
//      capacity = NextPowerOfTwo(capacity);
//      _avlTree = new Link<IStringId, int>[capacity];
//    }

//    /// <summary>Finds the first power of two greater than or equal to "minimum."</summary>
//    /// <param name="minimum">The minimum bound.</param>
//    /// <returns>The first power of two greater than or equal to "minimum."</returns>
//    /// <remarks>Runtime: Theta(ln(minimum)).</remarks>
//    private int NextPowerOfTwo(int minimum)
//    {
//      if (minimum > Int32.MaxValue / 2)
//        throw new Exception("Attempting to construct an AVL Tree of too large size: " + minimum + ".");
//      int nextPowerOfTwo = 2;
//      while (nextPowerOfTwo < minimum)
//        nextPowerOfTwo *= 2;
//      return nextPowerOfTwo;
//    }

//    private IStringId Find(string name, int index)
//    {
//      if (_avlTree[index] == null)
//        throw new Exception("Attempting to find a non-existing value.");
//      int compResult = name.CompareTo(_avlTree[index]);
//      if (compResult == 0)
//        return _avlTree[index].Left;
//      else if (compResult < 0)
//        // NOTE: "index * 2" is the index of the leftchild of the item at location "index"
//        return Find(name, index * 2);
//      else
//        // NOTE: "(index * 2) + 1" is the index of the rightchild of the item at location "index"
//        return Find(name, index * 2 + 1);
//    }

//    public void Add(IStringId item)
//    {
//      Add(item, 1);
//      _count++;
//    }

//    private void Add(IStringId addition, int index)
//    {
//      if (_avlTree[index].Left == null)
//      {
//        _avlTree[index].Left = addition;
//        _avlTree[index].Right = 0;
//      }
//      else
//      {
//        int compResult = addition.Id.CompareTo(_avlTree[index].Left.Id);//info.Name.CompareTo(t.Root.Left.Name);
//        if (compResult == 0)
//          throw new InvalidOperationException("A NameInfo with the given name already exists.");
//        else if (compResult < 0)
//        {
//          // NOTE: "index * 2" is the index of the leftchild of the item at location "index"
//          Add(addition, index * 2);
//          Balance(index);
//        }
//        else
//        {
//          // NOTE: "(index * 2) + 1" is the index of the rightchild of the item at location "index"
//          Add(addition, index * 2 + 1);
//          Balance(index);
//        }
//      }
//    }

//    //public void CopyTo(IList list) { CopyTo(1, list); }

//    //private void CopyTo(int index, IList list)
//    //{
//    //  if (_avlTree[index].Left != null)
//    //  {
//    //    // NOTE: "index * 2" is the index of the leftchild of the item at location "index"
//    //    CopyTo(index * 2, list);
//    //    list.Add(_avlTree[index]);
//    //    // NOTE: "(index * 2) + 1" is the index of the rightchild of the item at location "index"
//    //    CopyTo(index * 2 + 1, list);
//    //  }
//    //}

//    /// <summary>Finds the height of the given tree.</summary>
//    /// <param name="t">The tree.</param>
//    /// <returns>The height of t.</returns>
//    private int Height(int index)
//    {
//      if (_avlTree[index].Left == null)
//        return -1;
//      else
//        return _avlTree[index].Right;
//    }

//    /// <summary>Performs a single rotate right on the given tree.</summary>
//    /// <param name="t">The tree to rotate.</param>
//    /// <returns>The resulting tree.</returns>
//    private IStringId SingleRotateRight(int index)
//    {
//      ArraySwap(
//      IStringId temp = _avlTree[index * 2 + 1];
//      t.LeftChild = temp.RightChild;
//      temp.RightChild = t;
//      SetHeight(t);
//      SetHeight(temp);
//      return temp;
//    }

//    /// <summary>Sets the height of the given tree based on the heights set for its children.</summary>
//    /// <param name="t">The tree.</param>
//    private void SetHeight(int index)
//    {
//      // Functionality Note: imutable or mutable (next three lines)
//      if (Height(t.LeftChild) < Height(t.RightChild))
//        // t.Root = new Link<Type, int>(t.Root.Left, Math.Max(Height(t.LeftChild), Height(t.RightChild)) + 1);
//        t.Height = Math.Max(Height(t.LeftChild), Height(t.RightChild)) + 1;
//    }

//    /// <summary>Performs a single rotate left on the given tree.</summary>
//    /// <param name="t">The tree to rotate.</param>
//    /// <returns>The resulting tree.</returns>
//    private AvlTreeNode<IStringId> SingleRotateLeft(int index)
//    {
//      AvlTreeNode<IStringId> temp = t.RightChild;
//      t.RightChild = temp.LeftChild;
//      temp.LeftChild = t;
//      SetHeight(t);
//      SetHeight(temp);
//      return temp;
//    }

//    /// <summary>Performs a double rotate right on the given tree.</summary>
//    /// <param name="t">The tree to rotate.</param>
//    /// <returns>The resulting tree.</returns>
//    private AvlTreeNode<IStringId> DoubleRotateRight(AvlTreeNode<IStringId> t)
//    {
//      AvlTreeNode<IStringId> temp = t.LeftChild.RightChild;
//      t.LeftChild.RightChild = temp.LeftChild;
//      temp.LeftChild = t.LeftChild;
//      t.LeftChild = temp.RightChild;
//      temp.RightChild = t;
//      SetHeight(temp.LeftChild);
//      SetHeight(temp.RightChild);
//      SetHeight(temp);
//      return temp;
//    }

//    /// <summary>Performs a double rotate left on the given tree.</summary>
//    /// <param name="t">The tree to rotate.</param>
//    /// <returns>The resulting tree.</returns>
//    private AvlTreeNode<IStringId> DoubleRotateLeft(AvlTreeNode<IStringId> t)
//    {
//      AvlTreeNode<IStringId> temp = t.RightChild.LeftChild;
//      t.RightChild.LeftChild = temp.RightChild;
//      temp.RightChild = t.RightChild;
//      t.RightChild = temp.LeftChild;
//      temp.LeftChild = t;
//      SetHeight(temp.LeftChild);
//      SetHeight(temp.RightChild);
//      SetHeight(temp);
//      return temp;
//    }

//    private AvlTreeNode<IStringId> Balance(AvlTreeNode<IStringId> t)
//    {
//      if (Height(t.LeftChild) == Height(t.RightChild) + 2)
//      {
//        if (Height(t.LeftChild.LeftChild) > Height(t.RightChild))
//          return SingleRotateRight(t);
//        else
//          return DoubleRotateRight(t);
//      }
//      else if (Height(t.RightChild) == Height(t.LeftChild) + 2)
//      {
//        if (Height(t.RightChild.RightChild) > Height(t.LeftChild))
//          return SingleRotateLeft(t);
//        else
//          return DoubleRotateLeft(t);
//      }
//      SetHeight(t);
//      return t;
//    }

//    /// <summary>Standard array swap method.</summary>
//    /// <param name="indexOne">The first index of the swap.</param>
//    /// <param name="indexTwo">The second index of the swap.</param>
//    /// <remarks>Runtime O(1).</remarks>
//    private void ArraySwap(int indexOne, int indexTwo)
//    {
//      Link<IStringId, int> swapStorage = _avlTree[indexTwo];
//      _avlTree[indexTwo] = _avlTree[indexOne];
//      _avlTree[indexOne] = swapStorage;
//    }
//  }

//  #endregion

//  #region AvlTree

//  public class AvlTree
//  {
//    private class AvlTreeNode
//    {
//      private IStringId _root;
//      private AvlTreeNode _leftChild;
//      private AvlTreeNode _rightChild;
//      private int _height;

//      /// <summary> Gets or sets the data stored in the root.</summary>
//      public IStringId Root { get { return _root; } set { _root = value; } }
//      /// <summary>Gets or sets the left child.</summary>
//      public AvlTreeNode LeftChild { get { return _leftChild; } set { _leftChild = value; } }
//      /// <summary>Gets or sets the right child.</summary>
//      public AvlTreeNode RightChild { get { return _rightChild; } set { _rightChild = value; } }
//      /// <summary>Gets the height of the avl tree node.</summary>
//      public int Height { get { return _height; } set { _height = value; } }

//      /// <summary>Constructs an AvlTree node.</summary>
//      /// <param name="root">The value of the </param>
//      /// <param name="leftChild"></param>
//      /// <param name="rightChild"></param>
//      public AvlTreeNode(IStringId root, AvlTreeNode leftChild, AvlTreeNode rightChild, int height)
//      {
//        _root = root;
//        _leftChild = leftChild;
//        _rightChild = rightChild;
//        _height = height;
//      }
//    }

//    private AvlTreeNode _elements = null;
//    private int _count = 0;

//    /// <summary>Gets the number of elements in the collection.</summary>
//    public int Count { get { return _count; } }
//    /// <summary>Gets whether the binary search tree is empty.</summary>
//    public bool IsEmpty { get { return _elements == null; } }

//    /// <summary>Finds the NameInfo containing the given name. If no such NameInfo exists, returns null.</summary>
//    /// <param name="name">The name to look for.</param>
//    /// <returns>The NameInfo containing name, or null if no such NameInfo exists.</returns>
//    public IStringId Find(string name) { return Find(name, _elements); }

//    /// <summary>Finds the NameInfo containing the given name in the given binary search tree.
//    /// If no such NameInfo exists, returns null.</summary>
//    /// <param name="name">The name to look for.</param>
//    /// <param name="t">The binary search tree to look in.</param>
//    /// <returns></returns>
//    private IStringId Find(string name, AvlTreeNode t)
//    {
//      if (t == null)
//        throw new Exception("Attempting to find a non-existing value.");
//      int compResult = 0;// = name.CompareTo(t.Root.Left.Name);
//      if (compResult == 0)
//        return t.Root;
//      else if (compResult < 0)
//        return Find(name, t.LeftChild);
//      else
//        return Find(name, t.RightChild);
//    }

//    /// <summary>Adds the given NameInfo to the collection. If a NameInfo with the same
//    /// name already exists, throws an InvalidOperationException.</summary>
//    /// <param name="item">The NameInfo to add.</param>
//    public void Add(IStringId item)
//    {
//      _elements = Add(item, _elements);
//      _count++;
//    }

//    /// <summary>Adds the given NameInfo to the given binary search tree.  If a NameInfo
//    /// with the same name already exists in t, throws an InvalidOperationException.</summary>
//    /// <param name="addition">The NameInfo to add.</param>
//    /// <param name="t">The binary search tree to add to.</param>
//    /// <returns>The resulting tree.</returns>
//    private AvlTreeNode Add(IStringId addition, AvlTreeNode t)
//    {
//      if (t == null)
//        return new AvlTreeNode(addition, null, null, 0);
//      else
//      {
//        int compResult = addition.Id.CompareTo(t.Root.Id);
//        if (compResult == 0)
//          throw new InvalidOperationException("A NameInfo with the given name already exists.");
//        else if (compResult < 0)
//        {
//          t.LeftChild = Add(addition, t.LeftChild);
//          return Balance(t);
//        }
//        else
//        {
//          t.RightChild = Add(addition, t.RightChild);
//          return Balance(t);
//        }
//      }
//    }

//    ///// <summary>Copies all elements to the end of the given IList in alphabetic order.</summary>
//    ///// <param name="list">The list to copy to.</param>
//    //public void CopyTo(IList list) { CopyTo(_elements, list); }

//    ///// <summary>Copies all elements of the given binary search tree to the end of the given IList in alphabetic order.</summary>
//    ///// <param name="t">The binary search tree to copy.</param>
//    ///// <param name="list">The list to copy to.</param>
//    //private void CopyTo(AvlTreeNode<Type> t, IList list)
//    //{
//    //  if (t != null)
//    //  {
//    //    CopyTo(t.LeftChild, list);
//    //    list.Add(t.Root);
//    //    CopyTo(t.RightChild, list);
//    //  }
//    //}

//    /// <summary>Finds the height of the given tree.</summary>
//    /// <param name="t">The tree.</param>
//    /// <returns>The height of t.</returns>
//    private int Height(AvlTreeNode t)
//    {
//      if (t == null)
//        return -1;
//      else
//        return t.Height;
//    }

//    /// <summary>Performs a single rotate right on the given tree.</summary>
//    /// <param name="t">The tree to rotate.</param>
//    /// <returns>The resulting tree.</returns>
//    private AvlTreeNode SingleRotateRight(AvlTreeNode t)
//    {
//      AvlTreeNode temp = t.LeftChild;
//      t.LeftChild = temp.RightChild;
//      temp.RightChild = t;
//      SetHeight(t);
//      SetHeight(temp);
//      return temp;
//    }

//    /// <summary>Sets the height of the given tree based on the heights set for its children.</summary>
//    /// <param name="t">The tree.</param>
//    private void SetHeight(AvlTreeNode t)
//    {
//      // Functionality Note: imutable or mutable (next three lines)
//      if (Height(t.LeftChild) < Height(t.RightChild))
//        // t.Root = new Link<Type, int>(t.Root.Left, Math.Max(Height(t.LeftChild), Height(t.RightChild)) + 1);
//        t.Height = Math.Max(Height(t.LeftChild), Height(t.RightChild)) + 1;
//    }

//    /// <summary>Performs a single rotate left on the given tree.</summary>
//    /// <param name="t">The tree to rotate.</param>
//    /// <returns>The resulting tree.</returns>
//    private AvlTreeNode SingleRotateLeft(AvlTreeNode t)
//    {
//      AvlTreeNode temp = t.RightChild;
//      t.RightChild = temp.LeftChild;
//      temp.LeftChild = t;
//      SetHeight(t);
//      SetHeight(temp);
//      return temp;
//    }

//    /// <summary>Performs a double rotate right on the given tree.</summary>
//    /// <param name="t">The tree to rotate.</param>
//    /// <returns>The resulting tree.</returns>
//    private AvlTreeNode DoubleRotateRight(AvlTreeNode t)
//    {
//      AvlTreeNode temp = t.LeftChild.RightChild;
//      t.LeftChild.RightChild = temp.LeftChild;
//      temp.LeftChild = t.LeftChild;
//      t.LeftChild = temp.RightChild;
//      temp.RightChild = t;
//      SetHeight(temp.LeftChild);
//      SetHeight(temp.RightChild);
//      SetHeight(temp);
//      return temp;
//    }

//    /// <summary>Performs a double rotate left on the given tree.</summary>
//    /// <param name="t">The tree to rotate.</param>
//    /// <returns>The resulting tree.</returns>
//    private AvlTreeNode DoubleRotateLeft(AvlTreeNode t)
//    {
//      AvlTreeNode temp = t.RightChild.LeftChild;
//      t.RightChild.LeftChild = temp.RightChild;
//      temp.RightChild = t.RightChild;
//      t.RightChild = temp.LeftChild;
//      temp.LeftChild = t;
//      SetHeight(temp.LeftChild);
//      SetHeight(temp.RightChild);
//      SetHeight(temp);
//      return temp;
//    }

//    private AvlTreeNode Balance(AvlTreeNode t)
//    {
//      if (Height(t.LeftChild) == Height(t.RightChild) + 2)
//      {
//        if (Height(t.LeftChild.LeftChild) > Height(t.RightChild))
//          return SingleRotateRight(t);
//        else
//          return DoubleRotateRight(t);
//      }
//      else if (Height(t.RightChild) == Height(t.LeftChild) + 2)
//      {
//        if (Height(t.RightChild.RightChild) > Height(t.LeftChild))
//          return SingleRotateLeft(t);
//        else
//          return DoubleRotateLeft(t);
//      }
//      SetHeight(t);
//      return t;
//    }
//  }

//  #endregion
//}