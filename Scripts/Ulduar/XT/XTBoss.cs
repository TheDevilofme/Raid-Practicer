using Godot;
using System;

public class XTBoss : KinematicBody2D, IDamagable
{
	private Timer _DebuffCooldownTimer;
	private TextureProgress _HealthBarUnder;
	private TextureProgress _HealthBarOver;
	private Tween _UpdateTween;
	private bool _Immune = false;
	private bool _HardMode = false;
	private int _HeartPhases = 0;
	private int Healthpoints;
	private int _CurrentDebuffCooldown;

	
	[Signal]
	public delegate void PhaseSwitch();

	[Signal]
	public delegate void CastedDebuff(IDebuff debuff);
	
	[Export]
	public int MaxHealthpoints = 1000;

	[Export]
	public int InitialDebuffCooldown = 15;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		this.Connect(nameof(PhaseSwitch), this, nameof(OnPhaseSwitch));
		_DebuffCooldownTimer = GetNode<Timer>("DebuffCooldown");
		_HealthBarUnder = GetNode<TextureProgress>("HealthBar/HealthBarUnder");
		_HealthBarOver = GetNode<TextureProgress>("HealthBar/HealthBarOver");
		_UpdateTween = GetNode<Tween>("HealthBar/UpdateTween");
		Healthpoints = MaxHealthpoints;
		_HealthBarUnder.MaxValue = MaxHealthpoints;
		_HealthBarOver.MaxValue = MaxHealthpoints;
		_HealthBarUnder.Value = Healthpoints;
		_HealthBarOver.Value = Healthpoints;
		_DebuffCooldownTimer.Start(InitialDebuffCooldown);
		_CurrentDebuffCooldown = InitialDebuffCooldown;
	}

	public void TakeDamage(int damage) {
		if(!_Immune) {
			Healthpoints -= damage;
		}
		if(Healthpoints <= 0) {
			QueueFree();
			return;
		}
		float healthpointPercentage = ((float)Healthpoints / (float)MaxHealthpoints);
		if(!_HardMode && ((healthpointPercentage <= 0.75 && _HeartPhases <= 0) || (healthpointPercentage <= 0.50 && _HeartPhases <= 1) || (healthpointPercentage <= 0.25 && _HeartPhases <= 2))) {
			_HeartPhases++;
			EmitSignal(nameof(PhaseSwitch));
		}
		_HealthBarOver.Value = Healthpoints;
		_UpdateTween.InterpolateProperty(_HealthBarUnder, "value", _HealthBarUnder.Value, Healthpoints, (float)0.4, Tween.TransitionType.Sine, Tween.EaseType.InOut, (float)0.4);
		_UpdateTween.Start();
		
	}
	
	public void OnHeartDestroyed() {
		_HardMode = true;
		Healthpoints = MaxHealthpoints;
		SwitchBackToPhaseOne(0);
	}
	
	public void OnHeartTimerExpired(int damage) {
		SwitchBackToPhaseOne(damage);
	}

	private void SwitchBackToPhaseOne(int damage) {
		TakeDamage(damage);
		_Immune = false;
		_DebuffCooldownTimer.Paused = false;
		CallDeferred("Activate");
	}

	private void Activate() {
		GetNode<CollisionShape2D>("HurtBox/HurtBox").Disabled = false;
		GetNode<CollisionShape2D>("CollisionShape").Disabled = false;
		GetNode<Sprite>("Sprite").Visible = true;
	}

	private void Deactivate() {
		GetNode<CollisionShape2D>("HurtBox/HurtBox").Disabled = true;
		GetNode<CollisionShape2D>("CollisionShape").Disabled = true;
		GetNode<Sprite>("Sprite").Visible = false;
	}
	
	public void OnPhaseSwitch() {
		_DebuffCooldownTimer.Paused = true;
		CallDeferred("Deactivate");
		_Immune = true;
	}

	public void OnDebuffCooldownExpired() {
		_CurrentDebuffCooldown--;
		_DebuffCooldownTimer.Start(_CurrentDebuffCooldown);
		IDebuff debuff;
		int randomNumber = (int)(GD.Randi() % 1);
		switch(randomNumber) {
			case 0:
				debuff = new DarkDebuff();
				break;
			default:
				debuff = new LightDebuff();
				break;
		}
		EmitSignal(nameof(CastedDebuff), debuff);
	}
}
