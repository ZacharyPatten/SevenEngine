// Seven
// https://github.com/53V3N1X/SevenEngine
// LISCENSE: See "LISCENSE.txt" in th root project directory.
// SUPPORT: See "README.txt" in the root project directory.

using Seven;
using OpenTK.Graphics.OpenGL;
using Seven.Structures;

namespace SevenEngine.Shaders
{
  /// <summary>Represents a single shader program that has been loaded on the GPU.</summary>
  public class ShaderProgram
  {
    protected string _id;
    protected int _gpuHandle;
    protected int _existingReferences;

    /// <summary>The file path form which the shader was loaded.</summary>
    public string Id { get { return _id; } set { _id = value; } }
    /// <summary>The location of the shader program on the GPU.</summary>
    public int GpuHandle { get { return _gpuHandle; } set { _gpuHandle = value; } }
    /// <summary>The number of existing hardware instances of this model reference.</summary>
    public int ExistingReferences { get { return _existingReferences; } set { _existingReferences = value; } }

    /// <summary>Creates an instance of a GPU referencing class for a shader program.</summary>
    /// <param name="id">The id of this geometry shader used for look-up purposes.</param>
    /// <param name="gpuHandle">The GPU handle or location where the memory starts on VRAM.</param>
    public ShaderProgram(string id, int gpuHandle)
    {
      _id = id;
      _gpuHandle = gpuHandle;
      _existingReferences = 0;
    }

    public static Comparison CompareTo(ShaderProgram left, ShaderProgram right)
    {
      int comparison = left.Id.CompareTo(right.Id);
      if (comparison > 0)
        return Comparison.Greater;
      else if (comparison < 0)
        return Comparison.Less;
      else
        return Comparison.Equal;
    }

    public static Comparison CompareTo(ShaderProgram left, string right)
    {
      int comparison = left.Id.CompareTo(right);
      if (comparison > 0)
        return Comparison.Greater;
      else if (comparison < 0)
        return Comparison.Less;
      else
        return Comparison.Equal;
    }

    public void SetShaderParameter(string variable, float value)
    {
      int handle = GL.GetUniformLocation(_gpuHandle, variable);
      if (handle == -1)
        throw new ShaderException("Shader program " + _id + " does not have a parameter named " + variable);
      GL.Uniform1(handle, value);
    }

    public void SetShaderParameter(string variable, int value)
    {
      int handle = GL.GetUniformLocation(_gpuHandle, variable);
      if (handle == -1)
        throw new ShaderException("Shader program " + _id + " does not have a parameter named " + variable);
      GL.Uniform1(handle, value);
    }

    /*public static void SetShaderParameters(
      float[] worldMatrix, float[] viewMatrix, float[] projectionMatrix,
      int texture,
      float[] lightDirection, float[] diffuseLightColor, float[] ambientLight)
    {
      uint location;


      // Set the world matrix in the vertex shader.
      location = OpenGL->glGetUniformLocation(m_shaderProgram, "worldMatrix");
      if (location == -1)
      {
        return false;
      }
      OpenGL->glUniformMatrix4fv(location, 1, false, worldMatrix);

      // Set the view matrix in the vertex shader.
      location = OpenGL->glGetUniformLocation(m_shaderProgram, "viewMatrix");
      if (location == -1)
      {
        return false;
      }
      OpenGL->glUniformMatrix4fv(location, 1, false, viewMatrix);

      // Set the projection matrix in the vertex shader.
      location = OpenGL->glGetUniformLocation(m_shaderProgram, "projectionMatrix");
      if (location == -1)
      {
        return false;
      }
      OpenGL->glUniformMatrix4fv(location, 1, false, projectionMatrix);

      // Set the texture in the pixel shader to use the data from the first texture unit.
      location = OpenGL->glGetUniformLocation(m_shaderProgram, "shaderTexture");
      if (location == -1)
      {
        return false;
      }
      OpenGL->glUniform1i(location, textureUnit);

      // Set the light direction in the pixel shader.
      location = OpenGL->glGetUniformLocation(m_shaderProgram, "lightDirection");
      if (location == -1)
      {
        return false;
      }
      OpenGL->glUniform3fv(location, 1, lightDirection);

      // Set the light direction in the pixel shader.
      location = OpenGL->glGetUniformLocation(m_shaderProgram, "diffuseLightColor");
      if (location == -1)
      {
        return false;
      }
      OpenGL->glUniform4fv(location, 1, diffuseLightColor);
      //The ambient value in the pixel shader is set here.

      // Set the ambient light in the pixel shader.
      location = OpenGL->glGetUniformLocation(m_shaderProgram, "ambientLight");
      if (location == -1)
      {
        return false;
      }
      OpenGL->glUniform4fv(location, 1, ambientLight);

      return true;

    }*/

    /// <summary>This is used for throwing shader exceptions only to make debugging faster.</summary>
    private class ShaderException : System.Exception { public ShaderException(string message) : base(message) { } }
  }
}