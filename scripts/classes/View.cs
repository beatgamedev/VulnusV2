using Godot;
using System;

public partial class View : Control
{
	public void SetActive(bool active)
	{
		if (active)
			this.OnShow();
		else
			this.OnHide();
	}
	public virtual void OnShow() { }
	public virtual void OnHide() { }
}
