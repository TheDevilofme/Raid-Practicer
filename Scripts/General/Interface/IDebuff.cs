using Godot;
using System;

public interface IDebuff
{
	float Duration {get; set;}
    int Damage {get; set;}
    string DebuffName {get; set;}

    Texture Icon {get; set;}
    void ProcessTime(float delta, IBuffable target);
    bool IsExpired();
    void Expire(IBuffable target);
}
