//// SEVENENGINE LISCENSE:
//// You are free to use, modify, and distribute any or all code segments/files for any purpose
//// including commercial use under the following condition: any code using or originally taken 
//// from the SevenEngine project must include citation to its original author(s) located at the
//// top of each source code file, or you may include a reference to the SevenEngine project as
//// a whole but you must include the current SevenEngine official website URL and logo.
//// - Thanks.  :)  (support: seven@sevenengine.com)

//// Author(s):
//// - Zachary Aaron Patten (aka Seven) seven@sevenengine.com
//// Last Edited: 11-16-13

//using System;
//using SevenEngine.Mathematics;

//namespace SevenEngine.DataStructures
//{
//  /// <summary>Implementing classes contain a vector indicating X and Y position coordinates.</summary>
//  public interface IQuadTreeEntry
//  {
//    /// <summary>Indicates an object's X and Y position coordinates.</summary>
//    Vector Position { get; set; }
//  }

//  [Serializable]
//  public class QuadtreeLinked<ValueType, KeyType>
//  {
//    #region QuadtreeLinkedNode

//    public class QuadtreeLinkedNode<Type>
//      where Type : IQuadTreeEntry
//    {
//      private float _minX, _minY, _maxX, _maxY;
//      private QuadtreeLinkedNode<Type> _child0, _child1, _child2, _child3;
      
//      public float MinX { get { return _minX; } set { _minX = value; } }
//      public float MinY { get { return _minY; } set { _minY = value; } }
//      public float MaxX { get { return _maxX; } set { _maxX = value; } }
//      public float MaxY { get { return _maxY; } set { _maxY = value; } }

//      public QuadtreeLinkedNode<Type> Child0 { get { return _child0; } set { _child0 = value; } }
//      public QuadtreeLinkedNode<Type> Child1 { get { return _child1; } set { _child1 = value; } }
//      public QuadtreeLinkedNode<Type> Child2 { get { return _child2; } set { _child2 = value; } }
//      public QuadtreeLinkedNode<Type> Child3 { get { return _child3; } set { _child3 = value; } }

//      public QuadtreeLinkedNode(float minX, float minY, float maxX, float maxY)
//      {
//        _minX = minX;
//        _minY = minY;
//        _maxX = maxX;
//        _maxY = maxY;
//      }
//    }

//    #endregion

//    #region QuadtreeLinkedBranch

//    public class QuadtreeLinkedBranch<Type> : QuadtreeLinkedNode<Type>
//      where Type : IQuadTreeEntry
//    {
//      private float _minX, _minY, _maxX, _maxY;
//      private QuadtreeLinkedNode<Type> _child0, _child1, _child2, _child3;

//      public QuadtreeLinkedNode<Type> Child0 { get { return _child0; } set { _child0 = value; } }
//      public QuadtreeLinkedNode<Type> Child1 { get { return _child1; } set { _child1 = value; } }
//      public QuadtreeLinkedNode<Type> Child2 { get { return _child2; } set { _child2 = value; } }
//      public QuadtreeLinkedNode<Type> Child3 { get { return _child3; } set { _child3 = value; } }

//      public QuadtreeLinkedBranch(float minX, float minY, float maxX, float maxY)
//        : base(minX, minY, maxX, maxY) { }
//    }

//    #endregion

//    #region QuadtreeLinkedLeaf

//    public class QuadtreeLinkedLeaf<Type> : QuadtreeLinkedNode<Type>
//      where Type : IQuadTreeEntry
//    {
//      private Type[] _contents;

//      public Type[] Contents { get { return _contents; } }

//      public QuadtreeLinkedLeaf(float minX, float minY, float maxX, float maxY, int branchFactor)
//        : base(minX, minY, maxX, maxY) { _contents = new Type[branchFactor]; }
//    }

//    #endregion

//    #region OctreeLinkedReference

//    private class QuadtreeLinkedReference<Type>
//    {
//      private ValueType _value;
//      private QuadtreeLinkedLeaf<ValueType> _leaf;

//      internal ValueType Value { get { return _value; } set { _value = value; } }
//      internal QuadtreeLinkedLeaf<ValueType> Leaf { get { return _leaf; } set { _leaf = value; } }

//      internal QuadtreeLinkedReference<>(ValueType value, QuadtreeLinkedLeaf<ValueType> leaf) { _value = value; _leaf = leaf; }
//    }

//    #endregion

//    private int _branchFactor;
//    private int _count;
//    private AvlTree<QuadtreeLinkedReference, KeyType, ValueType> _referenceDatabase;
//    private QuadtreeLinkedNode<ValueType> _top;

//    private object _lock;
//    private int _readers;
//    private int _writers;

//    public int Count { get { return _count; } }
//    public bool IsEmpty { get { return _count == 0; } }
//  }
//}
