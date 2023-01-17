using Godot;
using System;

public class XTBoss : KinematicBody2D, IDamagable
{
	private Timer _DebuffCooldownTimer;
	private TextureProgress _HealthBarUnder;
	private TextureProgress _HealthBarOver;
	private Label _HealthText;
	private Label _HealthPercentage;
	private Tween _UpdateTween;
	private bool _Immune = false;
	private bool _HardMode = false;
	private int _HeartPhases = 0;
	private int _Healthpoints;
	private int _CurrentDebuffCooldown;

	public int Healthpoints {get {return _Healthpoints;}}
	[Signal]
	public delegate void BossKilled(int maxHealthpoints);

	[Signal]
	public delegate void PhaseSwitch();

	[Signal]
	public delegate void CastedDebuff(AbstractDebuff debuff);
	
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
		_HealthText = GetNode<Label>("HealthBar/HealthText");
		_HealthPercentage = GetNode<Label>("HealthBar/HealthPercentage");
		_UpdateTween = GetNode<Tween>("HealthBar/UpdateTween");
		_Healthpoints = MaxHealthpoints;
		_HealthBarUnder.MaxValue = MaxHealthpoints;
		_HealthBarOver.MaxValue = MaxHealthpoints;
		_HealthBarUnder.Value = _Healthpoints;
		_HealthBarOver.Value = _Healthpoints;
		_DebuffCooldownTimer.Start(InitialDebuffCooldown);
		_CurrentDebuffCooldown = InitialDebuffCooldown;
	}

	public void TakeDamage(int damage) {
		if(!_Immune) {
			_Healthpoints -= damage;
		
		if(_Healthpoints <= 0) {
			EmitSignal(nameof(BossKilled), MaxHealthpoints);
			QueueFree();
			return;
		}
		float healthpointPercentage = ((float)_Healthpoints / (float)MaxHealthpoints);
		if(!_HardMode && ((healthpointPercentage <= 0.75 && _HeartPhases <= 0) || (healthpointPercentage <= 0.50 && _HeartPhases <= 1) || (healthpointPercentage <= 0.25 && _HeartPhases <= 2))) {
			_HeartPhases++;
			EmitSignal(nameof(PhaseSwitch));
		}
		_HealthBarOver.Value = _Healthpoints;
		_UpdateTween.InterpolateProperty(_HealthBarUnder, "value", _HealthBarUnder.Value, _Healthpoints, (float)0.4, Tween.TransitionType.Sine, Tween.EaseType.InOut, (float)0.4);
		_UpdateTween.Start();
		_HealthPercentage.Text = healthpointPercentage * 100 + "%";
		_HealthText.Text = _Healthpoints.ToString();
		}
	}
	
	public void OnHeartDestroyed() {
		_HardMode = true;
		_Healthpoints = MaxHealthpoints;
		SwitchBackToPhaseOne(0);
	}
	
	public void OnHeartTimerExpired(int damage) {
		SwitchBackToPhaseOne(damage);
	}

	private void SwitchBackToPhaseOne(int damage) {
		_Immune = false;
		TakeDamage(damage);
		_DebuffCooldownTimer.Paused = false;
		CallDeferred("Activate");
	}

	private void Activate() {
		GetNode<CollisionShape2D>("Enemy_HurtBox/HurtBox").Disabled = false;
		GetNode<CollisionShape2D>("CollisionShape").Disabled = false;
		GetNode<Sprite>("Sprite").Visible = true;
	}

	private void Deactivate() {
		GetNode<CollisionShape2D>("Enemy_HurtBox/HurtBox").Disabled = true;
		GetNode<CollisionShape2D>("CollisionShape").Disabled = true;
		GetNode<Sprite>("Sprite").Visible = false;
	}
	
	public void OnPhaseSwitch() {
		_DebuffCooldownTimer.Paused = true;
		CallDeferred("Deactivate");
		_Immune = true;
	}

	public void OnDebuffCooldownExpired() {
		IDebuff debuff;
		int randomNumber = (int)(GD.Randi() % 2);
		switch(randomNumber) {
			case 0:
				debuff = new DarkDebuff();
				break;
			default:
				debuff = new LightDebuff();
				break;
		}
		if (debuff.Duration < _CurrentDebuffCooldown)
		{
			_CurrentDebuffCooldown--;
		}
		_DebuffCooldownTimer.Start(_CurrentDebuffCooldown);
		EmitSignal(nameof(CastedDebuff), debuff);
	}
}
