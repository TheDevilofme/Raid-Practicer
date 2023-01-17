using Godot;
using System;

public class Player_HurtBox : HurtBox
{
	public Player_HurtBox()
	{
		this.SetCollisionMaskBit(2, true);
		this.CollisionLayer = 0;
	}
}
