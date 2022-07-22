using Godot;
using System;

public class MainMenu : Control
{
	private Button[] Tabs;
	private Control[] Views;
	public override void _Ready()
	{
		var viewContainer = GetNode<Control>("ViewContainer");
		Control[] views = { viewContainer.GetNode<Control>("Play"), viewContainer.GetNode<Control>("Account"), viewContainer.GetNode<Control>("Settings") };
		Views = views;
		var tabBtns = GetNode<Control>("BottomBar/Buttons");
		Button[] tabs = { tabBtns.GetNode<Button>("Play"), tabBtns.GetNode<Button>("Account"), tabBtns.GetNode<Button>("Settings") };
		Tabs = tabs;
		for (var i = 0; i < Tabs.Length; i++)
		{
			Tabs[i].Connect("pressed", this, "TabPress", new Godot.Collections.Array() { i });
		}
		TabPress(0);
	}
	public void TabPress(int btn)
	{
		for (int i = 0; i < Tabs.Length; i++)
		{
			Tabs[i].Pressed = i == btn;
			Views[i].Visible = i == btn;
		}
	}
}
