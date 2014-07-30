// Seven
// https://github.com/53V3N1X/SevenEngine
// LISCENSE: See "LISCENSE.txt" in th root project directory.
// SUPPORT: See "README.txt" in the root project directory.

using Seven;
using SevenEngine.Imaging;
using Seven.Structures;

namespace SevenEngine.Texts
{
  public class Font
  {
    private string _id;
    private int _lineHeight;
    private int _base;
    private List<CharacterSprite> _characterDatum;
    private int _existingHardwareInstances;

    public string Id { get { return _id; } set { _id = value; } }
    public int ExistingHardwareInstances { get { return _existingHardwareInstances; } }
    internal int LineHeight { get { return _lineHeight; } }
    internal int Base { get { return _base; } }

    internal Font(string id, int lineHeight, int fontBase, List<CharacterSprite> characters)
    {
      _id = id;
      _lineHeight = lineHeight;
      _base = fontBase;
      _characterDatum = characters;
    }

    internal CharacterSprite Get(int id)
    {
      CharacterSprite lookup = null;
      _characterDatum.Foreach(
        (CharacterSprite i) =>
        {
          if (i.Id == id)
          {
            lookup = i;
            return ForeachStatus.Break;
          }
          return ForeachStatus.Continue;
        });
      if (lookup != null)
        return lookup;
      throw new System.Exception("Character not found");
    }

    public static Comparison CompareTo(Font left, Font right)
    {
      int comparison = left.Id.CompareTo(right.Id);
      if (comparison > 0)
        return Comparison.Greater;
      else if (comparison < 0)
        return Comparison.Less;
      else
        return Comparison.Equal;
    }

    public static Comparison CompareTo(Font left, string right)
    { 
      int comparison = left.Id.CompareTo(right);
      if (comparison > 0)
        return Comparison.Greater;
      else if (comparison < 0)
        return Comparison.Less;
      else
        return Comparison.Equal;
    }
  }
}