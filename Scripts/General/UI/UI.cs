using Godot;
using System;

public class UI : Control
{
    private HealthBar _HealthBar;
    public override void _Ready()
	{
        _HealthBar = GetNode<HealthBar>("HealthBar");
        InitializeWithValues("test", 200);
	}

    public void InitializeWithValues(string name, int maxHealth) {
        _HealthBar.InitializeWithValues(name, maxHealth);
    }
}
