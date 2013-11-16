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

using System;
using System.IO;
using OpenTK;
using OpenTK.Audio;
using OpenTK.Audio.OpenAL;
using SevenEngine.Audio;
using SevenEngine.DataStructures;

namespace SevenEngine
{
  /// <summary>SoundManager is used for audio management (loading, storing, hardware instance controling, and disposing).</summary>
  internal static class AudioManager
  {
    private static AvlTree<Sound> _soundDatabase = new AvlTree<Sound>();

    public static void Initialize()
    {
      try { AudioContext AC = new AudioContext(); }
      catch (AudioException e) { throw new AudioManagerException("Could not create an Audio Context (is there something wrong with your audio hardware or settings)."); }
    }

    /// <summary>This is used for throwing AVL Tree exceptions only to make debugging faster.</summary>
    private class AudioManagerException : Exception { public AudioManagerException(string message) : base(message) { } }

    #region OpenTK Audio Example
/*
    // This code was written for the OpenTK library and has been released
// to the Public Domain.
// It is provided "as is" without express or implied warranty of any kind.

using System;
using System.Diagnostics;
using System.Threading;
using System.IO;

using OpenTK.Audio;
using OpenTK.Audio.OpenAL;

namespace Examples
{
    [Example("Playback", ExampleCategory.OpenAL, "1.1", Documentation="Playback")]
    public class Playback
    {
        static readonly string filename = Path.Combine(Path.Combine("Data", "Audio"), "the_ring_that_fell.wav");

        // Loads a wave/riff audio file.
        public static byte[] LoadWave(Stream stream, out int channels, out int bits, out int rate)
        {
            if (stream == null)
                throw new ArgumentNullException("stream");

            using (BinaryReader reader = new BinaryReader(stream))
            {
                // RIFF header
                string signature = new string(reader.ReadChars(4));
                if (signature != "RIFF")
                    throw new NotSupportedException("Specified stream is not a wave file.");

                int riff_chunck_size = reader.ReadInt32();

                string format = new string(reader.ReadChars(4));
                if (format != "WAVE")
                    throw new NotSupportedException("Specified stream is not a wave file.");

                // WAVE header
                string format_signature = new string(reader.ReadChars(4));
                if (format_signature != "fmt ")
                    throw new NotSupportedException("Specified wave file is not supported.");

                int format_chunk_size = reader.ReadInt32();
                int audio_format = reader.ReadInt16();
                int num_channels = reader.ReadInt16();
                int sample_rate = reader.ReadInt32();
                int byte_rate = reader.ReadInt32();
                int block_align = reader.ReadInt16();
                int bits_per_sample = reader.ReadInt16();

                string data_signature = new string(reader.ReadChars(4));
                if (data_signature != "data")
                    throw new NotSupportedException("Specified wave file is not supported.");

                int data_chunk_size = reader.ReadInt32();

                channels = num_channels;
                bits = bits_per_sample;
                rate = sample_rate;

                return reader.ReadBytes((int)reader.BaseStream.Length);
            }
        }

        public static ALFormat GetSoundFormat(int channels, int bits)
        {
            switch (channels)
            {
                case 1: return bits == 8 ? ALFormat.Mono8 : ALFormat.Mono16;
                case 2: return bits == 8 ? ALFormat.Stereo8 : ALFormat.Stereo16;
                default: throw new NotSupportedException("The specified sound format is not supported.");
            }
        }

        public static void Main()
        {
            using (AudioContext context = new AudioContext())
            {
                int buffer = AL.GenBuffer();
                int source = AL.GenSource();
                int state;

                int channels, bits_per_sample, sample_rate;
                byte[] sound_data = LoadWave(File.Open(filename, FileMode.Open), out channels, out bits_per_sample, out sample_rate);
                AL.BufferData(buffer, GetSoundFormat(channels, bits_per_sample), sound_data, sound_data.Length, sample_rate);

                AL.Source(source, ALSourcei.Buffer, buffer);
                AL.SourcePlay(source);

                Trace.Write("Playing");

                // Query the source to find out when it stops playing.
                do
                {
                    Thread.Sleep(250);
                    Trace.Write(".");
                    AL.GetSource(source, ALGetSourcei.SourceState, out state);
                }
                while ((ALSourceState)state == ALSourceState.Playing);

                Trace.WriteLine("");

                AL.SourceStop(source);
                AL.DeleteSource(source);
                AL.DeleteBuffer(buffer);
            }
        }
    }
}
*/

    #endregion

