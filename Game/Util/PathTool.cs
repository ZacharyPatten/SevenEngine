using System;
using System.IO;
namespace Game
{
  public class PathTool
  {
    public static String GenerateCorrectRelativePath(String path)
    {
      if (IsRunningLinux())
      {
        String newPath = path.Trim().Replace("\\", Path.DirectorySeparatorChar.ToString());
        if (newPath[0] == '/')
        {
          newPath = newPath.Remove(0,1);
        }
        return newPath;
      }
      return path;
    }

   
    private static bool IsRunningLinux()
    {
      int systemIdentifier = (int)Environment.OSVersion.Platform;
      return (systemIdentifier == 4) || (systemIdentifier == 6) || (systemIdentifier == 128);
    }

       
  }
}

