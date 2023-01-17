using Godot;
using System;

public class XTMain : Node2D
{
	private bool _CheckForStandardPosition = false;
	private Timer _ExitedStandardPositionTimer;
	private Player _Player;

	[Signal]
	public delegate void BossDefeated(int score);

	[Signal]
	public delegate void GameOver(int score);

	[Export]
	public int TimeToGetBackIntoStandardPosition = 10;
	[Export]
	public int DamageForNotBeingInStandardPosition = 1;
	public override void _Ready()
	{
		GD.Randomize();
		_ExitedStandardPositionTimer = GetNode<Timer>("ExitedStandardPosition");
		_ExitedStandardPositionTimer.Start(TimeToGetBackIntoStandardPosition);
		_Player = GetNode<Player>("Player");
		HUD hud = GetNode<HUD>("HUD");
		hud.InitializeWithValues("Health Points", _Player.Hitpoints);
	}



	private void OnXTPhaseSwitch() {
		_CheckForStandardPosition = false;
	   PackedScene xtHeartScene = GD.Load<PackedScene>("res://Scenes/Ulduar/XT-Boss/XT-Heart.tscn");
	   Node xtHeartNode = xtHeartScene.Instance();
	   XTBoss xtBoss = GetNode<XTBoss>("XT");
	   xtBoss.CallDeferred("add_child", xtHeartNode);
	   xtHeartNode.Connect(nameof(XTHeart.HeartKilled), xtBoss, nameof(XTBoss.OnHeartDestroyed));
	   xtHeartNode.Connect(nameof(XTHeart.HeartTimerExpired), xtBoss, nameof(XTBoss.OnHeartTimerExpired));
	}

	private void OnCastDebuff(AbstractDebuff debuff) {
		_Player.AddDebuff(debuff);
	}

	private void OnExitedStandardPositionTimerExpiration() {
		_Player.TakeDamage(DamageForNotBeingInStandardPosition);
		_ExitedStandardPositionTimer.Start(TimeToGetBackIntoStandardPosition);
	}

	private void OnExitedStandardPosition(Player player) {
		if(player == null) return;
		_ExitedStandardPositionTimer.Start(TimeToGetBackIntoStandardPosition);
	}

	private void OnEnteredStandardPosition(Player player) {
		if(player == null) return;
		_ExitedStandardPositionTimer.Stop();
	}

	private void OnBossKilled(int maxHealthpoints) {
		XTBoss xtBoss = GetNode<XTBoss>("XT");
		int score = (_Player.Hitpoints + 1) * maxHealthpoints;
		EmitSignal(nameof(BossDefeated), score);
	}

	private void OnGameOver() {
		XTBoss xtBoss = GetNode<XTBoss>("XT");
		int score = xtBoss.MaxHealthpoints - xtBoss.Healthpoints;
		EmitSignal(nameof(GameOver), score);
	}
}
