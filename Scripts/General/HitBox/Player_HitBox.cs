using Godot;
using System;

public class Player_HitBox : HitBox
{
	[Export]
	public int Damage = 1;

	public Player_HitBox()
	{
		this.SetCollisionLayerBit(3, true);
		this.SetCollisionMask(0);
	}
}
