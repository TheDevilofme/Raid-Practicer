using Godot;
using System;

public class LightDebuffPosition : Area2D
{
    public void OnLightDebuffPositionBodyEntered(Player player) {
        if(player == null) return;
        player.UnitStateMachine.LightImmunity = true;
    }
    public void OnLightDebuffPositionBodyExited(Player player) {
        if(player == null) return;
        player.UnitStateMachine.LightImmunity = false;
    }
}
