using Godot;
using System;
using System.Collections.Generic;

public class Player : KinematicBody2D, IDamagable, IBuffable
{
	private AnimatedSprite _AnimatedSprite;
	private FacingDirection _FacingDirection = FacingDirection.Down;
	private bool _Moving = false;
	private AnimationPlayer _AnimationPlayer;
	private float _FacingAngle = 0;
	private float _AttackCooldown = 0;
	
	public List<IDebuff> DebuffList {get;}
	public UnitStateMachine UnitStateMachine {get; private set;}
	public Vector2 ScreenSize;
	
	[Export]
	public int Speed = 400;
	
	[Export]
	public int Hitpoints = 10;
	[Export]
	public float AttackSpeed = 1;
	
	[Signal]
	public delegate void GameOver();
	[Signal]
	public delegate void Attack();
	[Signal]
	public delegate void AddedDebuff(AbstractDebuff debuff);
	[Signal]
	public delegate void HealthChanged(int healthValue);
	[Signal]
	public delegate void RemovedDebuff(AbstractDebuff debuff);
	
	public void Start(Vector2 pos) {
		Position = pos;
		Show();
		GetNode<CollisionShape2D>("HitBox_CollisionShape").Disabled = false;
	}
	
	public Player() {
		DebuffList = new List<IDebuff>();
		
	}

	public override void _Ready()
	{
		UnitStateMachine = new UnitStateMachine();
		this.Connect(nameof(Attack), this, nameof(OnAttack));
		ScreenSize = GetViewportRect().Size;
		_AnimationPlayer = GetNode<AnimationPlayer>("AnimatedSprite/AnimationPlayer");
		_AnimatedSprite = GetNode<AnimatedSprite>("AnimatedSprite");
		//Hide();
	}
	
	public override void _Process(float delta)
  {
	var velocity = CalculateVelocity();
	CalculateStates(velocity);
	Animate();
	Move(velocity, delta);
	AttackCheck(delta);
	ProcessDebuffs(delta);
  }

private Vector2 CalculateVelocity() {
	var velocity = Vector2.Zero;
	
	if(Input.IsActionPressed("player_move_right")) {
		velocity.x += 1;
	}
	if(Input.IsActionPressed("player_move_left")) {
		velocity.x -= 1;
	}
	if(Input.IsActionPressed("player_move_down")) {
		velocity.y += 1;
	}
	if(Input.IsActionPressed("player_move_up")) {
		velocity.y -= 1;
	}
	
	return velocity;
}

private void Move(Vector2 velocity, float delta) {
	if(velocity.Length() > 0) {
		velocity = velocity.Normalized() * Speed;
	}
	MoveAndSlide(velocity);
}

private void CalculateStates(Vector2 velocity) {
	if(velocity.x != 0) {
		_Moving = true;
		bool facingLeft = velocity.x < 0;
		if(facingLeft) {
			_FacingDirection = FacingDirection.Left;
			_FacingAngle = Mathf.Deg2Rad(180);
		} else {
			_FacingDirection = FacingDirection.Right;
			_FacingAngle = Mathf.Deg2Rad(0);
		}
	}
	else if (velocity.y != 0) {
		_Moving = true;
		bool facingUpwards = velocity.y < 0;
		if(facingUpwards) {
			_FacingDirection = FacingDirection.Up;
			_FacingAngle = Mathf.Deg2Rad(90);
		} else {
			_FacingDirection = FacingDirection.Down;
			_FacingAngle = Mathf.Deg2Rad(270);
		}
	}
	else {
		_Moving = false;
	}
}

private void Animate() {
	switch(_FacingDirection) {
		case FacingDirection.Up:
			_AnimatedSprite.Scale = new Vector2(Math.Abs(_AnimatedSprite.Scale.x), _AnimatedSprite.Scale.y);
			if(_Moving) {
				_AnimationPlayer.PlayOrQueueAnimation("WalkUp", IsCurrentlyDoingAnAction(), true);
			} else {
				_AnimationPlayer.PlayOrQueueAnimation("IdleUp", IsCurrentlyDoingAnAction(), true);
			}
			break;
		case FacingDirection.Left:
		case FacingDirection.Right:
			if(_FacingDirection == FacingDirection.Left) {
				_AnimatedSprite.Scale = new Vector2(-Math.Abs(_AnimatedSprite.Scale.x), _AnimatedSprite.Scale.y);
			} else {
				_AnimatedSprite.Scale = new Vector2(Math.Abs(_AnimatedSprite.Scale.x), _AnimatedSprite.Scale.y);
			}
			if(_Moving) {
				_AnimationPlayer.PlayOrQueueAnimation("WalkSide", IsCurrentlyDoingAnAction(), true);
			} else {
				_AnimationPlayer.PlayOrQueueAnimation("IdleSide", IsCurrentlyDoingAnAction(), true);
			}
			break;
		default:
			Scale = new Vector2(Math.Abs(Scale.x), Scale.y);
			if(_Moving) {
				_AnimationPlayer.PlayOrQueueAnimation("WalkDown", IsCurrentlyDoingAnAction(), true);
			} else {
				_AnimationPlayer.PlayOrQueueAnimation("IdleDown", IsCurrentlyDoingAnAction(), true);
			}
			break;
	}
}

private bool IsCurrentlyDoingAnAction() {
	string[] actionAnimations = new string[] {"Death", "AttackUp", "AttackDown", "AttackSide"};
	if(_AnimationPlayer.IsPlaying() && Array.IndexOf(actionAnimations, _AnimationPlayer.CurrentAnimation) >= 0) {
		return true;
	}
	return false;
}



public void TakeDamage(int damage) {
	Hitpoints -= damage;
	EmitSignal(nameof(HealthChanged), Hitpoints);
	if(Hitpoints <= 0) {
		QueueFree();
		EmitSignal(nameof(GameOver));
	}
}

private void AttackCheck(float delta) {
	if(_AttackCooldown > 0) {
		_AttackCooldown -= delta;
	}
	if(Input.IsActionPressed("player_meele_attack") && _AttackCooldown <= 0 && !IsCurrentlyDoingAnAction()) {
				EmitSignal(nameof(Attack), AttackType.Meele, _FacingDirection, Position);
				_AttackCooldown = AttackSpeed;
	}
}

private void OnAttack(AttackType attackType, FacingDirection facingDirection, Vector2 position) {
	if(attackType == AttackType.Meele) {
		switch(facingDirection) {
		case FacingDirection.Up:
			_AnimationPlayer.Play("AttackUp");
			break;
		case FacingDirection.Left:
		case FacingDirection.Right:
			_AnimationPlayer.Play("AttackSide");
			break;
		default:
			_AnimationPlayer.Play("AttackDown");
			break;
	}
	}
}

private void ProcessDebuffs(float delta) {
	List<IDebuff> debuffsToRemove = new List<IDebuff>();
	foreach (var debuff in DebuffList)
	{
		debuff.ProcessTime(delta, this);
		if (debuff.IsExpired())
		{
			debuff.Expire(this);
			debuffsToRemove.Add(debuff);
			EmitSignal(nameof(RemovedDebuff), debuff);
		}
	}
	foreach (var debuff in debuffsToRemove)
	{
		DebuffList.Remove(debuff);
	}
}

public void AddDebuff(IDebuff debuff) {
	DebuffList.Add(debuff);
	EmitSignal(nameof(AddedDebuff), debuff);
}
}
