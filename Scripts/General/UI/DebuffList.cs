using Godot;
using System;

public class DebuffList : ItemList
{
	public override void _Ready()
	{
		if(this.GetItemCount() <= 0) {
			Hide();
		}
	}
	public void AddDebuff(IDebuff debuff) {
		this.AddItemAndShow(debuff.DebuffName, debuff.Icon, false);
	}
	public void RemoveDebuff(IDebuff debuff) {
		for (int i = 0; i < this.GetItemCount(); i++)
		{
			string debuffName = this.GetItemText(i);
			if(debuff.DebuffName.Equals(debuffName)) {
				this.RemoveItemAndHide(i);
			}
		}
	}
}
