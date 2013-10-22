using System;
using System.IO;

using OpenTK;
using OpenTK.Audio.OpenAL;

using SevenEngine.Audio;
using SevenEngine.DataStructures;

namespace SevenEngine
{
  public static class SoundManager
  {
    struct SoundSource
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
    }
  }
}