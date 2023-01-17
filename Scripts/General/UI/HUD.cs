using Godot;
using System;

public class HUD : CanvasLayer
{
    private HealthBar _HealthBar;
    private DebuffList _DebuffList;
    public override void _Ready()
	{
        _HealthBar = GetNode<HealthBar>("HealthBar");
        _DebuffList = GetNode<DebuffList>("DebuffList");
        InitializeWithValues("test", 200);
	}

    public void InitializeWithValues(string name, int maxHealth) {
        _HealthBar.InitializeWithValues(name, maxHealth);
    }

    public void UpdateHealthBar(int healthValue) {
        _HealthBar.UpdateHealthBar(healthValue);
    }

    public void AddDebuff(AbstractDebuff debuff) {
        _DebuffList.AddDebuff(debuff);
    }

    public void RemoveDebuff(AbstractDebuff debuff) {
        _DebuffList.RemoveDebuff(debuff);
    }
}
