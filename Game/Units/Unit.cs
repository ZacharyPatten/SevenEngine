using System;
using SevenEngine.DataStructures;
using SevenEngine.StaticModels;
using SevenEngine.Mathematics;

namespace Game.Units
{
  public abstract class Unit : IOctreeEntry
  {
    protected string _id;
    protected int _health;
    protected int _damage;
    protected float _viewDistance;
    protected float _attackRange;
    protected float _attackRangedSquared;
    protected float _moveSpeed;
    protected StaticModel _staticModel;
    protected bool _isDead;

    public string Id { get { return _id; } set { _id = value; } }
    public int Health { get { return _health; } set { _health = value; } }
    public int Damage { get { return _damage; } set { _damage = value; } }
    public float ViewDistance { get { return _viewDistance; } set { _viewDistance = value; } }
    public float MoveSpeed { get { return _moveSpeed; } set { _moveSpeed = value; } }
    public StaticModel StaticModel { get { return _staticModel; } set { _staticModel = value; } }
    public Vector Position { get { return _staticModel.Position; } set { _staticModel.Position = value; } }
    public virtual bool IsDead { get { return _isDead; } set { _isDead = value; } }
    
    public Unit(string id, StaticModel staticModel)
    {
      _id = id;
      _staticModel = staticModel;
      _isDead = false;
    }


    public void MoveTowards(Vector vector) 
    {
      //Vector v1 = new Vector(0, 0, -1);
      //Vector moveV = _staticModel.Position - vector;
      //Vector v2 = moveV.RotateBy(_staticModel.Orientation.W, 0, 1, 0);
      _staticModel.Position.X += (vector.X / vector.Length()) * _moveSpeed;
      _staticModel.Position.Y += (vector.Y / vector.Length()) * _moveSpeed;
      _staticModel.Position.Z += (vector.Z / vector.Length()) * _moveSpeed;

    }
    public virtual void AI(float elapsedTime, OctreeLinked<Unit> octree) { throw new NotImplementedException(); }

    public static int CompareTo(Unit left, Unit right) { return left.Id.CompareTo(right.Id); }
    public static int CompareTo(Unit left, string right) { return left.Id.CompareTo(right); }
  }
}
