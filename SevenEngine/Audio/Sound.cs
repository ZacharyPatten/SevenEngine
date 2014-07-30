// Seven
// https://github.com/53V3N1X/SevenEngine
// LISCENSE: See "LISCENSE.txt" in th root project directory.
// SUPPORT: See "README.txt" in the root project directory.

using Seven;
using Seven.Structures;

namespace SevenEngine.Audio
{
  // THE SOUND CLASSES ARE NOT YET FUNCTINAL, PLEASE WAIT FOR ME TO FINISH THEM...
  public class Sound
  {
    private string _id;

    public string Id { get { return _id; } set { _id = value; } }



    public int Channel { get; set; }

    // minus is an error state.
    public bool FailedToPlay { get { return (Channel == -1); } }

    public Sound(int channel) { Channel = channel; }

    public static Seven.Comparison CompareTo(Sound left, Sound right)
    {
      int comparison = left.Id.CompareTo(right.Id);
      if (comparison > 0)
        return Comparison.Greater;
      else if (comparison < 0)
        return Comparison.Less;
      else return Comparison.Equal;
    }

    public static Comparison CompareTo(Sound left, string right)
    {
      int comparison = left.Id.CompareTo(right);
      if (comparison > 0)
        return Comparison.Greater;
      else if (comparison < 0)
        return Comparison.Less;
      else return Comparison.Equal;
    }
  }
}