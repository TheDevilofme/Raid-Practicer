using Godot;
using System;

public class TitleScreen : Control
{
    [Signal]
    public delegate void PracticeBoss(string boss);

    public void OnBossPracticeButtonClicked(string boss) {
        EmitSignal(nameof(PracticeBoss), boss);
    }
}
