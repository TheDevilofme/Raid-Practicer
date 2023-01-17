using Godot;
using System;

public interface IBossScene
{
	void BossDefeated(int score);
	void GameOver(int score);
}
