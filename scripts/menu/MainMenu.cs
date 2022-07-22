using Godot;
using System;

public class MainMenuController : Control
{
	private Button[] Tabs;
	private Control[] Views;
	public override void _Ready()
	{
		var viewContainer = GetNode<Control>("ViewContainer");
		Control[] views = { viewContainer.GetNode<Control>("Play"), viewContainer.GetNode<Control>("Account"), viewContainer.GetNode<Control>("Settings") };
		Views = views;
		var tabBtns = GetNode<Control>("BottomBar/HBoxContainer");
		Button[] tabs = { tabBtns.GetNode<Button>("Play"), tabBtns.GetNode<Button>("Account"), tabBtns.GetNode<Button>("Settings") };
		Tabs = tabs;
		for (var i = 0; i < tabs.Length; i++)
		{
			GD.Print(i);
			tabs[i].Connect("pressed", this, nameof(TabPress), new Godot.Collections.Array() { i });
		}
		TabPress(0);
	}
	public void TabPress(int btn)
	{
		GD.Print("TabPress: " + btn);
		for (int i = 0; i < Tabs.Length; i++)
		{
			Tabs[i].Pressed = i == btn;
			Views[i].Visible = i == btn;
		}
	}
}
