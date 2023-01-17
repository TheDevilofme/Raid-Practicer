using Godot;
using System;

public class DarkDebuff : AbstractDebuff
{

    public DarkDebuff() {
        DebuffName = "DarkDebuff";
        Damage = 1;
        Duration = 3;
    }
    public DarkDebuff(int damage, int duration) {
        Damage = damage;
        Duration = duration;
    }
    public override void Expire(IBuffable target)
    {
        if (IsExpired() && !target.UnitStateMachine.DarkImmunity)
        {
            target.TakeDamage(Damage);
        }
    }
}
