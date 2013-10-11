using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.DataStructures
{
  public class Octree
  {
    // Max items per node (structure branching factor)
    static int _nodeMax;

    // Number of items within this octree
    private int _count;

    // First vertex location (-x, -y, -z)
    private double _vertexOneX;
    private double _vertexOneY;
    private double _vertexOneZ;
    // Second vertex location (-x, -y, z)
    private double _vertexTwoX;
    private double _vertexTwoY;
    private double _vertexTwoZ;
    // Third vertex location (-x, y, -z)
    private double _vertexThreeX;
    private double _vertexThreeY;
    private double _vertexThreeZ;
    // Fourth vertex location (-x, y, z)
    private double _vertexFourX;
    private double _vertexFourY;
    private double _vertexFourZ;
    // Fifth vertex location (x, -y. -z)
    private double _vertexFiveX;
    private double _vertexFiveY;
    private double _vertexFiveZ;
    // Sixth vertex location (x, -y, z)
    private double _vertexSixX;
    private double _vertexSixY;
    private double _vertexSixZ;
    // Seventh vertex location (x, y, -z)
    private double _vertexSevenX;
    private double _vertexSevenY;
    private double _vertexSevenZ;
    // Eighth vertex position (x, y, z)
    private double _vertexEightX;
    private double _vertexEightY;
    private double _vertexEightZ;

    // First octree child (-x, -y, -z)
    Octree _octreeOne;
    // First octree child (-x, -y, z)
    Octree _octreeTwo;
    // First octree child (-x, y, -z)
    Octree _octreeThree;
    // First octree child (-x, y, z)
    Octree _octreeFour;
    // First octree child (x, -y, -z)
    Octree _octreeFive;
    // First octree child (x, -y, z)
    Octree _octreeSix;
    // First octree child (x, y, -z)
    Octree _octreeSeven;
    // First octree child (x, y, z)
    Octree _octreeEight;

    
  }
}