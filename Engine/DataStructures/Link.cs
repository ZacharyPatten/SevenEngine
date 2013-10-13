// This file contains the following classes:
// - Link

// This file contains runtime values.
// All runtimes are in O-Notation. Here is a breif explanation:
// - "O(x)": the member has an upper bound of runtime equation "x"
// - "Omega(x)": the member has a lower bound of runtime equation "x"
// - "Theta(x)": the member has an upper and lower bound of runtime equation "x"
// - "EstAvg(x)": the runtime equation "x" to typically expect
//   (THIS IS MY PERSONAL ESTIMATION, AND CONSIDERING I WROTE THE CODE YOU SHOULD PROBABLY TRUST ME)
// Note that if the letter "n" is used, it typically means the current number of items within the set.

// Written by Seven (Zachary Aaron Patten)
// Last Edited on date 10-12-13
// Feel free to use this code in any manor you see fit.
// However, please site me because I put quite a bit of time into it.
// Special thanks to Rodney Howell, my previous data structures professor. 
// - Thanks. :)

namespace Engine.DataStructures
{
  /// <summary>Creates a link between two objects. (you may recognize this as a tuple)</summary>
  /// <typeparam name="LeftItem">The type of the left item to be linked.</typeparam>
  /// <typeparam name="RightItem">The type of the right item to be linked.</typeparam>
  public class Link<LeftItem, RightItem>
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
    /// <remarks>Runtime: O(1).</remarks>
    public Link(LeftItem left, RightItem right)
    {
      _left = left;
      _right = right;
    }
  }
}