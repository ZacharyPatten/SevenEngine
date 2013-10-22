using System;

namespace SevenEngine.Audio
{
  public class Sound
  {
    public int Channel { get; set; }

    public bool FailedToPlay
    {
      get
      {
        // minus is an error state.
        return (Channel == -1);
      }
    }

    public Sound(int channel)
    {
      Channel = channel;
    }
  }
}