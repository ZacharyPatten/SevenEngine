using System;

namespace SevenEngine.Audio
{
  // THE SOUND CLASSES ARE NOT YET FUNCTINAL, PLEASE WAIT FOR ME TO FINISH THEM...
  public class Sound
  {
    public int Channel { get; set; }

    // minus is an error state.
    public bool FailedToPlay { get { return (Channel == -1); } }

    public Sound(int channel) { Channel = channel; }
  }
}