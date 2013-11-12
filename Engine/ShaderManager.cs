// SEVENENGINE LISCENSE:
// You are free to use, modify, and distribute any or all code segments/files for any purpose
// including commercial use with the following condition: any code using or originally taken 
// from the SevenEngine project must include citation to its original author(s) located at the
// top of each source code file, or you may include a reference to the SevenEngine project as
// a whole but you must include the current SevenEngine official website URL and logo.
// - Thanks.  :)  (support: seven@sevenengine.com)

// Author(s):
// - Zachary Aaron Patten (aka Seven) seven@sevenengine.com
// Last Edited: 10-26-13

using System;
using System.IO;

using OpenTK;
using OpenTK.Graphics.OpenGL;

using SevenEngine.Shaders;
using SevenEngine.DataStructures;

namespace SevenEngine
{
  /// <summary>This static ShaderManager class is storage for all shader files and shader programs. It also includes the code for
  /// loading shaders from files and linking them into shader programs.</summary>
  public static class ShaderManager
  {
    private static AvlTree<VertexShader> _vertexShaderDatabase = new AvlTree<VertexShader>();
    private static AvlTree<FragmentShader> _fragmentShaderDatabase = new AvlTree<FragmentShader>();
    private static AvlTree<GeometryShader> _geometryShaderDatabase = new AvlTree<GeometryShader>();
    private static AvlTree<ExtendedGeometryShader> _extendedGeometryShaderDatabase = new AvlTree<ExtendedGeometryShader>();
    private static AvlTree<ShaderProgram> _shaderProgramDatabase = new AvlTree<ShaderProgram>();

    /// <summary>Get a vertex shader that has been loaded and compiled on the GPU.</summary>
    /// <param name="shaderId">The name associated with the shader when you loaded it.</param>
    /// <returns>The shader if it exists.</returns>
    internal static VertexShader GetVertexShader(string shaderId)
    {
      //VertexShader shader = _vertexShaderDatabase[shaderId];
      VertexShader shader = _vertexShaderDatabase.Get(shaderId);
      shader.ExistingReferences++;
      return shader;
    }
    
    /// <summary>Get a fragment shader that has been loaded and compiled on the GPU.</summary>
    /// <param name="shaderId">The name associated with the shader when you loaded it.</param>
    /// <returns>The shader if it exists.</returns>
    internal static FragmentShader GetFragmentShader(string shaderId)
    {
      //FragmentShader shader = _fragmentShaderDatabase[shaderId];
      FragmentShader shader = _fragmentShaderDatabase.Get(shaderId);
      shader.ExistingReferences++;
      return shader;
    }

    /// <summary>Get a geometry shader that has been loaded and compiled on the GPU.</summary>
    /// <param name="shaderId">The name associated with the shader when you loaded it.</param>
    /// <returns>The shader if it exists.</returns>
    internal static GeometryShader GetGeometryShader(string shaderId)
    {
      //GeometryShader shader = _geometryShaderDatabase[shaderId];
      GeometryShader shader = _geometryShaderDatabase.Get(shaderId);
      shader.ExistingReferences++;
      return shader;
    }

    /// <summary>Get an extended geometry shader that has been loaded and compiled on the GPU.</summary>
    /// <param name="shaderId">The name associated with the shader when you loaded it.</param>
    /// <returns>The shader if it exists.</returns>
    internal static ExtendedGeometryShader GetExtendedGeometryShader(string shaderId)
    {
      //ExtendedGeometryShader shader = _extendedGeometryShaderDatabase[shaderId];
      ExtendedGeometryShader shader = _extendedGeometryShaderDatabase.Get(shaderId);
      shader.ExistingReferences++;
      return shader;
    }
    
    /// <summary>Get a shader program that has been loaded and compiled on the GPU.</summary>
    /// <param name="shaderId">The name associated with the shader when you loaded it.</param>
    /// <returns>The shader if it exists.</returns>
    internal static ShaderProgram GetShaderProgram(string shaderId)
    {
      //ShaderProgram shaderProgram = _shaderProgramDatabase[shaderId];
      ShaderProgram shaderProgram = _shaderProgramDatabase.Get(shaderId);

      shaderProgram.ExistingReferences++;
      return shaderProgram;
    }

