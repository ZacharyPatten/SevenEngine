using System;
using OpenTK.Graphics.OpenGL;
using SevenEngine.Shaders;

namespace SevenEngine
{
  public class Light
  {
    public void SetPatameters()
    {
      ShaderProgram shader = ShaderManager.GetShaderProgram("toonShader");

	  float specIntensity = 0.98f;
	  float[] sc = new float[4] {0.8f,0.8f,0.8f,1.0f};
	  float[] threshold = new float[2] {0.5f,0.25f};
	  float[] colors = new float[12] {0.4f,0.4f,0.8f,1.0f,  0.2f,0.2f,0.4f,1.0f,  0.1f,0.1f,0.1f,1.0f};

      int handle1 = GL.GetUniformLocation(shader.GpuHandle, "specIntensity");
      GL.Uniform1(handle1, specIntensity);

      int handle2 = GL.GetUniformLocation(shader.GpuHandle, "specColor");
      GL.Uniform1(handle2, 1, sc);

      int handle3 = GL.GetUniformLocation(shader.GpuHandle, "t");
      GL.Uniform1(handle3, 2, threshold);

      int handle4 = GL.GetUniformLocation(shader.GpuHandle, "colors");
      GL.Uniform1(handle4, 3, colors);
    }
  }
}
