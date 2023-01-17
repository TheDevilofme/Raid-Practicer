using Godot;
using System;

public class Enemy_HurtBox : HurtBox
{	
	public Enemy_HurtBox()
	{
		this.SetCollisionMaskBit(3, true);
		this.CollisionLayer = 0;
	}
}
