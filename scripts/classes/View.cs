using Godot;
using System;

public partial class View : Control
{
	public virtual void Display(Control container)
	{
		if (this.GetParent<Control>() != container)
			container.AddChild(this);
		this.Show();
	}
	public virtual new void Hide()
	{
		base.Hide();
	}
}
