// SEVENENGINE LISCENSE:
// You are free to use, modify, and distribute any or all code segments/files for any purpose
// including commercial use under the following condition: any code using or originally taken 
// from the SevenEngine project must include citation to its original author(s) located at the
// top of each source code file, or you may include a reference to the SevenEngine project as
// a whole but you must include the current SevenEngine official website URL and logo.
// - Thanks.  :)  (support: seven@sevenengine.com)

// Author(s):
// - Zachary Aaron Patten (aka Seven) seven@sevenengine.com
// Last Edited: 11-22-13

using System;

namespace SevenEngine.DataStructures.Interfaces
{
  /// <summary>Data structures and implement this interface contain
  /// traversals using delegates to achieve optimized "foreach" 
  /// implementation.</summary>
  /// <typeparam name="Type">The generic type of the data structure
  /// that is implementing this interface.</typeparam>
  public interface InterfaceTraversable<Type>
  {
    /// <summary>Traverses a set with an imperative function to perform on each item.</summary>
    /// <param name="traversalFunction">The function to perform on each node of the iteration.</param>
    bool TraverseBreakable(Func<Type, bool> traversalFunction);

    /// <summary>Traverses a set with an imperative action to perform on each item.</summary>
    /// <param name="traversalAction">The action to perform on each node of the iteration.</param>
    void Traverse(Action<Type> traversalAction);
  }
}