    /*struct SoundSource
    {
      public SoundSource(int bufferId, string filePath)
      {
        _bufferId = bufferId;
        _filePath = filePath;
      }
      public int _bufferId;
      string _filePath;
    }

    //private static Dictionary<string, SoundSource> _soundIdentifier = new Dictionary<string, SoundSource>();
    private static AvlTree<SoundSource> _soundIdentifier = new AvlTree<SoundSource>();
    static readonly int MaxSoundChannels = 256;
    //static List<int> _soundChannels = new List<int>();
    static ListArray<int> _soundChannels = new ListArray<int>(100);
    static float _masterVolume = 1.0f;

    //public SoundManager()
    //{
      //Alut.alutInit();
      //DicoverSoundChannels();
    //}

    private static void DicoverSoundChannels()
    {
      while (_soundChannels.Count < MaxSoundChannels)
      {
        int src;
        AL.GenSources(1, out src);
        //Al.alGenSources(1, out src);
        if (AL.GetError() == ALError.NoError)
        //if (Al.alGetError() == Al.AL_NO_ERROR)
        {
          _soundChannels.Add(src);
        }
        else
        {
          break; // there's been an error - we've filled all the channels.
        }
      }
    }

    private static bool IsChannelPlaying(int channel)
    {
      return AL.GetSourceState(channel) == ALSourceState.Playing;
      //int value = 0; 
      //Al.alGetSourcei(channel, Al.AL_SOURCE_STATE, out value);
      //return (value == Al.AL_PLAYING);
    }

    private static int FindNextFreeChannel()
    {
      for (int i = 0; i < _soundChannels.Count; i++)
        if (!IsChannelPlaying(_soundChannels[i]))
          return _soundChannels[i];

      //foreach (int slot in _soundChannels)
        //if (!IsChannelPlaying(slot))
          //return slot;

        return -1;
    }


    public static void LoadSound(string soundId, string path)
    {
      // Generate a buffer.
      int buffer = -1;
      AL.GenBuffers(1, out buffer);
      //Al.alGenBuffers(1, out buffer);

      ALError errorCode = AL.GetError();
      System.Diagnostics.Debug.Assert(errorCode == ALError.NoError);
      //int errorCode = Al.alGetError();
      //System.Diagnostics.Debug.Assert(errorCode == Al.AL_NO_ERROR);

      ALFormat format;
      //int format;
      float frequency;
      int size;
      System.Diagnostics.Debug.Assert(File.Exists(path));

      //IntPtr data = AL.LoadMemoryFromFile(path, out format, out size, out frequency);
      //System.Diagnostics.Debug.Assert(data != IntPtr.Zero);
      // Load wav data into the generated buffer.
      //AL.BufferData(buffer, format, data, size, (int)frequency);
      // Every seems ok, add it to the library.
      _soundIdentifier.Add(soundId, new SoundSource(buffer, path));
    }

    public static Sound PlaySound(string soundId)
    {
      // Default play sound doesn't loop.
      return PlaySound(soundId, false);
    }
    public static Sound PlaySound(string soundId, bool loop)
    {
      int channel = FindNextFreeChannel();
      if (channel != -1)
      {
        AL.SourceStop(channel);
        //AL.Source(channel, ALSourcei.Buffer, _soundIdentifier[soundId]._bufferId);
        AL.Source(channel, ALSourcei.Buffer, _soundIdentifier.Get(soundId)._bufferId);
        AL.Source(channel, ALSourcef.Pitch, 1.0f);
        AL.Source(channel, ALSourcef.Gain, 1.0f);
        //Al.alSourceStop(channel);
        //Al.alSourcei(channel, Al.AL_BUFFER, _soundIdentifier[soundId]._bufferId);
        //Al.alSourcef(channel, Al.AL_PITCH, 1.0f);
        //Al.alSourcef(channel, Al.AL_GAIN, 1.0f);

        if (loop)
        {
          AL.Source(channel, ALSourceb.Looping, true);
          //Al.alSourcei(channel, Al.AL_LOOPING, 1);
        }
        else
        {
          AL.Source(channel, ALSourceb.Looping, false);
          //Al.alSourcei(channel, Al.AL_LOOPING, 0);
        }
        AL.Source(channel, ALSourcef.Gain, _masterVolume);
        //  Al.alSourcef(channel, Al.AL_GAIN, _masterVolume);
        AL.SourcePlay(channel);
        //Al.alSourcePlay(channel);
        return new Sound(channel);
      }
      else
      {
        // Error sound
        return new Sound(-1);
      }
    }


    public static void ChangeVolume(Sound sound, float value)
    {
      AL.Source(sound.Channel, ALSourcef.Gain, _masterVolume * value);
      //Al.alSourcef(sound.Channel, Al.AL_GAIN, _masterVolume * value);
    }

    public static bool IsSoundPlaying(Sound sound)
    {
      return IsChannelPlaying(sound.Channel);
    }

    public static void StopSound(Sound sound)
    {
      if (sound.Channel == -1)
      {
        return;
      }
      AL.SourceStop(sound.Channel);
      //Al.alSourceStop(sound.Channel);
    }

    public static void MasterVolume(float value)
    {
      _masterVolume = value;

      for (int i = 0; i < _soundChannels.Count; i++)
        AL.Source(_soundChannels[i], ALSourcef.Gain, value);

      //foreach (int channel in _soundChannels)
      //  AL.Source(channel, ALSourcef.Gain, value);
    }*/
  }
}