using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SevenEngine
{
  public interface InterfaceCloneable { object Clone(); }
  public interface InterfaceComparable { int CompareTo(object other); }
  public interface InterfaceConcatenable { object ConcatinateTo(object right); }
  public interface InterfaceDisposable : IDisposable { }
  public interface InterfaceEquatable { bool EqualTo(object right); }
  public interface InterfaceIndexed { object this[int index] { get; } }
}
