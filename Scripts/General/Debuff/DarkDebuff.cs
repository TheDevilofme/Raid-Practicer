using Godot;
using System;

public class DarkDebuff : IDebuff
{
    public int Damage {get; set;}
    public float Duration {get; set;}
    public DarkDebuff() {
        Damage = 1;
        Duration = 3;
    }

    public DarkDebuff(int damage, int duration) {
        Damage = damage;
        Duration = duration;
    }
}
