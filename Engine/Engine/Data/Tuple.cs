using System.Collections.Generic;

namespace Engine.Data
{
  public class Tuple<LeftItem, RightItem>
  {
    private LeftItem _left;
    private RightItem _right;

    public LeftItem Left
    {
      get { return _left; }
      set { _left = value; }
    }

    public RightItem Right
    {
      get { return _right; }
      set { _right = value; }
    }

    public Tuple(LeftItem left, RightItem right)
    {
      _left = left;
      _right = right;
    }
  }
}
