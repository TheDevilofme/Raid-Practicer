using Godot;
using System;

public class LightDebuff : IDebuff
{
    public int Damage {get; set;}
    public float Duration {get; set;}
    public LightDebuff() {
        Damage = 1;
        Duration = 3;
    }

    public LightDebuff(int damage, int duration) {
        Damage = damage;
        Duration = duration;
    }
}
