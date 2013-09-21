using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using System.Drawing.Imaging;

namespace Engine
{
  public class ModelManager : IDisposable
  {
    Dictionary<string, SuperModel> _superModelDatabase = new Dictionary<string, SuperModel>();

    public SuperModel Get(string superModelId)
    {
      return _superModelDatabase[superModelId];
    }

    private bool LoadModelFromDisk(string path, out int id, out int height, out int width)
    {
      throw new NotImplementedException();
    }

    public void LoadModel(string superModelId, string path)
    {
      throw new NotImplementedException();
    }

    #region IDisposable Members

    public void Dispose()
    {
      foreach (SuperModel t in _superModelDatabase.Values)
      {
        throw new NotImplementedException();
      }
    }

    #endregion

  }
}