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

// This file contains the following classes:
// - Octree
//   - OctreeBound
//   - OctreeNode
//   - OctreeLeaf
//   - OctreeBranch
//   - OctreeException

using System;
using System.Threading;
using SevenEngine.DataStructures.Interfaces;

namespace SevenEngine.DataStructures
{
  #region Octree

  public class Octree<ValueType, KeyType> : InterfaceTraversable<ValueType>
    where ValueType : InterfaceStringId, InterfacePositionVector
  {
    #region OctreeBound

    /// <summary>Represents a bounding cube. Includes coordinates of the center 
    /// and a scale of expansion along each axis.</summary>
    private class OctreeBound
    {
      private float _x, _y, _z, _scale;

      internal float X { get { return _x; } }
      internal float Y { get { return _y; } }
      internal float Z { get { return _z; } }
      internal float Scale { get { return _scale; } }

      internal OctreeBound(float x, float y, float z, float scale)
      { _x = x; _y = y; _z = z; _scale = scale; }
    }

    #endregion

    #region OctreeNode

    /// <summary>Represents a single node of the octree. Includes references both upwards and
    /// downwards the tree.</summary>
    private abstract class OctreeNode
    {
      private float _x, _y, _z, _scale;
      private OctreeBranch _parent;

      internal float X { get { return _x; } }
      internal float Y { get { return _y; } }
      internal float Z { get { return _z; } }
      internal float Scale { get { return _scale; } }
      internal OctreeBranch Parent { get { return _parent; } }

      internal OctreeNode(float x, float y, float z, float scale, OctreeBranch parent)
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
      internal OctreeBound DetermineChildBounds(int child)
      {
        float halfScale = _scale / 2;
        switch (child)
        {
          case 0:
            return new OctreeBound(_x - halfScale, _y - halfScale, _z - halfScale, halfScale);
          case 1:
            return new OctreeBound(_x - halfScale, _y - halfScale, _z + halfScale, halfScale);
          case 2:
            return new OctreeBound(_x - halfScale, _y + halfScale, _z - halfScale, halfScale);
          case 3:
            return new OctreeBound(_x - halfScale, _y + halfScale, _z + halfScale, halfScale);
          case 4:
            return new OctreeBound(_x + halfScale, _y - halfScale, _z - halfScale, halfScale);
          case 5:
            return new OctreeBound(_x + halfScale, _y - halfScale, _z + halfScale, halfScale);
          case 6:
            return new OctreeBound(_x + halfScale, _y + halfScale, _z - halfScale, halfScale);
          case 7:
            return new OctreeBound(_x + halfScale, _y + halfScale, _z + halfScale, halfScale);
        }
        throw new OctreeException("There is a glitch in my octree, sorry...");
      }
    }

    #endregion
    
    #region OctreeLeaf

    /// <summary>Represents a single node of the octree. Includes references both upwards and
    /// downwards the tree.</summary>
    private class OctreeLeaf : OctreeNode
    {
      //private OctreeEntry[] _contents;
      private ValueType[] _contents;
      private int _count;

      //internal OctreeEntry[] Contents { get { return _contents; } }
      internal ValueType[] Contents { get { return _contents; } }
      internal int Count { get { return _count; } set { _count = value; } }
      internal bool IsFull { get { return _count == _contents.Length; } }

      internal OctreeLeaf(float x, float y, float z, float scale, OctreeBranch parent, int branchFactor)
        : base(x, y, z, scale, parent)
      { _contents = new ValueType[branchFactor]; }

      internal int GetIndex(string id)
      {
        for (int i = 0; i < _count; i++)
          if (_contents[i].Id == id)
            return i;
        throw new OctreeException("There is a glitch in my octree, sorry...");
      }

      internal ValueType GetEntry(string id)
      {
        for (int i = 0; i < _count; i++)
          if (_contents[i].Id == id)
            return _contents[i];
        throw new OctreeException("There is a glitch in my octree, sorry...");
      }

      internal OctreeLeaf Add(ValueType addition)
      {
        if (_count == _contents.Length)
          throw new OctreeException("There is a glitch in my octree, sorry...");
        _contents[_count++] = addition;
        return this;
      }

      internal void Remove(string id)
      {
        for (int i = 0; i < _count; i++)
          if (_contents[i].Id == id)
          {
            ValueType swapStorage = _contents[_count - 1];
            _contents[_count - 1] = _contents[i];
            _contents[i] = swapStorage;
            return;
          }
        throw new OctreeException("My octree has a glitch, sorry...");
      }
    }

    #endregion

    #region OctreeBranch

    /// <summary>Represents a single node of the octree. Includes references both upwards and
    /// downwards the tree.</summary>
    private class OctreeBranch : OctreeNode
    {
      // The children are indexed as follows (relative to this node's center):
      // 0: (-x, -y, -z)   1: (-x, -y, z)   2: (-x, y, -z)   3: (-x, y, z)
      // 4: (x, -y, -z)   5: (x, -y, z)   6: (x, y, -z)   7: (x, y, z)
      private OctreeNode[] _children;

      internal OctreeNode[] Children { get { return _children; } }
      internal bool IsEmpty
      {
        get
        {
          return _children[0] == null && _children[1] == null && _children[2] == null
            && _children[3] == null && _children[4] == null && _children[5] == null
            && _children[6] == null && _children[7] == null;
        }
      }

      internal OctreeBranch(float x, float y, float z, float scale, OctreeBranch parent)
        : base(x, y, z, scale, parent)
      { _children = new OctreeNode[8]; }
    }

    #endregion

    #region OctreeReference

    private class OctreeReference : SevenEngine.DataStructures.Interfaces.InterfaceStringId
    {
      private ValueType _value;
      private OctreeLeaf _leaf;

      public string Id { get { return _value.Id; } set { _value.Id = value; } }
      internal ValueType Value { get { return _value; } set { _value = value; } }
      internal OctreeLeaf Leaf { get { return _leaf; } set { _leaf = value; } }

      internal OctreeReference(ValueType value, OctreeLeaf leaf) { _value = value; _leaf = leaf; }
    }

    #endregion

    // The maximum number of objects per leaf (branch factor)
    private int _branchFactor;
    private int _count;
    // A database of objects and their current octree nodes
    private AvlTree<OctreeReference, string> _referenceDatabase;
    // The top node of the tree
    private OctreeNode _top;

    private bool _modified;

    private Object _lock;
    private int _readers;
    private int _writers;

    /// <summary>Creates an octree for three dimensional space partitioning.</summary>
    /// <param name="x">The x coordinate of the center of the octree.</param>
    /// <param name="y">The y coordinate of the center of the octree.</param>
    /// <param name="z">The z coordinate of the center of the octree.</param>
    /// <param name="scale">How far the tree expands along each dimension.</param>
    /// <param name="branchFactor">The maximum items per octree node before expansion.</param>
    public Octree(float x, float y, float z, float scale, int branchFactor,
      Func<ValueType, ValueType, int> valueComparisonFunction,
      Func<ValueType, KeyType, int> keyComparisonFunction)
    {
      _branchFactor = branchFactor;
      _top = new OctreeLeaf(x, y, z, scale, null, _branchFactor);
      _count = 0;
      _lock = new Object();
      _readers = 0;
      _writers = 0;

      _referenceDatabase = new AvlTree<OctreeReference, string>
      (
        (OctreeReference left, OctreeReference right) => { return left.Id.CompareTo(right.Id); },
        (OctreeReference left, string right) => { return left.Id.CompareTo(right); }
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
      _referenceDatabase.Add(new OctreeReference(addition, Add(addition, _top)));
      _count++;
      _modified = true;
      WriterUnlock();
    }

    /// <summary>Recursively adds an item to the octree and returns the node where the addition was placed
    /// and adjusts the octree structure as needed.</summary>
    private OctreeLeaf Add(ValueType addition, OctreeNode octreeNode)
    {
      // If the node is a leaf we have reached the bottom of the tree
      if (octreeNode is OctreeLeaf)
      {
        OctreeLeaf leaf = (OctreeLeaf)octreeNode;
        if (!leaf.IsFull)
        {
          // We found a proper leaf, and the leaf has room, just add it
          leaf.Add(addition);
          return leaf;
        }
        else
        {
          // The leaf is full so we need to grow out the tree
          OctreeBranch parent = octreeNode.Parent;
          OctreeBranch growth;
          if (parent == null)
            growth = (OctreeBranch)(_top = new OctreeBranch(_top.X, _top.Y, _top.Z, _top.Scale, null));
          else
            growth = GrowBranch(parent, parent.DetermineChild(addition.Position.X, addition.Position.Y, addition.Position.Z));
          foreach (ValueType entry in leaf.Contents)
            _referenceDatabase.Get(entry.Id).Leaf = Add(entry, growth);
          return Add(addition, growth);
        }
      }
      // We are still traversing the tree, determine the next move
      else
      {
        OctreeBranch branch = (OctreeBranch)octreeNode;
        int child = branch.DetermineChild(addition.Position.X, addition.Position.Y, addition.Position.Z);
        // If the leaf is null, we must grow one before attempting to add to it
        if (branch.Children[child] == null)
          return GrowLeaf(branch, child).Add(addition);
        return Add(addition, branch.Children[child]);
      }
    }

    // Grows a branch on the tree at the desired location
    private OctreeBranch GrowBranch(OctreeBranch branch, int child)
    {
      OctreeBound childBounds = branch.DetermineChildBounds(child);
      branch.Children[child] =
        new OctreeBranch(childBounds.X, childBounds.Y, childBounds.Z, childBounds.Scale, branch);
      return (OctreeBranch)branch.Children[child];
    }

    // Grows a leaf on the tree at the desired location
    private OctreeLeaf GrowLeaf(OctreeBranch branch, int child)
    {
      if (branch.Children[child] != null)
        throw new OctreeException("My octree has a glitched, sorry.");
      OctreeBound childBounds = branch.DetermineChildBounds(child);
      branch.Children[child] =
        new OctreeLeaf(childBounds.X, childBounds.Y, childBounds.Z, childBounds.Scale, branch, _branchFactor);
      return (OctreeLeaf)branch.Children[child];
    }

    /// <summary>Removes an item from the octree by the id that was assigned to it.</summary>
    /// <param name="id">The string id of the removal that was given to the item when it was added.</param>
    public void Remove(string id)
    {
      WriterLock();
      Remove(id, _referenceDatabase.Get(id).Leaf);
      _referenceDatabase.Remove(id);
      _count--;
      _modified = false;
      WriterUnlock();
    }

    private void Remove(string id, OctreeLeaf leaf)
    {
      if (leaf.Count > 1) leaf.Remove(id);
      else PluckLeaf(leaf.Parent, leaf.Parent.DetermineChild(leaf.X, leaf.Y, leaf.Z));
    }

    private void PluckLeaf(OctreeBranch branch, int child)
    {
      if (!(branch.Children[child] is OctreeLeaf) || ((OctreeLeaf)branch.Children[child]).Count > 1)
        throw new OctreeException("There is a glitch in my octree, sorry.");
      branch.Children[child] = null;
      while (branch.IsEmpty)
      {
        ChopBranch(branch.Parent, branch.Parent.DetermineChild(branch.X, branch.Y, branch.Z));
        branch = branch.Parent;
      }
    }

    private void ChopBranch(OctreeBranch branch, int child)
    {
      if (branch.Children[child] == null)
        throw new OctreeException("There is a glitch in my octree, sorry...");
      branch.Children[child] = null;
    }

    /// <summary>Moves an existing item from one position to another.</summary>
    /// <param name="id">The string id of the item to be moved.</param>
    /// <param name="x">The x coordinate of the new position of the item.</param>
    /// <param name="y">The y coordinate of the new position of the item.</param>
    /// <param name="z">The z coordinate of the new position of the item.</param>
    public void Move(string id, float x, float y, float z)
    {
      WriterLock();
      OctreeLeaf leaf = _referenceDatabase.Get(id).Leaf;
      ValueType entry = leaf.GetEntry(id);
      entry.Position.X = x;
      entry.Position.Y = y;
      entry.Position.Z = z;
      if ((x > leaf.X - leaf.Scale && x < leaf.X + leaf.Scale)
        && (y > leaf.Y - leaf.Scale && y < leaf.Y + leaf.Scale)
        && (z > leaf.Z - leaf.Scale && z < leaf.Z + leaf.Scale))
        return;
      Remove(id, leaf);
      Add(entry, _top);
      WriterLock();
    }

    /// <summary>Iterates through the entire tree and ensures each item is in the proper node.</summary>
    public void Update()
    {
      WriterLock();
      WriterUnlock();
      throw new NotImplementedException("Sorry, I'm still working on the update function.");
      _modified = false;
      WriterUnlock();
    }

    /// <summary>Performs a functional paradigm traversal of the octree.</summary>
    /// <param name="traversalFunction"></param>
    public bool Traversal(Func<ValueType, bool> traversalFunction)
    {
      ReaderLock();
      if (!Traversal(traversalFunction, _top))
      {
        ReaderUnlock();
        return false;
      }
      ReaderUnlock();
      return true;
    }
    private bool Traversal(Func<ValueType, bool> traversalFunction, OctreeNode octreeNode)
    {
      if (octreeNode != null)
      {
        if (octreeNode is OctreeLeaf)
        {
          foreach (ValueType item in ((OctreeLeaf)octreeNode).Contents)
            if (!traversalFunction(item)) return false;
        }
        else
        {
          // The current node is a branch
          OctreeBranch branch = (OctreeBranch)octreeNode;
          if (!Traversal(traversalFunction, branch.Children[0])) return false;
          if (!Traversal(traversalFunction, branch.Children[1])) return false;
          if (!Traversal(traversalFunction, branch.Children[2])) return false;
          if (!Traversal(traversalFunction, branch.Children[3])) return false;
          if (!Traversal(traversalFunction, branch.Children[4])) return false;
          if (!Traversal(traversalFunction, branch.Children[5])) return false;
          if (!Traversal(traversalFunction, branch.Children[6])) return false;
          if (!Traversal(traversalFunction, branch.Children[7])) return false;
        }
      }
      return true;
    }

    /// <summary>Performs a functional paradigm traversal of the octree with data structure optimization.</summary>
    /// <param name="traversalFunction">The function to perform per iteration.</param>
    /// <param name="xMin">The minimum x of a rectangular prism to query the octree.</param>
    /// <param name="yMin">The minimum y of a rectangular prism to query the octree.</param>
    /// <param name="zMin">The minimum z of a rectangular prism to query the octree.</param>
    /// <param name="xMax">The maximum x of a rectangular prism to query the octree.</param>
    /// <param name="yMax">The maximum y of a rectangular prism to query the octree.</param>
    /// <param name="zMax">The maximum z of a rectangular prism to query the octree.</param>
    public void Traversal(Func<ValueType, bool> traversalFunction, float xMin, float yMin, float zMin, float xMax, float yMax, float zMax)
    {
      ReaderLock();
      Traversal(traversalFunction, _top, xMin, yMin, zMin, xMax, yMax, zMax);
      ReaderUnlock();
    }
    private bool Traversal(Func<ValueType, bool> traversalFunction, OctreeNode octreeNode, float xMin, float yMin, float zMin, float xMax, float yMax, float zMax)
    {
      if (octreeNode != null)
      {
        if (octreeNode is OctreeLeaf)
        {
          foreach (ValueType entry in ((OctreeLeaf)octreeNode).Contents)
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
          foreach (OctreeNode node in ((OctreeBranch)octreeNode).Children)
          {
            if (node == null) continue;
            else if (xMax < node.X - node.Scale) continue;
            else if (yMax < node.Y - node.Scale) continue;
            else if (zMax < node.Z - node.Scale) continue;
            else if (xMin > node.X + node.Scale) continue;
            else if (yMin > node.Y + node.Scale) continue;
            else if (zMin > node.Z + node.Scale) continue;
            if (!Traversal(traversalFunction, node, xMin, yMin, zMin, xMax, yMax, zMax)) return false;
          }
        }
      }
      return true;
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
    private class OctreeException : Exception { public OctreeException(string message) : base(message) { } }
  }

  #endregion
}