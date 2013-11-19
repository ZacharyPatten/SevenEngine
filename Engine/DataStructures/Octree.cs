// SEVENENGINE LISCENSE:
// You are free to use, modify, and distribute any or all code segments/files for any purpose
// including commercial use with the following condition: any code using or originally taken 
// from the SevenEngine project must include citation to its original author(s) located at the
// top of each source code file, or you may include a reference to the SevenEngine project as
// a whole but you must include the current SevenEngine official website URL and logo.
// - Thanks.  :)  (support: seven@sevenengine.com)

// Author(s):
// - Zachary Aaron Patten (aka Seven) seven@sevenengine.com
// Last Edited: 11-16-13

// This file contains the following classes:
// - Octree
//   - OctreeBound
//   - OctreeNode
//   - OctreeLeaf
//   - OctreeBranch
//   - OctreeException

using System;

namespace SevenEngine.DataStructures
{
  #region Octree

  public class Octree<Type>
    where Type :
      SevenEngine.DataStructures.Interfaces.InterfaceStringId,
      SevenEngine.DataStructures.Interfaces.InterfacePositionVector
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
      private Type[] _contents;
      private int _count;

      //internal OctreeEntry[] Contents { get { return _contents; } }
      internal Type[] Contents { get { return _contents; } }
      internal int Count { get { return _count; } set { _count = value; } }
      internal bool IsFull { get { return _count == _contents.Length; } }

      internal OctreeLeaf(float x, float y, float z, float scale, OctreeBranch parent, int branchFactor)
        : base(x, y, z, scale, parent)
      { _contents = new Type[branchFactor]; }

      internal int GetIndex(string id)
      {
        for (int i = 0; i < _count; i++)
          if (_contents[i].Id == id)
            return i;
        throw new OctreeException("There is a glitch in my octree, sorry...");
      }

      internal Type GetEntry(string id)
      {
        for (int i = 0; i < _count; i++)
          if (_contents[i].Id == id)
            return _contents[i];
        throw new OctreeException("There is a glitch in my octree, sorry...");
      }

      internal OctreeLeaf Add(Type addition)
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
            Type swapStorage = _contents[_count - 1];
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
      private Type _value;
      private OctreeLeaf _leaf;

      public string Id { get { return _value.Id; } set { _value.Id = value; } }
      internal Type Value { get { return _value; } set { _value = value; } }
      internal OctreeLeaf Leaf { get { return _leaf; } set { _leaf = value; } }

