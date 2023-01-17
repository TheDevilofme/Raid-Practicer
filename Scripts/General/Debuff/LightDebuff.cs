using Godot;
using System;

public class LightDebuff : AbstractDebuff
{
    public LightDebuff() {
        DebuffName = "LightDebuff";
        Damage = 1;
        Duration = 3;
    }
    public LightDebuff(int damage, int duration) {
        Damage = damage;
        Duration = duration;
    }
    public override void Expire(IBuffable target)
    {
        if (IsExpired() && !target.UnitStateMachine.LightImmunity)
        {
            target.TakeDamage(Damage);
        }
    }
}
