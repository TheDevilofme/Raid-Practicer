using Godot;
using System;

public class ScoreScreen : Control
{
	private Sprite _Background;
	private Label _Score;
	private Label _WinOrLoseText;
	[Signal]
	public delegate void BackToTitleScreen();
	public override void _Ready()
	{
		_Background = GetNode<Sprite>("CanvasLayer/Sprite");
		_WinOrLoseText = GetNode<Label>("WinOrLoseText");
		_Score = GetNode<Label>("Score");
		InitializeOnBossDefeated(0);
	}

	public void InitializeOnGameOver(int score) {
		_Background.SelfModulate = new Color("be3d24");
		_WinOrLoseText.Text = "Game Over";
		_Score.Text = score.ToString();
	}

	public void InitializeOnBossDefeated(int score) {
		_Background.SelfModulate = new Color("256c19");
		_WinOrLoseText.Text = "Boss defeated";
		_Score.Text = score.ToString();
	}

	private void OnBackToTitleScreenClick() {

	}
}
