using Godot;
using System;

public class XTBoss : KinematicBody2D, IDamagable
{
	private TextureProgress _HealthBarUnder;
	private TextureProgress _HealthBarOver;
	private Tween _UpdateTween;
	private bool _Immune = false;
	private bool _HardMode = false;
	private int _HeartPhases = 0;
	private int Healthpoints;
	
	[Signal]
	public delegate void PhaseSwitch();
	
	[Export]
	public int MaxHealthpoints = 1000;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_HealthBarUnder = GetNode<TextureProgress>("HealthBar/HealthBarUnder");
		_HealthBarOver = GetNode<TextureProgress>("HealthBar/HealthBarOver");
		_UpdateTween = GetNode<Tween>("HealthBar/UpdateTween");
		Healthpoints = MaxHealthpoints;
		_HealthBarUnder.MaxValue = MaxHealthpoints;
		_HealthBarOver.MaxValue = MaxHealthpoints;
		_HealthBarUnder.Value = Healthpoints;
		_HealthBarOver.Value = Healthpoints;
	}

	public void TakeDamage(int damage) {
		if(!_Immune) {
			Healthpoints -= damage;
		}
		if(!_HardMode && ((Healthpoints <= 75 && _HeartPhases <= 0) || (Healthpoints <= 50 && _HeartPhases <= 1) || (Healthpoints <= 25 && _HeartPhases <= 2))) {
			EmitSignal(nameof(PhaseSwitch));
		}
		_HealthBarOver.Value = Healthpoints;
		_UpdateTween.InterpolateProperty(_HealthBarUnder, "value", _HealthBarUnder.Value, Healthpoints, (float)0.4, Tween.TransitionType.Sine, Tween.EaseType.InOut, (float)0.4);
		_UpdateTween.Start();
		if(Healthpoints <= 0) {
			QueueFree();
		}
	}
	
	private void OnHeartDestroyed() {
		Healthpoints = MaxHealthpoints;
		TakeDamage(0);
		_HardMode = true;
	}
	
	private void OnHeartTimerExpired(int damage) {
		_Immune = false;
		TakeDamage(damage);
		GetNode<CollisionShape2D>("HurtBox/HurtBox").Disabled = false;
	}
	
	private void OnPhaseSwitch() {
		GetNode<CollisionShape2D>("HurtBox/HurtBox").Disabled = true;
		_Immune = true;
	}
}
