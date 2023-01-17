using Godot;
using System;
using System.Collections.Generic;

public interface IBuffable : IDamagable
{
	UnitStateMachine UnitStateMachine {get;}
    List<IDebuff> DebuffList {get;}
}
