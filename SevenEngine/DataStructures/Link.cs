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

namespace SevenEngine.DataStructures
{
  #region Link2

  /// <summary>Represents a link between two objects.</summary>
  /// <typeparam name="FirstItem">The type of the left item to be linked.</typeparam>
  /// <typeparam name="SecondItem">The type of the right item to be linked.</typeparam>
  [Serializable]
  public class Link2<FirstItem, SecondItem>
  {
    private FirstItem _first;
    private SecondItem _second;

    /// <summary>The left item in the link.</summary>
    /// <remarks>Runtime: O(1).</remarks>
    public FirstItem First { get { return _first; } set { _first = value; } }
    /// <summary>The right item in the link.</summary>
    /// <remarks>Runtime: O(1).</remarks>
    public SecondItem Second { get { return _second; } set { _second = value; } }

    /// <summary>Creates a link between two objects.</summary>
    /// <param name="first">The first item to be linked.</param>
    /// <param name="second">The second item to be linked.</param>
    /// <remarks>Runtime: O(1).</remarks>
    public Link2(FirstItem first, SecondItem second)
    {
      _first = first;
      _second = second;
    }
  }

  #endregion

  #region Link3

  /// <summary>Represents a link between three objects.</summary>
  /// <typeparam name="FirstType">The type of the first item to be linked.</typeparam>
  /// <typeparam name="SecondType">The type of the second item to be linked.</typeparam>
  /// <typeparam name="ThirdType">The type of the third item to be linked.</typeparam>
  [Serializable]
  public class Link3<FirstType, SecondType, ThirdType>
  {
    private FirstType _first;
    private SecondType _second;
    private ThirdType _third;

    /// <summary>The first item in the link.</summary>
    /// <remarks>Runtime: O(1).</remarks>
    public FirstType First { get { return _first; } set { _first = value; } }
    /// <summary>The second item of the link.</summary>
    /// <remarks>Runtime: O(1).</remarks>
    public SecondType Second { get { return _second; } set { _second = value; } }
    /// <summary>The third item in the link.</summary>
    /// <remarks>Runtime: O(1).</remarks>
    public ThirdType Third { get { return _third; } set { _third = value; } }

    /// <summary>Creates a link between three objects.</summary>
    /// <param name="first">The first item to be linked.</param>
    /// <param name="second">The second item to be linked.</param>
    /// <param name="third">The third item to be linked.</param>
    /// <remarks>Runtime: O(1).</remarks>
    public Link3(FirstType first, SecondType second, ThirdType third)
    {
      _first = first;
      _second = second;
      _third = third;
    }
  }

  #endregion

  #region Link4

  /// <summary>Represents a link between four objects.</summary>
  /// <typeparam name="FirstType">The type of the first item to be linked.</typeparam>
  /// <typeparam name="SecondType">The type of the second item to be linked.</typeparam>
  /// <typeparam name="ThirdType">The type of the third item to be linked.</typeparam>
  /// <typeparam name="FourthType">The type of the fourth item to be linked.</typeparam>
  [Serializable]
  public class Link4<FirstType, SecondType, ThirdType, FourthType>
  {
    private FirstType _first;
    private SecondType _second;
    private ThirdType _third;
    private FourthType _fourth;

    /// <summary>The first item in the link.</summary>
    /// <remarks>Runtime: O(1).</remarks>
    public FirstType First { get { return _first; } set { _first = value; } }
    /// <summary>The second item of the link.</summary>
    /// <remarks>Runtime: O(1).</remarks>
    public SecondType Second { get { return _second; } set { _second = value; } }
    /// <summary>The third item in the link.</summary>
    /// <remarks>Runtime: O(1).</remarks>
    public ThirdType Third { get { return _third; } set { _third = value; } }
    /// <summary>The fourth item in the link.</summary>
    /// <remarks>Runtime: O(1).</remarks>
    public FourthType Fourth { get { return _fourth; } set { _fourth = value; } }

    /// <summary>Creates a link between four objects.</summary>
    /// <param name="first">The left item to be linked.</param>
    /// <param name="second">The second item to be linked.</param>
    /// <param name="third">The third item to be linked.</param>
    /// <param name="fourth">The fourth item to be linked.</param>
    /// <remarks>Runtime: O(1).</remarks>
    public Link4(FirstType first, SecondType second, ThirdType third, FourthType fourth)
    {
      _first = first;
      _second = second;
      _third = third;
      _fourth = fourth;
    }
  }

  #endregion

  #region Link5

  /// <summary>Represents a link between five objects.</summary>
  /// <typeparam name="FirstType">The type of the first item to be linked.</typeparam>
  /// <typeparam name="SecondType">The type of the second item to be linked.</typeparam>
  /// <typeparam name="ThirdType">The type of the third item to be linked.</typeparam>
  /// <typeparam name="FourthType">The type of the fourth item to be linked.</typeparam>
  /// <typeparam name="FifthType">The type of the fifth item to be linked.</typeparam>
  [Serializable]
  public class Link5<FirstType, SecondType, ThirdType, FourthType, FifthType>
  {
    private FirstType _first;
    private SecondType _second;
    private ThirdType _third;
    private FourthType _fourth;
    private FifthType _fifth;

    /// <summary>The first item in the link.</summary>
    /// <remarks>Runtime: O(1).</remarks>
    public FirstType First { get { return _first; } set { _first = value; } }
    /// <summary>The second item of the link.</summary>
    /// <remarks>Runtime: O(1).</remarks>
    public SecondType Second { get { return _second; } set { _second = value; } }
    /// <summary>The third item in the link.</summary>
    /// <remarks>Runtime: O(1).</remarks>
    public ThirdType Third { get { return _third; } set { _third = value; } }
    /// <summary>The fourth item in the link.</summary>
    /// <remarks>Runtime: O(1).</remarks>
    public FourthType Fourth { get { return _fourth; } set { _fourth = value; } }
    /// <summary>The fifth item in the link.</summary>
    /// <remarks>Runtime: O(1).</remarks>
    public FifthType Fifth { get { return _fifth; } set { _fifth = value; } }

    /// <summary>Creates a link between four objects.</summary>
    /// <param name="first">The left item to be linked.</param>
    /// <param name="second">The second item to be linked.</param>
    /// <param name="third">The third item to be linked.</param>
    /// <param name="fourth">The fourth item to be linked.</param>
    /// <remarks>Runtime: O(1).</remarks>
    public Link5(FirstType first, SecondType second, ThirdType third, FourthType fourth, FifthType fifth)
    {
      _first = first;
      _second = second;
      _third = third;
      _fourth = fourth;
      _fifth = fifth;
    }
  }

  #endregion
}