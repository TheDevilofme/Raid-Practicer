using Godot;
using System;

public abstract class AbstractDebuff : Node, IDebuff
{
    public int Damage {get; set;}
    public float Duration {get; set;}
    public string DebuffName {get; set;}
    public Texture Icon {get; set;}
    public void ProcessTime(float delta, IBuffable target)
    {
        this.Duration -= delta;
    }
    public bool IsExpired()
    {
        return Duration <= 0;
    }
    public virtual void Expire(IBuffable target)
    {
        if (IsExpired())
        {
            target.TakeDamage(Damage);
        }
    }
}
