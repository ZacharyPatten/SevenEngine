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
using SevenEngine.Mathematics;

namespace SevenEngine.DataStructures
{
  /// <summary>Data structures and implement this interface contain
  /// traversals using delegates to achieve optimized "foreach" 
  /// implementation.</summary>
  /// <typeparam name="Type">The generic type of the values in the structure.</typeparam>
  public interface DataStructure<Type>
  {
    /// <summary>Traverses a structure and invokes a function on each item until the end of the structure or the function returns false.</summary>
    /// <param name="traversalFunction">The function to invoke on each item in the structure. Return false to break.</param>
    bool TraverseBreakable(Func<Type, bool> traversalFunction);
    /// <summary>Traverses a structure and invokes an action on each item.</summary>
    /// <param name="traversalAction">The action to invoke on each item in the structure.</param>
    void Traverse(Action<Type> traversalAction);
    /// <summary>Converts the structure into an array.</summary>
    /// <returns>An array containing all the item in the structure.</returns>
    Type[] ToArray();
  }

  /// <summary>Implementing classes contain a vector indicating X, Y, and Z position coordinates.</summary>
  public interface InterfacePositionVector
  {
    /// <summary>Indicates an object's X, Y, and Z position coordinates.</summary>
    Vector Position { get; set; }
  }
}
