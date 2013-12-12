using System;
using SevenEngine.DataStructures;
using SevenEngine.StaticModels;
using SevenEngine.Mathematics;
using SevenEngine.DataStructures.Interfaces;

namespace Game.Units
{
  public abstract class Unit : InterfacePositionVector, InterfaceStringId
  {
    protected string _id;
    protected int _health;
    protected int _damage;
    protected float _viewDistance;
    protected float _attackRange;
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
      vector = vector - _staticModel.Position;
      _staticModel.Position.X += (vector.X / vector.Length) * _moveSpeed;
      _staticModel.Position.Y += (vector.Y / vector.Length) * _moveSpeed;
      _staticModel.Position.Z += (vector.Z / vector.Length) * _moveSpeed;

    }
    public virtual void AI(float elapsedTime, OctreeLinked<Unit, string> octree) { throw new NotImplementedException(); }

    public static int CompareTo(Unit left, Unit right) { return left.Id.CompareTo(right.Id); }
    public static int CompareTo(Unit left, string right) { return left.Id.CompareTo(right); }
  }
}
