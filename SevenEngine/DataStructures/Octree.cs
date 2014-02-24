// SEVENENGINE LISCENSE:
// You are free to use, modify, and distribute any or all code segments/files for any purpose
// including commercial use under the following condition: any code using or originally taken 
// from the SevenEngine project must include citation to its original author(s) located at the
// top of each source code file, or you may include a reference to the SevenEngine project as
// a whole but you must include the current SevenEngine official website URL and logo.
// - Thanks.  :)  (support: seven@sevenengine.com)

// Author(s):
// - Zachary Aaron Patten (aka Seven) seven@sevenengine.com
// Last Edited: 11-23-13

using System;
using System.Threading;
using SevenEngine.Mathematics;
using SevenEngine.DataStructures;

namespace SevenEngine.DataStructures
{
  /// <summary>Implementing classes contain a vector indicating X, Y, and Z position coordinates.</summary>
  public interface IOctreeEntry
  {
    /// <summary>Indicates an object's X, Y, and Z position coordinates.</summary>
    Vector Position { get; set; }
  }

  public interface Octree<Type> : DataStructure<Type>
    where Type : IOctreeEntry
  {
    int Count { get; }
    bool IsEmpty { get; }
    void Add(Type addition);
    //void Remove(KeyType removal);
    bool TraverseBreakable(Func<Type, bool> traversalFunction, float minX, float minY, float minZ, float maxX, float maxY, float maxZ);
    void Traverse(Action<Type> traversalAction, float minX, float minY, float minZ, float maxX, float maxY, float maxZ);
    //void Move(KeyType moving);
    void Update();
  }

  #region OctreeLinked

