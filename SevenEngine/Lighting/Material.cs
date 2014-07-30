// Seven
// https://github.com/53V3N1X/SevenEngine
// LISCENSE: See "LISCENSE.txt" in th root project directory.
// SUPPORT: See "README.txt" in the root project directory.

using System;
using SevenEngine.Imaging;

namespace SevenEngine.Lighting
{
  public class Material
  {
    private Color _specular;
    private float _shininess;
    private Color _ambient;
    private Color _diffuse;

    public Color Specular { get { return _specular; } set { _specular = value; } }
    public float Shininess { get { return _shininess; } set { _shininess = value; } }
    public Color Ambient { get { return _ambient; } set { _ambient = value; } }
    public Color Diffuse { get { return _diffuse; } set { _diffuse = value; } }

    public Material(Color specular, float shininess, Color ambient, Color diffuse)
    {
      _specular = specular;
      _shininess = shininess;
      _ambient = ambient;
      _diffuse = diffuse;
    }
  }
}
