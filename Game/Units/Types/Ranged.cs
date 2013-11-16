using System;

using SevenEngine.StaticModels;

namespace Game.Units
{
  public abstract class Ranged : Unit
  {
    private readonly int _healthMin = 1;
    private readonly int _healthMax = 10;
    private readonly int _damageMin = 1;
    private readonly int _damageMax = 10;
    private readonly int _viewDistanceMin = 1;
    private readonly int _viewDistanceMax = 10000;
    private readonly int _moveSpeedMin = 1;
    private readonly int _moveSpeedMax = 100;

    public Ranged(string id, StaticModel staticModel) : base(id, staticModel)
    {
      Random random = new Random();
      _health = random.Next(_healthMin, _healthMax);
      _damage = random.Next(_damageMin, _damageMax);
      _viewDistance = random.Next(_viewDistanceMin, _viewDistanceMax);
      _moveSpeed = random.Next(_viewDistanceMin, _viewDistanceMax) / 10000f;
    }
  }
}