      internal OctreeReference(Type value, OctreeLeaf leaf) { _value = value; _leaf = leaf; }
    }

    #endregion

    // The maximum number of objects per leaf (branch factor)
    private int _branchFactor;
    private int _count;
    // A database of objects and their current octree nodes
    private AvlTree<OctreeReference> _referenceDatabase;
    // The top node of the tree
    private OctreeNode _top;

    /// <summary>Creates an octree for three dimensional space partitioning.</summary>
    /// <param name="x">The x coordinate of the center of the octree.</param>
    /// <param name="y">The y coordinate of the center of the octree.</param>
    /// <param name="z">The z coordinate of the center of the octree.</param>
    /// <param name="scale">How far the tree expands along each dimension.</param>
    /// <param name="branchFactor">The maximum items per octree node before expansion.</param>
    public Octree(float x, float y, float z, float scale, int branchFactor)
    {
      _branchFactor = branchFactor;
      _top = new OctreeLeaf(x, y, z, scale, null, _branchFactor);
      _referenceDatabase = new AvlTree<OctreeReference>();
      _count = 0;
    }

    /// <summary>Adds an item to the Octree.</summary>
    /// <param name="id">The id associated with the addition.</param>
    /// <param name="addition">The addition.</param>
    /// <param name="x">The x coordinate of the addition's location.</param>
    /// <param name="y">The y coordinate of the addition's location.</param>
    /// <param name="z">The z coordinate of the addition's location.</param>
    public void Add(Type addition)
    {
      _referenceDatabase.Add(new OctreeReference(addition, Add(addition, _top)));
      _count++;
    }

    /// <summary>Recursively adds an item to the octree and returns the node where the addition was placed
    /// and adjusts the octree structure as needed.</summary>
    private OctreeLeaf Add(Type addition, OctreeNode octreeNode)
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
          foreach (Type entry in leaf.Contents)
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
      Remove(id, _referenceDatabase.Get(id).Leaf);
      _referenceDatabase.Remove(id);
      _count--;
    }

    /// <summary>Recursively removes an item from the octree and adjusts the octree structure as needed.</summary>
    private void Remove(string id, OctreeLeaf leaf)
    {
      if (leaf.Count > 1)
        leaf.Remove(id);
      else
        PluckLeaf(leaf.Parent, leaf.Parent.DetermineChild(leaf.X, leaf.Y, leaf.Z));
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
      OctreeLeaf leaf = _referenceDatabase.Get(id).Leaf;
      Type entry = leaf.GetEntry(id);
      entry.Position.X = x;
      entry.Position.Y = y;
      entry.Position.Z = z;
      if ((x > leaf.X - leaf.Scale && x < leaf.X + leaf.Scale)
        && (y > leaf.Y - leaf.Scale && y < leaf.Y + leaf.Scale)
        && (z > leaf.Z - leaf.Scale && z < leaf.Z + leaf.Scale))
        return;
      else
      {
        Remove(id, leaf);
        Add(entry, _top);
      }
    }

    /// <summary>Gets contents within the octree within a specific axis-aligned bounding rectanglular prism.</summary>
    /// <param name="xMin">The minimum x coordinate of the bounding rectangle.</param>
    /// <param name="yMin">The minimum y coordinate of the bounding rectangle.</param>
    /// <param name="zMin">The minimum z coordinate of the bounding rectangle.</param>
    /// <param name="xMax">The maximum x coordinate of the bounding rectangle.</param>
    /// <param name="yMax">The maximum y coordinate of the bounding rectangle.</param>
    /// <param name="zMax">The maximum z coordinate of the bounding rectangle.</param>
    /// <returns>A list of the contents within the provided bounding box.</returns>
    public List<Type> GetList(float xMin, float yMin, float zMin, float xMax, float yMax, float zMax)
    {
      List<Type> contents = new List<Type>();
      GetList(xMin, yMin, zMin, xMax, yMax, zMax, contents, _top);
      return contents;
    }

    /// <summary>Recursively finds items within a given bounding cube and adds it to the octree.</summary>
    /// <param name="bounds"></param>
    /// <param name="contents"></param>
    /// <param name="octreeNode"></param>
    private void GetList(float xMin, float yMin, float zMin, float xMax, float yMax, float zMax, List<Type> contents, OctreeNode octreeNode)
    {
      if (octreeNode is OctreeLeaf)
      {
        OctreeLeaf leaf = (OctreeLeaf)octreeNode;
        foreach (Type entry in leaf.Contents)
          if (entry != null &&
            entry.Position.X > xMin && entry.Position.X < xMax
            && entry.Position.Y > yMin && entry.Position.Y < yMax
            && entry.Position.Z > zMin && entry.Position.Z < zMax)
            contents.Add(entry);
        return;
      }
      else
      {
        OctreeBranch branch = (OctreeBranch)octreeNode;
        foreach (OctreeNode node in branch.Children)
        {
          if (node == null)
            continue;
          else if (xMax < node.X - node.Scale)
            continue;
          else if (yMax < node.Y - node.Scale)
            continue;
          else if (zMax < node.Z - node.Scale)
            continue;
          else if (xMin > node.X + node.Scale)
            continue;
          else if (yMin > node.Y + node.Scale)
            continue;
          else if (zMin > node.Z + node.Scale)
            continue;
          else
            GetList(xMin, yMin, zMin, xMax, yMax, zMax, contents, node);
        }
      }
    }

    /// <summary>Gets contents within the octree within a specific axis-aligned bounding rectanglular prism.</summary>
    /// <param name="xMin">The minimum x coordinate of the bounding rectangle.</param>
    /// <param name="yMin">The minimum y coordinate of the bounding rectangle.</param>
    /// <param name="zMin">The minimum z coordinate of the bounding rectangle.</param>
    /// <param name="xMax">The maximum x coordinate of the bounding rectangle.</param>
    /// <param name="yMax">The maximum y coordinate of the bounding rectangle.</param>
    /// <param name="zMax">The maximum z coordinate of the bounding rectangle.</param>
    /// <returns>A list of the contents within the provided bounding box.</returns>
    public ListArray<Type> GetListArray(float xMin, float yMin, float zMin, float xMax, float yMax, float zMax)
    {
      ListArray<Type> contents = new ListArray<Type>(1);
      GetListArray(xMin, yMin, zMin, xMax, yMax, zMax, contents, _top);
      return contents;
    }

    /// <summary>Recursively finds items within a given bounding cube and adds it to the octree.</summary>
    /// <param name="bounds"></param>
    /// <param name="contents"></param>
    /// <param name="octreeNode"></param>
    private void GetListArray(float xMin, float yMin, float zMin, float xMax, float yMax, float zMax, ListArray<Type> contents, OctreeNode octreeNode)
    {
      if (octreeNode is OctreeLeaf)
      {
        OctreeLeaf leaf = (OctreeLeaf)octreeNode;
        foreach (Type entry in leaf.Contents)
          if (entry != null &&
            entry.Position.X > xMin && entry.Position.X < xMax
            && entry.Position.Y > yMin && entry.Position.Y < yMax
            && entry.Position.Z > zMin && entry.Position.Z < zMax)
            contents.Add(entry);
        return;
      }
      else
      {
        OctreeBranch branch = (OctreeBranch)octreeNode;
        foreach (OctreeNode node in branch.Children)
        {
          if (node == null)
            continue;
          else if (xMax < node.X - node.Scale)
            continue;
          else if (yMax < node.Y - node.Scale)
            continue;
          else if (zMax < node.Z - node.Scale)
            continue;
          else if (xMin > node.X + node.Scale)
            continue;
          else if (yMin > node.Y + node.Scale)
            continue;
          else if (zMin > node.Z + node.Scale)
            continue;
          else
            GetListArray(xMin, yMin, zMin, xMax, yMax, zMax, contents, node);
        }
      }
    }

    /// <summary>This is used for throwing OcTree exceptions only to make debugging faster.</summary>
    private class OctreeException : Exception { public OctreeException(string message) : base(message) { } }
  }

  #endregion
}