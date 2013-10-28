// SEVENENGINE LISCENSE:
// You are free to use, modify, and distribute any or all code segments/files for any purpose
// including commercial use with the following condition: any code using or originally taken from the 
// SevenEngine project must include citation to its original author(s) located at the top of each
// source code file. Alternatively, you may include a reference to the SevenEngine project as a whole,
// but you must include the current SevenEngine official website URL and logo.
// - Thanks.  :)  (support: seven@sevenengine.com)

// Author(s):
// - Zachary Aaron Patten (aka Seven) seven@sevenengine.com
// Last Edited: 10-26-13

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