    /// <summary>Loads a shader file, compiles it on the GPU, and adds it to the database.</summary>
    /// <param name="shaderId">The name you want to associate with this specific shader so you can use it later.</param>
    /// <param name="filePath">The file location of the shader file.</param>
    /// <returns>If the load was successful. (true if successful; false if unsuccessful)</returns>
    public static bool LoadVertexShader(string shaderId, string filePath)
    {
      // Store the entire file in a string to be compiled on the GPU.
      string shaderSource;
      using (StreamReader reader = new StreamReader(filePath)) { shaderSource = reader.ReadToEnd(); }

      // Attempt to load the shader on the GPU and compile it
      int shaderHandle;
      shaderHandle = GL.CreateShader(ShaderType.VertexShader);
      GL.ShaderSource(shaderHandle, shaderSource);
      GL.CompileShader(shaderHandle);

      // Check for compiler errors
      string[] filePathSplit = filePath.Split('\\');
      string compilationError = GL.GetShaderInfoLog(shaderHandle);
      if (//compilationError == "" ||  
        compilationError == "No errors\n")
      {
        Output.WriteLine("ERROR loading shader \"" + filePathSplit[filePathSplit.Length - 1] + "\";");
        Output.WriteLine(compilationError);
        return false;
      }

      // The load and comilation was successful, we can add it to the database.
      _vertexShaderDatabase.Add(new VertexShader(shaderId, filePath, shaderHandle));
      Output.WriteLine("Shader file compiled \"" + filePathSplit[filePathSplit.Length - 1] + "\";");
      return true;
    }

    /// <summary>Loads a shader file, compiles it on the GPU, and adds it to the database.</summary>
    /// <param name="shaderId">The name you want to associate with this specific shader so you can use it later.</param>
    /// <param name="filePath">The file location of the shader file.</param>
    /// <returns>If the load was successful. (true if successful; false if unsuccessful)</returns>
    public static bool LoadFragmentShader(string shaderId, string filePath)
    {
      // Store the entire file in a string to be compiled on the GPU.
      string shaderHandle;
      using (StreamReader reader = new StreamReader(filePath)) { shaderHandle = reader.ReadToEnd(); }

      // Attempt to load the shader on the GPU and compile it
      int shaderLocation;
      shaderLocation = GL.CreateShader(ShaderType.FragmentShader);
      GL.ShaderSource(shaderLocation, shaderHandle);
      GL.CompileShader(shaderLocation);

      // Check for compiler errors
      string[] filePathSplit = filePath.Split('\\');
      string compilationError = GL.GetShaderInfoLog(shaderLocation);
      if (//compilationError == "" || 
        compilationError == "No errors\n")
      {
        Output.WriteLine("ERROR loading shader \"" + filePathSplit[filePathSplit.Length - 1] + "\";");
        Output.WriteLine(compilationError);
        return false;
      }

      // The load and comilation was successful, we can add it to the database.
      _fragmentShaderDatabase.Add(new FragmentShader(shaderId, filePath, shaderLocation));
      Output.WriteLine("Shader file compiled \"" + filePathSplit[filePathSplit.Length - 1] + "\";");
      return true;
    }

    /// <summary>Loads a shader file, compiles it on the GPU, and adds it to the database.</summary>
    /// <param name="shaderId">The name you want to associate with this specific shader so you can use it later.</param>
    /// <param name="filePath">The file location of the shader file.</param>
    /// <returns>If the load was successful. (true if successful; false if unsuccessful)</returns>
    public static bool LoadGeometryShader(string shaderId, string filePath)
    {
      // Store the entire file in a string to be compiled on the GPU.
      string shaderSource;
      using (StreamReader reader = new StreamReader(filePath)) { shaderSource = reader.ReadToEnd(); }

      // Attempt to load the shader on the GPU and compile it
      int shaderHandle;
      shaderHandle = GL.CreateShader(ShaderType.GeometryShader);
      GL.ShaderSource(shaderHandle, shaderSource);
      GL.CompileShader(shaderHandle);

      // Check for compiler errors
      string[] filePathSplit = filePath.Split('\\');
      string compilationError = GL.GetShaderInfoLog(shaderHandle);
      if (compilationError == "" || compilationError == "No errors\n")
      {
        Output.WriteLine("ERROR loading shader \"" + filePathSplit[filePathSplit.Length - 1] + "\";");
        Output.WriteLine(compilationError);
        return false;
      }

      // The load and comilation was successful, we can add it to the database.
      _geometryShaderDatabase.Add(new GeometryShader(shaderId, filePath, shaderHandle));
      Output.WriteLine("Shader file compiled \"" + filePathSplit[filePathSplit.Length - 1] + "\";");
      return true;
    }