  [Serializable]
  public class OctreeLinked<Type> : Octree<Type>
    where Type : IOctreeEntry
  {
    private Func<Type, Type, int> _comparisonFunction;
    private Func<OctreeLinkedReference, Type, int> _referenceComparison;

    #region OctreeLinkedNode

    /// <summary>Represents a single node of the octree. Includes references both upwards and
    /// downwards the tree.</summary>
    private abstract class OctreeLinkedNode
    {
      private float _x, _y, _z, _scale;
      private OctreeLinkedBranch _parent;

      internal float X { get { return _x; } }
      internal float Y { get { return _y; } }
      internal float Z { get { return _z; } }
      internal float Scale { get { return _scale; } }
      internal OctreeLinkedBranch Parent { get { return _parent; } }

      internal OctreeLinkedNode(float x, float y, float z, float scale, OctreeLinkedBranch parent)
      {
        _x = x;
        _y = y;
        _z = z;
        _scale = scale;
        _parent = parent;
      }

      /// <summary>Finds the child index relative to "this" node given x, y, and z coordinates.</summary>
      static internal int DetermineChild(OctreeLinkedNode node, float x, float y, float z)
      {
        // Finds the child given an x, y, and z
        // Possible child (all): 0, 1, 2, 3, 4, 5, 6, 7
        if (z < node.Z)
        {
          // Possible child: 0, 2, 4, 6
          if (y < node.Y)
            // Possible child: 0, 4
            if (x < node.X) return 0;
            else return 4;
          else
            // Possible child: 2, 6, 
            if (x < node.X) return 2;
            else return 6;
        }
        else
        {
          // Possible child: 1, 3, 5, 7
          if (y < node.Y)
            // Possible child: 1, 5
            if (x < node.X) return 1;
            else return 5;
          else
            // Possible child: 3, 7 
            if (x < node.X) return 3;
            else return 7;
        }
      }

      /// <summary>Determins the bounds of a child node.</summary>
      static internal void DetermineChildBounds(OctreeLinkedNode node, int child, out float x, out float y, out float z, out float scale)
      {
        float halfScale = node.Scale * .5f;
        switch (child)
        {
          case 0: x = node.X - halfScale; y = node.Y - halfScale; z = node.Z - halfScale; scale = halfScale; return;
          case 1: x = node.X - halfScale; y = node.Y - halfScale; z = node.Z + halfScale; scale = halfScale; return;
          case 2: x = node.X - halfScale; y = node.Y + halfScale; z = node.Z - halfScale; scale = halfScale; return;
          case 3: x = node.X - halfScale; y = node.Y + halfScale; z = node.Z + halfScale; scale = halfScale; return;
          case 4: x = node.X + halfScale; y = node.Y - halfScale; z = node.Z - halfScale; scale = halfScale; return;
          case 5: x = node.X + halfScale; y = node.Y - halfScale; z = node.Z + halfScale; scale = halfScale; return;
          case 6: x = node.X + halfScale; y = node.Y + halfScale; z = node.Z - halfScale; scale = halfScale; return;
          case 7: x = node.X + halfScale; y = node.Y + halfScale; z = node.Z + halfScale; scale = halfScale; return;
          default: throw new OctreeLinkedException("There is a glitch in my octree, sorry...");
        }
      }

      static internal bool ContainsBounds(OctreeLinkedNode node, float xMin, float yMin, float zMin, float xMax, float yMax, float zMax)
      {
        return !(node == null ||
            xMax < node.X - node.Scale || xMin > node.X + node.Scale ||
            yMax < node.Y - node.Scale || yMin > node.Y + node.Scale ||
            zMax < node.Z - node.Scale || zMin > node.Z + node.Scale);
      }

      static internal bool ContainsCoordinate(OctreeLinkedNode node, float x, float y, float z)
      {
        return !(node == null ||
            x < node.X - node.Scale || x > node.X + node.Scale ||
            y < node.Y - node.Scale || y > node.Y + node.Scale ||
            z < node.Z - node.Scale || z > node.Z + node.Scale);
      }
    }

    #endregion

    #region OctreeLinkedLeaf

    /// <summary>Represents a single node of the octree. Includes references both upwards and
    /// downwards the tree.</summary>
    private class OctreeLinkedLeaf : OctreeLinkedNode
    {
      //private OctreeEntry[] _contents;
      private Type[] _contents;
      private int _count;

      //internal OctreeEntry[] Contents { get { return _contents; } }
      internal Type[] Contents { get { return _contents; } }
      internal int Count { get { return _count; } set { _count = value; } }
      internal bool IsFull { get { return _count == _contents.Length; } }

      internal OctreeLinkedLeaf(float x, float y, float z, float scale, OctreeLinkedBranch parent, int branchFactor)
        : base(x, y, z, scale, parent)
      { _contents = new Type[branchFactor]; }

      internal OctreeLinkedLeaf Add(Type addition)
      {
        if (_count == _contents.Length)
          throw new OctreeLinkedException("There is a glitch in my octree, sorry...");
        _contents[_count++] = addition;
        return this;
      }
    }

    #endregion

    #region OctreelinkedBranch

    /// <summary>Represents a single node of the octree. Includes references both upwards and
    /// downwards the tree.</summary>
    private class OctreeLinkedBranch : OctreeLinkedNode
    {
      // The children are indexed as follows (relative to this node's center):
      // 0: (-x, -y, -z)   1: (-x, -y, z)   2: (-x, y, -z)   3: (-x, y, z)
      // 4: (x, -y, -z)   5: (x, -y, z)   6: (x, y, -z)   7: (x, y, z)
      //private OctreeLinkedNode[] _children;

      private OctreeLinkedNode _child0;
      private OctreeLinkedNode _child1;
      private OctreeLinkedNode _child2;
      private OctreeLinkedNode _child3;
      private OctreeLinkedNode _child4;
      private OctreeLinkedNode _child5;
      private OctreeLinkedNode _child6;
      private OctreeLinkedNode _child7;

      public OctreeLinkedNode Child0 { get { return _child0; } }
      public OctreeLinkedNode Child1 { get { return _child1; } }
      public OctreeLinkedNode Child2 { get { return _child2; } }
      public OctreeLinkedNode Child3 { get { return _child3; } }
      public OctreeLinkedNode Child4 { get { return _child4; } }
      public OctreeLinkedNode Child5 { get { return _child5; } }
      public OctreeLinkedNode Child6 { get { return _child6; } }
      public OctreeLinkedNode Child7 { get { return _child7; } }

      public OctreeLinkedNode this[int index]
      {
        get
        {
          switch (index)
          {
            case 0: return _child0;
            case 1: return _child1;
            case 2: return _child2;
            case 3: return _child3;
            case 4: return _child4;
            case 5: return _child5;
            case 6: return _child6;
            case 7: return _child7;
            default: throw new OctreeLinkedException("index out of bounds.");
          }
        }
        set
        {
          switch (index)
          {
            case 0: _child0 = value; break;
            case 1: _child1 = value; break;
            case 2: _child2 = value; break;
            case 3: _child3 = value; break;
            case 4: _child4 = value; break;
            case 5: _child5 = value; break;
            case 6: _child6 = value; break;
            case 7: _child7 = value; break;
            default: throw new OctreeLinkedException("index out of bounds.");
          }
        }
      }

      //internal OctreeLinkedNode[] Children { get { return _children; } }

      internal bool IsEmpty
      {
        get
        {
          //return _children[0] == null && _children[1] == null && _children[2] == null
          //  && _children[3] == null && _children[4] == null && _children[5] == null
          //  && _children[6] == null && _children[7] == null;
          return _child0 == null && _child1 == null && _child2 == null
            && _child3 == null && _child4 == null && _child5== null
            && _child6 == null && _child7 == null;
        }
      }

      internal OctreeLinkedBranch(float x, float y, float z, float scale, OctreeLinkedBranch parent)
        : base(x, y, z, scale, parent)
      {
        //_children = new OctreeLinkedNode[8];
      }
    }

    #endregion

    #region OctreeLinkedReference

    private class OctreeLinkedReference
    {
      private Type _value;
      private OctreeLinkedLeaf _leaf;

      internal Type Value { get { return _value; } set { _value = value; } }
      internal OctreeLinkedLeaf Leaf { get { return _leaf; } set { _leaf = value; } }

      internal OctreeLinkedReference(Type value, OctreeLinkedLeaf leaf) { _value = value; _leaf = leaf; }
    }

    #endregion

    private int _branchFactor;
    private int _count;
    private AvlTree<OctreeLinkedReference> _referenceDatabase;
    private OctreeLinkedNode _top;

    private object _lock;
    private int _readers;
    private int _writers;

    public int Count { get { return _count; } }
    public bool IsEmpty { get { return _count == 0; } }

    /// <summary>Creates an octree for three dimensional space partitioning.</summary>
    /// <param name="x">The x coordinate of the center of the octree.</param>
    /// <param name="y">The y coordinate of the center of the octree.</param>
    /// <param name="z">The z coordinate of the center of the octree.</param>
    /// <param name="scale">How far the tree expands along each dimension.</param>
    /// <param name="branchFactor">The maximum items per octree node before expansion.</param>
    public OctreeLinked(float x, float y, float z, float scale, int branchFactor,
      Func<Type, Type, int> comparisonFunction)
    {
      _branchFactor = branchFactor;
      _top = new OctreeLinkedLeaf(x, y, z, scale, null, _branchFactor);
      _count = 0;
      _lock = new object();
      _readers = 0;
      _writers = 0;

      _comparisonFunction = comparisonFunction;

      _referenceComparison =
        (OctreeLinkedReference left, Type right) =>
        { return comparisonFunction(left.Value, right); };

      Func<OctreeLinkedReference, OctreeLinkedReference, int> octreeReferenceComparison =
        (OctreeLinkedReference left, OctreeLinkedReference right) =>
        { return comparisonFunction(left.Value, right.Value); };
      _referenceDatabase = new AvlTreeLinked<OctreeLinkedReference>(octreeReferenceComparison);
    }

    /// <summary>Adds an item to the Octree.</summary>
    /// <param name="id">The id associated with the addition.</param>
    /// <param name="addition">The addition.</param>
    /// <param name="x">The x coordinate of the addition's location.</param>
    /// <param name="y">The y coordinate of the addition's location.</param>
    /// <param name="z">The z coordinate of the addition's location.</param>
    public void Add(Type addition)
    {
      WriterLock();
      _referenceDatabase.Add(new OctreeLinkedReference(addition, Add(addition, _top)));
      _count++;
      WriterUnlock();
    }

    /// <summary>Recursively adds an item to the octree and returns the node where the addition was placed
    /// and adjusts the octree structure as needed.</summary>
    private OctreeLinkedLeaf Add(Type addition, OctreeLinkedNode octreeNode)
    {
      // If the node is a leaf we have reached the bottom of the tree
      if (octreeNode is OctreeLinkedLeaf)
      {
        OctreeLinkedLeaf leaf = (OctreeLinkedLeaf)octreeNode;
        if (!leaf.IsFull)
        {
          // We found a proper leaf, and the leaf has room, just add it
          leaf.Add(addition);
          return leaf;
        }
        else
        {
          // The leaf is full so we need to grow out the tree
          OctreeLinkedBranch parent = octreeNode.Parent;
          OctreeLinkedBranch growth;
          if (parent == null)
            growth = (OctreeLinkedBranch)(_top = new OctreeLinkedBranch(_top.X, _top.Y, _top.Z, _top.Scale, null));
          else
            growth = GrowBranch(parent, OctreeLinkedNode.DetermineChild(parent, addition.Position.X, addition.Position.Y, addition.Position.Z));
          foreach (Type entry in leaf.Contents)
            _referenceDatabase.Get<Type>(entry, _referenceComparison).Leaf = Add(entry, growth);
          return Add(addition, growth);
        }
      }
      // We are still traversing the tree, determine the next move
      else
      {
        OctreeLinkedBranch branch = (OctreeLinkedBranch)octreeNode;
        int child = OctreeLinkedNode.DetermineChild(branch, addition.Position.X, addition.Position.Y, addition.Position.Z);
        // If the leaf is null, we must grow one before attempting to add to it
        if (branch[child] == null)
          return GrowLeaf(branch, child).Add(addition);
        return Add(addition, branch[child]);
      }
    }

    // Grows a branch on the tree at the desired location
    private OctreeLinkedBranch GrowBranch(OctreeLinkedBranch branch, int child)
    {
      // values for the new node
      float x, y, z, scale;
      OctreeLinkedNode.DetermineChildBounds(branch, child, out x, out y, out z, out scale);
      branch[child] = new OctreeLinkedBranch(x, y, z, scale, branch);
      return (OctreeLinkedBranch)branch[child];
    }

    // Grows a leaf on the tree at the desired location
    private OctreeLinkedLeaf GrowLeaf(OctreeLinkedBranch branch, int child)
    {
      if (branch[child] != null)
        throw new OctreeLinkedException("My octree has a glitched, sorry.");
      // values for new node
      float x, y, z, scale;
      OctreeLinkedNode.DetermineChildBounds(branch, child, out x, out y, out z, out scale);
      branch[child] = new OctreeLinkedLeaf(x, y, z, scale, branch, _branchFactor);
      return (OctreeLinkedLeaf)branch[child];
    }

    /// <summary>Removes an item from the octree by the id that was assigned to it.</summary>
    /// <param name="id">The string id of the removal that was given to the item when it was added.</param>
    public void Remove(Type key)
    {
      WriterLock();
      Remove(key, _referenceDatabase.Get<Type>(key, _referenceComparison).Leaf);
      _referenceDatabase.Remove<Type>(key, _referenceComparison);
      _count--;
      WriterUnlock();
    }

    private void Remove(Type key, OctreeLinkedLeaf leaf)
    {
      if (leaf.Count > 1)
      {
        Type[] contents = leaf.Contents;
        for (int i = 0; i < leaf.Count; i++)
          if (_comparisonFunction(contents[i], key) == 0)
          {
            Type temp = contents[_count - 1];
            contents[_count - 1] = contents[i];
            contents[i] = temp;
            break;
          }
      }
      else PluckLeaf(leaf.Parent, OctreeLinkedNode.DetermineChild(leaf.Parent, leaf.X, leaf.Y, leaf.Z));
    }

    private void PluckLeaf(OctreeLinkedBranch branch, int child)
    {
      if (!(branch[child] is OctreeLinkedLeaf) || ((OctreeLinkedLeaf)branch[child]).Count > 1)
        throw new OctreeLinkedException("There is a glitch in my octree, sorry.");
      branch[child] = null;
      while (branch.IsEmpty)
      {
        ChopBranch(branch.Parent, OctreeLinkedNode.DetermineChild(branch.Parent, branch.X, branch.Y, branch.Z));
        branch = branch.Parent;
      }
    }

    private void ChopBranch(OctreeLinkedBranch branch, int child)
    {
      if (branch[child] == null)
        throw new OctreeLinkedException("There is a glitch in my octree, sorry...");
      branch[child] = null;
    }

    /// <summary>Moves an existing item from one position to another.</summary>
    /// <param name="key">The key of the item to be moved.</param>
    /// <param name="x">The x coordinate of the new position of the item.</param>
    /// <param name="y">The y coordinate of the new position of the item.</param>
    /// <param name="z">The z coordinate of the new position of the item.</param>
    public void Move(Type key, float x, float y, float z)
    {
      WriterLock();
      OctreeLinkedLeaf leaf = _referenceDatabase.Get<Type>(key, _referenceComparison).Leaf;
      Type entry = default(Type);
      bool found = false;
      foreach (Type value in leaf.Contents)
        if (_comparisonFunction(value, key) == 0)
        {
          entry = value;
          found = true;
          break;
        }
      if (found == false)
        throw new OctreeLinkedException("attempting to move a non-existing value.");
      entry.Position.X = x;
      entry.Position.Y = y;
      entry.Position.Z = z;
      if ((x > leaf.X - leaf.Scale && x < leaf.X + leaf.Scale)
        && (y > leaf.Y - leaf.Scale && y < leaf.Y + leaf.Scale)
        && (z > leaf.Z - leaf.Scale && z < leaf.Z + leaf.Scale))
        return;
      else
      {
        Remove(key, leaf);
        Add(entry, _top);
      }
      WriterLock();
    }

    /// <summary>Iterates through the entire tree and ensures each item is in the proper node.</summary>
    public void Update()
    {
      WriterLock();
      WriterUnlock();
      throw new NotImplementedException("Sorry, I'm still working on the update function.");
      //WriterUnlock();
    }

    /// <summary>Performs a functional paradigm traversal of the octree.</summary>
    /// <param name="traversalFunction"></param>
    public bool TraverseBreakable(Func<Type, bool> traversalFunction)
    {
      ReaderLock();
      if (!TraverseBreakable(traversalFunction, _top))
      {
        ReaderUnlock();
        return false;
      }
      ReaderUnlock();
      return true;
    }
    private bool TraverseBreakable(Func<Type, bool> traversalFunctionBreakable, OctreeLinkedNode octreeNode)
    {
      if (octreeNode != null)
      {
        if (octreeNode is OctreeLinkedLeaf)
        {
          foreach (Type item in ((OctreeLinkedLeaf)octreeNode).Contents)
            if (!traversalFunctionBreakable(item)) return false;
        }
        else
        {
          // The current node is a branch
          OctreeLinkedBranch branch = (OctreeLinkedBranch)octreeNode;
          if (!TraverseBreakable(traversalFunctionBreakable, branch.Child0)) return false;
          if (!TraverseBreakable(traversalFunctionBreakable, branch.Child1)) return false;
          if (!TraverseBreakable(traversalFunctionBreakable, branch.Child2)) return false;
          if (!TraverseBreakable(traversalFunctionBreakable, branch.Child3)) return false;
          if (!TraverseBreakable(traversalFunctionBreakable, branch.Child4)) return false;
          if (!TraverseBreakable(traversalFunctionBreakable, branch.Child5)) return false;
          if (!TraverseBreakable(traversalFunctionBreakable, branch.Child6)) return false;
          if (!TraverseBreakable(traversalFunctionBreakable, branch.Child7)) return false;
        }
      }
      return true;
    }

    public void Traverse(Action<Type> traversalFunction)
    {
      ReaderLock();
      Traverse(traversalFunction, _top);
      ReaderUnlock();
    }
    private void Traverse(Action<Type> traversalFunction, OctreeLinkedNode octreeNode)
    {
      if (octreeNode != null)
      {
        if (octreeNode is OctreeLinkedLeaf)
        {
          foreach (Type item in ((OctreeLinkedLeaf)octreeNode).Contents)
            traversalFunction(item);
        }
        else
        {
          // The current node is a branch
          OctreeLinkedBranch branch = (OctreeLinkedBranch)octreeNode;
          Traverse(traversalFunction, branch.Child0);
          Traverse(traversalFunction, branch.Child1);
          Traverse(traversalFunction, branch.Child2);
          Traverse(traversalFunction, branch.Child3);
          Traverse(traversalFunction, branch.Child4);
          Traverse(traversalFunction, branch.Child5);
          Traverse(traversalFunction, branch.Child6);
          Traverse(traversalFunction, branch.Child7);
        }
      }
    }

    /// <summary>Performs a functional paradigm traversal of the octree with data structure optimization.</summary>
    /// <param name="traversalFunction">The function to perform per iteration.</param>
    /// <param name="xMin">The minimum x of a rectangular prism to query the octree.</param>
    /// <param name="yMin">The minimum y of a rectangular prism to query the octree.</param>
    /// <param name="zMin">The minimum z of a rectangular prism to query the octree.</param>
    /// <param name="xMax">The maximum x of a rectangular prism to query the octree.</param>
    /// <param name="yMax">The maximum y of a rectangular prism to query the octree.</param>
    /// <param name="zMax">The maximum z of a rectangular prism to query the octree.</param>
    public bool TraverseBreakable(Func<Type, bool> traversalFunction, float xMin, float yMin, float zMin, float xMax, float yMax, float zMax)
    {
      ReaderLock();
      bool returnValue = TraverseBreakable(traversalFunction, _top, xMin, yMin, zMin, xMax, yMax, zMax);
      ReaderUnlock();
      return returnValue;
    }
    private bool TraverseBreakable(Func<Type, bool> traversalFunction, OctreeLinkedNode octreeNode, float xMin, float yMin, float zMin, float xMax, float yMax, float zMax)
    {
      if (octreeNode != null)
      {
        if (octreeNode is OctreeLinkedLeaf)
        {
          foreach (Type entry in ((OctreeLinkedLeaf)octreeNode).Contents)
            //if (!traversalFunction(item)) return false;
            if (entry != null &&
            entry.Position.X > xMin && entry.Position.X < xMax
            && entry.Position.Y > yMin && entry.Position.Y < yMax
            && entry.Position.Z > zMin && entry.Position.Z < zMax)
              if (!traversalFunction(entry)) return false;
        }
        else
        {
          OctreeLinkedBranch branch = (OctreeLinkedBranch)octreeNode;
          OctreeLinkedNode node = branch.Child0;
          if (OctreeLinkedNode.ContainsBounds(node, xMin, yMin, zMin, xMax, yMax, zMax))
            if (!TraverseBreakable(traversalFunction, node, xMin, yMin, zMin, xMax, yMax, zMax))
              return false;
          node = branch.Child1;
          if (OctreeLinkedNode.ContainsBounds(node, xMin, yMin, zMin, xMax, yMax, zMax))
            if (!TraverseBreakable(traversalFunction, node, xMin, yMin, zMin, xMax, yMax, zMax))
              return false;
          node = branch.Child2;
          if (OctreeLinkedNode.ContainsBounds(node, xMin, yMin, zMin, xMax, yMax, zMax))
            if (!TraverseBreakable(traversalFunction, node, xMin, yMin, zMin, xMax, yMax, zMax))
              return false;
          node = branch.Child3;
          if (OctreeLinkedNode.ContainsBounds(node, xMin, yMin, zMin, xMax, yMax, zMax))
            if (!TraverseBreakable(traversalFunction, node, xMin, yMin, zMin, xMax, yMax, zMax))
              return false;
          node = branch.Child4;
          if (OctreeLinkedNode.ContainsBounds(node, xMin, yMin, zMin, xMax, yMax, zMax))
            if (!TraverseBreakable(traversalFunction, node, xMin, yMin, zMin, xMax, yMax, zMax))
              return false;
          node = branch.Child5;
          if (OctreeLinkedNode.ContainsBounds(node, xMin, yMin, zMin, xMax, yMax, zMax))
            if (!TraverseBreakable(traversalFunction, node, xMin, yMin, zMin, xMax, yMax, zMax))
              return false;
          node = branch.Child6;
          if (OctreeLinkedNode.ContainsBounds(node, xMin, yMin, zMin, xMax, yMax, zMax))
            if (!TraverseBreakable(traversalFunction, node, xMin, yMin, zMin, xMax, yMax, zMax))
              return false;
          node = branch.Child7;
          if (OctreeLinkedNode.ContainsBounds(node, xMin, yMin, zMin, xMax, yMax, zMax))
            if (!TraverseBreakable(traversalFunction, node, xMin, yMin, zMin, xMax, yMax, zMax))
              return false;
        }
      }
      return true;
    }

    public void Traverse(Action<Type> traversalFunction, float xMin, float yMin, float zMin, float xMax, float yMax, float zMax)
    {
      ReaderLock();
      Traverse(traversalFunction, _top, xMin, yMin, zMin, xMax, yMax, zMax);
      ReaderUnlock();
    }
    private void Traverse(Action<Type> traversalFunction, OctreeLinkedNode octreeNode, float xMin, float yMin, float zMin, float xMax, float yMax, float zMax)
    {
      if (octreeNode != null)
      {
        if (octreeNode is OctreeLinkedLeaf)
        {
          foreach (Type entry in ((OctreeLinkedLeaf)octreeNode).Contents)
            if (entry != null &&
            entry.Position.X > xMin && entry.Position.X < xMax
            && entry.Position.Y > yMin && entry.Position.Y < yMax
            && entry.Position.Z > zMin && entry.Position.Z < zMax)
              traversalFunction(entry);
        }
        else
        {
          OctreeLinkedBranch branch = (OctreeLinkedBranch)octreeNode;
          OctreeLinkedNode node = branch.Child0;
          if (OctreeLinkedNode.ContainsBounds(node, xMin, yMin, zMin, xMax, yMax, zMax))
            Traverse(traversalFunction, node, xMin, yMin, zMin, xMax, yMax, zMax);
          node = branch.Child1;
          if (OctreeLinkedNode.ContainsBounds(node, xMin, yMin, zMin, xMax, yMax, zMax))
            Traverse(traversalFunction, node, xMin, yMin, zMin, xMax, yMax, zMax);
          node = branch.Child2;
          if (OctreeLinkedNode.ContainsBounds(node, xMin, yMin, zMin, xMax, yMax, zMax))
            Traverse(traversalFunction, node, xMin, yMin, zMin, xMax, yMax, zMax);
          node = branch.Child3;
          if (OctreeLinkedNode.ContainsBounds(node, xMin, yMin, zMin, xMax, yMax, zMax))
            Traverse(traversalFunction, node, xMin, yMin, zMin, xMax, yMax, zMax);
          node = branch.Child4;
          if (OctreeLinkedNode.ContainsBounds(node, xMin, yMin, zMin, xMax, yMax, zMax))
            Traverse(traversalFunction, node, xMin, yMin, zMin, xMax, yMax, zMax);
          node = branch.Child5;
          if (OctreeLinkedNode.ContainsBounds(node, xMin, yMin, zMin, xMax, yMax, zMax))
            Traverse(traversalFunction, node, xMin, yMin, zMin, xMax, yMax, zMax);
          node = branch.Child6;
          if (OctreeLinkedNode.ContainsBounds(node, xMin, yMin, zMin, xMax, yMax, zMax))
            Traverse(traversalFunction, node, xMin, yMin, zMin, xMax, yMax, zMax);
          node = branch.Child7;
          if (OctreeLinkedNode.ContainsBounds(node, xMin, yMin, zMin, xMax, yMax, zMax))
            Traverse(traversalFunction, node, xMin, yMin, zMin, xMax, yMax, zMax);
        }
      }
    }

    public Type[] ToArray()
    {
      ReaderLock();
      int finalIndex;
      Type[] array = new Type[_count];
      ToArray(_top, array, 0, out finalIndex);
      if (array.Length != finalIndex)
        throw new OctreeLinkedException("There is a glitch in my octree, sorry...");
      ReaderUnlock();
      return array;
    }
    private void ToArray(OctreeLinkedNode octreeNode, Type[] array, int entryIndex, out int returnIndex)
    {
      if (octreeNode != null)
      {
        if (octreeNode is OctreeLinkedLeaf)
        {
          returnIndex = entryIndex;
          foreach (Type item in ((OctreeLinkedLeaf)octreeNode).Contents)
            array[returnIndex++] = item;
        }
        else
        {
          // The current node is a branch
          OctreeLinkedBranch branch = (OctreeLinkedBranch)octreeNode;
          ToArray(branch.Child0, array, entryIndex, out entryIndex);
          ToArray(branch.Child1, array, entryIndex, out entryIndex);
          ToArray(branch.Child2, array, entryIndex, out entryIndex);
          ToArray(branch.Child3, array, entryIndex, out entryIndex);
          ToArray(branch.Child4, array, entryIndex, out entryIndex);
          ToArray(branch.Child5, array, entryIndex, out entryIndex);
          ToArray(branch.Child6, array, entryIndex, out entryIndex);
          ToArray(branch.Child7, array, entryIndex, out entryIndex);
          returnIndex = entryIndex;
        }
      }
      else
        returnIndex = entryIndex;
    }

    /// <summary>Thread safe enterance for readers.</summary>
    private void ReaderLock() { lock (_lock) { while (!(_writers == 0)) Monitor.Wait(_lock); _readers++; } }
    /// <summary>Thread safe exit for readers.</summary>
    private void ReaderUnlock() { lock (_lock) { _readers--; Monitor.Pulse(_lock); } }
    /// <summary>Thread safe enterance for writers.</summary>
    private void WriterLock() { lock (_lock) { while (!(_writers == 0) && !(_readers == 0)) Monitor.Wait(_lock); _writers++; } }
    /// <summary>Thread safe exit for readers.</summary>
    private void WriterUnlock() { lock (_lock) { _writers--; Monitor.PulseAll(_lock); } }

    /// <summary>This is used for throwing OcTree exceptions only to make debugging faster.</summary>
    private class OctreeLinkedException : Exception { public OctreeLinkedException(string message) : base(message) { } }
  }

  #endregion
}