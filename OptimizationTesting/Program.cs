using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

using SevenEngine;
using SevenEngine.DataStructures;
using SevenEngine.DataStructures.Interfaces;
using SevenEngine.Mathematics;
using SevenEngine.Imaging;

namespace OptimizationTesting
{
  class Program
  {
    public class item : InterfaceStringId
    {
      public string Id { get; set; }
      public item(string id)
      {
        Id = id;
      }
    }

    static void Main(string[] args)
    {
      // THIS PROJECT IS FOR THE DEVELOPER(S) OF THE ENGINE TO
      // TEST RUNTIMES OF VARIOUS FUNCTIONS.

      Stopwatch stopwatch = new Stopwatch();
      long garbage = stopwatch.ElapsedTicks;

      int range = 1000000;
      //SevenEngine.DataStructures.List2<item> list = new SevenEngine.DataStructures.List2<item>();
      SevenEngine.DataStructures.List<item> list3 = new SevenEngine.DataStructures.List<item>();
      System.Collections.Generic.LinkedList<item> list2 = new System.Collections.Generic.LinkedList<item>();

      item[] items = new item[range];
      for (int i = 0; i < range; i++)
        items[i] = new item(i.ToString());

      //stopwatch.Restart();
      //for (int i = 0; i < range; i++)
      //  list.Add(items[i]);
      ////list.Foreach(printItem);
      //for (int i = 0; i < range; i++)
      ////for (int i = range - 1; i > -1; i--)
      //  list.Remove(items[i].Id);
      ////list.Clear();
      //stopwatch.Stop();
      //Console.WriteLine("Mine: " + stopwatch.ElapsedTicks);

      stopwatch.Restart();
      for (int i = 0; i < range; i++)
        list3.Add(items[i]);
      //list.Foreach(printItem);
      for (int i = 0; i < range; i++)
      //for (int i = range - 1; i > -1; i--)
        list3.Remove(items[i].Id);
      //list.Clear();
      stopwatch.Stop();
      Console.WriteLine("MineOld: " + stopwatch.ElapsedTicks);

      stopwatch.Restart();
      for (int i = 0; i < range; i++)
        list2.AddLast(items[i]);
      for (int i = 0; i < range; i++)
      //for (int i = range - 1; i > -1; i--)
      //  list2.Remove(items[i]);
      list2.Clear();
      stopwatch.Stop();
      Console.WriteLine("System: " + stopwatch.ElapsedTicks);

      Console.ReadLine();
    }

    public static void printItem(InterfaceStringId item)
    {
      Console.WriteLine(item.Id);
    }
  }
}
