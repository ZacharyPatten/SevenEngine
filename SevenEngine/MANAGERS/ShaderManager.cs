// SEVENENGINE LISCENSE:
// You are free to use, modify, and distribute any or all code segments/files for any purpose
// including commercial use under the following condition: any code using or originally taken 
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
using OpenTK.Graphics.OpenGL;
using SevenEngine.Shaders;
using Seven.Structures;

namespace SevenEngine
{
  /// <summary>ShaderManager is used for shader management (loading, storing, hardware instance controling, disposing, and GPU compilation).</summary>
  public static class ShaderManager
  {
    internal static ShaderProgram _defaultShader;
    internal static ShaderProgram _colorShader;
    internal static ShaderProgram _textShader;
    internal static ShaderProgram _lightShader;

    public static ShaderProgram DefaultShader { get { return _defaultShader; } }
    public static ShaderProgram ColorShader { get { return _colorShader; } }
    public static ShaderProgram TextShader { get { return _textShader; } }
    public static ShaderProgram LightShader { get { return _lightShader; } }

    private static AvlTree<VertexShader> _vertexShaderDatabase = new AvlTree_Linked<VertexShader>(VertexShader.CompareTo);
    private static AvlTree<FragmentShader> _fragmentShaderDatabase = new AvlTree_Linked<FragmentShader>(FragmentShader.CompareTo);
    private static AvlTree<GeometryShader> _geometryShaderDatabase = new AvlTree_Linked<GeometryShader>(GeometryShader.CompareTo);
    private static AvlTree<ExtendedGeometryShader> _extendedGeometryShaderDatabase = new AvlTree_Linked<ExtendedGeometryShader>(ExtendedGeometryShader.CompareTo);
    private static AvlTree<ShaderProgram> _shaderProgramDatabase = new AvlTree_Linked<ShaderProgram>(ShaderProgram.CompareTo);

    public static int Comparison(Func<FragmentShader, int> function, FragmentShader right)
    {
      return function(right);
    }

    /// <summary>Get a vertex shader that has been loaded and compiled on the GPU.</summary>
    /// <param name="shaderId">The name associated with the shader when you loaded it.</param>
    /// <returns>The shader if it exists.</returns>
    public static VertexShader GetVertexShader(string shaderId)
    {
      //VertexShader shader = _vertexShaderDatabase[shaderId];
      VertexShader shader = _vertexShaderDatabase.Get<string>(shaderId, VertexShader.CompareTo);
      shader.ExistingReferences++;
      return shader;
    }
    
    /// <summary>Get a fragment shader that has been loaded and compiled on the GPU.</summary>
    /// <param name="shaderId">The name associated with the shader when you loaded it.</param>
    /// <returns>The shader if it exists.</returns>
    public static FragmentShader GetFragmentShader(string shaderId)
    {
      //FragmentShader shader = _fragmentShaderDatabase[shaderId];
      FragmentShader shader = _fragmentShaderDatabase.Get<string>(shaderId, FragmentShader.CompareTo);
      shader.ExistingReferences++;
      return shader;
    }

    /// <summary>Get a geometry shader that has been loaded and compiled on the GPU.</summary>
    /// <param name="shaderId">The name associated with the shader when you loaded it.</param>
    /// <returns>The shader if it exists.</returns>
    public static GeometryShader GetGeometryShader(string shaderId)
    {
      //GeometryShader shader = _geometryShaderDatabase[shaderId];
      GeometryShader shader = _geometryShaderDatabase.Get<string>(shaderId, GeometryShader.CompareTo);
      shader.ExistingReferences++;
      return shader;
    }

    /// <summary>Get an extended geometry shader that has been loaded and compiled on the GPU.</summary>
    /// <param name="shaderId">The name associated with the shader when you loaded it.</param>
    /// <returns>The shader if it exists.</returns>
    public static ExtendedGeometryShader GetExtendedGeometryShader(string shaderId)
    {
      //ExtendedGeometryShader shader = _extendedGeometryShaderDatabase[shaderId];
      ExtendedGeometryShader shader = _extendedGeometryShaderDatabase.Get<string>(shaderId, ExtendedGeometryShader.CompareTo);
      shader.ExistingReferences++;
      return shader;
    }
    
    /// <summary>Get a shader program that has been loaded and compiled on the GPU.</summary>
    /// <param name="shaderId">The name associated with the shader when you loaded it.</param>
    /// <returns>The shader if it exists.</returns>
    public static ShaderProgram GetShaderProgram(string shaderId)
    {
      //ShaderProgram shaderProgram = _shaderProgramDatabase[shaderId];
      ShaderProgram shaderProgram = _shaderProgramDatabase.Get<string>(shaderId, ShaderProgram.CompareTo);

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
      Output.WriteLine("Shader program created: \"" + programError + "\";");
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
      Renderer.DefaultShaderProgram = ShaderManager.GetShaderProgram(shaderProgramId);
      GL.UseProgram(GetShaderProgram(shaderProgramId).GpuHandle);
      Output.WriteLine("Shader program set to default: \"" + shaderProgramId + "\";");
      return true;
    }

