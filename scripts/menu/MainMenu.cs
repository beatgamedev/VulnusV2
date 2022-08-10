using Godot;
using System;

public class MainMenu : View
{
	public override void _Ready()
	{
		MenuHandler menu = GetParent<Control>().GetParent<MenuHandler>();
		var sidebar = GetNode<Control>("Sidebar");
		var buttons = sidebar.GetNode<Control>("Buttons");
		var singleBtn = buttons.GetNode<Button>("Singleplayer");
		singleBtn.Connect("pressed", menu, nameof(MenuHandler.GoTo), new Godot.Collections.Array(1));
		var optionsBtn = buttons.GetNode<Button>("Options");
		optionsBtn.Connect("pressed", Global.Instance.Overlays["Options"], nameof(Options.Open));
	}
}
