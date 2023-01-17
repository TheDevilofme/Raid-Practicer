using Godot;
using System;

public class HealthBar : Control
{
    private TextureProgress _HealthBarUnder;
	private TextureProgress _HealthBarOver;
    private RichTextLabel _LabelAboveHealthbar;
	private Tween _UpdateTween;

    public void InitializeWithValues(string name, int MaxHealth) {
		_HealthBarUnder.MaxValue = MaxHealth;
		_HealthBarOver.MaxValue = MaxHealth;
		_HealthBarUnder.Value = MaxHealth;
		_HealthBarOver.Value = MaxHealth;
        _LabelAboveHealthbar.Text = name;
    }

    public override void _Ready()
	{
		_HealthBarUnder = GetNode<TextureProgress>("HealthBarUnder");
		_HealthBarOver = GetNode<TextureProgress>("HealthBarOver");
		_UpdateTween = GetNode<Tween>("UpdateTween");
		_LabelAboveHealthbar = GetNode<RichTextLabel>("LabelAboveHealthBar");
	}

    public void UpdateHealthBar(int finalValue) {
        _HealthBarOver.Value = finalValue;
		_UpdateTween.InterpolateProperty(_HealthBarUnder, "value", _HealthBarUnder.Value, finalValue, (float)0.4, Tween.TransitionType.Sine, Tween.EaseType.InOut, (float)0.4);
		_UpdateTween.Start();
    }
}
