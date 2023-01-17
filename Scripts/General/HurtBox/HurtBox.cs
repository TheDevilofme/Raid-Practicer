using Godot;
using System;

public class HurtBox : Area2D
{
	public override void _Ready() {
		this.Connect("area_entered", this, nameof(OnAreaEntered));
	}
	
	public void OnAreaEntered(HitBox hitBox) {
		if(hitBox == null) {
			return;
		}
		if(Owner is IDamagable) {
		IDamagable damagable = (IDamagable)Owner;
			damagable.TakeDamage(hitBox.Damage);
		}
		
	}
}
