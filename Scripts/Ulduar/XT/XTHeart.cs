using Godot;
using System;

public class XTHeart : KinematicBody2D, IDamagable
{
    private Timer _HeartTimer;
	private TextureProgress _HealthBarUnder;
	private TextureProgress _HealthBarOver;
	private Tween _UpdateTween;
	private bool _HardMode = false;
	private int Healthpoints;
	
	[Signal]
	public delegate void HeartKilled();

    [Signal]
	public delegate void HeartTimerExpired(int damage);
	
	[Export]
	public int MaxHealthpoints = 250;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_HealthBarUnder = GetNode<TextureProgress>("HealthBar/HealthBarUnder");
		_HealthBarOver = GetNode<TextureProgress>("HealthBar/HealthBarOver");
		_UpdateTween = GetNode<Tween>("HealthBar/UpdateTween");
		_HeartTimer = GetNode<Timer>("HeartTimer");
		Healthpoints = MaxHealthpoints;
		_HealthBarUnder.MaxValue = MaxHealthpoints;
		_HealthBarOver.MaxValue = MaxHealthpoints;
		_HealthBarUnder.Value = Healthpoints;
		_HealthBarOver.Value = Healthpoints;
		_HeartTimer.Start();
	}

	public void TakeDamage(int damage) {
        Healthpoints -= 2*damage;
		_HealthBarOver.Value = Healthpoints;
		_UpdateTween.InterpolateProperty(_HealthBarUnder, "value", _HealthBarUnder.Value, Healthpoints, (float)0.4, Tween.TransitionType.Sine, Tween.EaseType.InOut, (float)0.4);
		_UpdateTween.Start();
		if(Healthpoints <= 0) {
            EmitSignal(nameof(HeartKilled));
			QueueFree();
		}
	}
	
	private void OnTimerExpired() {
		QueueFree();
        int damage = -(Healthpoints - MaxHealthpoints);
		EmitSignal(nameof(HeartTimerExpired), damage);
	}
}
