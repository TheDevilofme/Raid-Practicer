using Godot;
using System;

public class MainScene : Node
{
	private const string ScoreScreenScenePath = "res://Scenes/Ulduar/ScoreScreen.tscn";
	private const string XTScenePath = "res://Scenes/Ulduar/XT.tscn";
	private PackedScene _TitleScreenScene;
	private PackedScene _ScoreScreenScene;
	private PackedScene _RecentBossScene;
	private Node RecentSceneNode;
	private Node NextSceneNode;
	public override void _Ready()
	{
		_ScoreScreenScene = GD.Load<PackedScene>(ScoreScreenScenePath);
		_TitleScreenScene = GD.Load<PackedScene>("res://Scenes/Ulduar/TitleScreen.tscn");
		NextSceneNode = _TitleScreenScene.Instance();
		this.AddChild(NextSceneNode);
		NextSceneNode.Connect(nameof(TitleScreen.PracticeBoss), this, nameof(OnBossChoosen));
		RecentSceneNode = NextSceneNode;
	}

	private void OnBossChoosen(string boss) {
		string scenePath = string.Empty;
		switch (boss)
		{
			case "XT":
				scenePath = XTScenePath;
				break;
		}
		if (string.IsNullOrWhiteSpace(scenePath)) return;
		_RecentBossScene = GD.Load<PackedScene>(scenePath);
		NextSceneNode = _RecentBossScene.Instance();
		RecentSceneNode.QueueFree();
		this.AddChild(NextSceneNode);
		NextSceneNode.Connect(nameof(XTMain.BossDefeated), this, nameof(OnBossKilled)); 
		NextSceneNode.Connect(nameof(XTMain.GameOver), this, nameof(OnGameOver)); 
		RecentSceneNode = NextSceneNode;
	}

	private void OnBossKilled(int score) {
		NextSceneNode = _ScoreScreenScene.Instance();
		RecentSceneNode.QueueFree();
		this.AddChild(NextSceneNode);
		if (NextSceneNode is ScoreScreen)
		{
			((ScoreScreen)NextSceneNode).InitializeOnBossDefeated(score);
		}
		RecentSceneNode = NextSceneNode;
	}

	private void OnGameOver(int score) {
		
		NextSceneNode = _ScoreScreenScene.Instance();
		RecentSceneNode.QueueFree();
		this.AddChild(NextSceneNode);
		if (NextSceneNode is ScoreScreen)
		{
			((ScoreScreen)NextSceneNode).InitializeOnGameOver(score);
		}
        NextSceneNode.Connect(nameof(ScoreScreen.BackToTitleScreen), this, nameof(BackToTitleScreen));
		RecentSceneNode = NextSceneNode;
	}

	private void BackToTitleScreen() {
		NextSceneNode = _TitleScreenScene.Instance();
		RecentSceneNode.QueueFree();
		this.AddChild(NextSceneNode);
		NextSceneNode.Connect(nameof(TitleScreen.PracticeBoss), this, nameof(OnBossChoosen));
		RecentSceneNode = NextSceneNode;
	}
}
