using Godot;
using System;

public class XTMain : Node2D
{
	private PackedScene _XTHeartScene = (PackedScene)(ResourceLoader.Load("res://Scenes/Ulduar/XT-Boss/XT-Heart.tscn"));
	private bool _CheckForStandardPosition = false;
	private Timer _LeftStandardPositionTimer;

	[Export]
	public int TimeToGetBackIntoStandardPosition = 10;
	public override void _Ready()
	{
		GD.Randomize();
		_LeftStandardPositionTimer = GetNode<Timer>("LeftStandardPosition");
		_LeftStandardPositionTimer.Start(TimeToGetBackIntoStandardPosition);
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
}