    internal static void SetUpBuiltInShaders()
    {
      // Basic Vertex Shader
      int basicVertexShaderHandle = GL.CreateShader(ShaderType.VertexShader);
      GL.ShaderSource(basicVertexShaderHandle, VertexShader.Texture);
      GL.CompileShader(basicVertexShaderHandle);
      VertexShader basicVertexShader =
        new VertexShader("VertexShaderBasic", "Built-In", basicVertexShaderHandle);

      // Transform Vertex Shader
      int transformVertexShaderHandle = GL.CreateShader(ShaderType.VertexShader);
      GL.ShaderSource(transformVertexShaderHandle, VertexShader.FTransform);
      GL.CompileShader(transformVertexShaderHandle);
      VertexShader transformVertexShader =
        new VertexShader("VertexShaderTransform", "Built-In", transformVertexShaderHandle);

      // Basic Fragment Shader
      int basicFragmentShaderHandle = GL.CreateShader(ShaderType.FragmentShader);
      GL.ShaderSource(basicFragmentShaderHandle, FragmentShader.Texture);
      GL.CompileShader(basicFragmentShaderHandle);
      FragmentShader basicFragmentShader =
        new FragmentShader("FragmentShaderBasic", "Built-In", basicFragmentShaderHandle);

      // Color Fragment Shader
      int colorFragmentShaderHandle = GL.CreateShader(ShaderType.FragmentShader);
      GL.ShaderSource(colorFragmentShaderHandle, FragmentShader.Color);
      GL.CompileShader(colorFragmentShaderHandle);
      FragmentShader colorFragmentShader =
        new FragmentShader("FragmentShaderBasic", "Built-In", colorFragmentShaderHandle);

      // Text Fragment Shader
      int textFragmentShaderHandle = GL.CreateShader(ShaderType.FragmentShader);
      GL.ShaderSource(textFragmentShaderHandle, FragmentShader.Text);
      GL.CompileShader(textFragmentShaderHandle);
      FragmentShader textFragmentShader =
        new FragmentShader("FragmentShaderBasic", "Built-In", textFragmentShaderHandle);

      // Make the default shader program
      int basicProgramHandle = GL.CreateProgram();
      GL.AttachShader(basicProgramHandle, basicVertexShader.GpuHandle);
      GL.AttachShader(basicProgramHandle, basicFragmentShader.GpuHandle);
      GL.LinkProgram(basicProgramHandle);
      _defaultShader = new ShaderProgram("ShaderProgramBasic", basicProgramHandle);
      Renderer.DefaultShaderProgram = _defaultShader;

      // Make the color shader program
      int colorProgramHandle = GL.CreateProgram();
      GL.AttachShader(colorProgramHandle, transformVertexShader.GpuHandle);
      GL.AttachShader(colorProgramHandle, colorFragmentShader.GpuHandle);
      GL.LinkProgram(colorProgramHandle);
      _colorShader = new ShaderProgram("ShaderProgramColor", colorProgramHandle);

      // Make the default shader program
      int textProgramHandle = GL.CreateProgram();
      GL.AttachShader(textProgramHandle, basicVertexShader.GpuHandle);
      GL.AttachShader(textProgramHandle, textFragmentShader.GpuHandle);
      GL.LinkProgram(textProgramHandle);
      _textShader = new ShaderProgram("ShaderProgramText", textProgramHandle);

      // Vertex Shader Light
      int vertexShaderLightHandle = GL.CreateShader(ShaderType.VertexShader);
      GL.ShaderSource(vertexShaderLightHandle, VertexShader.Light);
      GL.CompileShader(vertexShaderLightHandle);
      VertexShader vertexShaderLight =
        new VertexShader("VertexShaderLight", "Built-In", vertexShaderLightHandle);

      // Fragment Shader Light
      int fragmentShaderLightHandle = GL.CreateShader(ShaderType.FragmentShader);
      GL.ShaderSource(fragmentShaderLightHandle, FragmentShader.Light);
      GL.CompileShader(fragmentShaderLightHandle);
      FragmentShader fragmentShaderLight =
        new FragmentShader("FragmentShaderBasic", "Built-In", fragmentShaderLightHandle);


      // Shader Program Light
      int shaderProgramLightHandle = GL.CreateProgram();
      GL.AttachShader(shaderProgramLightHandle, vertexShaderLight.GpuHandle);
      GL.AttachShader(shaderProgramLightHandle, fragmentShaderLight.GpuHandle);
      GL.LinkProgram(shaderProgramLightHandle);
      _lightShader = new ShaderProgram("ShaderProgramLight", shaderProgramLightHandle);
    }
  }
}