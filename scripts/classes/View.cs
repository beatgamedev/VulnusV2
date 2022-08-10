using Godot;
using System;

public partial class View : Control
{
	public bool IsActive { get; protected set; } = false;
	public void SetActive(bool active)
	{
		if (active)
			this.OnShow();
		else
			this.OnHide();
	}
	public virtual void OnShow()
	{
		this.IsActive = true;
	}
	public virtual void OnHide()
	{
		this.IsActive = false;
	}
}
