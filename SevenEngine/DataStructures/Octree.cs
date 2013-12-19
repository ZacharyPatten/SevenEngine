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

  public interface Octree<ValueType> : DataStructure<ValueType>
    where ValueType : IOctreeEntry
  {
    int Count { get; }
    bool IsEmpty { get; }
    void Add(ValueType addition);
    //void Remove(ValueType removal);
    //void TryAdd(ValueType addition);
    //void TryRemove(ValueType removal);
    void Update();
    bool TraverseBreakable(Func<ValueType, bool> traversalFunction, float minX, float minY, float minZ, float maxX, float maxY, float maxZ);
    void Traverse(Action<ValueType> traversalAction, float minX, float minY, float minZ, float maxX, float maxY, float maxZ);
  }

  public interface Octree<ValueType, KeyType> : Octree<ValueType>
    where ValueType : IOctreeEntry
  {
    //void Remove(KeyType removal);
    //void Move(KeyType moving);
  }

  /*public interface Octree<ValueType, FirstKeyType, SecondKeyType> : DataStructure<ValueType>
   // where ValueType : IOctreeEntry
  {
    int Count { get; }
    bool IsEmpty { get; }
    void Add(ValueType addition);
    //void Remove(KeyType removal);
    bool TraverseBreakable(Func<ValueType, bool> traversalFunction, float minX, float minY, float minZ, float maxX, float maxY, float maxZ);
    void Traverse(Action<ValueType> traversalAction, float minX, float minY, float minZ, float maxX, float maxY, float maxZ);
    //void Move(KeyType moving);
    void Update();
  }*/

  #region OctreeLinked

  public class OctreeLinked<ValueType, KeyType> : Octree<ValueType, KeyType>
    where ValueType : IOctreeEntry
  {
    private Func<ValueType, ValueType, int> _valueComparisonFunction;
    private Func<ValueType, KeyType, int> _keyComparisonFunction;

    #region OctreeLinkedBound

    /// <summary>Represents a bounding cube. Includes coordinates of the center 
    /// and a scale of expansion along each axis.</summary>
    private class OctreeLinkedBound
    {
      private float _x, _y, _z, _scale;

      internal float X { get { return _x; } }
      internal float Y { get { return _y; } }
      internal float Z { get { return _z; } }
      internal float Scale { get { return _scale; } }

      internal OctreeLinkedBound(float x, float y, float z, float scale)
      { _x = x; _y = y; _z = z; _scale = scale; }
    }

    #endregion

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
      internal int DetermineChild(float x, float y, float z)
      {
        // Finds the child given an x, y, and z
        // Possible child (all): 0, 1, 2, 3, 4, 5, 6, 7
        if (z < _z)
        {
          // Possible child: 0, 2, 4, 6
          if (y < _y)
            // Possible child: 0, 4
            if (x < _x) return 0;
            else return 4;
          else
            // Possible child: 2, 6, 
            if (x < _x) return 2;
            else return 6;
        }
        else
        {
          // Possible child: 1, 3, 5, 7
          if (y < _y)
            // Possible child: 1, 5
            if (x < _x) return 1;
            else return 5;
          else
            // Possible child: 3, 7 
            if (x < _x) return 3;
            else return 7;
        }
      }

      /// <summary>Determins the bounds of a child node.</summary>
      internal OctreeLinkedBound DetermineChildBounds(int child)
      {
        float halfScale = _scale / 2;
        switch (child)
        {
          case 0:
            return new OctreeLinkedBound(_x - halfScale, _y - halfScale, _z - halfScale, halfScale);
          case 1:
            return new OctreeLinkedBound(_x - halfScale, _y - halfScale, _z + halfScale, halfScale);
          case 2:
            return new OctreeLinkedBound(_x - halfScale, _y + halfScale, _z - halfScale, halfScale);
          case 3:
            return new OctreeLinkedBound(_x - halfScale, _y + halfScale, _z + halfScale, halfScale);
          case 4:
            return new OctreeLinkedBound(_x + halfScale, _y - halfScale, _z - halfScale, halfScale);
          case 5:
            return new OctreeLinkedBound(_x + halfScale, _y - halfScale, _z + halfScale, halfScale);
          case 6:
            return new OctreeLinkedBound(_x + halfScale, _y + halfScale, _z - halfScale, halfScale);
          case 7:
            return new OctreeLinkedBound(_x + halfScale, _y + halfScale, _z + halfScale, halfScale);
        }
        throw new OctreeLinkedException("There is a glitch in my octree, sorry...");
      }
    }

    #endregion

    #region OctreeLinkedLeaf

    /// <summary>Represents a single node of the octree. Includes references both upwards and
    /// downwards the tree.</summary>
    private class OctreeLinkedLeaf : OctreeLinkedNode
    {
      //private OctreeEntry[] _contents;
      private ValueType[] _contents;
      private int _count;

      //internal OctreeEntry[] Contents { get { return _contents; } }
      internal ValueType[] Contents { get { return _contents; } }
      internal int Count { get { return _count; } set { _count = value; } }
      internal bool IsFull { get { return _count == _contents.Length; } }

      internal OctreeLinkedLeaf(float x, float y, float z, float scale, OctreeLinkedBranch parent, int branchFactor)
        : base(x, y, z, scale, parent)
      { _contents = new ValueType[branchFactor]; }

      internal OctreeLinkedLeaf Add(ValueType addition)
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
      private OctreeLinkedNode[] _children;

      internal OctreeLinkedNode[] Children { get { return _children; } }
      internal bool IsEmpty
      {
        get
        {
          return _children[0] == null && _children[1] == null && _children[2] == null
            && _children[3] == null && _children[4] == null && _children[5] == null
            && _children[6] == null && _children[7] == null;
        }
      }

      internal OctreeLinkedBranch(float x, float y, float z, float scale, OctreeLinkedBranch parent)
        : base(x, y, z, scale, parent)
      { _children = new OctreeLinkedNode[8]; }
    }

    #endregion

    #region OctreeLinkedReference

    private class OctreeLinkedReference
    {
      private ValueType _value;
      private OctreeLinkedLeaf _leaf;

      internal ValueType Value { get { return _value; } set { _value = value; } }
      internal OctreeLinkedLeaf Leaf { get { return _leaf; } set { _leaf = value; } }

      internal OctreeLinkedReference(ValueType value, OctreeLinkedLeaf leaf) { _value = value; _leaf = leaf; }
    }

    #endregion

    private int _branchFactor;
    private int _count;
    private AvlTree<OctreeLinkedReference, KeyType, ValueType> _referenceDatabase;
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
      Func<ValueType, ValueType, int> valueComparisonFunction,
      Func<ValueType, KeyType, int> keyComparisonFunction)
    {
      _branchFactor = branchFactor;
      _top = new OctreeLinkedLeaf(x, y, z, scale, null, _branchFactor);
      _count = 0;
      _lock = new object();
      _readers = 0;
      _writers = 0;

      _valueComparisonFunction = valueComparisonFunction;
      _keyComparisonFunction = keyComparisonFunction;

      _referenceDatabase = new AvlTreeLinked<OctreeLinkedReference, KeyType, ValueType>
      (
        (OctreeLinkedReference left, OctreeLinkedReference right) => { return _valueComparisonFunction(left.Value, right.Value); },
        (OctreeLinkedReference left, KeyType right) => { return _keyComparisonFunction(left.Value, right); },
        (OctreeLinkedReference left, ValueType right) => { return _valueComparisonFunction(left.Value, right); }
      );
    }

    /// <summary>Adds an item to the Octree.</summary>
    /// <param name="id">The id associated with the addition.</param>
    /// <param name="addition">The addition.</param>
    /// <param name="x">The x coordinate of the addition's location.</param>
    /// <param name="y">The y coordinate of the addition's location.</param>
    /// <param name="z">The z coordinate of the addition's location.</param>
    public void Add(ValueType addition)
    {
      WriterLock();
      _referenceDatabase.Add(new OctreeLinkedReference(addition, Add(addition, _top)));
      _count++;
      WriterUnlock();
    }

    /// <summary>Recursively adds an item to the octree and returns the node where the addition was placed
    /// and adjusts the octree structure as needed.</summary>
    private OctreeLinkedLeaf Add(ValueType addition, OctreeLinkedNode octreeNode)
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
            growth = GrowBranch(parent, parent.DetermineChild(addition.Position.X, addition.Position.Y, addition.Position.Z));
          foreach (ValueType entry in leaf.Contents)
            _referenceDatabase.GetSecondGeneric(entry).Leaf = Add(entry, growth);
          return Add(addition, growth);
        }
      }
      // We are still traversing the tree, determine the next move
      else
      {
        OctreeLinkedBranch branch = (OctreeLinkedBranch)octreeNode;
        int child = branch.DetermineChild(addition.Position.X, addition.Position.Y, addition.Position.Z);
        // If the leaf is null, we must grow one before attempting to add to it
        if (branch.Children[child] == null)
          return GrowLeaf(branch, child).Add(addition);
        return Add(addition, branch.Children[child]);
      }
    }

    // Grows a branch on the tree at the desired location
    private OctreeLinkedBranch GrowBranch(OctreeLinkedBranch branch, int child)
    {
      OctreeLinkedBound childBounds = branch.DetermineChildBounds(child);
      branch.Children[child] =
        new OctreeLinkedBranch(childBounds.X, childBounds.Y, childBounds.Z, childBounds.Scale, branch);
      return (OctreeLinkedBranch)branch.Children[child];
    }

    // Grows a leaf on the tree at the desired location
    private OctreeLinkedLeaf GrowLeaf(OctreeLinkedBranch branch, int child)
    {
      if (branch.Children[child] != null)
        throw new OctreeLinkedException("My octree has a glitched, sorry.");
      OctreeLinkedBound childBounds = branch.DetermineChildBounds(child);
      branch.Children[child] =
        new OctreeLinkedLeaf(childBounds.X, childBounds.Y, childBounds.Z, childBounds.Scale, branch, _branchFactor);
      return (OctreeLinkedLeaf)branch.Children[child];
    }

    /// <summary>Removes an item from the octree by the id that was assigned to it.</summary>
    /// <param name="id">The string id of the removal that was given to the item when it was added.</param>
    public void Remove(KeyType key)
    {
      WriterLock();
      Remove(key, _referenceDatabase.Get(key).Leaf);
      _referenceDatabase.Remove(key);
      _count--;
      WriterUnlock();
    }

    private void Remove(KeyType key, OctreeLinkedLeaf leaf)
    {
      if (leaf.Count > 1)
      {
        ValueType[] contents = leaf.Contents;
        for (int i = 0; i < leaf.Count; i++)
          if (_keyComparisonFunction(contents[i], key) == 0)
          {
            ValueType temp = contents[_count - 1];
            contents[_count - 1] = contents[i];
            contents[i] = temp;
            break;
          }
      }
      else PluckLeaf(leaf.Parent, leaf.Parent.DetermineChild(leaf.X, leaf.Y, leaf.Z));
    }

    private void PluckLeaf(OctreeLinkedBranch branch, int child)
    {
      if (!(branch.Children[child] is OctreeLinkedLeaf) || ((OctreeLinkedLeaf)branch.Children[child]).Count > 1)
        throw new OctreeLinkedException("There is a glitch in my octree, sorry.");
      branch.Children[child] = null;
      while (branch.IsEmpty)
      {
        ChopBranch(branch.Parent, branch.Parent.DetermineChild(branch.X, branch.Y, branch.Z));
        branch = branch.Parent;
      }
    }

    private void ChopBranch(OctreeLinkedBranch branch, int child)
    {
      if (branch.Children[child] == null)
        throw new OctreeLinkedException("There is a glitch in my octree, sorry...");
      branch.Children[child] = null;
    }

    /// <summary>Moves an existing item from one position to another.</summary>
    /// <param name="key">The key of the item to be moved.</param>
    /// <param name="x">The x coordinate of the new position of the item.</param>
    /// <param name="y">The y coordinate of the new position of the item.</param>
    /// <param name="z">The z coordinate of the new position of the item.</param>
    public void Move(KeyType key, float x, float y, float z)
    {
      WriterLock();
      OctreeLinkedLeaf leaf = _referenceDatabase.Get(key).Leaf;
      ValueType entry = default(ValueType);
      bool found = false;
      foreach (ValueType value in leaf.Contents)
        if (_keyComparisonFunction(value, key) == 0)
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
      WriterUnlock();
    }

    /// <summary>Performs a functional paradigm traversal of the octree.</summary>
    /// <param name="traversalFunction"></param>
    public bool TraverseBreakable(Func<ValueType, bool> traversalFunction)
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
    private bool TraverseBreakable(Func<ValueType, bool> traversalFunctionBreakable, OctreeLinkedNode octreeNode)
    {
      if (octreeNode != null)
      {
        if (octreeNode is OctreeLinkedLeaf)
        {
          foreach (ValueType item in ((OctreeLinkedLeaf)octreeNode).Contents)
            if (!traversalFunctionBreakable(item)) return false;
        }
        else
        {
          // The current node is a branch
          OctreeLinkedBranch branch = (OctreeLinkedBranch)octreeNode;
          if (!TraverseBreakable(traversalFunctionBreakable, branch.Children[0])) return false;
          if (!TraverseBreakable(traversalFunctionBreakable, branch.Children[1])) return false;
          if (!TraverseBreakable(traversalFunctionBreakable, branch.Children[2])) return false;
          if (!TraverseBreakable(traversalFunctionBreakable, branch.Children[3])) return false;
          if (!TraverseBreakable(traversalFunctionBreakable, branch.Children[4])) return false;
          if (!TraverseBreakable(traversalFunctionBreakable, branch.Children[5])) return false;
          if (!TraverseBreakable(traversalFunctionBreakable, branch.Children[6])) return false;
          if (!TraverseBreakable(traversalFunctionBreakable, branch.Children[7])) return false;
        }
      }
      return true;
    }

    public void Traverse(Action<ValueType> traversalFunction)
    {
      ReaderLock();
      Traverse(traversalFunction, _top);
      ReaderUnlock();
    }
    private void Traverse(Action<ValueType> traversalFunction, OctreeLinkedNode octreeNode)
    {
      if (octreeNode != null)
      {
        if (octreeNode is OctreeLinkedLeaf)
        {
          foreach (ValueType item in ((OctreeLinkedLeaf)octreeNode).Contents)
            traversalFunction(item);
        }
        else
        {
          // The current node is a branch
          OctreeLinkedBranch branch = (OctreeLinkedBranch)octreeNode;
          Traverse(traversalFunction, branch.Children[0]);
          Traverse(traversalFunction, branch.Children[1]);
          Traverse(traversalFunction, branch.Children[2]);
          Traverse(traversalFunction, branch.Children[3]);
          Traverse(traversalFunction, branch.Children[4]);
          Traverse(traversalFunction, branch.Children[5]);
          Traverse(traversalFunction, branch.Children[6]);
          Traverse(traversalFunction, branch.Children[7]);
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
    public bool TraverseBreakable(Func<ValueType, bool> traversalFunction, float xMin, float yMin, float zMin, float xMax, float yMax, float zMax)
    {
      ReaderLock();
      bool returnValue = TraverseBreakable(traversalFunction, _top, xMin, yMin, zMin, xMax, yMax, zMax);
      ReaderUnlock();
      return returnValue;
    }
    private bool TraverseBreakable(Func<ValueType, bool> traversalFunction, OctreeLinkedNode octreeNode, float xMin, float yMin, float zMin, float xMax, float yMax, float zMax)
    {
      if (octreeNode != null)
      {
        if (octreeNode is OctreeLinkedLeaf)
        {
          foreach (ValueType entry in ((OctreeLinkedLeaf)octreeNode).Contents)
            //if (!traversalFunction(item)) return false;
            if (entry != null &&
            entry.Position.X > xMin && entry.Position.X < xMax
            && entry.Position.Y > yMin && entry.Position.Y < yMax
            && entry.Position.Z > zMin && entry.Position.Z < zMax)
              if (!traversalFunction(entry)) return false;
        }
        else
        {
          // The current node is a branch
          foreach (OctreeLinkedNode node in ((OctreeLinkedBranch)octreeNode).Children)
          {
            if (node == null) continue;
            else if (xMax < node.X - node.Scale) continue;
            else if (yMax < node.Y - node.Scale) continue;
            else if (zMax < node.Z - node.Scale) continue;
            else if (xMin > node.X + node.Scale) continue;
            else if (yMin > node.Y + node.Scale) continue;
            else if (zMin > node.Z + node.Scale) continue;
            if (!TraverseBreakable(traversalFunction, node, xMin, yMin, zMin, xMax, yMax, zMax)) return false;
          }
        }
      }
      return true;
    }

    public void Traverse(Action<ValueType> traversalFunction, float xMin, float yMin, float zMin, float xMax, float yMax, float zMax)
    {
      ReaderLock();
      Traverse(traversalFunction, _top, xMin, yMin, zMin, xMax, yMax, zMax);
      ReaderUnlock();
    }
    private void Traverse(Action<ValueType> traversalFunction, OctreeLinkedNode octreeNode, float xMin, float yMin, float zMin, float xMax, float yMax, float zMax)
    {
      if (octreeNode != null)
      {
        if (octreeNode is OctreeLinkedLeaf)
        {
          foreach (ValueType entry in ((OctreeLinkedLeaf)octreeNode).Contents)
            //if (!traversalFunction(item)) return false;
            if (entry != null &&
            entry.Position.X > xMin && entry.Position.X < xMax
            && entry.Position.Y > yMin && entry.Position.Y < yMax
            && entry.Position.Z > zMin && entry.Position.Z < zMax)
              traversalFunction(entry);
        }
        else
        {
          // The current node is a branch
          foreach (OctreeLinkedNode node in ((OctreeLinkedBranch)octreeNode).Children)
          {
            if (node == null) continue;
            else if (xMax < node.X - node.Scale) continue;
            else if (yMax < node.Y - node.Scale) continue;
            else if (zMax < node.Z - node.Scale) continue;
            else if (xMin > node.X + node.Scale) continue;
            else if (yMin > node.Y + node.Scale) continue;
            else if (zMin > node.Z + node.Scale) continue;
            Traverse(traversalFunction, node, xMin, yMin, zMin, xMax, yMax, zMax);
          }
        }
      }
    }

    public ValueType[] ToArray()
    {
      ReaderLock();
      int finalIndex;
      ValueType[] array = new ValueType[_count];
      ToArray(_top, array, 0, out finalIndex);
      if (array.Length != finalIndex)
        throw new OctreeLinkedException("There is a glitch in my octree, sorry...");
      ReaderUnlock();
      return array;
    }
    private void ToArray(OctreeLinkedNode octreeNode, ValueType[] array, int entryIndex, out int returnIndex)
    {
      if (octreeNode != null)
      {
        if (octreeNode is OctreeLinkedLeaf)
        {
          returnIndex = entryIndex;
          foreach (ValueType item in ((OctreeLinkedLeaf)octreeNode).Contents)
            array[returnIndex++] = item;
        }
        else
        {
          // The current node is a branch
          OctreeLinkedBranch branch = (OctreeLinkedBranch)octreeNode;
          ToArray(branch.Children[0], array, entryIndex, out entryIndex);
          ToArray(branch.Children[1], array, entryIndex, out entryIndex);
          ToArray(branch.Children[2], array, entryIndex, out entryIndex);
          ToArray(branch.Children[3], array, entryIndex, out entryIndex);
          ToArray(branch.Children[4], array, entryIndex, out entryIndex);
          ToArray(branch.Children[5], array, entryIndex, out entryIndex);
          ToArray(branch.Children[6], array, entryIndex, out entryIndex);
          ToArray(branch.Children[7], array, entryIndex, out entryIndex);
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