    /// <summary>Loads a shader file, compiles it on the GPU, and adds it to the database.</summary>
    /// <param name="shaderId">The name you want to associate with this specific shader so you can use it later.</param>
    /// <param name="filePath">The file location of the shader file.</param>
    /// <returns>If the load was successful. (true if successful; false if unsuccessful)</returns>
    public static bool LoadExtendedGeometryShader(string shaderId, string filePath)
    {
      // Store the entire file in a string to be compiled on the GPU.
      string shaderSource;
      using (StreamReader reader = new StreamReader(filePath)) { shaderSource = reader.ReadToEnd(); }

      // Attempt to load the shader on the GPU and compile it
      int shaderHandle;
      shaderHandle = GL.CreateShader(ShaderType.GeometryShaderExt);
      GL.ShaderSource(shaderHandle, shaderSource);
      GL.CompileShader(shaderHandle);

      // Check for compiler errors
      string[] filePathSplit = filePath.Split('\\');
      string compilationError = GL.GetShaderInfoLog(shaderHandle);
      if (compilationError == "" || compilationError == "No errors\n")
      {
        Output.WriteLine("ERROR loading shader \"" + filePathSplit[filePathSplit.Length - 1] + "\";");
        Output.WriteLine(compilationError);
        return false;
      }

      // The load and comilation was successful, we can add it to the database.
      _extendedGeometryShaderDatabase.Add(new ExtendedGeometryShader(shaderId, filePath, shaderHandle));
      Output.WriteLine("Shader file compiled \"" + filePathSplit[filePathSplit.Length - 1] + "\";");
      return true;
    }

    public static bool MakeShaderProgram(
      string programId,
      string vertexShaderId,
      string fragmentShaderId,
      string geometryShaderId,
      string extendedGeometryShaderId)
    {
      int programHandle;

      // Generate the shader program
      programHandle = GL.CreateProgram();

      // Link the desired shaders to complete the shader program on the GPU.
      if (vertexShaderId != null)
        GL.AttachShader(programHandle, GetVertexShader(vertexShaderId).GpuHandle);
      if (fragmentShaderId != null)
        GL.AttachShader(programHandle, GetFragmentShader(fragmentShaderId).GpuHandle);
      if (geometryShaderId != null)
        GL.AttachShader(programHandle, GetGeometryShader(geometryShaderId).GpuHandle);
      if (extendedGeometryShaderId != null)
        GL.AttachShader(programHandle, GetExtendedGeometryShader(extendedGeometryShaderId).GpuHandle);

      // Call for OpenGL to link the program together
      GL.LinkProgram(programHandle);

      // Check for errors
      string programError = GL.GetProgramInfoLog(programHandle);
      //if (programError == "" || programError == "No errors\n")
      //{
        //Output.WriteLine("ERROR creating shader program: \"" + programId + "\";");
        //Output.WriteLine(programError);
        //return false;
      //}

      // The program has been successfully created. Add it to the database so we can use it.
      _shaderProgramDatabase.Add(new ShaderProgram(programId, programHandle));
      Output.WriteLine("Shader program created: \"" + programId + "\";");
      return true;
    }

    /// <summary>Sets the currently active shader program by the name associated with it.</summary>
    /// <param name="shaderProgramId">The name you associated with this specific shader program when you created it.</param>
    /// <returns>If the shader selection was successful. (true if success, false if unsuccessful)</returns>
    public static bool SetActiveShader(string shaderProgramId)
    {
      //if (!_shaderProgramDatabase.ContainsKey(shaderProgramId))
      //{
        //Output.WriteLine("ERROR activating shader program: \"" + shaderProgramId + "\";");
        //return false;
      //}

      GL.UseProgram(GetShaderProgram(shaderProgramId).GpuHandle);
      Output.WriteLine("Shader program activated: \"" + shaderProgramId + "\";");
      return true;
    }
  }
}