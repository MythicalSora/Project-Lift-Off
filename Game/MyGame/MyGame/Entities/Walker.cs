using GXPEngine;
using GXPEngine.Core;
using TiledMapParser;

namespace MyGame.MyGame.Entities;

public class Walker : Enemy
{
	private bool _goingRight;

	public Walker(TiledObject obj) : base("enemyRobot.png", 8, 3, 8, 2, obj)
	{
		_goingRight = Utils.Random(0, 100) < 50;
		SetCycle(0, 8, 100);
		AnimateFixed();
	}


	protected new void Update()
	{
		if(!base.Update()) return;
		ApplyForce(new Vector2(_goingRight ? 1 : -1, 0));
	}

	protected override void CollidedWithSide(Vector2 normal)
	{
		_goingRight = !_goingRight;
	}

	protected override void Die()
	{
		game.AddChild(new WalkerExplosion(GetPos(), width, height));
		SoundManager.explosion.Play();
		base.Die();
	}
}
