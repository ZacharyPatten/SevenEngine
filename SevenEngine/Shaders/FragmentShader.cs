// Seven
// https://github.com/53V3N1X/SevenEngine
// LISCENSE: See "LISCENSE.txt" in th root project directory.
// SUPPORT: See "README.txt" in the root project directory.

using Seven;
using Seven.Structures;

namespace SevenEngine.Shaders
{
  /// <summary>Represents a single fragment shader that has been loaded on the GPU.</summary>
  public class FragmentShader
  {
    protected string _filePath;
    protected string _id;
    protected int _gpuHandle;
    protected int _existingReferences;

    /// <summary>The file path form which the shader was loaded.</summary>
    public string FilePath { get { return _filePath; } set { _filePath = value; } }
    /// <summary>The name associated with this shader when it was loaded.</summary>
    public string Id { get { return _id; } set { _id = value; } }
    /// <summary>The location of the shader program on the GPU.</summary>
    public int GpuHandle { get { return _gpuHandle; } set { _gpuHandle = value; } }
    /// <summary>The number of existing hardware instances of this model reference.</summary>
    public int ExistingReferences { get { return _existingReferences; } set { _existingReferences = value; } }

    /// <summary>Creates an instance of a GPU referencing class for a fragment shader.</summary>
    /// <param name="id">The id of this fragment shader used for look-up purposes.</param>
    /// <param name="filePath">The filepath from which this fragment shader was loaded.</param>
    /// <param name="gpuHandle">The GPU handle or location where the memory starts on VRAM.</param>
    public FragmentShader(string id, string filePath, int gpuHandle)
    {
      _id = id;
      _filePath = filePath;
      _gpuHandle = gpuHandle;
      _existingReferences = 0;
    }

    public static Comparison CompareTo(FragmentShader left, FragmentShader right)
    {
      int comparison = left.Id.CompareTo(right.Id);
      if (comparison > 0)
        return Comparison.Greater;
      else if (comparison < 0)
        return Comparison.Less;
      else
        return Comparison.Equal;
    }

    public static Comparison CompareTo(FragmentShader left, string right)
    {
      int comparison = left.Id.CompareTo(right);
      if (comparison > 0)
        return Comparison.Greater;
      else if (comparison < 0)
        return Comparison.Less;
      else
        return Comparison.Equal;
    }

    // These are the shaders that my engine uses:
    #region Fragment Shader Library
    internal const string Texture =
    @"uniform sampler2D tex;
    void main()
    {
      gl_FragColor = texture2D(tex, gl_TexCoord[0].st);
    }";

    internal const string Color =
    @"uniform vec4 color;
    void main()
    {
      gl_FragColor = vec4(color.x, color.y, color.z, color.w);
    }";

    internal const string Text =
    @"uniform sampler2D texture;
    uniform vec4 color;
    void main()
    {
      gl_FragColor = vec4(color.x, color.y, color.z, texture2D(texture, gl_TexCoord[0].st).w + color.w);
    }";

    internal const string Light =
    @"varying vec3 light;
    varying vec3 normal;
	uniform sampler2D tex;
	void main()
	{
      vec3 ct,cf;
      vec4 texel;
      float intensity,at,af;
      intensity = max(dot(light,normalize(normal)),0.0);
      cf = intensity * (gl_FrontMaterial.diffuse).rgb + gl_FrontMaterial.ambient.rgb;
      af = gl_FrontMaterial.diffuse.a;
      texel = texture2D(tex,gl_TexCoord[0].st);
      ct = texel.rgb;
      at = texel.a;
      gl_FragColor = vec4(ct * cf, at * af);	
	}";
    #endregion
  }
}