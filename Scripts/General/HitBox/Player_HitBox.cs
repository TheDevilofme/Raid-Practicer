using Godot;
using System;

public class Player_HitBox : HitBox
{
	public Player_HitBox()
	{
		this.SetCollisionLayerBit(3, true);
		this.CollisionMask = 0;
	}
}
