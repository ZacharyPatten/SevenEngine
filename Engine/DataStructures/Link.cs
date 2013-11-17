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
// - Link2
// - Link3

namespace SevenEngine.DataStructures
{
  #region Link2

  /// <summary>Creates a link between two objects. (you may recognize this as a tuple)</summary>
  /// <typeparam name="LeftItem">The type of the left item to be linked.</typeparam>
  /// <typeparam name="RightItem">The type of the right item to be linked.</typeparam>
  public class Link2<LeftItem, RightItem>
  {
    private LeftItem _left;
    private RightItem _right;

    /// <summary>The left item in the link.</summary>
    /// <remarks>Runtime: O(1).</remarks>
    public LeftItem Left { get { return _left; } set { _left = value; } }
    /// <summary>The right item in the link.</summary>
    /// <remarks>Runtime: O(1).</remarks>
    public RightItem Right { get { return _right; } set { _right = value; } }

    /// <summary>Creates a link between two objects.</summary>
    /// <param name="left">The left item to be linked.</param>
    /// <param name="right">The right item to be linked.</param>
    /// <remarks>Runtime: O(1). Space: Theta(2).</remarks>
    public Link2(LeftItem left, RightItem right)
    {
      _left = left;
      _right = right;
    }
  }

  #endregion

  #region Link3

  /// <summary>Creates a link between two objects. (you may recognize this as a tuple)</summary>
  /// <typeparam name="LeftItem">The type of the left item to be linked.</typeparam>
  /// <typeparam name="MiddleItem">The type of the middle item to be linked.</typeparam>
  /// <typeparam name="RightItem">The type of the right item to be linked.</typeparam>
  public class Link3<LeftItem, MiddleItem, RightItem>
  {
    private LeftItem _left;
    private MiddleItem _middle;
    private RightItem _right;

    /// <summary>The left item in the link.</summary>
    /// <remarks>Runtime: O(1).</remarks>
    public LeftItem Left { get { return _left; } set { _left = value; } }
    /// <summary>The middle item of the link.</summary>
    /// <remarks>Runtime: O(1).</remarks>
    public MiddleItem Middle { get { return _middle; } set { _middle = value; } }
    /// <summary>The right item in the link.</summary>
    /// <remarks>Runtime: O(1).</remarks>
    public RightItem Right { get { return _right; } set { _right = value; } }

    /// <summary>Creates a link between two objects.</summary>
    /// <param name="left">The left item to be linked.</param>
    /// <param name="right">The right item to be linked.</param>
    /// <remarks>Runtime: O(1). Space: Theta(2).</remarks>
    public Link3(LeftItem left, MiddleItem middle, RightItem right)
    {
      _left = left;
      _middle = middle;
      _right = right;
    }
  }

  #endregion
}