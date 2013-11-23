// SEVENENGINE LISCENSE:
// You are free to use, modify, and distribute any or all code segments/files for any purpose
// including commercial use under the following condition: any code using or originally taken 
// from the SevenEngine project must include citation to its original author(s) located at the
// top of each source code file, or you may include a reference to the SevenEngine project as
// a whole but you must include the current SevenEngine official website URL and logo.
// - Thanks.  :)  (support: seven@sevenengine.com)

// Author(s):
// - Zachary Aaron Patten (aka Seven) seven@sevenengine.com
// Last Edited: 11-23-13

using System;

namespace SevenEngine.DataStructures.Interfaces
{
  /// <summary>Interface for a data structure that supports linear addition. By this I mean 
  /// the structure contains an add method with a single generic parameter.</summary>
  /// <typeparam name="Type">The generic type of the data structure.</typeparam>
  public interface InterfaceLinearlyExpandable<Type>
  {
    /// <summary>Linearly adds a value and possible expands a data structure.</summary>
    /// <param name="addition">The item to be added to the structure.</param>
    void Add(Type addition);
  }
}