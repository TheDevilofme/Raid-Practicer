using Godot;
using System;

public class DarkDebuffPosition : Area2D
{
	public void OnDarkDebuffPositionBodyEntered(Player player) {
		if(player == null) return;
		player.UnitStateMachine.DarkImmunity = true;
	}
	public void OnDarkDebuffPositionBodyExited(Player player) {
		if(player == null) return;
		player.UnitStateMachine.DarkImmunity = false;
	}
}
